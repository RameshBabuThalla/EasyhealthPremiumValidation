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
using EasyhealthPremiumValidation.Models.Domain;
using DocumentFormat.OpenXml.InkML;
using System.Collections;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using Dapper;

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
            Console.Write("Listofpolicies"  + " " +listofpolicies.Count);
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
                                    IEnumerable<EasyHealthRNE> easyhealthRNEData = Enumerable.Empty<EasyHealthRNE>();
                                    string certificateNo = item[0];
                                    string productCode = item[1];
                                    var ehRNEData = await easyHealth.GetGCEasyHealthDataAsync(certificateNo);
                                    if (ehRNEData != null && ehRNEData.Any())
                                    {
                                        foreach (var row in ehRNEData)
                                        {
                                            if (row.upselltype1 == null || row.upselltype2 == null || row.upselltype3 == null || row.upselltype4 == null || row.upselltype5 == null || row.upselltype1 == string.Empty || row.upselltype2 == string.Empty || row.upselltype3 == string.Empty || row.upselltype4 == string.Empty || row.upselltype5 == string.Empty)
                                            {
                                                easyhealthRNEData = await easyHealth.GetEasyHealthValidation(ehRNEData, certificateNo, baserates, hdcrates, carates, cirates);
                                                EasyHealthPremiumValidation objEasyhealthPremiumValidation = new EasyHealthPremiumValidation
                                                {
                                                    policy_number = ehRNEData.FirstOrDefault()?.policy_number,
                                                    prod_code = ehRNEData.FirstOrDefault()?.prod_code,
                                                    prod_name = ehRNEData.FirstOrDefault()?.prod_name,
                                                    reference_num = ehRNEData.FirstOrDefault()?.reference_num,
                                                    policy_start_date = ehRNEData.FirstOrDefault()?.policy_start_date,
                                                    policy_expiry_date = ehRNEData.FirstOrDefault()?.policy_expiry_date,
                                                    policyplan = ehRNEData.FirstOrDefault()?.policyplan,
                                                    policy_type = ehRNEData.FirstOrDefault()?.policy_type,
                                                    policy_period = ehRNEData.FirstOrDefault()?.policy_period,
                                                    no_of_members = ehRNEData.FirstOrDefault()?.no_of_members,
                                                    eldest_member = ehRNEData.FirstOrDefault()?.eldest_member,
                                                    tier_type = ehRNEData.FirstOrDefault()?.tier_type,
                                                    txt_insured_dob1 = ehRNEData.FirstOrDefault()?.txt_insured_dob1,
                                                    txt_insured_dob2 = ehRNEData.FirstOrDefault()?.txt_insured_dob2,
                                                    txt_insured_dob3 = ehRNEData.FirstOrDefault()?.txt_insured_dob3,
                                                    txt_insured_dob4 = ehRNEData.FirstOrDefault()?.txt_insured_dob4,
                                                    txt_insured_dob5 = ehRNEData.FirstOrDefault()?.txt_insured_dob5,
                                                    txt_insured_dob6 = ehRNEData.FirstOrDefault()?.txt_insured_dob6,
                                                    txt_insured_age1 = ehRNEData.FirstOrDefault()?.txt_insured_age1,
                                                    txt_insured_age2 = ehRNEData.FirstOrDefault()?.txt_insured_age2,
                                                    txt_insured_age3 = ehRNEData.FirstOrDefault()?.txt_insured_age3,
                                                    txt_insured_age4 = ehRNEData.FirstOrDefault()?.txt_insured_age4,
                                                    txt_insured_age5 = ehRNEData.FirstOrDefault()?.txt_insured_age5,
                                                    txt_insured_age6 = ehRNEData.FirstOrDefault()?.txt_insured_age6,
                                                    txt_insured_relation1 = ehRNEData.FirstOrDefault()?.txt_insured_relation1,
                                                    txt_insured_relation2 = ehRNEData.FirstOrDefault()?.txt_insured_relation2,
                                                    txt_insured_relation3 = ehRNEData.FirstOrDefault()?.txt_insured_relation3,
                                                    txt_insured_relation4 = ehRNEData.FirstOrDefault()?.txt_insured_relation4,
                                                    txt_insured_relation5 = ehRNEData.FirstOrDefault()?.txt_insured_relation5,
                                                    txt_insured_relation6 = ehRNEData.FirstOrDefault()?.txt_insured_relation6,
                                                    insured_cb1 = ehRNEData.FirstOrDefault()?.insured_cb1,
                                                    insured_cb2 = ehRNEData.FirstOrDefault()?.insured_cb2,
                                                    insured_cb3 = ehRNEData.FirstOrDefault()?.insured_cb3,
                                                    insured_cb4 = ehRNEData.FirstOrDefault()?.insured_cb4,
                                                    insured_cb5 = ehRNEData.FirstOrDefault()?.insured_cb5,
                                                    insured_cb6 = ehRNEData.FirstOrDefault()?.insured_cb6,
                                                    sum_insured1 = ehRNEData.FirstOrDefault()?.sum_insured1,
                                                    sum_insured2 = ehRNEData.FirstOrDefault()?.sum_insured2,
                                                    sum_insured3 = ehRNEData.FirstOrDefault()?.sum_insured3,
                                                    sum_insured4 = ehRNEData.FirstOrDefault()?.sum_insured4,
                                                    sum_insured5 = ehRNEData.FirstOrDefault()?.sum_insured5,
                                                    sum_insured6 = ehRNEData.FirstOrDefault()?.sum_insured6,
                                                    upselltype1 = ehRNEData.FirstOrDefault()?.upselltype1,
                                                    upselltype2 = ehRNEData.FirstOrDefault()?.upselltype2,
                                                    upselltype3 = ehRNEData.FirstOrDefault()?.upselltype3,
                                                    upselltype4 = ehRNEData.FirstOrDefault()?.upselltype4,
                                                    upselltype5 = ehRNEData.FirstOrDefault()?.upselltype5,
                                                    upsellvalue1 = ehRNEData.FirstOrDefault()?.upsellvalue1,
                                                    upsellvalue2 = ehRNEData.FirstOrDefault()?.upsellvalue2,
                                                    upsellvalue3 = ehRNEData.FirstOrDefault()?.upsellvalue3,
                                                    upsellvalue4 = ehRNEData.FirstOrDefault()?.upsellvalue4,
                                                    upsellvalue5 = ehRNEData.FirstOrDefault()?.upsellvalue5,
                                                    upsellpremium1 = ehRNEData.FirstOrDefault()?.upsellpremium1,
                                                    upsellpremium2 = ehRNEData.FirstOrDefault()?.upsellpremium2,
                                                    upsellpremium3 = ehRNEData.FirstOrDefault()?.upsellpremium3,
                                                    upsellpremium4 = ehRNEData.FirstOrDefault()?.upsellpremium4,
                                                    upsellpremium5 = ehRNEData.FirstOrDefault()?.upsellpremium5,
                                                    coverbaseloadingrate1 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate1,
                                                    coverbaseloadingrate2 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate2,
                                                    coverbaseloadingrate3 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate3,
                                                    coverbaseloadingrate4 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate4,
                                                    coverbaseloadingrate5 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate5,
                                                    coverbaseloadingrate6 = ehRNEData.FirstOrDefault()?.coverbaseloadingrate6,
                                                    loyalty_discount = ehRNEData.FirstOrDefault()?.loyalty_discount,
                                                    employee_discount = ehRNEData.FirstOrDefault()?.employee_discount,
                                                    online_discount = ehRNEData.FirstOrDefault()?.online_discount,
                                                    family_discount = ehRNEData.FirstOrDefault()?.family_discount,
                                                    longterm_discount = ehRNEData.FirstOrDefault()?.longterm_discount,
                                                    family_discount_PRHDC = ehRNEData.FirstOrDefault()?.family_discount_PRHDC,
                                                    base_Premium1 = ehRNEData.FirstOrDefault()?.base_Premium1,
                                                    base_Premium2 = ehRNEData.FirstOrDefault()?.base_Premium2,
                                                    base_Premium3 = ehRNEData.FirstOrDefault()?.base_Premium3,
                                                    base_Premium4 = ehRNEData.FirstOrDefault()?.base_Premium4,
                                                    base_Premium5 = ehRNEData.FirstOrDefault()?.base_Premium5,
                                                    base_Premium6 = ehRNEData.FirstOrDefault()?.base_Premium6,
                                                    basePremLoading_Insured1 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured1,
                                                    basePremLoading_Insured2 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured2,
                                                    basePremLoading_Insured3 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured3,
                                                    basePremLoading_Insured4 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured4,
                                                    basePremLoading_Insured5 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured5,
                                                    basePremLoading_Insured6 = ehRNEData.FirstOrDefault()?.basePremLoading_Insured6,
                                                    basePrem_Loading = ehRNEData.FirstOrDefault()?.basePrem_Loading,
                                                    easyHealth_BaseAndLoading_Premium = ehRNEData.FirstOrDefault()?.easyHealth_BaseAndLoading_Premium,
                                                    easyHealth_Loyalty_Discount = ehRNEData.FirstOrDefault()?.easyHealth_Loyalty_Discount,
                                                    easyHealth_Employee_Discount = ehRNEData.FirstOrDefault()?.easyHealth_Employee_Discount,
                                                    easyHealth_Online_Discount = ehRNEData.FirstOrDefault()?.easyHealth_Online_Discount,
                                                    easyHealth_Family_Discount = ehRNEData.FirstOrDefault()?.easyHealth_Family_Discount,
                                                    easyHealth_LongTerm_Discount = ehRNEData.FirstOrDefault()?.easyHealth_LongTerm_Discount,
                                                    easyHealth_ORase_Premium = ehRNEData.FirstOrDefault()?.easyHealth_ORase_Premium,
                                                    hdc_opt = ehRNEData.FirstOrDefault()?.hdc_opt,
                                                    hdc_si = ehRNEData.FirstOrDefault()?.hdc_si,
                                                    hdc_rider_premium = ehRNEData.FirstOrDefault()?.hdc_rider_premium,
                                                    hdc_family_discount = ehRNEData.FirstOrDefault()?.hdc_family_discount,
                                                    hdc_longterm_discount = ehRNEData.FirstOrDefault()?.hdc_longterm_discount,
                                                    hdc_final_premium = ehRNEData.FirstOrDefault()?.hdc_final_premium,
                                                    hdc_Rider_Premium1 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium1,
                                                    hdc_Rider_Premium2 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium2,
                                                    hdc_Rider_Premium3 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium3,
                                                    hdc_Rider_Premium4 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium4,
                                                    hdc_Rider_Premium5 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium5,
                                                    hdc_Rider_Premium6 = ehRNEData.FirstOrDefault()?.hdc_Rider_Premium6,
                                                    cI_rider_Opt = ehRNEData.FirstOrDefault()?.cI_rider_Opt,
                                                    cI_si_1 = ehRNEData.FirstOrDefault()?.cI_si_1,
                                                    cI_si_2 = ehRNEData.FirstOrDefault()?.cI_si_2,
                                                    cI_si_3 = ehRNEData.FirstOrDefault()?.cI_si_3,
                                                    cI_si_4 = ehRNEData.FirstOrDefault()?.cI_si_4,
                                                    cI_si_5 = ehRNEData.FirstOrDefault()?.cI_si_5,
                                                    cI_si_6 = ehRNEData.FirstOrDefault()?.cI_si_6,
                                                    criticalIllness_Rider_Insured1 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured1,
                                                    criticalIllness_Rider_Insured2 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured2,
                                                    criticalIllness_Rider_Insured3 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured3,
                                                    criticalIllness_Rider_Insured4 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured4,
                                                    criticalIllness_Rider_Insured5 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured5,
                                                    criticalIllness_Rider_Insured6 = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Insured6,
                                                    criticalIllness_Rider_BasePremium = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_BasePremium,
                                                    criticalIllness_Rider_FamilyDiscount = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_FamilyDiscount,
                                                    criticalIllness_Rider_LongTermDiscount = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_LongTermDiscount,
                                                    criticalIllness_Rider_Premium = ehRNEData.FirstOrDefault()?.criticalIllness_Rider_Premium,
                                                    pr_opt = ehRNEData.FirstOrDefault()?.pr_opt,
                                                    pr_insured_1 = ehRNEData.FirstOrDefault()?.pr_insured_1,
                                                    pr_insured_2 = ehRNEData.FirstOrDefault()?.pr_insured_2,
                                                    pr_insured_3 = ehRNEData.FirstOrDefault()?.pr_insured_3,
                                                    pr_insured_4 = ehRNEData.FirstOrDefault()?.pr_insured_4,
                                                    pr_insured_5 = ehRNEData.FirstOrDefault()?.pr_insured_5,
                                                    pr_insured_6 = ehRNEData.FirstOrDefault()?.pr_insured_6,
                                                    pr_ProtectorRider_Premium = ehRNEData.FirstOrDefault()?.pr_ProtectorRider_Premium,
                                                    pr_loading_insured1 = ehRNEData.FirstOrDefault()?.pr_loading_insured1,
                                                    pr_loading_insured2 = ehRNEData.FirstOrDefault()?.pr_loading_insured2,
                                                    pr_loading_insured3 = ehRNEData.FirstOrDefault()?.pr_loading_insured3,
                                                    pr_loading_insured4 = ehRNEData.FirstOrDefault()?.pr_loading_insured4,
                                                    pr_loading_insured5 = ehRNEData.FirstOrDefault()?.pr_loading_insured5,
                                                    pr_loading_insured6 = ehRNEData.FirstOrDefault()?.pr_loading_insured6,
                                                    pr_protectorriderloading_premium = ehRNEData.FirstOrDefault()?.pr_protectorriderloading_premium,
                                                    pr_BaseLoading_Premium = ehRNEData.FirstOrDefault()?.pr_BaseLoading_Premium,
                                                    pr_Family_Discount = ehRNEData.FirstOrDefault()?.pr_Family_Discount,
                                                    pr_LongTerm_Discount = ehRNEData.FirstOrDefault()?.pr_LongTerm_Discount,
                                                    prpremium_Protector_Rider_Premium = ehRNEData.FirstOrDefault()?.prpremium_Protector_Rider_Premium,
                                                    individual_personalAR_opt = ehRNEData.FirstOrDefault()?.individual_personalAR_opt,
                                                    individual_personalAR_SI = ehRNEData.FirstOrDefault()?.individual_personalAR_SI,
                                                    individual_personalAR_Amt = ehRNEData.FirstOrDefault()?.individual_personalAR_Amt,
                                                    individual_personalAR_LongTermDiscount = ehRNEData.FirstOrDefault()?.individual_personalAR_LongTermDiscount,
                                                    individual_Personal_AccidentRiderPremium = ehRNEData.FirstOrDefault()?.individual_Personal_AccidentRiderPremium,
                                                    criticalAdvantage_Rider_opt = ehRNEData.FirstOrDefault()?.criticalAdvantage_Rider_opt,
                                                    criticalAdvantageRider_SumInsured_1 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_1,
                                                    criticalAdvantageRider_SumInsured_2 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_2,
                                                    criticalAdvantageRider_SumInsured_3 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_3,
                                                    criticalAdvantageRider_SumInsured_4 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_4,
                                                    criticalAdvantageRider_SumInsured_5 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_5,
                                                    criticalAdvantageRider_SumInsured_6 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_SumInsured_6,
                                                    criticalAdvantageRider_Insured_1 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_1,
                                                    criticalAdvantageRider_Insured_2 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_2,
                                                    criticalAdvantageRider_Insured_3 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_3,
                                                    criticalAdvantageRider_Insured_4 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_4,
                                                    criticalAdvantageRider_Insured_5 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_5,
                                                    criticalAdvantageRider_Insured_6 = ehRNEData.FirstOrDefault()?.criticalAdvantageRider_Insured_6,
                                                    criticalAdvantage_RiderBase_Premium = ehRNEData.FirstOrDefault()?.criticalAdvantage_RiderBase_Premium,
                                                    criticalAdvrider_loadinginsured1 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured1,
                                                    criticalAdvrider_loadinginsured2 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured2,
                                                    criticalAdvrider_loadinginsured3 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured3,
                                                    criticalAdvrider_loadinginsured4 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured4,
                                                    criticalAdvrider_loadinginsured5 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured5,
                                                    criticalAdvrider_loadinginsured6 = ehRNEData.FirstOrDefault()?.criticalAdvrider_loadinginsured6,
                                                    criticalAdvriderloading = ehRNEData.FirstOrDefault()?.criticalAdvriderloading,

                                                    criticalAdvriderbase_loading_premium = ehRNEData.FirstOrDefault()?.criticalAdvriderbase_loading_premium,
                                                    criticalAdvRiderPremium_LongTerm_Discount = ehRNEData.FirstOrDefault()?.criticalAdvRiderPremium_LongTerm_Discount,
                                                    criticalAdv_Rider_Premium = ehRNEData.FirstOrDefault()?.criticalAdv_Rider_Premium,

                                                    net_premium = ehRNEData.FirstOrDefault()?.net_premium.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().net_premium.Value, 2)
                                         : (decimal?)0,

                                                    final_Premium = ehRNEData.FirstOrDefault()?.final_Premium.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().final_Premium.Value, 2)
                                         : (decimal?)0,

                                                    gst = ehRNEData.FirstOrDefault()?.gst.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().gst.Value, 2)
                                         : (decimal?)0,

                                                    cross_Check = ehRNEData.FirstOrDefault()?.cross_Check.HasValue == true
                                                    ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().cross_Check.Value, 2)
                                                    : (decimal?)0,
                                                    easyhealth_total_Premium = ehRNEData.FirstOrDefault()?.easyhealth_total_Premium.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().easyhealth_total_Premium.Value, 2)
                                         : (decimal?)null,

                                                    easyhealth_netpremium = ehRNEData.FirstOrDefault()?.easyhealth_netpremium.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().easyhealth_netpremium.Value, 2)
                                         : (decimal?)null,

                                                    easy_health_gst = ehRNEData.FirstOrDefault()?.easy_health_gst.HasValue == true
                                         ? (decimal?)Math.Round(ehRNEData.FirstOrDefault().easy_health_gst.Value, 2)
                                         : (decimal?)null,
                                                };
                                                if (objEasyhealthPremiumValidation?.policy_number == null)
                                                {
                                                    Console.WriteLine("Policy number not found.");
                                                }
                                                using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
                                                {
                                                    dbConnection.Open();
                                                    if (objEasyhealthPremiumValidation != null)
                                                    {
                                                        var no_of_members = easyhealthRNEData.FirstOrDefault()?.no_of_members;
                                                        var ridercount = 5;
                                                        var policy_number = objEasyhealthPremiumValidation.policy_number;
                                                        var reference_number = objEasyhealthPremiumValidation.reference_num;
                                                        var newRecord = new List<rne_calculated_cover_rg>();
                                                        for (int i = 1; i <= no_of_members; i++)
                                                        {
                                                            var sumInsured = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"sum_insured{i}")?.GetValue(objEasyhealthPremiumValidation));
                                                            var basePremium = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"base_Premium{i}")?.GetValue(objEasyhealthPremiumValidation));
                                                            //var sumInsuredupsell = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"upsell_sum_insured{i}")?.GetValue(objEasyhealthPremiumValidation));
                                                            //var basePremiumupsell = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"base_upsell_Premium{i}")?.GetValue(objEasyhealthPremiumValidation));
                                                            //var finalPremiumupsell = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"final_Premium_upsell")?.GetValue(objEasyhealthPremiumValidation));
                                                            if (no_of_members > 1 && i >= 2 && i <= 6)
                                                            {
                                                                basePremium *= 0.45m;
                                                                //basePremiumupsell *= 0.45m;
                                                            }
                                                            var newRecord1 = new rne_calculated_cover_rg
                                                            {
                                                                policy_number = policy_number,
                                                                referencenum = reference_number,
                                                                suminsured = sumInsured,
                                                                premium = basePremium,
                                                                riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{i}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                covername = "Easyhealth Basic Cover"
                                                            };
                                                            //var newRecord2 = new rne_calculated_cover_rg
                                                            //{
                                                            //    isupsell = 1,
                                                            //    policy_number = policy_number,
                                                            //    referencenum = reference_number,
                                                            //    suminsured = sumInsuredupsell,
                                                            //    premium = basePremiumupsell,
                                                            //    totalpremium = finalPremiumupsell,//total premium column in rne_calculated_cover_rg will store the finalpremiumupsell from premium computation
                                                            //    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{i}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                            //    covername = "Upsell Cover"
                                                            //};
                                                            newRecord.Add(newRecord1);
                                                            //newRecord.Add(newRecord2);
                                                        }

                                                        for (int j = 1; j <= ridercount; j++)
                                                        {
                                                            var riderPremium = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"criticalAdvantageRiderInsuredList{j}")?.GetValue(objEasyhealthPremiumValidation));
                                                            var riderPremiumpr = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"pr_insured_{j}")?.GetValue(objEasyhealthPremiumValidation));
                                                            var riderPremiumipa = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"individual_Personal_AccidentRiderPremium")?.GetValue(objEasyhealthPremiumValidation));
                                                            var riderPremiumhdc = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"hdc_Rider_Premium{j}")?.GetValue(objEasyhealthPremiumValidation));
                                                            var riderPremiumur = Convert.ToDecimal(objEasyhealthPremiumValidation.GetType().GetProperty($"criticalIllness_Rider_Insured{j}")?.GetValue(objEasyhealthPremiumValidation));
                                                            if (riderPremium > 0)
                                                            {
                                                                var riderRecord = new rne_calculated_cover_rg
                                                                {
                                                                    policy_number = policy_number,
                                                                    referencenum = reference_number,
                                                                    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{j}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                    //suminsured = riderSumInsured,
                                                                    premium = riderPremium,
                                                                    covername = "Critical Advantage Rider"
                                                                };
                                                                newRecord.Add(riderRecord);
                                                            }
                                                            if (riderPremiumpr > 0)
                                                            {
                                                                var riderRecordpr = new rne_calculated_cover_rg
                                                                {
                                                                    policy_number = policy_number,
                                                                    referencenum = reference_number,
                                                                    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{j}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                    premium = riderPremiumpr,
                                                                    covername = "Protector Rider"
                                                                };
                                                                newRecord.Add(riderRecordpr);
                                                            }
                                                            if (riderPremiumipa > 0)
                                                            {
                                                                var riderRecordipa = new rne_calculated_cover_rg
                                                                {
                                                                    policy_number = policy_number,
                                                                    referencenum = reference_number,
                                                                    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{j}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                    premium = riderPremiumipa,
                                                                    covername = "Individual Personal Accident Rider"
                                                                };
                                                                newRecord.Add(riderRecordipa);
                                                            }
                                                            if (riderPremiumhdc > 0)
                                                            {
                                                                var riderRecordhdc = new rne_calculated_cover_rg
                                                                {
                                                                    policy_number = policy_number,
                                                                    referencenum = reference_number,
                                                                    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{j}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                    premium = riderPremiumhdc,
                                                                    covername = "Hospital Daily Cash Rider"
                                                                };
                                                                newRecord.Add(riderRecordhdc);
                                                            }
                                                            if (riderPremiumur > 0)
                                                            {
                                                                var riderRecordur = new rne_calculated_cover_rg
                                                                {
                                                                    policy_number = policy_number,
                                                                    referencenum = reference_number,
                                                                    riskname = objEasyhealthPremiumValidation.GetType().GetProperty($"txt_insuredname{j}")?.GetValue(objEasyhealthPremiumValidation)?.ToString(),
                                                                    premium = riderPremiumur,
                                                                    covername = "Critical Illness Rider"
                                                                };
                                                                newRecord.Add(riderRecordur);
                                                            }
                                                        }

                                                        var insertQuery = @"
                                    INSERT INTO rne_calculated_cover_rg (policy_number, referencenum, suminsured, premium, totalpremium, riskname, covername, isupsell)
                                    VALUES (@policy_number, @referencenum, @suminsured, @premium, @totalpremium, @riskname, @covername, @isupsell);
                                    ";
                                                        dbConnection.ExecuteAsync(insertQuery, newRecord);
                                                    }
                                                }
                                                break;
                                            }
                                        }
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




