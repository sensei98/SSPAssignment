param storageAccounts_serversidestorage1_name string = 'serversidestorage1'

resource storageAccounts_serversidestorage1_name_resource 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccounts_serversidestorage1_name
  location: 'eastus'
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
  kind: 'StorageV2'
  properties: {
    dnsEndpointType: 'Standard'
    defaultToOAuthAuthentication: false
    publicNetworkAccess: 'Enabled'
    allowCrossTenantReplication: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: true
    allowSharedKeyAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      requireInfrastructureEncryption: false
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

resource storageAccounts_serversidestorage1_name_default 'Microsoft.Storage/storageAccounts/blobServices@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_resource
  name: 'default'
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
  properties: {
    changeFeed: {
      enabled: false
    }
    restorePolicy: {
      enabled: false
    }
    containerDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      allowPermanentDelete: false
      enabled: true
      days: 7
    }
    isVersioningEnabled: false
  }
}

resource Microsoft_Storage_storageAccounts_fileServices_storageAccounts_serversidestorage1_name_default 'Microsoft.Storage/storageAccounts/fileServices@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_resource
  name: 'default'
  sku: {
    name: 'Standard_RAGRS'
    tier: 'Standard'
  }
  properties: {
    protocolSettings: {
      smb: {
      }
    }
    cors: {
      corsRules: []
    }
    shareDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

resource Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default 'Microsoft.Storage/storageAccounts/queueServices@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_tableServices_storageAccounts_serversidestorage1_name_default 'Microsoft.Storage/storageAccounts/tableServices@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource storageAccounts_serversidestorage1_name_default_azure_webjobs_hosts 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_default
  name: 'azure-webjobs-hosts'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_azure_webjobs_secrets 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_default
  name: 'azure-webjobs-secrets'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_devopsassignment 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_default
  name: 'devopsassignment'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_productimages 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_default
  name: 'productimages'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_testde 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_serversidestorage1_name_default
  name: 'testde'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_onlinestore20221105015138 'Microsoft.Storage/storageAccounts/fileServices/shares@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_fileServices_storageAccounts_serversidestorage1_name_default
  name: 'onlinestore20221105015138'
  properties: {
    accessTier: 'TransactionOptimized'
    shareQuota: 5120
    enabledProtocols: 'SMB'
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_imagequeue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'imagequeue'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_myqueuesitems 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'myqueuesitems'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_myqueuesitems_poison 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'myqueuesitems-poison'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_productqueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'productqueue'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_queuedemo 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'queuedemo'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_queuedemo_poison 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_queueServices_storageAccounts_serversidestorage1_name_default
  name: 'queuedemo-poison'
  properties: {
    metadata: {
    }
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_Customers 'Microsoft.Storage/storageAccounts/tableServices/tables@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_tableServices_storageAccounts_serversidestorage1_name_default
  name: 'Customers'
  properties: {
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}

resource storageAccounts_serversidestorage1_name_default_imagetable 'Microsoft.Storage/storageAccounts/tableServices/tables@2022-05-01' = {
  parent: Microsoft_Storage_storageAccounts_tableServices_storageAccounts_serversidestorage1_name_default
  name: 'imagetable'
  properties: {
  }
  dependsOn: [

    storageAccounts_serversidestorage1_name_resource
  ]
}