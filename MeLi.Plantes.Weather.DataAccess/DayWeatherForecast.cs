using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.DataAccess
{
    public class DayWeatherForecast : BaseEntity
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
