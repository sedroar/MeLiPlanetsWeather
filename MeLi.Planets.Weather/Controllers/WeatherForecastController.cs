using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeLi.Planets.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeLi.Planets.Weather.Controllers
{
    [ApiController]
    [Route("clima")]
    public class WeatherForecastController : ControllerBase
    { 
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PlanetsWeatherForecastService planetsWeatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, PlanetsWeatherForecastService planetsWeatherForecastService)
        {
            _logger = logger;
            this.planetsWeatherForecastService = planetsWeatherForecastService;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            try
            {
                var loadResult = await planetsWeatherForecastService.LoadWeatherForecastPrevisions(10);
                return Ok(loadResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading days weather forecast");
                return BadRequest("Error al cargar la previsión del clima.");
            }
        }

        [HttpGet]        
        public async Task<ActionResult> Get([FromQuery] int? dia)
        {
            try
            {
                if (!dia.HasValue)
                {
                    return BadRequest("No se ha enviado el dia");
                }
                var dayWeatherForecast = await planetsWeatherForecastService.GetDayWeatherForecast(dia.Value);
                return Ok(dayWeatherForecast);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving day weather forecast");
                return BadRequest("Error al obtener la previsión del clima para el día.");
            }
        }
    }
}
