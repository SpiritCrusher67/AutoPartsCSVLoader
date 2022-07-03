using AutoPartsCSVLoader.Data;
using AutoPartsCSVLoader.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.Run();

void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration["DbConnection"];
    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    services.AddScoped<IPriceItemRepository, PriceItemRepository>();
}
