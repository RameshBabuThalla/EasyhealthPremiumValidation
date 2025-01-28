﻿using HERGPremiumValidationSchedular.Data;
using HERGPremiumValidationSchedular.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Collections;
using Serilog;

namespace HERGPremiumValidationSchedular.BussinessLogic
{
   
    public class EasyHealth
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<EasyHealth> _logger;
       
        public EasyHealth(HDFCDbContext hDFCDbContext, ILogger<EasyHealth> logger)
        {
            this.dbContext = hDFCDbContext;
            _logger = logger;
           
        }


        public async Task GetEasyHealthValidation(IEnumerable<EasyHealthRNE> ehRNEData, string policyNo,HDFCDbContext dbContext, Dictionary<string, Hashtable> baseRateHashTable, Dictionary<string, Hashtable> hdcRatesTable, Dictionary<string, Hashtable> caRatesTable, Dictionary<string, Hashtable> ciRatesTable)
        {
            _logger.LogInformation("EasyHealthPremiumValidation is Started!");
           await CalculateEasyHealthPremium(ehRNEData,policyNo,dbContext, baseRateHashTable, hdcRatesTable, caRatesTable, ciRatesTable);

        }
        async Task HandleCrosschecksAndUpdateStatus(EasyHealthRNE ehRNEData, decimal? crosscheck1, decimal? crosscheck2)
        {
            var record = dbContext.premium_validation.FirstOrDefault(item => item.certificate_no == ehRNEData.policy_number.ToString());
            if (record != null && record.rn_generation_status == "Reconciliation Successful")
            {
                decimal crosscheck1Value = crosscheck1.HasValue ? crosscheck1.Value : 0;
                decimal crosscheck2Value = crosscheck2.HasValue ? crosscheck2.Value : 0;
                if (crosscheck1.HasValue)
                {
                    if ((Math.Abs(crosscheck1.Value) <= 10) || ((Math.Abs(crosscheck1.Value) <= 10 && Math.Abs(crosscheck2Value) <= 10)))
                    {
                        record.rn_generation_status = "RN Generation Awaited";
                        record.final_remarks = "RN Generation Awaited";
                        record.dispatch_status = "PDF Gen Under Process With CLICK PSS Team";
                    }
                    else if ((Math.Abs(crosscheck1.Value) > 10) ||( Math.Abs(crosscheck1.Value) > 10 && Math.Abs(crosscheck2Value) <= 10))
                    {
                        record.rn_generation_status = "IT Issue - QC Failed";
                        record.final_remarks = "IT Issues";
                        record.dispatch_status = "Revised Extraction REQ From IT Team QC Failed Cases";
                        record.error_description = "Premium verification failed due to premium difference of more than 10 rupees";
                    }
                    else if (Math.Abs(crosscheck1.Value) <= 10 && Math.Abs(crosscheck2Value) > 10)
                    {
                        record.rn_generation_status = "IT Issue - Upsell QC Failed";
                    }
                    else if (Math.Abs(crosscheck1.Value) > 10 && Math.Abs(crosscheck2Value) > 10)
                    {
                        record.rn_generation_status = "IT Issue - QC Failed";
                    }
                }
                dbContext.premium_validation.Update(record);


                int maxRetries = 5;
                int attempt = 0;
                int baseDelay = 1000; // Base delay in milliseconds
                Random rng = new Random();
                while (attempt < maxRetries)
                {
                    try
                    {
                        await dbContext.SaveChangesAsync();
                        break;  // Exit if successful
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        await entry.ReloadAsync();
                    }
                    catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "40P01")
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                            throw;

                        int delay = baseDelay * (int)Math.Pow(2, attempt) + rng.Next(0, 500);  // Random jitter added
                        await Task.Delay(delay);
                    }
                }
            }
        }

        public async Task<IEnumerable<EasyHealthRNE>> GetGCEasyHealthDataAsync(string policyNo, HDFCDbContext dbContext)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            using (var connection = dbContext.Database.GetDbConnection())
            {
                var sqlQuery = @"
                SELECT
                eh.prod_code,
                eh.reference_num,
                eh.prod_name,
                eh.policy_number,
               
                eh.policy_start_date,
                eh.policy_expiry_date,
                eh.policy_period,
                eh.tier_type,
                eh.policyplan,
                eh.policy_type,
                eh.txt_family,
             
                eh.num_tot_premium,
                eh.num_net_premium,
                eh.num_service_tax,
                eh.coverbaseloadingrate1,
                eh.coverbaseloadingrate2,
                eh.coverbaseloadingrate3,
                eh.coverbaseloadingrate4,
                eh.coverbaseloadingrate5,
                eh.coverbaseloadingrate6,
                ehidst.loading_per_insured1,
                ehidst.loading_per_insured2,
                ehidst.loading_per_insured3,
                ehidst.loading_per_insured4,
                ehidst.loading_per_insured5,
                ehidst.loading_per_insured6,
                eh.txt_insuredname1,
                eh.txt_insuredname2,
                eh.txt_insuredname3,
                eh.txt_insuredname4,
                eh.txt_insuredname5,
                eh.txt_insuredname6,
                eh.txt_insured_dob1,
                eh.txt_insured_dob2,
                eh.txt_insured_dob3,
                eh.txt_insured_dob4,
                eh.txt_insured_dob5,
                eh.txt_insured_dob6,
                eh.txt_insured_relation1,
                eh.txt_insured_relation2,
                eh.txt_insured_relation3,
                eh.txt_insured_relation4,
                eh.txt_insured_relation5,
                eh.txt_insured_relation6,
                eh.txt_insured_age1,
                eh.txt_insured_age2,
                eh.txt_insured_age3,
                eh.txt_insured_age4,
                eh.txt_insured_age5,
                eh.txt_insured_age6,
                eh.txt_insured_gender1,
                eh.txt_insured_gender2,
                eh.txt_insured_gender3,
                eh.txt_insured_gender4,
                eh.txt_insured_gender5,
                eh.txt_insured_gender6,
               
                eh.pollddesc1,
                eh.pollddesc2,
                eh.pollddesc3,
                eh.pollddesc4,
                eh.pollddesc5,
                eh.upselltype1,
                eh.upselltype2,
                eh.upselltype3,
                eh.upselltype4,
                eh.upselltype5,
                eh.upsellvalue1,
                eh.upsellvalue2,
                eh.upsellvalue3,
                eh.upsellvalue4,
                eh.upsellvalue5,
                eh.upsellpremium1,
                eh.upsellpremium2,
                eh.upsellpremium3,
                eh.upsellpremium4,
                eh.upsellpremium5,
                eh.sum_insured1,
                eh.sum_insured2,
                eh.sum_insured3,
                eh.sum_insured4,
                eh.sum_insured5,
                eh.sum_insured6,
                eh.insured_cb1,
                eh.insured_cb2,
                eh.insured_cb3,
                eh.insured_cb4,
                eh.insured_cb5,
                eh.insured_cb6,

                eh.premium_insured1,
                eh.premium_insured2,
                eh.premium_insured3,
                eh.premium_insured4,
                eh.premium_insured5,
                eh.premium_insured6,
                eh.covername11,
                eh.covername12,
                eh.covername13,
                eh.covername14,
                eh.covername15,
                eh.covername16,
                eh.covername17,
                eh.covername18,
                eh.covername19,
                eh.covername21,
                eh.covername22,
                eh.covername23,
                eh.covername24,
                eh.covername25,
                eh.covername26,
                eh.covername27,
                eh.covername28,
                eh.covername29,
                eh.covername31,
                eh.covername32,
                eh.covername33,
                eh.covername34,
                eh.covername35,
                eh.covername36,
                eh.covername37,
                eh.covername38,
                eh.covername39,
                eh.covername41,
                eh.covername42,
                eh.covername43,
                eh.covername44,
                eh.covername45,
                eh.covername46,
                eh.covername47,
                eh.covername48,
                eh.covername49,
                eh.covername51,
                eh.covername52,
                eh.covername53,
                eh.covername54,
                eh.covername55,
                eh.covername56,
                eh.covername57,
                eh.covername58,
                eh.covername59,
                eh.covername61,
                eh.covername62,
                eh.covername63,
                eh.covername64,
                eh.covername65,
                eh.covername66,
                eh.covername67,
                eh.covername68,
                eh.covername69,
                eh.covername101,
                eh.covername102,
                eh.covername103,
                eh.covername104,
                eh.covername105,
                eh.covername106,
                eh.covername107,
                eh.covername108,
                eh.covername109,
                eh.covername110,
                eh.covername210,
                eh.covername310,
                eh.covername410,
                eh.covername510,
                eh.covername610,
                eh.covername1010,
                
                eh.coversi11,
                eh.coversi12,
                eh.coversi13,
                eh.coversi14,
                eh.coversi15,
                eh.coversi16,
                eh.coversi17,
                eh.coversi18,
                eh.coversi19,
                eh.coversi21,
                eh.coversi22,
                eh.coversi23,
                eh.coversi24,
                eh.coversi25,
                eh.coversi26,
                eh.coversi27,
                eh.coversi28,
                eh.coversi29,
                eh.coversi31,
                eh.coversi32,
                eh.coversi33,
                eh.coversi34,
                eh.coversi35,
                eh.coversi36,
                eh.coversi37,
                eh.coversi38,
                eh.coversi39,
                eh.coversi41,
                eh.coversi42,
                eh.coversi43,
                eh.coversi44,
                eh.coversi46,
                eh.coversi47,
                eh.coversi48,
                eh.coversi49,
                eh.coversi51,
                eh.coversi52,
                eh.coversi53,
                eh.coversi54,
                eh.coversi55,
                eh.coversi56,
                eh.coversi57,
                eh.coversi58,
                eh.coversi59,
                eh.coversi61,
                eh.coversi62,
                eh.coversi63,
                eh.coversi64,
                eh.coversi65,
                eh.coversi66,
                eh.coversi67,
                eh.coversi68,
                eh.coversi69,
                eh.coversi101,
                eh.coversi102,
                eh.coversi103,
                eh.coversi104,
                eh.coversi105,
                eh.coversi106,
                eh.coversi107,
                eh.coversi108,
                eh.coversi109,
                eh.coversi210,
                eh.coversi310,
                eh.coversi410,
                eh.coversi510,
                eh.coversi610,
                eh.coversi1010,

                eh.coverprem11,
                eh.coverprem12,
                eh.coverprem13,
                eh.coverprem14,
                eh.coverprem15,
                eh.coverprem16,
                eh.coverprem17,
                eh.coverprem18,
                eh.coverprem19,
                eh.coverprem21,
                eh.coverprem22,
                eh.coverprem23,
                eh.coverprem24,
                eh.coverprem25,
                eh.coverprem26,
                eh.coverprem27,
                eh.coverprem28,
                eh.coverprem29,
                eh.coverprem31,
                eh.coverprem32,
                eh.coverprem33,
                eh.coverprem34,
                eh.coverprem35,
                eh.coverprem36,
                eh.coverprem37,
                eh.coverprem38,
                eh.coverprem39,
                eh.coverprem41,
                eh.coverprem42,
                eh.coverprem43,
                eh.coverprem44,
                eh.coverprem46,
                eh.coverprem47,
                eh.coverprem48,
                eh.coverprem49,
                eh.coverprem51,
                eh.coverprem52,
                eh.coverprem53,
                eh.coverprem54,
                eh.coverprem55,
                eh.coverprem56,
                eh.coverprem57,
                eh.coverprem58,
                eh.coverprem59,
                eh.coverprem61,
                eh.coverprem62,
                eh.coverprem63,
                eh.coverprem64,
                eh.coverprem65,
                eh.coverprem66,
                eh.coverprem67,
                eh.coverprem68,
                eh.coverprem69,
                eh.coverprem101,
                eh.coverprem102,
                eh.coverprem103,
                eh.coverprem104,
                eh.coverprem105,
                eh.coverprem106,
                eh.coverprem107,
                eh.coverprem108,
                eh.coverprem109,
                eh.coverprem210,
                eh.coverprem310,
                eh.coverprem410,
                eh.coverprem510,
                eh.coverprem610,
                eh.coverprem1010,
                eh.insured_loadingamt1,
                eh.insured_loadingamt2,
                eh.insured_loadingamt3,
                eh.insured_loadingamt4,
                eh.insured_loadingamt5,
                eh.insured_loadingamt6
                
            FROM ins.rne_healthtab eh
            INNER JOIN ins.idst_renewal_data_rgs ehidst ON eh.policy_number = ehidst.certificate_no
            WHERE eh.policy_number = @PolicyNo";

                var result = await connection.QueryAsync<EasyHealthRNE>(sqlQuery, new { PolicyNo = policyNo });
                return result;
            }
        }
        
        static Dictionary<string, string> DataRowToDictionary(DataRow row)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (DataColumn column in row.Table.Columns)
            {
                dictionary[column.ColumnName] = row[column].ToString();
            }

            return dictionary;
        }

        public async Task<IEnumerable<IdstData>> GetIdstRenewalData(string policyNo)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            string sqlQuery = @"
        SELECT
            certificate_no,
            loading_per_insured1,
            loading_per_insured2,
            loading_per_insured3,
            loading_per_insured4,
            loading_per_insured5,
            loading_per_insured6,
            loading_per_insured7,
            loading_per_insured8,
            loading_per_insured9,
            loading_per_insured10,
            loading_per_insured11,
            loading_per_insured12
        FROM
            ins.idst_renewal_data_rgs
        WHERE
            certificate_no = @PolicyNo";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<IdstData>(sqlQuery, new { PolicyNo = policyNo }).ConfigureAwait(false);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log the error (use your preferred logging mechanism)
                Log.Error(ex, "An error occurred while fetching renewal data for policy: {PolicyNo}", policyNo);

                // Optionally, handle the error here or return a default value like an empty list
                return Enumerable.Empty<IdstData>();  // Returning an empty list in case of failure
            }
        }
        public static List<object> ExtractData(HERGPremiumValidationSchedular.Models.Domain.EasyHealthRNE easyHealthRNE)
        {
            var data = new List<object>();
            var properties = typeof(HERGPremiumValidationSchedular.Models.Domain.EasyHealthRNE).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                for (int i = 11; i <= 1010; i += 1)  // Adjust the range if necessary
                {
                    if (property.Name.StartsWith($"covername{i}") ||
                        property.Name.StartsWith($"coversi{i}") ||
                        property.Name.StartsWith($"coverprem{i}"))
                    {
                        data.Add(property.GetValue(easyHealthRNE));
                    }
                }
            }

            return data;
        }

        private async Task CalculateEasyHealthPremium(IEnumerable<EasyHealthRNE> ehRNEData,string policyNo,HDFCDbContext dbContext, Dictionary<string, Hashtable> baseRateHashTable, Dictionary<string, Hashtable> hdcRatesTable, Dictionary<string, Hashtable> caRatesTable, Dictionary<string, Hashtable> ciRatesTable)
        {
            EasyHealthRNE eh = null;
            var columnNames = new List<string>();
            var idstData = await GetIdstRenewalData(policyNo);
            foreach (var row in ehRNEData)
            {
                var policNo16 = row.policy_number;
                var iDSTData = idstData.FirstOrDefault(x => x.certificate_no == policNo16);
                DataTable table = new DataTable();
                for (int i = 11; i <= 66; i += 10)
                {
                    for (int j = i; j < i + 6; j++)
                    {
                        //Log.Information($"i {j}");
                        table.Columns.Add($"covername{j}", typeof(string));
                        table.Columns.Add($"coversi{j}", typeof(string));
                        table.Columns.Add($"coverprem{j}", typeof(string));
                        table.Columns.Add($"coverloadingrate{j}", typeof(string));
                    }
                }
                // Loop for special range 101 to 109
                for (int i = 101; i <= 109; i++)
                {
                    table.Columns.Add($"covername{i}", typeof(string));
                    table.Columns.Add($"coversi{i}", typeof(string));
                    table.Columns.Add($"coverprem{i}", typeof(string));
                    table.Columns.Add($"coverloadingrate{i}", typeof(string));
                }
                for (int i = 1; i <= 10; i++)
                {
                    int baseValue = i * 100;
                    int columnBaseValue = baseValue + 10;
                    table.Columns.Add($"covername{columnBaseValue}", typeof(string));
                    table.Columns.Add($"coversi{columnBaseValue}", typeof(string));
                    table.Columns.Add($"coverprem{columnBaseValue}", typeof(string));
                    table.Columns.Add($"coverloadingrate{columnBaseValue}", typeof(string));
                }
                var data = ExtractData(row);
                DataRow newRow = table.NewRow();
                int indexrow = 0;
                foreach (DataColumn column in table.Columns)
                {
                    if (indexrow < data.Count)
                    {
                        newRow[column.ColumnName] = data[indexrow] ?? DBNull.Value;
                        indexrow++;
                    }
                }   
                table.Rows.Add(newRow);

                string searchRider1 = "Critical Illness Rider";
                string searchRider2 = "Critical Advantage Rider";
                string searchRider3 = "Hospital Daily Cash Rider";
                string searchRider4 = "Individual Personal Accident Rider";
                string searchRider5 = "Protector Rider";

                DataTable siRiderOneDataTable = new DataTable();
                siRiderOneDataTable.Columns.Add("RiderName", typeof(string));
                siRiderOneDataTable.Columns.Add("SIValue", typeof(object));
                siRiderOneDataTable = GetRiderSI(table, searchRider1);

                DataTable siRiderTwoDataTable = new DataTable();
                siRiderTwoDataTable.Columns.Add("RiderName", typeof(string));
                siRiderTwoDataTable.Columns.Add("SIValue", typeof(object));
                siRiderTwoDataTable = GetRiderSI(table, searchRider2);

                DataTable siRiderThreeDataTable = new DataTable();
                siRiderThreeDataTable.Columns.Add("RiderName", typeof(string));
                siRiderThreeDataTable.Columns.Add("SIValue", typeof(object));
                siRiderThreeDataTable = GetRiderSI(table, searchRider3);

                DataTable siRiderFourDataTable = new DataTable();
                siRiderFourDataTable.Columns.Add("RiderName", typeof(string));
                siRiderFourDataTable.Columns.Add("SIValue", typeof(object));
                siRiderFourDataTable = GetRiderSI(table, searchRider4);

                DataTable siRiderFiveDataTable = new DataTable();
                siRiderFiveDataTable.Columns.Add("RiderName", typeof(string));
                siRiderFiveDataTable.Columns.Add("SIValue", typeof(object));
                siRiderFiveDataTable = GetRiderSI(table, searchRider5);
                string? policyLdDesc1 = row.pollddesc1;
                string? policyLdDesc2 = row.pollddesc2;
                string? policyLdDesc3 = row.pollddesc3;
                string? policyLdDesc4 = row.pollddesc4;
                string? policyLdDesc5 = row.pollddesc5;
                List<string?> policyLdDescValues = new List<string?>();
                policyLdDescValues.Add(policyLdDesc1);
                policyLdDescValues.Add(policyLdDesc2);
                policyLdDescValues.Add(policyLdDesc3);
                policyLdDescValues.Add(policyLdDesc4);
                policyLdDescValues.Add(policyLdDesc5);
                string? upsellType1 = row.upselltype1;
                string? upsellType2 = row.upselltype2;
                string? upsellType3 = row.upselltype3;
                string? upsellType4 = row.upselltype4;
                string? upsellType5 = row.upselltype5;
                string? upsellValue1 = row.upsellvalue1;
                string? upsellValue2 = row.upsellvalue2;
                string? upsellValue3 = row.upsellvalue3;
                string? upsellValue4 = row.upsellvalue4;
                string? upsellValue5 = row.upsellvalue5;

                List<string?> upsellvalueValues = new List<string?>()
                {
                    upsellValue1,
                    upsellValue2,
                    upsellValue3,
                    upsellValue4,
                    upsellValue5
                };

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);//c18
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);//c19            
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);//c20
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);//c21
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);//c22
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);//c23

                var zone = row.tier_type;//c11


                List<int?> ageValues = new List<int?>();
                int?[] ageStrings = {
            insuredAgeOne,
            insuredAgeTwo,
            insuredAgeThree,
            insuredAgeFour,
            insuredAgeFive,
            insuredAgeSix
        };
                foreach (var age in ageStrings)
                {
                    ageValues.Add(age); 
                }

                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;

                // Loyalty Discount c44
                string searchLoyaltyDescText = "Loyalty Discount";
                bool containsLoyaltyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchLoyaltyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsLoyaltyDescText ? 1 : 0;
                decimal? loyaltyDiscountValue = containsLoyaltyDescText ? 2.5m : 0.0m;
                // Employee Discount
                string searchEmployeeDescText = "Employee Discount";
                bool containsEmployeeDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchEmployeeDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchEmployeeDescText = containsEmployeeDescText ? 1 : 0;
                decimal? employeeDiscountValue = containsEmployeeDescText ? 5.0m : 0.0m;
                // Online Discount
                string searchOnlineDescText = "Online Discount";
                bool containsOnlineDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchOnlineDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchOnlineDescText = containsOnlineDescText ? 1 : 0;
                decimal? onlineDiscountValue = containsOnlineDescText ? 5.0m : 0.0m;
                // Family Discount
                var policyType = row.policy_type;
                string searcFamilyDescText = "Family Discount";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                decimal? familyDiscountValue = GetFamilyDiscount(policyType, numberOfMemberss);
                decimal? familyDiscountPRHDC = GetFamilyDiscountPRHDC(policyType, numberOfMemberss);
                // Long Term Discount
                var policyPeriod = row.policy_period;
                decimal longTermDiscount = GetLongTermDiscount(policyPeriod);
                var columnName = GetColumnNameForPolicyPeriod(policyPeriod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
                }

                decimal? loyaltyDiscount = resultLoyaltyDescText;// row.loyalty_discount;//c798
                decimal? employeeDiscount = resultSearchEmployeeDescText;//c799
                decimal? onlineDiscount = resultSearchOnlineDescText;//c800

                //decimal? siOne = row.sum_insured1;//c26
                //decimal? siTwo = row.sum_insured2;//c27
                //decimal? siThree = row.sum_insured3;//c28
                //decimal? siFour = row.sum_insured4;//c29
                //decimal? siFive = row.sum_insured5;//c30
                //decimal? siSix = row.sum_insured6;//c31
                //decimal? totalsuminsured = (siOne ?? 0) + (siTwo ?? 0) + (siThree ?? 0) + (siFour ?? 0) + (siFive ?? 0) + (siSix ?? 0);

                //decimal? cbOne = Convert.ToDecimal(row.insured_cb1);
                //decimal? cbTwo = Convert.ToDecimal(row.insured_cb2);
                //decimal? cbThree = Convert.ToDecimal(row.insured_cb3);
                //decimal? cbFour = Convert.ToDecimal(row.insured_cb4);
                //decimal? cbFive = Convert.ToDecimal(row.insured_cb5);
                //decimal? cbSix = Convert.ToDecimal(row.insured_cb6);
                //decimal? cumulativebonus = (cbOne ?? 0) + (cbTwo ?? 0) + (cbThree ?? 0) + (cbFour ?? 0) + (cbFive ?? 0) + (cbSix ?? 0);

                //decimal? basicLoadingRateOne = iDSTData.loading_per_insured1 ?? 0;//c38
                //decimal? basicLoadingRateTwo = iDSTData.loading_per_insured2 ?? 0;//c39
                //decimal? basicLoadingRateThree = iDSTData.loading_per_insured3 ?? 0;//c40
                //decimal? basicLoadingRateFour = iDSTData.loading_per_insured4 ?? 0;//c41
                //decimal? basicLoadingRateFive = iDSTData.loading_per_insured5 ?? 0;//c42
                //decimal? basicLoadingRateSix = iDSTData.loading_per_insured6 ?? 0;//c43

                List<decimal?> sumInsuredList = new List<decimal?>();

                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? sumInsured = (decimal?)row.GetType().GetProperty($"sum_insured{i}").GetValue(row);
                    sumInsuredList.Add(sumInsured);
                }

                decimal? totalsuminsured = sumInsuredList.Sum(si => si ?? 0);

                List<decimal?> cumulativeBonusList = new List<decimal?>();

                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? bonusValue = Convert.ToDecimal(row.GetType().GetProperty($"insured_cb{i}")?.GetValue(row));
                    cumulativeBonusList.Add(bonusValue);
                }

                decimal? cumulativeBonus = cumulativeBonusList.Sum(cb => cb ?? 0);


                List<decimal?> basicLoadingRates = new List<decimal?>();

                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? loadingRate = (decimal?)iDSTData.GetType().GetProperty($"loading_per_insured{i}")?.GetValue(iDSTData);

                    basicLoadingRates.Add(loadingRate ?? 0);
                }

                var c12 = row.policyplan.Trim();//c16

                decimal defaultValue = 0;

                List<decimal?> basePremiumsList = new List<decimal?>();
                decimal? basePrem = 0;
                for (int i = 0; i < noOfMembers; i++)
                {
                    basePrem = baseRateHashTable
                        .Where(row =>
                            row.Value is Hashtable rateDetails &&
                            (int)rateDetails["si"] == sumInsuredList[i] &&  // Using sumInsuredList[i]
                            (int)rateDetails["age"] == ageValues[i] &&
                            rateDetails["tier"].ToString() == zone &&
                            rateDetails["variant"].ToString() == c12)
                        .Select(row =>
                            row.Value is Hashtable details &&
                            details[columnName] != null
                                ? Convert.ToDecimal(details[columnName])
                                : (decimal?)null)
                        .FirstOrDefault();

                    basePremiumsList.Add(basePrem);
                }

                //var sql = $@"
                //SELECT {columnName}
                //FROM easyhealth_baserates
                //WHERE si = @p0 AND age = @p1 AND tier = @p2 AND variant = @p3";

                //// Execute the raw SQL query
                //var basePremium1 = await dbContext.easyhealth_baserates
                //.FromSqlRaw(sql, siOne, insuredAgeOne, zone, c12)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? defaultValue;
                //var basePremium2 = await dbContext.easyhealth_baserates
                //    .FromSqlRaw(sql, siTwo, insuredAgeTwo, zone, c12)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? defaultValue;
                //// Execute the raw SQL query
                //var basePremium3 = await dbContext.easyhealth_baserates
                //    .FromSqlRaw(sql, siThree, insuredAgeThree, zone, c12)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? defaultValue;
                //var basePremium4 = await dbContext.easyhealth_baserates
                //   .FromSqlRaw(sql, siFour, insuredAgeFour, zone, c12)
                //   .Select(r => EF.Property<decimal?>(r, columnName))
                //   .FirstOrDefaultAsync() ?? defaultValue;
                //// Execute the raw SQL query
                //var basePremium5 = await dbContext.easyhealth_baserates
                //    .FromSqlRaw(sql, siFive, insuredAgeFive, zone, c12)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? defaultValue;
                //var basePremium6 = await dbContext.easyhealth_baserates
                //   .FromSqlRaw(sql, siSix, insuredAgeSix, zone, c12)
                //   .Select(r => EF.Property<decimal?>(r, columnName))
                //   .FirstOrDefaultAsync() ?? defaultValue;

                //var basePremiumvalues = new List<decimal?> { basePremium1, basePremium2, basePremium3, basePremium4, basePremium5, basePremium6 };
                string condition = policyType; 

                decimal? basePremium = GetBasePremium(condition, basePremiumsList);

                // Base Premium Loading
                decimal? basePremLoadingInsured1 = GetBasePremLoadingInsured1(0, basePremiumsList, basicLoadingRates);
                decimal? basePremLoadingInsured2 = GetBasePremLoadingInsured1(1, basePremiumsList, basicLoadingRates);
                decimal? basePremLoadingInsured3 = GetBasePremLoadingInsured1(2, basePremiumsList, basicLoadingRates);
                decimal? basePremLoadingInsured4 = GetBasePremLoadingInsured1(3, basePremiumsList, basicLoadingRates);
                decimal? basePremLoadingInsured5 = GetBasePremLoadingInsured1(4, basePremiumsList, basicLoadingRates);
                decimal? basePremLoadingInsured6 = GetBasePremLoadingInsured1(5, basePremiumsList, basicLoadingRates);
                var loadingPremvalues = new List<decimal?> { basePremLoadingInsured1 + basePremLoadingInsured2 + basePremLoadingInsured3 + basePremLoadingInsured4 + basePremLoadingInsured5 + basePremLoadingInsured6 }; //c62
                decimal? basePremLoading = loadingPremvalues.Sum();

                decimal? easyHealthBaseAndLoadingPremium = basePremium + basePremLoading;
                decimal? easyHealthLoyaltyDiscount = loyaltyDiscountValue / 100 * easyHealthBaseAndLoadingPremium;
                decimal? easyHealthEmployeeDiscount = employeeDiscountValue / 100 * easyHealthBaseAndLoadingPremium;
                decimal? easyHealthOnlineDiscount = (onlineDiscountValue / 100) * easyHealthBaseAndLoadingPremium;
                decimal? easyHealthFamilyDiscount = familyDiscountValue * easyHealthBaseAndLoadingPremium;
                decimal? easyHealthLongTermDiscount = (easyHealthBaseAndLoadingPremium - (easyHealthLoyaltyDiscount + easyHealthEmployeeDiscount + easyHealthOnlineDiscount + easyHealthFamilyDiscount)) * longTermDiscount;
                decimal? easyHealthORasePremium = easyHealthBaseAndLoadingPremium - (easyHealthLoyaltyDiscount + easyHealthEmployeeDiscount + easyHealthOnlineDiscount + easyHealthFamilyDiscount + easyHealthLongTermDiscount);

                // Critical Illness Rider
                decimal? SI = 0;
                string criticalIllnessRideropt = "N";
                decimal? ci_si_1 = 0;
                decimal? ci_si_2 = 0;
                decimal? ci_si_3 = 0;
                decimal? ci_si_4 = 0;
                decimal? ci_si_5 = 0;
                decimal? ci_si_6 = 0;
                int indexci = 1;
                if (siRiderOneDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderOneDataTable.Rows)
                    {

                        criticalIllnessRideropt = "Y";
                        decimal siValue = (decimal)itemRow["SIValue"];
                        switch (indexci)
                        {
                            case 1:
                                ci_si_1 += siValue;
                                break;
                            case 2:
                                ci_si_2 += siValue;
                                break;
                            case 3:
                                ci_si_3 += siValue;
                                break;
                            case 4:
                                ci_si_4 += siValue;
                                break;
                            case 5:
                                ci_si_5 += siValue;
                                break;
                            case 6:
                                ci_si_6 += siValue;
                                break;

                        }
                        indexci++;
                    }
                }
                //  var sqlci = $@"
                //  SELECT {columnName}
                //  FROM easyhealth_cirates
                //  WHERE age = @p0  AND si = @p1";

                //  // Execute the raw SQL query
                //  decimal criticalIllnessRiderInsured1 = await dbContext.easyhealth_cirates
                //  .FromSqlRaw(sqlci, insuredAgeOne, ci_si_1)
                //      .Select(r => EF.Property<decimal?>(r, columnName))
                //      .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalIllnessRiderInsured2 = await dbContext.easyhealth_cirates
                // .FromSqlRaw(sqlci, insuredAgeTwo, ci_si_2)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalIllnessRiderInsured3 = await dbContext.easyhealth_cirates
                // .FromSqlRaw(sqlci, insuredAgeThree, ci_si_3)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalIllnessRiderInsured4 = await dbContext.easyhealth_cirates
                // .FromSqlRaw(sqlci, insuredAgeFour, ci_si_4)
                //   .Select(r => EF.Property<decimal?>(r, columnName))
                //   .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalIllnessRiderInsured5 = await dbContext.easyhealth_cirates
                // .FromSqlRaw(sqlci, insuredAgeFive, ci_si_5)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalIllnessRiderInsured6 = await dbContext.easyhealth_cirates
                //.FromSqlRaw(sqlci, insuredAgeSix, ci_si_6)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? 0;
                //decimal? criticalIllnessRiderBasePremium = criticalIllnessRiderInsured1 + criticalIllnessRiderInsured2 + criticalIllnessRiderInsured3 + criticalIllnessRiderInsured4 + criticalIllnessRiderInsured5 + criticalIllnessRiderInsured6;

                List<decimal?> ciratesList = new List<decimal?>();
                decimal? ciRiderinsured = 0;
                for (int i = 0; i < noOfMembers; i++)
                {
                     ciRiderinsured = ciRatesTable
                   .Where(roww =>
                       roww.Value is Hashtable rateDetails &&
                       (int)rateDetails["age"] == eldestMember &&
                       (int)rateDetails["si"] == sumInsuredList[i])
                   .Select(roww =>
                       roww.Value is Hashtable details && details[columnName] != null
                       ? Convert.ToDecimal(details[columnName])
                       : (decimal?)null
                   )
                   .FirstOrDefault();

                    ciratesList.Add(ciRiderinsured);
                }

                var policYPeriod = GetColumnNameForPolicyPeriod(policyPeriod);
                if (policyPeriod == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
                }

                // Critical Illness Rider Premiums
                decimal? criticalIllnessRiderFamilyDiscount = 0;
                decimal? criticalIllnessRiderLongTermDiscount = (ciRiderinsured - criticalIllnessRiderFamilyDiscount) * longTermDiscount;
                decimal? criticalIllnessRiderPremium = ciRiderinsured - criticalIllnessRiderFamilyDiscount - criticalIllnessRiderLongTermDiscount;

                // Critical Advantage Rider
                string criticalAdvantageRideropt = "N";
                decimal? caSI = 0;
                decimal? ca_si_1 = 0;
                decimal? ca_si_2 = 0;
                decimal? ca_si_3 = 0;
                decimal? ca_si_4 = 0;
                decimal? ca_si_5 = 0;
                decimal? ca_si_6 = 0;
                int indexca = 1;
                if (siRiderTwoDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderTwoDataTable.Rows)
                    {

                        criticalAdvantageRideropt = "Y";
                        decimal siValue = (decimal)itemRow["SIValue"];
                        //caSI = Convert.ToDecimal(siValue);
                        switch (indexca)
                        {
                            case 1:
                                ca_si_1 += siValue;
                                break;
                            case 2:
                                ca_si_2 += siValue;
                                break;
                            case 3:
                                ca_si_3 += siValue;
                                break;
                            case 4:
                                ca_si_4 += siValue;
                                break;
                            case 5:
                                ca_si_5 += siValue;
                                break;
                            case 6:
                                ca_si_6 += siValue;
                                break;
                        }
                        indexca++;
                    }
                }
                List<decimal?> criticalAdvantageList = new List<decimal?>();
                decimal? criticalAdvantageRiderInsured = 0;
                for (int i = 0; i < noOfMembers; i++)
                {
                    criticalAdvantageRiderInsured = caRatesTable
                        .Where(row =>
                            row.Value is Hashtable rateDetails &&
                            (int)rateDetails["age"] == ageValues[i] &&
                            (int)rateDetails["si"] == sumInsuredList[i])
                        .Select(row =>
                            row.Value is Hashtable details &&
                            details[columnName] != null
                                ? Convert.ToDecimal(details[columnName])
                                : (decimal?)null)
                        .FirstOrDefault();

                    criticalAdvantageList.Add(criticalAdvantageRiderInsured);
                }

                //  var sqlca = $@"
                //  SELECT {columnName}
                //  FROM easyhealth_carates
                //  WHERE age = @p0  AND si = @p1";
                //  // Execute the raw SQL query
                //  decimal criticalAdvantageRiderInsured1 = await dbContext.easyhealth_carates
                //  .FromSqlRaw(sqlca, insuredAgeOne, ca_si_1)
                //      .Select(r => EF.Property<decimal?>(r, columnName))
                //      .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalAdvantageRiderInsured2 = await dbContext.easyhealth_carates
                // .FromSqlRaw(sqlca, insuredAgeTwo, ca_si_2)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalAdvantageRiderInsured3 = await dbContext.easyhealth_carates
                // .FromSqlRaw(sqlca, insuredAgeThree, ca_si_3)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalAdvantageRiderInsured4 = await dbContext.easyhealth_carates
                // .FromSqlRaw(sqlca, insuredAgeFour, ca_si_4)
                //   .Select(r => EF.Property<decimal?>(r, columnName))
                //   .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalAdvantageRiderInsured5 = await dbContext.easyhealth_carates
                // .FromSqlRaw(sqlca, insuredAgeFive, ca_si_5)
                //     .Select(r => EF.Property<decimal?>(r, columnName))
                //     .FirstOrDefaultAsync() ?? 0;
                //  decimal criticalAdvantageRiderInsured6 = await dbContext.easyhealth_carates
                //.FromSqlRaw(sqlca, insuredAgeSix, ca_si_6)
                //    .Select(r => EF.Property<decimal?>(r, columnName))
                //    .FirstOrDefaultAsync() ?? 0;
                //  decimal? criticalAdvantageRiderBasePremium = criticalAdvantageRiderInsured1 + criticalAdvantageRiderInsured2 + criticalAdvantageRiderInsured3 + criticalAdvantageRiderInsured4 + criticalAdvantageRiderInsured5 + criticalAdvantageRiderInsured6;

                // Critical Advantage Rider Loading
                List<decimal?> adjustedLoadingRates = basicLoadingRates.Select(rate => rate / 100).ToList();
                decimal? criticalAdvantageRiderLoadingInsured1 = GetCARRiderLoadingInsured1(criticalAdvantageList,0, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoadingInsured2 = GetCARRiderLoadingInsured1(criticalAdvantageList,1, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoadingInsured3 = GetCARRiderLoadingInsured1(criticalAdvantageList,2, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoadingInsured4 = GetCARRiderLoadingInsured1(criticalAdvantageList,3, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoadingInsured5 = GetCARRiderLoadingInsured1(criticalAdvantageList,4, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoadingInsured6 = GetCARRiderLoadingInsured1(criticalAdvantageList,5, adjustedLoadingRates);
                decimal? criticalAdvantageRiderLoading = GetCriticalAdvantageRiderLoading(policyType, criticalAdvantageRiderLoadingInsured1, criticalAdvantageRiderLoadingInsured2, criticalAdvantageRiderLoadingInsured3, criticalAdvantageRiderLoadingInsured4, criticalAdvantageRiderLoadingInsured5, criticalAdvantageRiderLoadingInsured6);

                // Critical Advantage Rider Premium
                decimal? criticalAdvRiderBaseLoadingPremium = criticalAdvantageRiderInsured + criticalAdvantageRiderLoading;
                decimal? criticalAdvRiderPremiumLongTermDiscount = criticalAdvRiderBaseLoadingPremium * (longTermDiscount);
                decimal? criticalAdvRiderPremium = criticalAdvRiderBaseLoadingPremium - criticalAdvRiderPremiumLongTermDiscount;

                // Hospital Daily Cash Rider
                string Opt = "N"; //need to calculate 
                decimal? hdcSI = 0;//rider_si_11 
                if (siRiderThreeDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderThreeDataTable.Rows)
                    {
                        Opt = "Y";
                        object siValueObject = itemRow["SIValue"];
                        hdcSI = Convert.ToDecimal(siValueObject);
                    }
                }
                int? insuredageone = int.TryParse(row.txt_insured_age1, out var age1) ? (int?)age1 : null;
                int? insuredagetwo = int.TryParse(row.txt_insured_age2, out var age2) ? (int?)age2 : null;
                int? insuredagethree = int.TryParse(row.txt_insured_age3, out var age3) ? (int?)age3 : null;
                int? insuredagefour = int.TryParse(row.txt_insured_age4, out var age4) ? (int?)age4 : null;
                int? insuredagefive = int.TryParse(row.txt_insured_age5, out var age5) ? (int?)age5 : null;
                int? insuredagesix = int.TryParse(row.txt_insured_age6, out var age6) ? (int?)age6 : null;
                string familysize = row.txt_family;
                if (policyType == "Individual")
                {
                    familysize = "1 Adult";
                };
                //var sqlhdc = $@"
                //SELECT {columnName}
                //FROM easyhealth_hdcrates
                //WHERE age = @p0  AND suminsured = @p1 AND plan = @p2";
                //// Execute the raw SQL query
                //var hdcOpt = await dbContext.easyhealth_hdcrates
                // .FromSqlRaw(sqlhdc, insuredageone, siOne, familysize)
                // .Select(r => EF.Property<decimal?>(r, columnName))
                // .FirstOrDefaultAsync();

                List<decimal?> hdcratesList = new List<decimal?>();
                decimal? hdcRate = 0;
                for (int i = 0; i < noOfMembers; i++)
                {
                    var hdcOpt = hdcRatesTable
                   .Where(roww =>
                       roww.Value is Hashtable rateDetails &&
                       (int)rateDetails["age"] == eldestMember &&
                       (int)rateDetails["suminsured"] == sumInsuredList[i] &&
                       rateDetails["plan"].ToString() == familysize)
                   .Select(roww =>
                       roww.Value is Hashtable details && details[columnName] != null
                       ? Convert.ToDecimal(details[columnName])
                       : (decimal?)null
                   )
                   .FirstOrDefault();
                }

                decimal? hdcRiderPremium1 = await GetHDCRiderPremium1(policyPeriod, insuredageone, hdcSI, familysize, hdcRatesTable);
                decimal? hdcRiderPremium2 = await GetHDCRiderPremium2(policyPeriod, insuredagetwo, hdcSI, familysize, hdcRatesTable);
                decimal? hdcRiderPremium3 = await GetHDCRiderPremium3(policyPeriod, insuredagethree, hdcSI, familysize, hdcRatesTable);
                decimal? hdcRiderPremium4 = await GetHDCRiderPremium4(policyPeriod, insuredagefour, hdcSI, familysize, hdcRatesTable);
                decimal? hdcRiderPremium5 = await GetHDCRiderPremium5(policyPeriod, insuredagefive, hdcSI, familysize, hdcRatesTable);
                decimal? hdcRiderPremium6 = await GetHDCRiderPremium6(policyPeriod, insuredagesix, hdcSI, familysize, hdcRatesTable);
                var hdcRiderPremvalues = new List<decimal?> { hdcRiderPremium1 + hdcRiderPremium2 + hdcRiderPremium3 + hdcRiderPremium4 + hdcRiderPremium5 + hdcRiderPremium6 };
                decimal? hdcRiderPremiumsfinalvalue = hdcRiderPremvalues.Sum();

                decimal? hdcRiderPremium = await GetHDCRiderPremium(policyPeriod, eldestMember, hdcSI, familysize, hdcRatesTable);
                var policytype = row.policy_type;
                decimal? hdcFamilyDiscount;
                if (policytype == "Individual")
                {
                    hdcFamilyDiscount = hdcRiderPremiumsfinalvalue * familyDiscountPRHDC / 100;
                }
                else
                {
                    hdcFamilyDiscount = hdcRiderPremium * familyDiscountPRHDC;
                }

                decimal? hdcLongTermDiscount;
                if (policytype == "Individual")
                {
                    hdcLongTermDiscount = (hdcRiderPremiumsfinalvalue - hdcFamilyDiscount) * longTermDiscount;
                }
                else
                {
                    hdcLongTermDiscount = (hdcRiderPremium - familyDiscountPRHDC) * (longTermDiscount);
                }
                decimal? hdcFinalPremium;
                if (policytype == "Individual")
                {
                    hdcFinalPremium = hdcRiderPremiumsfinalvalue - hdcFamilyDiscount - hdcLongTermDiscount;
                }
                else
                {
                    hdcFinalPremium = (hdcRiderPremium ?? 0) - hdcFamilyDiscount - hdcLongTermDiscount;
                }
                // Individual Personal Accident Rider

                string individualpersonalARopt = "N"; //need to calculate and add the logic hardcoded as of now
                if (siRiderFourDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFourDataTable.Rows)
                    {
                        individualpersonalARopt = "Y";
                    }
                }
                decimal? individualpersonalARSI = GetIndividualPersonalARSI(individualpersonalARopt, sumInsuredList);
                var policystartdate = row.policy_start_date;
                var policyenddate = row.policy_expiry_date;
                decimal? individualpersonalARAmt = GetIndividualPersonalARAmt(individualpersonalARopt, individualpersonalARSI, policystartdate, policyenddate);
                decimal? individualpersonalARLongTermDiscount = individualpersonalARAmt * longTermDiscount;
                decimal? individualPersonalAccidentRiderPremium = individualpersonalARAmt - individualpersonalARLongTermDiscount;

                // Protecter Rider
                string propt = "N";

                if (siRiderFiveDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFiveDataTable.Rows)
                    {
                        propt = "Y";
                        object prsiValueObject = itemRow["SIValue"];

                    }
                }
                decimal? prInsured1 = GetProtectorRiderInsured1(propt,0, sumInsuredList, basePremiumsList);
                decimal? prInsured2 = GetProtectorRiderInsured1(propt,1, sumInsuredList, basePremiumsList);
                decimal? prInsured3 = GetProtectorRiderInsured1(propt,2, sumInsuredList, basePremiumsList);
                decimal? prInsured4 = GetProtectorRiderInsured1(propt,3, sumInsuredList, basePremiumsList);
                decimal? prInsured5 = GetProtectorRiderInsured1(propt,4, sumInsuredList, basePremiumsList);
                decimal? prInsured6 = GetProtectorRiderInsured1(propt,5, sumInsuredList, basePremiumsList);
                decimal? prProtectorRiderPremium = GetProtectorRiderPremium(policyType, prInsured1, prInsured2, prInsured3, prInsured4, prInsured5, prInsured6);

                // Protector Rider Loading
                decimal? prLoadingInsured1 = 0;//GetProtectorRiderLoadingInsured1(prInsured1, basicLoadingRateOne / 100);
                decimal? prLoadingInsured2 = 0; // GetProtectorRiderLoadingInsured2(prInsured2, basicLoadingRateTwo / 100);
                decimal? prLoadingInsured3 = 0;// GetProtectorRiderLoadingInsured3(prInsured3, basicLoadingRateThree / 100);
                decimal? prLoadingInsured4 = 0; // GetProtectorRiderLoadingInsured4(prInsured4, basicLoadingRateFour / 100);
                decimal? prLoadingInsured5 = 0; // GetProtectorRiderLoadingInsured5(prInsured5, basicLoadingRateFive / 100);
                decimal? prLoadingInsured6 = 0; // GetProtectorRiderLoadingInsured6(prInsured6, basicLoadingRateSix / 100);
                decimal? ProtectorRiderLoadingPremium = prLoadingInsured1 + prLoadingInsured2 + prLoadingInsured3 + prLoadingInsured4 + prLoadingInsured5 + prLoadingInsured6;

                // Protector Rider Premium
                decimal? prBaseLoadingPremium = prProtectorRiderPremium + ProtectorRiderLoadingPremium;
                decimal? prFamilyDiscount = prProtectorRiderPremium * (familyDiscountPRHDC / 100);
                decimal? prLongTermDiscount = (prBaseLoadingPremium - prFamilyDiscount) * (longTermDiscount);
                decimal? prpremiumProtectorRiderPremium = prBaseLoadingPremium - prFamilyDiscount - prLongTermDiscount;

                decimal? netPremium = (easyHealthORasePremium + criticalIllnessRiderPremium + hdcFinalPremium + prpremiumProtectorRiderPremium + individualPersonalAccidentRiderPremium + criticalAdvRiderPremium);
                decimal? GST = netPremium * 0.18m;
                decimal? finalPremium = netPremium + GST;
                decimal? Crosscheck = row.num_tot_premium - finalPremium;
                decimal? Crosscheck1 = row.num_tot_premium - finalPremium;
               
                var record = dbContext.premium_validation.FirstOrDefault(item => item.certificate_no == policNo16.ToString());
                if (record != null)
                {
                    record.verified_prem = netPremium ?? 0;
                    record.verified_gst = GST ?? 0 ;
                    record.verified_total_prem = finalPremium ?? 0;
                    dbContext.premium_validation.Update(record);
                }
                else
                {
                    var newRecord = new premium_validation
                    {
                        certificate_no = policyNo.ToString(),
                        verified_prem = netPremium ?? 0,
                        verified_gst = GST ?? 0,
                        verified_total_prem = finalPremium ?? 0
                    };
                    dbContext.premium_validation.Add(newRecord);
                }
                int maxRetries = 5;
                int attempt = 0;
                int baseDelay = 1000; // Base delay in milliseconds
                Random rng = new Random();
                while (attempt < maxRetries)
                {
                    try
                    {
                        await dbContext.SaveChangesAsync();
                        break;  // Exit if successful
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        await entry.ReloadAsync();
                    }
                    catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "40P01")
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                            throw;  // Rethrow if max retries exceeded

                        int delay = baseDelay * (int)Math.Pow(2, attempt) + rng.Next(0, 500);  // Random jitter added
                        await Task.Delay(delay);
                    }
                }

            }

        }

        static DataTable GetRiderSI(DataTable table, string riderName)
        { // Create a new DataTable to hold the results
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("RiderName", typeof(string));
            resultTable.Columns.Add("SIValue", typeof(object));

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName.StartsWith("covername"))
                {
                    // Construct the corresponding SI column name
                    string siColumnName = column.ColumnName.Replace("name", "si");

                    // Check for matching rider name
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[column].ToString() == riderName)
                        {
                            //return row[siColumnName].ToString();
                            // Add the rider name and corresponding SI value to the result table
                            DataRow newRow = resultTable.NewRow();
                            newRow["RiderName"] = riderName;
                            newRow["SIValue"] = Convert.ToDecimal(row[siColumnName]);
                            resultTable.Rows.Add(newRow);
                        }
                    }
                }
            }
            return resultTable;
        }

        public static decimal GetFamilyDiscount(string policyType, int numberOfMemberss)
        {
            if (policyType == "Individual")
            {
                if (numberOfMemberss == 2)
                {
                    return 0.05m; // 5% discount
                }
                else if (numberOfMemberss > 2)
                {
                    return 0.10m; // 10% discount
                }
            }

            return 0.00m; // 0% discount
        }

        public static decimal GetFamilyDiscountPRHDC(string policyType, int numberOfMemberss)
        {
            if (policyType == "Individual" && numberOfMemberss > 1)
            {
                return 010m; // 10% discount
            }
            else
            {
                return 0.00m; // 0% discount
            }
        }

        public static decimal GetLongTermDiscount(string policyPeriod)
        {
            if (policyPeriod == "2 Years")
            {
                return 0.075m; // 7.5%
            }
            else if (policyPeriod == "3 Years")
            {
                return 0.10m; // 10%
            }
            else
            {
                return 0.00m; // 0%
            }
        }

        public static string GetColumnNameForPolicyPeriod(string policyPeriod)
        {
            return policyPeriod switch
            {
                "1 Year" => "one_year",
                "2 Years" => "two_years",
                "3 Years" => "three_years",
                _ => null,
            };
        }
        
        public decimal? GetBasePremium(string condition, List<decimal?> values)
        {
            decimal? sum = values.Sum();
            decimal? max = values.Max();
            decimal? difference = sum - max;
            decimal? percentageAdjustment = difference * 0.45m;

            if (condition == "Individual")
            {
                return sum;
            }
            else
            {
                return max + percentageAdjustment;
            }
        }

        static decimal? GetBasePremLoadingInsured1(int i, List<decimal?> basePremiumList, List<decimal?> baseLoadingRates)
        {
            if (basePremiumList != null && baseLoadingRates != null &&
         i >= 0 && i < basePremiumList.Count && i < baseLoadingRates.Count)
            {
                return basePremiumList[i] * baseLoadingRates[i] / 100;

            }
            else
            {
                return 0;
            }
        }
        
        //static decimal? GetBasePremLoadingInsured2(decimal? basepremium2, decimal? base_loading_insured_2)
        //{

        //    if (base_loading_insured_2.HasValue && base_loading_insured_2 != 0)
        //    {
        //        return basepremium2 * base_loading_insured_2 / 100;

        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        
        //static decimal? GetBasePremLoadingInsured3(decimal? basepremium3, decimal? base_loading_insured_3)
        //{
        //    if (base_loading_insured_3.HasValue && base_loading_insured_3 != 0)
        //    {
        //        return basepremium3 * base_loading_insured_3 / 100;

        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        
        //static decimal? GetBasePremLoadingInsured4(decimal? basepremium4, decimal? base_loading_insured_4)
        //{
        //    if (base_loading_insured_4.HasValue && base_loading_insured_4 != 0)
        //    {
        //        return basepremium4 * base_loading_insured_4 / 100;

        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        
        //static decimal? GetBasePremLoadingInsured5(decimal? basepremium5, decimal? base_loading_insured_5)
        //{
        //    if (base_loading_insured_5.HasValue && base_loading_insured_5 != 0)
        //    {
        //        return basepremium5 * base_loading_insured_5 / 100;

        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        
        //static decimal? GetBasePremLoadingInsured6(decimal? basepremium6, decimal? base_loading_insured_6)
        //{
        //    if (base_loading_insured_6.HasValue && base_loading_insured_6 != 0)
        //    {
        //        return basepremium6 * base_loading_insured_6 / 100;

        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        
        static decimal? GetCARRiderLoadingInsured1(List<decimal?> sumInsuredList, int i, List<decimal?> baseLoadingRates)
        {
            try
            {
                if (sumInsuredList[i].HasValue && baseLoadingRates[i].HasValue)
                {
                    decimal loadingRate = baseLoadingRates[i].Value;

                    decimal loadingAmount = sumInsuredList[i].Value * loadingRate;
                    return loadingAmount;
                }
                else
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }
        
        //static decimal? GetCARRiderLoadingInsured2(decimal? si_2, decimal? loading_per_insured_2)
        //{
        //    try
        //    {
        //        if (si_2.HasValue && loading_per_insured_2.HasValue)
        //        {
        //            decimal loadingRate = loading_per_insured_2.Value;

        //            decimal loadingAmount = si_2.Value * loadingRate;
        //            return loadingAmount;
        //        }
        //        else
        //        {

        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating loading amount: {ex.Message}");
        //        return 0;
        //    }
        //}

        //static decimal? GetCARRiderLoadingInsured3(decimal? si_3, decimal? loading_per_insured_3)
        //{
        //    try
        //    {
        //        if (si_3.HasValue && loading_per_insured_3.HasValue)
        //        {
        //            decimal loadingRate = loading_per_insured_3.Value;

        //            decimal loadingAmount = si_3.Value * loadingRate;
        //            return loadingAmount;
        //        }
        //        else
        //        {

        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating loading amount: {ex.Message}");
        //        return 0;
        //    }
        //}

        //static decimal? GetCARRiderLoadingInsured4(decimal? si_4, decimal? loading_per_insured_4)
        //{
        //    try
        //    {
        //        if (si_4.HasValue && loading_per_insured_4.HasValue)
        //        {
        //            decimal loadingRate = loading_per_insured_4.Value;

        //            decimal loadingAmount = si_4.Value * loadingRate;
        //            return loadingAmount;
        //        }
        //        else
        //        {

        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating loading amount: {ex.Message}");
        //        return 0;
        //    }
        //}

        //static decimal? GetCARRiderLoadingInsured5(decimal? si_5, decimal? loading_per_insured_5)
        //{
        //    try
        //    {
        //        if (si_5.HasValue && loading_per_insured_5.HasValue)
        //        {
        //            decimal loadingRate = loading_per_insured_5.Value;

        //            decimal loadingAmount = si_5.Value * loadingRate;
        //            return loadingAmount;
        //        }
        //        else
        //        {

        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating loading amount: {ex.Message}");
        //        return 0;
        //    }
        //}
        
        //static decimal? GetCARRiderLoadingInsured6(decimal? si_6, decimal? loading_per_insured_6)
        //{
        //    try
        //    {
        //        if (si_6.HasValue && loading_per_insured_6.HasValue)
        //        {
        //            decimal loadingRate = loading_per_insured_6.Value;

        //            decimal loadingAmount = si_6.Value * loadingRate;
        //            return loadingAmount;
        //        }
        //        else
        //        {

        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating loading amount: {ex.Message}");
        //        return 0;
        //    }
        //}
        
        static decimal? GetCriticalAdvantageRiderLoading(string policyType, decimal? criticalAdvantageRiderLoadingInsured1, decimal? criticalAdvantageRiderLoadingInsured2, decimal? criticalAdvantageRiderLoadingInsured3, decimal? criticalAdvantageRiderLoadingInsured4, decimal? criticalAdvantageRiderLoadingInsured5, decimal? criticalAdvantageRiderLoadingInsured6)
        {
            var premiums = new List<decimal?> { criticalAdvantageRiderLoadingInsured1, criticalAdvantageRiderLoadingInsured2, criticalAdvantageRiderLoadingInsured3, criticalAdvantageRiderLoadingInsured4, criticalAdvantageRiderLoadingInsured5, criticalAdvantageRiderLoadingInsured6 };

            decimal? totalPremium = premiums.Sum();

            decimal? maxPremium = premiums.Max();

            if (policyType == "Individual")
            {
                return totalPremium;
            }
            else
            {
                return maxPremium + (totalPremium - maxPremium) * 0.45m;
            }

        }
        
        public async Task<decimal?> GetHDCRiderPremium1(string policyperiod, int? insuredageone, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            if (string.IsNullOrEmpty(column))
            {
                return 0;
            }
            var rate = hdcRatesTable
                    .Where(roww =>
                       roww.Value is Hashtable rateDetails &&
                       rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredageone &&
                       rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                       rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                    )
                    .Select(roww =>
                       roww.Value is Hashtable details && details[column] != null
                       ? Convert.ToDecimal(details[column])
                       : (decimal?)null
                    )
                    .FirstOrDefault();

            return rate ?? 0;

        }
        
        public async Task<decimal?> GetHDCRiderPremium2(string policyperiod, int? insuredagetwo, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                     .Where(roww =>
                        roww.Value is Hashtable rateDetails &&
                        rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredagetwo &&
                        rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                        rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                     )
                     .Select(roww =>
                        roww.Value is Hashtable details && details[column] != null
                        ? Convert.ToDecimal(details[column])
                        : (decimal?)null
                     )
                     .FirstOrDefault();

            return rate ?? 0;

        }
        
        public async Task<decimal?> GetHDCRiderPremium3(string policyperiod, int? insuredagethree, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                    .Where(roww =>
                       roww.Value is Hashtable rateDetails &&
                       rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredagethree &&
                       rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                       rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                    )
                    .Select(roww =>
                       roww.Value is Hashtable details && details[column] != null
                       ? Convert.ToDecimal(details[column])
                       : (decimal?)null
                    )
                    .FirstOrDefault();

            return rate ?? 0;
        }
        
        public async Task<decimal?> GetHDCRiderPremium4(string policyperiod, int? insuredagefour, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                   .Where(roww =>
                      roww.Value is Hashtable rateDetails &&
                      rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredagefour &&
                      rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                      rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                   )
                   .Select(roww =>
                      roww.Value is Hashtable details && details[column] != null
                      ? Convert.ToDecimal(details[column])
                      : (decimal?)null
                   )
                   .FirstOrDefault();

            return rate ?? 0;

        }
        
        public async Task<decimal?> GetHDCRiderPremium5(string policyperiod, int? insuredagefive, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                   .Where(roww =>
                      roww.Value is Hashtable rateDetails &&
                      rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredagefive &&
                      rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                      rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                   )
                   .Select(roww =>
                      roww.Value is Hashtable details && details[column] != null
                      ? Convert.ToDecimal(details[column])
                      : (decimal?)null
                   )
                   .FirstOrDefault();


            return rate ?? 0;

        }
        
        public async Task<decimal?> GetHDCRiderPremium6(string policyperiod, int? insuredagesix, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                   .Where(roww =>
                      roww.Value is Hashtable rateDetails &&
                      rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == insuredagesix &&
                      rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                      rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                   )
                   .Select(roww =>
                      roww.Value is Hashtable details && details[column] != null
                      ? Convert.ToDecimal(details[column])
                      : (decimal?)null
                   )
                   .FirstOrDefault();

            return rate ?? 0;

        }
        
        public async Task<decimal?> GetHDCRiderPremium(string policyperiod, int? eldestMember, decimal? hdcSI, string familysize, Dictionary<string, Hashtable> hdcRatesTable)
        {
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var rate = hdcRatesTable
                  .Where(roww =>
                     roww.Value is Hashtable rateDetails &&
                     rateDetails["age"] != null && (rateDetails["age"] as int? ?? 0) == eldestMember &&
                     rateDetails["suminsured"] != null && (rateDetails["suminsured"] as decimal? ?? 0) == hdcSI &&
                     rateDetails["plan"] != null && rateDetails["plan"].ToString() == familysize
                  )
                  .Select(roww =>
                     roww.Value is Hashtable details && details[column] != null
                     ? Convert.ToDecimal(details[column])
                     : (decimal?)null
                  )
                  .FirstOrDefault();

            return rate ?? 0;

        }
        
        static decimal? GetIndividualPersonalARSI(string? individualpersonalARopt, List<decimal?> sumInsuredList)
        {
            if (individualpersonalARopt == "N" || sumInsuredList[0] == null) return 0;

            var isApplicable = individualpersonalARopt.Equals("Y", StringComparison.OrdinalIgnoreCase);

            var sumInsured = sumInsuredList[1] * 5 < 10000000 ? sumInsuredList[1] * 5 : 10000000;
            return isApplicable ? sumInsured : 0;
        }
        
        static decimal? GetIndividualPersonalARAmt(string? individualpersonalARopt, decimal? individualpersonalARSI, DateTime? policy_start_date, DateTime? policy_end_date)
        {
            if (individualpersonalARopt == "N" || individualpersonalARSI == 0 || policy_start_date == null || policy_end_date == null) return 0;

            var dailyRate = individualpersonalARopt.Equals("Y", StringComparison.OrdinalIgnoreCase) ? individualpersonalARSI.Value * 0.99m / 1000 : 0;

            var years = (policy_end_date.Value.Year - policy_start_date.Value.Year) -
                        ((policy_end_date.Value.DayOfYear < policy_start_date.Value.DayOfYear) ? 1 : 0);
            return dailyRate * years;
        }
        
        static decimal? GetProtectorRiderInsured1(string propt, int i, List<decimal?> sumInsuredList, List<decimal?> basePremiumList)
        {
            try
            {
                if (propt == "Y")
                {
                    if (sumInsuredList[i] > 500000)
                    {
                        return basePremiumList[i] * 0.075m;
                    }
                    else
                    {
                        return basePremiumList[i] * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        //static decimal? GetProtectorRiderInsured2(string propt, decimal? si_2, decimal? basePremiumInsured2)
        //{
        //    try
        //    {
        //        if (propt == "Y")
        //        {
        //            if (si_2 > 500000)
        //            {
        //                return basePremiumInsured2 * 0.075m;
        //            }
        //            else
        //            {
        //                return basePremiumInsured2 * 0.10m;
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        //static decimal? GetProtectorRiderInsured3(string propt, decimal? si_3, decimal? basePremiumInsured3)
        //{
        //    try
        //    {
        //        if (propt == "Y")
        //        {
        //            if (si_3 > 500000)
        //            {
        //                return basePremiumInsured3 * 0.075m;
        //            }
        //            else
        //            {
        //                return basePremiumInsured3 * 0.10m;
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        //static decimal? GetProtectorRiderInsured4(string propt, decimal? si_4, decimal? basePremiumInsured4)
        //{
        //    try
        //    {
        //        if (propt == "Y")
        //        {
        //            if (si_4 > 500000)
        //            {
        //                return basePremiumInsured4 * 0.075m;
        //            }
        //            else
        //            {
        //                return basePremiumInsured4 * 0.10m;
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        //static decimal? GetProtectorRiderInsured5(string propt, decimal? si_5, decimal? basePremiumInsured5)
        //{
        //    try
        //    {
        //        if (propt == "Y")
        //        {
        //            if (si_5 > 500000)
        //            {
        //                return basePremiumInsured5 * 0.075m;
        //            }
        //            else
        //            {
        //                return basePremiumInsured5 * 0.10m;
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        //static decimal? GetProtectorRiderInsured6(string propt, decimal? si_6, decimal? basePremiumInsured6)
        //{
        //    try
        //    {
        //        if (propt == "Y")
        //        {
        //            if (si_6 > 500000)
        //            {
        //                return basePremiumInsured6 * 0.075m;
        //            }
        //            else
        //            {
        //                return basePremiumInsured6 * 0.10m;
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        static decimal? GetProtectorRiderPremium(string policyType, decimal? prInsured1, decimal? prInsured2, decimal? prInsured3, decimal? prInsured4, decimal? prInsured5, decimal? prInsured6)
        {
            var premiums = new List<decimal?> { prInsured1, prInsured2, prInsured3, prInsured4, prInsured5, prInsured6 };

            decimal? totalPremium = premiums.Sum();
            decimal? maxPremium = premiums.Max();

            if (policyType == "Individual")
            {
                return totalPremium;
            }
            else
            {
                return maxPremium + (totalPremium - maxPremium) * 0.45m;
            }
        }
        public async Task<Dictionary<string, Hashtable>> GetBaseRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();

            var rates = await dbContext.easyhealth_baserates.ToListAsync().ConfigureAwait(false);

            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.si}-{rate.age}-{rate.tier}-{rate.variant}";
                var rateDetails = new Hashtable
                {
                    { "si", rate.si },
                    { "age", rate.age },
                    { "tier", rate.tier },
                    { "variant", rate.variant },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years }
                };

                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }

        public async Task<Dictionary<string, Hashtable>> GetHdcRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();

            var rates = await dbContext.easyhealth_hdcrates.ToListAsync().ConfigureAwait(false);

            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.age}-{rate.suminsured}-{rate.plan}";
                var rateDetails = new Hashtable
                {
                    { "age", rate.age },
                    { "suminsured", rate.suminsured },
                    { "plan", rate.plan },
                    { "age_band", rate.age_band },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years }

                };

                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }

        public async Task<Dictionary<string, Hashtable>> GetCARatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();

            var rates = await dbContext.easyhealth_carates.ToListAsync().ConfigureAwait(false);

            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.age}-{rate.si}";
                var rateDetails = new Hashtable
                {
                    { "age", rate.age },
                    { "si", rate.si },
                    { "age_band", rate.age_band },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years }
                };

                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }

        public async Task<Dictionary<string, Hashtable>> GetCiRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();

            var rates = await dbContext.easyhealth_cirates.ToListAsync().ConfigureAwait(false);

            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.age}-{rate.si}";
                var rateDetails = new Hashtable
                {
                    { "age", rate.age },
                    { "si", rate.si },
                    { "age_band", rate.age_band },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years }
                };

                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }
        public List<List<string>> FetchNewBatchIds(NpgsqlConnection postgresConnection)
        {
            string status = ConfigurationManager.AppSettings["Status"];
            var sqlSource = $"SELECT ir.certificate_no, ir.product_code FROM ins.idst_renewal_data_rgs ir INNER JOIN ins.rne_healthtab ht" +
                $" ON ir.certificate_no = ht.policy_number WHERE ir.rn_generation_status = @Status AND ht.prod_code in (2806)";
            var sourceResults = postgresConnection.Query(sqlSource, new { Status = status });
            var sourceBatchIds = new List<List<string>>();
            foreach (var result in sourceResults)
            {
                var batchInfo = new List<string> { result.certificate_no, result.product_code.ToString() };
                sourceBatchIds.Add(batchInfo);
            }
            return sourceBatchIds;
        }

        public class IdstData
        {
            public string certificate_no { get; set; }
            public decimal? loading_per_insured1 { get; set; }
            public decimal? loading_per_insured2 { get; set; }
            public decimal? loading_per_insured3 { get; set; }
            public decimal? loading_per_insured4 { get; set; }
            public decimal? loading_per_insured5 { get; set; }
            public decimal? loading_per_insured6 { get; set; }

        }
    }
}
