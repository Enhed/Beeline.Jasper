using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                    return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync());
        }

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
}