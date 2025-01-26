using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class premium_validation
    {
        [Key]
        public string certificate_no { get; set; }
        public decimal? verified_prem { get; set; }
        public decimal? verified_gst { get; set; }
        public decimal? verified_total_prem { get; set; }
        public string? rn_generation_status { get; set; }
        public string? final_remarks { get; set; }
        public string? dispatch_status { get; set; }
        public string? error_description { get; set; }
        [ForeignKey("certificate_no")]
        public virtual idst_renewal_data_rgs Idst_Renewal_Data_Rgs { get; set; }
    }
}
