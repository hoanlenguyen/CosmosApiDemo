using CosmosApiDemo.Models;
using CosmosApiDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosApiDemo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(ICosmosDbService cosmosDbService,
                                ILogger<ItemController> logger)
        {
            _cosmosDbService = cosmosDbService;
            _logger = logger;
        }

        [Route("GetAllItems")]
        [HttpGet]
        public async Task<IEnumerable<Item>> GetAllItems()
        {
            return await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
        }

        [Route("GetItemById")]
        [HttpGet]
        public async Task<Item> GetItemById(string id)
        {
            return await _cosmosDbService.GetItemAsync(id);
        }

        [Route("Create")]
        [HttpPost]
        public async Task AddItemAsync(Item item)
        {
            await _cosmosDbService.AddItemAsync(item);
        }

        [Route("Update")]
        [HttpPut]
        public async Task UpdateItemAsync(Item item)
        {
            await _cosmosDbService.UpdateItemAsync(item);
        }

        [Route("DeleteById")]
        [HttpPut]
        public async Task DeleteItemAsync(string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
        }

    }
}