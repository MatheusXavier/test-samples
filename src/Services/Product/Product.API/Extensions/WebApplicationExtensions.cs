using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MigrateDbContext<TContext>(
        this WebApplication app,
        Action<TContext, IServiceProvider> seeder) where TContext : notnull, DbContext
    {
        using IServiceScope scope = app.Services.CreateScope();

        IServiceProvider services = scope.ServiceProvider;
        ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
        TContext context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database used on context {context}", typeof(TContext).Name);

            context.Database.Migrate();
            seeder(context, services);

            logger.LogInformation("Database used on context {context} migrated succesfully", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the datatabase used on context {context}", typeof(TContext).Name);
        }

        return app;
    }
}