using HERGPremiumValidationSchedular.BussinessLogic;
using HERGPremiumValidationSchedular.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Hosting;
using HERGPremiumValidationSchedular;
using Serilog.Filters;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog.Events;
using HERGPremiumValidationSchedular.Models.Domain;
using DocumentFormat.OpenXml.InkML;
using System.Collections;

var builder = Host.CreateDefaultBuilder(args).ConfigureLogging((context, logging) =>
{    
    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
});
string logFilePath = @"C:\temp\EasyHealthPremiumValidationSchedular\EasyHealthPremiumValidationSchedular_log.txt";
Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
Log.Information("Application has started.");
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
   .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Hour,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
      .Filter.ByExcluding(logEvent =>
        logEvent.Properties.ContainsKey("SourceContext") &&
        logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command") &&
        logEvent.Level == Serilog.Events.LogEventLevel.Information &&
        logEvent.MessageTemplate.Text.Contains("Executed DbCommand")  // Exclude logs that contain 'Executed DbCommand'
    )
  
    .CreateLogger();
string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"]?.ConnectionString;
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing from app.config");
    return;
}
builder.ConfigureServices((context, services) =>
{
    services.AddLogging(configure => configure.AddSerilog());
    services.AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString));
    services.AddTransient<EasyHealth>();
    services.AddHostedService<MyWorker>();
});
var host = builder.Build();
builder.ConfigureServices((context, services) =>
{
    services.AddLogging(configure => configure.AddConsole());
    services.AddHostedService<MyWorker>();
    services.AddTransient<Program>();
    services.AddSingleton<EasyHealth>();
});
Console.WriteLine("Schedular is Started!");
Console.WriteLine("Premium Validation Schedular Started!");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var serviceProvider = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<EasyHealth>()
      .BuildServiceProvider();
var easyHealth = serviceProvider.GetService<EasyHealth>();
string postgresConnectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
using (var postgresConnection = new NpgsqlConnection(postgresConnectionString))
{
    try
    {
        postgresConnection.Open();
        try
        {
            List<string> idPlaceholders = new List<string>();
            var listofpolicies = easyHealth.FetchNewBatchIds(postgresConnection);
            Console.Write("Listofpolicies" , listofpolicies.Count);
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HDFCDbContext>();
                var baserates = await easyHealth.GetBaseRatesAsync(dbContext);
                var cirates = await easyHealth.GetCiRatesAsync(dbContext);
                var hdcrates = await easyHealth.GetHdcRatesAsync(dbContext);
                var carates = await easyHealth.GetCARatesAsync(dbContext);
                if (listofpolicies.Count > 0)
                {
                    var tasks = new List<System.Threading.Tasks.Task>();
                    {
                        var semaphore = new SemaphoreSlim(10);
                        foreach (var item in listofpolicies)
                        {
                            var task = System.Threading.Tasks.Task.Run(async () =>
                            {
                                await semaphore.WaitAsync();
                                try
                                {
                                    string certificateNo = item[0];
                                    string productCode = item[1];
                                    var ehRNEData = await easyHealth.GetGCEasyHealthDataAsync(certificateNo);
                                    if (ehRNEData != null && ehRNEData.Any())
                                    {
                                        await easyHealth.GetEasyHealthValidation(ehRNEData, certificateNo, baserates, hdcrates, carates, cirates);
                                     }
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            });
                            tasks.Add(task);
                        }
                        await System.Threading.Tasks.Task.WhenAll(tasks);

                    }
                }
            }
        }

        catch (Exception ex)
        {
            //transaction.Rollback();
            Log.Error(ex, "An error occurred while processing calculating premium.");
            Console.WriteLine("Error occurred: " + ex.Message);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while processing the application.");
        Console.WriteLine("Error occurred: " + ex.Message);
    }
}
Console.WriteLine("Schedular is Completed!");
Log.Information("Application has finished processing.");
//EmailService.SendEmail();
Log.CloseAndFlush();




