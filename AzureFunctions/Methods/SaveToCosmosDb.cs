using System;
using System.Text;
using Azure.Messaging.EventHubs;
using AzureFunctions.Contexts;
using AzureFunctions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Methods
{
    public class SaveToCosmosDb
    {
        private readonly ILogger<SaveToCosmosDb> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public SaveToCosmosDb(ILogger<SaveToCosmosDb> logger)
        {
            _logger = logger;
 
            _cosmosClient = new CosmosClient("AccountEndpoint=https://kyh-smartunitcosmosdb.documents.azure.com:443/;AccountKey=ImCYA5pgXRqg6qSGDu7bkaOn3HRrrlp6nfeyyzDfYizs45y6Esi19ZSbYqGETqXWrnLIiswGjaRHACDbWY58pw==;");
            var database = _cosmosClient.GetDatabase("deviceDb");
            _container = database.GetContainer("devicemessages");
        }

        [Function(nameof(SaveToCosmosDb))]
        public async Task Run([EventHubTrigger("iothub-ehub-kyh-smartu-25230147-8f58cd7876",
            Connection = "IotHub")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {
                    var json = Encoding.UTF8.GetString(@event.Body.ToArray()); 
                    var data = JsonConvert.DeserializeObject<DataMessage>(json);
                    await _container.CreateItemAsync(data, new PartitionKey(data.Id)); 

                    _logger.LogInformation($"Saved message: {data}"); 
                }
                catch
                {
                    _logger.LogInformation("Failed to save");
                }
            }

          
        }
    }
}
