using Garaio.DevCampServerless.Common.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Garaio.DevCampServerless.ServiceFuncApp.Functions
{
    public static class SearchResults
    {
        [FunctionName(nameof(Get) + nameof(SearchResults))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = Constants.Routes.Search)] HttpRequest req,
            ILogger log)
        {
            var query = (string)req.Query[Constants.QueryParams.SearchQuery] ?? await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(query))
            {
                return new BadRequestResult();
            }

            // Simulate consuming operation (for OPS challenges)
            // Note: If you are working on DEV challenges you may remove this code-block with no worries!
            if (int.TryParse(query, out var n))
            {
                int count = 0;
                long a = 2;
                while (count < n)
                {
                    long b = 2;
                    int prime = 1; // to check if found a prime
                    while (b * b <= a)
                    {
                        if (a % b == 0)
                        {
                            prime = 0;
                            break;
                        }
                        b++;
                    }
                    if (prime > 0)
                    {
                        count++;
                    }
                    a++;
                }

                log.LogDebug($"Just for fun: nth prime number for {n} is {--a}");
            }

            var results = new List<SearchResult>();

            log.LogInformation($"Found {results.Count} results for query '{query}'");

            var @event = new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "Garaio.DevCampServerless.Search",
                Data = results,
                EventTime = DateTime.UtcNow,
                Subject = $"Query: {query}",
                DataVersion = "1.0"
            };

            await FunctionHelper.PublishEvent(@event, log);

            return new OkObjectResult(FunctionHelper.ToJson(results));
        }
    }
}
