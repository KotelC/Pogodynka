using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using PogodynkaCzaja.Modele;

namespace PogodynkaCzaja
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string ApiKey = "27f1b8eae9dcb629ebe304e41bb1c34a";
        private readonly WeatherService _weatherService;
        private WeatherData weatherData;

        public event PropertyChangedEventHandler PropertyChanged;

        public string WeatherInfo => $"{weatherData?.Name} Weather";
        public string Temperature => $"Temperature: {weatherData?.Main?.Temp}°C";
        public string Humidity => $"Humidity: {weatherData?.Main?.Humidity}%";
        public string Wind => $"Wind: {weatherData?.Wind?.Speed} m/s, {GetWindDirection(weatherData?.Wind?.Deg)}";
        public string Sunrise => $"Sunrise: {GetLocalTime(weatherData?.Sys?.Sunrise ?? 0)}";
        public string Sunset => $"Sunset: {GetLocalTime(weatherData?.Sys?.Sunset ?? 0)}";
        public string CurrentTime => $"Current time: {GetLocalTime(DateTimeOffset.Now.ToUnixTimeSeconds())}";

        public MainWindow()
        {
            InitializeComponent();
            _weatherService = new WeatherService(ApiKey);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locationTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(location))
            {
                try
                {
                    weatherData = await _weatherService.GetCurrentWeatherAsync(location);
                    UpdateWeatherInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateWeatherInfo()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WeatherInfo)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Humidity)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wind)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset)));
        }


        private string GetWindDirection(int? deg)
        {
            if (deg.HasValue)
            {
                Dictionary<string, (double, double)> windDirections = new Dictionary<string, (double, double)>
                {
                    { "N", (337.5, 22.5) },
                    { "NE", (22.5, 67.5) },
                    { "E", (67.5, 112.5) },
                    { "SE", (112.5, 157.5) },
                    { "S", (157.5, 202.5) },
                    { "SW", (202.5, 247.5) },
                    { "W", (247.5, 292.5) },
                    { "NW", (292.5, 337.5) }
                };

                foreach (var direction in windDirections)
                {
                    if (deg >= direction.Value.Item1 && deg < direction.Value.Item2)
                    {
                        return direction.Key;
                    }
                }

                throw new ArgumentException("Invalid wind direction degree.");
            }
            else
            {
                return "Unknown";
            }
        }

        // Method to convert UNIX timestamp to local time
        private string GetLocalTime(long unixTime)
        {
            try
            {
                DateTime localTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                return localTime.ToString("HH:mm:ss");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid UNIX timestamp: {ex.Message}");
                return "Invalid time";
            }
        }

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
