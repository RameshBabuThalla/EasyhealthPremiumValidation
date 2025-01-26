using HERGPremiumValidationSchedular.Data;
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
using System.Configuration;
using Microsoft.Extensions.Logging;
using HERG_HERGPremiumValidationSchedularAPI_Services.Models.Domain;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Bibliography;

namespace HERGPremiumValidationSchedular.BussinessLogic
{
    public class NewBatchId
    {
       
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
    }
}
