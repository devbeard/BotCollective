using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BirthdayBot.Core.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace BirthdayBot.Core.Repositories
{
    public class DatabaseController
    {
        private string PartitionKey { get; }

        private const string TableName = "people";

        private CloudStorageAccount StorageAccount { get; }

        public DatabaseController(string connectionString, string partitionkey)
        {
            PartitionKey = partitionkey;
            StorageAccount = CloudStorageAccount.Parse(connectionString);
            CreateTable();
        }

        private void CreateTable()
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            table.CreateIfNotExists();
        }

        private void EmptyTable()
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            var batchOperation = new TableBatchOperation();

            var projectionQuery = new TableQuery<DynamicTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, PartitionKey))
                .Select(new[] { "RowKey" });

            var counter = 0;

            foreach (var e in table.ExecuteQuery(projectionQuery))
            {
                counter++;
                batchOperation.Delete(e);
            }

            if (counter > 0)
            {
                table.ExecuteBatch(batchOperation);
            }
        }
        

        public IEnumerable<PersonEntity> GetAllPersonEntities()
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            var query = new TableQuery<PersonEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey));

            var results = table.ExecuteQuery(query);

            return results;
        }

        public bool InsertOrReplacePersonEntity(PersonEntity pe)
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            var operation = TableOperation.InsertOrReplace(pe);
            table.Execute(operation);
            return true;
        }

        public bool DeletePerson(PersonEntity pe)
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            var operation = TableOperation.Delete(pe);
            table.Execute(operation);
            return true;
        }

        public void SetCongratulated(IEnumerable<PersonEntity> people)
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            foreach (var pe in people)
            {
                pe.LastCongratulation = DateTime.Now;
                var operation = TableOperation.InsertOrReplace(pe);
                table.Execute(operation);
            }
        }

        public void FillTable()
        {
            EmptyTable();

            const string filename = @"data-set.txt";
            var sr = new StreamReader(filename);

            var list = new List<PersonEntity>();

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line == null) continue;

                var split = line.Split(';');

                var person = new PersonEntity()
                {
                    Name = split[0],
                    Birthday = string.IsNullOrEmpty(split[1]) ? (DateTime?)null : DateTime.Parse(split[1], CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal),
                    SlackUserName = string.IsNullOrEmpty(split[2]) ? null : split[2],
                    LastCongratulation = DateTime.UtcNow,
                    Active = bool.Parse(split[3]),
                    PartitionKey = PartitionKey,
                    Gender = split[4],
                    RowKey = Guid.NewGuid().ToString()
                };

                list.Add(person);
            }

            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            foreach (var operation in list.Select(TableOperation.Insert))
            {
                table.Execute(operation);
            }            
        }
    }
}
