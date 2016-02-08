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
    public class Ets2PluginSetup : ISetup
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly SetupHelper SetupHelper = new SetupHelper();

        SetupStatus _status;

        static string LocalEts2X86TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x86\plugins\", SetupHelper.TelemetryDllName);
            }
        }

        static string LocalEts2X64TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x64\plugins", SetupHelper.TelemetryDllName);
            }
        }

        public Ets2PluginSetup()
        {
            try
            {
                Log.Info("Checking plugin DLL files...");
                string gamePath = Settings.Instance.Ets2GamePath;
                
                if (SetupHelper.IsInstalled(gamePath))
                {
                    _status = SetupHelper.IsEts2TelemetryPluginInstalled(gamePath) ? SetupStatus.Installed : SetupStatus.Uninstalled;
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

        public SetupStatus Status
        {
            get { return _status; }
        }

        public SetupStatus Install(IWin32Window owner)
        {
            if (_status == SetupStatus.Installed)
                return _status;

            try
            {
                string gamePath = Settings.Instance.Ets2GamePath;
                if (string.IsNullOrEmpty(gamePath))
                {
                    gamePath = SetupHelper.GetDefaultSteamPath();
                    if (!string.IsNullOrEmpty(gamePath))
                    {
                        gamePath = SetupHelper.GetDefaultGamePath(gamePath, "Euro Truck Simulator 2");
                    }
                }

                if (!SetGamePath(owner, gamePath))
                {
                    return _status;
                }

                // make sure that we disable old plugin (2.1.0 and earlier)
                // ReSharper disable once AssignNullToNotNullAttribute
                string oldX86PluginDllFileName = Path.Combine(SetupHelper.GetPluginPath(gamePath, x64: false),
                    "ets2-telemetry.dll");
                // ReSharper disable once AssignNullToNotNullAttribute
                string oldX64PluginDllFileName = Path.Combine(SetupHelper.GetPluginPath(gamePath, x64: true),
                    "ets2-telemetry.dll");
                if (File.Exists(oldX86PluginDllFileName))
                    File.Move(oldX86PluginDllFileName,
                        Path.ChangeExtension(oldX86PluginDllFileName, ".deprecated.bak"));
                if (File.Exists(oldX64PluginDllFileName))
                    File.Move(oldX64PluginDllFileName,
                        Path.ChangeExtension(oldX64PluginDllFileName, ".deprecated.bak"));

                // install new plugin

                string x64DllFileName = SetupHelper.GetTelemetryPluginDllFileName(gamePath, x64: true);
                string x86DllFileName = SetupHelper.GetTelemetryPluginDllFileName(gamePath, x64: false);

                Log.InfoFormat("Copying x86 plugin DLL file to: {0}", x86DllFileName);
                File.Copy(LocalEts2X86TelemetryPluginDllFileName, x86DllFileName, true);

                Log.InfoFormat("Copying x64 plugin DLL file to: {0}", x64DllFileName);
                File.Copy(LocalEts2X64TelemetryPluginDllFileName, x64DllFileName, true);

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

        public static bool SetGamePath(IWin32Window owner, string gamePath)
        {
            while (!SetupHelper.IsInstalled(gamePath))
            {
                MessageBox.Show(owner,
                    @"Could not detect ETS2 game path. " +
                    @"Please click OK and select it manually." + Environment.NewLine + Environment.NewLine +
                    @"For example:" + Environment.NewLine + @"D:\STEAM\SteamApps\common\Euro Truck Simulator 2",
                    @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                var browser = new FolderBrowserDialog
                {
                    Description = @"Select ETS2 game path",
                    ShowNewFolderButton = false
                };
                var result = browser.ShowDialog(owner);
                if (result == DialogResult.Cancel)
                {
                    // The user decided to give up. Just leave.
                    return false;
                }
                gamePath = browser.SelectedPath;
            }

            Settings.Instance.Ets2GamePath = gamePath;
            Settings.Instance.Save();
            return true;
        }

        public SetupStatus Uninstall(IWin32Window owner)
        {
            if (_status == SetupStatus.Uninstalled)
                return _status;

            SetupStatus status;
            try
            {
                // rename plugin dll files to .bak
                Log.Info("Backing up plugin DLL files...");
                string x64DllFileName = SetupHelper.GetTelemetryPluginDllFileName(Settings.Instance.Ets2GamePath, x64: true);
                string x86DllFileName = SetupHelper.GetTelemetryPluginDllFileName(Settings.Instance.Ets2GamePath, x64: false);
                string x86BakFileName = Path.ChangeExtension(x86DllFileName, ".bak");
                string x64BakFileName = Path.ChangeExtension(x64DllFileName, ".bak");
                if (File.Exists(x86BakFileName))
                    File.Delete(x86BakFileName);
                if (File.Exists(x64BakFileName))
                    File.Delete(x64BakFileName);
                File.Move(x86DllFileName, x86BakFileName);
                File.Move(x64DllFileName, x64BakFileName);
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