targetScope = 'subscription'

param location string
param resourceGroupName string
param appServiceAppName string
param appServicePlanName string
param appServicePlanSku string
param acrName string
param acrSku string
param acrImageName string
param acrImageTag string
param logAnalyticsWorkspaceId string

resource resourceGroupDeploy 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

module containerRegistryDeploy 'ContainerRegistry.bicep' = {
  name: 'containerRegistryDeploy'
  scope: resourceGroupDeploy
  params: {
    acrName: acrName
    location: location
    acrSku: acrSku
  }
}

module webAppDeploy 'AppServiceApp.bicep' = {
  name: 'webAppDeploy'
  scope: resourceGroupDeploy
  params: {
    appServiceAppName: appServiceAppName
    appServicePlanName: appServicePlanName
    location: location
    appServicePlanSku: appServicePlanSku
    acrUrl: containerRegistryDeploy.outputs.deploymentOutputs.acr.loginServer
    acrImageName: acrImageName
    acrImageTag: acrImageTag
    logAnalyticsWorkspaceId: logAnalyticsWorkspaceId
  }
}

output deploymentOutputs object = {
  resourceGroupName: resourceGroupName
  acrDeployment: {
    loginServer: containerRegistryDeploy.outputs.deploymentOutputs.acr.loginServer
  }
  webAppDeployment: {
    defaultHostName: webAppDeploy.outputs.deploymentOutputs.webApp.defaultHostName
    appInsightsInstrumentationKey: webAppDeploy.outputs.deploymentOutputs.appInsights.instrumentationKey
  }
}
