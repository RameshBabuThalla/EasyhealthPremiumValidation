using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class GC_insured_details
    {
        public long batch_id { get; set; }
        public long customer_id { get; set; }
        public string customer_name { get; set; }
        public string salutation { get; set; }
        public string location_code { get; set; }

        public string? address_line_1  { get; set; }
        public string? address_line_2 { get; set; }
        public string apartment { get; set; }
        public string street { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string state_code { get; set; }
        public string region { get; set; }
        public int? pincode { get; set; }
        public string nationality { get; set; }
        public int? mobile { get; set; }
        public int? telephone { get; set; }
        public string email { get; set; }
        public decimal intermediary_code { get; set; }
        public string intermediary_name { get; set; }
        public string psm_name { get; set; }
        public string ssm_name { get; set; }
        public decimal insured_memberid_1 { get; set; }
        public decimal insured_memberid_2 { get; set; }
        public decimal insured_memberid_3 { get; set; }
        public decimal insured_memberid_4 { get; set; }
        public decimal insured_memberid_5 { get; set; }
        public decimal insured_memberid_6 { get; set; }
        public decimal insured_memberid_7 { get; set; }
        public decimal insured_memberid_8 { get; set; }
        public decimal insured_memberid_9 { get; set; }
        public decimal insured_memberid_10 { get; set; }
        public decimal insured_memberid_11 { get; set; }
        public decimal insured_memberid_12 { get; set; }
        public int? loading_per_insured_1 { get; set; }
        public int? loading_per_insured_2 { get; set; }
        public int? loading_per_insured_3 { get; set; }
        public int? loading_per_insured_4 { get; set; }
        public int? loading_per_insured_5 { get; set; }
        public int? loading_per_insured_6 { get; set; }
        public int? loading_per_insured_7 { get; set; }
        public int? loading_per_insured_8 { get; set; }
        public int? loading_per_insured_9 { get; set; }
        public int? loading_per_insured_10 { get; set; }
        public int? loading_per_insured_11 { get; set; }
        public int? loading_per_insured_12 { get; set; }
        public int? loading_amount_insured_1 { get; set; }
        public int? loading_amount_insured_2 { get; set; }
        public int? loading_amount_insured_3 { get; set; }
        public int? loading_amount_insured_4 { get; set; }
        public int? loading_amount_insured_5 { get; set; }
        public int? loading_amount_insured_6 { get; set; }
        public int? loading_amount_insured_7 { get; set; }
        public int? loading_amount_insured_8 { get; set; }
        public int? loading_amount_insured_9 { get; set; }
        public int? loading_amount_insured_10 { get; set; }
        public int? loading_amount_insured_11 { get; set; }
        public int? loading_amount_insured_12 { get; set; }
        public DateTime insured_dob_1 { get; set; }
        public DateTime insured_dob_2 { get; set; }
        public DateTime insured_dob_3 { get; set; }
        public DateTime insured_dob_4 { get; set; }
        public DateTime insured_dob_5 { get; set; }
        public DateTime insured_dob_6 { get; set; }
        public DateTime insured_dob_7 { get; set; }
        public DateTime insured_dob_8 { get; set; }
        public DateTime insured_dob_9 { get; set; }
        public DateTime insured_dob_10 { get; set; }
        public DateTime insured_dob_11 { get; set; }
        public DateTime insured_dob_12 { get; set; }
        public int? ped_1 { get; set; }
        public int? ped_2 { get; set; }
        public int? ped_3 { get; set; }
        public int? ped_4 { get; set; }
        public int? ped_5 { get; set; }
        public int? ped_6 { get; set; }
        public int? ped_7 { get; set; }
        public int? ped_8 { get; set; }
        public int? ped_9 { get; set; }
        public int? ped_10 { get; set; }
        public int? ped_11 { get; set; }
        public int? ped_12 { get; set; }
        [Key]
        public long policy_no { get; set; }
        
    }
}
