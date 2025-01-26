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
    // Set the log level for EF Core commands to a higher level (e.g., Warning, Error, etc.)
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
    // Exclude EF Core related warnings
    //.Filter.ByExcluding(logEvent =>
    //    logEvent.MessageTemplate.Text.Contains("RowLimitingOperationWithoutOrderByWarning") ||
    //    logEvent.MessageTemplate.Text.Contains("The query uses the 'First'/'FirstOrDefault' operator without 'OrderBy' and filter operators")
    //)
    // Optionally exclude all EF Core logs
    //.Filter.ByExcluding(logEvent =>
    //    logEvent.Properties.ContainsKey("SourceContext") &&
    //    logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
    //)
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
    services.AddTransient<NewBatchId>();
    services.AddTransient<EasyHealth>();
    services.AddHostedService<MyWorker>();
});
var host = builder.Build();
builder.ConfigureServices((context, services) =>
{
    services.AddLogging(configure => configure.AddConsole());
    services.AddHostedService<MyWorker>();
    services.AddTransient<Program>();
    services.AddSingleton<NewBatchId>();
    services.AddSingleton<EasyHealth>();
});
Console.WriteLine("Schedular is Started!");
Console.WriteLine("Premium Validation Schedular Started!");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var serviceProvider = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<NewBatchId>()
    .BuildServiceProvider();

var serviceProviderEH = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<EasyHealth>()
      .BuildServiceProvider();

//var dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<HDFCDbContext>>();
var fetchbatchid = serviceProvider.GetService<NewBatchId>();
var easyHealth = serviceProviderEH.GetService<EasyHealth>();
string postgresConnectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
using (var postgresConnection = new NpgsqlConnection(postgresConnectionString))
{
    try
    {
        postgresConnection.Open();
        //using (var transaction = postgresConnection.BeginTransaction())
        //{
        try
        {
            List<string> idPlaceholders = new List<string>();
            var listofpolicies = fetchbatchid.FetchNewBatchIds(postgresConnection);
            Console.WriteLine(listofpolicies.Count);

            if (listofpolicies.Count > 0)
            {
                foreach (var item in listofpolicies)
                {
                    var tasks = Enumerable.Range(0, 10).Select(async i =>
                    {
                        using (var scope = host.Services.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<HDFCDbContext>();
                            var baserates = await easyHealth.GetBaseRatesAsync(dbContext);//easyhealth_baserates
                            var cirates = await easyHealth.GetCiRatesAsync(dbContext);//easyhealth_cirates
                            var hdcrates = await easyHealth.GetHdcRatesAsync(dbContext);//easyhealth_hdcrates
                            var carates = await easyHealth.GetCARatesAsync(dbContext);//easyhealth_carates

                            string certificateNo = item[0];  // First item is certificate_no
                            string productCode = item[1];
                            // Resolve a new instance of DbContext for each task
                            var ehRNEData = await easyHealth.GetGCEasyHealthDataAsync(certificateNo, dbContext);
                            if (productCode == "2806")
                            {
                                await easyHealth.GetEasyHealthValidation(ehRNEData, certificateNo, dbContext,baserates, hdcrates, carates, cirates);
                            }
                            else
                            {
                                Console.WriteLine($"No validation found for value: {certificateNo}");
                            }

                        }
                    }).ToList();

                    await System.Threading.Tasks.Task.WhenAll(tasks);

                }
                Console.Write("Upsell calculation started");
                //foreach (var item in listofpolicies)
                //{
                //    var tasks = Enumerable.Range(0, 10).Select(async i =>
                //    {
                //        using (var scope = host.Services.CreateScope())
                //        {
                //            var dbContext = scope.ServiceProvider.GetRequiredService<HDFCDbContext>();
                //            string certificateNo = item[0];  // First item is certificate_no
                //            string productCode = item[1];
                //            if (productCode == "2856")
                //            {
                //                var GCData = await optimaSecure.GetGCOptimaSecureDataAsync(certificateNo, dbContext);
                //                IEnumerable<OptimaSecureRNE> OptimaSecureValidationResultUpSell = Enumerable.Empty<OptimaSecureRNE>();
                //                foreach (var row1 in GCData)
                //                {
                //                    if (row1.upselltype1 == "SI_UPSELL" || row1.upselltype2 == "SI_UPSELL" || row1.upselltype3 == "SI_UPSELL" || row1.upselltype4 == "SI_UPSELL" || row1.upselltype5 == "SI_UPSELL" || row1.upselltype1 == "UPSELLBASESI_1" || row1.upselltype2 == "UPSELLBASESI_1" || row1.upselltype3 == "UPSELLBASESI_1" || row1.upselltype4 == "UPSELLBASESI_1" || row1.upselltype5 == "UPSELLBASESI_1")
                //                    {
                //                        OptimaSecureValidationResultUpSell = await optimaSecure.CalculateOptimaSecurePremiumqUpsell(GCData, certificateNo, dbContext);
                //                        await optimaSecure.UpdateRnGenerationStatus(OptimaSecureValidationResultUpSell, dbContext);

                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //    }).ToList();
                //    await System.Threading.Tasks.Task.WhenAll(tasks);
                //}      //transaction.Commit();

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




