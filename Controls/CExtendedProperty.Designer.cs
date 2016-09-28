namespace CodeGenerator
{
    partial class CExtendedProperty
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
            this._Content_richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _Content_richTextBox
            // 
            this._Content_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Content_richTextBox.Location = new System.Drawing.Point(0, 0);
            this._Content_richTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._Content_richTextBox.Name = "_Content_richTextBox";
            this._Content_richTextBox.ReadOnly = true;
            this._Content_richTextBox.Size = new System.Drawing.Size(767, 406);
            this._Content_richTextBox.TabIndex = 2;
            this._Content_richTextBox.Text = "";
            // 
            // CExtendProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._Content_richTextBox);
            this.Name = "CExtendProperty";
            this.Size = new System.Drawing.Size(767, 406);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _Content_richTextBox;
    }
}
