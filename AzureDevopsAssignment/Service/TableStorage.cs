using System;
using Azure.Storage.Blobs;

namespace AzureDevopsAssignment.Service
{
    public class TableStorage
    {
        public string tableStorage;

        string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        BlobContainerClient containerClient;

        public TableStorage(String tableStorage)
        {
            this.tableStorage = tableStorage;
            containerClient = new(Connection, tableStorage);
        }
    }
}

