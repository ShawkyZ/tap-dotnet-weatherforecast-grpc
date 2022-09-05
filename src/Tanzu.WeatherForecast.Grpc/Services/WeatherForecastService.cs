using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Tanzu.WeatherForecast.Grpc.Options;

namespace Tanzu.WeatherForecast.Grpc.Services;

public class WeatherForecastService : WeatherForecast.WeatherForecastBase
{
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly IOptions<WeatherOptions> _weatherOptions;
    
    public WeatherForecastService(ILogger<WeatherForecastService> logger, IOptions<WeatherOptions> weatherOptions)
    {
        _logger = logger;
        _weatherOptions = weatherOptions;
    }

    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    
    public override Task<WeatherForecastReply> Get(Empty request, ServerCallContext context)
    {
        var weatherForecastReply = new WeatherForecastReply();
        
        var modelList = Enumerable.Range(1, 5).Select(index =>
            {
                var temperatureC = Random.Shared.Next(-20, 55);
                return new WeatherForecastModel
                {
                    Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddDays(index)),
                    TemperatureC = temperatureC,
                    TemperatureF = (int)(temperatureC / 0.5556),
                    Summary = Summaries[index]
                };
            });
        
        weatherForecastReply.WeatherForecastList.AddRange(modelList);

        return Task.FromResult(weatherForecastReply);
    }
}
