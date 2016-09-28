namespace CodeGenerator.Components.UI.SilverLight
{
    partial class FGen_Database_Config
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._cancel_button = new System.Windows.Forms.Button();
            this._isSupportSchema_checkBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._submit_button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._isSupportWCF_checkBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._namespace_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSelectUiModel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Location = new System.Drawing.Point(25, 84);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(369, 1);
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // _cancel_button
            // 
            this._cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_button.Location = new System.Drawing.Point(305, 222);
            this._cancel_button.Name = "_cancel_button";
            this._cancel_button.Size = new System.Drawing.Size(75, 25);
            this._cancel_button.TabIndex = 17;
            this._cancel_button.Text = "取消(&C)";
            this._cancel_button.UseVisualStyleBackColor = true;
            // 
            // _isSupportSchema_checkBox
            // 
            this._isSupportSchema_checkBox.AutoSize = true;
            this._isSupportSchema_checkBox.Location = new System.Drawing.Point(168, 137);
            this._isSupportSchema_checkBox.Name = "_isSupportSchema_checkBox";
            this._isSupportSchema_checkBox.Size = new System.Drawing.Size(144, 16);
            this._isSupportSchema_checkBox.TabIndex = 14;
            this._isSupportSchema_checkBox.Text = "是否支持 &Schema 生成";
            this._isSupportSchema_checkBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F);
            this.label2.Location = new System.Drawing.Point(16, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "请填选当前数据库相关生成选项";
            // 
            // _submit_button
            // 
            this._submit_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._submit_button.Location = new System.Drawing.Point(216, 222);
            this._submit_button.Name = "_submit_button";
            this._submit_button.Size = new System.Drawing.Size(75, 25);
            this._submit_button.TabIndex = 16;
            this._submit_button.Text = "生成(&G)";
            this._submit_button.UseVisualStyleBackColor = true;
            this._submit_button.Click += new System.EventHandler(this._submit_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(25, 211);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(369, 1);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // _isSupportWCF_checkBox
            // 
            this._isSupportWCF_checkBox.AutoSize = true;
            this._isSupportWCF_checkBox.Location = new System.Drawing.Point(168, 164);
            this._isSupportWCF_checkBox.Name = "_isSupportWCF_checkBox";
            this._isSupportWCF_checkBox.Size = new System.Drawing.Size(186, 16);
            this._isSupportWCF_checkBox.TabIndex = 13;
            this._isSupportWCF_checkBox.Text = "是否为 &WCF 生成数据对象标记";
            this._isSupportWCF_checkBox.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(0, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 68);
            this.panel1.TabIndex = 12;
            // 
            // _namespace_textBox
            // 
            this._namespace_textBox.Location = new System.Drawing.Point(168, 106);
            this._namespace_textBox.Name = "_namespace_textBox";
            this._namespace_textBox.Size = new System.Drawing.Size(234, 21);
            this._namespace_textBox.TabIndex = 11;
            this._namespace_textBox.TextChanged += new System.EventHandler(this._namespace_textBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "WCF支持：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "数据库架构：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "命名空间(&N)：";
            // 
            // cmbSelectUiModel
            // 
            this.cmbSelectUiModel.FormattingEnabled = true;
            this.cmbSelectUiModel.Location = new System.Drawing.Point(168, 187);
            this.cmbSelectUiModel.Name = "cmbSelectUiModel";
            this.cmbSelectUiModel.Size = new System.Drawing.Size(234, 20);
            this.cmbSelectUiModel.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "模板选择：";
            // 
            // FGen_Silverlight_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 263);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbSelectUiModel);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this._cancel_button);
            this.Controls.Add(this._isSupportSchema_checkBox);
            this.Controls.Add(this._submit_button);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._isSupportWCF_checkBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._namespace_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "FGen_Silverlight_Config";
            this.Text = "Silverlight 生成选项设定";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button _cancel_button;
        private System.Windows.Forms.CheckBox _isSupportSchema_checkBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _submit_button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox _isSupportWCF_checkBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _namespace_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSelectUiModel;
        private System.Windows.Forms.Label label5;
    }
}