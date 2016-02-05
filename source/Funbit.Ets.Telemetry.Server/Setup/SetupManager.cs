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
                new Ets2PluginSetup(),
                new FirewallSetup(), 
                new UrlReservationSetup()
            };
        }
    }
}