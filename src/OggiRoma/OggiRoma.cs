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

        List<Task<Event>> TaskList = [];

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

        Event[] eventlist = new Event[TaskList.Count];

        for (int i = 0; i < TaskList.Count; i++)
        {
            eventlist[i] = TaskList[i].Result;
        }



        return eventlist;

    }


    private static async Task<Event> RichiediEvento(OggiRomaResponse baseEvent, string categoria)
    {
        HtmlWeb web = new();

        Console.WriteLine($"    checking event {baseEvent.name} of {categoria}");

        HtmlNode doc = web.Load(baseEvent.url).DocumentNode;

        baseEvent.description = doc.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div[1]/div[1]").InnerText ?? baseEvent.description;

        HtmlNode via = doc.SelectSingleNode("/html/body/div[1]/div[1]/div[1]/div/div[1]/ul/li[5]/a");
        baseEvent.location.address.streetAddress = via?.InnerText ?? baseEvent.location.address.streetAddress;

        Event newEvent = baseEvent.Convert(categoria);

        string[] coords = via?.Attributes["href"].Value.Split("@")[^1].Split(",") ?? ["" + Tools.ROME_LAT, "" + Tools.ROME_LON, ""];


        NumberFormatInfo separator = new()
        {
            NumberDecimalSeparator = "."
        };
        newEvent.address.latitude = Convert.ToDouble(coords[^3], separator);
        newEvent.address.longitude = Convert.ToDouble(coords[^2], separator);

        return newEvent;


    }

}