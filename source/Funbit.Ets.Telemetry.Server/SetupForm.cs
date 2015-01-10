using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Funbit.Ets.Telemetry.Server.Helpers;
using Funbit.Ets.Telemetry.Server.Properties;
using Funbit.Ets.Telemetry.Server.Setup;

namespace Funbit.Ets.Telemetry.Server
{
    public partial class SetupForm : Form
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool _setupFinished;

        readonly Dictionary<ISetup, PictureBox> _setupStatusImages = new Dictionary<ISetup, PictureBox>();

        public SetupForm()
        {
            InitializeComponent();

            string port = ConfigurationManager.AppSettings["Port"];
            if (Program.UninstallMode)
            {
                pluginStatusLabel.Text = @"Uninstall ETS2 telemetry plugin DLL";
                firewallStatusLabel.Text = string.Format(@"Delete firewall rule for {0} port", port);
                urlReservationStatusLabel.Text = string.Format(@"Delete ACL rule for http://+:{0}/", port);
                okButton.Text = @"Uninstall";
            }
            else
            {
                pluginStatusLabel.Text = @"Install ETS2 telemetry plugin DLL";
                firewallStatusLabel.Text = string.Format(@"Add firewall rule for {0} port", port);
                urlReservationStatusLabel.Text = string.Format(@"Add ACL rule for http://+:{0}/", port);
                okButton.Text = @"Install";
            }
        }

        void SetStepStatus(ISetup step, SetupStatus status)
        {
            SetupStatus inverseStatus = status;
            if (Program.UninstallMode)
            {
                switch (status)
                {
                    case SetupStatus.Installed:
                        inverseStatus = SetupStatus.Uninstalled;
                        break;
                    case SetupStatus.Uninstalled:
                        inverseStatus = SetupStatus.Installed;
                        break;
                }
            }
            // convert status enumeration to images
            Bitmap statusImage = inverseStatus == SetupStatus.Uninstalled
                                     ? Resources.StatusIcon
                                     : (inverseStatus == SetupStatus.Installed
                                            ? Resources.SuccessStatusIcon
                                            : Resources.FailureStatusIcon);
            _setupStatusImages[step].Image = statusImage;
        }
        
        private void SetupForm_Load(object sender, EventArgs e)
        {
            // show application version 
            Text += @" " + AssemblyHelper.Version;

            // make sure that we have Administrator rights
            if (!Uac.IsProcessElevated())
            {
                try
                {
                    // we have to restart the setup with Administrator privileges
                    Uac.RestartElevated();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    // if succeeded or user declined elevation 
                    // then we just exit from the current process
                    Environment.Exit(0);
                }
            }
            
            // initialize status mapping
            foreach (var step in SetupManager.Steps)
            {
                if (step is PluginSetup)
                    _setupStatusImages.Add(step, pluginStatusImage);
                else if (step is FirewallSetup)
                    _setupStatusImages.Add(step, firewallStatusImage);
                else if (step is UrlReservationSetup)
                    _setupStatusImages.Add(step, urlReservationStatusImage);
                SetStepStatus(step, step.Status);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _setupFinished = true;

            foreach (var step in SetupManager.Steps)
            {
                try
                {
                    SetStepStatus(step, Program.UninstallMode ? step.Uninstall() : step.Install());
                }
                catch (Exception ex)
                {
                    _setupFinished = false;
                    ex.ShowAsMessageBox(this, @"Setup error");
                }
            }

            if (_setupFinished)
            {
                string message = Program.UninstallMode
                                     ? @"Uninstallation has been successfully completed. Press OK to exit."
                                     : @"Installation has been successfully completed. Press OK to start the server.";
                MessageBox.Show(this, message, @"Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (Program.UninstallMode)
                    Environment.Exit(0);
                Close();
            }
        }

        private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if form is closed by OS we just exit from the application
            if (e.CloseReason != CloseReason.UserClosing || !_setupFinished || Program.UninstallMode)
                Environment.Exit(0);
        }
    }
}
