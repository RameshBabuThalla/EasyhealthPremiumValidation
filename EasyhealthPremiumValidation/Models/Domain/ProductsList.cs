using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class ProductsList
    {
        [Key]
        public int product_id { get; set; }
        public string ?product_name { get; set; }
        public int ?product_code { get; set; }
        public int ?applicable_plan_codes { get; set; }
    }
}
