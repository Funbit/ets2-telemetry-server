using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class JsonHelper
    {
        static readonly Lazy<JsonSerializerSettings> RestSettingsLazy = new Lazy<JsonSerializerSettings>(() =>
        {
            var restJsonSettings = new JsonSerializerSettings();
            restJsonSettings.Converters.Add(new StringEnumConverter());
            restJsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            restJsonSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            return restJsonSettings;
        });

        public static readonly JsonSerializerSettings RestSettings = RestSettingsLazy.Value;
    }
}