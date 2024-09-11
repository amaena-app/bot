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
    public static  City ROMA = new("Roma", "Rome", "101752607", 216 ,[new OggiRoma()], 41.8992, 12.5450);
    public static City MILANO = new("Milano", "Milan", "101752703", 215,[], 45.4613, 9.1595);
    public static City PERUGIA = new("Perugia", "Perugia", "101752621", 1095,[], 43.1107, 12.3908);
    public static City TERNI = new("Terni", "Terni", "101752615", 1095,[], 42.5636168, 12.6426604);
    public static City GUBBIO = new("Gubbio", "Gubbio", "101798747", 1095,[], 43.3554, 12.5734);
    public static City FOLIGNO = new("Foligno", "Foligno", "101836987", 1095,[], 42.9500, 12.7000);
    public static City CASTELLO = new("Città di Castello", "Città di Castello", "101798737", 1095,[], 43.4628, 12.2360);
    public static City SPOLETO = new("Spoleto", "Spoleto", "101800173", 1095,[], 42.7300, 12.7400);
    public static City ASSISI = new("Assisi", "Assisi", "101800185", 1095,[], 43.0700, 12.6170);
    public static City CORCIANO = new("Corciano", "Corciano", "101847691", 1095,[], 43.1100, 12.2960);
    public static City BASTIA = new("Bastia Umbra", "Bastia Umbra", "1125866703", 1095,[], 43.0667, 12.5667);
    public static City ORVIETO = new("Orvieto", "Orvieto", "101836907", 1095,[], 42.7167, 12.1167);
    public static City MARSCIANO = new("Marsciano", "Marsciano", "101800199", 1095,[], 42.8833, 12.3833);
    public static City NARNI = new("Narni", "Narni", "101836903", 1095,[], 42.5167, 12.5167);
    public static City UMBERTIDE = new("Umbertide", "Umbertide", "101798759", 1095,[], 43.3167, 12.3333);
    public static City TODI = new("Todi", "Todi", "101800153", 1095,[], 42.7833, 12.4167);
    public static City CASTIGLIONE = new("Castiglione del Lago", "Castiglione del Lago", "101800189", 1095,[], 43.1333, 12.0333);
    public static City MAGIONE = new("Magione", "Magione", "101800191", 1095,[], 43.1333, 12.2167);
    public static City GUALDO_T = new("Gualdo Tadino", "Gualdo Tadino", "101798761", 1095,[], 43.2833, 12.7667);
    public static City AMELIA = new("Amelia", "Amelia", "101799947", 1095,[], 42.5500, 12.4167);
    public static City[] list = [
            ROMA,
            MILANO,
            PERUGIA,
            TERNI,
            GUBBIO,
            FOLIGNO,
            CASTELLO,
            SPOLETO,
            ASSISI,
            CORCIANO,
            BASTIA,
            ORVIETO,
            MARSCIANO,
            NARNI,
            UMBERTIDE,
            TODI,
            CASTIGLIONE,
            MAGIONE,
            GUALDO_T,
            AMELIA

    ];
}