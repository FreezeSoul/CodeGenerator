namespace CodeGenerator
{
	partial class CTable
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._TableName_TextBox = new System.Windows.Forms.TextBox();
            this._Scheme_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._CreateTime_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._DataGridView = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this._Desc_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._Caption_TextBox = new System.Windows.Forms.TextBox();
            this._PK = new System.Windows.Forms.DataGridViewImageColumn();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._AllowNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._IsUnique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._Display = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._TableName_TextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._Scheme_TextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._CreateTime_TextBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._DataGridView, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._Desc_TextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._Caption_TextBox, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(968, 328);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "TableName:";
            // 
            // _TableName_TextBox
            // 
            this._TableName_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TableName_TextBox.Location = new System.Drawing.Point(103, 3);
            this._TableName_TextBox.Name = "_TableName_TextBox";
            this._TableName_TextBox.ReadOnly = true;
            this._TableName_TextBox.Size = new System.Drawing.Size(378, 21);
            this._TableName_TextBox.TabIndex = 1;
            // 
            // _Scheme_TextBox
            // 
            this._Scheme_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Scheme_TextBox.Location = new System.Drawing.Point(103, 30);
            this._Scheme_TextBox.Name = "_Scheme_TextBox";
            this._Scheme_TextBox.ReadOnly = true;
            this._Scheme_TextBox.Size = new System.Drawing.Size(378, 21);
            this._Scheme_TextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Schema:";
            // 
            // _CreateTime_TextBox
            // 
            this._CreateTime_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._CreateTime_TextBox.Location = new System.Drawing.Point(587, 30);
            this._CreateTime_TextBox.Name = "_CreateTime_TextBox";
            this._CreateTime_TextBox.ReadOnly = true;
            this._CreateTime_TextBox.Size = new System.Drawing.Size(378, 21);
            this._CreateTime_TextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(487, 35);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "CreateTime:";
            // 
            // _DataGridView
            // 
            this._DataGridView.AllowUserToAddRows = false;
            this._DataGridView.AllowUserToDeleteRows = false;
            this._DataGridView.AllowUserToOrderColumns = true;
            this._DataGridView.AllowUserToResizeRows = false;
            this._DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._PK,
            this._Name,
            this._Caption,
            this._Memo,
            this._DataType,
            this._Length,
            this._AllowNull,
            this._IsUnique,
            this._Display});
            this.tableLayoutPanel1.SetColumnSpan(this._DataGridView, 4);
            this._DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._DataGridView.ImeMode = System.Windows.Forms.ImeMode.On;
            this._DataGridView.Location = new System.Drawing.Point(3, 84);
            this._DataGridView.MultiSelect = false;
            this._DataGridView.Name = "_DataGridView";
            this._DataGridView.RowHeadersWidth = 26;
            this._DataGridView.RowTemplate.Height = 23;
            this._DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._DataGridView.Size = new System.Drawing.Size(962, 241);
            this._DataGridView.TabIndex = 2;
            this._DataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._DataGridView_CellValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 62);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Description:";
            // 
            // _Desc_TextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._Desc_TextBox, 3);
            this._Desc_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Desc_TextBox.Location = new System.Drawing.Point(103, 57);
            this._Desc_TextBox.Name = "_Desc_TextBox";
            this._Desc_TextBox.Size = new System.Drawing.Size(862, 21);
            this._Desc_TextBox.TabIndex = 3;
            this._Desc_TextBox.TextChanged += new System.EventHandler(this._Desc_TextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(487, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "DisplayName:";
            // 
            // _Caption_TextBox
            // 
            this._Caption_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Caption_TextBox.Location = new System.Drawing.Point(587, 3);
            this._Caption_TextBox.Name = "_Caption_TextBox";
            this._Caption_TextBox.Size = new System.Drawing.Size(378, 21);
            this._Caption_TextBox.TabIndex = 4;
            this._Caption_TextBox.TextChanged += new System.EventHandler(this._Caption_TextBox_TextChanged);
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
            this._IsUnique.MinimumWidth = 20;
            this._IsUnique.Name = "_IsUnique";
            this._IsUnique.ReadOnly = true;
            this._IsUnique.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._IsUnique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._IsUnique.ToolTipText = "是否属于某唯一键";
            // 
            // _Display
            // 
            this._Display.HeaderText = "D(Changeable)";
            this._Display.MinimumWidth = 120;
            this._Display.Name = "_Display";
            this._Display.ToolTipText = "是否显示";
            // 
            // CTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CTable";
            this.Size = new System.Drawing.Size(968, 328);
            this.Load += new System.EventHandler(this.CTable_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _TableName_TextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox _Scheme_TextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _CreateTime_TextBox;
		private System.Windows.Forms.DataGridView _DataGridView;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox _Desc_TextBox;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _Caption_TextBox;
        private System.Windows.Forms.DataGridViewImageColumn _PK;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Caption;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Memo;
        private System.Windows.Forms.DataGridViewTextBoxColumn _DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Length;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _AllowNull;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _IsUnique;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _Display;
	}
}
