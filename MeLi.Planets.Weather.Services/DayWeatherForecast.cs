using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.Services
{
    public class DayWeatherForecast
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public Weather Weather { get; set; }
        public bool IsMaxTrianglePerimeter { get; set; }
    }

    public enum Weather
    {
        Optimal = 1,
        Drought = 2,
        Rain = 4
    }
}
