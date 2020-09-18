# Overview
1. [O01 - Monitoring Dashboard](#o01-monitoring-dashboard)
1. [O02 - Operating Surveillance](#o02-operating-surveillance)
1. [O03 - Business Intelligence](#o03-business-intelligence)

# O01: Monitoring Dashboard
## Goal
Provide a central dashboard which visualize the health and usage of every relevant service used in the application.

## Potential Tools / Azure Services
Dashboard, Monitor, Application Insights

## Hints
* The functions emit some custom metrics to Application Insights which provides an easy measuremnt of business activities

# O02: Operating Surveillance
## Goal
The deployed application contains several errors and flaws. Can you provide a simple mechanism to find them? How can the support team be notified (in an appropriate way) when criticial errors occur systematically?

Additionally you may take care of the impact of the load to costs and performance. How can this relationship be measured and the operations team be informed?

## Potential Tools / Azure Services
Alert Rules, Action Groups, Log Analytics (Workspace / Application Insights)

## Hints
* Azure Recipes for [Best Practices and Examples for Alerts](https://github.com/garaio/AzureRecipes/tree/master/Knowledge/BestPractices-AzureSolutions-Monitoring#alerts)

# O03: Business Intelligence
## Goal
Provide all relevant business data in Power BI for custom analytics. How can they be extracted and imported from different sources? Can you extend those data with operational insights (such as current costs or the timestamp of last releases/changes)?

Based on your concept you may generate a dashboard for demonstration (including e.g. range of skills from all users). Additionally you can setup an automated actualization of the dataset.

## Potential Tools / Azure Services
Power BI (Desktop), Log Analytics Workspace, Activity Log

## Hints
* There is a Power BI Desktop Template file provided including the most relevant data connections and models: [Template](./O03/Application-Dashboard.pbit)
* You can directly generate and export Power BI snippets (M queries) of KUSTO queries in Log Analytics views
