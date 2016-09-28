namespace CodeGenerator
{
	partial class FConnector
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
            this._ServerType_ComboBox = new System.Windows.Forms.ComboBox();
            this._ServerName_ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this._Authentication_ComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._Username_Label = new System.Windows.Forms.Label();
            this._Password_Label = new System.Windows.Forms.Label();
            this._Username_ComboBox = new System.Windows.Forms.ComboBox();
            this._Password_TextBox = new System.Windows.Forms.TextBox();
            this._RememberPassword_CheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._Connect_Button = new System.Windows.Forms.Button();
            this._Help_Button = new System.Windows.Forms.Button();
            this._Cancel_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server &type:";
            // 
            // _ServerType_ComboBox
            // 
            this._ServerType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ServerType_ComboBox.FormattingEnabled = true;
            this._ServerType_ComboBox.Items.AddRange(new object[] {
            "Microsoft SQL Server v200x"});
            this._ServerType_ComboBox.Location = new System.Drawing.Point(185, 86);
            this._ServerType_ComboBox.Name = "_ServerType_ComboBox";
            this._ServerType_ComboBox.Size = new System.Drawing.Size(325, 21);
            this._ServerType_ComboBox.TabIndex = 2;
            // 
            // _ServerName_ComboBox
            // 
            this._ServerName_ComboBox.FormattingEnabled = true;
            this._ServerName_ComboBox.Location = new System.Drawing.Point(185, 115);
            this._ServerName_ComboBox.MaxLength = 255;
            this._ServerName_ComboBox.Name = "_ServerName_ComboBox";
            this._ServerName_ComboBox.Size = new System.Drawing.Size(325, 21);
            this._ServerName_ComboBox.TabIndex = 4;
            this._ServerName_ComboBox.Text = ".";
            this._ServerName_ComboBox.DropDown += new System.EventHandler(this._ServerName_ComboBox_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Server name:";
            // 
            // _Authentication_ComboBox
            // 
            this._Authentication_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._Authentication_ComboBox.FormattingEnabled = true;
            this._Authentication_ComboBox.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this._Authentication_ComboBox.Location = new System.Drawing.Point(185, 144);
            this._Authentication_ComboBox.Name = "_Authentication_ComboBox";
            this._Authentication_ComboBox.Size = new System.Drawing.Size(325, 21);
            this._Authentication_ComboBox.TabIndex = 6;
            this._Authentication_ComboBox.SelectedIndexChanged += new System.EventHandler(this._Authentication_ComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Authentication:";
            // 
            // _Username_Label
            // 
            this._Username_Label.AutoSize = true;
            this._Username_Label.Location = new System.Drawing.Point(31, 181);
            this._Username_Label.Name = "_Username_Label";
            this._Username_Label.Size = new System.Drawing.Size(36, 13);
            this._Username_Label.TabIndex = 7;
            this._Username_Label.Text = "&Login:";
            // 
            // _Password_Label
            // 
            this._Password_Label.AutoSize = true;
            this._Password_Label.Location = new System.Drawing.Point(31, 211);
            this._Password_Label.Name = "_Password_Label";
            this._Password_Label.Size = new System.Drawing.Size(56, 13);
            this._Password_Label.TabIndex = 8;
            this._Password_Label.Text = "&Password:";
            // 
            // _Username_ComboBox
            // 
            this._Username_ComboBox.FormattingEnabled = true;
            this._Username_ComboBox.Location = new System.Drawing.Point(201, 173);
            this._Username_ComboBox.MaxLength = 255;
            this._Username_ComboBox.Name = "_Username_ComboBox";
            this._Username_ComboBox.Size = new System.Drawing.Size(309, 21);
            this._Username_ComboBox.TabIndex = 9;
            // 
            // _Password_TextBox
            // 
            this._Password_TextBox.Location = new System.Drawing.Point(201, 202);
            this._Password_TextBox.MaxLength = 255;
            this._Password_TextBox.Name = "_Password_TextBox";
            this._Password_TextBox.PasswordChar = '*';
            this._Password_TextBox.Size = new System.Drawing.Size(309, 20);
            this._Password_TextBox.TabIndex = 10;
            // 
            // _RememberPassword_CheckBox
            // 
            this._RememberPassword_CheckBox.AutoSize = true;
            this._RememberPassword_CheckBox.Location = new System.Drawing.Point(201, 238);
            this._RememberPassword_CheckBox.Name = "_RememberPassword_CheckBox";
            this._RememberPassword_CheckBox.Size = new System.Drawing.Size(125, 17);
            this._RememberPassword_CheckBox.TabIndex = 11;
            this._RememberPassword_CheckBox.Text = "Re&member password";
            this._RememberPassword_CheckBox.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::CodeGenerator.Properties.Resources.line;
            this.pictureBox2.Location = new System.Drawing.Point(15, 279);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(493, 2);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.pictureBox1.BackgroundImage = global::CodeGenerator.Properties.Resources.title;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(526, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // _Connect_Button
            // 
            this._Connect_Button.Location = new System.Drawing.Point(234, 289);
            this._Connect_Button.Name = "_Connect_Button";
            this._Connect_Button.Size = new System.Drawing.Size(93, 23);
            this._Connect_Button.TabIndex = 13;
            this._Connect_Button.Text = "&Connect";
            this._Connect_Button.UseVisualStyleBackColor = true;
            this._Connect_Button.Click += new System.EventHandler(this._Connect_Button_Click);
            // 
            // _Help_Button
            // 
            this._Help_Button.Location = new System.Drawing.Point(435, 289);
            this._Help_Button.Name = "_Help_Button";
            this._Help_Button.Size = new System.Drawing.Size(75, 23);
            this._Help_Button.TabIndex = 14;
            this._Help_Button.Text = "&Help";
            this._Help_Button.UseVisualStyleBackColor = true;
            this._Help_Button.Click += new System.EventHandler(this._Help_Button_Click);
            // 
            // _Cancel_Button
            // 
            this._Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Cancel_Button.Location = new System.Drawing.Point(337, 289);
            this._Cancel_Button.Name = "_Cancel_Button";
            this._Cancel_Button.Size = new System.Drawing.Size(88, 23);
            this._Cancel_Button.TabIndex = 15;
            this._Cancel_Button.Text = "Cancel";
            this._Cancel_Button.UseVisualStyleBackColor = true;
            this._Cancel_Button.Click += new System.EventHandler(this._Cancel_Button_Click);
            // 
            // FConnector
            // 
            this.AcceptButton = this._Connect_Button;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this._Cancel_Button;
            this.ClientSize = new System.Drawing.Size(526, 324);
            this.Controls.Add(this._Cancel_Button);
            this.Controls.Add(this._Help_Button);
            this.Controls.Add(this._Connect_Button);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this._RememberPassword_CheckBox);
            this.Controls.Add(this._Password_TextBox);
            this.Controls.Add(this._Username_ComboBox);
            this.Controls.Add(this._Password_Label);
            this.Controls.Add(this._Username_Label);
            this.Controls.Add(this._Authentication_ComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._ServerName_ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._ServerType_ComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FConnector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to server";
            this.Load += new System.EventHandler(this.FConnector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox _ServerType_ComboBox;
		private System.Windows.Forms.ComboBox _ServerName_ComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox _Authentication_ComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label _Username_Label;
		private System.Windows.Forms.Label _Password_Label;
		private System.Windows.Forms.ComboBox _Username_ComboBox;
		private System.Windows.Forms.TextBox _Password_TextBox;
		private System.Windows.Forms.CheckBox _RememberPassword_CheckBox;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button _Connect_Button;
		private System.Windows.Forms.Button _Help_Button;
		private System.Windows.Forms.Button _Cancel_Button;
	}
}