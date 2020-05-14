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
        Óptimo = 1,
        [Description("Sequía")]
        Sequía = 2,
        [Description("Lluvia")]
        Lluvia = 4
    }
}
