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
}