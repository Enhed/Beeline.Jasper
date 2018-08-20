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
    public abstract class Request<T>
    {
        public const string BaseAddress = "https://restapi10.jasper.com/rws/api/v1";
        public readonly string Key;
        protected abstract string Method { get; }
        protected abstract IEnumerable<KeyValuePair<string, string>> Parameters { get; }

        private string Params => Parameters == null || Parameters.Count() == 0
            ? string.Empty : Parameters.Select(kv => $"{kv.Key}={kv.Value}")
                .Join("&").Get(x => $"?{x}");

        public string FullUrl => $"{BaseAddress}/{Method}{Params}";

        public Request(string authorizationKey)
        {
            if(authorizationKey.IsNullOrWhiteSpace())
                throw new ArgumentException($"{nameof(authorizationKey)} can't be null or empty or whitespace");

            Key = authorizationKey;
        }

        public virtual async Task<T> GetResponse()
        {
            using(var response = await GetWebRequest()
                .GetResponseAsync())
                using(var reader = new StreamReader(response.GetResponseStream()))
                    return Convert(await reader.ReadToEndAsync());
        }

        protected virtual T Convert(string source) => JsonConvert.DeserializeObject<T>(source);

        public WebRequest GetWebRequest()
        {
            var request = WebRequest.Create(FullUrl);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Basic {Key}==");
            return request;
        }
    }

    public sealed class DeviceDetailsRequest : Request<DeviceDetails>
    {
        private readonly long iccid;

        public DeviceDetailsRequest(string authorizationKey, long iccid)
            : base(authorizationKey)
        {
            this.iccid = iccid;
        }

        protected override string Method => $"devices/{iccid}";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters
            => null;
    }

    public sealed class SearchDeviceRequest : Request<SearchDeviceResponse>
    {
        public SearchDeviceRequest(string authorizationKey, DateTime modifiedSince,
            int? pageNumber = null, int? pageSize = 50, Status? status = null, long? accountId = null)
            : base(authorizationKey)
        {
            var count = 1;
            if(pageNumber != null) count += 1;
            if(pageSize != null) count += 1;
            if(status != null) count += 1;
            if(accountId != null) count += 1;;

            var buf = new List<KeyValuePair<string, string>>(count);
            
            buf.Add(new KeyValuePair<string, string>(nameof(modifiedSince), modifiedSince.ToJasperString()));

            if(pageNumber != null)
                buf.Add(new KeyValuePair<string, string>(nameof(pageNumber), pageNumber.ToString()));

            if(pageSize != null)
                buf.Add(new KeyValuePair<string, string>(nameof(pageSize), pageSize.ToString()));

            if(status != null)
                buf.Add(new KeyValuePair<string, string>(nameof(status), status.ToString()));

            if(pageSize != null)
                buf.Add(new KeyValuePair<string, string>(nameof(accountId), accountId.ToString()));

            Parameters = buf;
        }

        protected override string Method => "devices";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters { get; }
    }

    public class Pagination
    {
        [JsonProperty("pageNumber")]
        public int PageNumber;

        [JsonProperty("lastPage")]
        public bool LastPage;
    }

    public sealed class SearchDeviceResponse : Pagination
    {
        [JsonProperty("devices")]
        public Device[] Devices;
    }

    public sealed class SessionDetailsRequest : Request<SessionDetails>
    {
        private readonly long iccid;

        public SessionDetailsRequest(string authorizationKey, long iccid)
            : base(authorizationKey)
        {
            this.iccid = iccid;
        }

        protected override string Method => $"devices/{iccid}/sessionInfo";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters
            => null;
    }

    public sealed class SessionDetails
    {
        [JsonProperty("iccid")]
        public long ICCID;

        [JsonProperty("ipAddress")]
        public string IpAddress;

        [JsonProperty("ipv6Address")]
        public string Ipv6Address;

        [JsonProperty("dateSessionStarted")]
        public DateTime DateSessionStarted;

        [JsonProperty("dateSessionEnded")]
        public DateTime? DateSessionEnded;

        [JsonIgnore]
        public bool InSession => DateSessionStarted != null && DateSessionEnded == null;
    }

    public sealed class SearchSMSRequest : Request<SearchSMSResponse>
    {
        public SearchSMSRequest(string authorizationKey, long iccid,
            DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, long? accountId = null)
            : base(authorizationKey)
        {

            if(fromDate == null) fromDate = DateTime.Today;
            if(toDate == null) toDate = DateTime.Now;

            var count = 3;
            if(pageNumber != null) count += 1;
            if(pageSize != null) count += 1;
            if(accountId != null) count += 1;;

            var buf = new List<KeyValuePair<string, string>>(count);

            buf.Add(new KeyValuePair<string, string>(nameof(iccid), iccid.ToString()));
            buf.Add(new KeyValuePair<string, string>(nameof(fromDate), fromDate.Value.ToJasperString()));
            buf.Add(new KeyValuePair<string, string>(nameof(toDate), toDate.Value.ToJasperString()));

            if(pageNumber != null)
                buf.Add(new KeyValuePair<string, string>(nameof(pageNumber), pageNumber.ToString()));

            if(pageSize != null)
                buf.Add(new KeyValuePair<string, string>(nameof(pageSize), pageSize.ToString()));

            if(pageSize != null)
                buf.Add(new KeyValuePair<string, string>(nameof(accountId), accountId.ToString()));

            Parameters = buf;
        }

        protected override string Method => "smsMessages";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters { get; }
    }

    public sealed class SearchSMSResponse : Pagination
    {
        [JsonProperty("smsMsgIds")]
        public long[] Ids;
    }

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

    public sealed class SendSMSRequest : Request<long>
    {
        private readonly long iccid;
        private readonly Params parameters;

        public SendSMSRequest(string authorizationKey, long iccid,
            Params parameters)
            : base(authorizationKey)
        {
            this.iccid = iccid;
            this.parameters = parameters;
        }

        public SendSMSRequest(string authorizationKey, long iccid,
            string text)
            : base(authorizationKey)
        {
            this.iccid = iccid;
            this.parameters = new Params
            {
                Text = text
            };
        }

        protected override string Method => $"devices/{iccid}/smsMessages";

        protected override IEnumerable<KeyValuePair<string, string>> Parameters => null;

        public async override Task<long> GetResponse()
        {
            var request = GetWebRequest();
            request.Method = "POST";
            request.ContentType = "application/json";
            var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(parameters));
            request.ContentLength = bytes.Length;

            using(var stream = await request.GetRequestStreamAsync())
                await stream.WriteAsync(bytes, 0, bytes.Length);

            using(var response = await request.GetResponseAsync())
                using(var reader = new StreamReader(response.GetResponseStream()))
                    return reader.ReadToEnd().Get(JsonConvert.DeserializeObject<SendSMSRequest.Response>).Id;
        }

        private sealed class Response
        {
            [JsonProperty("smsMessageId")]
            public long Id;
        }

        public sealed class Params
        {
            [JsonProperty("messageText")]
            public string Text;

            [JsonProperty("messageEncoding", NullValueHandling = NullValueHandling.Ignore)]
            public SMSDetailsRequest.Encoding? Encoding = null;

            [JsonProperty("dataCoding", NullValueHandling = NullValueHandling.Ignore)]
            public DataCoding? Coding = null;

            [JsonProperty("tpvp", NullValueHandling = NullValueHandling.Ignore)]
            public int? TimeValidityPeriod = null;
        }

        public enum DataCoding
        {
            SMSC,
            ASCII,
            Latin,
            Binary,
            Unicode
        }
    }

    public static class DateTimeExtension
    {
        public const string JasperFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public static string ToJasperString(this DateTime target)
            => target.ToString(JasperFormat);
    }
}