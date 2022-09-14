namespace MyOwn.Company.Items
{
    using Newtonsoft.Json;

    public class ToDoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string userId { get; set; }
        
         
         [JsonProperty("productId")]
        public string productId { get; set; }
       
         [JsonProperty("timestamp")]
        public string timestamp { get; set; }
       
         [JsonProperty("locationName")]
        public string locationName { get; set; }
       
        
         [JsonProperty("rating")]
        public string rating { get; set; }
    
     
         [JsonProperty("userNotes")]
        public string userNotes { get; set; }
    
  
    }
}