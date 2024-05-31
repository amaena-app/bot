
using System.Text.Json;
using BaseEvents;

public class Zero
{

    public static async Task<Event[]> Richiedi(City city)
    {


        int pages = 0;
        int page = 1;

        HttpClient client = new();

        List<Event> eventList = [];

        do
        {
            

            string end_date = DateOnly.FromDateTime(DateTime.Now).AddDays(31).ToString("yyyy-MM-dd");
            
            if(Tools.VERBOSE){
                Console.WriteLine($"Zero.eu({city.nomeIta}) page {page} of {pages}");
                Console.WriteLine(end_date);
            }

            string url = $"https://zero.eu/api/v2/events?page={page}&per_page=100&metro_area={city.nomeIta.ToLower()}&end_date={end_date}&orderby=rating&order=desc&lang=it";

            HttpResponseMessage mess = await client.GetAsync(url);


            IEnumerable<string> vals;

            if (mess.Headers.TryGetValues("X-WP-TotalPages", out vals))
            {
                pages = int.Parse(vals.First());
            }

            try{

            ZeroResponse[] events = JsonSerializer.Deserialize<ZeroResponse[]>(await mess.Content.ReadAsStringAsync()) ?? [];

            foreach (ZeroResponse zr in events)
            {
                eventList.Add(zr.Convert());
            }
            }catch(Exception e){
                Console.WriteLine(e.Message);
            }

            page++;

        } while (page <= pages);

        return [.. eventList];

    }
}