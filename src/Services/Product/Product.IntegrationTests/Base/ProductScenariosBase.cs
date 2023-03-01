using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Product.API;
using Product.API.Extensions;
using Product.API.Infrastructure.Persistence;

using System.Text;
using System.Text.Json;

namespace Product.IntegrationTests.Base;

public class ProductScenariosBase
{
    public static ProductTestServer CreateServer()
    {
        IWebHostBuilder hostBuilder = new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
            })
            .UseStartup<Startup>();

        ProductTestServer testServer = new(hostBuilder);

        testServer.Host.MigrateDbContext<ProductContext>((_, __) => { });

        return testServer;
    }

    public static async Task<T?> GetRequestContent<T>(
        HttpResponseMessage httpResponseMessage)
    {
        JsonSerializerOptions jsonSettings = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        return JsonSerializer.Deserialize<T>(
            await httpResponseMessage.Content.ReadAsStringAsync(),
            jsonSettings);
    }

    public static StringContent BuildRequestContent<T>(T content)
    {
        string serialized = JsonSerializer.Serialize(content);

        return new StringContent(serialized, Encoding.UTF8, "application/json");
    }
}