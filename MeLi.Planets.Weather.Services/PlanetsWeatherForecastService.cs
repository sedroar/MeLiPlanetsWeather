using MeLi.Planets.Weather.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.Services
{
    public class PlanetsWeatherForecastService
    {
        private readonly Repository<DayWeatherForecast> repository;

        public PlanetsWeatherForecastService(Repository<DayWeatherForecast> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> LoadWeatherForecastPrevisions(int years)
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
                    Weather = (MeLi.Planets.Weather.DataAccess.Weather)WeatherForecastService.DetermineDayWether(ferengiPosition, betasoidePosition, vulcanoPosition)
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

            await repository.InsertMany(planetDayWeatherForecasts);

            return true;
        }

        public async Task<object> GetDayWeatherForecast(int day)
        {
            var dayWeatherForecast = (await repository.Find(dayWeatherForecast => dayWeatherForecast.Day == day)).SingleOrDefault();
            return new { Dia = dayWeatherForecast.Day, Fecha = dayWeatherForecast.Date.ToShortDateString(), Clima = dayWeatherForecast.Weather.ToString(), PicoDeLluvia = dayWeatherForecast.IsMaxTrianglePerimeter } ;
        }
    }
}
