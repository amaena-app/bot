
public class EventResponseList
{
    public EventResponseList()
    {
        events = new Events();
    }

    public Events events { get; set; }

    public static EventResponseList operator +(EventResponseList a, EventResponseList  b){
        a.events += b.events;

        return a;
    }

    public override string ToString()
    {
        return $"{events}";
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
            public string display_name { get; set; }
            public string prefix { get; set; }
        }

        public DateOnly? start_date { get; set; }
        public string summary { get; set; }
        public string name { get; set; }

        public Place primary_venue { get; set; }

        public Tag[] tags { get; set; }

        public override string ToString()
        {
            string bettername = name.Replace("\n", "").Replace("\"", "\\\"");
            string bettersummary = summary.Replace("\n", "").Replace("\"", "\\\"");

            return $"{{\"name\": \"{bettername}\", \"summary\" : \"{bettersummary}\",  \"date\":\"{start_date}\", \"address\" : {primary_venue},  \"type\" : \"{tags.Where<Tag>((item) => item.prefix.Equals("EventbriteCategory")).First().display_name}\" }}";
        }
    }

    public class Pagination
    {

        public Pagination()
        {
            object_count = 0;
        }
        public int object_count { get; set; }
    }

    public Event[] results { get; set; }

    public Pagination pagination { get; set; }

    public static Events operator +(Events a, Events b){
        Event[] result = new Event[a.results.Length + b.results.Length];
        a.results.CopyTo(result, 0);
        b.results.CopyTo(result, a.results.Length);

        a.results = result;
        a.pagination.object_count = Math.Max(a.pagination.object_count, b.pagination.object_count);
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

        public string address_1 { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }


    }

    public string name { get; set; }

    public Address address { get; set; }

    public override string ToString()
    {
        return $"{{ \"name\" : \"{name}\", \"address\" : \"{address.address_1}\", \"latitude\" : {address.latitude}, \"longitude\" : {address.longitude} }}";
    }


}