namespace CodeGenerator
{
	partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this._SplitContainer = new System.Windows.Forms.SplitContainer();
            this._TreeView = new System.Windows.Forms.TreeView();
            this._MenuStrip = new System.Windows.Forms.MenuStrip();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ToolStrip = new System.Windows.Forms.ToolStrip();
            this._Refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._ImageList = new System.Windows.Forms.ImageList(this.components);
            this._SplitContainer.Panel1.SuspendLayout();
            this._SplitContainer.SuspendLayout();
            this._MenuStrip.SuspendLayout();
            this._ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _SplitContainer
            // 
            this._SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._SplitContainer.Location = new System.Drawing.Point(0, 49);
            this._SplitContainer.Name = "_SplitContainer";
            // 
            // _SplitContainer.Panel1
            // 
            this._SplitContainer.Panel1.Controls.Add(this._TreeView);
            this._SplitContainer.Size = new System.Drawing.Size(1079, 625);
            this._SplitContainer.SplitterDistance = 263;
            this._SplitContainer.TabIndex = 0;
            // 
            // _TreeView
            // 
            this._TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TreeView.Location = new System.Drawing.Point(0, 0);
            this._TreeView.Name = "_TreeView";
            this._TreeView.Size = new System.Drawing.Size(263, 625);
            this._TreeView.TabIndex = 0;
            this._TreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._TreeView_MouseClick);
            this._TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._TreeView_AfterSelect);
            // 
            // _MenuStrip
            // 
            this._MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.readmeToolStripMenuItem});
            this._MenuStrip.Location = new System.Drawing.Point(0, 0);
            this._MenuStrip.Name = "_MenuStrip";
            this._MenuStrip.Size = new System.Drawing.Size(1079, 24);
            this._MenuStrip.TabIndex = 1;
            this._MenuStrip.Text = "menuStrip1";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出XToolStripMenuItem});
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.refreshToolStripMenuItem.Text = "&File";
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.退出XToolStripMenuItem.Text = "E&xit";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // readmeToolStripMenuItem
            // 
            this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
            this.readmeToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.readmeToolStripMenuItem.Text = "&Help";
            this.readmeToolStripMenuItem.Click += new System.EventHandler(this.readmeToolStripMenuItem_Click);
            // 
            // _ToolStrip
            // 
            this._ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._Refresh_ToolStripButton});
            this._ToolStrip.Location = new System.Drawing.Point(0, 24);
            this._ToolStrip.Name = "_ToolStrip";
            this._ToolStrip.Size = new System.Drawing.Size(1079, 25);
            this._ToolStrip.TabIndex = 2;
            this._ToolStrip.Text = "toolStrip1";
            // 
            // _Refresh_ToolStripButton
            // 
            this._Refresh_ToolStripButton.Image = global::CodeGenerator.Properties.Resources.RefreshDocViewHS;
            this._Refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._Refresh_ToolStripButton.Name = "_Refresh_ToolStripButton";
            this._Refresh_ToolStripButton.Size = new System.Drawing.Size(66, 22);
            this._Refresh_ToolStripButton.Text = "&Refresh";
            this._Refresh_ToolStripButton.Click += new System.EventHandler(this._Refresh_ToolStripButton_Click);
            // 
            // _ImageList
            // 
            this._ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_ImageList.ImageStream")));
            this._ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this._ImageList.Images.SetKeyName(0, "SQL_Column.png");
            this._ImageList.Images.SetKeyName(1, "SQL_Constrain.png");
            this._ImageList.Images.SetKeyName(2, "SQL_DataType.png");
            this._ImageList.Images.SetKeyName(3, "SQL_Diagram.png");
            this._ImageList.Images.SetKeyName(4, "SQL_Folder.png");
            this._ImageList.Images.SetKeyName(5, "SQL_Function_Scale.png");
            this._ImageList.Images.SetKeyName(6, "SQL_Function_Table.png");
            this._ImageList.Images.SetKeyName(7, "SQL_Key.png");
            this._ImageList.Images.SetKeyName(8, "SQL_Server.png");
            this._ImageList.Images.SetKeyName(9, "SQL_StoredProcedure.png");
            this._ImageList.Images.SetKeyName(10, "SQL_Table.png");
            this._ImageList.Images.SetKeyName(11, "SQL_Trigger.png");
            this._ImageList.Images.SetKeyName(12, "SQL_Database.png");
            // 
            // FMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1079, 674);
            this.Controls.Add(this._SplitContainer);
            this.Controls.Add(this._ToolStrip);
            this.Controls.Add(this._MenuStrip);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this._MenuStrip;
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CodeGenerator";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this._SplitContainer.Panel1.ResumeLayout(false);
            this._SplitContainer.ResumeLayout(false);
            this._MenuStrip.ResumeLayout(false);
            this._MenuStrip.PerformLayout();
            this._ToolStrip.ResumeLayout(false);
            this._ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer _SplitContainer;
		private System.Windows.Forms.TreeView _TreeView;
		private System.Windows.Forms.MenuStrip _MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStrip _ToolStrip;
		private System.Windows.Forms.ToolStripButton _Refresh_ToolStripButton;
		private System.Windows.Forms.ImageList _ImageList;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem readmeToolStripMenuItem;
	}
}

