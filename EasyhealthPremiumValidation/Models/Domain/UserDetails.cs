using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class UserDetails
    {
        [Key]
        public int employee_id {  get; set; }
        public string? employee_name { get; set; }
        public string? email_id { get; set; }
        public string? mobile_number { get; set; }
        public string? nt_login_id { get; set; }
        [NotMapped]
        public List<UserRoles> user_Roles { get; set; } = new List<UserRoles>();
        public string? department {  get; set; } 
        public DateTime? last_login_date { get; set; }
        public string? rg_status { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public string? created_by { get; set; }
        public string? arm_id { get; set; }

    }
    public class UserRoles
    {
        [ForeignKey("UserDetails")]
        public int employeeid { get; set; }
        [ForeignKey("RoleMaster")]
        public int roleid { get; set; }

        //public virtual UserDetails? UserDetails { get; set; }
        //public virtual RoleMaster? Role { get; set; }
    }
}
