
using System.Globalization;
using BaseEvents;

public class EventBriteResponseList
{
    public EventBriteResponseList()
    {
        events = new Events();
    }

    public Events events { get; set; }

    public static EventBriteResponseList operator +(EventBriteResponseList a, EventBriteResponseList b)
    {
        a.events += b.events;

        return a;
    }

    public override string ToString()
    {
        return $"{events}";
    }

    public string convertTags(string[] tags)
    {
        if (tags.Contains("Sports & Fitness"))
            return "sport";
        if (tags.Contains("Concert or Performance"))
            return "concert";
        if (tags.Contains("Class, Training, or Workshop"))
            return "training";
        if (tags.Contains("Fashion & Beauty"))
            return "fashion";
        if (tags.Contains("Charity & Causes"))
            return "charity";
        if (tags.Contains("Tour") || tags.Contains("Travel & Outdoor"))
            return "escursion";
        if (tags.Contains("Book") || tags.Contains("Books"))
            return "book";
        if (tags.Contains("Conference"))
            return "conference";
        if (tags.Contains("Aperitivo") || tags.Contains("Apericena"))
            return "aperitive";
        if (tags.Contains("Performing & Visual Arts"))
            return "mostra";
        if (tags.Contains("Festival or Fair"))
            return "festival";

        return tags.FirstOrDefault("event");
    }


    public BaseEvents.Event[] Convert()
    {

        List<Event> converted = [];

        //Event[] converted = new Event[events.results.Length];

        Event newEvent;

        foreach (Events.Event i in events.results)
        {
            NumberFormatInfo separator = new()
            {
                NumberDecimalSeparator = "."
            };

            DateOnly start = i.start_date ?? DateOnly.FromDateTime(DateTime.Now);
            DateOnly stop = i.end_date ?? DateOnly.FromDateTime(DateTime.Now);
            


            for (int day = 0; day <= stop.DayNumber - start.DayNumber; day++)
            {
                newEvent = new()
                {
                    name = i.name,
                    summary = i.summary ?? "",

                    date = start.AddDays(day).ToDateTime(i.start_time != null ? TimeOnly.Parse(i.start_time) : TimeOnly.FromDateTime(DateTime.Now)),

                    address = new()
                    {
                        name = i.primary_venue.name ?? $"{i.name} Location",
                        address = i.primary_venue.address.address_1 ?? "",
                        latitude = System.Convert.ToDouble(i.primary_venue.address.latitude, separator),
                        longitude = System.Convert.ToDouble(i.primary_venue.address.longitude, separator)

                    },

                    tags = new string[i.tags.Length],
                    images = [i.image != null ? i.image.url : ""]

                };

                for (int j = 0; j < i.tags.Length; j++)
                {
                    newEvent.tags[j] = i.tags[j].display_name;
                }


                newEvent.tags = [convertTags(newEvent.tags)];

                converted.Add(newEvent);
            }


        }

        return [.. converted];


    }

}

public class Events
{

    public Events()
    {
        results = Array.Empty<Event>();
        pagination = new Pagination();
    }
    public class Event
    {

        public class Tag
        {
            public string? display_name { get; set; }
            public string? prefix { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
        }

        public DateOnly? start_date { get; set; }

        public string? start_time { get; set; }

        public DateOnly? end_date { get; set; }

        public string? summary { get; set; }
        public string name { get; set; }

        public Place primary_venue { get; set; }

        public Tag[] tags { get; set; }

        public Image? image { get; set; }

        public override string ToString()
        {
            string bettername = Tools.SanitizeString(name, false);
            string bettersummary = Tools.SanitizeString(summary);
            List<string> list = new();

            foreach (Tag tag in tags)
            {
                list.Add("\"" + tag.display_name + "\"");
            }

            string tag_list = "[" + string.Join(", ", [.. list]) + "]";


            DateOnly dateOnly = start_date ?? DateOnly.FromDateTime(DateTime.Now);

            TimeOnly timeOnly = start_time != null ? TimeOnly.Parse(start_time) : TimeOnly.FromDateTime(DateTime.Now);

            DateTime date = dateOnly.ToDateTime(timeOnly);

            string notNullImage = image != null ? image.url : "";

            return $"{{\"name\": \"{bettername}\", \"summary\" : \"{bettersummary}\",  \"date\":\"{date.ToString("yyyy-MM-dd HH:mm:ss")}\", \"address\" : {primary_venue},  \"tags\" : {tag_list}, \"images\" : [ \"{notNullImage}\"] }}";
        }
    }

    public class Pagination
    {

        public Pagination()
        {
            object_count = 0;
            page_count = 1;
        }
        public int object_count { get; set; }

        public int page_count { get; set; }
    }

    public Event[] results { get; set; }

    public Pagination pagination { get; set; }

    public static Events operator +(Events a, Events b)
    {
        Event[] result = new Event[a.results.Length + b.results.Length];
        a.results.CopyTo(result, 0);
        b.results.CopyTo(result, a.results.Length);

        a.results = result;
        a.pagination.object_count = Math.Max(a.pagination.object_count, b.pagination.object_count);
        a.pagination.page_count = Math.Max(a.pagination.page_count, b.pagination.page_count);
        return a;
    }

    public override string ToString()
    {

        string ret = "[";

        foreach (Event item in results)
        {
            ret += item.ToString() + ", ";
        }

        ret = ret.Remove(ret.Length - 2, 1) + "]";

        return ret;
    }

}

public class Place
{

    public class Address
    {

        public string? address_1 { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }


    }

    public string? name { get; set; }

    public Address address { get; set; }

    public override string ToString()
    {
        string bettername = Tools.SanitizeString(name, false);

        string betteraddress = Tools.SanitizeString(address.address_1);

        return $"{{ \"name\" : \"{bettername}\", \"address\" : \"{betteraddress}\", \"latitude\" : {address.latitude}, \"longitude\" : {address.longitude} }}";
    }


}