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
    public partial class SetupAtsForm : Form
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool _setupFinished;

        readonly Dictionary<ISetup, PictureBox> _setupStatusImages = new Dictionary<ISetup, PictureBox>();

        public SetupAtsForm()
        {
            InitializeComponent();

            DialogResult = DialogResult.OK;

            string port = ConfigurationManager.AppSettings["Port"];
            pluginStatusLabel.Text = @"Install ATS telemetry plugin DLL";
            firewallStatusLabel.Text = string.Format(@"Add firewall rule for port {0}", port);
            urlReservationStatusLabel.Text = string.Format(@"Add ACL rule for http://+:{0}/", port);
            okButton.Text = @"Install";
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
            if (_setupStatusImages.ContainsKey(step))
                _setupStatusImages[step].Image = statusImage;
        }
        
        private void SetupForm_Load(object sender, EventArgs e)
        {
            // show application version 
            Text += @" " + AssemblyHelper.Version + @" - Setup";

            // make sure that game is not running
            if (Ets2ProcessHelper.IsAtsRunning)
            {
                MessageBox.Show(this,
                    @"In order to proceed, ATS must not be running." + Environment.NewLine +
                    @"Please exit both games and try again.", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Abort;
                return;
            }

            // make sure that we have Administrator rights
            if (!Uac.IsProcessElevated())
            {
                try
                {
                    // we have to restart the setup with Administrator privileges
                    Uac.RestartElevated();
                    DialogResult = DialogResult.Abort;
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
            
            // update UI 
            foreach (var step in SetupManager.AtsSteps)
            {
                if (step is AtsPluginSetup)
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
            okButton.Enabled = false;
            _setupFinished = true;

            foreach (var step in SetupManager.AtsSteps)
            {
                try
                {
                    SetStepStatus(step, Program.UninstallMode ? step.Uninstall(this) : step.Install(this));
                }
                catch (Exception ex)
                {
                    _setupFinished = false;
                    Log.Error(ex);
                    ex.ShowAsMessageBox(this, @"Setup error");
                }
            }

            string message;
            if (_setupFinished)
            {
                message = @"Server has been successfully installed. " + Environment.NewLine +
                            "Press OK to continue.";
            }
            else
            {
                message = @"Server has been installed with errors. " + Environment.NewLine +
                            "Press OK to continue.";
            }

            if (Program.UninstallMode)
                Helpers.Settings.Clear();

            MessageBox.Show(this, message, @"Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing || Program.UninstallMode)
                DialogResult = DialogResult.Abort;
        }

        private void helpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrl("https://github.com/Funbit/ets2-telemetry-server");
        }
    }
}
