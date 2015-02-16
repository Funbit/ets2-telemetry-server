using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public class PluginSetup : ISetup
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        SetupStatus _status = SetupStatus.Uninstalled;
        const string TelemetryDllName = "ets2-telemetry-server.dll";
        
        static string Ets2GamePath
        {
            get
            {
                string gamePath = ConfigurationManager.AppSettings["Ets2GamePath"];
                if (string.IsNullOrWhiteSpace(gamePath))
                {
                    var steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
                    if (steamKey != null)
                    {
                        string steamPath = steamKey.GetValue("SteamPath") as string;
                        if (steamPath != null)
                            gamePath = Path.Combine(
                                steamPath.Replace('/', '\\'), @"SteamApps\common\Euro Truck Simulator 2");
                    }
                    if (string.IsNullOrWhiteSpace(gamePath))
                        throw new Exception("Could not detect game directory. " +
                        "Please make sure that you have a Steam version, " + 
                        "otherwise Ets2GamePath must be defined in the app.config file.");
                }
                return gamePath;
            }
        }

        static string LocalEts2X86TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x86\plugins\", TelemetryDllName);
            }
        }

        static string LocalEts2X64TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x64\plugins", TelemetryDllName);
            }
        }

        static string Ets2X86TelemetryPluginDllFileName
        {
            get
            {
                string pluginDirectory = Path.Combine(Ets2GamePath, @"bin\win_x86\plugins");
                if (!Directory.Exists(pluginDirectory))
                    Directory.CreateDirectory(pluginDirectory);
                return Path.Combine(pluginDirectory, TelemetryDllName);
            }
        }

        static string Ets2X64TelemetryPluginDllFileName
        {
            get
            {
                string pluginDirectory = Path.Combine(Ets2GamePath, @"bin\win_x64\plugins");
                if (!Directory.Exists(pluginDirectory))
                    Directory.CreateDirectory(pluginDirectory);
                return Path.Combine(pluginDirectory, TelemetryDllName);
            }
        }

        public PluginSetup()
        {
            try
            {
                Log.Info("Checking plugin DLL files...");
                bool pluginsExist = File.Exists(Ets2X86TelemetryPluginDllFileName) &&
                                    File.Exists(Ets2X64TelemetryPluginDllFileName);
                _status = pluginsExist ? SetupStatus.Installed : SetupStatus.Uninstalled;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                _status = SetupStatus.Failed;
            }
        }

        public SetupStatus Status
        {
            get { return _status; }
        }

        public SetupStatus Install()
        {
            if (_status == SetupStatus.Installed)
                return _status;

            try
            {
                Log.InfoFormat("Copying x86 plugin DLL file to: {0}", Ets2X86TelemetryPluginDllFileName);
                if (!File.Exists(Ets2X86TelemetryPluginDllFileName))
                    File.Copy(LocalEts2X86TelemetryPluginDllFileName, Ets2X86TelemetryPluginDllFileName);
                
                Log.InfoFormat("Copying x64 plugin DLL file to: {0}", Ets2X64TelemetryPluginDllFileName);
                if (!File.Exists(Ets2X64TelemetryPluginDllFileName))
                    File.Copy(LocalEts2X64TelemetryPluginDllFileName, Ets2X64TelemetryPluginDllFileName);
                _status = SetupStatus.Installed;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                _status = SetupStatus.Failed;
                throw;
            }
            
            return _status;
        }

        public SetupStatus Uninstall()
        {
            if (_status == SetupStatus.Uninstalled)
                return _status;

            SetupStatus status;
            try
            {
                // rename plugin dll files to .bak
                Log.Info("Backing up plugin DLL files...");
                string x86BakFileName = Path.ChangeExtension(Ets2X86TelemetryPluginDllFileName, ".bak");
                string x64BakFileName = Path.ChangeExtension(Ets2X64TelemetryPluginDllFileName, ".bak");
                if (File.Exists(x86BakFileName))
                    File.Delete(x86BakFileName);
                if (File.Exists(x64BakFileName))
                    File.Delete(x64BakFileName);
                File.Move(Ets2X86TelemetryPluginDllFileName, x86BakFileName);
                File.Move(Ets2X64TelemetryPluginDllFileName, x64BakFileName);
                status = SetupStatus.Uninstalled;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                status = SetupStatus.Failed;
            }
            return status;
        }
    }
}