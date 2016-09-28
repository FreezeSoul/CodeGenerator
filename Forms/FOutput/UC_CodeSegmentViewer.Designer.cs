namespace CodeGenerator
{
	partial class UC_CodeSegmentViewer
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_CodeSegmentViewer));
			this._code_richTextBox = new System.Windows.Forms.RichTextBox();
			this._toolStrip1 = new System.Windows.Forms.ToolStrip();
			this._copy_toolStripButton = new System.Windows.Forms.ToolStripButton();
			this._title_toolStripLabel = new System.Windows.Forms.ToolStripLabel();
			this._toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _code_richTextBox
			// 
			this._code_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._code_richTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._code_richTextBox.Location = new System.Drawing.Point(0, 25);
			this._code_richTextBox.Name = "_code_richTextBox";
			this._code_richTextBox.Size = new System.Drawing.Size(427, 361);
			this._code_richTextBox.TabIndex = 6;
			this._code_richTextBox.Text = "";
			// 
			// _toolStrip1
			// 
			this._toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copy_toolStripButton,
            this._title_toolStripLabel});
			this._toolStrip1.Location = new System.Drawing.Point(0, 0);
			this._toolStrip1.Name = "_toolStrip1";
			this._toolStrip1.Size = new System.Drawing.Size(427, 25);
			this._toolStrip1.TabIndex = 5;
			this._toolStrip1.Text = "toolStrip1";
			// 
			// _copy_toolStripButton
			// 
			this._copy_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_copy_toolStripButton.Image")));
			this._copy_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._copy_toolStripButton.Name = "_copy_toolStripButton";
			this._copy_toolStripButton.Size = new System.Drawing.Size(49, 22);
			this._copy_toolStripButton.Text = "&Copy";
			this._copy_toolStripButton.Click += new System.EventHandler(this._copy_toolStripButton_Click);
			// 
			// _title_toolStripLabel
			// 
			this._title_toolStripLabel.Name = "_title_toolStripLabel";
			this._title_toolStripLabel.Size = new System.Drawing.Size(35, 22);
			this._title_toolStripLabel.Text = "title";
			// 
			// UC_CodeSegmentViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._code_richTextBox);
			this.Controls.Add(this._toolStrip1);
			this.Name = "UC_CodeSegmentViewer";
			this.Size = new System.Drawing.Size(427, 386);
			this._toolStrip1.ResumeLayout(false);
			this._toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.RichTextBox _code_richTextBox;
		private System.Windows.Forms.ToolStrip _toolStrip1;
		private System.Windows.Forms.ToolStripButton _copy_toolStripButton;
		private System.Windows.Forms.ToolStripLabel _title_toolStripLabel;
	}
}
