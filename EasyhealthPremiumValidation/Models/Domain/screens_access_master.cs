using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class screens_access_details
    {
        [Key]
            public int id { get; set; }
            public int screenid { get; set; }
            public int? roleid { get; set; }
            public bool view { get; set; }
            public bool download { get; set; }
            public bool submit { get; set; }
            public bool re_trigger_comm { get; set; }
            public bool audit { get; set; }
            public bool downloadrn { get; set; }
            public string? created_by { get; set; }
            public DateTime? created_date { get; set; }
            public string? updated_by { get; set; }
            public DateTime? updated_date { get; set; }
           
    }
}
