using System.Globalization;
using System.Net;
using System.Text.Json;
using BaseEvents;
using HtmlAgilityPack;

class OggiRoma
{


    public static async Task<Event[]> Richiedi(string categoria)
    {

        string url = "https://www.oggiroma.it/eventi/" + categoria + "/";

        Console.WriteLine($"checking categoria {categoria}");

        HtmlWeb web = new();

        int max_pages = 1;

        List<Task<Event[]>> TaskList = [];

        //Console.WriteLine(max_pages);


        for (int i = 1; i <= max_pages; i++)
        {
            HtmlNode doc = web.Load(url + i + "/").DocumentNode;

            HtmlNode? lastPageNode = doc.SelectSingleNode("/html/body/div[1]/div[1]/div/div/div[1]/div[11]/div/ul/li[5]/a");

            if (lastPageNode != null)
            {
                string[] temp = lastPageNode.Attributes["href"].Value.Split("/");

                max_pages = int.Parse(temp[^2]);
            }


            string? eventList = doc.SelectSingleNode("/html/body/script[12]")?.InnerText.Replace("\\\\", "\\");

            OggiRomaResponse[] list = JsonSerializer.Deserialize<OggiRomaResponse[]>(eventList ?? "[]");


            foreach (OggiRomaResponse ev in list)
            {

                TaskList.Add(Task.Run(() => RichiediEvento(ev, categoria)));

            }
        }

        Task.WaitAll(TaskList.ToArray());

        List<Event> eventlist = [];

        for (int i = 0; i < TaskList.Count; i++)
        {
            eventlist.AddRange(TaskList[i].Result);
        }



        return [.. eventlist];

    }


    private static async Task<Event[]> RichiediEvento(OggiRomaResponse baseEvent, string categoria)
    {
        HtmlWeb web = new();

        Console.WriteLine($"    checking event {baseEvent.name} of {categoria}");

        HtmlNode doc = web.Load(baseEvent.url).DocumentNode;

        baseEvent.description = doc.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div[1]/div[1]").InnerText ?? baseEvent.description;

        HtmlNode via = doc.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div[1]/ul/li[5]/a");
        baseEvent.location.address.streetAddress = via?.InnerText ?? baseEvent.location.address.streetAddress;


        string[] coords = via?.Attributes["href"].Value.Split("@")[^1].Split(",") ?? ["" + Tools.ROME_LAT, "" + Tools.ROME_LON, ""];

        if(coords.Length < 2){
            coords = ["" + Tools.ROME_LAT, "" + Tools.ROME_LON, ""];
        }
        if (coords[0].Equals("")){
            coords[0] = "" + Tools.ROME_LAT;
            Console.WriteLine("lat: " + coords[0]);
        }
        if(coords[1].Equals("")){
            coords[1] = "" + Tools.ROME_LON;
            Console.WriteLine("lon: " + coords[1]);
        }

        NumberFormatInfo pointSeparator = new()
        {
            NumberDecimalSeparator = "."
        };

        NumberFormatInfo commaSeparator = new(){
            NumberDecimalSeparator = ","
        };

        NumberFormatInfo latSeparator, lonSeparator;

        if(coords[0].Contains(',')){
            latSeparator = commaSeparator;
        }else{
            latSeparator = pointSeparator;
        }

        if(coords[1].Contains(',')){
            lonSeparator = commaSeparator;
        }else{
            lonSeparator = pointSeparator;
        }

        double latitude, longitude;
       try
       {
            latitude = Convert.ToDouble(coords[0] , latSeparator);
            longitude = Convert.ToDouble(coords[1], lonSeparator);
       }
       catch (System.Exception)
       {
        foreach (string item in coords)
        {
            Console.WriteLine(item);
        }
        throw;
       }
        DateOnly oggi = DateOnly.FromDateTime(DateTime.Now);

        int stop = Math.Min(oggi.AddDays(31).DayNumber, baseEvent.endDate.DayNumber);

        int start = Math.Max(oggi.DayNumber, DateOnly.FromDateTime(baseEvent.startDate).DayNumber);

        List<Event> events = [];

        for(int i = 0; i <=  stop - start; i++){

        Event newEvent = baseEvent.Convert(latitude, longitude,categoria);

        newEvent.date = newEvent.date.AddDays(i);

        events.Add(newEvent);

        }

        return [.. events];

    }

}