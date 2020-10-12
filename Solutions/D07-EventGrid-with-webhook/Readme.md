# Summary
To provide external systems with the ability to react on some events by calling a WebHook function, an EventGrid is deployed and configured. There is some confusion regaring the different Azure Resources for EventGrids, which may be clarified with these statements:
* EventGrid mainly is an "internal" services which is integrated to many Azure Resources (ResourceGroup, StorageAccount, AppService, ...) which allows other Resources to react on specific event. A typical use case: If you configure a trigger in Azure Data Factory to start a pipeline when a blob is created or updated in a Storage Account, it basically sets up this "internal" EventGrid on the Storage Account and registers a subcription for this event.
* You find a section "Automation" with the menu item "Events" on many Azure Resources
* These events are called "System" events along with all related resources and functionality
* As the service-integration with EventGrid is very powerfull it is interessting to adapt it also for external systems. The idea is that any system may dispatch events in a standardized matter so that Azure Resources may connect to it in a uniform way. This topic is called "Partner" events and there also exists some resources (e.g. EventGrid Partner Namespace/Topic). More on this: https://docs.microsoft.com/en-us/azure/event-grid/partner-topics-overview
* Custom solution (such as the current challenge) are build with "Custom" events. This requires an Azure Resource of type "EventGrid Topic" which directly accepts events from publishers. Per consumers a subcription is deployed which may include a variety of filters and some advanced configurations such as retry, dead-lettering or event expiration (with automatic deletion)

More on this: https://docs.microsoft.com/en-us/azure/event-grid/concepts

# Azure Event Grid Viewer
Microsoft provided a simple test application which provides a WebHook URL. You get it from: https://docs.microsoft.com/en-us/samples/azure-samples/azure-event-grid-viewer/azure-event-grid-viewer

# Implementation
## Deployment
Note: It includes a filter on the subscription for example purpose. You may remove or adjust the `advancedFilters` object.
```json
{
    "type": "Microsoft.EventGrid/topics",
    "apiVersion": "2020-04-01-preview",
    "name": "[variables('eventGridTopicName')]",
    "location": "[resourceGroup().location]",
    "properties": {
        "inputSchema": "EventGridSchema",
        "publicNetworkAccess": "Enabled"
    },
    "sku": {
        "name": "Basic"
    },
    "identity": {
        "type": "SystemAssigned"
    }
},
{
    "type": "Microsoft.EventGrid/topics/providers/diagnosticSettings",
    "name": "[concat(variables('eventGridTopicName'), '/Microsoft.Insights/', 'LogAnalytics')]",
    "apiVersion": "2017-05-01-preview",
    "dependsOn": [
        "[resourceId('Microsoft.EventGrid/topics', variables('eventGridTopicName'))]",
        "[resourceId('Microsoft.OperationalInsights/workspaces', variables('logAnalyticsWsName'))]"
    ],
    "properties": {
        "name": "LogAnalytics",
        "workspaceId": "[resourceId('Microsoft.OperationalInsights/workspaces', variables('logAnalyticsWsName'))]",
        "logs": [
            {
                "category": "DeliveryFailures",
                "enabled": true
            },
            {
                "category": "PublishFailures",
                "enabled": true
            }
        ],
        "metrics": [
            {
                "category": "AllMetrics",
                "enabled": true
            }
        ]
    }
},
{
    "type": "Microsoft.EventGrid/topics/providers/eventSubscriptions",
    "name": "[concat(variables('eventGridTopicName'), '/Microsoft.EventGrid/', variables('eventGridWebhookSubscriptionName'))]",
    "location": "[resourceGroup().location]",
    "apiVersion": "2020-06-01",
    "properties": {
        "destination": {
            "endpointType": "WebHook",
            "properties": {
                "endpointUrl": "[parameters('eventGridWebhookSubscriptionUrl')]"
            }
        },
        "filter": {
            "includedEventTypes": null,
            "advancedFilters": [
                {
                    "key": "EventType",
                    "operatorType": "StringBeginsWith",
                    "values": [
                        "Garaio.DevCampServerless."
                    ]
                }
            ]
        }
    },
    "dependsOn": [
        "[resourceId('Microsoft.EventGrid/topics', variables('eventGridTopicName'))]"
    ]
}
```

And its references in app-settings of function:
```json
{
	...
    "EventGridTopicEndpoint": "[reference(resourceId('Microsoft.EventGrid/topics', variables('eventGridTopicName'))).endpoint]",
    "EventGridTopicKey": "[listKeys(resourceId('Microsoft.EventGrid/topics', variables('eventGridTopicName')), '2020-04-01-preview').key1]"
}
```

# Nuget Package
```xml
<PackageReference Include="Microsoft.Azure.EventGrid" Version="3.2.0" />
```

# Client integration
As functions should share client instances as much as possible, this has been implemented in the static `FunctionHelper` class:
```csharp
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
```

Functions can then dispatch events in a very simple way:
```csharp
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
```