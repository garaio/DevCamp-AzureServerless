using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using MimeMapping;
using Newtonsoft.Json.Linq;

namespace Garaio.DevCampServerless.EmulatorFuncApp.Initialization
{
    public static class DeployClientApp
    {
        private const string IndexDocument = "index.html";
        private const string ConfigDocument = "assets/config.json";

        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(DeployClientApp))]
        public static async Task Run([TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]TimerInfo timer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"Deployment of Client-App (Demo UI) triggered");

            CloudBlobClient blobClient = CloudStorageAccount.Parse(Configurations.StorageConnectionString).CreateCloudBlobClient();

            var currentProperties = await blobClient.GetServicePropertiesAsync();
            if (currentProperties?.StaticWebsite?.Enabled == true)
            {
                log.LogInformation($"StorageAccount is already configured for static website hosting");
                return;
            }

            // Update storage account for static website hosting
            ServiceProperties blobServiceProperties = new ServiceProperties
            {
                StaticWebsite = new StaticWebsiteProperties
                {
                    Enabled = true,
                    IndexDocument = IndexDocument
                }
            };
            await blobClient.SetServicePropertiesAsync(blobServiceProperties);

            CloudBlobContainer container = blobClient.GetContainerReference("$web");
            await container.CreateIfNotExistsAsync();

            // Download deployment package from repository
            // Example: https://github.com/garaio/DevCamp-AzureServerless/raw/feature/demo-ui/Foundation/Garaio.DevCampServerless.Deployment/blobs/%24web.zip
            var baseUrl = Environment.GetEnvironmentVariable(Constants.Configurations.RepoUrl).Replace(".git", "");
            var url = baseUrl + string.Format(Environment.GetEnvironmentVariable(Constants.Configurations.RepoClientAppPackagePattern), Environment.GetEnvironmentVariable(Constants.Configurations.RepoBranch));

            var zipStream = await url.GetStreamAsync();

            // Unpack zip to container and manipulate index.html file
            using (ZipArchive archive = new ZipArchive(zipStream))
            {
                var entries = archive.Entries;
                foreach (var entry in entries)
                {
                    CloudBlockBlob blob = container.GetBlockBlobReference(entry.FullName);

                    using (var stream = entry.Open())
                    {
                        if (entry.FullName == ConfigDocument)
                        {
                            var configText = await new StreamReader(stream).ReadToEndAsync();
                            JObject configJson = JObject.Parse(configText);
                            JObject apiJson = (JObject)configJson["api"];

                            apiJson["baseUrl"] = Configurations.ServiceFuncUrl;
                            apiJson["authCode"] = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKeyClient);
                            
                            await blob.UploadTextAsync(configJson.ToString());
                        }
                        else
                        {
                            await blob.UploadFromStreamAsync(stream);
                        }
                    }

                    blob.Properties.ContentType = MimeUtility.GetMimeMapping(entry.Name);
                    await blob.SetPropertiesAsync();
                }
            }

            log.LogInformation($"Deployment of Client-App (Demo UI) successfully executed");
        }
    }
}
