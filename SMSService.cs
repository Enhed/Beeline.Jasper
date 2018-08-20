using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeline.Jasper.Http;

namespace Beeline.Jasper
{
        public sealed class SMSService
    {
        private readonly string key;

        public SMSService(string key)
        {
            this.key = key;
        }

        public Task<long> Send(long iccid, SendSMSRequest.Params parameters)
        {
            return new SendSMSRequest(key, iccid, parameters).GetResponse();
        }

        public Task<long> Send(long iccid, string text)
        {
            return new SendSMSRequest(key, iccid, text).GetResponse();
        }

        public Task<SMSDetails> GetDetails(long id, SMSDetailsRequest.Encoding? encoding = null)
        {
            return new SMSDetailsRequest(key, id, encoding).GetResponse();
        }

        public Task<SearchSMSResponse> Search(long iccid, DateTime? fromDate = null, DateTime? toDate = null,
            int? pageSize = null, int? pageNumber = null, long? accountId = null)
        {
            return new SearchSMSRequest(key, iccid, fromDate, toDate, pageSize, pageNumber, accountId).GetResponse();
        }

        public IEnumerable<SMSDetails> GetAllDetails(long iccid, DateTime? fromDate = null, DateTime? toDate = null,
            int? pageSize = null, int? pageNumber = null, long? accountId = null)
        {
            var page = 1;

            SearchSMSResponse resp = null;

            do
            {
                resp = Search(iccid, fromDate, toDate, pageSize, pageNumber, accountId).Result;

                // Console.WriteLine($"[GetAllDetails]: [{iccid}] page x{page} = x[{resp.Ids.Length}] ({resp.Ids.CreateString(",")})");

                foreach(var device in resp.Ids.Select(id => GetDetails(id).Result))
                {
                    yield return device;
                }

                page = resp.PageNumber + 1;
            } while(!resp?.LastPage ?? true);
        }
    }
}