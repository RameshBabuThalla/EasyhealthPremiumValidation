using Newtonsoft.Json;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class mergeTriggerRecordDataWithAttachments

    {
        [JsonProperty("mergetriggerrecordswithattachments")]
        public List<mergeTriggerRecordsWithAttachments> MergeTriggerRecordsWithAttachments { get; set; }
        [JsonProperty("fieldnames")]
        public List<string> FieldNames { get; set; }
    }

    public class mergeTriggerRecordsWithAttachments

    {
        [JsonProperty("fieldvalues")]
        public List<string> FieldValues { get; set; }

        [JsonProperty("optionaldata")]
        public List<OptionalData> OptionalData { get; set; }

        [JsonProperty("attachmentdata")]
        public List<AttachmentData> AttachmentData { get; set; }
    }
   

    public class AttachmentData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

 

    public class MergeDataWrapper
    {
        [JsonProperty("mergeTriggerRecordDataWithAttachments")]
        public mergeTriggerRecordDataWithAttachments mergeTriggerRecordDataWithAttachments { get; set; }

        [JsonProperty("mergeRule")]
        public MergeRule MergeRule { get; set; }
    }
}
