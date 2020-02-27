using System;
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

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class DeployClientApp
    {
        private const string IndexDocument = "index.html";

        [FunctionName(nameof(DeployClientApp))]
        public static async Task Run([TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]TimerInfo timer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"Deployment of Client-App (Demo UI) triggered");

            string storageConnectionString = Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);
            CloudBlobClient blobClient = CloudStorageAccount.Parse(storageConnectionString).CreateCloudBlobClient();

            var currentProperties = await blobClient.GetServicePropertiesAsync();
            if (currentProperties?.StaticWebsite?.Enabled == true)
            {
                log.LogInformation($"StorageAccount is already configured for static website hosting");
                return;
            }

            // Update storage account for static website hosting
            ServiceProperties blobServiceProperties = new ServiceProperties();
            blobServiceProperties.StaticWebsite = new StaticWebsiteProperties
            {
                Enabled = true,
                IndexDocument = IndexDocument
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
                        if (entry.FullName == IndexDocument)
                        {
                            var indexHtml = await new StreamReader(stream).ReadToEndAsync();
                            var pattern = "<base href=\"/\"><script>api = {{ baseUrl:'{0}', authCode:'{1}' }};</script>";
                            var api = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl);
                            var code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKeyClient);
                            var replacement = string.Format(pattern, api, code);

                            indexHtml = indexHtml.Replace("<base href=\"/\">", replacement);

                            await blob.UploadTextAsync(indexHtml);
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
