using System;
using Newtonsoft.Json;

namespace Beeline.Jasper
{
    public sealed class DeviceDetails
    {
        [JsonProperty("iccid")]
        public string ICCID;

        [JsonProperty("imsi")]
        public string IMSI;

        [JsonProperty("msisdn")]
        public string MSISDN;

        [JsonProperty("imei")]
        public string IMEI;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("ratePlan")]
        public string RatePlan;

        [JsonProperty("CommunicationPlan")]
        public string CommunicationPlan;

        [JsonProperty("customer")]
        public object Customer;

        [JsonProperty("endConsumerId")]
        public object EndConsumerId;

        [JsonProperty("dateActivated")]
        public DateTime DateActivated;

        [JsonProperty("dateAdded")]
        public DateTime DateAdded;

        [JsonProperty("dateUpdated")]
        public DateTime DateUpdated;

        [JsonProperty("dateShipped")]
        public DateTime DateShipped;

        [JsonProperty("accountId")]
        public string AccountId;

        [JsonProperty("fixedIPAddress")]
        public object FixedIPAddress;

        [JsonProperty("AccountCustom1")]
        public string AccountCustom1;

        [JsonProperty("accountCustom2")]
        public string AccountCustom2;

        [JsonProperty("accountCustom3")]
        public string AccountCustom3;

        [JsonProperty("accountCustom4")]
        public string AccountCustom4;

        [JsonProperty("accountCustom5")]
        public string AccountCustom5;

        [JsonProperty("accountCustom6")]
        public string AccountCustom6;

        [JsonProperty("accountCustom7")]
        public string AccountCustom7;

        [JsonProperty("accountCustom8")]
        public string AccountCustom8;

        [JsonProperty("accountCustom9")]
        public string AccountCustom9;

        [JsonProperty("accountCustom10")]
        public string AccountCustom10;

        [JsonProperty("simNotes")]
        public object SimNotes;

        [JsonProperty("deviceID")]
        public object DeviceID;

        [JsonProperty("modemID")]
        public string ModemID;

        [JsonProperty("globalSimType")]
        public string GlobalSimType;
    }
}