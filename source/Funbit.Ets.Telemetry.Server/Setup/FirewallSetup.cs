using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Helpers;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public class FirewallSetup : ISetup
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        static readonly string FirewallRuleName = $"ETS2 TELEMETRY SERVER (PORT {ConfigurationManager.AppSettings["Port"]})";

        SetupStatus _status;

        public FirewallSetup()
        {
            try
            {
                if (Settings.Instance.FirewallSetupHadErrors)
                {
                    _status = SetupStatus.Installed;
                }
                else
                {
                    string port = ConfigurationManager.AppSettings["Port"];
                    const string arguments = "advfirewall firewall show rule dir=in name=all";
                    Log.Info("Checking Firewall rule...");
                    string output = ProcessHelper.RunNetShell(arguments, "Failed to check Firewall rule status");
                    // this check is kind of lame, but it works in any locale...
                    _status = output.Contains(port) && output.Contains(FirewallRuleName)
                        ? SetupStatus.Installed : SetupStatus.Uninstalled;
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
                string port = ConfigurationManager.AppSettings["Port"];
                string arguments = $"advfirewall firewall add rule name=\"{FirewallRuleName}\" " +
                                   $"dir=in action=allow protocol=TCP localport={port} remoteip=localsubnet";
                Log.Info("Adding Firewall rule...");
                ProcessHelper.RunNetShell(arguments, "Failed to add Firewall rule");
                _status = SetupStatus.Installed;
            }
            catch (Exception ex)
            {
                _status = SetupStatus.Failed;
                Log.Error(ex);
                Settings.Instance.FirewallSetupHadErrors = true;
                Settings.Instance.Save();
                throw new Exception("Cannot configure Windows Firewall." + Environment.NewLine +
                                    "If you are using some 3rd-party firewall please open " +
                                    ConfigurationManager.AppSettings["Port"] + " TCP port manually!", ex);
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
                string arguments = $"advfirewall firewall delete rule name=\"{FirewallRuleName}\"";
                Log.Info("Deleting Firewall rule...");
                ProcessHelper.RunNetShell(arguments, "Failed to delete Firewall rule");
                status = SetupStatus.Uninstalled;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                _status = SetupStatus.Failed;
                throw new Exception("Cannot configure Windows Firewall." + Environment.NewLine +
                                    "If you are using some 3rd-party firewall please close " +
                                    ConfigurationManager.AppSettings["Port"] + " TCP port manually!", ex);
            }
            return status;
        }
    }
}