using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class UC_CodeSegmentViewer : UserControl
	{
		public UC_CodeSegmentViewer()
		{
			InitializeComponent();
		}

		public UC_CodeSegmentViewer(string title, string code) : this()
		{
			_title_toolStripLabel.Text = title;
			_code_richTextBox.Text = code;
		}

		public string Title
		{
			get { return _title_toolStripLabel.Text; }
		}

		public string Code
		{
			get { return _code_richTextBox.Text; }
		}

		private void _copy_toolStripButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(this.Code);
		}
	}
}
