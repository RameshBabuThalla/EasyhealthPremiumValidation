using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
   
        public class dwh
        {
            public long cn_no { get; set; }
            [Key]
            public long policy_no { get; set; }
            public decimal endorsement_no { get; set; }
            public decimal certificate_no { get; set; }
            public decimal? account_code_bi { get; set; }
            public string overriding_agent_name { get; set; }
            public string producer_overriding_agent_name { get; set; }
            public decimal product_code { get; set; }
            public string product_name { get; set; }
            public string name_of_policy { get; set; }
            public DateTime entry_date { get; set; }
            public DateTime effective_date { get; set; }
            public DateTime expiry_date { get; set; }
            public DateTime policy_renewal_on_expirydate_1 { get; set; }
            public DateTime renewal_policy_expiry_date { get; set; }
            public decimal sum_insured_including { get; set; }
            public decimal expiring_policy_gross_premium { get; set; }
            public decimal expiring_policy_premium { get; set; }
            public decimal renewal_sum_insured { get; set; }
            public decimal renewal_gross_premium { get; set; }
            public decimal renewal_premium_includingst { get; set; }
            public decimal renewal_ncb { get; set; }
            public decimal pol_branch_code { get; set; }
            public string branch_name { get; set; }
            public string branch { get; set; }
            public decimal branch_Code { get; set; }
            public string business_group { get; set; }
            public string vertical { get; set; }
            public string sub_vertical { get; set; }
            public string bdn_name { get; set; }
            public string sse_name { get; set; }
            public string lg_code { get; set; }
            public string los_code { get; set; }
            public string bdr_code { get; set; }
            public string name_of_insured { get; set; }
            public string pin_code { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public decimal home_no { get; set; }
            public decimal office_no { get; set; }
            public decimal claims_amount_paid { get; set; }
            public decimal osclaim_amount_reserves { get; set; }
            public string occupancy_details_project_type { get; set; }
            public string type_of_commodity { get; set; }
            public string type_of_cargo { get; set; }
            public string equipment_details { get; set; }
            public decimal year_of_manufacturing { get; set; }
            public string tac_code { get; set; }
            public decimal customer_id { get; set; }
            public decimal hdfc_bank_location_code { get; set; }
            public string di_code { get; set; }
            public string vehicle_make { get; set; }
            public string model_desp { get; set; }
            public string engine_no { get; set; }
            public string chassis_no { get; set; }
            public string registration_no { get; set; }
            public decimal renewal_net_premium { get; set; }
            public decimal service_tax { get; set; }
            public decimal total_amount_payable { get; set; }
            public decimal renewal_idv_sum_insured { get; set; }
            public decimal ncb_rate_cb { get; set; }
            public string renewable_nonrenewable_product { get; set; }
            public string loan_account_number { get; set; }
            public string lob { get; set; }
            public string system { get; set; }
            public string status_accept_decline_case { get; set; }
            public string? final_remarks { get; set; }
            public string? dispatch_status { get; set; }
            public string dispatched { get; set; }
            public DateTime customer_dispatch_date { get; set; }
            public DateTime channel_dispatch_date { get; set; }
            public string? awb { get; set; }
            public string new_sub_vertical { get; set; }
            public string new_business_group { get; set; }
            public string? active_inactive { get; set; }
            public decimal new_dealer_code { get; set; }
            public string new_intermediary_name { get; set; }
            public string new_bdm_name { get; set; }
             public string expr1083 { get; set; }
             public string new_sse_name { get; set; }
            public string despatch_branch_name { get; set; }
            public string bosg_name { get; set; }
            public string channel_partner_sub_type { get; set; }
            public string new_sse_code { get; set; }
            public string new_bdm_code { get; set; }
            public string bank_cust_id { get; set; }
            public decimal? age_discount { get; set; }
            public decimal profession_discount { get; set; }
            public decimal ncb_premium_desc { get; set; }
            public decimal zero_dep_desc { get; set; }
            public decimal zero_dep_prem_desc { get; set; }
            public decimal loss_down_protection_prem { get; set; }
            public decimal loss_dwntime_service_tax { get; set; }
            public decimal loss_dwntime_prem_payable { get; set; }
            public decimal ncb_protection_prem { get; set; }
            public decimal ncb_protection_st { get; set; }
            public decimal ncb_protection_prem_payable { get; set; }
            public decimal engine_gearbox_prem { get; set; }
            public decimal engine_gearbox_st { get; set; }
            public decimal engine_gearbox_prem_payable { get; set; }
            public decimal cost_of_consumables_prm { get; set; }
            public decimal cost_consumable_st { get; set; }
            public decimal cost_consumable_prem_payable { get; set; }
            public string bdm_mail_id { get; set; }
            public string sse_mail_id { get; set; }
            public string pol_company_id { get; set; }
            public string company_name { get; set; }
            public decimal policy_no1 { get; set; }

        
             public string rn_generation_status { get; set; }

    }
    
}
