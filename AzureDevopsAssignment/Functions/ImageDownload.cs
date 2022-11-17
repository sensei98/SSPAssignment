using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Azure;
using AzureDevopsAssignment.Model;

using AzureDevopsAssignment.Service;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs;

namespace AzureDevopsAssignment.Functions
{
    public class ImageDownload
    {
        public static string containerName = Environment.GetEnvironmentVariable("ContainerName");
        public static string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");


        [FunctionName("ImageDownload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ImageDownload/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            //downloading image
            var blobClient = new BlobContainerClient(Connection, containerName);

            BlobStorage editedImages = new BlobStorage(containerName);

            if(!await blobClient.ExistsAsync())
            {
                return new NotFoundResult();
            }
            else
            {
                byte[] result = await editedImages.DownloadBlob(id);

                return new FileContentResult(result, "image/png");
            }

            //


        }



       
    }

}