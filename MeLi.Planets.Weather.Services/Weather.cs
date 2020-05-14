using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MeLi.Planets.Weather.Services
{
    public enum Weather
    {
        [Description("Óptimo")]
        Optimal = 1,
        [Description("Sequía")]
        Drought = 2,
        [Description("Lluvia")]
        Rain = 4
    }
}
