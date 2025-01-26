using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models{

    public class koti_ipa_rates
    {
        public string  plan_name{ get; set; }
        public int plan_code { get; set; }
        public string sub_plan_name { get; set; }
        public double total_premiumself_pretax { get; set; }

    }
}