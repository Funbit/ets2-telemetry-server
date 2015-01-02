using System.Net.Http;
using System.Web.Http;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("apps/ets2")]
    public class Ets2AppController : StaticFileController
    {
        public const string TelemetryAppUriPath = "/apps/ets2/index.htm";

        const string AppName = "Ets2";

        [HttpGet]
        [Route("{fileName}")]
        public HttpResponseMessage GetRootFile(string fileName)
        {
            return ServeStaticFile(AppName, "", fileName);
        }

        [HttpGet]
        [Route("gfx/{fileName}")]
        public HttpResponseMessage GetGfxFile(string fileName)
        {
            return ServeStaticFile(AppName, "gfx", fileName);
        }

        [HttpGet]
        [Route("styles/{fileName}")]
        public HttpResponseMessage GetStyleFile(string fileName)
        {
            return ServeStaticFile(AppName, "styles", fileName);
        }

        [HttpGet]
        [Route("scripts/{fileName}")]
        public HttpResponseMessage GetScriptFile(string fileName)
        {
            return ServeStaticFile(AppName, "scripts", fileName);
        }
    }
}