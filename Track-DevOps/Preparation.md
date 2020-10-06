# Recommended infrastructure for challenges
## Development Environment
Especially if you work with ARM templates for the definition of the deployed Azure components, either [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017+](https://visualstudio.microsoft.com/de/vs/) (Community edition sufficient) is recommended as it provides syntax and validation support.

### Extensions for Visual Studio
![](..\Resources\Preparation_VisualStudio2019-Configuration.png)

### Extensions for Visual Studio Code
-   [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account)
-   [Azure Resource Manager (ARM) Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account)
-   [Azure CLI Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azurecli)

## Other Tools
-   [Postman](https://www.postman.com/downloads/)
-   [Azure PowerShell](https://github.com/Azure/azure-powershell#installation)
-   [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)

# Some relevant online resources
- [Azure REST API Reference](https://docs.microsoft.com/en-us/rest/api/azure/)
- [ARM Template Reference](https://docs.microsoft.com/en-us/azure/templates/)
- [ARM Quickstart Template](https://azure.microsoft.com/en-us/resources/templates/)
- [Azure CLI Reference](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest)
- [Azure PowerShell Reference](https://docs.microsoft.com/en-us/powershell/module/az.resources/?view=azps-3.4.0)

# Access reference deployment
As a GARAIO employee you see the reference deployment in the Resource Group **`gro-dcs-ref`** in the Azure Portal. You only have read-rights there so everyone has a stable basis to work with. Additionally you can access and use the demo-application available with this URL: **`https://grodcssaref.z6.web.core.windows.net`** (only working during the DevCamp and without guarantee).

# Notes to work on challenges
* You may use the described [setup with Azure DevOps](../Foundation/Setup/Manual.md) as the basis
* There is also a small [architecture documentation](../Foundation/README.md) in the same folder.
