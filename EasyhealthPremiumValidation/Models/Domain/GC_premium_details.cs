using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class gc_premium_details
    {
        public long batch_id { get; set; }
        public int tot_premium { get; set; }
        public int net_premium { get; set; }
        public int gst { get; set; }
        public int premium_insured_1 { get; set; }
        public int premium_insured_2 { get; set; }
        public int premium_insured_3 { get; set; }
        public int premium_insured_4 { get; set; }
        public int premium_insured_5 { get; set; }
        public int premium_insured_6 { get; set; }
        public int premium_insured_7 { get; set; }
        public int premium_insured_8 { get; set; }
        public int premium_insured_9 { get; set; }
        public int premium_insured_10 { get; set; }
        public int premium_insured_11 { get; set; }
        public int premium_insured_12 { get; set; }
        public int upsell_premium_1 { get; set; }
        public int upsell_premium_2 { get; set; }
        public int upsell_premium_3 { get; set; }
        public int upsell_gst_1 { get; set; }
        public int upsell_gst_2 { get; set; }
        public int upsell_gst_3 { get; set; }
        [Key]
        public long policy_no { get; set; }
      
    }
}
