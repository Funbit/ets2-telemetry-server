namespace Funbit.Ets.Telemetry.Server
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusTitleLabel = new System.Windows.Forms.Label();
            this.apiEndpointUrlTitleLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.apiEndpointUrlLabel = new System.Windows.Forms.LinkLabel();
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.appUrlLabel = new System.Windows.Forms.LinkLabel();
            this.appUrlTitleLabel = new System.Windows.Forms.Label();
            this.serverIpTitleLabel = new System.Windows.Forms.Label();
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipTitle = "ETS2 Telemetry Server is running...";
            this.trayIcon.ContextMenuStrip = this.contextMenuStrip;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "ETS2 Telemetry Server is running...";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(104, 26);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::Funbit.Ets.Telemetry.Server.Properties.Resources.CloseIcon;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // statusTitleLabel
            // 
            this.statusTitleLabel.AutoSize = true;
            this.statusTitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusTitleLabel.Location = new System.Drawing.Point(88, 27);
            this.statusTitleLabel.Name = "statusTitleLabel";
            this.statusTitleLabel.Size = new System.Drawing.Size(43, 17);
            this.statusTitleLabel.TabIndex = 1;
            this.statusTitleLabel.Text = "Status:";
            // 
            // apiEndpointUrlTitleLabel
            // 
            this.apiEndpointUrlTitleLabel.AutoSize = true;
            this.apiEndpointUrlTitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiEndpointUrlTitleLabel.Location = new System.Drawing.Point(17, 137);
            this.apiEndpointUrlTitleLabel.Name = "apiEndpointUrlTitleLabel";
            this.apiEndpointUrlTitleLabel.Size = new System.Drawing.Size(113, 17);
            this.apiEndpointUrlTitleLabel.TabIndex = 2;
            this.apiEndpointUrlTitleLabel.Text = "Telemetry Api URL:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(134, 27);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(68, 17);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Checking...";
            // 
            // apiEndpointUrlLabel
            // 
            this.apiEndpointUrlLabel.AutoSize = true;
            this.apiEndpointUrlLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiEndpointUrlLabel.Location = new System.Drawing.Point(133, 137);
            this.apiEndpointUrlLabel.Name = "apiEndpointUrlLabel";
            this.apiEndpointUrlLabel.Size = new System.Drawing.Size(101, 17);
            this.apiEndpointUrlLabel.TabIndex = 4;
            this.apiEndpointUrlLabel.TabStop = true;
            this.apiEndpointUrlLabel.Text = "endpointUrlLabel";
            this.apiEndpointUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.endpointUrlLabel_LinkClicked);
            // 
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Interval = 1000;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.statusUpdateTimer_Tick);
            // 
            // appUrlLabel
            // 
            this.appUrlLabel.AutoSize = true;
            this.appUrlLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appUrlLabel.Location = new System.Drawing.Point(134, 105);
            this.appUrlLabel.Name = "appUrlLabel";
            this.appUrlLabel.Size = new System.Drawing.Size(72, 17);
            this.appUrlLabel.TabIndex = 6;
            this.appUrlLabel.TabStop = true;
            this.appUrlLabel.Text = "appUrlLabel";
            this.appUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.appUrlLabel_LinkClicked);
            // 
            // appUrlTitleLabel
            // 
            this.appUrlTitleLabel.AutoSize = true;
            this.appUrlTitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appUrlTitleLabel.Location = new System.Drawing.Point(28, 105);
            this.appUrlTitleLabel.Name = "appUrlTitleLabel";
            this.appUrlTitleLabel.Size = new System.Drawing.Size(102, 17);
            this.appUrlTitleLabel.TabIndex = 5;
            this.appUrlTitleLabel.Text = "HTML5 App URL:";
            // 
            // serverIpTitleLabel
            // 
            this.serverIpTitleLabel.AutoSize = true;
            this.serverIpTitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpTitleLabel.Location = new System.Drawing.Point(70, 72);
            this.serverIpTitleLabel.Name = "serverIpTitleLabel";
            this.serverIpTitleLabel.Size = new System.Drawing.Size(60, 17);
            this.serverIpTitleLabel.TabIndex = 7;
            this.serverIpTitleLabel.Text = "Server IP:";
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoSize = true;
            this.serverIpLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F);
            this.serverIpLabel.ForeColor = System.Drawing.Color.Purple;
            this.serverIpLabel.Location = new System.Drawing.Point(134, 72);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(81, 17);
            this.serverIpLabel.TabIndex = 9;
            this.serverIpLabel.Text = "serverIpLabel";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 182);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.serverIpTitleLabel);
            this.Controls.Add(this.appUrlLabel);
            this.Controls.Add(this.appUrlTitleLabel);
            this.Controls.Add(this.apiEndpointUrlLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.apiEndpointUrlTitleLabel);
            this.Controls.Add(this.statusTitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETS2 Telemetry Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label statusTitleLabel;
        private System.Windows.Forms.Label apiEndpointUrlTitleLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.LinkLabel apiEndpointUrlLabel;
        private System.Windows.Forms.Timer statusUpdateTimer;
        private System.Windows.Forms.LinkLabel appUrlLabel;
        private System.Windows.Forms.Label appUrlTitleLabel;
        private System.Windows.Forms.Label serverIpTitleLabel;
        private System.Windows.Forms.Label serverIpLabel;
    }
}

