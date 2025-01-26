using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERGPremiumValidationSchedular.Model.Domain
{
    public class idst_ora_respo_delivery_status
    {
        public string POLICY_NO { get; set; }
        public string DYN_JOB_REQ_ID { get; set; }
        public string From_RG_DB { get; set; }
        public string SUBJECT { get; set; }
        public DateTime EVENT_CAPTURED_DT { get; set; }
        public DateTime SRC_INSERT_DATE { get; set; }
        public string RECORD_TYPE { get; set; }
        public string EMAIL { get; set; }
        public string EMAIL_ADDRESS { get; set; }
    }
}
