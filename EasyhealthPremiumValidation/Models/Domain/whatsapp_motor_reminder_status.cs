using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERGPremiumValidationSchedular.Model.Domain
{
    public class whatsapp_motor_reminder_status
    {
        public string POLICY_NO { get; set; }
        public string JOB_REQ_ID { get; set; }
        public DateTime WA_REQ_SENT_DATE { get; set; }
        public DateTime SRC_INSERT_DATE { get; set; }
        public string WA_API_STATUS { get; set; }
        public string MOBILE_NUM { get; set; }
        public string From_RG_DB { get; set; }
    }
}
