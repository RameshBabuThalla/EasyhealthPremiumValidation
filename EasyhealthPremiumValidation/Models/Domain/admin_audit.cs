
using System.ComponentModel.DataAnnotations;
namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class admin_audit
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public DateTime createddate { get; set; }
        public string screenname { get; set; }
        public string eventname { get; set; }
        public string eventdescription { get; set; }
        public string createdby { get; set; }
    }
}
