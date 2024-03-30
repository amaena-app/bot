using System.Text.Json;
using System.Text.Json.Serialization;
using BaseEvents;

public class EventimResponse
{
    public int totalPages { get; set; }

    public EventimEvent[] productGroups { get; set; }


    private string convertTags(string[] tags)
    {
        if (tags.Contains("Concerti"))
            return "concert";
        if (tags.Contains("Teatro"))
            return "theater";
        if (tags.Contains("Mostre e Musei"))
            return "mostra";
        if (tags.Contains("Cinema"))
            return "cinema";
        if (tags.Contains("Sport"))
            return "sport";

        return "event";
    }

    public Event[] Convert(City city)
    {
        List<Event> events = new();

        foreach (EventimEvent ee in productGroups)
        {
            Event e = new()
            {
                name = ee.name,
                summary = ee.description,
                images = [ee.imageUrl]
            };
            List<string> categories = new();
            foreach (EventimEvent.EventimCategory ec in ee.categories ?? [])
            {
                categories.Add(ec?.name ?? "Event");
            }
            e.tags = [convertTags([.. categories])];


            try
            {

                e.address = new()
                {
                    name = ee.products[0].typeAttributes.liveEntertainment.location.name,
                    address = "",
                    latitude = ee.products[0].typeAttributes.liveEntertainment.location.geoLocation?.latitude ?? city.lat,
                    longitude = ee.products[0].typeAttributes.liveEntertainment.location.geoLocation?.longitude ?? city.lon
                };
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine(JsonSerializer.Serialize(ee.products));
                throw;
            }

            e.from_date = DateOnly.FromDateTime(ee.startDate);
            e.to_date = DateOnly.FromDateTime(ee.endDate);
            e.time = TimeOnly.FromDateTime(ee.startDate);

            events.Add(e);
        }

        return [.. events];
    }
}


public class EventimEvent
{

    public class EventimCategory
    {
        public string name { get; set; }
    }

    public class EventimProduct
    {
        public class TypeAttribute
        {

            public class LiveEntertainment
            {
                public class Location
                {
                    public class Coords
                    {
                        public double latitude { get; set; }
                        public double longitude { get; set; }
                    }
                    public string name { get; set; }
                    public Coords geoLocation { get; set; }
                }
                public Location location { get; set; }
            }

            public LiveEntertainment liveEntertainment { get; set; }
        }

        public TypeAttribute typeAttributes { get; set; }
    }

    public string name { get; set; }
    public string description { get; set; }
    public string imageUrl { get; set; }

    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }

    public EventimCategory[] categories { get; set; }
    public EventimProduct[] products { get; set; }


}