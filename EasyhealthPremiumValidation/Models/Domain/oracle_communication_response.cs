using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class oracle_communication_response
    {
        [Key]
        public int id { get; set; }
        public long template_id { get; set; }
        public string policy_number { get; set; }

        public string? subject { get; set; }
        public string? message { get; set; }
        public DateTime? triggered_date { get; set; }
        public DateTime? delivered_date { get; set; }
        public string? delivery_status { get; set; }
        public string? mail_id { get; set; }
        public string? mobile_number { get; set; }
        public string? communication_type { get; set; }
        public string? customer_name { get; set; }

        public DateTime? expiry_date { get; set; }

        public string? product_name { get; set; }

        public int? job_req_id{ get; set; }        
      

    }
}
