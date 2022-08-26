using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CosmosDbDemo.Models
{
    public class NewsItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "title")]
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string? Description { get; set; }

        [JsonProperty(PropertyName = "imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonProperty(PropertyName = "creationTime")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}