namespace Funbit.Ets.Telemetry.Server
{
    partial class InitialSetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialSetupForm));
            this.helpLabel = new System.Windows.Forms.LinkLabel();
            this.atsLogo = new System.Windows.Forms.PictureBox();
            this.ets2Logo = new System.Windows.Forms.PictureBox();
            this.ets2Install = new System.Windows.Forms.Button();
            this.atsSetGamePath = new System.Windows.Forms.Button();
            this.ets2SetGamePath = new System.Windows.Forms.Button();
            this.atsInstall = new System.Windows.Forms.Button();
            this.ets2Installed = new System.Windows.Forms.Label();
            this.atsInstalled = new System.Windows.Forms.Label();
            this.goToMainForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.atsLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ets2Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpLabel.Location = new System.Drawing.Point(21, 284);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(34, 17);
            this.helpLabel.TabIndex = 17;
            this.helpLabel.TabStop = true;
            this.helpLabel.Text = "Help";
            this.helpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked);
            // 
            // atsLogo
            // 
            this.atsLogo.Image = ((System.Drawing.Image)(resources.GetObject("atsLogo.Image")));
            this.atsLogo.Location = new System.Drawing.Point(395, 12);
            this.atsLogo.Name = "atsLogo";
            this.atsLogo.Size = new System.Drawing.Size(267, 146);
            this.atsLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.atsLogo.TabIndex = 19;
            this.atsLogo.TabStop = false;
            // 
            // ets2Logo
            // 
            this.ets2Logo.Image = global::Funbit.Ets.Telemetry.Server.Properties.Resources.ets2_logo;
            this.ets2Logo.Location = new System.Drawing.Point(56, 12);
            this.ets2Logo.Name = "ets2Logo";
            this.ets2Logo.Size = new System.Drawing.Size(267, 146);
            this.ets2Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ets2Logo.TabIndex = 18;
            this.ets2Logo.TabStop = false;
            // 
            // ets2Install
            // 
            this.ets2Install.Location = new System.Drawing.Point(56, 164);
            this.ets2Install.Name = "ets2Install";
            this.ets2Install.Size = new System.Drawing.Size(109, 45);
            this.ets2Install.TabIndex = 20;
            this.ets2Install.Text = "Install";
            this.ets2Install.UseVisualStyleBackColor = true;
            this.ets2Install.Click += new System.EventHandler(this.ets2Install_Click);
            // 
            // atsSetGamePath
            // 
            this.atsSetGamePath.Enabled = false;
            this.atsSetGamePath.Location = new System.Drawing.Point(553, 164);
            this.atsSetGamePath.Name = "atsSetGamePath";
            this.atsSetGamePath.Size = new System.Drawing.Size(109, 45);
            this.atsSetGamePath.TabIndex = 26;
            this.atsSetGamePath.Text = "Set Game Path";
            this.atsSetGamePath.UseVisualStyleBackColor = true;
            this.atsSetGamePath.Click += new System.EventHandler(this.atsSetGamePath_Click);
            // 
            // ets2SetGamePath
            // 
            this.ets2SetGamePath.Enabled = false;
            this.ets2SetGamePath.Location = new System.Drawing.Point(214, 164);
            this.ets2SetGamePath.Name = "ets2SetGamePath";
            this.ets2SetGamePath.Size = new System.Drawing.Size(109, 45);
            this.ets2SetGamePath.TabIndex = 27;
            this.ets2SetGamePath.Text = "Set Game Path";
            this.ets2SetGamePath.UseVisualStyleBackColor = true;
            this.ets2SetGamePath.Click += new System.EventHandler(this.ets2SetGamePath_Click);
            // 
            // atsInstall
            // 
            this.atsInstall.Location = new System.Drawing.Point(395, 164);
            this.atsInstall.Name = "atsInstall";
            this.atsInstall.Size = new System.Drawing.Size(109, 45);
            this.atsInstall.TabIndex = 22;
            this.atsInstall.Text = "Install";
            this.atsInstall.UseVisualStyleBackColor = true;
            this.atsInstall.Click += new System.EventHandler(this.atsInstall_Click);
            // 
            // ets2Installed
            // 
            this.ets2Installed.BackColor = System.Drawing.Color.Transparent;
            this.ets2Installed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ets2Installed.ForeColor = System.Drawing.Color.Black;
            this.ets2Installed.Location = new System.Drawing.Point(56, 212);
            this.ets2Installed.Name = "ets2Installed";
            this.ets2Installed.Size = new System.Drawing.Size(267, 28);
            this.ets2Installed.TabIndex = 28;
            this.ets2Installed.Text = "Installed";
            this.ets2Installed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ets2Installed.Visible = false;
            // 
            // atsInstalled
            // 
            this.atsInstalled.BackColor = System.Drawing.Color.Transparent;
            this.atsInstalled.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.atsInstalled.ForeColor = System.Drawing.Color.Black;
            this.atsInstalled.Location = new System.Drawing.Point(390, 212);
            this.atsInstalled.Name = "atsInstalled";
            this.atsInstalled.Size = new System.Drawing.Size(272, 28);
            this.atsInstalled.TabIndex = 29;
            this.atsInstalled.Text = "Installed";
            this.atsInstalled.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.atsInstalled.Visible = false;
            // 
            // goToMainForm
            // 
            this.goToMainForm.Location = new System.Drawing.Point(553, 256);
            this.goToMainForm.Name = "goToMainForm";
            this.goToMainForm.Size = new System.Drawing.Size(109, 45);
            this.goToMainForm.TabIndex = 30;
            this.goToMainForm.Text = "Continue >>";
            this.goToMainForm.UseVisualStyleBackColor = true;
            this.goToMainForm.Visible = false;
            this.goToMainForm.Click += new System.EventHandler(this.goToMainForm_Click);
            // 
            // InitialSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 319);
            this.Controls.Add(this.goToMainForm);
            this.Controls.Add(this.atsInstalled);
            this.Controls.Add(this.ets2Installed);
            this.Controls.Add(this.atsInstall);
            this.Controls.Add(this.ets2SetGamePath);
            this.Controls.Add(this.atsSetGamePath);
            this.Controls.Add(this.ets2Install);
            this.Controls.Add(this.atsLogo);
            this.Controls.Add(this.ets2Logo);
            this.Controls.Add(this.helpLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitialSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETS2/ATS Telemetry Server";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InitialSetupForm_FormClosing);
            this.Load += new System.EventHandler(this.InitialSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.atsLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ets2Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel helpLabel;
        private System.Windows.Forms.PictureBox ets2Logo;
        private System.Windows.Forms.PictureBox atsLogo;
        private System.Windows.Forms.Button ets2Install;
        private System.Windows.Forms.Button atsSetGamePath;
        private System.Windows.Forms.Button ets2SetGamePath;
        private System.Windows.Forms.Button atsInstall;
        private System.Windows.Forms.Label ets2Installed;
        private System.Windows.Forms.Label atsInstalled;
        private System.Windows.Forms.Button goToMainForm;
    }
}