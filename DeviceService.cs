using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeline.Jasper.Http;

namespace Beeline.Jasper
{
    public sealed class DeviceService
    {
        private static readonly DateTime defaultModifiedSince
            = DateTime.Today.AddYears(-10);
        private readonly string key;

        public DeviceService(string key)
        {
            this.key = key;
        }

        public Task<DeviceDetails> GetDeviceDetails(long iccid)
            => new DeviceDetailsRequest(key, iccid).GetResponse();

        public Task<SessionDetails> GetSessionDetails(long iccid)
            => new SessionDetailsRequest(key, iccid).GetResponse();

        public Task<SearchDeviceResponse> GetDevices(DateTime modifiedSince,
            int? pageNumber = null, int? pageSize = 50, Status? status = null, long? accountId = null)
        {
            return new SearchDeviceRequest(key, modifiedSince, pageNumber, pageSize, status, accountId)
                .GetResponse();
        }

        public IEnumerable<Device> GetAllDevices(Status? status = null, DateTime? modifiedSince = null)
        {
            var date = modifiedSince ?? defaultModifiedSince;
            var page = 1;

            SearchDeviceResponse resp = null;

            do
            {
                resp = new SearchDeviceRequest(key, date, page, null, status).GetResponse().Result;

                foreach(var device in resp.Devices)
                {
                    yield return device;
                }

                page = resp.PageNumber + 1;
            } while(!resp?.LastPage ?? true);
        }

        public IEnumerable<DeviceDetails> GetAllDeviceDetails(Status? status = null, DateTime? modifiedSince = null)
        {
            return GetAllDevices(status, modifiedSince).Select( x => GetDeviceDetails(x.ICCID).Result );
        }
    }
}