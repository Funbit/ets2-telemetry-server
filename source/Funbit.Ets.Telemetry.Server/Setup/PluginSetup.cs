using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Helpers;
using Microsoft.Win32;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public class PluginSetup : ISetup
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string Ets2 = "ETS2";
        const string Ats = "ATS";
        SetupStatus _status;
        
        public PluginSetup()
        {
            try
            {
                Log.Info("Checking plugin DLL files...");
                
                var ets2State = new GameState(Ets2, Settings.Instance.Ets2GamePath);
                var atsState = new GameState(Ats, Settings.Instance.AtsGamePath);

                if (ets2State.IsPluginValid() && atsState.IsPluginValid())
                {
                    _status = SetupStatus.Installed;
                }
                else
                {
                    _status = SetupStatus.Uninstalled;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                _status = SetupStatus.Failed;
            }
        }

        public SetupStatus Status => _status;

        public SetupStatus Install(IWin32Window owner)
        {
            if (_status == SetupStatus.Installed)
                return _status;

            try
            {
                var ets2State = new GameState(Ets2, Settings.Instance.Ets2GamePath);
                var atsState = new GameState(Ats, Settings.Instance.AtsGamePath);

                if (!ets2State.IsPluginValid())
                {
                    ets2State.DetectPath();
                    if (!ets2State.IsPathValid())
                        ets2State.BrowserForValidPath(owner);
                    ets2State.InstallPlugin();
                }

                if (!atsState.IsPluginValid())
                {
                    atsState.DetectPath();
                    if (!atsState.IsPathValid())
                        atsState.BrowserForValidPath(owner);
                    atsState.InstallPlugin();
                }
                
                Settings.Instance.Ets2GamePath = ets2State.GamePath;
                Settings.Instance.AtsGamePath = atsState.GamePath;
                Settings.Instance.Save();
                
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

        public SetupStatus Uninstall(IWin32Window owner)
        {
            if (_status == SetupStatus.Uninstalled)
                return _status;

            SetupStatus status;
            try
            {
                var ets2State = new GameState(Ets2, Settings.Instance.Ets2GamePath);
                var atsState = new GameState(Ats, Settings.Instance.AtsGamePath);
                ets2State.UninstallPlugin();
                atsState.UninstallPlugin();
                status = SetupStatus.Uninstalled;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                status = SetupStatus.Failed;
            }
            return status;
        }
        
        class GameState
        {
            const string InstallationSkippedPath = "N/A";
            const string TelemetryDllName = "ets2-telemetry-server.dll";
            const string TelemetryX64DllMd5 = "aeffffe2e6c3c4fd6ef1ad1eb44171cd";
            const string TelemetryX86DllMd5 = "ab473bea2dd9480e249133ec908e8252";

            readonly string _gameName;

            public GameState(string gameName, string gamePath)
            {
                _gameName = gameName;
                GamePath = gamePath;
            }

            string GameDirectoryName
            {
                get
                {
                    string fullName = "Euro Truck Simulator 2";
                    if (_gameName == Ats)
                        fullName = "American Truck Simulator";
                    return fullName;
                }
            }

            public string GamePath { get; private set; }

            public bool IsPathValid()
            {
                if (GamePath == InstallationSkippedPath)
                    return true;

                if (string.IsNullOrEmpty(GamePath))
                    return false;

                var baseScsPath = Path.Combine(GamePath, "base.scs");
                var binPath = Path.Combine(GamePath, "bin");
                bool validated = File.Exists(baseScsPath) && Directory.Exists(binPath);
                Log.InfoFormat("Validating {2} path: '{0}' ... {1}", GamePath, validated ? "OK" : "Fail", _gameName);
                return validated;
            }

            public bool IsPluginValid()
            {
                if (GamePath == InstallationSkippedPath)
                    return true;

                if (!IsPathValid())
                    return false;

                return Md5(GetTelemetryPluginDllFileName(GamePath, x64: true)) == TelemetryX64DllMd5 &&
                    Md5(GetTelemetryPluginDllFileName(GamePath, x64: false)) == TelemetryX86DllMd5;
            }

            public void InstallPlugin()
            {
                if (GamePath == InstallationSkippedPath)
                    return;

                string x64DllFileName = GetTelemetryPluginDllFileName(GamePath, x64: true);
                string x86DllFileName = GetTelemetryPluginDllFileName(GamePath, x64: false);

                Log.InfoFormat("Copying {1} x86 plugin DLL file to: {0}", x86DllFileName, _gameName);
                File.Copy(LocalEts2X86TelemetryPluginDllFileName, x86DllFileName, true);

                Log.InfoFormat("Copying {1} x64 plugin DLL file to: {0}", x64DllFileName, _gameName);
                File.Copy(LocalEts2X64TelemetryPluginDllFileName, x64DllFileName, true);
            }

            public void UninstallPlugin()
            {
                if (GamePath == InstallationSkippedPath)
                    return;

                Log.InfoFormat("Backing up plugin DLL files for {0}...", _gameName);
                string x64DllFileName = GetTelemetryPluginDllFileName(GamePath, x64: true);
                string x86DllFileName = GetTelemetryPluginDllFileName(GamePath, x64: false);
                string x86BakFileName = Path.ChangeExtension(x86DllFileName, ".bak");
                string x64BakFileName = Path.ChangeExtension(x64DllFileName, ".bak");
                if (File.Exists(x86BakFileName))
                    File.Delete(x86BakFileName);
                if (File.Exists(x64BakFileName))
                    File.Delete(x64BakFileName);
                File.Move(x86DllFileName, x86BakFileName);
                File.Move(x64DllFileName, x64BakFileName);
            }

            static string GetDefaultSteamPath()
            {
                var steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
                return steamKey?.GetValue("SteamPath") as string;
            }

            static string LocalEts2X86TelemetryPluginDllFileName => Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x86\plugins\", TelemetryDllName);

            static string LocalEts2X64TelemetryPluginDllFileName => Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x64\plugins", TelemetryDllName);
            
            static string GetPluginPath(string gamePath, bool x64)
            {
                return Path.Combine(gamePath, x64 ? @"bin\win_x64\plugins" : @"bin\win_x86\plugins");
            }

            static string GetTelemetryPluginDllFileName(string gamePath, bool x64)
            {
                string path = GetPluginPath(gamePath, x64);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return Path.Combine(path, TelemetryDllName);
            }
            
            static string Md5(string fileName)
            {
                if (!File.Exists(fileName))
                    return null;
                using (var provider = new MD5CryptoServiceProvider())
                {
                    var bytes = File.ReadAllBytes(fileName);
                    var hash = provider.ComputeHash(bytes);
                    var result = string.Concat(hash.Select(b => $"{b:x02}"));
                    return result;
                }
            }

            public void DetectPath()
            {
                GamePath = GetDefaultSteamPath();
                if (!string.IsNullOrEmpty(GamePath))
                    GamePath = Path.Combine(
                        GamePath.Replace('/', '\\'), @"SteamApps\common\" + GameDirectoryName);
            }

            public void BrowserForValidPath(IWin32Window owner)
            {
                while (!IsPathValid())
                {
                    var result = MessageBox.Show(owner,
                        @"Could not detect " + _gameName + @" game path. " +
                        @"If you do not have this game installed press [Cancel] to skip, " + 
                        @"otherwise press [OK] to select path manually." + Environment.NewLine + Environment.NewLine +
                        @"For example:" + Environment.NewLine + @"D:\STEAM\SteamApps\common\" + 
                        GameDirectoryName,
                        @"Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel)
                    {
                        GamePath = InstallationSkippedPath;
                        return;
                    }
                    var browser = new FolderBrowserDialog();
                    browser.Description = @"Select " + _gameName + @" game path";
                    browser.ShowNewFolderButton = false;
                    result = browser.ShowDialog(owner);
                    if (result == DialogResult.Cancel)
                        Environment.Exit(1);
                    GamePath = browser.SelectedPath;
                }
            }
        }
    }
}