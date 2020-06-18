using CosmosApiDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosApiDemo.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Item>> GetItemsAsync(string stringQuery);

        Task<Item> GetItemAsync(string id);

        Task AddItemAsync(Item item);

        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(string id);
    }
}