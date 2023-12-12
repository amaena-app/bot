using BaseEvents;

public class ZeroResponse
{

    public class MultiValue
    {

        public string plain { get; set; }
    }

    public class Coords
    {

        public double lat { get; set; }

        public double lon { get; set; }
    }

    public class Image{

        public string file {get; set;}
    }

    public DateTime date_scope { get; set; }

    public MultiValue name { get; set; }

    public MultiValue content { get; set; }

    public MultiValue excerpt { get; set; }

    public string start_time { get; set; }

    public string venue_name { get; set; }

    public Coords venue_coords {get; set;}

    public string[] category {get; set;}

    public Image featured_image {get; set;}

    public Event Convert(){

        string cdn = "https://cdn.zero.eu/uploads/";

        DateTime finalDate = date_scope;

        if(!start_time.Equals("")){
            TimeOnly time = TimeOnly.Parse(start_time);

            finalDate = DateOnly.FromDateTime(date_scope).ToDateTime(time);
        }
        
        string summ = content.plain.Length > excerpt.plain.Length ? content.plain : excerpt.plain;

        return new(){

            name = name.plain,

            address = new(){
                address = "",
                name = venue_name,
                latitude = venue_coords.lat,
                longitude = venue_coords.lon
            },

            date = finalDate,
            images = [cdn + featured_image.file],
            summary = summ,
            tags = category

        };

    }

}