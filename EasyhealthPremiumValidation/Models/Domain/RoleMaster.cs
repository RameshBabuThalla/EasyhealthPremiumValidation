using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class RoleMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int roleid { get; set; }
        public string rolename { get; set; }
        public string? description { get; set; }
        public bool active { get; set; }
        public string? createdby { get; set; }
        public DateTime createdeate { get; set; }
        public string? updatedby { get; set; }
        public DateTime? updateddate { get; set; }
        public string? rn_generation_status { get; set; }
    }
}
