namespace CodeGenerator
{
	partial class FOutputHtml
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
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// _webBrowser
			// 
			this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._webBrowser.Location = new System.Drawing.Point(0, 0);
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.Size = new System.Drawing.Size(539, 333);
			this._webBrowser.TabIndex = 0;
			// 
			// FOutputHtml
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(539, 333);
			this.Controls.Add(this._webBrowser);
			this.Name = "FOutputHtml";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "页面内容输出";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser _webBrowser;
	}
}