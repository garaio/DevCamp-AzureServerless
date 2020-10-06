# Setup Manual
This process should work with any device. You just need an Internet Browser and connectivity (there is no need to install any particular tools).

## Init Azure DevOps project and link with Azure Subscription
1. ![](Setup_Step1.png)
1. ![](Setup_Step2.png)
1. ![](Setup_Step3.png)
1. ![](Setup_Step4.png)
1. ![](Setup_Step5.png)
Service connection name: **ARM Service Connection** (_basically it could be any value - but the prepared pipeline references this particular name_)

## Import / fork the Repository
1. ![](Setup_Step6.png)
Clone URL: **https://github.com/garaio/DevCamp-AzureServerless**

## Create CI/CD Pipeline
1. ![](Setup_Step7.png)
1. ![](Setup_Step8.png)
1. ![](Setup_Step9.png)
1. ![](Setup_Step10.png)
Path: **Foundation/Garaio.DevCampServerless.Deployment/azure-pipeline.yml**
1. ![](Setup_Step11.png)
1. ![](Setup_Step12.png)
1. ![](Setup_Step13.png)
Suffix: **Specify any value which uniquely identifies the environment (e.g. your name abbreviation)**