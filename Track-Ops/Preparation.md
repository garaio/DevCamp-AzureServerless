# Recommended infrastructure for challenges
## Development Environment
For the creation of analytics and monitoring dashboards you may use PowerBI. Therefore it is highly recommended to install [PowerBI Desktop](https://powerbi.microsoft.com/de-de/downloads/).

The pure Azure related challenges can be realized directly in the [Portal](https://portal.azure.com/) without further tools.

# Some relevant online resources
- [Kusto Query Language](https://docs.microsoft.com/en-us/azure/kusto/query/)
- [Kusto Query Demo Space](https://portal.loganalytics.io/demo#/discover/query/main)
- [Power Query M function reference](https://docs.microsoft.com/en-us/powerquery-m/power-query-m-function-reference)

# Deploy lab to your Azure environment
1. Clone the repository to your local system with [Azure PowerShell](https://github.com/Azure/azure-powershell#installation) installed
1. Run this PowerShell script which provides a guided installation into your subscription: [`Foundation\Garaio.DevCampServerless.Deployment\Deploy-WithLogin.ps1`](..\Foundation\Garaio.DevCampServerless.Deployment\Deploy-WithLogin.ps1). You have to enter a suffix to be appended to the name of all Azure services and its URL's which have to be unique. It is recommended to use an abbreviation of your name (e.g. John Wayne -> jwa).
1. Wait some minutes after the deployment completed (the functions are automatically built and provisionioned)
1. The components including the function's API is ready to use and the demo-application is available composing this URL: **`https://gadcssa{YOUR-SUFFIX}.z6.web.core.windows.net`**

_Note: The URL looks different when you deploy to another Azure Region. In case of any troubles navigate to your Storage Account in the Portal and use the URL displayed in the section "Static website"_

# Notes to work on challenges
* The deployed Azure components are not connected or bound to the initial deployment source and won't get updated by others
* If you need to better understand the application or the cause of some behaviour you may look into the source which is the folder `Foundation` of this repository. There is also a small [architecture documentation](..\Foundation\README.md).
