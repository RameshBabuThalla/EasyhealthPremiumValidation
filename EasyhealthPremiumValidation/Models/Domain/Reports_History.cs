using Microsoft.EntityFrameworkCore;

namespace HERGPremiumValidationSchedular.Models.Domain
{ 
    
    public class reports_history
    {
        public string report_name { get; set; }
 
        public DateTime last_downloaded_date { get; set; }
        public string downloaded_by { get; set; }
    }
}
