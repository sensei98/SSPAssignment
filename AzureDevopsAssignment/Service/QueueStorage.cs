using System;
using Azure.Storage.Queues;
using System.Threading.Tasks;
using System.Text;

namespace AzureDevopsAssignment.Service
{
    public class QueueStorage
    {
        private string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private QueueClient queueClient;
        private string queueName;

        public QueueStorage(string queueName)
        {
            this.queueName = queueName;
            this.queueClient = new(Connection, queueName);
        }
        //public async Task AddToQueue(string message)
        //{
        //    await queueClient.SendMessageAsync(message);
        //}

        public static async Task AddToQueue(string message, QueueClient queueClient)
        {
            string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
            await queueClient.SendMessageAsync(base64Message);
        }
    }
}

