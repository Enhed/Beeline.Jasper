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