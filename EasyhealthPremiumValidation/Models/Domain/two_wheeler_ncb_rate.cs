using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class two_wheeler_ncb_rate
    {
        [Key]
        public int sl_no {  get; set; }
        public int age { get; set; }
        public decimal claim_count { get; set; }
        public decimal ncb_percentage { get; set; }
    }
}
