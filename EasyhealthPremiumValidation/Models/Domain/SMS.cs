using Newtonsoft.Json;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class MergeTriggerRecordData
    {
        [JsonProperty("mergeTriggerRecords")]
        public List<MergeTriggerRecord> MergeTriggerRecords { get; set; }

        [JsonProperty("fieldNames")]
        public List<string> FieldNames { get; set; }
    }

    public class MergeTriggerRecord
    {
        [JsonProperty("fieldValues")]
        public List<string> FieldValues { get; set; }

        [JsonProperty("optionalData")]
        public List<OptionalData> OptionalData { get; set; }
    }

    public class OptionalData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class MergeRule
    {
        [JsonProperty("htmlValue")]
        public string HtmlValue { get; set; }

        [JsonProperty("matchColumnName1")]
        public string MatchColumnName1 { get; set; }

        [JsonProperty("optoutValue")]
        public string OptoutValue { get; set; }

        [JsonProperty("insertOnNoMatch")]
        public bool InsertOnNoMatch { get; set; }

        [JsonProperty("defaultPermissionStatus")]
        public string DefaultPermissionStatus { get; set; }

        [JsonProperty("rejectRecordIfChannelEmpty")]
        public string RejectRecordIfChannelEmpty { get; set; }

        [JsonProperty("optinValue")]
        public string OptinValue { get; set; }

        [JsonProperty("updateOnMatch")]
        public string UpdateOnMatch { get; set; }

        [JsonProperty("textValue")]
        public string TextValue { get; set; }

        [JsonProperty("matchOperator")]
        public string MatchOperator { get; set; }
    }

    public class RequestData
    {
        [JsonProperty("mergeTriggerRecordData")]
        public MergeTriggerRecordData MergeTriggerRecordData { get; set; }

        [JsonProperty("mergeRule")]
        public MergeRule MergeRule { get; set; }

    }
}
