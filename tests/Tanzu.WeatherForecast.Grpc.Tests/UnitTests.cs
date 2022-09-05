using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Moq;
using Tanzu.WeatherForecast.Grpc.Options;
using Tanzu.WeatherForecast.Grpc.Services;
using Tanzu.WeatherForecast.Grpc.Tests.Helpers;

namespace Tanzu.WeatherForecast.Grpc.Tests;

[Trait("Category", "SmokeTest")]
public class UnitTests
{
    private readonly ILogger<WeatherForecastService> _mockedLogger = new Mock<ILogger<WeatherForecastService>>().Object;
    private readonly IOptions<WeatherOptions> _mockedOptions = new Mock<IOptions<WeatherOptions>>().Object;

    [Fact]
    public async Task Test_CallGet_ReturnList()
    {
        // Arrange
        var weatherForecastService = new WeatherForecastService(_mockedLogger, _mockedOptions);
        var summaries = new[]{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        var expectedResult = Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
        {
            Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[index]
        }).OrderBy(x => x.Date).ToArray();
        
        // Act
        var actualResult = await weatherForecastService.Get(new Empty(), TestServerCallContext.Create());

        // Assert
        Assert.NotNull(actualResult);
        for (var i = 0; i < actualResult.WeatherForecastList.Count; i++)
        {
            var actualWeatherForecast = actualResult.WeatherForecastList.ElementAt(i);
            var expectedWeatherForecast = expectedResult[i];
            Assert.Equal(actualWeatherForecast.Summary, expectedWeatherForecast.Summary);
        }
    }
}