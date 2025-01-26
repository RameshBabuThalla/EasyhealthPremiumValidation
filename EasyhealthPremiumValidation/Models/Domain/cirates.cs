using Microsoft.EntityFrameworkCore;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    [Keyless]
    public class cirates
    {
        public int age { get; set; }
        public int ci_variant { get; set; }
        public decimal? one_year { get; set; }
        public decimal? two_years { get; set; }
        public decimal? three_years { get; set; }

    }
}
