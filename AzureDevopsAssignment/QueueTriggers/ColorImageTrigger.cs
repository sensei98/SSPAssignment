using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using AzureDevopsAssignment.Model;
using AzureDevopsAssignment.Service;
using AzureDevopsAssignment.Functions;
using Azure.Data.Tables;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Linq;
using Azure.Data.Tables.Models;
using Azure;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDevopsAssignment.QueueTriggers
{
    public class ColorImageTrigger
    {
        private readonly HttpClient httpClient;

        [FunctionName("ColorImageTrigger")]
        public async Task Run([QueueTrigger("myqueuesitems", Connection = "AzureWebJobsStorage")] string filename, ILogger log)
        {
           

            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string queueName = Environment.GetEnvironmentVariable("queue-name");

            string tableName = Environment.GetEnvironmentVariable("table-storage");
            string tableContainer = Environment.GetEnvironmentVariable("tablecontainer");
            string imageStatusTable = Environment.GetEnvironmentVariable("imageTableStatus");

            TableClient client = new(Connection, imageStatusTable);
            QueueClient queueClient = new(Connection, queueName);
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Connection);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);


            HttpClient httpClient = new HttpClient();

            MemoryStream memoryStream = new MemoryStream();
            
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Position = 0;


            var (changedImage, colors) = ImageHelper.EditImage(memoryStream.ToArray());

            BlobClient blobClient = new BlobClient(Connection, tableContainer, filename);
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }
            
            MemoryStream stream = new MemoryStream(changedImage);

           

            await blobClient.UploadAsync(stream);


        }

        private async Task<String> getColorName(string hex)
        {
            var res = await httpClient.GetAsync($"https://www.thecolorapi.com/id?hex={hex}");
            string content = await res.Content.ReadAsStringAsync();
            Color color = JsonConvert.DeserializeObject<Color>(content);
            return color.Name.Value;

        }
    }
}

