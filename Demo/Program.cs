using Demo.Model;
using Demo.Model.Interface;
using Demo.Model.Repository;
using EmailItscLib.ITSC.Interface;
using EmailItscLib.ITSC.Repository;
using ITSC_API_GATEWAY_LIB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;

Logger logger = null;
try
{
    logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    logger.Debug("init main");

    var builder = WebApplication.CreateBuilder(args);
    IConfiguration Configuration = builder.Configuration;
    // Add services to the container.
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();
    builder.Services.AddHttpClient();

    if (builder.Environment.IsEnvironment("test"))
    {
        builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseInMemoryDatabase(databaseName: "ApplicationDBContext").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
    }
    else
    {
        //builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("xx")));
        builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseInMemoryDatabase(databaseName: "ApplicationDBContext").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
    }
    //builder.Services.AddCors(options =>
    //{
    //    options.AddPolicy(name: "_myAllowSpecificOrigins",
    //                      builder =>
    //                      {
    //                          builder.WithOrigins("*")
    //                                   .AllowAnyMethod()
    //                                   .AllowAnyHeader();
    //                      });
    //});
    builder.Services.AddScoped<IEmailRepository, EmailRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddITSC(builder.Configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        IWebHostEnvironment env = builder.Environment;
        SetData setData = new SetData(env, dbContext);


    }
    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
public partial class Program
{
}



