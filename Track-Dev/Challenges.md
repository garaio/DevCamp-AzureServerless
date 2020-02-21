# Overview
1. [D01 - Provide and manage API](#d01-provide-and-manage-api)
1. [D02 - Intelligent Search](#d02-intelligent-search)
1. [D03 - Flexible data storage](#d03-flexible-data-storage)
1. [D04 - User management & Authentication](#d04-user-management-&-authentication)
1. [D05 - Data-integrations](#d05-data-integrations)
1. [D06 - Data-processing pipeline](#d06-data-processing-pipeline)
1. [D07 - Security hardening](#d07-security-hardening)

# D01: Provide and manage API
## Goal
The funtionality shall be available to other applications - developped internally as well by external providers. You need to provide a simple API documentation (OpenAPI) as well as the possibility to test the API without custom implementation. The type of API integration shall be provided to the function in a stable way. There is already one concrete requirement that one external integration shall exchange the data XML-formatted instead of standard JSON.

## Potential Azure Services
API Management

## Hints
(none)

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
(none)

# D07: Security hardening
## Goal
All services must be used according to best practices, especially regarding security aspects. This shall mainly include a scalable solution for sensitive configurations and data.

## Potential Azure Services
Key Vault, Advisor

## Hints
This challenge is more reasonable (and challenging) when implemented after some other challenges in this track. Even more interesting on the basis of the DevOps basic challenge.
