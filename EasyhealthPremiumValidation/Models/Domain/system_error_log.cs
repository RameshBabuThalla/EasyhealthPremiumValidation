using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class system_error_log
    {
        [Key]
        public int id { get; set; }  // Primary Key
        public string error_type { get; set; }
        public DateTime time_of_error { get; set; }
        public string message { get; set; }
        public string user_ip { get; set; }
        public string user_host_name { get; set; }
        public string user_name { get; set; }
        public string server_name { get; set; }
        public string stack_trace { get; set; }
        public DateTime created_time { get; set; }

    }
}

