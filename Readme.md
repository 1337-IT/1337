# Merch Store

# MerchStore - ASP.NET Core Docker App

This is a full-stack .NET web application containerized with Docker and hosted on Docker Hub.

## 🔽 Pull the Image.

```bash
docker pull muhammadaamir11/merchstore:net9

docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ApiKey__Value="Merchstore_ApiKey" \
  -e CosmosDbSettings__ConnectionString="mongodb://cosmosdbmerchstore:6hca7dJsQUTyict7vnXZ2oHX7jduoNHLrtlwwUkfn6ed09p5xvsjtHT1YtiJAZsJZjUmIvMRCZ2BACDbPfBlsQ==@cosmosdbmerchstore.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@cosmosdbmerchstore@" \
  -e BlobStorage__ConnectionString="DefaultEndpointsProtocol=https;AccountName=merchstoreblobstorage;AccountKey=7MUoHLJFvPwyKwKAoOs26ATDLWaru09z/ItgkIa7YzS0vbqOhGUdZRg+lEwHVm5Su4jZb+4hk+yv+AStEgjcYw==;EndpointSuffix=core.windows.net" \
  -e BlobStorage__ContainerName="hats" \
  muhammadaamir11/merchstore:net9
