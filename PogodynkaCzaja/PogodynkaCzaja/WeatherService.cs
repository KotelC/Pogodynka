using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PogodynkaCzaja.Modele;

namespace PogodynkaCzaja
{
    public class WeatherService
    {
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/";
        private readonly string _apiKey;

        public WeatherService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<WeatherData> GetCurrentWeatherAsync(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{BaseUrl}weather?q={city}&appid={_apiKey}&units=metric");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                    return weatherData;
                }
                else
                {
                    throw new HttpRequestException($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
        }
    }
}
