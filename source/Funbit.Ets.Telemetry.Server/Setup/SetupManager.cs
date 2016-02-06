using System.Linq;
using System.Windows.Forms;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public static class SetupManager
    {
        public static ISetup[] Ets2Steps;
        public static ISetup[] AtsSteps;

        static SetupManager()
        {
            Ets2Steps = new ISetup[]
            {
                new Ets2PluginSetup(),
                new FirewallSetup(), 
                new UrlReservationSetup()
            };
            
            AtsSteps = new ISetup[]
            {
                new AtsPluginSetup(), 
                new FirewallSetup(), 
                new UrlReservationSetup()
            };
        }
    }
}