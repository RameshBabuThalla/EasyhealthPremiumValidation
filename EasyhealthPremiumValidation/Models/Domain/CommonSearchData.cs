using System.Text.Json;
using System.Text.Json.Serialization;

namespace HERGPremiumValidationSchedular.Models.Domain
{

    public class CommonSearchData
    {       
            public string? policy_number { get; set; }
            public string? lob { get; set; }
            public string? product_name { get; set; }
            public string? product_code { get; set; }
            public DateTime? entry_date { get; set; }
            public DateTime? expiry_date { get; set; }
            public string? vertical { get; set; }
            public string? new_intermediary_name { get; set; }
            public string? new_dealer_code { get; set; }
            public string? renewal_sum_insured { get; set; }
            [JsonConverter(typeof(DecimalConverter))]
            public decimal? num_net_premium { get; set; }
            [JsonConverter(typeof(DecimalConverter))]
            public decimal? num_service_tax { get; set; }
            [JsonConverter(typeof(DecimalConverter))]
            public decimal? num_tot_premium { get; set; }
            //public string total_amount_payable { get; set; }
            public string? despatch_branch_name { get; set; }
            public string? new_bdm_name { get; set; }
            public string? new_bdm_code { get; set; }
            public string? new_sse_name { get; set; }
            public string? new_sse_code { get; set; }
            public string? rn_generation_status { get; set; }
            public string? customername { get; set; } 
            public string? jobid { get; set; }
             public string? error_description { get; set; }
    }
    public class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDecimal();
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("0.00"));
        }
    }
}
