using Microsoft.EntityFrameworkCore;

using Product.API.Extensions;
using Product.API.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<ProductContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionString"]);
    });

WebApplication app = builder.Build();

app.MigrateDbContext<ProductContext>((_, __) => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.MapControllers();

await app.RunAsync();
