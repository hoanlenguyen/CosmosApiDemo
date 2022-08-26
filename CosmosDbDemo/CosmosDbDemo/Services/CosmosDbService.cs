using CosmosDbDemo.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDbDemo.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<NewsItem>> GetItemsAsync(string queryString)
        {
            var query = container.GetItemQueryIterator<NewsItem>(new QueryDefinition(queryString));
            List<NewsItem> results = new List<NewsItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<NewsItem> GetItemAsync(string id)
        {
            if (id == null) return null;
            try
            {
                ItemResponse<NewsItem> response = await container.ReadItemAsync<NewsItem>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddItemAsync(NewsItem item)
        {
            await container.CreateItemAsync<NewsItem>(item, new PartitionKey(item.Id));
        }

        public async Task UpdateItemAsync(NewsItem item)
        {
            await container.UpsertItemAsync<NewsItem>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            if (id == null) return;

            var item = await GetItemAsync(id);

            if (item != null)
            {
                await container.DeleteItemAsync<NewsItem>(id, new PartitionKey(id));
            }
            else
            {
                throw new ArgumentException("Can not find item with this id");
            }
        }
    }
}