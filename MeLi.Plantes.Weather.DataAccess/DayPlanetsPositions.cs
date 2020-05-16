using System;
using System.Collections.Generic;
using System.Text;

namespace MeLi.Planets.Weather.DataAccess
{
    public class DayPlanetsPositions : BaseEntity
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public Point Ferengi { get; set; }
        public Point Betasoide { get; set; }
        public Point Vulcano { get; set; }
        public bool IsTriangle { get; set; }
        public bool SunIsInsideTriangle { get; set; }
        public bool IsLine { get; set; }
        public bool SunIsInLine { get; set; }
    }
}
