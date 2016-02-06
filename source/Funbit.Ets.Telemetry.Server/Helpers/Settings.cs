using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public class Settings
    {
        #region Settings

        public string DefaultNetworkInterfaceId { get; set; }
        public string Ets2GamePath { get; set; }
        public string AtsGamePath { get; set; }

        public bool FirewallSetupHadErrors { get; set; }
        public bool UrlReservationSetupHadErrors { get; set; }

        #endregion

        const string ApplicationName = "ETS2-ATS Telemetry Server";
        const string SettingsName = "Settings.json";

        public static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationName);
        static readonly string SettingsFileName = Path.Combine(SettingsDirectory, SettingsName);

        public static void Clear()
        {
            if (File.Exists(SettingsFileName))
                File.Delete(SettingsFileName);
            if (Directory.Exists(SettingsDirectory))
                Directory.Delete(SettingsDirectory);
        }

        static Settings Load()
        {
            if (!File.Exists(SettingsFileName))
                return new Settings();
            return JsonConvert.DeserializeObject<Settings>(
                File.ReadAllText(SettingsFileName, Encoding.UTF8));
        }

        public void Save()
        {
            if (!Directory.Exists(SettingsDirectory))
                Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(SettingsFileName, 
                JsonConvert.SerializeObject(this, Formatting.Indented), Encoding.UTF8);
        }

        static readonly Lazy<Settings> LazyInstance = new Lazy<Settings>(Load);
        public static readonly Settings Instance = LazyInstance.Value;
    }
}