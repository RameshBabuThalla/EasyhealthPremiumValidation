using System.ComponentModel.DataAnnotations;

namespace HERGHERGPremiumValidationSchedular_API_Services.Models.Domain
{
    public class renewed_policies
    {
        [Key]
        public string policy_number { get; set; }
        public string new_policy_number { get; set; }
        public DateTime renewal_date { get; set; }
        public DateTime expiry_date { get; set; }
        public string product { get; set; }
    }
}
