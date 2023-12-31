
using System.Text.Json;
using BaseEvents;

public class Zero
{

    public static async Task<Event[]> Richiedi()
    {


        int pages = 0;
        int page = 1;

        HttpClient client = new();

        List<Event> eventList = [];

        do
        {

            Console.WriteLine($"Zero.eu page {page} of {pages}");

            string end_date = DateOnly.FromDateTime(DateTime.Now).AddDays(31).ToString("yyyy-MM-dd");
            Console.WriteLine(end_date);

            string url = $"https://zero.eu/api/v2/events?page={page}&per_page=100&metro_area=roma&end_date={end_date}&orderby=rating&order=desc&lang=it";

            HttpResponseMessage mess = await client.GetAsync(url);

            IEnumerable<string> vals;

            if (mess.Headers.TryGetValues("X-WP-TotalPages", out vals))
            {
                pages = int.Parse(vals.First());
            }


            ZeroResponse[] events = JsonSerializer.Deserialize<ZeroResponse[]>(await mess.Content.ReadAsStringAsync()) ?? [];

            foreach (ZeroResponse zr in events)
            {
                eventList.AddRange(zr.Convert());
            }

            page++;

        } while (page <= pages);

        return [.. eventList];

    }
}