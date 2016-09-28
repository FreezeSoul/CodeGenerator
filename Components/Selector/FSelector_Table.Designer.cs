namespace CodeGenerator.Components.Selector
{
    partial class FSelector_Table
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
            this._Cancel_button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._DataGridView = new System.Windows.Forms.DataGridView();
            this._Submit_button = new System.Windows.Forms.Button();
            this.Schema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _Cancel_button
            // 
            this._Cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._Cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Cancel_button.Location = new System.Drawing.Point(756, 421);
            this._Cancel_button.Name = "_Cancel_button";
            this._Cancel_button.Size = new System.Drawing.Size(75, 23);
            this._Cancel_button.TabIndex = 9;
            this._Cancel_button.Text = "&Cancel";
            this._Cancel_button.UseVisualStyleBackColor = true;
            this._Cancel_button.Click += new System.EventHandler(this._Cancel_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox1.Location = new System.Drawing.Point(13, 407);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(818, 1);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // _DataGridView
            // 
            this._DataGridView.AllowUserToAddRows = false;
            this._DataGridView.AllowUserToDeleteRows = false;
            this._DataGridView.AllowUserToOrderColumns = true;
            this._DataGridView.AllowUserToResizeRows = false;
            this._DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Schema,
            this._Name,
            this._Caption,
            this._Memo});
            this._DataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._DataGridView.Location = new System.Drawing.Point(12, 12);
            this._DataGridView.MultiSelect = false;
            this._DataGridView.Name = "_DataGridView";
            this._DataGridView.ReadOnly = true;
            this._DataGridView.RowHeadersWidth = 26;
            this._DataGridView.RowTemplate.Height = 23;
            this._DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._DataGridView.Size = new System.Drawing.Size(819, 382);
            this._DataGridView.TabIndex = 6;
            // 
            // _Submit_button
            // 
            this._Submit_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._Submit_button.Location = new System.Drawing.Point(661, 421);
            this._Submit_button.Name = "_Submit_button";
            this._Submit_button.Size = new System.Drawing.Size(75, 23);
            this._Submit_button.TabIndex = 8;
            this._Submit_button.Text = "&Next";
            this._Submit_button.UseVisualStyleBackColor = true;
            this._Submit_button.Click += new System.EventHandler(this._Submit_button_Click);
            // 
            // Schema
            // 
            this.Schema.HeaderText = "Schema";
            this.Schema.MinimumWidth = 100;
            this.Schema.Name = "Schema";
            this.Schema.ReadOnly = true;
            // 
            // _Name
            // 
            this._Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._Name.HeaderText = "TableName";
            this._Name.MinimumWidth = 155;
            this._Name.Name = "_Name";
            this._Name.ReadOnly = true;
            this._Name.Width = 155;
            // 
            // _Caption
            // 
            this._Caption.HeaderText = "DisplayName";
            this._Caption.MinimumWidth = 100;
            this._Caption.Name = "_Caption";
            this._Caption.ReadOnly = true;
            // 
            // _Memo
            // 
            this._Memo.HeaderText = "Discription";
            this._Memo.MinimumWidth = 255;
            this._Memo.Name = "_Memo";
            this._Memo.ReadOnly = true;
            // 
            // FSelector_Table
            // 
            this.AcceptButton = this._Submit_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._Cancel_button;
            this.ClientSize = new System.Drawing.Size(843, 456);
            this.Controls.Add(this._Cancel_button);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._DataGridView);
            this.Controls.Add(this._Submit_button);
            this.Name = "FSelector_Table";
            this.Text = "表选择器";
            this.Load += new System.EventHandler(this.FSelector_Table_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _Cancel_button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView _DataGridView;
        private System.Windows.Forms.Button _Submit_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn Schema;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Caption;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Memo;
    }
}