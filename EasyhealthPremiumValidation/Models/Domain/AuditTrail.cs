using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class AuditTrail
    {
        
        public string policy_no { get; set; }
        public string current_rn_generation_status { get; set; }
        public string modified_rn_generation_status { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_datetime { get; set; }
    }
}
