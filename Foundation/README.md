# Architecture
## Application Components (Azure Services)
![Components](./Resources/Components.png)

### Service Function App
All challenges of this lab are focused to this component. It mainly provides a simple RESTful API of:
* Persons
* Projects
* Technologies
* Search (used for challenge D02)
* User-profile (used for challenge D04)

### Emulator Function App
This is just a utility function to initialize some data (seeding) and to simulate usage by periodically calling the REST API of the service function. It is not foreseen to do anything with this function when working on the challenges.

### Web Application
_coming soon_

## Data Model
![DataModel](./Resources/DataModel.png)

# Deployment
Make sure [Azure PowerShell](https://github.com/Azure/azure-powershell#installation) is installed on your device and run script `Garaio.DevCampServerless.Deployment\Deploy-WithLogin.ps1`.
For subsequent deployments it is recommendet to open `Windows PowerShell ISE` and run `Garaio.DevCampServerless.Deployment\Deploy-AzTemplate.ps1` directly with existing authentication context and selection of subscription.