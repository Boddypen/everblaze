namespace EverblazeServer
{
	partial class ServerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
			this.ServerOutputBox = new System.Windows.Forms.RichTextBox();
			this.ServerTimer = new System.Windows.Forms.Timer(this.components);
			this.PlayersGroup = new System.Windows.Forms.GroupBox();
			this.PlayersView = new System.Windows.Forms.ListView();
			this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.IPColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.ServerMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.StartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.QuitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.managementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.kickSelectedPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.banSelectedPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unbanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.broadcastMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.clearServerLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StatusStrip = new System.Windows.Forms.StatusStrip();
			this.TitleLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.PlayersGroup.SuspendLayout();
			this.MenuStrip.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// ServerOutputBox
			// 
			this.ServerOutputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ServerOutputBox.BackColor = System.Drawing.Color.White;
			this.ServerOutputBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.ServerOutputBox.DetectUrls = false;
			this.ServerOutputBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ServerOutputBox.ForeColor = System.Drawing.Color.Black;
			this.ServerOutputBox.Location = new System.Drawing.Point(12, 29);
			this.ServerOutputBox.Name = "ServerOutputBox";
			this.ServerOutputBox.ReadOnly = true;
			this.ServerOutputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.ServerOutputBox.Size = new System.Drawing.Size(501, 560);
			this.ServerOutputBox.TabIndex = 0;
			this.ServerOutputBox.Text = "";
			// 
			// ServerTimer
			// 
			this.ServerTimer.Enabled = true;
			this.ServerTimer.Tick += new System.EventHandler(this.ServerTimer_Tick);
			// 
			// PlayersGroup
			// 
			this.PlayersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PlayersGroup.Controls.Add(this.PlayersView);
			this.PlayersGroup.Location = new System.Drawing.Point(519, 29);
			this.PlayersGroup.Name = "PlayersGroup";
			this.PlayersGroup.Size = new System.Drawing.Size(238, 560);
			this.PlayersGroup.TabIndex = 4;
			this.PlayersGroup.TabStop = false;
			this.PlayersGroup.Text = "Connected Players";
			// 
			// PlayersView
			// 
			this.PlayersView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PlayersView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PlayersView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.IPColumn});
			this.PlayersView.GridLines = true;
			this.PlayersView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.PlayersView.Location = new System.Drawing.Point(6, 22);
			this.PlayersView.MultiSelect = false;
			this.PlayersView.Name = "PlayersView";
			this.PlayersView.Size = new System.Drawing.Size(226, 532);
			this.PlayersView.TabIndex = 0;
			this.PlayersView.UseCompatibleStateImageBehavior = false;
			this.PlayersView.View = System.Windows.Forms.View.Details;
			// 
			// NameColumn
			// 
			this.NameColumn.Text = "Name";
			this.NameColumn.Width = 100;
			// 
			// IPColumn
			// 
			this.IPColumn.Text = "IP";
			this.IPColumn.Width = 100;
			// 
			// MenuStrip
			// 
			this.MenuStrip.BackColor = System.Drawing.SystemColors.Info;
			this.MenuStrip.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServerMenu,
            this.managementToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.MenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.MenuStrip.Size = new System.Drawing.Size(769, 26);
			this.MenuStrip.TabIndex = 5;
			this.MenuStrip.Text = "menuStrip1";
			// 
			// ServerMenu
			// 
			this.ServerMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartMenuItem,
            this.StopMenuItem,
            this.toolStripSeparator1,
            this.QuitMenuItem});
			this.ServerMenu.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ServerMenu.ForeColor = System.Drawing.Color.Black;
			this.ServerMenu.Name = "ServerMenu";
			this.ServerMenu.Size = new System.Drawing.Size(60, 22);
			this.ServerMenu.Text = "Server";
			// 
			// StartMenuItem
			// 
			this.StartMenuItem.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartMenuItem.Image = global::EverblazeServer.Properties.Resources.play;
			this.StartMenuItem.Name = "StartMenuItem";
			this.StartMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.StartMenuItem.Size = new System.Drawing.Size(147, 22);
			this.StartMenuItem.Text = "&Start";
			this.StartMenuItem.Click += new System.EventHandler(this.StartMenuItem_Click);
			// 
			// StopMenuItem
			// 
			this.StopMenuItem.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StopMenuItem.Image = global::EverblazeServer.Properties.Resources.stop;
			this.StopMenuItem.Name = "StopMenuItem";
			this.StopMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F5)));
			this.StopMenuItem.Size = new System.Drawing.Size(147, 22);
			this.StopMenuItem.Text = "Sto&p";
			this.StopMenuItem.Click += new System.EventHandler(this.StopMenuItem_Click_1);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
			// 
			// QuitMenuItem
			// 
			this.QuitMenuItem.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.QuitMenuItem.Image = global::EverblazeServer.Properties.Resources.quit;
			this.QuitMenuItem.Name = "QuitMenuItem";
			this.QuitMenuItem.ShortcutKeyDisplayString = "";
			this.QuitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.QuitMenuItem.Size = new System.Drawing.Size(147, 22);
			this.QuitMenuItem.Text = "&Quit";
			this.QuitMenuItem.Click += new System.EventHandler(this.QuitMenuItem_Click);
			// 
			// managementToolStripMenuItem
			// 
			this.managementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informationToolStripMenuItem,
            this.toolStripSeparator2,
            this.kickSelectedPlayerToolStripMenuItem,
            this.banSelectedPlayerToolStripMenuItem,
            this.unbanToolStripMenuItem,
            this.toolStripSeparator3,
            this.broadcastMessageToolStripMenuItem,
            this.toolStripSeparator4,
            this.clearServerLogToolStripMenuItem});
			this.managementToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
			this.managementToolStripMenuItem.Name = "managementToolStripMenuItem";
			this.managementToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
			this.managementToolStripMenuItem.Text = "Management";
			// 
			// informationToolStripMenuItem
			// 
			this.informationToolStripMenuItem.Image = global::EverblazeServer.Properties.Resources.info;
			this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
			this.informationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.informationToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.informationToolStripMenuItem.Text = "&Information...";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(267, 6);
			// 
			// kickSelectedPlayerToolStripMenuItem
			// 
			this.kickSelectedPlayerToolStripMenuItem.Image = global::EverblazeServer.Properties.Resources.caution;
			this.kickSelectedPlayerToolStripMenuItem.Name = "kickSelectedPlayerToolStripMenuItem";
			this.kickSelectedPlayerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.K)));
			this.kickSelectedPlayerToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.kickSelectedPlayerToolStripMenuItem.Text = "&Kick Selected Player";
			// 
			// banSelectedPlayerToolStripMenuItem
			// 
			this.banSelectedPlayerToolStripMenuItem.Image = global::EverblazeServer.Properties.Resources.quit;
			this.banSelectedPlayerToolStripMenuItem.Name = "banSelectedPlayerToolStripMenuItem";
			this.banSelectedPlayerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
			this.banSelectedPlayerToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.banSelectedPlayerToolStripMenuItem.Text = "&Ban Selected Player";
			// 
			// unbanToolStripMenuItem
			// 
			this.unbanToolStripMenuItem.Name = "unbanToolStripMenuItem";
			this.unbanToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
			this.unbanToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.unbanToolStripMenuItem.Text = "&Unban...";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(267, 6);
			// 
			// broadcastMessageToolStripMenuItem
			// 
			this.broadcastMessageToolStripMenuItem.Image = global::EverblazeServer.Properties.Resources.message;
			this.broadcastMessageToolStripMenuItem.Name = "broadcastMessageToolStripMenuItem";
			this.broadcastMessageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.broadcastMessageToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.broadcastMessageToolStripMenuItem.Text = "Broadcast &Message...";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(267, 6);
			// 
			// clearServerLogToolStripMenuItem
			// 
			this.clearServerLogToolStripMenuItem.Name = "clearServerLogToolStripMenuItem";
			this.clearServerLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
			this.clearServerLogToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
			this.clearServerLogToolStripMenuItem.Text = "&Clear Server Log";
			this.clearServerLogToolStripMenuItem.Click += new System.EventHandler(this.clearServerLogToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
			this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(46, 22);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Image = global::EverblazeServer.Properties.Resources.info;
			this.AboutMenuItem.Name = "AboutMenuItem";
			this.AboutMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.AboutMenuItem.Size = new System.Drawing.Size(132, 22);
			this.AboutMenuItem.Text = "&About";
			// 
			// StatusStrip
			// 
			this.StatusStrip.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TitleLabel});
			this.StatusStrip.Location = new System.Drawing.Point(0, 592);
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.StatusStrip.Size = new System.Drawing.Size(769, 23);
			this.StatusStrip.TabIndex = 6;
			this.StatusStrip.Text = "statusStrip1";
			// 
			// TitleLabel
			// 
			this.TitleLabel.Name = "TitleLabel";
			this.TitleLabel.Size = new System.Drawing.Size(98, 18);
			this.TitleLabel.Text = "Everblaze Server";
			// 
			// ServerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(769, 615);
			this.Controls.Add(this.StatusStrip);
			this.Controls.Add(this.ServerOutputBox);
			this.Controls.Add(this.PlayersGroup);
			this.Controls.Add(this.MenuStrip);
			this.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.MenuStrip;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "ServerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Everblaze Server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
			this.PlayersGroup.ResumeLayout(false);
			this.MenuStrip.ResumeLayout(false);
			this.MenuStrip.PerformLayout();
			this.StatusStrip.ResumeLayout(false);
			this.StatusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void StopMenuItem_Click1(System.Object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		#endregion
		private System.Windows.Forms.RichTextBox ServerOutputBox;
		private System.Windows.Forms.Timer ServerTimer;
		private System.Windows.Forms.GroupBox PlayersGroup;
		private System.Windows.Forms.ListView PlayersView;
		private System.Windows.Forms.ColumnHeader NameColumn;
		private System.Windows.Forms.ColumnHeader IPColumn;
		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem ServerMenu;
		private System.Windows.Forms.ToolStripMenuItem QuitMenuItem;
		private System.Windows.Forms.ToolStripMenuItem StartMenuItem;
		private System.Windows.Forms.ToolStripMenuItem StopMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem managementToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
		private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem kickSelectedPlayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem banSelectedPlayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unbanToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem broadcastMessageToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem clearServerLogToolStripMenuItem;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel TitleLabel;
	}
}

