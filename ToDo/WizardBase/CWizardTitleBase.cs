using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class CWizardTitleBase : UserControl
	{
		public CWizardTitleBase()
		{
			InitializeComponent();
		}

		public CWizardTitleBase(string title, string intro)
			: this()
		{
			_Title_label.Text = title;
			_Intro_label.Text = intro;
		}


		//private void CWizardTitleBase_Load(object sender, EventArgs e)
		//{
		//    _Title_label = _title;
		//    _Intro_label = _intro;
		//}

		//private string _title;
		//private string _intro;


		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			get { return _Title_label.Text; }
			set { _Title_label.Text = value; }
		}

		/// <summary>
		/// 简介
		/// </summary>
		public string Intro
		{
			get { return _Intro_label.Text; }
			set { _Intro_label.Text = value; }
		}


	}
}
