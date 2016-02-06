using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Helpers;
using Microsoft.Win32;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public class AtsPluginSetup : ISetup
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        SetupStatus _status;

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
                string gamePath = Settings.Instance.AtsGamePath;
                if (string.IsNullOrEmpty(gamePath))
                {
                    gamePath = SetupHelper.GetDefaultSteamPath();
                    if (!string.IsNullOrEmpty(gamePath))
                        gamePath = Path.Combine(
                            gamePath.Replace('/', '\\'), @"SteamApps\common\American Truck Simulator");
                }

                if (!SetGamePath(owner, gamePath))
                {
                    return _status;
                }

                // make sure that we disable old plugin (2.1.0 and earlier)
                // ReSharper disable once AssignNullToNotNullAttribute
                string oldX64PluginDllFileName = Path.Combine(SetupHelper.GetPluginPath(gamePath, x64: true),
                    "ets2-telemetry.dll");
                if (File.Exists(oldX64PluginDllFileName))
                    File.Move(oldX64PluginDllFileName,
                        Path.ChangeExtension(oldX64PluginDllFileName, ".deprecated.bak"));

                // install new plugin

                string x64DllFileName = SetupHelper.GetTelemetryPluginDllFileName(gamePath, x64: true);

                Log.InfoFormat("Copying x64 plugin DLL file to: {0}", x64DllFileName);
                File.Copy(LocalX64TelemetryPluginDllFileName, x64DllFileName, true);

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
            while (!ValidateAtsPath(gamePath))
            {
                MessageBox.Show(owner,
                    @"Could not detect ATS game path. " +
                    @"Please click OK and select it manually." + Environment.NewLine + Environment.NewLine +
                    @"For example:" + Environment.NewLine + @"D:\STEAM\SteamApps\common\American Truck Simulator",
                    @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                var browser = new FolderBrowserDialog();
                browser.Description = @"Select ATS game path";
                browser.ShowNewFolderButton = false;
                var result = browser.ShowDialog(owner);
                if (result == DialogResult.Cancel)
                {
                    // The user decided to give up. Just leave.
                    return false;
                }

                gamePath = browser.SelectedPath;
            }

            Settings.Instance.AtsGamePath = gamePath;
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
                string x64DllFileName = SetupHelper.GetTelemetryPluginDllFileName(Settings.Instance.AtsGamePath, x64: true);
                string x64BakFileName = Path.ChangeExtension(x64DllFileName, ".bak");
                if (File.Exists(x64BakFileName))
                    File.Delete(x64BakFileName);
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

        static bool ValidateAtsPath(string atsPath)
        {
            if (string.IsNullOrEmpty(atsPath))
                return false;
            var baseScsPath = Path.Combine(atsPath, "base.scs");
            var binPath = Path.Combine(atsPath, "bin");
            bool validated = File.Exists(baseScsPath) && Directory.Exists(binPath);
            Log.InfoFormat("Validating ATS path: '{0}' ... {1}", atsPath, validated ? "OK" : "Fail");
            return validated;
        }

        static string LocalX64TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x64\plugins", SetupHelper.TelemetryDllName);
            }
        }

        public AtsPluginSetup()
        {
            try
            {
                Log.Info("Checking plugin DLL files...");
                string gamePath = Settings.Instance.AtsGamePath;

                if (ValidateAtsPath(gamePath))
                {
                    _status = SetupHelper.IsAtsTelemetryPluginInstalled(gamePath) ? SetupStatus.Installed : SetupStatus.Uninstalled;
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
    }
}
