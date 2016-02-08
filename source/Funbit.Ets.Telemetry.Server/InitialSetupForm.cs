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
    public partial class InitialSetupForm : Form
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SetupHelper SetupHelper = new SetupHelper();

        public InitialSetupForm()
        {
            InitializeComponent();

            DialogResult = DialogResult.OK;
        }
        
        private void InitialSetupForm_Load(object sender, EventArgs e)
        {
            // show application version 
            Text += @" " + AssemblyHelper.Version + @" - Setup";
            
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

            // Check to see if ETS2 and/or ATS are installed on the user's machine.
            UpdateViewForEts2();
            UpdateViewForAts();
        }

        private void UpdateViewForEts2()
        {
            if (!SetupHelper.IsInstalled(SetupHelper.GetEts2InstallDirectory()))
            {
                ets2Logo.Image = Resources.ets2_logo_grayscale;
                ets2Install.Enabled = false;
                ets2SetGamePath.Enabled = true;
            }
            else
            {
                ets2Logo.Image = Resources.ets2_logo;
                ets2SetGamePath.Enabled = false;

                if (SetupHelper.IsEts2TelemetryPluginInstalled(SetupHelper.GetEts2InstallDirectory()))
                {
                    ets2Installed.Visible = true;
                    ets2Install.Enabled = false;
                }
            }

            goToMainForm.Visible = goToMainForm.Visible || ets2Installed.Visible;
        }

        private void UpdateViewForAts()
        {
            if (!SetupHelper.IsInstalled(SetupHelper.GetAtsInstallDirectory()))
            {
                atsLogo.Image = Resources.ats_logo_grayscale;
                atsInstall.Enabled = false;
                atsSetGamePath.Enabled = true;
            }
            else
            {
                atsLogo.Image = Resources.ats_logo;
                atsSetGamePath.Enabled = false;

                if (SetupHelper.IsAtsTelemetryPluginInstalled(SetupHelper.GetAtsInstallDirectory()))
                {
                    atsInstalled.Visible = true;
                    atsInstall.Enabled = false;
                }
            }

            goToMainForm.Visible = goToMainForm.Visible || atsInstalled.Visible;
        }

        private void ets2Install_Click(object sender, EventArgs e)
        {
            new SetupEts2Form().ShowDialog(this);
            UpdateViewForEts2();
        }

        private void atsInstall_Click(object sender, EventArgs e)
        {
            new SetupAtsForm().ShowDialog(this);
            UpdateViewForAts();
        }

        private void ets2SetGamePath_Click(object sender, EventArgs e)
        {
            Ets2PluginSetup.SetGamePath(this, SetupHelper.GetEts2InstallDirectory());
            UpdateViewForEts2();
        }

        private void atsSetGamePath_Click(object sender, EventArgs e)
        {
            AtsPluginSetup.SetGamePath(this, SetupHelper.GetAtsInstallDirectory());
            UpdateViewForAts();
        }

        private void goToMainForm_Click(object sender, EventArgs e)
        {
            // Clicking continue will simply close this form and allow the MainForm to continue rendering.
            Close();
        }


        private void InitialSetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void helpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrl("https://github.com/Funbit/ets2-telemetry-server");
        }
    }
}
