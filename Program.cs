using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using System.Net.Http.Headers;

// Initializes a new HostApplicationBuilder. 
// The starting point for configuring a .NET Generic Host.
var builder = Host.CreateEmptyApplicationBuilder(settings: null);

// Tells the .NET injection system to set up an MCP server.
builder.Services.AddMcpServer()
    // Specify communication through starndard i/o.
    .WithStdioServerTransport()
    // Instruct MCP server to find and register any tools
    // defined withing the project code.
    .WithToolsFromAssembly();


builder.Services.AddSingleton(_ =>
{
    // Set up the http address.
    var client = new HttpClient() { BaseAddress = new Uri("https://api.weather.gov") };
    // Set up the UserAgent value.
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
    // Returns the fully configured HttpClient instance.
    return client;
});

// Constructs the actual app and returns the IHost
// which represents the fully configured application.
var app = builder.Build();

// Console.WriteLine("Ok");
await app.RunAsync();

