using CosmosDbDemo.Models;

namespace CosmosDbDemo.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<NewsItem>> GetItemsAsync(string stringQuery);

        Task<NewsItem> GetItemAsync(string id);

        Task AddItemAsync(NewsItem item);

        Task UpdateItemAsync(NewsItem item);

        Task DeleteItemAsync(string id);

    }
}
