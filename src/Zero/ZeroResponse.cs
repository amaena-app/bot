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

    public class Image
    {

        public string file { get; set; }
    }

    public DateTime date_scope { get; set; }

    public DateOnly start_date { get; set; }

    public DateOnly end_date { get; set; }

    public MultiValue name { get; set; }

    public MultiValue content { get; set; }

    public MultiValue excerpt { get; set; }

    public string start_time { get; set; }

    public string venue_name { get; set; }

    public Coords venue_coords { get; set; }

    public string[] category { get; set; }

    public Image featured_image { get; set; }

    public string convertTags(string[] tags)
    {
        if (tags.Contains("Bere e Mangiare"))
            return "food";
        if (tags.Contains("Mercatini"))
            return "market";
        if (tags.Contains("Teatro") || tags.Contains("Spettacoli"))
            return "theater";
        if (tags.Contains("Cinema"))
            return "cinema";
        if (tags.Contains("Incontri"))
            return "conference";
        if (tags.Contains("Festival"))
            return "festival";
        if (tags.Contains("Clubbing") || tags.Contains("Concerti"))
            return "concert";
        if (tags.Contains("Mostre"))
            return "mostra";
        if (tags.Contains("Festival"))
            return "festival";

        return "event";

    }

    public Event Convert()
    {

        string cdn = "https://cdn.zero.eu/uploads/";

        /*List<Event> events = [];

        DateOnly oggi = DateOnly.FromDateTime(DateTime.Now);

        int end = Math.Min(oggi.AddDays(31).DayNumber, end_date.DayNumber);
        int start = Math.Max(oggi.DayNumber, start_date.DayNumber);

        for (int i = 0; i <= end - start; i++)
        {
            DateTime finalDate = DateOnly.FromDayNumber(start).AddDays(i).ToDateTime(TimeOnly.Parse(!start_time.Equals("") ? start_time : "00:00:00"));

            string summ = content.plain.Length > excerpt.plain.Length ? content.plain : excerpt.plain;

            events.Add (new()
            {

                name = name.plain,

                address = new()
                {
                    address = "",
                    name = venue_name,
                    latitude = venue_coords.lat,
                    longitude = venue_coords.lng
                },

                date = finalDate,
                images = [cdn + featured_image.file],
                summary = summ,
                tags = [convertTags(category)]

            });
        }

        return [.. events];*/

        return new(){
            name = name.plain,
            from_date = start_date,
            to_date = end_date,
            time = TimeOnly.Parse(!start_time.Equals("") ? start_time : "00:00:00"),
            images = [cdn + featured_image.file],
            summary = content.plain,
            tags = [convertTags(category)],
            address = new(){
                address = "",
                name = venue_name,
                latitude = venue_coords.lat,
                longitude = venue_coords.lng
            }
        };


    }

}