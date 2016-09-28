using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class FOutput2 : Form
	{
		public FOutput2()
		{
			InitializeComponent();
		}

		public FOutput2(string s1, string s2) : this()
		{
			_richTextBox1.Text = s1;
			_richTextBox2.Text = s2;
		}

		private void _copy_toolStripButton1_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();
			Clipboard.SetText(_richTextBox1.Text);
		}

		private void _copy_toolStripButton2_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();
			Clipboard.SetText(_richTextBox2.Text);
		}
	}
}