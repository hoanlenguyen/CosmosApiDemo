using CosmosApiDemo.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosApiDemo.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id == null) return null;
            try
            {
                ItemResponse<Item> response = await this._container.ReadItemAsync<Item>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddItemAsync(Item item)
        {
            item.Id = Guid.NewGuid().ToString();
            await this._container.CreateItemAsync<Item>(item, new PartitionKey(item.Id));
        }

        public async Task UpdateItemAsync(Item item)
        {
            await this._container.UpsertItemAsync<Item>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            if (id == null) return;

            var item = await GetItemAsync(id);

            if (item != null)
            {
                await this._container.DeleteItemAsync<Item>(id, new PartitionKey(id));
            }
            else
            {
                throw new ArgumentException("Can not find item with this id");
            }
        }
    }
}