using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

class Bot
{
    private static HttpClient client = new()
    {
        BaseAddress = new Uri("https://www.eventbrite.it"),

    };

    async static Task<string> Richiedi(String request)
    {

        client.DefaultRequestHeaders.Add("Referer", "https://www.eventbrite.it/b/italy--roma/sports-and-fitness/");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        client.DefaultRequestHeaders.Add("X-CSRFToken", "vxX4zDkfKhma7Fpv46ZIy3Gi0lhxjR4u");
        client.DefaultRequestHeaders.Add("Cookie", "G=v%3D2%26i%3Dedc4c47d-e80a-4a8b-a59d-77a478c2011b%26a%3D11f5%26s%3D0f40bcb6d807e435c81c02ef76ef4cb8ac5dab97; csrftoken=vxX4zDkfKhma7Fpv46ZIy3Gi0lhxjR4u; mgref=typeins; _dd_s=rum=0&expire=1701448488919; tinuiti=a6cdc070-9bd9-4fce-ab37-e59f17f23eae");




        HttpResponseMessage mess = await client.PostAsync(content: new StringContent(request, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json")), requestUri: new Uri("https://www.eventbrite.it/api/v3/destination/search/"));

        return await mess.Content.ReadAsStringAsync();
    }

    static void Main(String[] args)
    {
        String request;
        using (StreamReader file = new StreamReader("request.json"))
        {
            request = file.ReadToEnd();
            file.Close();
        }

        //Console.WriteLine(request);
        var t = Task.Run(() => Richiedi(request));
        t.Wait();
        /*,
            "EventbriteFormat/16",
            "EventbriteFormat/1"
        */

        //Console.WriteLine(t.Result.ToString());
        EventResponseList erl = JsonSerializer.Deserialize<EventResponseList>(t.Result.ToString());

        Console.WriteLine(erl);

        using (StreamWriter file = new StreamWriter("events.json")){
            file.WriteLine(erl.ToString());
            file.Flush();
            file.Close();
        }

        //Console.WriteLine(req.Result);

    }
}

