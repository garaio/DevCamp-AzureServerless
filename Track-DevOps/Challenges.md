# Overview
1. [DO01 - Infrastructure as Code](#do01-infrastructure-as-code)
1. [DO02 - Continous Deployment](#do02-continous-deployment)

# DO01: Infrastructure as Code
## Goal
The solution shall be deployable on ideally "one click" (or very, very few steps) to new or existing environments (such as Dev, Test, Int, Prod). In case of existing environments it shall be re-entrant and update only new/changed settings. The definition should not be environment specific but work with variables and parameters for specific settings.

Based on your implementation: What are your thoughts regarding:
* Validation of the definitions?
* Conditional / module-specific deployments?
* Versioning / release-documentation?

It is not required that your solution includes everything (e.g. Binaries for Functions or Search Index). In such cases some conceptual thoughts / proposal is sufficent.

## Potential Tools / Azure Services
CLI / PowerShell, Resource Manager

## Hints
* As a GARAIO employee you see the reference deployment in the Resource Group `Coming soon` in the Azure Portal. You can check the settings or event export the ARM template (on most services)

# DO02: Continous Deployment
## Goal
Based on [DO01](#do01-infrastructure-as-code), the definition shall be deployed to your Azure subscription on appropriate events in development process.

This may include following aspects:
* Environment & configurations management
* Initialization of deployed components & application data
* Manage pipeline-definition in source

## Potential Tools / Azure Services
DevOps

## Hints
You should work with an Azure subscription in your own tenant/directory. If the setup of a Service Connection in DevOps (recently refactored) does not work, you may [follow these instructions](https://docs.microsoft.com/de-de/azure-stack/operator/azure-stack-create-service-principals).
