param appServiceAppName string
param appServicePlanName string
param location string
param appServicePlanSku string
param acrUrl string
param acrImageName string
param acrImageTag string
param logAnalyticsWorkspaceId string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: appServicePlanSku
  }
  properties: {
    reserved: true
  }
}

resource appServiceApp 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOCKER|${acrUrl}/${acrImageName}:${acrImageTag}'
    }
  }
  kind: 'app,linux,container'
  identity: {
    type: 'SystemAssigned'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${appServiceAppName}-insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    IngestionMode: 'LogAnalytics'
    WorkspaceResourceId: logAnalyticsWorkspaceId
  }
}

output deploymentOutputs object = {
  webApp: {
    defaultHostName: 'https://${appServiceApp.properties.defaultHostName}/'
  }
  appInsights: {
    instrumentationKey: appInsights.properties.InstrumentationKey
  }
}
