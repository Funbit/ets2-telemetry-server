using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Controllers;
using Funbit.Ets.Telemetry.Server.Helpers;
using Funbit.Ets.Telemetry.Server.Setup;
using Microsoft.Owin.Hosting;

namespace Funbit.Ets.Telemetry.Server
{
    public partial class MainForm : Form
    {
        IDisposable _server;
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
        }

        static string EndpointPort
        {
            get { return ConfigurationManager.AppSettings["Port"]; }
        }

        static string IpToEndpointUrl(string host)
        {
            return string.Format("http://{0}:{1}", host, EndpointPort);
        }

        string StartServer()
        {
            string bindIp = string.Empty;
            try
            {
                var options = new StartOptions();
                options.Urls.Add(IpToEndpointUrl("+"));
                try
                {
                    bindIp = NetworkHelper.GetDefaultIpAddress(
                        ConfigurationManager.AppSettings["NetworkInterfaceId"]).ToString();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    ex.ShowAsMessageBox(this, @"Network error", MessageBoxIcon.Exclamation);
                }
                _server = WebApp.Start<Startup>(options);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, @"Server error");
            }
            return bindIp;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log.InfoFormat("Running application on {0} ({1}) {2}", Environment.OSVersion, 
                Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit",
                Program.UninstallMode ? "[UNINSTALL MODE]" : "");
            
            // show application version 
            Text += @" " + AssemblyHelper.Version;

            try
            {
                if (Program.UninstallMode || SetupManager.Steps.Any(s => s.Status != SetupStatus.Installed))
                    // we wait here until setup is complete
                    new SetupForm().ShowDialog(this);
                
                // start server
                string bindIp = StartServer();

                // show full URLs to the telemetry data
                appUrlLabel.Text = IpToEndpointUrl(bindIp) + Ets2AppController.TelemetryAppUriPath;
                apiEndpointUrlLabel.Text = IpToEndpointUrl(bindIp) + Ets2TelemetryController.TelemetryApiUriPath;
                serverIpLabel.Text = bindIp;

                // raise priority to make server more responsive
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

                // start ETS2 process status timer
                statusUpdateTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                ex.ShowAsMessageBox(this, @"Setup error");
            } 
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_server != null)
                _server.Dispose();
        }
    
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void statusUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Ets2ProcessHelper.IsEts2Running)
                {
                    statusLabel.Text = @"Simulator is running";
                    statusLabel.ForeColor = Color.DarkGreen;
                }
                else
                {
                    statusLabel.Text = @"Simulator is not running";
                    statusLabel.ForeColor = Color.FromArgb(240, 55, 30);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, @"Process error");
                statusUpdateTimer.Enabled = false;
            }
        }

        private void endpointUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteLink(sender);
        }

        private void appUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteLink(sender);
        }

        static void ExecuteLink(object sender)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(((LinkLabel)sender).Text);
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
        }
    }
}
