using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public class SetupHelper
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string TelemetryDllName = "ets2-telemetry-server.dll";
        public const string TelemetryX64DllMd5 = "d9f6b478e953e86e3bfc077d3ca3ded7";
        public const string TelemetryX86DllMd5 = "9d65d492dd9126b68fa277f607afaa69";

        public static string GetDefaultSteamPath()
        {
            var steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");

            return steamKey?.GetValue("SteamPath") as string;
        }

        public static string GetEts2InstallDirectory()
        {
            return string.IsNullOrEmpty(Settings.Instance.Ets2GamePath)
                ? GetDefaultGamePath(GetDefaultSteamPath(), "Euro Truck Simulator 2")
                : Settings.Instance.Ets2GamePath;
        }

        public static string GetAtsInstallDirectory()
        {
            return string.IsNullOrEmpty(Settings.Instance.AtsGamePath)
                ? GetDefaultGamePath(GetDefaultSteamPath(), "American Truck Simulator")
                : Settings.Instance.AtsGamePath;
        }

        /// <summary>
        /// This checks to see if the Telemetry plugin DLL is already installed for ETS2. This should only be called by the InitialSetupForm.
        /// </summary>
        /// <returns>True if the telemetry plugin is installed, false otherwise.</returns>
        public static bool IsEts2TelemetryPluginInstalled(string gamePath)
        {
            if (string.IsNullOrEmpty(gamePath))
            {
                throw new ArgumentException("gamePath cannot be null.");
            }

            return Md5(GetTelemetryPluginDllFileName(gamePath, x64: true)) == TelemetryX64DllMd5 &&
                                        Md5(GetTelemetryPluginDllFileName(gamePath, x64: false)) == TelemetryX86DllMd5;
        }

        public static string GetTelemetryPluginDllFileName(string gamePath, bool x64)
        {
            string path = GetPluginPath(gamePath, x64);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, TelemetryDllName);
        }

        public static string GetPluginPath(string path, bool x64)
        {
            return Path.Combine(path, x64 ? @"bin\win_x64\plugins" : @"bin\win_x86\plugins");
        }

        /// <summary>
        /// This checks to see if the Telemetry plugin DLL is already installed for ATS. This should only be called by the InitialSetupForm.
        /// </summary>
        /// <returns>True if the telemetry plugin is installed, false otherwise.</returns>
        public static bool IsAtsTelemetryPluginInstalled(string gamePath)
        {
            if (string.IsNullOrEmpty(gamePath))
            {
                throw new ArgumentException("gamePath cannot be null.");
            }

            return Md5(GetTelemetryPluginDllFileName(gamePath, true)) == TelemetryX64DllMd5;
        }

        public static bool IsInstalled(string gamePath)
        {
            if (string.IsNullOrEmpty(gamePath))
                return false;
            var baseScsPath = Path.Combine(gamePath, "base.scs");
            var binPath = Path.Combine(gamePath, "bin");
            bool validated = File.Exists(baseScsPath) && Directory.Exists(binPath);
            Log.InfoFormat("Validating ETS2 path: '{0}' ... {1}", gamePath, validated ? "OK" : "Fail");
            return validated;
        }

        public static string GetDefaultGamePath(string gamePath, string gameTitle)
        {
            return Path.Combine(gamePath.Replace("/", "\\"), $@"SteamApps\common\{gameTitle}");
        }

        static string Md5(string fileName)
        {
            if (!File.Exists(fileName))
                return null;
            using (var provider = new MD5CryptoServiceProvider())
            {
                var bytes = File.ReadAllBytes(fileName);
                byte[] hash = provider.ComputeHash(bytes);
                var result = string.Concat(hash.Select(b => $"{b:x02}"));
                return result;
            }
        }
    }
}
