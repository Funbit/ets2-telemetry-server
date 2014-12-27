using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Funbit.Ets.Telemetry.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            var restJsonSettings = new JsonSerializerSettings();
            restJsonSettings.Converters.Add(new StringEnumConverter());
            restJsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            restJsonSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters
                  .JsonFormatter
                  .SerializerSettings = restJsonSettings;
            
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

            appBuilder.UseWebApi(config);
            appBuilder.UseCors(CorsOptions.AllowAll);
        }
    }
}
