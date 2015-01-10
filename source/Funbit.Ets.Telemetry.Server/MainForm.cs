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

        void StartServer()
        {
            try
            {
                _server = WebApp.Start<Startup>(IpToEndpointUrl("+"));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, @"Server error");
            }
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
                if (Program.UninstallMode && SetupManager.Steps.All(s => s.Status == SetupStatus.Uninstalled))
                {
                    MessageBox.Show(this, @"Server is not installed, nothing to uninstall.",  @"Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }
                if (Program.UninstallMode || SetupManager.Steps.Any(s => s.Status != SetupStatus.Installed))
                    // we wait here until setup is complete
                    new SetupForm().ShowDialog(this);
                
                // start server
                StartServer();

                try
                {
                    // load list of available network interfaces
                    var networkInterfaces = NetworkHelper.GetAllActiveNetworkInterfaces();
                    interfacesDropDown.Items.Clear();
                    foreach (var networkInterface in networkInterfaces)
                        interfacesDropDown.Items.Add(networkInterface);
                    // select remembered interface or default
                    var rememberedInterface = networkInterfaces.FirstOrDefault(
                        i => i.Id == Settings.Instance.DefaultNetworkInterfaceId);
                    if (rememberedInterface != null)
                        interfacesDropDown.SelectedItem = rememberedInterface;
                    else
                        interfacesDropDown.SelectedIndex = 0; // select default interface
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    ex.ShowAsMessageBox(this, @"Network error", MessageBoxIcon.Exclamation);
                }

                // raise priority to make server more responsive
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

                // start ETS2 process status timer
                statusUpdateTimer.Enabled = true;

                // show tray icon
                trayIcon.Visible = true;
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
            trayIcon.Visible = false;
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

        private void apiUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteLink(sender);
        }

        private void appUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExecuteLink(sender);
        }

        static void ExecuteLink(object sender)
        {
            ProcessHelper.OpenUrl(((LinkLabel) sender).Text);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
        }

        private void interfaceDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedInterface = (NetworkInterfaceInfo) interfacesDropDown.SelectedItem;
            appUrlLabel.Text = IpToEndpointUrl(selectedInterface.Ip) + Ets2AppController.TelemetryAppUriPath;
            apiUrlLabel.Text = IpToEndpointUrl(selectedInterface.Ip) + Ets2TelemetryController.TelemetryApiUriPath;
            ipAddressLabel.Text = selectedInterface.Ip;
            Settings.Instance.DefaultNetworkInterfaceId = selectedInterface.Id;
            Settings.Instance.Save();
        }
    }
}
