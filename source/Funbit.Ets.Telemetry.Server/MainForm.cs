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

        static string Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = string.Format("{0}.{1}.{2}", 
                    versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.ProductBuildPart);
                return version;
            }
        }

        static string EndpointPort
        {
            get { return ConfigurationManager.AppSettings["Port"]; }
        }

        static string HostToEndpointUrl(string host)
        {
            return string.Format("http://{0}:{1}", host, EndpointPort);
        }

        string StartServer()
        {
            string bindUrl = string.Empty;
            try
            {
                var options = new StartOptions();
                options.Urls.Add(HostToEndpointUrl("localhost"));
                bindUrl = options.Urls.First();
                try
                {
                    if (Uac.IsProcessElevated())
                    {
                        // bind to local IPs
                        options.Urls.Add(HostToEndpointUrl("127.0.0.1"));
                        options.Urls.Add(HostToEndpointUrl(Environment.MachineName));
                        // bind to the default network IP as well
                        var defaultIp = NetworkHelper.GetDefaultIpAddress(
                            ConfigurationManager.AppSettings["NetworkInterfaceId"]).ToString();
                        options.Urls.Add(HostToEndpointUrl(defaultIp));
                        bindUrl = options.Urls.Last();
                        // raise priority to make server more responsive
                        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
                    }
                    else
                    {
                        MessageBox.Show(this, @"The process was not run under Administrator " +
                            @"so you won't be able to access your telemetry API server remotely " +
                            @"(i.e. from iPhone or other devices connected to your network). " +
                            Environment.NewLine + Environment.NewLine +
                            @"To fix this use right click context menu and select 'Run as Administrator'.", @"Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    Log.InfoFormat("Binding WebApi server to the following URLs: {0}{1}", Environment.NewLine,
                        string.Join(", " + Environment.NewLine, options.Urls.Select(u => "'" + u + "'")));
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    MessageBox.Show(this, ex.ToString(), @"Network error",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                _server = WebApp.Start<Startup>(options);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show(this, ex.ToString(), @"Server error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return bindUrl;
        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {
            // start server
            string bindUrl = StartServer();

            // show full URLs to the telemetry data
            appUrlLabel.Text = bindUrl + Ets2AppController.TelemetryAppUriPath;
            apiEndpointUrlLabel.Text = bindUrl + Ets2TelemetryController.TelemetryApiUriPath;
            
            // show application version 
            Text += @" " + Version;
        }

        private void TrayIconForm_FormClosed(object sender, FormClosedEventArgs e)
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
                MessageBox.Show(this, ex.ToString(), @"Process error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
