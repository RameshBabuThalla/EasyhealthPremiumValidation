using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class PremiumUpload
    {
        [Key]
        public string policy_no { get; set; }
        public string? endorsement_no { get; set; }
        public decimal? sum_insured_including { get; set; }
        public decimal? expiring_policy_gross_premium { get; set; }
        public decimal? renewal_net_premium { get; set; }
        public decimal? service_tax { get; set; }
        public decimal? total_amount_payable { get; set; }
        public decimal? renewal_idv_sum_insured { get; set; }
        public decimal? ncb_rate_cb { get; set; }
        public string? final_remarks { get; set; }
        public string? new_dealer_code { get; set; }
        public string? hli_policy_no { get; set; }
        public string? application_no { get; set; }
        public string? hdfc_life_premium { get; set; }
        public string? hehi_total_premium { get; set; }
        public string? upsell_type1 { get; set; }
        public string? upsell_value1 { get; set; }
        public decimal? upsell_premium1 { get; set; }
        public string? upsell_type2 { get; set; }
        public string? os_sum_insured { get; set; }
        public string? os_premium_with_gst { get; set; }

    }
}
