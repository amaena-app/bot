using System.Text.Json.Serialization;

class EventBriteRequest{
    
    public class EventSearch{

        public int page_size {get; set;}
        public int page {get; set;}
        public string[] dates {get; set;}

        public string[] places {get; set;}

        public string[] tags {get; set;}


    }

    
    public string browse_surface {get; set;}

    public EventSearch event_search {get; set;}

    [JsonPropertyName("expand.destination_event")]
    public string[] destination_event {get; set;}

    [JsonPropertyName("expand.destination_profile")]
    public string[] destination_profile {get; set;}

}