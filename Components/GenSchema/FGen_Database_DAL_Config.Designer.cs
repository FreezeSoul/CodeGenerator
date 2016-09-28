namespace CodeGenerator.Components.DAL
{
    partial class FGen_Database_DAL_Config
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
            this._namespace_textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._isSupportSchema_checkBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this._isSupportWCF_checkBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._submit_button = new System.Windows.Forms.Button();
            this._cancel_button = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._IsSupportDS_checkBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this._IsSupportOO_checkBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this._IsSupportDB_Table_checkBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this._IsSupportDB_View_checkBox = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this._IsSupportDB_Function_checkBox = new System.Windows.Forms.CheckBox();
            this._IsSupportDB_SP_checkBox = new System.Windows.Forms.CheckBox();
            this._DBGen_groupBox = new System.Windows.Forms.GroupBox();
            this._OBGen_groupBox = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this._IsSupportOB_Extend_checkBox = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this._IsSupportOB_Table_checkBox = new System.Windows.Forms.CheckBox();
            this._IsSupportOB_Function_checkBox = new System.Windows.Forms.CheckBox();
            this._IsSupportOB_View_checkBox = new System.Windows.Forms.CheckBox();
            this._IsSupportOB_SP_checkBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this._DBGen_groupBox.SuspendLayout();
            this._OBGen_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "命名空间：";
            // 
            // _namespace_textBox
            // 
            this._namespace_textBox.Location = new System.Drawing.Point(141, 91);
            this._namespace_textBox.Name = "_namespace_textBox";
            this._namespace_textBox.Size = new System.Drawing.Size(234, 21);
            this._namespace_textBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 68);
            this.panel1.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(87, 41);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(278, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "注意：您所作的修改将于点击“保存”之后保存";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(16, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(288, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "请填选当前数据库于 DAL 相关生成选项";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "数据库架构支持：";
            // 
            // _isSupportSchema_checkBox
            // 
            this._isSupportSchema_checkBox.AutoSize = true;
            this._isSupportSchema_checkBox.Location = new System.Drawing.Point(141, 120);
            this._isSupportSchema_checkBox.Name = "_isSupportSchema_checkBox";
            this._isSupportSchema_checkBox.Size = new System.Drawing.Size(192, 16);
            this._isSupportSchema_checkBox.TabIndex = 3;
            this._isSupportSchema_checkBox.Text = "对象将以 Schema 名作为前缀名";
            this._isSupportSchema_checkBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "WCF 支持：";
            // 
            // _isSupportWCF_checkBox
            // 
            this._isSupportWCF_checkBox.AutoSize = true;
            this._isSupportWCF_checkBox.Location = new System.Drawing.Point(137, 305);
            this._isSupportWCF_checkBox.Name = "_isSupportWCF_checkBox";
            this._isSupportWCF_checkBox.Size = new System.Drawing.Size(228, 16);
            this._isSupportWCF_checkBox.TabIndex = 3;
            this._isSupportWCF_checkBox.Text = "添加 [DataMember] 特性 (.net 3.0+)";
            this._isSupportWCF_checkBox.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(0, 451);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(420, 1);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // _submit_button
            // 
            this._submit_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._submit_button.Location = new System.Drawing.Point(242, 461);
            this._submit_button.Name = "_submit_button";
            this._submit_button.Size = new System.Drawing.Size(75, 25);
            this._submit_button.TabIndex = 5;
            this._submit_button.Text = "保存(&G)";
            this._submit_button.UseVisualStyleBackColor = true;
            this._submit_button.Click += new System.EventHandler(this._submit_button_Click);
            // 
            // _cancel_button
            // 
            this._cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_button.Location = new System.Drawing.Point(331, 461);
            this._cancel_button.Name = "_cancel_button";
            this._cancel_button.Size = new System.Drawing.Size(75, 25);
            this._cancel_button.TabIndex = 6;
            this._cancel_button.Text = "取消(&C)";
            this._cancel_button.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Location = new System.Drawing.Point(0, 69);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(420, 1);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // _IsSupportDS_checkBox
            // 
            this._IsSupportDS_checkBox.AutoSize = true;
            this._IsSupportDS_checkBox.Location = new System.Drawing.Point(141, 154);
            this._IsSupportDS_checkBox.Name = "_IsSupportDS_checkBox";
            this._IsSupportDS_checkBox.Size = new System.Drawing.Size(108, 16);
            this._IsSupportDS_checkBox.TabIndex = 9;
            this._IsSupportDS_checkBox.Text = "强类型 DataSet";
            this._IsSupportDS_checkBox.UseVisualStyleBackColor = true;
            this._IsSupportDS_checkBox.CheckedChanged += new System.EventHandler(this._IsSupportDS_checkBox_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "数据集支持：";
            // 
            // _IsSupportOO_checkBox
            // 
            this._IsSupportOO_checkBox.AutoSize = true;
            this._IsSupportOO_checkBox.Location = new System.Drawing.Point(137, 285);
            this._IsSupportOO_checkBox.Name = "_IsSupportOO_checkBox";
            this._IsSupportOO_checkBox.Size = new System.Drawing.Size(96, 16);
            this._IsSupportOO_checkBox.TabIndex = 11;
            this._IsSupportOO_checkBox.Text = "Class Entity";
            this._IsSupportOO_checkBox.UseVisualStyleBackColor = true;
            this._IsSupportOO_checkBox.CheckedChanged += new System.EventHandler(this._IsSupportOO_checkBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 286);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "类实例支持：";
            // 
            // _IsSupportDB_Table_checkBox
            // 
            this._IsSupportDB_Table_checkBox.AutoSize = true;
            this._IsSupportDB_Table_checkBox.Location = new System.Drawing.Point(67, 27);
            this._IsSupportDB_Table_checkBox.Name = "_IsSupportDB_Table_checkBox";
            this._IsSupportDB_Table_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportDB_Table_checkBox.TabIndex = 13;
            this._IsSupportDB_Table_checkBox.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "表：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "视图：";
            // 
            // _IsSupportDB_View_checkBox
            // 
            this._IsSupportDB_View_checkBox.AutoSize = true;
            this._IsSupportDB_View_checkBox.Location = new System.Drawing.Point(67, 51);
            this._IsSupportDB_View_checkBox.Name = "_IsSupportDB_View_checkBox";
            this._IsSupportDB_View_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportDB_View_checkBox.TabIndex = 13;
            this._IsSupportDB_View_checkBox.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(237, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 12;
            this.label11.Text = "函数：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(213, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "存储过程：";
            // 
            // _IsSupportDB_Function_checkBox
            // 
            this._IsSupportDB_Function_checkBox.AutoSize = true;
            this._IsSupportDB_Function_checkBox.Location = new System.Drawing.Point(284, 27);
            this._IsSupportDB_Function_checkBox.Name = "_IsSupportDB_Function_checkBox";
            this._IsSupportDB_Function_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportDB_Function_checkBox.TabIndex = 13;
            this._IsSupportDB_Function_checkBox.UseVisualStyleBackColor = true;
            // 
            // _IsSupportDB_SP_checkBox
            // 
            this._IsSupportDB_SP_checkBox.AutoSize = true;
            this._IsSupportDB_SP_checkBox.Location = new System.Drawing.Point(284, 51);
            this._IsSupportDB_SP_checkBox.Name = "_IsSupportDB_SP_checkBox";
            this._IsSupportDB_SP_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportDB_SP_checkBox.TabIndex = 13;
            this._IsSupportDB_SP_checkBox.UseVisualStyleBackColor = true;
            // 
            // _DBGen_groupBox
            // 
            this._DBGen_groupBox.Controls.Add(this.label7);
            this._DBGen_groupBox.Controls.Add(this.label9);
            this._DBGen_groupBox.Controls.Add(this.label11);
            this._DBGen_groupBox.Controls.Add(this.label12);
            this._DBGen_groupBox.Controls.Add(this._IsSupportDB_Table_checkBox);
            this._DBGen_groupBox.Controls.Add(this._IsSupportDB_Function_checkBox);
            this._DBGen_groupBox.Controls.Add(this._IsSupportDB_View_checkBox);
            this._DBGen_groupBox.Controls.Add(this._IsSupportDB_SP_checkBox);
            this._DBGen_groupBox.Enabled = false;
            this._DBGen_groupBox.Location = new System.Drawing.Point(46, 178);
            this._DBGen_groupBox.Name = "_DBGen_groupBox";
            this._DBGen_groupBox.Size = new System.Drawing.Size(329, 80);
            this._DBGen_groupBox.TabIndex = 22;
            this._DBGen_groupBox.TabStop = false;
            this._DBGen_groupBox.Text = "数据集相关方法生成";
            // 
            // _OBGen_groupBox
            // 
            this._OBGen_groupBox.Controls.Add(this.label15);
            this._OBGen_groupBox.Controls.Add(this.label8);
            this._OBGen_groupBox.Controls.Add(this.label10);
            this._OBGen_groupBox.Controls.Add(this.label13);
            this._OBGen_groupBox.Controls.Add(this._IsSupportOB_Extend_checkBox);
            this._OBGen_groupBox.Controls.Add(this.label14);
            this._OBGen_groupBox.Controls.Add(this._IsSupportOB_Table_checkBox);
            this._OBGen_groupBox.Controls.Add(this._IsSupportOB_Function_checkBox);
            this._OBGen_groupBox.Controls.Add(this._IsSupportOB_View_checkBox);
            this._OBGen_groupBox.Controls.Add(this._IsSupportOB_SP_checkBox);
            this._OBGen_groupBox.Enabled = false;
            this._OBGen_groupBox.Location = new System.Drawing.Point(46, 327);
            this._OBGen_groupBox.Name = "_OBGen_groupBox";
            this._OBGen_groupBox.Size = new System.Drawing.Size(329, 106);
            this._OBGen_groupBox.TabIndex = 22;
            this._OBGen_groupBox.TabStop = false;
            this._OBGen_groupBox.Text = "类实例相关方法生成";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "扩展：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "表：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 12;
            this.label10.Text = "视图：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(237, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 12;
            this.label13.Text = "函数：";
            // 
            // _IsSupportOB_Extend_checkBox
            // 
            this._IsSupportOB_Extend_checkBox.AutoSize = true;
            this._IsSupportOB_Extend_checkBox.Enabled = false;
            this._IsSupportOB_Extend_checkBox.Location = new System.Drawing.Point(67, 75);
            this._IsSupportOB_Extend_checkBox.Name = "_IsSupportOB_Extend_checkBox";
            this._IsSupportOB_Extend_checkBox.Size = new System.Drawing.Size(210, 16);
            this._IsSupportOB_Extend_checkBox.TabIndex = 13;
            this._IsSupportOB_Extend_checkBox.Text = "表，视图实例扩展方法(.net 3.5+)";
            this._IsSupportOB_Extend_checkBox.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(213, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 12;
            this.label14.Text = "存储过程：";
            // 
            // _IsSupportOB_Table_checkBox
            // 
            this._IsSupportOB_Table_checkBox.AutoSize = true;
            this._IsSupportOB_Table_checkBox.Location = new System.Drawing.Point(67, 27);
            this._IsSupportOB_Table_checkBox.Name = "_IsSupportOB_Table_checkBox";
            this._IsSupportOB_Table_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportOB_Table_checkBox.TabIndex = 13;
            this._IsSupportOB_Table_checkBox.UseVisualStyleBackColor = true;
            this._IsSupportOB_Table_checkBox.CheckedChanged += new System.EventHandler(this._IsSupportOB_Table_checkBox_CheckedChanged);
            // 
            // _IsSupportOB_Function_checkBox
            // 
            this._IsSupportOB_Function_checkBox.AutoSize = true;
            this._IsSupportOB_Function_checkBox.Location = new System.Drawing.Point(284, 27);
            this._IsSupportOB_Function_checkBox.Name = "_IsSupportOB_Function_checkBox";
            this._IsSupportOB_Function_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportOB_Function_checkBox.TabIndex = 13;
            this._IsSupportOB_Function_checkBox.UseVisualStyleBackColor = true;
            // 
            // _IsSupportOB_View_checkBox
            // 
            this._IsSupportOB_View_checkBox.AutoSize = true;
            this._IsSupportOB_View_checkBox.Location = new System.Drawing.Point(67, 51);
            this._IsSupportOB_View_checkBox.Name = "_IsSupportOB_View_checkBox";
            this._IsSupportOB_View_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportOB_View_checkBox.TabIndex = 13;
            this._IsSupportOB_View_checkBox.UseVisualStyleBackColor = true;
            this._IsSupportOB_View_checkBox.CheckedChanged += new System.EventHandler(this._IsSupportOB_View_checkBox_CheckedChanged);
            // 
            // _IsSupportOB_SP_checkBox
            // 
            this._IsSupportOB_SP_checkBox.AutoSize = true;
            this._IsSupportOB_SP_checkBox.Location = new System.Drawing.Point(284, 51);
            this._IsSupportOB_SP_checkBox.Name = "_IsSupportOB_SP_checkBox";
            this._IsSupportOB_SP_checkBox.Size = new System.Drawing.Size(15, 14);
            this._IsSupportOB_SP_checkBox.TabIndex = 13;
            this._IsSupportOB_SP_checkBox.UseVisualStyleBackColor = true;
            // 
            // FGen_Database_DAL_Config
            // 
            this.AcceptButton = this._submit_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel_button;
            this.ClientSize = new System.Drawing.Size(418, 496);
            this.Controls.Add(this._OBGen_groupBox);
            this.Controls.Add(this._DBGen_groupBox);
            this.Controls.Add(this._IsSupportOO_checkBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._IsSupportDS_checkBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this._cancel_button);
            this.Controls.Add(this._submit_button);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._isSupportWCF_checkBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._isSupportSchema_checkBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._namespace_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FGen_Database_DAL_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DAL生成选项设定";
            this.Load += new System.EventHandler(this.FGen_Database_DAL_Config_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this._DBGen_groupBox.ResumeLayout(false);
            this._DBGen_groupBox.PerformLayout();
            this._OBGen_groupBox.ResumeLayout(false);
            this._OBGen_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _namespace_textBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox _isSupportSchema_checkBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _isSupportWCF_checkBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button _submit_button;
        private System.Windows.Forms.Button _cancel_button;
        private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.CheckBox _IsSupportDS_checkBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox _IsSupportOO_checkBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox _IsSupportDB_Table_checkBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox _IsSupportDB_View_checkBox;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox _IsSupportDB_Function_checkBox;
		private System.Windows.Forms.CheckBox _IsSupportDB_SP_checkBox;
		private System.Windows.Forms.GroupBox _DBGen_groupBox;
		private System.Windows.Forms.GroupBox _OBGen_groupBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.CheckBox _IsSupportOB_Table_checkBox;
		private System.Windows.Forms.CheckBox _IsSupportOB_Function_checkBox;
		private System.Windows.Forms.CheckBox _IsSupportOB_View_checkBox;
		private System.Windows.Forms.CheckBox _IsSupportOB_SP_checkBox;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox _IsSupportOB_Extend_checkBox;
		private System.Windows.Forms.Label label16;
    }
}