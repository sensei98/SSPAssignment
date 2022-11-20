using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using AzureDevopsAssignment.Service;
using System.Security.Cryptography;
using System.Linq;
using Azure;
using Azure.Data.Tables;
using AzureDevopsAssignment.Model;

namespace AzureDevopsAssignment.Functions
{
    public class UploadImage
    {
        public static readonly MD5 Algo = MD5.Create();

        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string queueName = Environment.GetEnvironmentVariable("queue-name");

            QueueClient queueClient = new(Connection, queueName);

            Stream myBlob = new MemoryStream();
            var file = req.Form.Files["image"];

            if (file.Length > 0x800000) // 8 MB
                return new BadRequestObjectResult("An image can be at least 8mb");


            myBlob = file.OpenReadStream();
            BlobHttpHeaders header = new()
            {
                ContentType = file.ContentType
            };
            var blobClient = new BlobContainerClient(Connection, containerName);
            var blob = blobClient.GetBlobClient(file.FileName);
            //string uuid = Guid.NewGuid().ToString();

            string md5 = GenerateHash(myBlob);

            if (await blob.ExistsAsync())
            {
                return new OkObjectResult($"File exists, Result={md5}");
            }
            else
            {
                myBlob.Position = 0;

                await blob.UploadAsync(myBlob, header);
                await QueueStorage.AddToQueue(file.FileName, queueClient);
            }


            return new OkObjectResult($"image uploaded successfully: {md5}");
        }
        private static string GenerateHash(Stream stream)
        {
            var hash = Algo.ComputeHash(stream);
            return string.Concat(hash.Select(i => i.ToString("x2")));
        }
    }
}
