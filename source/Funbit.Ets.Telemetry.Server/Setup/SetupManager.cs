namespace Funbit.Ets.Telemetry.Server.Setup
{
    public static class SetupManager
    {
        public static ISetup[] Steps;

        static SetupManager()
        {
            Steps = new ISetup[]
            {
                new PluginSetup(), 
                new FirewallSetup(), 
                new UrlReservationSetup()
            };
        }
    }
}