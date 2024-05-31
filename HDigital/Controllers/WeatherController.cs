using HDigital.Helper;
using HDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenWeatherMapOptions _options;

        public WeatherController(IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather(double lat, double lon)
        {
            try
            {
                if (string.IsNullOrEmpty(_options.ApiKey))
                {
                    return StatusCode(500, "API key is not configured.");
                }

                // Current weather endpoint
                var currentWeatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_options.ApiKey}";
                var httpClient = _httpClientFactory.CreateClient();
                var currentWeatherResponse = await httpClient.GetAsync(currentWeatherUrl);

                if (!currentWeatherResponse.IsSuccessStatusCode)
                {
                    var errorContent = await currentWeatherResponse.Content.ReadAsStringAsync();
                    return StatusCode((int)currentWeatherResponse.StatusCode, $"Error fetching current weather data: {errorContent}");
                }

                var currentWeatherContent = await currentWeatherResponse.Content.ReadAsStringAsync();
                var currentWeatherData = JsonSerializer.Deserialize<WeatherData>(currentWeatherContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Forecast weather endpoint (5 day / 3 hour forecast)
                var forecastWeatherUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={_options.ApiKey}";
                var forecastWeatherResponse = await httpClient.GetAsync(forecastWeatherUrl);

                if (!forecastWeatherResponse.IsSuccessStatusCode)
                {
                    var errorContent = await forecastWeatherResponse.Content.ReadAsStringAsync();
                    return StatusCode((int)forecastWeatherResponse.StatusCode, $"Error fetching forecast weather data: {errorContent}");
                }

                var forecastWeatherContent = await forecastWeatherResponse.Content.ReadAsStringAsync();
                var forecastWeatherData = JsonSerializer.Deserialize<ForecastWeatherData>(forecastWeatherContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var combinedWeatherData = new CombinedWeatherData
                {
                    Current = currentWeatherData,
                    Forecast = forecastWeatherData
                };

                return Ok(combinedWeatherData);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Error parsing JSON response: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
