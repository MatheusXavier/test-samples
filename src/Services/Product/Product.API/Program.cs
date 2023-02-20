using Product.API;
using Product.API.Extensions;
using Product.API.Infrastructure.Persistence;

IConfiguration configuration = GetConfiguration();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder
            .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
            .UseStartup<Startup>()
            .UseContentRoot(Directory.GetCurrentDirectory());
    })
    .Build();

host.MigrateDbContext<ProductContext>((_, __) => { });

await host.RunAsync();

static IConfiguration GetConfiguration()
{
    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}
