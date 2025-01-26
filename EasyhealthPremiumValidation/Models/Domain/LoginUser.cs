namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class LoginUser
    {
        public string nt_login_id { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public bool? active { get; set; }
        public int roleid { get; set; }
        public string? rolename { get; set; } = string.Empty;
        public string? createdby { get; set; }
        public DateTime? createdeate { get; set; }
        public string? updatedby { get; set; }
        public DateTime? updateddate { get; set; }
        public DateTime? lastlogindate { get; set; }


    }
}
