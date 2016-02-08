using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Controllers;
using Funbit.Ets.Telemetry.Server.Data;
using Funbit.Ets.Telemetry.Server.Helpers;
using Funbit.Ets.Telemetry.Server.Properties;
using Funbit.Ets.Telemetry.Server.Setup;
using Microsoft.Owin.Hosting;
using Settings = Funbit.Ets.Telemetry.Server.Helpers.Settings;

namespace Funbit.Ets.Telemetry.Server
{
    public partial class MainForm : Form
    {
        IDisposable _server;
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string BroadcastUrl = ConfigurationManager.AppSettings["BroadcastUrl"];
        static readonly int BroadcastRateInSeconds = Math.Min(Math.Max(1, 
            Convert.ToInt32(ConfigurationManager.AppSettings["BroadcastRate"])), 86400);
        static readonly bool UseTestTelemetryData = Convert.ToBoolean(
            ConfigurationManager.AppSettings["UseEts2TestTelemetryData"]);

        private const int WindowHeightNoInstallButton = 291;
        private const int WindowHeightWithInstallButton = 342;

        public MainForm()
        {
            InitializeComponent();
        }

        static string IpToEndpointUrl(string host)
        {
            return string.Format("http://{0}:{1}", host, ConfigurationManager.AppSettings["Port"]);
        }

        void Setup()
        {
            try
            {
                if (Program.UninstallMode)
                {
                    if (SetupManager.Ets2Steps.All(s => s.Status == SetupStatus.Uninstalled))
                    {
                        MessageBox.Show(this, @"Server is not installed, nothing to uninstall.", @"Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Environment.Exit(0);
                    }
                    else
                    {
                        // we wait here until uninstall is complete
                        var result = new UninstallForm().ShowDialog(this);
                        if (result == DialogResult.Abort)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    // none of the steps have been installed, so display the setup screen
                    if (SetupManager.Ets2Steps.All(s => s.Status != SetupStatus.Installed))
                    {
                        // display setup screen
                        var form = new InitialSetupForm().ShowDialog(this);
                        if (form == DialogResult.Abort)
                        {
                            Environment.Exit(1);
                        }
                    }
                    
                    // otherwise we'll continue onto the main application. If the user wants to install an additional
                    // plugin, they can do so at the main view.
                }

                // raise priority to make server more responsive (it does not eat CPU though!)
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, "Setup error");
            }
        }

        void Start()
        {
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

                // bind to all available interfaces
                _server = WebApp.Start<Startup>(IpToEndpointUrl("+"));

                // start ETS2 process watchdog timer
                statusUpdateTimer.Enabled = true;

                // turn on broadcasting if set
                if (!string.IsNullOrEmpty(BroadcastUrl))
                {
                    broadcastTimer.Interval = BroadcastRateInSeconds * 1000;
                    broadcastTimer.Enabled = true;
                }

                // show tray icon
                trayIcon.Visible = true;

                // determine if we should show or hide the "Install Telemetry Plugin" button
                UpdateFormSize();

                // make sure that form is visible
                Activate();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, "Network error", MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateFormSize()
        {
            if (SetupHelper.IsEts2TelemetryPluginInstalled(SetupHelper.GetEts2InstallDirectory())
                && SetupHelper.IsAtsTelemetryPluginInstalled(SetupHelper.GetAtsInstallDirectory()))
            {
                Size = new Size(Size.Width, WindowHeightNoInstallButton);
                installTelemetryPlugin.Visible = false;
            }
            else
            {
                Size = new Size(Size.Width, WindowHeightWithInstallButton);
                installTelemetryPlugin.Visible = true;
            }
        }

        private void installTelemetryPlugin_Click(object sender, EventArgs e)
        {
            new InitialSetupForm().ShowDialog();
            UpdateFormSize();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // log current version for debugging
            Log.InfoFormat("Running application on {0} ({1}) {2}", Environment.OSVersion, 
                Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit",
                Program.UninstallMode ? "[UNINSTALL MODE]" : "");
            Text += " " + AssemblyHelper.Version;

            // install or uninstall server if needed
            Setup();

            // start WebApi server
            Start();
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
                if (UseTestTelemetryData)
                {
                    statusLabel.Text = "Connected to Ets2TestTelemetry.json";
                    statusLabel.ForeColor = Color.DarkGreen;

                    runningGamePicture.Image = null;
                } 
                else if ((Ets2ProcessHelper.IsEts2Running || Ets2ProcessHelper.IsAtsRunning) && Ets2TelemetryDataReader.Instance.IsConnected)
                {
                    statusLabel.Text = "Connected to the simulator";
                    statusLabel.ForeColor = Color.DarkGreen;

                    runningGamePicture.Image = Ets2ProcessHelper.IsEts2Running
                        ? Resources.ets2_logo
                        : Resources.ats_logo;
                }
                else if (Ets2ProcessHelper.IsEts2Running || Ets2ProcessHelper.IsAtsRunning)
                {
                    statusLabel.Text = "Simulator is running";
                    statusLabel.ForeColor = Color.Teal;

                    runningGamePicture.Image = Ets2ProcessHelper.IsEts2Running
                        ? Resources.ets2_logo
                        : Resources.ats_logo;
                }
                else
                {
                    statusLabel.Text = "Simulator is not running";
                    statusLabel.ForeColor = Color.FromArgb(240, 55, 30);

                    runningGamePicture.Image = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ex.ShowAsMessageBox(this, "Process error");
                statusUpdateTimer.Enabled = false;
            }
        }

        private void apiUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrl(((LinkLabel)sender).Text);
        }

        private void appUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrl(((LinkLabel)sender).Text);
        }
        
        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
            if (!ShowInTaskbar && trayIcon.Tag == null)
            {
                trayIcon.ShowBalloonTip(1000, "ETS2/ATS Telemetry Server", "Double-click to restore.", ToolTipIcon.Info);
                trayIcon.Tag = "Already shown";
            }
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

        private async void broadcastTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                broadcastTimer.Enabled = false;
                using (var client = new HttpClient())
                    await client.PostAsJsonAsync(BroadcastUrl, Ets2TelemetryDataReader.Instance.Read());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            broadcastTimer.Enabled = true;
        }

        private void helpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrl("https://github.com/Funbit/ets2-telemetry-server");
        }
    }
}
