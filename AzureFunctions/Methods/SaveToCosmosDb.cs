using System;
using System.Text;
using Azure.Messaging.EventHubs;
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
        private readonly CosmosClient _cosmosclient;
        private readonly Container _container;
        public SaveToCosmosDb(ILogger<SaveToCosmosDb> logger)
        {
            _logger = logger;

            _cosmosclient = new CosmosClient("AccountEndpoint=https://kyh-smartunitcosmosdb.documents.azure.com:443/;AccountKey=ImCYA5pgXRqg6qSGDu7bkaOn3HRrrlp6nfeyyzDfYizs45y6Esi19ZSbYqGETqXWrnLIiswGjaRHACDbWY58pw==;");
            var database = _cosmosclient.GetDatabase("kyh");
            _container = database.GetContainer("Messages");
        }

        [Function(nameof(SaveToCosmosDb))]
        public async Task Run([EventHubTrigger("iothub-ehub-kyh-smartu-25230147-8f58cd7876samples-workitems", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {

                try
                {
                    var json = Encoding.UTF8.GetString(@event.EventBody.ToArray());
                    var data = JsonConvert.DeserializeObject<DataMessage>(json);
                    await _container.CreateItemAsync<DataMessage>(data, new PartitionKey(data.id));
                    _logger.LogInformation($"Message saved: {data}");
                }
                catch
                {
                    _logger.LogInformation("Failed to save!");
                }
            }
        }
    }
}
