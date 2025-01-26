using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class ClickPSS
    {
        [Key]
        public string policy_no { get; set; }
        public string? vertical_name { get; set; }
        public string? vertical_receipant { get; set; }
        public string? dispatch_location { get; set; }
        public string? bosg_recipient { get; set; }
        public string? bdm_name { get; set; }
        public string? dealer_code { get; set; }
        public string? dealer_name { get; set; }
        public string? customer_dispatch { get; set; }
        public string? channel_dispatch { get; set; }
        public string? serial_no { get; set; }
        public string? batch_id { get; set; }
        public string? product_code { get; set; }
        public ProductInfo ProductDetails { get; set; }
        public string ProductIds { get; internal set; }
    }
    public class ProductInfo
    {
        public string ProductIds { get; set; }
    }
}
