# Overview
## Basic Azure Serverless
1. [DO01 - Infrastructure as Code](#do01-infrastructure-as-code)
1. [DO02 - Continous Deployment](#do02-continous-deployment)
1. [DO03 - Measure Performance](#do03-measure-performance)

## Frontend Development
1. [DO10 - Vulnerability & License Scanning](#do10-vulnerability-&-license-scanning)
1. [DO11 - Quality Testing](#do11-quality-testing)
1. [DO12 - E2E Testing](#do12-e2e-testing)

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
* As a GARAIO employee you see the reference deployment in the Resource Group **`gro-dcs-ref`** in the Azure Portal. You can check the settings or event export the ARM template (on most services or on resource group)
* There are some definition files exported and available in the folder `DO01`

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
_(none)_

# DO03: Measure Performance
## Goal
DevOps (the theme, not the tool Azure DevOps) is about increasing the performance of a delivery team to deliver features more frequently in higher quality. Key for continous improvement is to continously measure the performance and that the delivery team has the ability to gain insights.

Provide a concept and some reference implementation of how according KPI's may be defined, measured, visualized and made available to the project team and stakeholders.

## Potential Tools / Azure Services
DevOps, Power BI

## Hints
* Microsoft Learn Module and [Summary of DevOps KPI's](https://docs.microsoft.com/de-de/learn/modules/get-started-with-devops/5-summary)
* MSDN documentation to [Connect Power BI with Azure DevOps](https://docs.microsoft.com/en-us/azure/devops/report/powerbi/data-connector-connect?view=azure-devops)

# DO10: Vulnerability & License Scanning
## Goal
As JavaScript packages are sometimes hard to manage there is an automatic check for known vulnerabilities wanted during the CI process.

Elaborate a reasonable solution for this need.

## Potential Tools / Azure Services
DevOps

## Hints
* [Microsoft Learn module as introduction](https://docs.microsoft.com/en-us/learn/modules/scan-open-source)

# DO11: Quality Testing
## Goal
The Frontend-application shall include unit test which are executed during each build.

Elaborate a reasonable solution for this need and provide a reference implementation.

## Potential Tools / Azure Services
DevOps, [Jest](https://jestjs.io/)

## Hints
_(none)_

# DO12: E2E Testing
## Goal
For builds on the main and release branches additionaly stability tests shall be integrated which test the availabilty and correctness of the UI. Initially these may be just some Smoke-Tests (later it may be extended to more complex integration tests).

Elaborate a reasonable solution for this need and provide a reference implementation.

## Potential Tools / Azure Services
DevOps, [Cypress](https://www.cypress.io/)

## Hints
_(none)_
