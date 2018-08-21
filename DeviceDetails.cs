using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Beeline.Jasper.Http;
using System.Linq;
using SharpExtension;

namespace Beeline.Jasper
{
    public class Device
    {
        [JsonProperty("iccid")]
        public long ICCID;

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status;

        [JsonProperty("ratePlan")]
        public string RatePlan;

        [JsonProperty("CommunicationPlan")]
        public string CommunicationPlan;

        public Task<DeviceDetails> GetDetails(string key)
            => new DeviceDetailsRequest(key, ICCID).GetResponse();

        public Task<SessionDetails> GetSessionDetails(string key)
            => new SessionDetailsRequest(key, ICCID).GetResponse();


        [JsonIgnore]
        public bool IsActive => Status == Status.Activated || Status == Status.ActivationReady;
    }

    public enum Status
    {
        [EnumMember(Value="ACTIVATION_READY")]
        ActivationReady,

        [EnumMember(Value="DEACTIVATED")]
        Deactivated,

        [EnumMember(Value="ACTIVATED")]
        Activated,

        [EnumMember(Value="RETIRED")]
        Retired,

        [EnumMember(Value="PURGED")]
        Purget
    }

    public sealed class DeviceDetails : Device
    {
        [JsonProperty("imsi")]
        public string IMSI;

        [JsonProperty("msisdn")]
        public string MSISDN;

        [JsonIgnore]
        public string Phone => $"+{MSISDN}";


        [JsonProperty("imei")]
        public string IMEI;


        [JsonIgnore]
        public string RelativeIMEI
            => IMEI.Take(14).CreateString();


        [JsonProperty("customer")]
        public object Customer;

        [JsonProperty("endConsumerId")]
        public object EndConsumerId;

        [JsonProperty("dateActivated")]
        public DateTime? DateActivated;

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