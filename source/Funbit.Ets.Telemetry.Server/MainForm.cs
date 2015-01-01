using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Helpers;
using Microsoft.Owin.Hosting;

namespace Funbit.Ets.Telemetry.Server
{
    public partial class MainForm : Form
    {
        IDisposable _server;

        public MainForm()
        {
            InitializeComponent();
        }

        static string EndpointPort
        {
            get { return ConfigurationManager.AppSettings["Port"]; }
        }

        static string HostToEndpointUrl(string host)
        {
            return string.Format("http://{0}:{1}", host, EndpointPort);
        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {
            try
            {
                var options = new StartOptions();
                options.Urls.Add(HostToEndpointUrl("localhost"));
                apiEndpointUrlLabel.Text = options.Urls.First();
                try
                {
                    if (Uac.IsProcessElevated())
                    {
                        // bind to local IPs
                        options.Urls.Add(HostToEndpointUrl("127.0.0.1"));
                        options.Urls.Add(HostToEndpointUrl(Environment.MachineName));
                        // bind to the default network IP as well
                        var defaultIp = NetworkHelper.GetDefaultIpAddress().ToString();
                        options.Urls.Add(HostToEndpointUrl(defaultIp));
                        apiEndpointUrlLabel.Text = options.Urls.Last();
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, @"Network error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                // show full URL to the telemetry data
                appUrlLabel.Text = apiEndpointUrlLabel.Text + @"/apps/ets2/index.htm";
                apiEndpointUrlLabel.Text += @"/api/ets2/telemetry";
                _server = WebApp.Start<Startup>(options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, @"Server error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
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
                bool ets2Running = Ets2ProcessHelper.IsEts2Running();
                if (ets2Running)
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
                MessageBox.Show(this, ex.Message, @"Process error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
    }
}
