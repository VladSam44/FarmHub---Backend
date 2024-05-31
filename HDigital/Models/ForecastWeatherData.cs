namespace HDigital.Models
{
    public class ForecastWeatherData
    {
        public string Cod { get; set; }
        public double Message { get; set; }
        public int Cnt { get; set; }
        public List<Forecast> List { get; set; }
        public City City { get; set; }
    }

    public class Forecast
    {
        public long Dt { get; set; }
        public MainData Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public WindData Wind { get; set; }
        public SysPod Sys { get; set; }
        public string DtTxt { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class SysPod
    {
        public string Pod { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
    }

}
