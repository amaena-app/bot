using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class EventBrite
{
    public static async Task<EventBriteResponseList> Richiedi(string request, City city)
    {

        HttpClient client = new();
        
        client.DefaultRequestHeaders.Add("Referer", $"https://www.eventbrite.it/b/italy--{city.nomeIta.ToLower()}/sports-and-fitness/");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        client.DefaultRequestHeaders.Add("X-CSRFToken", "vxX4zDkfKhma7Fpv46ZIy3Gi0lhxjR4u");
        client.DefaultRequestHeaders.Add("Cookie", "G=v%3D2%26i%3Dedc4c47d-e80a-4a8b-a59d-77a478c2011b%26a%3D11f5%26s%3D0f40bcb6d807e435c81c02ef76ef4cb8ac5dab97; csrftoken=vxX4zDkfKhma7Fpv46ZIy3Gi0lhxjR4u; mgref=typeins; _dd_s=rum=0&expire=1701448488919; tinuiti=a6cdc070-9bd9-4fce-ab37-e59f17f23eae");


        HttpResponseMessage mess = await client.PostAsync(content: new StringContent(request, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json")), requestUri: new Uri("https://www.eventbrite.it/api/v3/destination/search/"));

        
        EventBriteResponseList newEvent = JsonSerializer.Deserialize<EventBriteResponseList>(await mess.Content.ReadAsStringAsync()) ?? new();

        return newEvent;
    }
}