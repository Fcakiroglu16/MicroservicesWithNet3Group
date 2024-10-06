using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQExample.API.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //rabbitmq

            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("");

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ConfirmSelect();
            channel.QueueDeclare("WeatherForecast", true, false, false, null);


            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish("", "", false, null, null);

            channel.WaitForConfirms(TimeSpan.FromSeconds(60));


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}