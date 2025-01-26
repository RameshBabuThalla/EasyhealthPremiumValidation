using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class AdminConfiguration
    {
        
        public DateTime? created_date {  get; set; }
        public int? bot_cases_trigger {  get; set; }
        public int? deactive_inactive_user { get; set; }
        public string? bot_case_folder_location { get; set; }
        public int? history_data_cleanup { get; set; }
        public int? gc_trigger_frequency { get; set; }
        public DateTime? day_of_month_dwhupload { get; set; }

        //status of trigger field is pending 

    }
}
