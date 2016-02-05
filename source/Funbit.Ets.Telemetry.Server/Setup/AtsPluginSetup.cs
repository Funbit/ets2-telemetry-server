﻿using System;
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
        const string TelemetryDllName = "ets2-telemetry-server.dll";
        const string TelemetryX64DllMd5 = "d9f6b478e953e86e3bfc077d3ca3ded7";
        const string TelemetryX86DllMd5 = "9d65d492dd9126b68fa277f607afaa69";

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
                    gamePath = GetDefaultSteamPath();
                    if (!string.IsNullOrEmpty(gamePath))
                        gamePath = Path.Combine(
                            gamePath.Replace('/', '\\'), @"SteamApps\common\American Truck Simulator");
                }

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
                        Environment.Exit(1);
                    gamePath = browser.SelectedPath;
                }

                Settings.Instance.AtsGamePath = gamePath;
                Settings.Instance.Save();

                // make sure that we disable old plugin (2.1.0 and earlier)
                // ReSharper disable once AssignNullToNotNullAttribute
                string oldX86PluginDllFileName = Path.Combine(GetPluginPath(gamePath, x64: false),
                    "ets2-telemetry.dll");
                // ReSharper disable once AssignNullToNotNullAttribute
                string oldX64PluginDllFileName = Path.Combine(GetPluginPath(gamePath, x64: true),
                    "ets2-telemetry.dll");
                if (File.Exists(oldX86PluginDllFileName))
                    File.Move(oldX86PluginDllFileName,
                        Path.ChangeExtension(oldX86PluginDllFileName, ".deprecated.bak"));
                if (File.Exists(oldX64PluginDllFileName))
                    File.Move(oldX64PluginDllFileName,
                        Path.ChangeExtension(oldX64PluginDllFileName, ".deprecated.bak"));

                // install new plugin

                string x64DllFileName = GetTelemetryPluginDllFileName(gamePath, x64: true);
                string x86DllFileName = GetTelemetryPluginDllFileName(gamePath, x64: false);

                Log.InfoFormat("Copying x86 plugin DLL file to: {0}", x86DllFileName);
                File.Copy(LocalX86TelemetryPluginDllFileName, x86DllFileName, true);

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

        public SetupStatus Uninstall(IWin32Window owner)
        {
            if (_status == SetupStatus.Uninstalled)
                return _status;

            SetupStatus status;
            try
            {
                // rename plugin dll files to .bak
                Log.Info("Backing up plugin DLL files...");
                string x64DllFileName = GetTelemetryPluginDllFileName(Settings.Instance.AtsGamePath, x64: true);
                string x86DllFileName = GetTelemetryPluginDllFileName(Settings.Instance.AtsGamePath, x64: false);
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

        static string GetDefaultSteamPath()
        {
            var steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");

            return steamKey?.GetValue("SteamPath") as string;
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

        static string GetPluginPath(string atsPath, bool x64)
        {
            return Path.Combine(atsPath, x64 ? @"bin\win_x64\plugins" : @"bin\win_x86\plugins");
        }

        static string GetTelemetryPluginDllFileName(string atsPath, bool x64)
        {
            string path = GetPluginPath(atsPath, x64);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, TelemetryDllName);
        }

        static string LocalX86TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x86\plugins\", TelemetryDllName);
            }
        }

        static string LocalX64TelemetryPluginDllFileName
        {
            get
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"Ets2Plugins\win_x64\plugins", TelemetryDllName);
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
                    bool pluginsExist = Md5(GetTelemetryPluginDllFileName(gamePath, x64: true)) == TelemetryX64DllMd5 &&
                                        Md5(GetTelemetryPluginDllFileName(gamePath, x64: false)) == TelemetryX86DllMd5;
                    _status = pluginsExist ? SetupStatus.Installed : SetupStatus.Uninstalled;
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

        static string Md5(string fileName)
        {
            if (!File.Exists(fileName))
                return null;
            using (var provider = new MD5CryptoServiceProvider())
            {
                var bytes = File.ReadAllBytes(fileName);
                byte[] hash = provider.ComputeHash(bytes);
                var result = string.Concat(hash.Select(b => string.Format("{0:x02}", b)));
                return result;
            }
        }
    }
}
