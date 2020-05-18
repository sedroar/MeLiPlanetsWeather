using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeLi.Planets.Weather.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeLi.Planets.Weather.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly PlanetsWeatherForecastService planetsWeatherForecastService;

        public Worker(ILogger<Worker> logger, PlanetsWeatherForecastService planetsWeatherForecastService)
        {
            _logger = logger;
            this.planetsWeatherForecastService = planetsWeatherForecastService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                var loadResult = await planetsWeatherForecastService.LoadWeatherForecastPrevisions(10);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading days weather forecast");
            }
        }
    }
}
