
using System.Text.Json;
using BaseEvents;

public class Zero{

    public static async Task<Event[]> Richiedi(){


        int pages = 0;
        int page = 1;

        HttpClient client = new();

        List<Event> eventList = [];

        do{

            Console.WriteLine($"Zero.eu page {page} of {pages}");

            string url = $"https://zero.eu/api/v2/events?page={page}&per_page=100&metro_area=roma&end_date=2024-02-05&orderby=rating&order=desc&lang=it";

            HttpResponseMessage mess = await client.GetAsync(url);

            IEnumerable<string> vals;

            if(mess.Headers.TryGetValues("X-WP-TotalPages", out vals)){
                pages = int.Parse(vals.First());
            }


            ZeroResponse[] events = JsonSerializer.Deserialize<ZeroResponse[]>(await mess.Content.ReadAsStringAsync()) ?? [];

            foreach(ZeroResponse zr in events){
                eventList.Add(zr.Convert());
            }

            page++;

        }while(page <= pages);

        return [.. eventList];

    }
}