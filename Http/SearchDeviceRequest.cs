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

    public sealed class SearchDeviceResponse : Pagination
    {
        [JsonProperty("devices")]
        public Device[] Devices;
    }
}