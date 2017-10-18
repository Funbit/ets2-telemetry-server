using System.Web.Http;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public partial class Ets2AppController
    {
        [HttpGet]
        [Route("chart/{dbname}/{tablename}")]
        public IHttpActionResult GetData(string dbname, string tablename)
        {
            var result = new[] { "hello", "world" };
            return Ok(result);
        }
    }
}