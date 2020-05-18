using MeLi.Planets.Weather.DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.Services
{
    public class PlanetsWeatherForecastService
    {
        private readonly DayWeatherForecastRepository dayWeatherForecastRepository;
        private readonly Repository<DayPlanetsPositions> dayPlanetsPositionsRepository;

        public PlanetsWeatherForecastService(DayWeatherForecastRepository dayWeatherForecastRepository, Repository<DayPlanetsPositions> dayPlanetsPositionsRepository)
        {
            this.dayWeatherForecastRepository = dayWeatherForecastRepository;
            this.dayPlanetsPositionsRepository = dayPlanetsPositionsRepository;
        }

        public async Task<bool> LoadWeatherForecastPrevisions(int years)
        {
            var now = DateTime.Now;
            var lastDate = now.AddYears(years);
            var timeSpan = lastDate.Subtract(now);

            var totalDays = Math.Floor(timeSpan.TotalDays);

            int dayOfMaximumTrianglePerimeter = 0;
            double maximumTrianglePerimeter = 0;

            Point sun = new Point { X = 0, Y = 0 };

            ConcurrentBag<DayWeatherForecast> planetDayWeatherForecasts = new ConcurrentBag<DayWeatherForecast>();
            ConcurrentBag<DayPlanetsPositions> planetDayPositions = new ConcurrentBag<DayPlanetsPositions>();

            List<Task> weatherForecastTasks = new List<Task>();

            Parallel.For(1, Convert.ToInt32(totalDays + 1), (i) =>
                {
                    double ferengiAngle = i;
                    var ferengiPosition = GeometricsService.GetPointInCircleCoordinates(500, ferengiAngle);

                    double betasoideAngle = 3 * i;
                    var betasoidePosition = GeometricsService.GetPointInCircleCoordinates(2000, betasoideAngle);

                    double vulcanoAngle = -5 * i;
                    var vulcanoPosition = GeometricsService.GetPointInCircleCoordinates(1000, vulcanoAngle);

                    var date = now.AddDays(i);

                    var isLine = GeometricsService.PointsAreInLine(ferengiPosition, betasoidePosition, vulcanoPosition);
                    var sunIsInsideTriangle = GeometricsService.PointIsInsideTriangle(sun, ferengiPosition, betasoidePosition, vulcanoPosition);

                    planetDayPositions.Add(new DayPlanetsPositions
                    {
                        Day = i,
                        Date = date,
                        Ferengi = new DataAccess.Point { X = ferengiPosition.X, Y = ferengiPosition.Y },
                        Betasoide = new DataAccess.Point { X = betasoidePosition.X, Y = betasoidePosition.Y },
                        Vulcano = new DataAccess.Point { X = vulcanoPosition.X, Y = vulcanoPosition.Y },
                        IsLine = isLine.PointsAreInLine,
                        SunIsInLine = isLine.LineCrossPointZero,
                        SunIsInsideTriangle = sunIsInsideTriangle
                    });

                    planetDayWeatherForecasts.Add(new DayWeatherForecast
                    {
                        Date = date.Date,
                        Day = i,
                        Weather = WeatherForecastService.DetermineDayWether(sun, ferengiPosition, betasoidePosition, vulcanoPosition)
                    });

                    var trianglePerimeter = GeometricsService.CalculateTrianglePerimeter(ferengiPosition, betasoidePosition, vulcanoPosition);

                    if (trianglePerimeter > maximumTrianglePerimeter)
                    {
                        maximumTrianglePerimeter = trianglePerimeter;
                        dayOfMaximumTrianglePerimeter = i;
                    }
                });

            var rainPeakDay = planetDayWeatherForecasts.Single(forecast => forecast.Day == dayOfMaximumTrianglePerimeter);
            rainPeakDay.IsMaxTrianglePerimeter = true;

            await dayWeatherForecastRepository.DeleteAllDocumentsFromCollection();

            await dayWeatherForecastRepository.InsertMany(planetDayWeatherForecasts.OrderBy(p => p.Date));

            return true;
        }

        public async Task<object> GetDayWeatherForecast(int day)
        {
            var dayWeatherForecast = (await dayWeatherForecastRepository.Find(dayWeatherForecast => dayWeatherForecast.Day == day)).SingleOrDefault();
            return new { Dia = dayWeatherForecast.Day, Fecha = dayWeatherForecast.Date.ToShortDateString(), Clima = ((Weather)dayWeatherForecast.Weather).ToString(), PicoDeLluvia = dayWeatherForecast.IsMaxTrianglePerimeter };
        }

        public async Task<object> GetWeatherPeriods(Weather weather)
        {
            return (await dayWeatherForecastRepository.GetWeatherPeriods((DataAccess.Weather)weather));
        }
    }
}
