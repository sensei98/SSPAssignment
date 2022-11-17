using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace AzureDevopsAssignment.Service
{
    public class BlobStorage
    {
        string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        string containerName;
        BlobContainerClient containerClient;

        public BlobStorage(String containerName)
        {
            this.containerName = containerName;
            this.containerClient = new(Connection, containerName);
        }

        public async Task<Byte[]> DownloadBlob(String blob)
        {
            BlobClient client = containerClient.GetBlobClient(blob);
            var res = await client.DownloadContentAsync();
            return res.Value.Content.ToArray();
        }
    }
}

