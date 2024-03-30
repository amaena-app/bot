using System.Net;
using System.Text.Json;
using BaseEvents;

public class Eventim {
    public static async Task<Event[]> Richiedi(City city){

        int page = 1;
        int pages = 1;

        CookieContainer cookies = new();
        HttpClientHandler handler = new(){
            CookieContainer = cookies,
            AllowAutoRedirect = false,
            UseProxy = false
        };

        HttpClient client = new(handler){
            DefaultRequestHeaders = {
                { "User-Agent", "curl/8.6.0" },
            },
            
        };

        client.DefaultRequestHeaders.Add("accept", "*/*");

        List<Event> eventList = []; 

        do
        {
            //Console.WriteLine($"Eventim({city.nomeIta}) page {page} of {pages}");
            string url = $"https://public-api.eventim.com/websearch/search/api/exploration/v2/productGroups?webId=web__ticketone-it&language=it&retail_partner=ITT&city_ids={city.eventimCode}&city_ids=null&sort=Recommendation&in_stock=true&page={page}";
                        //https://public-api.eventim.com/websearch/search/api/exploration/v2/productGroups?webId=web__ticketone-it&language=it&retail_partner=ITT&city_ids=216&city_ids=null&sort=Recommendation&in_stock=true&page=2
                        //Console.WriteLine(url);
            HttpResponseMessage mess = await client.GetAsync(url );

            //Console.WriteLine(await mess.Content.ReadAsStringAsync());

            EventimResponse events = JsonSerializer.Deserialize<EventimResponse>(await mess.Content.ReadAsStringAsync()) ?? new();


            pages = events.totalPages;
            eventList.AddRange(events.Convert(city));

            page++;

        } while (page <= pages);

        return [.. eventList];


    }
}