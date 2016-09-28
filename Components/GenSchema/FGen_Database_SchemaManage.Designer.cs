namespace CodeGenerator.Components.GenSchema
{
	partial class FGen_Database_SchemaManage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._Save_button = new System.Windows.Forms.Button();
            this._Name_textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._Filter_button = new System.Windows.Forms.Button();
            this._GenOption_button = new System.Windows.Forms.Button();
            this._Gen_button = new System.Windows.Forms.Button();
            this._Schemes_listBox = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this._Delete_button = new System.Windows.Forms.Button();
            this._Insert_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._Memo_textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "方案名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "说明：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.34783F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 325F));
            this.tableLayoutPanel1.Controls.Add(this._Save_button, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._Name_textBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this._Schemes_listBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._Memo_textBox, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(549, 338);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // _Save_button
            // 
            this._Save_button.Location = new System.Drawing.Point(227, 276);
            this._Save_button.Name = "_Save_button";
            this._Save_button.Size = new System.Drawing.Size(197, 22);
            this._Save_button.TabIndex = 1;
            this._Save_button.Text = "保存方案名和说明的修改(&S)";
            this._Save_button.UseVisualStyleBackColor = true;
            this._Save_button.Click += new System.EventHandler(this._Save_button_Click);
            // 
            // _Name_textBox
            // 
            this._Name_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Name_textBox.Location = new System.Drawing.Point(227, 3);
            this._Name_textBox.Name = "_Name_textBox";
            this._Name_textBox.Size = new System.Drawing.Size(319, 21);
            this._Name_textBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._Filter_button);
            this.panel1.Controls.Add(this._GenOption_button);
            this.panel1.Controls.Add(this._Gen_button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(224, 301);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(325, 37);
            this.panel1.TabIndex = 4;
            // 
            // _Filter_button
            // 
            this._Filter_button.Location = new System.Drawing.Point(3, 5);
            this._Filter_button.Name = "_Filter_button";
            this._Filter_button.Size = new System.Drawing.Size(96, 23);
            this._Filter_button.TabIndex = 1;
            this._Filter_button.Text = "过滤规则(&F)";
            this._Filter_button.UseVisualStyleBackColor = true;
            this._Filter_button.Click += new System.EventHandler(this._Filter_button_Click);
            // 
            // _GenOption_button
            // 
            this._GenOption_button.Location = new System.Drawing.Point(105, 5);
            this._GenOption_button.Name = "_GenOption_button";
            this._GenOption_button.Size = new System.Drawing.Size(95, 23);
            this._GenOption_button.TabIndex = 1;
            this._GenOption_button.Text = "生成选项(&O)";
            this._GenOption_button.UseVisualStyleBackColor = true;
            this._GenOption_button.Click += new System.EventHandler(this._GenOption_button_Click);
            // 
            // _Gen_button
            // 
            this._Gen_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Gen_button.Location = new System.Drawing.Point(251, 5);
            this._Gen_button.Name = "_Gen_button";
            this._Gen_button.Size = new System.Drawing.Size(71, 23);
            this._Gen_button.TabIndex = 0;
            this._Gen_button.Text = "生成(&G)";
            this._Gen_button.UseVisualStyleBackColor = true;
            this._Gen_button.Click += new System.EventHandler(this._Gen_button_Click);
            // 
            // _Schemes_listBox
            // 
            this._Schemes_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Schemes_listBox.FormattingEnabled = true;
            this._Schemes_listBox.ItemHeight = 12;
            this._Schemes_listBox.Location = new System.Drawing.Point(3, 30);
            this._Schemes_listBox.Name = "_Schemes_listBox";
            this.tableLayoutPanel1.SetRowSpan(this._Schemes_listBox, 2);
            this._Schemes_listBox.Size = new System.Drawing.Size(154, 268);
            this._Schemes_listBox.TabIndex = 5;
            this._Schemes_listBox.SelectedIndexChanged += new System.EventHandler(this._Schema_listBox_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._Delete_button);
            this.panel2.Controls.Add(this._Insert_button);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 301);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(160, 37);
            this.panel2.TabIndex = 6;
            // 
            // _Delete_button
            // 
            this._Delete_button.Location = new System.Drawing.Point(75, 5);
            this._Delete_button.Name = "_Delete_button";
            this._Delete_button.Size = new System.Drawing.Size(66, 23);
            this._Delete_button.TabIndex = 1;
            this._Delete_button.Text = "删除(&D)";
            this._Delete_button.UseVisualStyleBackColor = true;
            this._Delete_button.Click += new System.EventHandler(this._Delete_button_Click);
            // 
            // _Insert_button
            // 
            this._Insert_button.Location = new System.Drawing.Point(3, 5);
            this._Insert_button.Name = "_Insert_button";
            this._Insert_button.Size = new System.Drawing.Size(66, 23);
            this._Insert_button.TabIndex = 1;
            this._Insert_button.Text = "新增(&I)";
            this._Insert_button.UseVisualStyleBackColor = true;
            this._Insert_button.Click += new System.EventHandler(this._Insert_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "方案列表：";
            // 
            // _Memo_textBox
            // 
            this._Memo_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Memo_textBox.Location = new System.Drawing.Point(227, 30);
            this._Memo_textBox.Multiline = true;
            this._Memo_textBox.Name = "_Memo_textBox";
            this._Memo_textBox.Size = new System.Drawing.Size(319, 240);
            this._Memo_textBox.TabIndex = 7;
            // 
            // FGen_Database_SchemaManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 338);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FGen_Database_SchemaManage";
            this.Text = "多配置方案管理器";
            this.Load += new System.EventHandler(this.FGen_Database_ExtendSchema_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox _Name_textBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button _Filter_button;
		private System.Windows.Forms.Button _GenOption_button;
		private System.Windows.Forms.Button _Gen_button;
		private System.Windows.Forms.ListBox _Schemes_listBox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button _Delete_button;
		private System.Windows.Forms.Button _Insert_button;
		private System.Windows.Forms.Button _Save_button;
		private System.Windows.Forms.TextBox _Memo_textBox;
	}
}