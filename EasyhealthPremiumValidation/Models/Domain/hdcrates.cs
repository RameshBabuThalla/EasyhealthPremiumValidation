using Microsoft.EntityFrameworkCore;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    [Keyless]
    public class hdcrates
    {
        public int? age { get; set; }
        public string? age_band { get; set; }
        public int? si { get; set; }
        public string? plan_type { get; set; }
        public decimal? one_year { get; set; }
        public decimal? two_years { get; set; }
        public decimal ?three_years { get; set; }
    }
}
