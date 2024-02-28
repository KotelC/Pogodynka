using System;

namespace PogodynkaCzaja.Modele
{
    public class WeatherData
    {
        public string Name { get; set; }
        public MainData Main { get; set; }
        public WindData Wind { get; set; }
        public SysData Sys { get; set; }
    }

    public class MainData
    {
        public float Temp { get; set; }
        public int Humidity { get; set; }
    }

    public class WindData
    {
        public float Speed { get; set; }
        public int Deg { get; set; }
    }

    public class SysData
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }
}
