using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERGPremiumValidationSchedular.Model.Domain
{
    public class idst_ora_respo_sms_status
    {
        public string POLICY_NO { get; set; }
        public string DYN_JOB_REQ_ID { get; set; }
        public string DYN_MESSAGE_CONTENT { get; set; }
        public DateTime EVENT_CAPTURED_DT { get; set; }
        public DateTime SRC_INSERT_DATE { get; set; }
        public string RECORD_TYPE { get; set; }
        public string MOBILE_NUMBER { get; set; }
    }
}
