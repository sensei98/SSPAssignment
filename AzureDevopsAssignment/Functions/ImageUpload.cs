using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using Azure.Storage.Blobs.Models;
using AzureDevopsAssignment.Service;

using Azure.Storage.Queues;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureDevopsAssignment
{
    
    public static class ImageUpload
    {
        public static readonly MD5 Algo = MD5.Create();
        

        [FunctionName("ImageUpload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,ILogger log)
        {
            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string queueName = Environment.GetEnvironmentVariable("queue-name");

            QueueClient queueClient = new(Connection, queueName);

            Stream myBlob = new MemoryStream();
            var file = req.Form.Files["image"];


            myBlob = file.OpenReadStream();
            BlobHttpHeaders header = new()
            {
                ContentType = file.ContentType
            };
            var blobClient = new BlobContainerClient(Connection, containerName);
            var blob = blobClient.GetBlobClient(file.FileName);
            //string uuid = Guid.NewGuid().ToString();

            string md5 = GenerateHash(myBlob);

            if(await blob.ExistsAsync()) {
                return new OkObjectResult($"File exists, Result={md5}");
            }
            else
            {
                myBlob.Position = 0;

                await blob.UploadAsync(myBlob, header);
                //await QueueStorage.AddToQueue(md5".png", queueClient); //get image type and pass it here
                await QueueStorage.AddToQueue(file.FileName, queueClient);
            }
           

            return new OkObjectResult($"image uploaded successfully: {md5}");
        }
        private static string GenerateHash(Stream stream)
        {
            var hash = Algo.ComputeHash(stream);
            return string.Concat(hash.Select(i => i.ToString("x2")));
        }

        //public static async Task CreateQueueMessage(string message,QueueClient queueClient)
        //{
        //    string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        //    await queueClient.SendMessageAsync(base64Message);
        //}


    }
   

}

