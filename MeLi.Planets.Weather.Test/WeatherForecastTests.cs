using MeLi.Planets.Weather.Services;
using NUnit.Framework;

namespace MeLi.Planets.Weather.Test
{
    public class WeatherForecastTests
    {
        private Point sun = new Point { X = 0, Y = 0 };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WeatherServiceTests()
        {
            // Planets are in line, and the sun IS in line with the planets.
            // So, the system will have a drought

            var weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -3, Y = -3 }, new Point { X = 1, Y = 1 }, new Point { X = -7, Y = -7 });

            Assert.AreEqual(DataAccess.Weather.Drought, weather);

            // Planets are in line, and the sun IS in line with the planets.
            // So, the system will have a drought

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -2000, Y = -2000 }, new Point { X = 500, Y = 500 }, new Point { X = 1000, Y = 1000 });

            Assert.AreEqual(DataAccess.Weather.Drought, weather);


            // Planets are in line, but sun is not in the line. So, the weather will be Optimal

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -3, Y = -3 }, new Point { X = 1, Y = -3 }, new Point { X = -7, Y = -3 });

            Assert.AreEqual(DataAccess.Weather.Optimal, weather);

            // Planets are in line, but sun is not in the line. So, the weather will be Optimal

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -3, Y = -3 }, new Point { X = -3, Y = 5 }, new Point { X = -3, Y = 9 });

            Assert.AreEqual(DataAccess.Weather.Optimal, weather);

            // Planets are in line, but sun is not in the line. So, the weather will be Optimal

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -2000, Y = -2500 }, new Point { X = -2000, Y = 500 }, new Point { X = -2000, Y = 1000 });

            Assert.AreEqual(DataAccess.Weather.Optimal, weather);

            // Planets are not in line. And the sun is in the triangle.
            // So, the weather will be rain.

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -2000, Y = -2000 }, new Point { X = 100, Y = 1000 }, new Point { X = 150, Y = -500 });

            Assert.AreEqual(DataAccess.Weather.Rain, weather);

            // Planets are not in line. And the sun is in the triangle.
            // So, the weather will be rain.

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -2000, Y = 0 }, new Point { X = 100, Y = 1000 }, new Point { X = 150, Y = -500 });

            Assert.AreEqual(DataAccess.Weather.Rain, weather);

            // Planets are not in line. And the sun is not in the triangle.
            // So, the weather will be good.

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -1000, Y = -2000 }, new Point { X = 1000, Y = 2000 }, new Point { X = 5000, Y = -500 });

            Assert.AreEqual(DataAccess.Weather.Incorrect, weather);


            // Planets are not in line. And the sun is in the triangle.
            // So, the weather will be rain.

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -1000, Y = -2000 }, new Point { X = 1000, Y = 4000 }, new Point { X = 5000, Y = -500 });

            Assert.AreEqual(DataAccess.Weather.Rain, weather);

            // Planets are not in line. And the sun is in the triangle.
            // So, the weather will be rain.

            weather = WeatherForecastService.DetermineDayWether(sun, new Point { X = -1000, Y = -2000 }, new Point { X = 1000, Y = 4000 }, new Point { X = 5000, Y = -500 });

            Assert.AreEqual(DataAccess.Weather.Rain, weather);
        }
    }
}