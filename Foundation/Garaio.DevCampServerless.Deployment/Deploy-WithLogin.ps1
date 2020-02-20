Connect-AzAccount

Get-AzSubscription

$SubscriptionId = Read-Host -Prompt 'Enter id of subscription you want to use for deployment'

Set-AzContext -SubscriptionId $SubscriptionId

& ((Split-Path $MyInvocation.InvocationName) + "\Deploy-AzTemplate.ps1")