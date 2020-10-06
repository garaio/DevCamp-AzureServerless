# Recommended infrastructure for challenges
## Development Environment
Either [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017+](https://visualstudio.microsoft.com/de/vs/) (Community edition sufficient) is highly recommended.

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
1. Follow [this setup manual](../Foundation/Setup/Manual.md)
1. After the pipeline has been executed successfully, the components including the function's API is ready to use and the demo-application is available composing this URL: **`https://grodcssa{YOUR-SUFFIX}.z6.web.core.windows.net`**

_Note: The URL looks different when you deploy to another Azure Region. In case of any troubles navigate to your Storage Account in the Portal and use the URL displayed in the section "Static website"_

# Notes to work on challenges
* The deployed Azure components are not connected or bound to the initial deployment source and won't get updated by others
* You can start by forking the solution in the folder `Foundation` or on a complete fresh basis. There is a small [architecture documentation](../Foundation/README.md).
* Functions can be deployed (and updated) directly from the recommended IDE's (with according extensions)
* There is a function which simulates usage by manipulating the data. This can be usefull but also be annoying for your work. In the latter situation you can easily stop it by stopping the function in the Azure Portal