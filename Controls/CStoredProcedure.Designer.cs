namespace CodeGenerator
{
	partial class CStoredProcedure
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this._SPName_TextBox = new System.Windows.Forms.TextBox();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.defaultValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTypeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precisionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scaleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parmsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dS = new CodeGenerator.Misc.DS();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this._ResultType_comboBox = new System.Windows.Forms.ComboBox();
            this._SingleLine_checkBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this._MethodName_textBox = new System.Windows.Forms.TextBox();
            this._Behavior_comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this._Extract_button = new System.Windows.Forms.Button();
            this._Desc_richTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._SPHead_richTextBox = new System.Windows.Forms.RichTextBox();
            this._SPBody_richTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parmsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "StoredProcName:";
            // 
            // _SPName_TextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._SPName_TextBox, 3);
            this._SPName_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPName_TextBox.Location = new System.Drawing.Point(103, 4);
            this._SPName_TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._SPName_TextBox.Name = "_SPName_TextBox";
            this._SPName_TextBox.ReadOnly = true;
            this._SPName_TextBox.Size = new System.Drawing.Size(543, 20);
            this._SPName_TextBox.TabIndex = 2;
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.AllowUserToResizeRows = false;
            this._dataGridView.AutoGenerateColumns = false;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.directionDataGridViewTextBoxColumn,
            this.defaultValueDataGridViewTextBoxColumn,
            this.descDataGridViewTextBoxColumn,
            this.dataTypeNameDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.precisionDataGridViewTextBoxColumn,
            this.scaleDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this._dataGridView, 4);
            this._dataGridView.DataSource = this.parmsBindingSource;
            this._dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._dataGridView.Location = new System.Drawing.Point(3, 162);
            this._dataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.RowTemplate.Height = 23;
            this._dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dataGridView.Size = new System.Drawing.Size(643, 136);
            this._dataGridView.TabIndex = 2;
            this._dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._dataGridView_CellValueChanged);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.Frozen = true;
            this.nameDataGridViewTextBoxColumn.HeaderText = "ParameterName";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // directionDataGridViewTextBoxColumn
            // 
            this.directionDataGridViewTextBoxColumn.DataPropertyName = "IsOutput";
            this.directionDataGridViewTextBoxColumn.FillWeight = 70F;
            this.directionDataGridViewTextBoxColumn.HeaderText = "IsOutput";
            this.directionDataGridViewTextBoxColumn.Name = "directionDataGridViewTextBoxColumn";
            this.directionDataGridViewTextBoxColumn.ReadOnly = true;
            this.directionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.directionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.directionDataGridViewTextBoxColumn.Width = 70;
            // 
            // defaultValueDataGridViewTextBoxColumn
            // 
            this.defaultValueDataGridViewTextBoxColumn.DataPropertyName = "DefaultValue";
            this.defaultValueDataGridViewTextBoxColumn.HeaderText = "DefaultValue";
            this.defaultValueDataGridViewTextBoxColumn.Name = "defaultValueDataGridViewTextBoxColumn";
            this.defaultValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descDataGridViewTextBoxColumn
            // 
            this.descDataGridViewTextBoxColumn.DataPropertyName = "Desc";
            this.descDataGridViewTextBoxColumn.FillWeight = 200F;
            this.descDataGridViewTextBoxColumn.HeaderText = "Description(Changeable)";
            this.descDataGridViewTextBoxColumn.Name = "descDataGridViewTextBoxColumn";
            this.descDataGridViewTextBoxColumn.Width = 200;
            // 
            // dataTypeNameDataGridViewTextBoxColumn
            // 
            this.dataTypeNameDataGridViewTextBoxColumn.DataPropertyName = "DataTypeName";
            this.dataTypeNameDataGridViewTextBoxColumn.FillWeight = 90F;
            this.dataTypeNameDataGridViewTextBoxColumn.HeaderText = "DataType";
            this.dataTypeNameDataGridViewTextBoxColumn.Name = "dataTypeNameDataGridViewTextBoxColumn";
            this.dataTypeNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.dataTypeNameDataGridViewTextBoxColumn.Width = 90;
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            this.lengthDataGridViewTextBoxColumn.FillWeight = 40F;
            this.lengthDataGridViewTextBoxColumn.HeaderText = "Length";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            this.lengthDataGridViewTextBoxColumn.Width = 40;
            // 
            // precisionDataGridViewTextBoxColumn
            // 
            this.precisionDataGridViewTextBoxColumn.DataPropertyName = "Precision";
            this.precisionDataGridViewTextBoxColumn.FillWeight = 40F;
            this.precisionDataGridViewTextBoxColumn.HeaderText = "Precision";
            this.precisionDataGridViewTextBoxColumn.Name = "precisionDataGridViewTextBoxColumn";
            this.precisionDataGridViewTextBoxColumn.ReadOnly = true;
            this.precisionDataGridViewTextBoxColumn.Width = 40;
            // 
            // scaleDataGridViewTextBoxColumn
            // 
            this.scaleDataGridViewTextBoxColumn.DataPropertyName = "Scale";
            this.scaleDataGridViewTextBoxColumn.FillWeight = 40F;
            this.scaleDataGridViewTextBoxColumn.HeaderText = "Scale";
            this.scaleDataGridViewTextBoxColumn.Name = "scaleDataGridViewTextBoxColumn";
            this.scaleDataGridViewTextBoxColumn.ReadOnly = true;
            this.scaleDataGridViewTextBoxColumn.Width = 40;
            // 
            // parmsBindingSource
            // 
            this.parmsBindingSource.DataMember = "Parms";
            this.parmsBindingSource.DataSource = this.dS;
            // 
            // dS
            // 
            this.dS.DataSetName = "DS";
            this.dS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(649, 490);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.Controls.Add(this._dataGridView, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._SPName_TextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._ResultType_comboBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._SingleLine_checkBox, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._MethodName_textBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._Behavior_comboBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._Desc_richTextBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(649, 302);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 135);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "ResultDataType:";
            // 
            // _ResultType_comboBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._ResultType_comboBox, 2);
            this._ResultType_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ResultType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ResultType_comboBox.FormattingEnabled = true;
            this._ResultType_comboBox.Location = new System.Drawing.Point(103, 131);
            this._ResultType_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._ResultType_comboBox.Name = "_ResultType_comboBox";
            this._ResultType_comboBox.Size = new System.Drawing.Size(482, 21);
            this._ResultType_comboBox.TabIndex = 6;
            // 
            // _SingleLine_checkBox
            // 
            this._SingleLine_checkBox.AutoSize = true;
            this._SingleLine_checkBox.Dock = System.Windows.Forms.DockStyle.Right;
            this._SingleLine_checkBox.Enabled = false;
            this._SingleLine_checkBox.Location = new System.Drawing.Point(598, 131);
            this._SingleLine_checkBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._SingleLine_checkBox.Name = "_SingleLine_checkBox";
            this._SingleLine_checkBox.Size = new System.Drawing.Size(48, 23);
            this._SingleLine_checkBox.TabIndex = 8;
            this._SingleLine_checkBox.Text = "Row";
            this._SingleLine_checkBox.UseVisualStyleBackColor = true;
            this._SingleLine_checkBox.CheckedChanged += new System.EventHandler(this._SingleLine_checkBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Method Name:";
            // 
            // _MethodName_textBox
            // 
            this._MethodName_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._MethodName_textBox.Location = new System.Drawing.Point(103, 100);
            this._MethodName_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._MethodName_textBox.Name = "_MethodName_textBox";
            this._MethodName_textBox.Size = new System.Drawing.Size(238, 20);
            this._MethodName_textBox.TabIndex = 7;
            this._MethodName_textBox.TextChanged += new System.EventHandler(this._MethodName_textBox_TextChanged);
            // 
            // _Behavior_comboBox
            // 
            this._Behavior_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Behavior_comboBox.FormattingEnabled = true;
            this._Behavior_comboBox.Location = new System.Drawing.Point(347, 100);
            this._Behavior_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._Behavior_comboBox.Name = "_Behavior_comboBox";
            this._Behavior_comboBox.Size = new System.Drawing.Size(238, 21);
            this._Behavior_comboBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(594, 104);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = ":Behavior";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this._Extract_button);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 35);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(94, 57);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description:";
            // 
            // _Extract_button
            // 
            this._Extract_button.Location = new System.Drawing.Point(3, 25);
            this._Extract_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._Extract_button.Name = "_Extract_button";
            this._Extract_button.Size = new System.Drawing.Size(63, 25);
            this._Extract_button.TabIndex = 3;
            this._Extract_button.Text = "&Get";
            this._Extract_button.UseVisualStyleBackColor = true;
            this._Extract_button.Click += new System.EventHandler(this._Extract_button_Click);
            // 
            // _Desc_richTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._Desc_richTextBox, 3);
            this._Desc_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Desc_richTextBox.Location = new System.Drawing.Point(103, 35);
            this._Desc_richTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._Desc_richTextBox.Name = "_Desc_richTextBox";
            this._Desc_richTextBox.Size = new System.Drawing.Size(543, 57);
            this._Desc_richTextBox.TabIndex = 13;
            this._Desc_richTextBox.Text = "";
            this._Desc_richTextBox.TextChanged += new System.EventHandler(this._Memo_textBox_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._SPHead_richTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._SPBody_richTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(649, 183);
            this.splitContainer2.SplitterDistance = 63;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 1;
            // 
            // _SPHead_richTextBox
            // 
            this._SPHead_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPHead_richTextBox.Location = new System.Drawing.Point(0, 0);
            this._SPHead_richTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._SPHead_richTextBox.Name = "_SPHead_richTextBox";
            this._SPHead_richTextBox.ReadOnly = true;
            this._SPHead_richTextBox.Size = new System.Drawing.Size(649, 63);
            this._SPHead_richTextBox.TabIndex = 0;
            this._SPHead_richTextBox.Text = "";
            // 
            // _SPBody_richTextBox
            // 
            this._SPBody_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPBody_richTextBox.Location = new System.Drawing.Point(0, 0);
            this._SPBody_richTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._SPBody_richTextBox.Name = "_SPBody_richTextBox";
            this._SPBody_richTextBox.ReadOnly = true;
            this._SPBody_richTextBox.Size = new System.Drawing.Size(649, 115);
            this._SPBody_richTextBox.TabIndex = 1;
            this._SPBody_richTextBox.Text = "";
            // 
            // CStoredProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CStoredProcedure";
            this.Size = new System.Drawing.Size(649, 490);
            this.Load += new System.EventHandler(this.CStoredProcedure_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parmsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _SPName_TextBox;
		private System.Windows.Forms.DataGridView _dataGridView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.BindingSource parmsBindingSource;
        private CodeGenerator.Misc.DS dS;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox _ResultType_comboBox;
		private System.Windows.Forms.TextBox _MethodName_textBox;
        private System.Windows.Forms.CheckBox _SingleLine_checkBox;
		private System.Windows.Forms.ComboBox _Behavior_comboBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _Extract_button;
		private System.Windows.Forms.RichTextBox _SPBody_richTextBox;
		private System.Windows.Forms.RichTextBox _SPHead_richTextBox;
		private System.Windows.Forms.RichTextBox _Desc_richTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn directionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataTypeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precisionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleDataGridViewTextBoxColumn;
	}
}
