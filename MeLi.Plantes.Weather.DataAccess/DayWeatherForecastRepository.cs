using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.DataAccess
{
    public class DayWeatherForecastRepository : Repository<DayWeatherForecast>
    {
        public DayWeatherForecastRepository(IConfiguration configuration, IMongoClient mongoClient) : base(configuration, mongoClient)
        {
            
        }

        public async Task DeleteAllDocumentsFromCollection()
        {
            var emptyFilter = Builders<DayWeatherForecast>.Filter.Empty;
            await collection.DeleteManyAsync(emptyFilter);
        }

        public async Task<object> GetWeatherPeriods(Weather weather)
        {
            var weatherDays = await Find(day => day.Weather == weather);

            var periodsGroups = weatherDays.OrderBy(rd => rd.Date)
                            .Select((d, index) => new { Day = d, Index = index })
                            .GroupBy(di => new { GroupDate = di.Day.Date.AddDays(-di.Index) });

            var periods = periodsGroups
                            .Select(g => new
                            {
                                StartDate = g.Min(d => d.Day.Date),
                                EndDate = g.Max(d => d.Day.Date)
                            })
                            .ToList();

            return periods;
        }
    }
}
