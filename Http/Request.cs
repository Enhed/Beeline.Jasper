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

    public class Pagination
    {
        [JsonProperty("pageNumber")]
        public int PageNumber;

        [JsonProperty("lastPage")]
        public bool LastPage;
    }

    public static class DateTimeExtension
    {
        public const string JasperFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public static string ToJasperString(this DateTime target)
            => target.ToString(JasperFormat);
    }
}