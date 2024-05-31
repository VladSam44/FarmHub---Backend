namespace HDigital.Models
{
    public class CombinedWeatherData
    {
        public WeatherData Current { get; set; }
        public ForecastWeatherData Forecast { get; set; }
    }
}
