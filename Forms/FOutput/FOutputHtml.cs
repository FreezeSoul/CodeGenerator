using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class FOutputHtml : Form
	{
		public FOutputHtml()
		{
			InitializeComponent();
		}

		public FOutputHtml(string title)
			: this()
		{
			this.Text = title;
		}

		public FOutputHtml(string title, string filename)
			: this(title)
		{
			if (!string.IsNullOrEmpty(filename))
			{
				string path = System.IO.Path.Combine(Application.StartupPath, filename);
				_webBrowser.Url = new Uri("file://" + path);
			}
		}

		public FOutputHtml(string title, string filename, int width, int height)
			: this(title, filename)
		{
			this.Width = width;
			this.Height = height;
		}

		public FOutputHtml(string title, Uri uri, int width, int height)
			: this(title, "", width, height)
		{
			_webBrowser.Url = uri;
		}
	}
}
