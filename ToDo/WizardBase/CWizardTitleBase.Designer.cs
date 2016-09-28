namespace CodeGenerator
{
	partial class CWizardTitleBase
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
			this._Intro_label = new System.Windows.Forms.Label();
			this._Title_label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _Intro_label
			// 
			this._Intro_label.AutoSize = true;
			this._Intro_label.Location = new System.Drawing.Point(43, 29);
			this._Intro_label.Name = "_Intro_label";
			this._Intro_label.Size = new System.Drawing.Size(29, 12);
			this._Intro_label.TabIndex = 3;
			this._Intro_label.Text = "说明";
			// 
			// _Title_label
			// 
			this._Title_label.AutoSize = true;
			this._Title_label.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this._Title_label.Location = new System.Drawing.Point(20, 9);
			this._Title_label.Name = "_Title_label";
			this._Title_label.Size = new System.Drawing.Size(42, 16);
			this._Title_label.TabIndex = 2;
			this._Title_label.Text = "标题";
			// 
			// CWizardTitleBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this._Intro_label);
			this.Controls.Add(this._Title_label);
			this.Name = "CWizardTitleBase";
			this.Size = new System.Drawing.Size(684, 80);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _Intro_label;
		private System.Windows.Forms.Label _Title_label;
	}
}
