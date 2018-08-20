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
}