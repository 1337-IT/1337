#!/bin/bash

# ========== CONFIGURATION ==========
resourceGroup="MerchStoreRG-deployment"

echo "⚠️ WARNING: This will permanently delete the Azure Web App, App Service Plan, and all resources in group '$resourceGroup'."

read -p "Are you sure you want to continue? (yes/no): " confirm
if [[ "$confirm" != "yes" ]]; then
  echo "❌ Aborted."
  exit 1
fi

# ========== DELETE THE RESOURCE GROUP ==========
echo "🧹 Deleting resource group: $resourceGroup..."
az group delete --name $resourceGroup --yes --no-wait

echo "✅ Deletion initiated for resource group: $resourceGroup"
echo "⏳ It may take a few minutes for everything to be removed."
