using BaseEvents;

public class OggiRomaResponse
{

    public class Location
    {

        public string name { get; set; }

        public class Address
        {
            public string streetAddress { get; set; }
        }

        public Address address { get; set; }
    }

    public string name { get; set; }

    public DateTime startDate { get; set; }
    
    public DateOnly endDate {get; set;}

    public string description { get; set; }

    public string image { get; set; }

    public Location location { get; set; }

    public string url {get; set;}

    public string convertTag(string tag){
        
        switch (tag)
        {
            case "mostre" :
                return "mostra";
            case "festival":
                return "festival";
            case "bandi-e-concorsi":
                return "concorsi";
            case "spettacoli":
                return "concert";
            case "bambini-e-famiglie":
                return "kids";
            default:
                return "event";
        }
    }


    public Event Convert(double lat, double lon,string tag = "")
    {

        return new()
        {
            name = name,
            from_date = DateOnly.FromDateTime(startDate),
            to_date = endDate,
            time = TimeOnly.FromDateTime(startDate),
            images = [image],
            summary = description,
            tags = [convertTag(tag)],
            address = new()
            {
                address = location.address.streetAddress ?? "",
                name = location.name,
                latitude = lat,
                longitude = lon,
            },

        };

    }


}