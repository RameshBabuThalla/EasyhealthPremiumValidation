using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HERGPremiumValidationSchedular.Models.Domain;
using HERG_HERGPremiumValidationSchedularAPI_Services.Models.Domain;
using HERGHERGPremiumValidationSchedular_API_Services.Models.Domain;
using HERGPremiumValidationSchedular.Model.Domain;
using HERGPremiumValidationSchedular.Models;


namespace HERGPremiumValidationSchedular.Data
{
    public class HDFCDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        //public HDFCDbContext(DbContextOptions options) : base(options)
        //{

        //}
        public HDFCDbContext(DbContextOptions<HDFCDbContext> options) : base(options)
        {
        }
        public HDFCDbContext(DbContextOptions<HDFCDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        //public HDFCDbContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        public DbSet<AuditTrail> audittrail { get; set; }
        public DbSet<relations> relations { get; set; }
        public DbSet<hdcrates> hdcrates { get; set; }
        public DbSet<hdcproportionsplit> hdcproportionsplit { get; set; }
        public DbSet<hdcrates_optimarestore> hdcrates_optimarestore { get; set; }
        public DbSet<baserate_optimarestore> baserate_optimarestore { get; set; }
        public DbSet<carates> carates { get; set; }
        public DbSet<ProductsList> productslist { get; set; }
        public DbSet<dwh> dwh { get; set; }
        public DbSet<cirates> cirates { get; set; }
        public DbSet<deductiblediscount> deductiblediscount { get; set; }
        public DbSet<baserate> baserate { get; set; }
        public DbSet<easyhealth_baserates> easyhealth_baserates { get; set; }
        public DbSet<easyhealth_carates> easyhealth_carates { get; set; }
        public DbSet<easyhealth_cirates> easyhealth_cirates { get; set; }
        public DbSet<easyhealth_hdcrates> easyhealth_hdcrates { get; set; }
        public DbSet<gc_hdfc> gc_hdfc { get; set; }
        public DbSet<reports_history> reports_history { get; set; }
        public DbSet<easyhealth_new_baserates> easyhealth_new_baserates { get; set; }
        public DbSet<easyhealth_new_carates> easyhealth_new_carates { get; set; }
        public DbSet<easyhealth_new_cirates> easyhealth_new_cirates { get; set; }
        public DbSet<easyhealth_new_hdcrates> easyhealth_new_hdcrates { get; set; }
        public DbSet<aarogya_sanjeevani_baserates> aarogya_sanjeevani_baserates { get; set; }
        public DbSet<AdminConfiguration> Admin_Configuration { get; set; }
        public DbSet<two_wheeler_ncb_rate> two_wheeler_ncb_rate { get; set; }
        public DbSet<vehicle_theft_data> vehicle_theft_data { get; set; }
        public DbSet<cc_equivalent> cc_equivalent { get; set; }
        public DbSet<vehicle_classification> vehicle_classification { get; set; }
        public DbSet<optimasenior_baserates> optimasenior_baserates { get; set; }
        public DbSet<optimavital_baserates> optimavital_baserates { get; set; }

        public DbSet<rne_healthtab> rne_healthtab { get; set; }
        public DbSet<rne_motortab> rne_motortab { get; set; }
        public DbSet<rne_calculated_cover_rg> rne_calculated_cover_rg { get; set; }
        public DbSet<tab_health_renewalnotice> tab_health_renewalnotice { get; set; }
        public DbSet<UserDetails> user_details { get; set; }
        public DbSet<UserRoles> user_roles { get; set; }
        public DbSet<RoleMaster> rolemaster { get; set; }
        public DbSet<Motor_ProductCode_Names> Motor_ProductCode_Names { get; set; }
        public DbSet<Vehicle_Code> Vehicle_Code { get; set; }
        public DbSet<Vahan_VehicleClassification> Vahan_VehicleClassification { get; set; }
        public DbSet<oem_master> oem_master { get; set; }
        public DbSet<renewed_policies> renewed_policies { get; set; }
        public DbSet<oracle_communication_response> oracle_communication_response { get; set; }

        public DbSet<idst_renewal_data_rgs> idst_renewal_data_rgs { get; set; }
        public DbSet<gc_policy_details> gc_policy_details { get; set; }
        public DbSet<GC_insured_details> gc_insured_details { get; set; }
        public DbSet<gc_loading_discount> gc_loading_discount { get; set; }
        public DbSet<gc_rider_details> gc_rider_details { get; set; }
        public DbSet<gc_premium_details> gc_premium_details { get; set; }
        // public DbSet<LoginUser> usercreation { get; set; }
        public DbSet<ErrorLog> error_log { get; set; }
        public DbSet<system_error_log> system_Error_Logs { get; set; }
        public DbSet<koti_ipa_rates> koti_ipa_rates { get; set; }
        public DbSet<policydocuments> policydocuments { get; set; }
        public DbSet<policydocumentslistcontent> policydocumentslistcontent { get; set; }
        public DbSet<botcasetbl> botcasetbl { get; set; }
        public DbSet<screens_access_details> screens_access_details { get; set; }
        public DbSet<screen_access_master> screen_access_master { get; set; }
        public DbSet<workflow_status_master> workflow_status_master { get; set; }
        public DbSet<admin_audit> admin_audit { get; set; }
        public DbSet<idst_ora_respo_delivery_status> idst_ora_respo_delivery_status { get; set; }
        public DbSet<idst_ora_respo_sms_status> idst_ora_respo_sms_status { get; set; }
        public DbSet<whatsapp_healthreminder_status> whatsapp_healthreminder_status { get; set; }
        public DbSet<whatsapp_motor_reminder_status> whatsapp_motor_reminder_status { get; set; }
        public DbSet<medisure_age_group> medisure_age_group { get; set; }
        public DbSet<medisure_baserates> medisure_baserates { get; set; }
        public DbSet<medisure_family_defn> medisure_family_defn { get; set; }
        public DbSet<medisure_si_deductible> medisure_si_deductible { get; set; }
        public DbSet<premium_validation> premium_validation { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the entity as keyless
            modelBuilder.Entity<rne_healthtab>().ToTable("rne_healthtab", "ins");
            modelBuilder.Entity<rne_motortab>().ToTable("rne_motortab", "ins");
            modelBuilder.Entity<rne_calculated_cover_rg>().ToTable("rne_calculated_cover_rg", "ins");
            modelBuilder.Entity<tab_health_renewalnotice>().ToTable("tab_health_renewalnotice", "ins");
            modelBuilder.Entity<idst_renewal_data_rgs>().ToTable("idst_renewal_data_rgs", "ins");
            modelBuilder.Entity<botcasetbl>().ToTable("botcasetbl", "ins");
            modelBuilder.Entity<policydocumentslistcontent>().ToTable("policydocumentslistcontent", "ins");
            modelBuilder.Entity<policydocuments>().ToTable("policydocuments", "ins");
            modelBuilder.Entity<premium_validation>().ToTable("premium_validation", "ins");

            modelBuilder.Entity<tab_health_renewalnotice>().HasNoKey();
            modelBuilder.Entity<hdcproportionsplit>().HasNoKey();
            modelBuilder.Entity<deductiblediscount>().HasNoKey();
            modelBuilder.Entity<UserDetails>();
            modelBuilder.Entity<RoleMaster>().Property(m => m.roleid)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<AdminConfiguration>().HasKey(ur => new { ur.created_date, ur.deactive_inactive_user, ur.bot_cases_trigger });
            modelBuilder.Entity<UserRoles>().HasKey(ur => new { ur.employeeid, ur.roleid });
            modelBuilder.Entity<baserate>().HasNoKey();
            modelBuilder.Entity<cirates>().HasNoKey();
            modelBuilder.Entity<relations>().HasNoKey();
            // modelBuilder.Entity<LoginUser>().HasNoKey();
            modelBuilder.Entity<reports_history>().HasKey(ur => new { ur.report_name, ur.last_downloaded_date });
            modelBuilder.Entity<AuditTrail>().HasKey(ur => new { ur.policy_no, ur.modified_datetime });
            //motor validation tables
            modelBuilder.Entity<two_wheeler_ncb_rate>();
            modelBuilder.Entity<vehicle_theft_data>().HasNoKey();
            modelBuilder.Entity<cc_equivalent>().HasNoKey();
            modelBuilder.Entity<vehicle_classification>().HasNoKey();
            modelBuilder.Entity<Motor_ProductCode_Names>();
            modelBuilder.Entity<Vehicle_Code>();
            modelBuilder.Entity<Vahan_VehicleClassification>();
            modelBuilder.Entity<oem_master>().HasKey(ur => new { ur.vehicle_make, ur.vertical });
            modelBuilder.Entity<renewed_policies>();
            modelBuilder.Entity<oracle_communication_response>();

            // Configure the entity as keyless
            modelBuilder.Entity<deductiblediscount>().HasNoKey();
            modelBuilder.Entity<baserate>().HasNoKey();
            modelBuilder.Entity<cirates>().HasNoKey();
            modelBuilder.Entity<relations>().HasNoKey();
            modelBuilder.Entity<carates>().HasNoKey();
            modelBuilder.Entity<hdcrates>().HasNoKey();
            modelBuilder.Entity<baserate_optimarestore>().HasNoKey();
            modelBuilder.Entity<hdcrates_optimarestore>().HasNoKey();
            modelBuilder.Entity<hdcproportionsplit>().HasNoKey();
            modelBuilder.Entity<easyhealth_baserates>().HasNoKey();
            modelBuilder.Entity<easyhealth_carates>().HasNoKey();
            modelBuilder.Entity<easyhealth_cirates>().HasNoKey();
            modelBuilder.Entity<easyhealth_hdcrates>().HasNoKey();
            modelBuilder.Entity<easyhealth_new_baserates>().HasNoKey();
            modelBuilder.Entity<easyhealth_new_carates>().HasNoKey();
            modelBuilder.Entity<easyhealth_new_cirates>().HasNoKey();
            modelBuilder.Entity<easyhealth_new_hdcrates>().HasNoKey();
            modelBuilder.Entity<system_error_log>().ToTable("system_error_log", "ins");
            modelBuilder.Entity<koti_ipa_rates>().ToTable("koti_ipa_rates", "ins").HasNoKey();
            modelBuilder.Entity<optimasenior_baserates>().HasNoKey();
            modelBuilder.Entity<optimavital_baserates>().HasNoKey();
            modelBuilder.Entity<aarogya_sanjeevani_baserates>().HasNoKey();
            modelBuilder.Entity<idst_ora_respo_delivery_status>().HasNoKey();
            modelBuilder.Entity<idst_ora_respo_sms_status>().HasNoKey();
            modelBuilder.Entity<whatsapp_healthreminder_status>().HasNoKey();
            modelBuilder.Entity<whatsapp_motor_reminder_status>().HasNoKey();
            modelBuilder.Entity<medisure_si_deductible>().HasNoKey();
            modelBuilder.Entity<medisure_age_group>().HasNoKey();
            modelBuilder.Entity<medisure_family_defn>().HasNoKey();
            modelBuilder.Entity<medisure_baserates>().HasNoKey();
            base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Check if options have already been configured
            {
                string dbConn = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder
                   .UseNpgsql(dbConn)
                    .EnableSensitiveDataLogging(); // Enables detailed error information
            }
        }

    }
}

