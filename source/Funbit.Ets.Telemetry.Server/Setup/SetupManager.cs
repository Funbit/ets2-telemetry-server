using System.Linq;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public static class SetupManager
    {
        public static ISetup[] Steps;

        static SetupManager()
        {
            Steps = new ISetup[]
            {
                // visible to user:
                new PluginSetup(), 
                new FirewallSetup(), 
                new UrlReservationSetup(),
                // internal:
                new SettingsSetup()
            };
        }
    }
}