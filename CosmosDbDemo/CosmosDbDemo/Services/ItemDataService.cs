using CosmosDbDemo.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CosmosDbDemo.Services
{
    public static class ItemDataService
    {
        public static void AddItemDataService(this WebApplication app)
        {
            app.MapGet("NewsItem/crawl",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService) =>
            {
                var url = "https://tuoitre.vn/";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);
                var htmlWeb = new HtmlWeb();
                htmlWeb.OverrideEncoding = Encoding.Unicode;
                var doc = htmlWeb.Load(url);
                doc.LoadHtml(html); 
                var node = doc.DocumentNode;
                var divs = node.Descendants("div")
                .Where(p => p.GetAttributeValue("class", "id").Equals("list-news-focus", StringComparison.OrdinalIgnoreCase))
                .ToList();
                var result = new List<NewsItem>();
                foreach (var item in divs)
                {
                    var links = item.Descendants("a").ToList();
                    foreach (var link in links)
                    {
                        var news = new NewsItem
                        {
                            Title = link.GetAttributeValue("title", "href") ?? "",
                            ImageUrl = link.Descendants("img").FirstOrDefault()?.GetAttributeValue("src","") ??""
                        };
                        result.Add(news);
                        await cosmosDbService.AddItemAsync(news);
                    }
                }

                return Results.Ok(result);
            });

            app.MapGet("NewsItem",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService,
            string id) =>
            {
                return Results.Ok(await cosmosDbService.GetItemAsync(id));
            });

            app.MapPost("NewsItem",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService,
            [FromBody] NewsItem item) =>
            {
                item.Id = Guid.NewGuid().ToString();
                await cosmosDbService.AddItemAsync(item);
                return Results.Ok();
            });

            app.MapPut("NewsItem",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService,
            [FromBody] NewsItem item) =>
            {
                await cosmosDbService.UpdateItemAsync(item);
                return Results.Ok();
            });

            app.MapDelete("NewsItem",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService,
            string id) =>
            {
                await cosmosDbService.DeleteItemAsync(id);
                return Results.Ok();
            });

            app.MapGet("NewsItem/all",
            async Task<IResult> (
            [FromServices] ICosmosDbService cosmosDbService) =>
            {
                return Results.Ok(await cosmosDbService.GetItemsAsync("SELECT * FROM c"));
            });
        }
    }
}