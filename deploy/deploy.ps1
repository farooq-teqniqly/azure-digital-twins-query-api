<#
.SYNOPSIS
The deployment script creates the following resources:

    - An Azure Static Web App instance.

.PARAMETER location
The location the Azure resources will be created in.

.PARAMETER subscriptionId
The ID of the Azure subscription to use for the operation.
#>
Param (
    [Parameter(Mandatory = $true)]
    [string]$location,
    
    [Parameter(Mandatory = $true)]
    [string]$subscriptionId
)

function Get-UniquePrefix([int]$length) {
    $alphabet = "abcdefghijklmnopqrstuvwxyz"
    $uniquePrefix = ""

    for ($i = 0; $i -lt $length; $i++) {
        $random = Get-Random -Minimum 0 -Maximum ($alphabet.Length - 1)
        $randomLetter = $alphabet[$random]
        $uniquePrefix += $randomLetter
    }

    return $uniquePrefix
}

$uniquePrefix = Get-UniquePrefix -length 6
$baseName = "adtqueryapi"
$baseNameWithUniquePrefix = "{0}-{1}" -f $baseName, $uniquePrefix
$deploymentName = "{0}-deployment-{1}" -f $baseNameWithUniquePrefix, (Get-Random)
$resourceGroupName = "{0}-rg" -f $baseNameWithUniquePrefix
$appServiceAppName = "{0}appsvc" -f $baseNameWithUniquePrefix -replace "-", ""
$appServicePlanName = "{0}asp" -f $baseNameWithUniquePrefix -replace "-", ""
$appServicePlanSku = "B1"
$acrName = "{0}acr" -f $baseNameWithUniquePrefix -replace "-", ""
$acrSku = "Basic"
$acrImageName = "adt-query-api"
$acrImageTag = "latest"

$logAnalyticsWorkspaceId =  az monitor log-analytics workspace show `
    --resource-group DefaultResourceGroup-$location `
    --workspace-name DefaultWorkspace-$location `
    --query customerId -o tsv

Write-Host "Your unique prefix for the Azure resource group and resources is " -NoNewline
Write-Host $uniquePrefix -ForegroundColor Cyan

Write-Host "Your resource group name is " -NoNewline 
Write-Host $resourceGroupName -ForegroundColor Cyan

az login --scope https://management.core.windows.net/.default
az account set --subscription $subscriptionId
az group create --name $resourceGroupName --location $location

$templateFileName = Join-Path $PSScriptRoot "Main.bicep"

Write-Host "Starting deployment..."

$output = az deployment sub create `
    --name $deploymentName `
    --location $location `
    --template-file $templateFileName `
    --parameters `
    resourceGroupName=$resourceGroupName `
    location=$location `
    appServiceAppName=$appServiceAppName `
    appServicePlanName=$appServicePlanName `
    appServicePlanSku=$appServicePlanSku `
    acrName=$acrName `
    acrSku=$acrSku `
    acrImageName=$acrImageName `
    acrImageTag=$acrImageTag `
    logAnalyticsWorkspaceId=$logAnalyticsWorkspaceId
   
if (!$output) {
    Write-Warning "Deployment failed. Resource group will be deleted in the background."
    az group delete --name $resourceGroupName --yes --no-wait
    exit 1
}

Write-Host $output | ConvertFrom-Json | Format-Table -AutoSize
Write-Host "Deployment complete." -ForegroundColor Cyan
Write-Host "To cleanup Azure resources, you may run the following commands:" -ForegroundColor Cyan
Write-Host "az group delete --name $resourceGroupName --no-wait --yes" -ForegroundColor Yellow