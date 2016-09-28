using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class FOutputCodes : Form
	{
		public FOutputCodes()
		{
			InitializeComponent();
		}
		public FOutputCodes(List<KeyValuePair<string, string>> codes)
			: this()
		{
			foreach (KeyValuePair<string, string> code in codes)
			{
				UC_CodeSegmentViewer viewer = new UC_CodeSegmentViewer(code.Key, code.Value);
				viewer.Height = 200;
				viewer.Width = _codes_flowLayoutPanel.Width - 24;
				_codes_flowLayoutPanel.Controls.Add(viewer);
				_codes_flowLayoutPanel.SizeChanged += delegate(object sender, EventArgs e)
				{
					viewer.Width = _codes_flowLayoutPanel.Width - 24;
				};
			}
		}

		private void FOutputCodes_Load(object sender, EventArgs e)
		{

		}
	}
}
