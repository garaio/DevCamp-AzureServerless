# Recommended infrastructure for challenges
## Development Environment
Especially when working with Azure Function, either [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017+](https://visualstudio.microsoft.com/de/vs/) (Community edition sufficient) is highly recommended.

### Extensions for Visual Studio
![](..\Resources\Preparation_VisualStudio2019-Configuration.png)

-   [Cloud Explorer](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.CloudExplorerForVS2019) (included in Azure Development Workload)
-   When working with Logic Apps: [Logic App Designer](https://marketplace.visualstudio.com/items?itemName=VinaySinghMSFT.AzureLogicAppsToolsForVS2019)

### Extensions for Visual Studio Code
-   [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account)
-   [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
-   When working with Logic Apps: [Azure Logic Apps](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-logicapps)

## Other Tools
-   [Postman](https://www.postman.com/downloads/)
-   [Azure PowerShell](https://github.com/Azure/azure-powershell#installation)
-   [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)
-   [Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)

# Some relevant online resources
- [Cloud Design Patterns](https://docs.microsoft.com/en-us/azure/architecture/patterns/)

# Deploy lab to your Azure environment
1. Clone the repository to your local system with [Azure PowerShell](https://github.com/Azure/azure-powershell#installation) installed
1. Run this PowerShell script which provides a guided installation into your subscription: [`Foundation\Garaio.DevCampServerless.Deployment\Deploy-WithLogin.ps1`](..\Foundation\Garaio.DevCampServerless.Deployment\Deploy-WithLogin.ps1). You have to enter a suffix to be appended to the name of all Azure services and its URL's which have to be unique. It is recommended to use an abbreviation of your name (e.g. John Wayne -> jwa).
1. Wait some minutes after the deployment completed (the functions are automatically built and provisionioned)
1. The components including the function's API is ready to use and the demo-application is available composing this URL: _coming soon_

# Notes to work on challenges
* The deployed Azure components are not connected or bound to the initial deployment source and won't get updated by others
* You can start by forking the solution in the folder `Foundation` or on a complete fresh basis. There is a small [architecture documentation](..\Foundation\README.md).
* Functions can be deployed (and updated) directly from the recommended IDE's (with according extensions)
* There is a function which simulates usage by manipulating the data. This can be usefull but also be annoying for your work. In the latter situation you can easily stop it by stopping the function in the Azure Portal