using System.Net.Http;
using System.Web.Http;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("")]
    public class Ets2AppController : StaticFileController
    {
        public const string TelemetryAppUriPath = "/";

        [HttpGet]
        [Route("", Name = "GetRoot")]
        public HttpResponseMessage GetRoot()
        {
            return ServeStaticFile("", "index.html");
        }

        [HttpGet]
        [Route("{fileName}", Name = "GetRootFile")]
        public HttpResponseMessage GetRootFile(string fileName)
        {
            return ServeStaticFile("", fileName);
        }

        [HttpGet]
        [Route("{dirName}/{fileName}", Name = "GetResourceFile")]
        public HttpResponseMessage GetResourceFile(string dirName, string fileName)
        {
            return ServeStaticFile(dirName, fileName);
        }
    }
}