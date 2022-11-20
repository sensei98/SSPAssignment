az group create --name ServerSIdeProgrammingClass --location eastus
az deployment group create --resource-group ServerSIdeProgrammingClass --template-file main.bicep

