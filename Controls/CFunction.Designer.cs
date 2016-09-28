namespace CodeGenerator
{
	partial class CFunction
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
            this._Parms_dataGridView = new System.Windows.Forms.DataGridView();
            this.parmsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dS = new CodeGenerator.Misc.DS();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._Result_dataGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this._MethodName_textBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this._Extract_button = new System.Windows.Forms.Button();
            this._Desc_richTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._SPHead_richTextBox = new System.Windows.Forms.RichTextBox();
            this._SPBody_richTextBox = new System.Windows.Forms.RichTextBox();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.defaultValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTypeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precisionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scaleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._PK = new System.Windows.Forms.DataGridViewImageColumn();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._AllowNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._IsUnique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._IsComputed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._Parms_dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parmsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._Result_dataGridView)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Function Name:";
            // 
            // _SPName_TextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._SPName_TextBox, 2);
            this._SPName_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPName_TextBox.Location = new System.Drawing.Point(103, 3);
            this._SPName_TextBox.Name = "_SPName_TextBox";
            this._SPName_TextBox.ReadOnly = true;
            this._SPName_TextBox.Size = new System.Drawing.Size(543, 20);
            this._SPName_TextBox.TabIndex = 2;
            // 
            // _Parms_dataGridView
            // 
            this._Parms_dataGridView.AllowUserToAddRows = false;
            this._Parms_dataGridView.AllowUserToDeleteRows = false;
            this._Parms_dataGridView.AllowUserToResizeRows = false;
            this._Parms_dataGridView.AutoGenerateColumns = false;
            this._Parms_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._Parms_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.directionDataGridViewTextBoxColumn,
            this.defaultValueDataGridViewTextBoxColumn,
            this.descDataGridViewTextBoxColumn,
            this.dataTypeNameDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.precisionDataGridViewTextBoxColumn,
            this.scaleDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this._Parms_dataGridView, 3);
            this._Parms_dataGridView.DataSource = this.parmsBindingSource;
            this._Parms_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Parms_dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._Parms_dataGridView.Location = new System.Drawing.Point(3, 128);
            this._Parms_dataGridView.Name = "_Parms_dataGridView";
            this._Parms_dataGridView.RowTemplate.Height = 23;
            this._Parms_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._Parms_dataGridView.Size = new System.Drawing.Size(643, 83);
            this._Parms_dataGridView.TabIndex = 2;
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
            this.splitContainer1.Size = new System.Drawing.Size(649, 491);
            this.splitContainer1.SplitterDistance = 303;
            this.splitContainer1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this._Result_dataGridView, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._Parms_dataGridView, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._SPName_TextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._MethodName_textBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._Desc_richTextBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(649, 303);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // _Result_dataGridView
            // 
            this._Result_dataGridView.AllowUserToAddRows = false;
            this._Result_dataGridView.AllowUserToDeleteRows = false;
            this._Result_dataGridView.AllowUserToOrderColumns = true;
            this._Result_dataGridView.AllowUserToResizeRows = false;
            this._Result_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._Result_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._Result_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._PK,
            this._Name,
            this._Caption,
            this._Memo,
            this._DataType,
            this._Length,
            this._AllowNull,
            this._IsUnique,
            this._IsComputed});
            this.tableLayoutPanel1.SetColumnSpan(this._Result_dataGridView, 3);
            this._Result_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Result_dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._Result_dataGridView.Location = new System.Drawing.Point(3, 217);
            this._Result_dataGridView.MultiSelect = false;
            this._Result_dataGridView.Name = "_Result_dataGridView";
            this._Result_dataGridView.RowHeadersWidth = 26;
            this._Result_dataGridView.RowTemplate.Height = 23;
            this._Result_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._Result_dataGridView.Size = new System.Drawing.Size(643, 83);
            this._Result_dataGridView.TabIndex = 8;
            this._Result_dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._Result_dataGridView_CellValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Method Name:";
            // 
            // _MethodName_textBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._MethodName_textBox, 2);
            this._MethodName_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._MethodName_textBox.Location = new System.Drawing.Point(103, 98);
            this._MethodName_textBox.Name = "_MethodName_textBox";
            this._MethodName_textBox.Size = new System.Drawing.Size(543, 20);
            this._MethodName_textBox.TabIndex = 7;
            this._MethodName_textBox.TextChanged += new System.EventHandler(this._MethodName_textBox_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this._Extract_button);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 33);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(94, 59);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description:";
            // 
            // _Extract_button
            // 
            this._Extract_button.Location = new System.Drawing.Point(3, 25);
            this._Extract_button.Name = "_Extract_button";
            this._Extract_button.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Extract_button.Size = new System.Drawing.Size(59, 25);
            this._Extract_button.TabIndex = 5;
            this._Extract_button.Text = "&Get";
            this._Extract_button.UseVisualStyleBackColor = true;
            this._Extract_button.Click += new System.EventHandler(this._Extract_button_Click);
            // 
            // _Desc_richTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._Desc_richTextBox, 2);
            this._Desc_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Desc_richTextBox.Location = new System.Drawing.Point(103, 33);
            this._Desc_richTextBox.Name = "_Desc_richTextBox";
            this._Desc_richTextBox.Size = new System.Drawing.Size(543, 59);
            this._Desc_richTextBox.TabIndex = 10;
            this._Desc_richTextBox.Text = "";
            this._Desc_richTextBox.TextChanged += new System.EventHandler(this._Memo_textBox_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer2.Size = new System.Drawing.Size(649, 184);
            this.splitContainer2.SplitterDistance = 64;
            this.splitContainer2.TabIndex = 1;
            // 
            // _SPHead_richTextBox
            // 
            this._SPHead_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPHead_richTextBox.Location = new System.Drawing.Point(0, 0);
            this._SPHead_richTextBox.Name = "_SPHead_richTextBox";
            this._SPHead_richTextBox.ReadOnly = true;
            this._SPHead_richTextBox.Size = new System.Drawing.Size(649, 64);
            this._SPHead_richTextBox.TabIndex = 0;
            this._SPHead_richTextBox.Text = "";
            // 
            // _SPBody_richTextBox
            // 
            this._SPBody_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._SPBody_richTextBox.Location = new System.Drawing.Point(0, 0);
            this._SPBody_richTextBox.Name = "_SPBody_richTextBox";
            this._SPBody_richTextBox.ReadOnly = true;
            this._SPBody_richTextBox.Size = new System.Drawing.Size(649, 116);
            this._SPBody_richTextBox.TabIndex = 0;
            this._SPBody_richTextBox.Text = "";
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
            this.descDataGridViewTextBoxColumn.HeaderText = "Description(changeable)";
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
            // _PK
            // 
            this._PK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._PK.Frozen = true;
            this._PK.HeaderText = "";
            this._PK.MinimumWidth = 25;
            this._PK.Name = "_PK";
            this._PK.ReadOnly = true;
            this._PK.Width = 25;
            // 
            // _Name
            // 
            this._Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._Name.Frozen = true;
            this._Name.HeaderText = "FieldName";
            this._Name.MinimumWidth = 155;
            this._Name.Name = "_Name";
            this._Name.ReadOnly = true;
            this._Name.Width = 155;
            // 
            // _Caption
            // 
            this._Caption.HeaderText = "DisplayName(Changeable)";
            this._Caption.MinimumWidth = 100;
            this._Caption.Name = "_Caption";
            // 
            // _Memo
            // 
            this._Memo.HeaderText = "Description(Changeable)";
            this._Memo.MinimumWidth = 255;
            this._Memo.Name = "_Memo";
            // 
            // _DataType
            // 
            this._DataType.HeaderText = "DataType";
            this._DataType.MinimumWidth = 100;
            this._DataType.Name = "_DataType";
            this._DataType.ReadOnly = true;
            // 
            // _Length
            // 
            this._Length.HeaderText = "L";
            this._Length.MinimumWidth = 30;
            this._Length.Name = "_Length";
            this._Length.ReadOnly = true;
            this._Length.ToolTipText = "字段长度";
            // 
            // _AllowNull
            // 
            this._AllowNull.HeaderText = "E";
            this._AllowNull.MinimumWidth = 20;
            this._AllowNull.Name = "_AllowNull";
            this._AllowNull.ReadOnly = true;
            this._AllowNull.ToolTipText = "是否可空";
            // 
            // _IsUnique
            // 
            this._IsUnique.HeaderText = "U";
            this._IsUnique.Name = "_IsUnique";
            this._IsUnique.ReadOnly = true;
            this._IsUnique.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._IsUnique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._IsUnique.ToolTipText = "是否属于某唯一键";
            // 
            // _IsComputed
            // 
            this._IsComputed.HeaderText = "C";
            this._IsComputed.MinimumWidth = 20;
            this._IsComputed.Name = "_IsComputed";
            this._IsComputed.ReadOnly = true;
            this._IsComputed.ToolTipText = "是否为计算列";
            // 
            // CFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CFunction";
            this.Size = new System.Drawing.Size(649, 491);
            this.Load += new System.EventHandler(this.CFunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this._Parms_dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parmsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._Result_dataGridView)).EndInit();
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
		private System.Windows.Forms.DataGridView _Parms_dataGridView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.BindingSource parmsBindingSource;
        private CodeGenerator.Misc.DS dS;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _MethodName_textBox;
        private System.Windows.Forms.DataGridView _Result_dataGridView;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _Extract_button;
		private System.Windows.Forms.RichTextBox _SPHead_richTextBox;
		private System.Windows.Forms.RichTextBox _SPBody_richTextBox;
        private System.Windows.Forms.RichTextBox _Desc_richTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn directionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataTypeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precisionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn _PK;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Caption;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Memo;
        private System.Windows.Forms.DataGridViewTextBoxColumn _DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Length;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _AllowNull;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _IsUnique;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _IsComputed;
	}
}
