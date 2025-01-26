using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class botcasetbl
    {
        [Key]
        public string certificate_no { get; set; }
        public string? product_code { get; set; }
        public string? product_name { get; set; }
        public string? status { get; set; }
        public decimal? old_premium { get; set; }
        public decimal? new_premium { get; set; }
        public string? popup_remark { get; set; }
        public string? proposal { get; set; }
        public string? gc_global { get; set; }

    }
}
