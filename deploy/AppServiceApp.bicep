param appServiceAppName string
param appServicePlanName string
param location string
param appServicePlanSku string

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: appServicePlanSku
  }
}

resource appServiceApp 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}

output deploymentOutputs object = {
  webApp: {
    defaultHostName: 'https://${appServiceApp.properties.defaultHostName}/'
  }
}

