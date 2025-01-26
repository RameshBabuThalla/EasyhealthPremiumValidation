using Microsoft.EntityFrameworkCore;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    [Keyless]
    public class baserate
    {
        public int age { get; set; }
        public int si { get; set; }
        public string tier { get; set; }
        public decimal? one_year { get; set; }
        public decimal? two_years { get; set; }
        public decimal? three_years { get; set; }
        public string product { get; set; }
    }
}
