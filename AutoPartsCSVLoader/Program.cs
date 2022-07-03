using AutoPartsCSVLoader.Data;
using AutoPartsCSVLoader.Data.Configuration;
using AutoPartsCSVLoader.Interfaces;
using AutoPartsCSVLoader.Services;
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
    }, ServiceLifetime.Singleton);

    services.AddTransient<IPriceItemRepository, PriceItemRepository>();

    var emailsData = new SuppliersEmailData();
    configuration.Bind("SuppliersEmails", emailsData.Values);
    services.AddSingleton(emailsData);

    services.AddTransient<IMailInboxService, MailInboxService>();
    services.AddTransient<IPriceItemDataDbLoader, PriceItemDataDbLoader>();
    services.AddSingleton<IInboxMessagesService, InboxMessagesService>();
    services.AddHostedService<InboxCheckingService>();
}
