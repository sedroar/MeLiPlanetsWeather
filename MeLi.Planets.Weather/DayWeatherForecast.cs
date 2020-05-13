using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather
{
    public class PlanetWeatherForecast
    {
        public string Planet { get; set; }
        public DayWeatherForecast DayWeatherForecast { get; set; }
    }

    public class DayWeatherForecast
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public Weather Weather { get; set; }
        public int TrianglePerimeter { get; set; }
    }

    public enum Weather
    {
        Optimal = 1,
        Drought = 2,
        Rain = 4
    }
}
