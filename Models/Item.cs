namespace CosmosApiDemo.Models
{
    //must have
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }
    }
}