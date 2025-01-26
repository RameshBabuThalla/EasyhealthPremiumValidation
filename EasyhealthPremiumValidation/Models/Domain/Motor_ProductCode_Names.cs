using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class Motor_ProductCode_Names
    {
        [Key]
        public int product_code { get; set; }
        public string product_name { get; set; }
        public string product_category { get; set; }
    }
}
