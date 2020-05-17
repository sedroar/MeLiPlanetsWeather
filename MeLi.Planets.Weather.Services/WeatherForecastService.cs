using System;

namespace MeLi.Planets.Weather.Services
{
    public static class WeatherForecastService
    {
        public static DataAccess.Weather DetermineDayWether(Point sun, Point planetOne, Point planetTwo, Point planetThree)
        {
            // Determine if planets are in line
            var planetsInLine = GeometricsService.PointsAreInLine(planetOne, planetTwo, planetThree);
            
            if (planetsInLine.PointsAreInLine)
            {
                // If the sun is in line of the planets, there will be a drought
                if (planetsInLine.LineCrossPointZero)
                {
                    return DataAccess.Weather.Drought;
                }

                // If planets are in line, the weather in the system is optimal
                return DataAccess.Weather.Optimal;
            }

            // If the sun is inside the planets triangle, it will be a rainy day
            if (GeometricsService.PointIsInsideTriangle(sun, planetOne, planetTwo, planetThree))
                return DataAccess.Weather.Rain;

            return DataAccess.Weather.Incorrect;
        }
    }
}
