using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Tanzu.WeatherForecast.Grpc.Options;
using Tanzu.WeatherForecast.Grpc.Services;

var port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "8080");

var builder = WebApplication.CreateBuilder(args);

// Setup a HTTP/2 endpoint without TLS for MacOS.
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    builder.WebHost.ConfigureKestrel(options =>
    {
            options.ListenLocalhost(port, o => o.Protocols = HttpProtocols.Http2);
    });

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

// Options configuration
builder.Services.Configure<WeatherOptions>(builder.Configuration.GetSection(WeatherOptions.Weather));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<WeatherForecastService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
