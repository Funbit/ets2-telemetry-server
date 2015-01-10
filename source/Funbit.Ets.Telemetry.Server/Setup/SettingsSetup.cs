using System.IO;
using Funbit.Ets.Telemetry.Server.Helpers;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public class SettingsSetup : ISetup
    {
        public SetupStatus Status
        {
            get
            {
                return Directory.Exists(Settings.SettingsDirectory) 
                    ? SetupStatus.Installed : SetupStatus.Uninstalled;
            }
        }

        public SetupStatus Install()
        {
            return SetupStatus.Installed;
        }

        public SetupStatus Uninstall()
        {
            Settings.Clear();
            return SetupStatus.Uninstalled;
        }
    }
}