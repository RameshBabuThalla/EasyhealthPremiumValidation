using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class idst_renewal_data_rgs
    {
        [Key]
        public string certificate_no { get; set; }
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
        [ConcurrencyCheck]
        public string? final_remarks { get; set; }
        [ConcurrencyCheck]
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
        [ConcurrencyCheck]
        public decimal? verified_prem { get; set; }
        [ConcurrencyCheck]
        public decimal? verified_gst { get; set; }
        [ConcurrencyCheck]
        public decimal? verified_total_prem { get; set; }
        [ConcurrencyCheck]
        public string? rn_generation_status { get; set; }
        public int? claim_count { get; set; }
        public decimal? loading_per_insured1 { get; set; }
        public decimal? loading_amount_insured1 { get; set; }
        public DateTime? dat_customer_reference_date1 { get; set; }
        public decimal? loading_per_insured2 { get; set; }
        public decimal? loading_amount_insured2 { get; set; }
        public DateTime? dat_customer_reference_date2 { get; set; }
        public decimal? loading_per_insured3 { get; set; }
        public decimal? loading_amount_insured3 { get; set; }
        public DateTime? dat_customer_reference_date3 { get; set; }
        public decimal? loading_per_insured4 { get; set; }
        public decimal? loading_amount_insured4 { get; set; }
        public DateTime? dat_customer_reference_date4 { get; set; }
        public decimal? loading_per_insured5 { get; set; }
        public decimal? loading_amount_insured5 { get; set; }
        public DateTime? dat_customer_reference_date5 { get; set; }
        public decimal? loading_per_insured6 { get; set; }
        public decimal? loading_amount_insured6 { get; set; }
        public DateTime? dat_customer_reference_date6 { get; set; }
        public decimal? loading_per_insured7 { get; set; }
        public decimal? loading_amount_insured7 { get; set; }
        public DateTime? dat_customer_reference_date7 { get; set; }
        public decimal? loading_per_insured8 { get; set; }
        public decimal? loading_amount_insured8 { get; set; }
        public DateTime? dat_customer_reference_date8 { get; set; }
        public decimal? loading_per_insured9 { get; set; }
        public decimal? loading_amount_insured9 { get; set; }
        public DateTime? dat_customer_reference_date9 { get; set; }
        public decimal? loading_per_insured10 { get; set; }
        public decimal? loading_amount_insured10 { get; set; }
        public DateTime? dat_customer_reference_date10 { get; set; }
        public decimal? loading_per_insured11 { get; set; }
        public decimal? loading_amount_insured11 { get; set; }
        public DateTime? dat_customer_reference_date11 { get; set; }
        public decimal? loading_per_insured12 { get; set; }
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
        [ConcurrencyCheck]
        public string? error_description { get; set; } // added for reconciliation 
    }
}
