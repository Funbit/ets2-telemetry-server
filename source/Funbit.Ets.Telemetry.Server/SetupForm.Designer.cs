namespace Funbit.Ets.Telemetry.Server
{
    partial class SetupForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.urlReservationStatusImage = new System.Windows.Forms.PictureBox();
            this.urlReservationStatusLabel = new System.Windows.Forms.Label();
            this.firewallStatusImage = new System.Windows.Forms.PictureBox();
            this.firewallStatusLabel = new System.Windows.Forms.Label();
            this.pluginStatusImage = new System.Windows.Forms.PictureBox();
            this.pluginStatusLabel = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.urlReservationStatusImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firewallStatusImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pluginStatusImage)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.okButton.Location = new System.Drawing.Point(390, 225);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(127, 42);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Install";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.urlReservationStatusImage);
            this.groupBox.Controls.Add(this.urlReservationStatusLabel);
            this.groupBox.Controls.Add(this.firewallStatusImage);
            this.groupBox.Controls.Add(this.firewallStatusLabel);
            this.groupBox.Controls.Add(this.pluginStatusImage);
            this.groupBox.Controls.Add(this.pluginStatusLabel);
            this.groupBox.Location = new System.Drawing.Point(12, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(505, 201);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Setup steps";
            // 
            // urlReservationStatusImage
            // 
            this.urlReservationStatusImage.Image = global::Funbit.Ets.Telemetry.Server.Properties.Resources.StatusIcon;
            this.urlReservationStatusImage.InitialImage = null;
            this.urlReservationStatusImage.Location = new System.Drawing.Point(41, 133);
            this.urlReservationStatusImage.Name = "urlReservationStatusImage";
            this.urlReservationStatusImage.Size = new System.Drawing.Size(40, 40);
            this.urlReservationStatusImage.TabIndex = 7;
            this.urlReservationStatusImage.TabStop = false;
            // 
            // urlReservationStatusLabel
            // 
            this.urlReservationStatusLabel.AutoSize = true;
            this.urlReservationStatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlReservationStatusLabel.Location = new System.Drawing.Point(96, 143);
            this.urlReservationStatusLabel.Name = "urlReservationStatusLabel";
            this.urlReservationStatusLabel.Size = new System.Drawing.Size(170, 21);
            this.urlReservationStatusLabel.TabIndex = 6;
            this.urlReservationStatusLabel.Text = "HTTP URL reservation...";
            // 
            // firewallStatusImage
            // 
            this.firewallStatusImage.Image = global::Funbit.Ets.Telemetry.Server.Properties.Resources.StatusIcon;
            this.firewallStatusImage.InitialImage = null;
            this.firewallStatusImage.Location = new System.Drawing.Point(41, 81);
            this.firewallStatusImage.Name = "firewallStatusImage";
            this.firewallStatusImage.Size = new System.Drawing.Size(40, 40);
            this.firewallStatusImage.TabIndex = 5;
            this.firewallStatusImage.TabStop = false;
            // 
            // firewallStatusLabel
            // 
            this.firewallStatusLabel.AutoSize = true;
            this.firewallStatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firewallStatusLabel.Location = new System.Drawing.Point(96, 91);
            this.firewallStatusLabel.Name = "firewallStatusLabel";
            this.firewallStatusLabel.Size = new System.Drawing.Size(189, 21);
            this.firewallStatusLabel.TabIndex = 4;
            this.firewallStatusLabel.Text = "Port in the Firewall";
            // 
            // pluginStatusImage
            // 
            this.pluginStatusImage.Image = global::Funbit.Ets.Telemetry.Server.Properties.Resources.StatusIcon;
            this.pluginStatusImage.InitialImage = null;
            this.pluginStatusImage.Location = new System.Drawing.Point(41, 30);
            this.pluginStatusImage.Name = "pluginStatusImage";
            this.pluginStatusImage.Size = new System.Drawing.Size(40, 40);
            this.pluginStatusImage.TabIndex = 3;
            this.pluginStatusImage.TabStop = false;
            // 
            // pluginStatusLabel
            // 
            this.pluginStatusLabel.AutoSize = true;
            this.pluginStatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pluginStatusLabel.Location = new System.Drawing.Point(96, 40);
            this.pluginStatusLabel.Name = "pluginStatusLabel";
            this.pluginStatusLabel.Size = new System.Drawing.Size(235, 21);
            this.pluginStatusLabel.TabIndex = 2;
            this.pluginStatusLabel.Text = "ETS2 telemetry plugin DLL setup";
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 280);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETS2 Telemetry Server Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.urlReservationStatusImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firewallStatusImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pluginStatusImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.PictureBox pluginStatusImage;
        private System.Windows.Forms.Label pluginStatusLabel;
        private System.Windows.Forms.PictureBox firewallStatusImage;
        private System.Windows.Forms.Label firewallStatusLabel;
        private System.Windows.Forms.PictureBox urlReservationStatusImage;
        private System.Windows.Forms.Label urlReservationStatusLabel;
    }
}