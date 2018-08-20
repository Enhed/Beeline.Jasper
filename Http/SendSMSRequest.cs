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
}