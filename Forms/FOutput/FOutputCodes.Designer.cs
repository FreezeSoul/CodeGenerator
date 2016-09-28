namespace CodeGenerator
{
	partial class FOutputCodes
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
			this._codes_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// _codes_flowLayoutPanel
			// 
			this._codes_flowLayoutPanel.AutoScroll = true;
			this._codes_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._codes_flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._codes_flowLayoutPanel.Name = "_codes_flowLayoutPanel";
			this._codes_flowLayoutPanel.Size = new System.Drawing.Size(951, 725);
			this._codes_flowLayoutPanel.TabIndex = 0;
			// 
			// FOutputCodes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(951, 725);
			this.Controls.Add(this._codes_flowLayoutPanel);
			this.Name = "FOutputCodes";
			this.Text = "多代码段输出";
			this.Load += new System.EventHandler(this.FOutputCodes_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel _codes_flowLayoutPanel;
	}
}