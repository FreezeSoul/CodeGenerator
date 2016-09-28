namespace CodeGenerator
{
	partial class CView
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
            this.label1 = new System.Windows.Forms.Label();
            this._ViewName_TextBox = new System.Windows.Forms.TextBox();
            this._Scheme_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._CreateTime_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._DataGridView = new System.Windows.Forms.DataGridView();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._IsPrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._AllowNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this._Desc_TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._Caption_TextBox = new System.Windows.Forms.TextBox();
            this._BaseTable_comboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ViewName:";
            // 
            // _ViewName_TextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._ViewName_TextBox, 3);
            this._ViewName_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ViewName_TextBox.Location = new System.Drawing.Point(103, 3);
            this._ViewName_TextBox.Name = "_ViewName_TextBox";
            this._ViewName_TextBox.ReadOnly = true;
            this._ViewName_TextBox.Size = new System.Drawing.Size(470, 21);
            this._ViewName_TextBox.TabIndex = 1;
            // 
            // _Scheme_TextBox
            // 
            this._Scheme_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Scheme_TextBox.Location = new System.Drawing.Point(103, 30);
            this._Scheme_TextBox.Name = "_Scheme_TextBox";
            this._Scheme_TextBox.ReadOnly = true;
            this._Scheme_TextBox.Size = new System.Drawing.Size(182, 21);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._ViewName_TextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._Scheme_TextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._CreateTime_TextBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._DataGridView, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._Desc_TextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this._Caption_TextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._BaseTable_comboBox, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(576, 406);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _CreateTime_TextBox
            // 
            this._CreateTime_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._CreateTime_TextBox.Location = new System.Drawing.Point(391, 30);
            this._CreateTime_TextBox.Name = "_CreateTime_TextBox";
            this._CreateTime_TextBox.ReadOnly = true;
            this._CreateTime_TextBox.Size = new System.Drawing.Size(182, 21);
            this._CreateTime_TextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(291, 35);
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
            this._Name,
            this._IsPrimaryKey,
            this._Caption,
            this._Memo,
            this._DataType,
            this._Length,
            this._AllowNull});
            this.tableLayoutPanel1.SetColumnSpan(this._DataGridView, 4);
            this._DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._DataGridView.Location = new System.Drawing.Point(3, 111);
            this._DataGridView.MultiSelect = false;
            this._DataGridView.Name = "_DataGridView";
            this._DataGridView.RowHeadersWidth = 26;
            this._DataGridView.RowTemplate.Height = 23;
            this._DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._DataGridView.Size = new System.Drawing.Size(570, 292);
            this._DataGridView.TabIndex = 2;
            this._DataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._DataGridView_CellValueChanged);
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
            // _IsPrimaryKey
            // 
            this._IsPrimaryKey.HeaderText = "PrimaryKey(Changeable)";
            this._IsPrimaryKey.MinimumWidth = 100;
            this._IsPrimaryKey.Name = "_IsPrimaryKey";
            this._IsPrimaryKey.ToolTipText = "是否属于主键";
            // 
            // _Caption
            // 
            this._Caption.HeaderText = "DisplayName(Changeable)";
            this._Caption.MinimumWidth = 100;
            this._Caption.Name = "_Caption";
            // 
            // _Memo
            // 
            this._Memo.HeaderText = "Discription(Changeable)";
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
            this._Length.ToolTipText = "数据最大长度";
            // 
            // _AllowNull
            // 
            this._AllowNull.HeaderText = "E";
            this._AllowNull.MinimumWidth = 20;
            this._AllowNull.Name = "_AllowNull";
            this._AllowNull.ReadOnly = true;
            this._AllowNull.ToolTipText = "是否可空";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 89);
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
            this._Desc_TextBox.Location = new System.Drawing.Point(103, 84);
            this._Desc_TextBox.Name = "_Desc_TextBox";
            this._Desc_TextBox.Size = new System.Drawing.Size(470, 21);
            this._Desc_TextBox.TabIndex = 3;
            this._Desc_TextBox.TextChanged += new System.EventHandler(this._Desc_TextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 62);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "DisplayName:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(291, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "BaseTable:";
            // 
            // _Caption_TextBox
            // 
            this._Caption_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Caption_TextBox.Location = new System.Drawing.Point(103, 57);
            this._Caption_TextBox.Name = "_Caption_TextBox";
            this._Caption_TextBox.Size = new System.Drawing.Size(182, 21);
            this._Caption_TextBox.TabIndex = 5;
            this._Caption_TextBox.TextChanged += new System.EventHandler(this._Caption_TextBox_TextChanged);
            // 
            // _BaseTable_comboBox
            // 
            this._BaseTable_comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._BaseTable_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._BaseTable_comboBox.FormattingEnabled = true;
            this._BaseTable_comboBox.Location = new System.Drawing.Point(391, 57);
            this._BaseTable_comboBox.Name = "_BaseTable_comboBox";
            this._BaseTable_comboBox.Size = new System.Drawing.Size(182, 20);
            this._BaseTable_comboBox.TabIndex = 4;
            // 
            // CView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CView";
            this.Size = new System.Drawing.Size(576, 406);
            this.Load += new System.EventHandler(this.CView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _ViewName_TextBox;
		private System.Windows.Forms.TextBox _Scheme_TextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox _CreateTime_TextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridView _DataGridView;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox _Desc_TextBox;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _BaseTable_comboBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox _Caption_TextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _IsPrimaryKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Caption;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Memo;
        private System.Windows.Forms.DataGridViewTextBoxColumn _DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Length;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _AllowNull;
	}
}
