using System;
using Azure;
using Azure.Data.Tables;
namespace AzureDevopsAssignment.Model
{
    public class Status : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string status;

        public Status(string key, string status)
        {
            PartitionKey = status;
            RowKey = key;
            this.status = status;
        }
        public Status() { }
    }
}

