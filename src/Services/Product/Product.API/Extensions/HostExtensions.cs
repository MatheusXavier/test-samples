using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions;

public static class HostExtensions
{
    public static IWebHost MigrateDbContext<TContext>(
        this IWebHost webHost,
        Action<TContext, IServiceProvider> seeder)
        where TContext : notnull, DbContext
    {
        using IServiceScope scope = webHost.Services.CreateScope();

        MigrateDbContext(scope, seeder);

        return webHost;
    }

    public static IHost MigrateDbContext<TContext>(
        this IHost host,
        Action<TContext, IServiceProvider> seeder)
        where TContext : notnull, DbContext
    {
        using IServiceScope scope = host.Services.CreateScope();

        MigrateDbContext(scope, seeder);

        return host;
    }

    private static void MigrateDbContext<TContext>(
        IServiceScope scope,
        Action<TContext, IServiceProvider> seeder)
        where TContext : notnull, DbContext
    {
        IServiceProvider services = scope.ServiceProvider;
        ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
        TContext context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation(
                "Migrating database used on context {context}",
                typeof(TContext).Name);

            context.Database.Migrate();
            seeder(context, services);

            logger.LogInformation(
                "Database used on context {context} migrated succesfully",
                typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while migrating the datatabase used on context {context}",
                typeof(TContext).Name);
        }
    }
}