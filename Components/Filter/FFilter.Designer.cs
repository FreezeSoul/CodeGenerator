namespace CodeGenerator
{
	partial class FFilter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFilter));
            this._ds = new CodeGenerator.Misc.DS();
            this.filtersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.filtersBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorTest = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveUp = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveDown = new System.Windows.Forms.ToolStripButton();
            this.filtersBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.filtersDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Schema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.typesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtersBindingNavigator)).BeginInit();
            this.filtersBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filtersDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // _ds
            // 
            this._ds.DataSetName = "DS";
            this._ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // filtersBindingSource
            // 
            this.filtersBindingSource.DataMember = "SchemesFilters";
            this.filtersBindingSource.DataSource = this._ds;
            this.filtersBindingSource.Filter = "SchemesID = 1";
            this.filtersBindingSource.Sort = "SortOrder";
            // 
            // filtersBindingNavigator
            // 
            this.filtersBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.filtersBindingNavigator.BindingSource = this.filtersBindingSource;
            this.filtersBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.filtersBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.filtersBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.bindingNavigatorTest,
            this.bindingNavigatorMoveUp,
            this.bindingNavigatorMoveDown,
            this.filtersBindingNavigatorSaveItem});
            this.filtersBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.filtersBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.filtersBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.filtersBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.filtersBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.filtersBindingNavigator.Name = "filtersBindingNavigator";
            this.filtersBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.filtersBindingNavigator.Size = new System.Drawing.Size(814, 25);
            this.filtersBindingNavigator.TabIndex = 0;
            this.filtersBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this.bindingNavigatorAddNewItem_Click);
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(39, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this.bindingNavigatorDeleteItem_Click);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorTest
            // 
            this.bindingNavigatorTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorTest.Image = global::CodeGenerator.Properties.Resources.FormRunHS;
            this.bindingNavigatorTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bindingNavigatorTest.Name = "bindingNavigatorTest";
            this.bindingNavigatorTest.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorTest.Text = "toolStripButton1";
            this.bindingNavigatorTest.ToolTipText = "Test & Watch Result";
            this.bindingNavigatorTest.Click += new System.EventHandler(this.bindingNavigatorTest_Click);
            // 
            // bindingNavigatorMoveUp
            // 
            this.bindingNavigatorMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveUp.Image = global::CodeGenerator.Properties.Resources.up;
            this.bindingNavigatorMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bindingNavigatorMoveUp.Name = "bindingNavigatorMoveUp";
            this.bindingNavigatorMoveUp.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveUp.Text = "toolStripButton1";
            this.bindingNavigatorMoveUp.ToolTipText = "Move UP";
            this.bindingNavigatorMoveUp.Click += new System.EventHandler(this.bindingNavigatorMoveUp_Click);
            // 
            // bindingNavigatorMoveDown
            // 
            this.bindingNavigatorMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveDown.Image = global::CodeGenerator.Properties.Resources.down;
            this.bindingNavigatorMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bindingNavigatorMoveDown.Name = "bindingNavigatorMoveDown";
            this.bindingNavigatorMoveDown.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveDown.Text = "toolStripButton2";
            this.bindingNavigatorMoveDown.ToolTipText = "Move DOWN";
            this.bindingNavigatorMoveDown.Click += new System.EventHandler(this.bindingNavigatorMoveDown_Click);
            // 
            // filtersBindingNavigatorSaveItem
            // 
            this.filtersBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filtersBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("filtersBindingNavigatorSaveItem.Image")));
            this.filtersBindingNavigatorSaveItem.Name = "filtersBindingNavigatorSaveItem";
            this.filtersBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.filtersBindingNavigatorSaveItem.Text = "Save Data";
            this.filtersBindingNavigatorSaveItem.Click += new System.EventHandler(this.filtersBindingNavigatorSaveItem_Click);
            // 
            // filtersDataGridView
            // 
            this.filtersDataGridView.AutoGenerateColumns = false;
            this.filtersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Schema,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.Memo});
            this.filtersDataGridView.DataSource = this.filtersBindingSource;
            this.filtersDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersDataGridView.Location = new System.Drawing.Point(0, 25);
            this.filtersDataGridView.MultiSelect = false;
            this.filtersDataGridView.Name = "filtersDataGridView";
            this.filtersDataGridView.RowTemplate.Height = 23;
            this.filtersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.filtersDataGridView.Size = new System.Drawing.Size(814, 407);
            this.filtersDataGridView.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "SortOrder";
            this.dataGridViewTextBoxColumn1.HeaderText = "SortOrder";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // Schema
            // 
            this.Schema.DataPropertyName = "Schema";
            this.Schema.HeaderText = "架构名";
            this.Schema.Name = "Schema";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TypeName";
            this.dataGridViewTextBoxColumn2.DataSource = this.typesBindingSource;
            this.dataGridViewTextBoxColumn2.DisplayMember = "TypeName";
            this.dataGridViewTextBoxColumn2.HeaderText = "数据库对象类型";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.ValueMember = "TypeName";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // typesBindingSource
            // 
            this.typesBindingSource.DataMember = "TypeNames";
            this.typesBindingSource.DataSource = this._ds;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsAllow";
            this.dataGridViewCheckBoxColumn1.HeaderText = "是否为允许";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 80;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FilterString";
            this.dataGridViewTextBoxColumn3.HeaderText = "过滤字串（正则）";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // Memo
            // 
            this.Memo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Memo.DataPropertyName = "Memo";
            this.Memo.HeaderText = "备注";
            this.Memo.MinimumWidth = 200;
            this.Memo.Name = "Memo";
            // 
            // FFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 432);
            this.Controls.Add(this.filtersDataGridView);
            this.Controls.Add(this.filtersBindingNavigator);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "过滤规则设定（从上到下，覆盖式生效）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FFilter_FormClosing);
            this.Load += new System.EventHandler(this.FFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this._ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtersBindingNavigator)).EndInit();
            this.filtersBindingNavigator.ResumeLayout(false);
            this.filtersBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filtersDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private CodeGenerator.Misc.DS _ds;
		private System.Windows.Forms.BindingSource filtersBindingSource;
		private System.Windows.Forms.BindingNavigator filtersBindingNavigator;
		private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
		private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
		private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
		private System.Windows.Forms.ToolStripButton filtersBindingNavigatorSaveItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveUp;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveDown;
		private System.Windows.Forms.DataGridView filtersDataGridView;
		private System.Windows.Forms.BindingSource typesBindingSource;
		private System.Windows.Forms.ToolStripButton bindingNavigatorTest;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Schema;
		private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Memo;
	}
}