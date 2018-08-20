using System;
using Newtonsoft.Json;

namespace Beeline.Jasper
{
    public sealed class DeviceDetails
    {
        [JsonProperty("iccid")]
        public string Iccid;

        [JsonProperty("imsi")]
        public string Imsi;

        [JsonProperty("msisdn")]
        public string Msisdn;
        public string imei;
        public string status;
        public string ratePlan;
        public string communicationPlan;
        public object customer;
        public object endConsumerId;
        public DateTime dateActivated;
        public DateTime dateAdded;
        public DateTime dateUpdated;
        public DateTime dateShipped;
        public string accountId;
        public object fixedIPAddress;
        public string accountCustom1;
        public string accountCustom2;
        public string accountCustom3;
        public string accountCustom4;
        public string accountCustom5;
        public string accountCustom6;
        public string accountCustom7;
        public string accountCustom8;
        public string accountCustom9;
        public string accountCustom10;
        public object simNotes;
        public object deviceID;
        public string modemID;
        public string globalSimType;
    }
}