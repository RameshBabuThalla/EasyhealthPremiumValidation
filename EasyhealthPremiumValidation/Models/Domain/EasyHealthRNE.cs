﻿using System.ComponentModel.DataAnnotations;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class EasyHealthRNE
    {
        //public string? jobid { get; set; }
        //public string? batchid { get; set; }
        //public string? splitid { get; set; }
        //public TimeSpan? extractedon { get; set; }
        //public short? qcstatus { get; set; }
        //public string? qcmsdg { get; set; }
        //public TimeSpan? qcon { get; set; }
        //public short? ispreintreq { get; set; }
        //public short? printstatus { get; set; }
        //public string? printmesg { get; set; }
        //public TimeSpan? printedon { get; set; }
        //public string? partnerflag { get; set; }
        //public TimeSpan? noticedate { get; set; }
        //public decimal? qc_tot_premium { get; set; }
        //public decimal? qc_service_tax { get; set; }
        //public string? partnername { get; set; }
        //public string? partneruniqueid { get; set; }
        //public decimal? partnertotpremium { get; set; }
        //public decimal? partner_tax { get; set; }
        public int? prod_code { get; set; }
        public string? prod_name { get; set; }
        //public string? split_flag { get; set; }
        public long? reference_num { get; set; }
        public TimeSpan? reference_date { get; set; }
        [Key]
        public string policy_number { get; set; }
        //public long? branchcode { get; set; }
        //public string? branchname { get; set; }
        //public string? customer_id { get; set; }
        //public string? customername { get; set; }
        //public string? txt_salutation { get; set; }
        //public string? location_code { get; set; }
        //public string? txt_address_line_1 { get; set; }
        //public string? txt_address_line_2 { get; set; }
        //public string? txt_apartment { get; set; }
        //public string? txt_street { get; set; }
        //public string? txt_areavillage { get; set; }
        //public string? txt_citydistrict { get; set; }
        //public string? txt_state { get; set; }
        //public string? state_code { get; set; }
        //public string? state_regis { get; set; }
        //public string? txt_pincode { get; set; }
        //public string? txt_nationality { get; set; }
        //public string? txt_mobile { get; set; }
        //public string? txt_telephone { get; set; }
        //public string? txt_dealer_cd { get; set; }
        //public string? imdname { get; set; }
        //public string? imdmobile { get; set; }
        //public string? vertical_name { get; set; }
        //public string? verticalname { get; set; }
        public DateTime? policy_start_date { get; set; }
        public DateTime? policy_expiry_date { get; set; }
        public string? policy_period { get; set; }
        public string? policyplan { get; set; }
        public string? policy_type { get; set; }
        public string? txt_family { get; set; }
        public string? tier_type { get; set; }
        //public int? claimcount { get; set; }
        public decimal? num_tot_premium { get; set; }
        public decimal? num_net_premium { get; set; }
        public decimal? num_service_tax { get; set; }
        public string? txt_insuredname1 { get; set; }
        public string? txt_insuredname2 { get; set; }
        public string? txt_insuredname3 { get; set; }
        public string? txt_insuredname4 { get; set; }
        public string? txt_insuredname5 { get; set; }
        public string? txt_insuredname6 { get; set; }
        public string? txt_insured_dob1 { get; set; }
        public string? txt_insured_dob2 { get; set; }
        public string? txt_insured_dob3 { get; set; }
        public string? txt_insured_dob4 { get; set; }
        public string? txt_insured_dob5 { get; set; }
        public string? txt_insured_dob6 { get; set; }
        public string? txt_insured_relation1 { get; set; }
        public string? txt_insured_relation2 { get; set; }
        public string? txt_insured_relation3 { get; set; }
        public string? txt_insured_relation4 { get; set; }
        public string? txt_insured_relation5 { get; set; }
        public string? txt_insured_relation6 { get; set; }
        public string? txt_insured_age1 { get; set; }
        public string? txt_insured_age2 { get; set; }
        public string? txt_insured_age3 { get; set; }
        public string? txt_insured_age4 { get; set; }
        public string? txt_insured_age5 { get; set; }
        public string? txt_insured_age6 { get; set; }
        public string? txt_insured_gender1 { get; set; }
        public string? txt_insured_gender2 { get; set; }
        public string? txt_insured_gender3 { get; set; }
        public string? txt_insured_gender4 { get; set; }
        public string? txt_insured_gender5 { get; set; }
        public string? txt_insured_gender6 { get; set; }
        public decimal? sum_insured1 { get; set; }
        public decimal? sum_insured2 { get; set; }
        public decimal? sum_insured3 { get; set; }
        public decimal? sum_insured4 { get; set; }
        public decimal? sum_insured5 { get; set; }
        public decimal? sum_insured6 { get; set; }
        public string? insured_cb1 { get; set; }
        public string? insured_cb2 { get; set; }
        public string? insured_cb3 { get; set; }
        public string? insured_cb4 { get; set; }
        public string? insured_cb5 { get; set; }
        public string? insured_cb6 { get; set; }
        public decimal? premium_insured1 { get; set; }
        public decimal? premium_insured2 { get; set; }
        public decimal? premium_insured3 { get; set; }
        public decimal? premium_insured4 { get; set; }
        public decimal? premium_insured5 { get; set; }
        public decimal? premium_insured6 { get; set; }
        public decimal? insured_loadingper1 { get; set; }
        public decimal? insured_loadingper2 { get; set; }
        public decimal? insured_loadingper3 { get; set; }
        public decimal? insured_loadingper4 { get; set; }
        public decimal? insured_loadingper5 { get; set; }
        public decimal? insured_loadingper6 { get; set; }
        public decimal? insured_loadingamt1 { get; set; }
        public decimal? insured_loadingamt2 { get; set; }
        public decimal? insured_loadingamt3 { get; set; }
        public decimal? insured_loadingamt4 { get; set; }
        public decimal? insured_loadingamt5 { get; set; }
        public decimal? insured_loadingamt6 { get; set; }
        public string? covername11 { get; set; }
        public decimal? coversi11 { get; set; }
        public decimal? coverprem11 { get; set; }
        public string? covername12 { get; set; }
        public decimal? coversi12 { get; set; }
        public decimal? coverprem12 { get; set; }
        public string? covername13 { get; set; }
        public decimal? coversi13 { get; set; }
        public decimal? coverprem13 { get; set; }
        public string? covername14 { get; set; }
        public decimal? coversi14 { get; set; }
        public decimal? coverprem14 { get; set; }
        public string? covername15 { get; set; }
        public decimal? coversi15 { get; set; }
        public decimal? coverprem15 { get; set; }
        public string? covername16 { get; set; }
        public decimal? coversi16 { get; set; }
        public decimal? coverprem16 { get; set; }
        public string? covername21 { get; set; }
        public decimal? coversi21 { get; set; }
        public decimal? coverprem21 { get; set; }
        public string? covername22 { get; set; }
        public decimal? coversi22 { get; set; }
        public decimal? coverprem22 { get; set; }
        public string? covername23 { get; set; }
        public decimal? coversi23 { get; set; }
        public decimal? coverprem23 { get; set; }
        public string? covername24 { get; set; }
        public decimal? coversi24 { get; set; }
        public decimal? coverprem24 { get; set; }
        public string? covername25 { get; set; }
        public decimal? coversi25 { get; set; }
        public decimal? coverprem25 { get; set; }
        public string? covername26 { get; set; }
        public decimal? coversi26 { get; set; }
        public decimal? coverprem26 { get; set; }
        public string? covername31 { get; set; }
        public decimal? coversi31 { get; set; }
        public decimal? coverprem31 { get; set; }
        public string? covername32 { get; set; }
        public decimal? coversi32 { get; set; }
        public decimal? coverprem32 { get; set; }
        public string? covername33 { get; set; }
        public decimal? coversi33 { get; set; }
        public decimal? coverprem33 { get; set; }
        public string? covername34 { get; set; }
        public decimal? coversi34 { get; set; }
        public decimal? coverprem34 { get; set; }
        public string? covername35 { get; set; }
        public decimal? coversi35 { get; set; }
        public decimal? coverprem35 { get; set; }
        public string? covername36 { get; set; }
        public decimal? coversi36 { get; set; }
        public decimal? coverprem36 { get; set; }
        public string? covername41 { get; set; }
        public decimal? coversi41 { get; set; }
        public decimal? coverprem41 { get; set; }
        public string? covername42 { get; set; }
        public decimal? coversi42 { get; set; }
        public decimal? coverprem42 { get; set; }
        public string? covername43 { get; set; }
        public decimal? coversi43 { get; set; }
        public decimal? coverprem43 { get; set; }
        public string? covername44 { get; set; }
        public decimal? coversi44 { get; set; }
        public decimal? coverprem44 { get; set; }
        public string? covername45 { get; set; }
        public decimal? coversi45 { get; set; }
        public decimal? coverprem45 { get; set; }
        public string? covername46 { get; set; }
        public decimal? coversi46 { get; set; }
        public decimal? coverprem46 { get; set; }
        public string? covername51 { get; set; }
        public decimal? coversi51 { get; set; }
        public decimal? coverprem51 { get; set; }
        public string? covername52 { get; set; }
        public decimal? coversi52 { get; set; }
        public decimal? coverprem52 { get; set; }
        public string? covername53 { get; set; }
        public decimal? coversi53 { get; set; }
        public decimal? coverprem53 { get; set; }
        public string? covername54 { get; set; }
        public decimal? coversi54 { get; set; }
        public decimal? coverprem54 { get; set; }
        public string? covername55 { get; set; }
        public decimal? coversi55 { get; set; }
        public decimal? coverprem55 { get; set; }
        public string? covername56 { get; set; }
        public decimal? coversi56 { get; set; }
        public decimal? coverprem56 { get; set; }
        public string? covername61 { get; set; }
        public decimal? coversi61 { get; set; }
        public decimal? coverprem61 { get; set; }
        public string? covername62 { get; set; }
        public decimal? coversi62 { get; set; }
        public decimal? coverprem62 { get; set; }
        public string? covername63 { get; set; }
        public decimal? coversi63 { get; set; }
        public decimal? coverprem63 { get; set; }
        public string? covername64 { get; set; }
        public decimal? coversi64 { get; set; }
        public decimal? coverprem64 { get; set; }
        public string? covername65 { get; set; }
        public decimal? coversi65 { get; set; }
        public decimal? coverprem65 { get; set; }
        public string? covername66 { get; set; }
        public decimal? coversi66 { get; set; }
        public decimal? coverprem66 { get; set; }
        public string? upselltype1 { get; set; }
        public string? upselltype2 { get; set; }
        public string? upselltype3 { get; set; }
        public string? upselltype4 { get; set; }
        public string? upselltype5 { get; set; }
        public string? upsellvalue1 { get; set; }
        public string? upsellvalue2 { get; set; }
        public string? upsellvalue3 { get; set; }
        public string? upsellvalue4 { get; set; }
        public string? upsellvalue5 { get; set; }
        public decimal? upsellpremium1 { get; set; }
        public decimal? upsellpremium2 { get; set; }
        public decimal? upsellpremium3 { get; set; }
        public decimal? upsellpremium4 { get; set; }
        public decimal? upsellpremium5 { get; set; }
        public decimal? upselltax1 { get; set; }
        public decimal? upselltax2 { get; set; }
        public decimal? upselltax3 { get; set; }
        public decimal? upselltax4 { get; set; }
        public decimal? upselltax5 { get; set; }
        public string? pollddesc1 { get; set; }
        public string? pollddesc2 { get; set; }
        public string? pollddesc3 { get; set; }
        public string? pollddesc4 { get; set; }
        public string? pollddesc5 { get; set; }
        public decimal? polldrate1 { get; set; }
        public decimal? polldrate2 { get; set; }
        public decimal? polldrate3 { get; set; }
        public decimal? polldrate4 { get; set; }
        public decimal? polldrate5 { get; set; }
        public decimal? polldamt1 { get; set; }
        public decimal? polldamt2 { get; set; }
        public decimal? polldamt3 { get; set; }
        public decimal? polldamt4 { get; set; }
        public decimal? polldamt5 { get; set; }
        public decimal? coverloadingrate11 { get; set; }
        public decimal? coverloadingrate12 { get; set; }
        public decimal? coverloadingrate13 { get; set; }
        public decimal? coverloadingrate14 { get; set; }
        public decimal? coverloadingrate15 { get; set; }
        public decimal? coverloadingrate16 { get; set; }
        public decimal? coverloadingrate21 { get; set; }
        public decimal? coverloadingrate22 { get; set; }
        public decimal? coverloadingrate23 { get; set; }
        public decimal? coverloadingrate24 { get; set; }
        public decimal? coverloadingrate25 { get; set; }
        public decimal? coverloadingrate26 { get; set; }
        public decimal? coverloadingrate31 { get; set; }
        public decimal? coverloadingrate32 { get; set; }
        public decimal? coverloadingrate33 { get; set; }
        public decimal? coverloadingrate34 { get; set; }
        public decimal? coverloadingrate35 { get; set; }
        public decimal? coverloadingrate36 { get; set; }
        public decimal? coverloadingrate41 { get; set; }
        public decimal? coverloadingrate42 { get; set; }
        public decimal? coverloadingrate43 { get; set; }
        public decimal? coverloadingrate44 { get; set; }
        public decimal? coverloadingrate45 { get; set; }
        public decimal? coverloadingrate46 { get; set; }
        public decimal? coverloadingrate51 { get; set; }
        public decimal? coverloadingrate52 { get; set; }
        public decimal? coverloadingrate53 { get; set; }
        public decimal? coverloadingrate54 { get; set; }
        public decimal? coverloadingrate55 { get; set; }
        public decimal? coverloadingrate56 { get; set; }
        public decimal? coverloadingrate61 { get; set; }
        public decimal? coverloadingrate62 { get; set; }
        public decimal? coverloadingrate63 { get; set; }
        public decimal? coverloadingrate64 { get; set; }
        public decimal? coverloadingrate65 { get; set; }
        public decimal? coverloadingrate66 { get; set; }
        public decimal? coverbaseloadingrate1 { get; set; }
        public decimal? coverbaseloadingrate2 { get; set; }
        public decimal? coverbaseloadingrate3 { get; set; }
        public decimal? coverbaseloadingrate4 { get; set; }
        public decimal? coverbaseloadingrate5 { get; set; }
        public decimal? coverbaseloadingrate6 { get; set; }
        public string? txt_email { get; set; }
        public string? covername17 { get; set; }
        public string? covername18 { get; set; }
        public string? covername19 { get; set; }
        public string? covername110 { get; set; }
        public string? covername27 { get; set; }
        public string? covername28 { get; set; }
        public string? covername29 { get; set; }
        public string? covername210 { get; set; }
        public string? covername37 { get; set; }
        public string? covername38 { get; set; }
        public string? covername39 { get; set; }
        public string? covername310 { get; set; }
        public string? covername47 { get; set; }
        public string? covername48 { get; set; }
        public string? covername49 { get; set; }
        public string? covername410 { get; set; }
        public string? covername57 { get; set; }
        public string? covername58 { get; set; }
        public string? covername59 { get; set; }
        public string? covername510 { get; set; }
        public string? covername67 { get; set; }
        public string? covername68 { get; set; }
        public string? covername69 { get; set; }
        public string? covername610 { get; set; }
        public string? covername101 { get; set; }
        public string? covername102 { get; set; }
        public string? covername103 { get; set; }
        public string? covername104 { get; set; }
        public string? covername105 { get; set; }
        public string? covername106 { get; set; }
        public string? covername107 { get; set; }
        public string? covername108 { get; set; }
        public string? covername109 { get; set; }
        public string? covername1010 { get; set; }
        public decimal? coversi17 { get; set; }
        public decimal? coversi18 { get; set; }
        public decimal? coversi19 { get; set; }
        public decimal? coversi110 { get; set; }
        public decimal? coversi27 { get; set; }
        public decimal? coversi28 { get; set; }
        public decimal? coversi29 { get; set; }
        public decimal? coversi210 { get; set; }
        public decimal? coversi37 { get; set; }
        public decimal? coversi38 { get; set; }
        public decimal? coversi39 { get; set; }
        public decimal? coversi310 { get; set; }
        public decimal? coversi47 { get; set; }
        public decimal? coversi48 { get; set; }
        public decimal? coversi49 { get; set; }
        public decimal? coversi410 { get; set; }
        public decimal? coversi57 { get; set; }
        public decimal? coversi58 { get; set; }
        public decimal? coversi59 { get; set; }
        public decimal? coversi510 { get; set; }
        public decimal? coversi67 { get; set; }
        public decimal? coversi68 { get; set; }
        public decimal? coversi69 { get; set; }
        public decimal? coversi610 { get; set; }
        public decimal? coversi101 { get; set; }
        public decimal? coversi102 { get; set; }
        public decimal? coversi103 { get; set; }
        public decimal? coversi104 { get; set; }
        public decimal? coversi105 { get; set; }
        public decimal? coversi106 { get; set; }
        public decimal? coversi107 { get; set; }
        public decimal? coversi108 { get; set; }
        public decimal? coversi109 { get; set; }
        public decimal? coversi1010 { get; set; }
        public decimal? coverprem17 { get; set; }
        public decimal? coverprem18 { get; set; }
        public decimal? coverprem19 { get; set; }
        public decimal? coverprem110 { get; set; }
        public decimal? coverprem27 { get; set; }
        public decimal? coverprem28 { get; set; }
        public decimal? coverprem29 { get; set; }
        public decimal? coverprem210 { get; set; }
        public decimal? coverprem37 { get; set; }
        public decimal? coverprem38 { get; set; }
        public decimal? coverprem39 { get; set; }
        public decimal? coverprem310 { get; set; }
        public decimal? coverprem47 { get; set; }
        public decimal? coverprem48 { get; set; }
        public decimal? coverprem49 { get; set; }
        public decimal? coverprem410 { get; set; }
        public decimal? coverprem57 { get; set; }
        public decimal? coverprem58 { get; set; }
        public decimal? coverprem59 { get; set; }
        public decimal? coverprem510 { get; set; }
        public decimal? coverprem67 { get; set; }
        public decimal? coverprem68 { get; set; }
        public decimal? coverprem69 { get; set; }
        public decimal? coverprem610 { get; set; }
        public decimal? coverprem101 { get; set; }
        public decimal? coverprem102 { get; set; }
        public decimal? coverprem103 { get; set; }
        public decimal? coverprem104 { get; set; }
        public decimal? coverprem105 { get; set; }
        public decimal? coverprem106 { get; set; }
        public decimal? coverprem107 { get; set; }
        public decimal? coverprem108 { get; set; }
        public decimal? coverprem109 { get; set; }
        public decimal? coverprem1010 { get; set; }
        public decimal? coverloadingrate17 { get; set; }
        public decimal? coverloadingrate18 { get; set; }
        public decimal? coverloadingrate19 { get; set; }
        public decimal? coverloadingrate110 { get; set; }
        public decimal? coverloadingrate27 { get; set; }
        public decimal? coverloadingrate28 { get; set; }
        public decimal? coverloadingrate29 { get; set; }
        public decimal? coverloadingrate210 { get; set; }
        public decimal? coverloadingrate37 { get; set; }
        public decimal? coverloadingrate38 { get; set; }
        public decimal? coverloadingrate39 { get; set; }
        public decimal? coverloadingrate310 { get; set; }
        public decimal? coverloadingrate47 { get; set; }
        public decimal? coverloadingrate48 { get; set; }
        public decimal? coverloadingrate49 { get; set; }
        public decimal? coverloadingrate410 { get; set; }
        public decimal? coverloadingrate57 { get; set; }
        public decimal? coverloadingrate58 { get; set; }
        public decimal? coverloadingrate59 { get; set; }
        public decimal? coverloadingrate510 { get; set; }
        public decimal? coverloadingrate67 { get; set; }
        public decimal? coverloadingrate68 { get; set; }
        public decimal? coverloadingrate69 { get; set; }
        public decimal? coverloadingrate610 { get; set; }
        public decimal? coverloadingrate101 { get; set; }
        public decimal? coverloadingrate102 { get; set; }
        public decimal? coverloadingrate103 { get; set; }
        public decimal? coverloadingrate104 { get; set; }
        public decimal? coverloadingrate105 { get; set; }
        public decimal? coverloadingrate106 { get; set; }
        public decimal? coverloadingrate107 { get; set; }
        public decimal? coverloadingrate108 { get; set; }
        public decimal? coverloadingrate109 { get; set; }
        public decimal? coverloadingrate1010 { get; set; }
        public decimal? loyalty_discount { get; set; }
        public decimal? employee_discount { get; set; }
        public decimal? online_discount { get; set; }
        public decimal? family_discount { get; set; }
        public decimal? longterm_discount { get; set; }
        public decimal? family_discount_PRHDC { get; set; }
        public decimal? base_Premium1 { get; set; }
        public decimal? base_Premium2 { get; set; }
        public decimal? base_Premium3 { get; set; }
        public decimal? base_Premium4 { get; set; }
        public decimal? base_Premium5 { get; set; }
        public decimal? base_Premium6 { get; set; }
        public decimal? base_Premium { get; set; }
        public decimal? basePremLoading_Insured1 { get; set; }
        public decimal? basePremLoading_Insured2 { get; set; }
        public decimal? basePremLoading_Insured3 { get; set; }
        public decimal? basePremLoading_Insured4 { get; set; }
        public decimal? basePremLoading_Insured5 { get; set; }
        public decimal? basePremLoading_Insured6 { get; set; }
        public decimal? basePrem_Loading { get; set; }
        public decimal? easyHealth_BaseAndLoading_Premium { get; set; }
        public decimal? easyHealth_Loyalty_Discount { get; set; }
        public decimal? easyHealth_Employee_Discount { get; set; }
        public decimal? easyHealth_Online_Discount { get; set; }
        public decimal? easyHealth_Family_Discount { get; set; }
        public decimal? easyHealth_LongTerm_Discount { get; set; }
        public decimal? easyHealth_ORase_Premium { get; set; }
        public string? hdc_opt { get; set; }
        public decimal? hdc_si { get; set; }
        public decimal? hdc_rider_premium { get; set; }
        public decimal? hdc_family_discount { get; set; }
        public decimal? hdc_longterm_discount { get; set; }
        public decimal? hdc_final_premium { get; set; }
        public decimal? hdc_Rider_Premium1 { get; set; }
        public decimal? hdc_Rider_Premium2 { get; set; }
        public decimal? hdc_Rider_Premium3 { get; set; }
        public decimal? hdc_Rider_Premium4 { get; set; }
        public decimal? hdc_Rider_Premium5 { get; set; }
        public decimal? hdc_Rider_Premium6 { get; set; }
        public string? cI_rider_Opt { get; set; }
        public decimal? cI_si_1 { get; set; }
        public decimal? cI_si_2 { get; set; }
        public decimal? cI_si_3 { get; set; }
        public decimal? cI_si_4 { get; set; }
        public decimal? cI_si_5 { get; set; }
        public decimal? cI_si_6 { get; set; }
        public decimal? criticalIllness_Rider_Insured1 { get; set; }
        public decimal? criticalIllness_Rider_Insured2 { get; set; }
        public decimal? criticalIllness_Rider_Insured3 { get; set; }
        public decimal? criticalIllness_Rider_Insured4 { get; set; }
        public decimal? criticalIllness_Rider_Insured5 { get; set; }
        public decimal? criticalIllness_Rider_Insured6 { get; set; }
        public decimal? criticalIllness_Rider_BasePremium { get; set; }
        public decimal? criticalIllness_Rider_FamilyDiscount { get; set; }
        public decimal? criticalIllness_Rider_LongTermDiscount { get; set; }
        public decimal? criticalIllness_Rider_Premium { get; set; }
        public string? pr_opt { get; set; }
        public decimal? pr_insured_1 { get; set; }
        public decimal? pr_insured_2 { get; set; }
        public decimal? pr_insured_3 { get; set; }
        public decimal? pr_insured_4 { get; set; }
        public decimal? pr_insured_5 { get; set; }
        public decimal? pr_insured_6 { get; set; }
        public decimal? pr_ProtectorRider_Premium { get; set; }
        public decimal? pr_loading_insured1 { get; set; }
        public decimal? pr_loading_insured2 { get; set; }
        public decimal? pr_loading_insured3 { get; set; }
        public decimal? pr_loading_insured4 { get; set; }
        public decimal? pr_loading_insured5 { get; set; }
        public decimal? pr_loading_insured6 { get; set; }
        public decimal? pr_protectorriderloading_premium { get; set; }
        public decimal? pr_BaseLoading_Premium { get; set; }
        public decimal? pr_Family_Discount { get; set; }
        public decimal? pr_LongTerm_Discount { get; set; }
        public decimal? prpremium_Protector_Rider_Premium { get; set; }
        public string? individual_personalAR_opt { get; set; }
        public decimal? individual_personalAR_SI { get; set; }
        public decimal? individual_personalAR_Amt { get; set; }
        public decimal? individual_personalAR_LongTermDiscount { get; set; }
        public decimal? individual_Personal_AccidentRiderPremium { get; set; }
        public string? criticalAdvantage_Rider_opt { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_1 { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_2 { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_3 { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_4 { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_5 { get; set; }
        public decimal? criticalAdvantageRider_SumInsured_6 { get; set; }
        public decimal? criticalAdvantageRider_Insured_1 { get; set; }
        public decimal? criticalAdvantageRider_Insured_2 { get; set; }
        public decimal? criticalAdvantageRider_Insured_3 { get; set; }
        public decimal? criticalAdvantageRider_Insured_4 { get; set; }
        public decimal? criticalAdvantageRider_Insured_5 { get; set; }
        public decimal? criticalAdvantageRider_Insured_6 { get; set; }
        public decimal? criticalAdvantage_RiderBase_Premium { get; set; }
        public decimal? criticalAdvrider_loadinginsured1 { get; set; }
        public decimal? criticalAdvrider_loadinginsured2 { get; set; }
        public decimal? criticalAdvrider_loadinginsured3 { get; set; }
        public decimal? criticalAdvrider_loadinginsured4 { get; set; }
        public decimal? criticalAdvrider_loadinginsured5 { get; set; }
        public decimal? criticalAdvrider_loadinginsured6 { get; set; }
        public decimal? criticalAdvriderloading { get; set; }
        public decimal? criticalAdvriderbase_loading_premium { get; set; }
        public decimal? criticalAdvRiderPremium_Family_Discount { get; set; }
        public decimal? criticalAdvRiderPremium_LongTerm_Discount { get; set; }
        public decimal? criticalAdv_Rider_Premium { get; set; }
        public decimal? net_premium { get; set; }
        public decimal? final_Premium { get; set; }
        public decimal? gst { get; set; }
        public decimal? cross_Check { get; set; }
        public decimal? easyhealth_total_Premium { get; set; }
        public decimal? easyhealth_netpremium { get; set; }
        public decimal? easy_health_gst { get; set; }
        public decimal? verified_total_Premium { get; set; }
        public decimal? verified_netpremium { get; set; }
        public decimal? verified_gst { get; set; }
        public int? eldest_member { get; set; }
        public int? no_of_members { get; set; }
        public decimal? final_Premium_upsell { get; set; }
    }
}
