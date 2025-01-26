using System.Text.Json.Serialization;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class PolicyDetails
    {
        public string? certificate_no { get; set; }
        public string? account_code_bi { get; set; }
        public string? overriding_agent_name { get; set; }
        public string? product_code { get; set; }
        public string? product_name { get; set; }
        public DateTime? entry_date { get; set; }
        public DateTime? effective_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public DateTime? policy_renewal_on_expirydate_1 { get; set; }
        public DateTime? renewal_policy_expiry_date { get; set; }
        public string? pol_branch_code { get; set; }
        public string? branch_name { get; set; }
        public string? branch { get; set; }
        public decimal? branch_code { get; set; }
        public string? business_group { get; set; }
        public string? vertical { get; set; }
        public string? sub_vertical { get; set; }
        public string? bdm_name { get; set; }
        public string? sse_name { get; set; }
        public string? lg_code { get; set; }
        public string? name_of_insured { get; set; }
        public long? pin_code { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? customer_id { get; set; }
        public string? dl_code { get; set; }
        public string? renewable_nonrenewable_product { get; set; }
        public string? final_remarks { get; set; }
        public string? dispatch_status { get; set; }
        public string? new_sub_vertical { get; set; }
        public string? new_business_group { get; set; }
        public string? new_dealer_code { get; set; }
        public string? new_intermediary_name { get; set; }
        public string? new_bdm_name { get; set; }
        public string? new_sse_name { get; set; }
        public string? despatch_branch_name { get; set; }
        public string? bosg_name { get; set; }
        public string? new_bdm_code { get; set; }
        public string? bdm_mail_id { get; set; }
        public string? sse_mail_id { get; set; }
        public string? hdfc_bank_location_code { get; set; }
        public string? vertical_recepient { get; set; }
        public string? active_inactive { get; set; }
        public string? last_year_ncb { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? verified_prem { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? verified_gst { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? verified_total_prem { get; set; }
        public string? rn_generation_status { get; set; }
        public int? claim_count { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured1 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured1 { get; set; }
        public DateTime? dat_customer_reference_date1 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured2 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured2 { get; set; }// changes
        public DateTime? dat_customer_reference_date2 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured3 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured3 { get; set; }
        public DateTime? dat_customer_reference_date3 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured4 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured4 { get; set; }
        public DateTime? dat_customer_reference_date4 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured5 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured5 { get; set; }
        public DateTime? dat_customer_reference_date5 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured6 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured6 { get; set; }
        public DateTime? dat_customer_reference_date6 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured7 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured7 { get; set; }
        public DateTime? dat_customer_reference_date7 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured8 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured8 { get; set; }
        public DateTime? dat_customer_reference_date8 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured9 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured9 { get; set; }
        public DateTime? dat_customer_reference_date9 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured10 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured10 { get; set; }
        public DateTime? dat_customer_reference_date10 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured11 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured11 { get; set; }
        public DateTime? dat_customer_reference_date11 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_per_insured12 { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? loading_amount_insured12 { get; set; }
        public DateTime? dat_customer_reference_date12 { get; set; }
        public string? renewal_flag3 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email3 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile3 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg3 { get; set; } // character varying(200) COLLATE pg_catalog."default"        
        public string? ind_corp_flag4 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_4 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag4 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email4 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile4 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg4 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? ind_corp_flag5 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_5 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag5 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email5 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile5 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg5 { get; set; } // character varying(200) COLLATE pg_catalog."default"     
        public string? ind_corp_flag6 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_6 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag6 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email6 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile6 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg6 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? ind_corp_flag7 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_7 { get; set; } // character varying(200) COLLATE pg_catalog."default"    
        public string? renewal_flag7 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email7 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile7 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg7 { get; set; } // character varying(200) COLLATE pg_catalog."default"      
        public string? ind_corp_flag8 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_8 { get; set; } // character varying(200) COLLATE pg_catalog."default"        
        public string? renewal_flag8 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email8 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile8 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg8 { get; set; } // character varying(200) COLLATE pg_catalog."default"
        public string? ind_corp_flag9 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_9 { get; set; } // character varying(200) COLLATE pg_catalog."default"        
        public string? renewal_flag9 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email9 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile9 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg9 { get; set; } // character varying(200) COLLATE pg_catalog."default"     
        public string? ind_corp_flag10 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_10 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag10 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email10 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile10 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg10 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? ind_corp_flag11 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_11 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag11 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email11 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile11 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg11 { get; set; } // character varying(200) COLLATE pg_catalog."default"        
        public string? ind_corp_flag12 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_12 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? renewal_flag12 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email12 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile12 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg12 { get; set; } // character varying(200) COLLATE pg_catalog."default"        
        public string? ind_corp_flag1 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_1 { get; set; } // character varying(200) COLLATE pg_catalog."default"      
        public string? renewal_flag1 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email1 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile1 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg1 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? ind_corp_flag2 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_2 { get; set; } // character varying(200) COLLATE pg_catalog."default"      
        public string? renewal_flag2 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? email2 { get; set; } // character varying(100) COLLATE pg_catalog."default"
        public string? mobile2 { get; set; } // character varying(15) COLLATE pg_catalog."default"
        public string? errmsg2 { get; set; } // character varying(200) COLLATE pg_catalog."default"       
        public string? ind_corp_flag3 { get; set; } // character varying COLLATE pg_catalog."default"
        public string? insured1_information2_3 { get; set; } // character varying(200) COLLATE pg_catalog."default"
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? service_tax { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? renewal_net_premium { get; set; }
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? total_premium_payable { get; set; }
        public string? txt_pincode { get; set; }
        public string? txt_citydistrict { get; set; }
        public string? txt_state { get; set; }
        public Insured_details[] Insured_details { get; set; }
        public Motor_details Motor_details { get; set; }
        public Upsell_values[] Upsell_values { get; set; }
        public Base_Premiums[] Base_Premiums { get; set; }
        public Rider_Premiums[] Rider_Premiums { get; set; }
    }
    public class Insured_details
    {
        public string name { get; set; }
        public string age { get; set; }
        public string relation { get; set; }
        public string gender { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }
    public class Motor_details
    {
        public string? policy_holders_name { get; set; }
        public decimal? idv { get; set; }
        public string? engine_no { get; set; }
        public string? rto_location { get; set; }
        public string? chassis_no { get; set; }
        public string? registration_no { get; set; }
        public string? vehicle_make { get; set; }
        public string? vehicle_model { get; set; }
        public string? cubic_capacity { get; set; }
        public string zone { get; set; }
        public string year_of_manufacture { get; set; }
    }
    public class Base_Premiums
    {
        public decimal? insuredloadingamt { get; set; }
        public decimal? basepremiumsinsured { get; set; }
        public decimal? basepremiumvaluerg { get; set; }
        public decimal? loading_amt_rg { get; set; }
    }

    public class Upsell_values
    {
        public string? upselltype_gc { get; set; }
        public decimal? upsell_premium_gc { get; set; }
        public string? upselltype_rg { get; set; }
        public decimal? upsell_premium_rg { get; set; }

    }
    public class Rider_Premiums
    {
        public string? covername { get; set; }
        public decimal? coverpremium { get; set; }
        public string? covername_rg { get; set; }
        public decimal? coverpremium_rg { get; set; }

    }
}
