#!/bin/bash

# ========== CONFIG ==========
resourceGroup="MerchStoreRG-deployment"
appname="fanta-stick6"

# ========== SECRETS (insert actual values) ==========
apiKey="Merchstore_ApiKey"

cosmosConn="mongodb://cosmosdbmerchstore:6hca7dJsQUTyict7vnXZ2oHX7jduoNHLrtlwwUkfn6ed09p5xvsjtHT1YtiJAZsJZjUmIvMRCZ2BACDbPfBlsQ==@cosmosdbmerchstore.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@cosmosdbmerchstore@"
cosmosDb="MerchStore"
cosmosCollection="Products"

blobConn="DefaultEndpointsProtocol=https;AccountName=merchstoreblobstorage;AccountKey=7MUoHLJFvPwyKwKAoOs26ATDLWaru09z/ItgkIa7YzS0vbqOhGUdZRg+lEwHVm5Su4jZb+4hk+yv+AStEgjcYw==;EndpointSuffix=core.windows.net"
blobContainer="hats"

# ========== INJECT APP SETTINGS ==========
echo "Injecting secrets into Azure Web App settings..."

az webapp config appsettings set \
  --resource-group $resourceGroup \
  --name $appname \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ApiKey__Value="$apiKey" \
    CosmosDbSettings__ConnectionString="$cosmosConn" \
    CosmosDbSettings__DatabaseName="$cosmosDb" \
    CosmosDbSettings__CollectionName="$cosmosCollection" \
    BlobStorage__ConnectionString="$blobConn" \
    BlobStorage__ContainerName="$blobContainer"

echo "✅ Secrets injected for $appname"

