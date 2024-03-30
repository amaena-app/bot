public class City
{


    public string nomeIta { get; set; }
    public string nomeEng { get; set; }

    public string eventBriteCode { get; set; }

    public CustomRequests[] requests { get; set; }

    public double lat { get; set; }
    public double lon { get; set; }

    public int eventimCode { get; set; }

    public City(string nomeIta, string nomeEng, string eventBriteCode, int eventimCode, CustomRequests[] requests, double lat, double lon)
    {

        this.nomeIta = nomeIta;
        this.nomeEng = nomeEng;
        this.eventBriteCode = eventBriteCode;
        this.requests = requests;
        this.lat = lat;
        this.lon = lon;
        this.eventimCode = eventimCode;

    }

}

public static class Cities
{
    public static  City ROMA = new City("Roma", "Rome", "101752607", 216 ,[new OggiRoma()], 41.8992, 12.5450);

    public static City MILANO = new City("Milano", "Milan", "101752703", 215,[], 45.4613, 9.1595);

    public static City[] list = [
            ROMA,
            MILANO,
    ];
}