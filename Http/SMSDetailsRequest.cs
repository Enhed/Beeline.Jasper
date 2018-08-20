using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharpExtension;

namespace Beeline.Jasper.Http
{
    public sealed class SMSDetailsRequest : Request<SMSDetails>
    {
        private readonly long id;

        public SMSDetailsRequest(string authorizationKey, long id, Encoding? messageEncoding = null)
            : base(authorizationKey)
        {
            this.id = id;

            if(messageEncoding != null)
            {
                Parameters = new [] { new KeyValuePair<string, string>(nameof(messageEncoding), messageEncoding.ToString()) };
            }
        }

        protected override string Method => $"smsMessages/{id}";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters { get; } = null;

        public enum Encoding
        {
            LITERAL,
            BASE64
        }
    }

    public sealed class SMSDetails
    {
        [JsonProperty("smsMsgId")]
        public long Id;

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveryStatus Status;

        [JsonProperty("messageText")]
        public string MessageText;

        [JsonProperty("senderLogin")]
        public string SenderLogin;

        [JsonProperty("iccid")]
        public long ICCID;

        [JsonProperty("sentTo")]
        public string SentTo;

        [JsonProperty("sentFrom")]
        public string SentFrom;

        [JsonProperty("msgType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type;

        [JsonProperty("dateSent")]
        public DateTime? DateSent;

        [JsonProperty("dateReceived")]
        public DateTime DateReceived;


        [JsonProperty("dateModified")]
        public DateTime DateModified;

        public enum DeliveryStatus
        {
            Received,
            Cancelled,
            CancelFailed,
            CancelPending,
            Delivered,
            Pending,
            Failed,

            [EnumMember(Value="Delivered-InLog")]
            DeliveredInLog,

            Unknown
        }

        public enum MessageType
        {
            [EnumMember(Value = "MT")]
            MobileTerminated,

            [EnumMember(Value = "MO")]
            MobileOriginated
        }
    }
}