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

namespace AzureDevopsAssignment.QueueTriggers
{
    public class ColorImageTrigger
    {
        public readonly HttpClient httpClient;

        [FunctionName("ColorImageTrigger")]
        public async Task Run([QueueTrigger("myqueuesitems", Connection = "AzureWebJobsStorage")] string filename, ILogger log)
        {

            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string queueName = Environment.GetEnvironmentVariable("queue-name");

            string tableName = Environment.GetEnvironmentVariable("table-storage");
            TableClient client = new(Connection, tableName);
            QueueClient queueClient = new(Connection, queueName);
            //CloudBlobClient

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Connection);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

            HttpClient httpClient = new HttpClient();

            MemoryStream memoryStream = new MemoryStream();
            
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Position = 0;

            var (changedImage, colors) = ImageHelper.EditImage(memoryStream.ToArray());

            BlobClient blobClient = new BlobClient(Connection, "testde", filename);
            MemoryStream stream = new MemoryStream(changedImage);

            //var colorname = getColorName("#ffffff", httpClient);
            //log.LogInformation("");
            var colorname = getColorName(changedImage.ToString());
            //log.LogInformation(colorname.ToString());
            Console.WriteLine(colorname);

            await blobClient.UploadAsync(stream);
           // await blobClient.UploadAsync(stream);




        }







        private async Task UpdateTableStatus(string id, string updatedStatus, TableClient client)
        {
            Status entity = await client.GetEntityAsync<Status>("status", id);
            entity.status = updatedStatus;
            await client.UpdateEntityAsync(entity, Azure.ETag.All, TableUpdateMode.Replace);
        }

        private async Task<String> getColorName(string hex)
        {
            var res = await httpClient.GetAsync($"https://www.thecolorapi.com/id?hex={hex}");
            string content = await res.Content.ReadAsStringAsync();
            Color color = JsonConvert.DeserializeObject<Color>(content);
            return color.Name.Value;

        }
        //public async Task<String> FetchColorHexNames(string query, HttpClient httpClient)
        //{
        //    var res = await httpClient.GetAsync($"https://api.color.pizza/v1/?values={query}");
        //    string content = await res.Content.ReadAsStringAsync();
        //    //TODO

        //}
        //GET IMAGE FROM BLOB STORAGE
        //DONWLOAD IMAGE
        //CALL EDIT AND COLOR THING IN TRIGGER
        //APPLY THOSE TO THE IMAGE
        //SEND THAT TO A NEW QUEUE
        //THATS IT
    }

}


//BlobStorage blobStorage = new(containerName);
//TableStorage tableStorage = new(tableName);
//QueueStorage queueStorage = new(queueName);
//var httpClient = new HttpClient();
//byte[] data = await blobStorage.DownloadBlob(id);

//get image
//log.LogInformation($"processing color of {id}");
//await UpdateTableStatus(id, "getting image",client);

//extract colors
//(byte[], string[]) res = ImageHelper.EditImage(data);

//await UpdateTableStatus(id, "", client);
//string color = await getColorName(res.Item2[0], httpClient);


//add text to image
//byte[] image = ImageHelper.AddTextToImage(data,
//    (color, (10, 10), 48, "FFFF00"), ("this is a test", (10, 80), 24, "fffffff"));

//update status
//add to queue

// log.LogInformation("does it work??")
//
// 


//CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);




//var blobClient = new BlobContainerClient(Connection, containerName);
//var blob = cloudBlobClient.GetBlobClient(filename);
//CloudBlockBlob cloudBlockBlob = blob.


//CloudBlockBlob blobReference = cloudBlobContainer.GetBlockBlobReference(filename);;