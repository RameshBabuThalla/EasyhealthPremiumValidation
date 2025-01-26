using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class ErrorLog
    {
        [Key]
        public long policy_no { get; set; }
        public string? product_code { get; set; }
        public string? product_name { get; set; }
        public string? vehicle_make { get; set; }
        public string? new_sub_vertical { get; set; }
        public string? final_remarks { get; set; }
        public string? dispatch_status { get; set; }
        public string? error_log {  get; set; }


    }
}
