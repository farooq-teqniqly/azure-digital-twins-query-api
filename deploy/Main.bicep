targetScope = 'subscription'

param location string
param resourceGroupName string
param appServiceAppName string
param appServicePlanName string
param appServicePlanSku string

resource resourceGroupDeploy 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

module webAppDeploy 'AppServiceApp.bicep' = {
  name: 'webAppDeploy'
  scope: resourceGroupDeploy
  params: {
    appServiceAppName: appServiceAppName
    appServicePlanName: appServicePlanName
    location: location
    appServicePlanSku: appServicePlanSku
  }
}

output deploymentOutputs object = {
  resourceGroupName: resourceGroupName
  webAppDeployment: {
    defaultHostName: webAppDeploy.outputs.deploymentOutputs.webApp.defaultHostName
  }
}
