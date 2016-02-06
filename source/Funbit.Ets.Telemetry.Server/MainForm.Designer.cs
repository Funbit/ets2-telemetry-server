﻿namespace Funbit.Ets.Telemetry.Server
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
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.helpLabel = new System.Windows.Forms.LinkLabel();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.interfacesDropDown = new System.Windows.Forms.ComboBox();
            this.networkInterfaceTitleLabel = new System.Windows.Forms.Label();
            this.serverIpTitleLabel = new System.Windows.Forms.Label();
            this.appUrlLabel = new System.Windows.Forms.LinkLabel();
            this.appUrlTitleLabel = new System.Windows.Forms.Label();
            this.apiUrlLabel = new System.Windows.Forms.LinkLabel();
            this.statusLabel = new System.Windows.Forms.Label();
            this.apiEndpointUrlTitleLabel = new System.Windows.Forms.Label();
            this.statusTitleLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.broadcastTimer = new System.Windows.Forms.Timer(this.components);
            this.installTelemetryPlugin = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipTitle = "ETS2 Telemetry Server is running...";
            this.trayIcon.ContextMenuStrip = this.contextMenuStrip;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "ETS2 Telemetry Server is running...";
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
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Interval = 1000;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.statusUpdateTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.helpLabel);
            this.groupBox1.Controls.Add(this.ipAddressLabel);
            this.groupBox1.Controls.Add(this.interfacesDropDown);
            this.groupBox1.Controls.Add(this.networkInterfaceTitleLabel);
            this.groupBox1.Controls.Add(this.serverIpTitleLabel);
            this.groupBox1.Controls.Add(this.appUrlLabel);
            this.groupBox1.Controls.Add(this.appUrlTitleLabel);
            this.groupBox1.Controls.Add(this.apiUrlLabel);
            this.groupBox1.Controls.Add(this.statusLabel);
            this.groupBox1.Controls.Add(this.apiEndpointUrlTitleLabel);
            this.groupBox1.Controls.Add(this.statusTitleLabel);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(527, 233);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server status";
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpLabel.Location = new System.Drawing.Point(479, 194);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(34, 17);
            this.helpLabel.TabIndex = 22;
            this.helpLabel.TabStop = true;
            this.helpLabel.Text = "Help";
            this.helpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked);
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressLabel.ForeColor = System.Drawing.Color.Purple;
            this.ipAddressLabel.Location = new System.Drawing.Point(139, 131);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(95, 17);
            this.ipAddressLabel.TabIndex = 21;
            this.ipAddressLabel.Text = "111.222.333.444";
            this.toolTip.SetToolTip(this.ipAddressLabel, "Use this IP address for mobile application (Android)");
            // 
            // interfacesDropDown
            // 
            this.interfacesDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.interfacesDropDown.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.interfacesDropDown.FormattingEnabled = true;
            this.interfacesDropDown.Location = new System.Drawing.Point(144, 91);
            this.interfacesDropDown.Name = "interfacesDropDown";
            this.interfacesDropDown.Size = new System.Drawing.Size(369, 25);
            this.interfacesDropDown.TabIndex = 20;
            this.interfacesDropDown.TabStop = false;
            this.interfacesDropDown.SelectedIndexChanged += new System.EventHandler(this.interfaceDropDown_SelectedIndexChanged);
            // 
            // networkInterfaceTitleLabel
            // 
            this.networkInterfaceTitleLabel.AutoSize = true;
            this.networkInterfaceTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.networkInterfaceTitleLabel.Location = new System.Drawing.Point(18, 94);
            this.networkInterfaceTitleLabel.Name = "networkInterfaceTitleLabel";
            this.networkInterfaceTitleLabel.Size = new System.Drawing.Size(120, 17);
            this.networkInterfaceTitleLabel.TabIndex = 19;
            this.networkInterfaceTitleLabel.Text = "Network Interfaces:";
            // 
            // serverIpTitleLabel
            // 
            this.serverIpTitleLabel.AutoSize = true;
            this.serverIpTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpTitleLabel.Location = new System.Drawing.Point(76, 131);
            this.serverIpTitleLabel.Name = "serverIpTitleLabel";
            this.serverIpTitleLabel.Size = new System.Drawing.Size(62, 17);
            this.serverIpTitleLabel.TabIndex = 17;
            this.serverIpTitleLabel.Text = "Server IP:";
            // 
            // appUrlLabel
            // 
            this.appUrlLabel.AutoSize = true;
            this.appUrlLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appUrlLabel.Location = new System.Drawing.Point(140, 163);
            this.appUrlLabel.Name = "appUrlLabel";
            this.appUrlLabel.Size = new System.Drawing.Size(72, 17);
            this.appUrlLabel.TabIndex = 16;
            this.appUrlLabel.TabStop = true;
            this.appUrlLabel.Text = "appUrlLabel";
            this.toolTip.SetToolTip(this.appUrlLabel, "Use this URL to view HTML5 mobile dashboard in desktop or mobile browsers (click " +
        "to open)");
            this.appUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.appUrlLabel_LinkClicked);
            // 
            // appUrlTitleLabel
            // 
            this.appUrlTitleLabel.AutoSize = true;
            this.appUrlTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appUrlTitleLabel.Location = new System.Drawing.Point(31, 163);
            this.appUrlTitleLabel.Name = "appUrlTitleLabel";
            this.appUrlTitleLabel.Size = new System.Drawing.Size(107, 17);
            this.appUrlTitleLabel.TabIndex = 15;
            this.appUrlTitleLabel.Text = "HTML5 App URL:";
            // 
            // apiUrlLabel
            // 
            this.apiUrlLabel.AutoSize = true;
            this.apiUrlLabel.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiUrlLabel.Location = new System.Drawing.Point(139, 194);
            this.apiUrlLabel.Name = "apiUrlLabel";
            this.apiUrlLabel.Size = new System.Drawing.Size(68, 17);
            this.apiUrlLabel.TabIndex = 14;
            this.apiUrlLabel.TabStop = true;
            this.apiUrlLabel.Text = "apiUrlLabel";
            this.toolTip.SetToolTip(this.apiUrlLabel, "Use this URL to develop your own applications based on the REST API (click to ope" +
        "n)");
            this.apiUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.apiUrlLabel_LinkClicked);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusLabel.Location = new System.Drawing.Point(141, 41);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 17);
            this.statusLabel.TabIndex = 13;
            this.statusLabel.Text = "Checking...";
            // 
            // apiEndpointUrlTitleLabel
            // 
            this.apiEndpointUrlTitleLabel.AutoSize = true;
            this.apiEndpointUrlTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiEndpointUrlTitleLabel.Location = new System.Drawing.Point(21, 194);
            this.apiEndpointUrlTitleLabel.Name = "apiEndpointUrlTitleLabel";
            this.apiEndpointUrlTitleLabel.Size = new System.Drawing.Size(116, 17);
            this.apiEndpointUrlTitleLabel.TabIndex = 12;
            this.apiEndpointUrlTitleLabel.Text = "Telemetry API URL:";
            // 
            // statusTitleLabel
            // 
            this.statusTitleLabel.AutoSize = true;
            this.statusTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusTitleLabel.Location = new System.Drawing.Point(92, 41);
            this.statusTitleLabel.Name = "statusTitleLabel";
            this.statusTitleLabel.Size = new System.Drawing.Size(46, 17);
            this.statusTitleLabel.TabIndex = 11;
            this.statusTitleLabel.Text = "Status:";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 6000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // broadcastTimer
            // 
            this.broadcastTimer.Interval = 1000;
            this.broadcastTimer.Tick += new System.EventHandler(this.broadcastTimer_Tick);
            // 
            // installTelemetryPlugin
            // 
            this.installTelemetryPlugin.Location = new System.Drawing.Point(406, 250);
            this.installTelemetryPlugin.Name = "installTelemetryPlugin";
            this.installTelemetryPlugin.Size = new System.Drawing.Size(133, 41);
            this.installTelemetryPlugin.TabIndex = 12;
            this.installTelemetryPlugin.Text = "Install Telemetry Plugin";
            this.installTelemetryPlugin.UseVisualStyleBackColor = true;
            this.installTelemetryPlugin.Click += new System.EventHandler(this.installTelemetryPlugin_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 303);
            this.Controls.Add(this.installTelemetryPlugin);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Timer statusUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label serverIpTitleLabel;
        private System.Windows.Forms.LinkLabel appUrlLabel;
        private System.Windows.Forms.Label appUrlTitleLabel;
        private System.Windows.Forms.LinkLabel apiUrlLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label apiEndpointUrlTitleLabel;
        private System.Windows.Forms.Label statusTitleLabel;
        private System.Windows.Forms.Label ipAddressLabel;
        private System.Windows.Forms.ComboBox interfacesDropDown;
        private System.Windows.Forms.Label networkInterfaceTitleLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer broadcastTimer;
        private System.Windows.Forms.LinkLabel helpLabel;
        private System.Windows.Forms.Button installTelemetryPlugin;
    }
}

