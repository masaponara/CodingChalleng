using CodingChallenge_Nextech.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallenge_Nextech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStoriesService _storiesService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IStoriesService storiesService)
        {
            _logger = logger;
            _storiesService = storiesService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            try
            {
                var test = await _storiesService.GetStoryById(23);

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = test?.Url ?? string.Empty,
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get data");
                throw;
            }
        }
    }
}