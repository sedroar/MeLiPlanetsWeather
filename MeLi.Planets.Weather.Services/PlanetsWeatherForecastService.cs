using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeLi.Planets.Weather.Services
{
    public class PlanetsWeatherForecastService
    {
        private readonly WeatherForecastService weatherForecastService;

        public PlanetsWeatherForecastService(WeatherForecastService weatherForecastService)
        {
            this.weatherForecastService = weatherForecastService;
        }

        public IEnumerable<DayWeatherForecast> GetWeatherForecastPrevisions(int years)
        {
            var now = DateTime.Now;
            var lastDate = now.AddYears(years);
            var timeSpan = lastDate.Subtract(now);

            var totalDays = Math.Floor(timeSpan.TotalDays);

            int dayOfMaximumTrianglePerimeter = 0;
            double maximumTrianglePerimeter = 0;

            List<DayWeatherForecast> planetDayWeatherForecasts = new List<DayWeatherForecast>();

            for (int i = 1; i <= totalDays; i++)
            {
                double ferengiAngle = i;
                var ferengiPosition = GeometricsService.GetPointInCircleCoordinates(500, ferengiAngle);

                double betasoideAngle = 3 * i;
                var betasoidePosition = GeometricsService.GetPointInCircleCoordinates(2000, betasoideAngle);

                double vulcanoAngle = -5 * i;
                var vulcanoPosition = GeometricsService.GetPointInCircleCoordinates(1000, vulcanoAngle);

                planetDayWeatherForecasts.Add(new DayWeatherForecast
                {
                    Date = now.AddDays(i),
                    Day = i,
                    Weather = weatherForecastService.DetermineDayWether(ferengiPosition, betasoidePosition, vulcanoPosition)
                });

                var trianglePerimeter = GeometricsService.CalculateTrianglePerimeter(ferengiPosition, betasoidePosition, vulcanoPosition);

                if (trianglePerimeter > maximumTrianglePerimeter)
                {
                    maximumTrianglePerimeter = trianglePerimeter;
                    dayOfMaximumTrianglePerimeter = i;
                }
            }

            var rainPeakDay = planetDayWeatherForecasts.Single(forecast => forecast.Day == dayOfMaximumTrianglePerimeter);
            rainPeakDay.IsMaxTrianglePerimeter = true;

            return planetDayWeatherForecasts;
        }
    }
}
