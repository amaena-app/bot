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

        public double lng { get; set; }
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

    public String convertTags(string[] tags){
        if(tags.Contains("Bere e Mangiare"))
            return "food";
        if(tags.Contains("Mercatini"))
            return "market";
        if(tags.Contains("Teatro") || tags.Contains("Spettacoli"))
            return "theater";
        if(tags.Contains("Cinema"))
            return "cinema";
        if(tags.Contains("Incontri"))
            return "conference";
        if(tags.Contains("Festival"))
            return "festival";
        if(tags.Contains("Clubbing") || tags.Contains("Concerti"))
            return "concert";
        if(tags.Contains("Mostre"))
            return "mostra";
        if(tags.Contains("Festival"))
            return "festival";

        return "event";
        
    }

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
                longitude = venue_coords.lng
            },

            date = finalDate,
            images = [cdn + featured_image.file],
            summary = summ,
            tags = [convertTags(category)]

        };

    }

}