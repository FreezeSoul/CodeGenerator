namespace CodeGenerator
{
	partial class FWizardBase
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
			this._Title_panel = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this._Back_button = new System.Windows.Forms.Button();
			this._Next_button = new System.Windows.Forms.Button();
			this._Finish_button = new System.Windows.Forms.Button();
			this._Cancel_button = new System.Windows.Forms.Button();
			this._Help_button = new System.Windows.Forms.Button();
			this._Content_panel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// _Title_panel
			// 
			this._Title_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._Title_panel.BackColor = System.Drawing.Color.White;
			this._Title_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._Title_panel.Location = new System.Drawing.Point(0, 0);
			this._Title_panel.Name = "_Title_panel";
			this._Title_panel.Size = new System.Drawing.Size(684, 80);
			this._Title_panel.TabIndex = 0;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.DarkGray;
			this.pictureBox1.Location = new System.Drawing.Point(2, 414);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(680, 2);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox2.BackColor = System.Drawing.Color.Gray;
			this.pictureBox2.Location = new System.Drawing.Point(0, 78);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(684, 2);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// _Back_button
			// 
			this._Back_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._Back_button.Location = new System.Drawing.Point(247, 425);
			this._Back_button.Name = "_Back_button";
			this._Back_button.Size = new System.Drawing.Size(95, 28);
			this._Back_button.TabIndex = 1;
			this._Back_button.Text = "< 上一步(&B)";
			this._Back_button.UseVisualStyleBackColor = true;
			this._Back_button.Click += new System.EventHandler(this._Back_button_Click);
			// 
			// _Next_button
			// 
			this._Next_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._Next_button.Location = new System.Drawing.Point(348, 425);
			this._Next_button.Name = "_Next_button";
			this._Next_button.Size = new System.Drawing.Size(95, 28);
			this._Next_button.TabIndex = 2;
			this._Next_button.Text = "下一步(&N) >";
			this._Next_button.UseVisualStyleBackColor = true;
			this._Next_button.Click += new System.EventHandler(this._Next_button_Click);
			// 
			// _Finish_button
			// 
			this._Finish_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._Finish_button.Location = new System.Drawing.Point(458, 425);
			this._Finish_button.Name = "_Finish_button";
			this._Finish_button.Size = new System.Drawing.Size(95, 28);
			this._Finish_button.TabIndex = 3;
			this._Finish_button.Text = "完成(&F) >>|";
			this._Finish_button.UseVisualStyleBackColor = true;
			this._Finish_button.Click += new System.EventHandler(this._Finish_button_Click);
			// 
			// _Cancel_button
			// 
			this._Cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._Cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._Cancel_button.Location = new System.Drawing.Point(569, 425);
			this._Cancel_button.Name = "_Cancel_button";
			this._Cancel_button.Size = new System.Drawing.Size(95, 28);
			this._Cancel_button.TabIndex = 4;
			this._Cancel_button.Text = "取消(&C)";
			this._Cancel_button.UseVisualStyleBackColor = true;
			// 
			// _Help_button
			// 
			this._Help_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._Help_button.Location = new System.Drawing.Point(20, 425);
			this._Help_button.Name = "_Help_button";
			this._Help_button.Size = new System.Drawing.Size(95, 28);
			this._Help_button.TabIndex = 0;
			this._Help_button.Text = "帮助(&H)";
			this._Help_button.UseVisualStyleBackColor = true;
			this._Help_button.Click += new System.EventHandler(this._Help_button_Click);
			// 
			// _Content_panel
			// 
			this._Content_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._Content_panel.Location = new System.Drawing.Point(0, 80);
			this._Content_panel.Name = "_Content_panel";
			this._Content_panel.Size = new System.Drawing.Size(684, 334);
			this._Content_panel.TabIndex = 3;
			// 
			// FWizardBase
			// 
			this.AcceptButton = this._Next_button;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._Cancel_button;
			this.ClientSize = new System.Drawing.Size(684, 464);
			this.Controls.Add(this._Content_panel);
			this.Controls.Add(this._Cancel_button);
			this.Controls.Add(this._Finish_button);
			this.Controls.Add(this._Next_button);
			this.Controls.Add(this._Help_button);
			this.Controls.Add(this._Back_button);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this._Title_panel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(586, 442);
			this.Name = "FWizardBase";
			this.Text = "FWizardBase";
			this.Load += new System.EventHandler(this.FFilterWizard_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel _Title_panel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button _Back_button;
		private System.Windows.Forms.Button _Next_button;
		private System.Windows.Forms.Button _Finish_button;
		private System.Windows.Forms.Button _Cancel_button;
		private System.Windows.Forms.Button _Help_button;
		private System.Windows.Forms.Panel _Content_panel;

	}
}