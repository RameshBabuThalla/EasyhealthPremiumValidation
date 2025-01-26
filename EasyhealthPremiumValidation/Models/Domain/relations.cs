using Microsoft.EntityFrameworkCore;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    [Keyless]
    public class relations
    {
        public string Insured_Relation { get; set; }
        public string Relation_Tag { get; set; }
    }
}
