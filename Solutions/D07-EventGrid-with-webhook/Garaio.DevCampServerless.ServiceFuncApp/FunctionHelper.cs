using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class FunctionHelper
    {
        public static readonly Lazy<EventGridClient> EventGridClient = new Lazy<EventGridClient>(() => new EventGridClient(new TopicCredentials(Configurations.EventGridTopicKey)));
        public static readonly Lazy<string> EventGridTopicHostname = new Lazy<string>(() => new Uri(Configurations.EventGridTopicEndpoint).Host);
        
        public static async Task PublishEvent(EventGridEvent @event, ILogger log)
        {
            var events = new[] { @event };

            try
            {
                await EventGridClient.Value.PublishEventsAsync(EventGridTopicHostname.Value, events);
            }
            catch (Exception e)
            {
                log.LogError(e, "Publish events to EventGrid failed");
            }
        }

        public static readonly Lazy<JsonSerializerSettings> SerializerSettings = new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore });

        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings.Value);
        }
    }
}
