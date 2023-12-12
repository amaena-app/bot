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

    public string description { get; set; }

    public string image { get; set; }

    public Location location { get; set; }

    public string url {get; set;}


    public Event Convert(string tag = "")
    {

        return new()
        {
            name = name,
            date = startDate,
            images = [image],
            summary = description,
            tags = [tag],
            address = new()
            {
                address = location.address.streetAddress ?? "",
                name = location.name,
                latitude = Tools.ROME_LAT,
                longitude = Tools.ROME_LON
            },

        };

    }


}