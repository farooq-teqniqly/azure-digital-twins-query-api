param acrName string
param location string
param acrSku string

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-11-01-preview' = {
  name: acrName
  location: location
  sku: {
    name: acrSku
  }
  properties: {
    adminUserEnabled: true
  }
}

output deploymentOutputs object = {
  acr: {
    loginServer: containerRegistry.properties.loginServer
  }
}
