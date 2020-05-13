using System;

namespace MeLi.Planets.Weather.Services
{
    public class WeatherForecastService
    {
        public Weather DetermineDayWether(Point planetOne, Point planetTwo, Point planetThree)
        {
            // Determine if planets are in line
            var planetsInLine = GeometricsService.PointsAreInLine(planetOne, planetTwo, planetThree);
            
            if (planetsInLine.PointsAreInLine)
            {
                // If the sun is in line of the planets, there will be a drought
                if (planetsInLine.LineCrossPointZero)
                {
                    return Weather.Drought;
                }

                // If planets are in line, the weather in the system is optimal
                return Weather.Optimal;
            }

            return Weather.Rain;
        }
    }
}
