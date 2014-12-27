using System.Net;
using System.Net.Http;
using System.Web.Http;
using Funbit.Ets.Telemetry.Server.Data;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("api")]
    public class Ets2TelemetryController : ApiController
    {
        [HttpGet]
        [HttpPost]
        [Route("ets2/telemetry", Name = "GetEts2Telemetry")]
        public HttpResponseMessage GetEts2Telemetry()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, 
                Ets2TelemetryDataReader.Instance.Read());
            return response;
        }
    }
}