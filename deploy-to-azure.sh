#!/bin/bash

# ========== CONFIGURATION ==========
appname="fanta-stick-six"  
resourceGroup="MerchStoreRG-deployment"
planName="MerchStorePlan"
location="swedencentral"
dockerImage="muhammadaamir11/merchstore:v1.0.0"

# ========== EXECUTION ==========
echo "Creating resource group..."
az group create --name $resourceGroup --location $location

echo "Creating Linux App Service plan..."
az appservice plan create \
  --name $planName \
  --resource-group $resourceGroup \
  --is-linux \
  --sku B1

echo "Creating Web App and deploying Docker image..."
az webapp create \
  --resource-group $resourceGroup \
  --plan $planName \
  --name $appname \
  --deployment-container-image-name $dockerImage

echo "Opening app in browser..."
az webapp browse --resource-group $resourceGroup --name $appname

echo "✅ DONE: Your app should now be live at:"
echo "https://$appname.azurewebsites.net"
