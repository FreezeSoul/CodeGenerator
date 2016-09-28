namespace CodeGenerator
{
	partial class FOutput2
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FOutput2));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this._richTextBox1 = new System.Windows.Forms.RichTextBox();
			this._toolStrip1 = new System.Windows.Forms.ToolStrip();
			this._copy_toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this._richTextBox2 = new System.Windows.Forms.RichTextBox();
			this._toolStrip2 = new System.Windows.Forms.ToolStrip();
			this._copy_toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this._toolStrip1.SuspendLayout();
			this._toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this._richTextBox1);
			this.splitContainer1.Panel1.Controls.Add(this._toolStrip1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this._richTextBox2);
			this.splitContainer1.Panel2.Controls.Add(this._toolStrip2);
			this.splitContainer1.Size = new System.Drawing.Size(953, 591);
			this.splitContainer1.SplitterDistance = 279;
			this.splitContainer1.TabIndex = 0;
			// 
			// _richTextBox1
			// 
			this._richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._richTextBox1.Location = new System.Drawing.Point(0, 25);
			this._richTextBox1.Name = "_richTextBox1";
			this._richTextBox1.Size = new System.Drawing.Size(953, 254);
			this._richTextBox1.TabIndex = 4;
			this._richTextBox1.Text = "";
			// 
			// _toolStrip1
			// 
			this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copy_toolStripButton1});
			this._toolStrip1.Location = new System.Drawing.Point(0, 0);
			this._toolStrip1.Name = "_toolStrip1";
			this._toolStrip1.Size = new System.Drawing.Size(953, 25);
			this._toolStrip1.TabIndex = 2;
			this._toolStrip1.Text = "toolStrip1";
			// 
			// _copy_toolStripButton1
			// 
			this._copy_toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("_copy_toolStripButton1.Image")));
			this._copy_toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._copy_toolStripButton1.Name = "_copy_toolStripButton1";
			this._copy_toolStripButton1.Size = new System.Drawing.Size(58, 22);
			this._copy_toolStripButton1.Text = "&Copy";
			this._copy_toolStripButton1.Click += new System.EventHandler(this._copy_toolStripButton1_Click);
			// 
			// _richTextBox2
			// 
			this._richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this._richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._richTextBox2.Location = new System.Drawing.Point(0, 25);
			this._richTextBox2.Name = "_richTextBox2";
			this._richTextBox2.Size = new System.Drawing.Size(953, 283);
			this._richTextBox2.TabIndex = 3;
			this._richTextBox2.Text = "";
			// 
			// _toolStrip2
			// 
			this._toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copy_toolStripButton2});
			this._toolStrip2.Location = new System.Drawing.Point(0, 0);
			this._toolStrip2.Name = "_toolStrip2";
			this._toolStrip2.Size = new System.Drawing.Size(953, 25);
			this._toolStrip2.TabIndex = 2;
			this._toolStrip2.Text = "toolStrip2";
			// 
			// _copy_toolStripButton2
			// 
			this._copy_toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("_copy_toolStripButton2.Image")));
			this._copy_toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._copy_toolStripButton2.Name = "_copy_toolStripButton2";
			this._copy_toolStripButton2.Size = new System.Drawing.Size(58, 22);
			this._copy_toolStripButton2.Text = "&Copy";
			this._copy_toolStripButton2.Click += new System.EventHandler(this._copy_toolStripButton2_Click);
			// 
			// FOutput2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(953, 591);
			this.Controls.Add(this.splitContainer1);
			this.Name = "FOutput2";
			this.Text = "组合式代码段输出";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this._toolStrip1.ResumeLayout(false);
			this._toolStrip1.PerformLayout();
			this._toolStrip2.ResumeLayout(false);
			this._toolStrip2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip _toolStrip1;
		private System.Windows.Forms.ToolStripButton _copy_toolStripButton1;
		private System.Windows.Forms.ToolStrip _toolStrip2;
		private System.Windows.Forms.ToolStripButton _copy_toolStripButton2;
		public System.Windows.Forms.RichTextBox _richTextBox2;
		public System.Windows.Forms.RichTextBox _richTextBox1;
	}
}