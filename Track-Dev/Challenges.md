# Overview
## Basic Azure Serverless
1. [D01 - Provide and manage API](#d01-provide-and-manage-api)
1. [D02 - Intelligent Search](#d02-intelligent-search)
1. [D03 - Flexible data storage](#d03-flexible-data-storage)
1. [D04 - User management & Authentication](#d04-user-management-&-authentication)
1. [D05 - Data-integrations](#d05-data-integrations)
1. [D06 - Data-processing pipeline](#d06-data-processing-pipeline)
1. [D07 - Event Sourcing & Propagation](#d07-event-sourcing--propagation)
1. [D08 - Improved SPA-Provisioning](#d08-improved-spa-provisioning)
1. [D09 - App Configuration](#d09-app-configuration)
1. [D10 - Client Notifications](#d10-client-notifications)

## Experience Platform
1. [D20 - Big Data Analytics](#d20-big-data-analytics)
1. [D21 - Power Plattform Integration](#d21-power-plattform-integration)
1. [D22 - Microsoft Teams Integration](#d22-microsoft-teams-integration)

## Frontend Development
1. [D30 - Simplified Frontend Apps](#d30-simplified-frontend-apps)
1. [D31 - Static Landing Page](#d31-static-landing-page)
1. [D32 - Frontend Component Packaging](#d32-frontend-component-packaging)
1. [D33 - Corporate Design System](#d33-corporate-design-system)
1. [D34 - Consolidated Logging](#d34-consolidated-logging)

# D01: Provide and manage API
## Goal
The funtionality shall be available to other applications - developped internally as well by external providers. You need to provide a simple API documentation (OpenAPI) as well as the possibility to test the API without custom implementation. The type of API integration shall be provided to the function in a stable way. There is already one concrete requirement that one external integration shall exchange the data XML-formatted instead of standard JSON.

## Potential Azure Services
API Management

## Hints
* Recipe for setup of [API Management with Functions](https://github.com/garaio/AzureRecipes/tree/master/Snippets/ARM/function-api-management)

# D02: Intelligent Search
## Goal
Provide search functionality to the pre-existing function. Optionally you may include:
* fuzzy-search
* facets
* entity-recognition from project descriptions
* or ability to search all (localized) content in English

How can you consider similarities? How can you score or use scoring to qualify results? Examples:
* If one search for an Angular-skilled Dev, it may also show Devs with other JavaScript-Framework experience
* If one search for a JavaScript professional, it could be worthy to get results ordered by a score combined form e.g. range of known Frameworks, number of different projects & years of experiance

You may further extend application-logic with type-ahead/autosuggest functionality in the search-box.
Another interesting functionality could be the extraction of intends from the search input. So if the user enters e.g. "define skill" the application opens the according functionality instead of displaying search results.

## Potential Azure Services
Cognitive Search (with Skills), LUIS

## Hints
Integration in Frotend-application is optional (it is completely okay to just demonstrate functionality and concepts on API or Search Explorer).

# D03: Flexible data storage
## Goal
Implement a data storage concept which allows the dynamic definition of properties for projects and users (in an optimized way). This shall also include the optimiced storage of document attachements. Additionally you may provide a basis implementation for a simple "media library" of project contents.

## Potential Azure Services
Cosmos DB

## Hints
* Adation of Frotend-application to changes is optional (it is completely okay to show functionality on Functions test page only)
* Recipe for setup of [Cosmos DB with Entity Framework Core](https://github.com/garaio/AzureRecipes/tree/master/Snippets/csharp/ef-core-with-cosmos-db-sql)
* Some data is highly referenced to each other (Projects-Users-Skills-Technologies) - what are the chances and benfits of a graph database ([Cosmos DB Gremlin API](https://docs.microsoft.com/en-us/azure/cosmos-db/graph-introduction))?

# D04: User management & Authentication
## Goal
The application needs an appropriate management of its users. It shall as much as possible be separated form the application itself. Provide a management solution with some test accounts as well as a basic integration into the application which shows the concept for data-linking and authorization functionality (roles).

## Potential Azure Services
AD B2C

## Hints
The authentication process with the Frotend-application should work, but its not required to integrate user data in the Frontend (e.g. profile or sign-out functionality).

# D05: Data-integrations
## Goal
The application shall integrate other systems and data sources to provide valueable predefined content for users which simplifies usage. The idea is to have a reference implementation to show how this can be achieved in a simple and easy maintainable way. There are two concreate ideas for this (you can implement one or both):
* List of technologies with sort of a "popularity score" which can be used for cool analytics later. This should be periodically updated / re-evaluated so that the trend of this score is available. The pre-evaluated source if [hotframeworks.com](https://hotframeworks.com).
* List of project from organisations SharePoint. It should only create not yet existing projects but avoid the creation of multiple projects with the same name. It shall copy or link all relevant data (e.g. description, logo, links).

## Potential Azure Services
Function with Graph-Binding, Logic App, Data Factory Pipeline

## Hints
* For this lab it is not required to scrap data from [hotframeworks.com](https://hotframeworks.com) automatically (CSV exports in folder `D05`)
* On the GARAIO AG SharePoint you find project sites under: `\<base-url>/sites/solutions-hub/SitePages/Alle-Projekte.aspx`

# D06: Data-processing pipeline
## Goal
There are two scenarios for asynchronous data processing:
1. The definition of a technology should be simple and recognize automatically additive information such as Logo, Framework-site and others
1. As the CV-contents as well as project descriptions shall be used for marketing purposes (public), it must be ensured that it does not contain damage causing content. Therefore whenever a sensitive field is edited by a user, it shall become a status "validating" which starts an automatic validation process to check if it can directly be "published" or needs to be set to "manual validation".

## Potential Azure Services
Event Grid, Functions/Logic Apps, Content Moderator, Bing Entity Search, Bing Spell Check, SendGrid Account

## Hints
_(none)_

# D07: Event Sourcing & Propagation
## Goal
As a business requirement all changes on entities must be tracked so a complete history could be made available.

Furthermore external systems must have a possibility to react on some key events such as creation or deletion of an entity (at best filterable to particular types). Such subscriptions shall be triggered as immediately as possible and be properly manageable. Potential interfaces could be:
* HTTP Webhooks (defined format)
* AMQP Queue/Topic Binding

## Potential Azure Services
Event Grid, Service Bus, Functions/Logic Apps

## Hints
_(none)_

# D08: Improved SPA-Provisioning
## Goal
The provisioning of the Demo UI (as Angular-based Single Page Application) with the capabilities of the Storage Account lacks the support of client side routing functionality. As soon as a sub-route is established, the application cannot be reloaded anymore. As this is a crucial functionality (e.g. to share links) a solution is needed to resolve this problem.
Additionally you may provide a solution to use a custom domain or subdomain to access the application.

## Potential Azure Services
CDN, DNS Zone

## Hints
* To implement and test the custom domain functionality you need to have such a domain available.
* Recipe for setup of [CDN with routing rules](https://github.com/garaio/AzureRecipes/tree/master/Snippets/ARM/setup-CDN-with-rule-for-SPA)

# D09: App Configuration
## Goal
It is foreseen to massively extend the application with additional services implemented with specific Function Apps. To support this, a solution for central, stable management of business configurations is required. This should also include the concept of [`Feature Toggles`](https://martinfowler.com/articles/feature-toggles.html).

Provide a solution which allows the activation/deactivation of some functionality (e.g. search functionality). Integration into Frotend-application is optional.

## Potential Azure Services
App Configuration

## Hints
* [Azure App Configuration Quickstart Tutorial](https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-functions-csharp)

# D10: Client Notifications
## Goal
The application shall be extended with notification functionaly to shows news of e.g. finished asynchronous tasks or messages to user.

Provide a solution to consume such notifications for the current user in an efficient way (i.e. polling should be avoided).

## Potential Azure Services
SignalR Service

## Hints
* [Azure SignalR Service Quickstart Tutorial](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview)

# D20: Big Data Analytics
## Goal
Look at the current application as it is part of a bigger landscape with many similar application. A central solution to ingest all relevant application data shall be implemented. Provide a base implementation which shows the relevant concepts for ingestion and direct analytics.

## Potential Azure Services
Azure Synapse Workspace

## Hints
* It may be reasonable to first migrate the Storage Tables based database to Cosmos DB SQL API

# D21: Power Plattform Integration
## Goal
The data managed within the application shall be directly available to Power Apps and Power Automate implementations. To prevent additional license costs, the integration shall be done using Microsoft Dataflex (previously known as Common Data Service (CDS) consisting of data structures defined with Common Data Model definitions).

## Potential Azure Services
Microsoft Dataflex (aka Common Data Service)

## Hints
* _Note: It is unsure if this is currently realizable and/or what licenses are required to setup and use Microsoft Dataflex_
* [Demo Session of Build 2019](https://azure.microsoft.com/en-us/resources/videos/build-2019-building-azure-apps-using-the-common-data-service)

# D22: Microsoft Teams Integration
## Goal
As Microsoft Teams is becoming the central platform for employee application, a reference integration shall be provided.

The goal is to view the current Frontend-application (or a simplified rebuild using another technology) as an App in Teams. At best the user authentication can be linked with a SSO experience and upcoming notifications in the application are transmitted also to the Teams integration.

## Potential Azure Services
_(none)_

## Hints
* The basis of a Teams App is to have a `Progressive Web App (PWA)`

# D30: Simplified Frontend Apps
## Goal
The vision is to provide frontend applications as more simple `Micro Frontends`. Single parts of the current application shall be provided as independent modules with an independent technology stack (e.g. Vue.js, Blazor). 

Provide a simple reference implementation of such a module (e.g. management of technologies) with an independent, lightweight provisioning.

## Potential Azure Services
Static Web Apps

## Hints
* [Simplified setup with SWA templates (navigate to 'Quickstarts')](https://docs.microsoft.com/en-us/azure/static-web-apps/overview)

# D31: Static Landing Page
## Goal
To provide an immediate available of the Frontend-application on the initial load, so called `Static Site Generators` have been discussed. There is an idea to load initially a very lightweight, static page (aka `Jamstack`) while loadind and materializing the SPA components in the background.

Provide a simple reference implementation which demonstrates the functionality.

It is not required that this bases on the current Frontend-application or any specific technology.

## Potential Azure Services
_(none)_

## Hints
_(none)_

# D32: Frontend Component Packaging
## Goal
As there are bunch of similar Frontend-applications planned, a system/solutions which allows the easy reuse of single components could be valuable.

Provide a simple reference implementation which show how components can be defined and reused. At best this is integrated in a continous integration/delivery process which also handles versioning and update functionality. 

It is not required that this bases on the current Frontend-application or any specific technology.

## Potential Azure Services
_(none)_

## Hints
* Popular solutions are [`bit.dev`](https://bit.dev/) and [`Lerna`](https://github.com/lerna/lerna)

# D33: Corporate Design System
## Goal
The company has a growing number of web applications and is struggling to maintain a consistent styling and look&feel of all those applications. The idea is to somehow build a `design system` which beside a living style guide also provides a way to centrally manage CSS styles/themes wich are directly applied by applications.

Provide a simple reference implementation which show how CSS styles can be managed outside of the application scope and applied by the application in a stable and managed way. At best this is integrated in a continous integration/delivery process which also handles versioning and update functionality.

It is not required that this bases on the current Frontend-application or any specific technology.

## Potential Azure Services
_(none)_

## Hints
* It became popular to manage company-wide Style Guides with `Brand Management Software` such as e.g. [Frontify](https://www.frontify.com/en/). It may improve customer acceptance if style definitions can be completely managed and controlled in such a system

# D34: Consolidated Logging
## Goal
For improved error analysis but mainly to generate comprehensive business insights, relevant events occurring in the Frontend-application shall directly be logged to the `Application Insights` already set up and used by the Azure services.

Depending on the data available, the logging shall include context information (e.g. user identity, device information) to enable the pre-build usage analysis in `Application Insights`.

## Potential Azure Services
Application Insights

## Hints
* [Microsoft Tutorial for Angular integration](https://devblogs.microsoft.com/premier-developer/angular-how-to-add-application-insights-to-an-angular-spa/)
