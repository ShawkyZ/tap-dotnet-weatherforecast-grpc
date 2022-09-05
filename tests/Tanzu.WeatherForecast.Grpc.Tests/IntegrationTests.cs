using System.Threading.Channels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
using Moq;
using Tanzu.WeatherForecast.Grpc.Options;
using Tanzu.WeatherForecast.Grpc.Services;
using Tanzu.WeatherForecast.Grpc.Tests.Helpers;

namespace Tanzu.WeatherForecast.Grpc.Tests;

[Trait("Category", "IntegrationTest")]
public class IntegrationTests
{
    private readonly ILogger<WeatherForecastService> _mockedLogger = new Mock<ILogger<WeatherForecastService>>().Object;
    private readonly IOptions<WeatherOptions> _mockedOptions = new Mock<IOptions<WeatherOptions>>().Object;
    private readonly TestServer _testServer;
    
    public IntegrationTests()
    {
        _testServer = new TestServer(WebHost.CreateDefaultBuilder().UseUrls("http://localhost:5001"));

    }
    
    [Fact]
    public async Task Test01_CallGet_ReturnList()
    {
        // Arrange
        var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpClient = _testServer.CreateClient()
        });
        
        //WeatherForecast.Client
        // WeatherForecast.
        // var client = _testServer
        // var summaries = new[]{
        //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        // };
        // var expectedResult = Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
        // {
        //     Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = summaries[index]
        // }).OrderBy(x => x.Date).ToArray();
        //
        // // Act
        // var actualResult = await client.Get(new Empty(), TestServerCallContext.Create());
        //
        // // Assert
        // Assert.NotNull(actualResult);
        // for (var i = 0; i < actualResult.WeatherForecastList.Count; i++)
        // {
        //     var actualWeatherForecast = actualResult.WeatherForecastList.ElementAt(i);
        //     var expectedWeatherForecast = expectedResult[i];
        //     Assert.Equal(actualWeatherForecast.Summary, expectedWeatherForecast.Summary);
        // }
    }
}