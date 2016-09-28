using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;

namespace CodeGenerator
{
	public partial class FOutputCode : Form
	{
		RichTextBoxWriter _writer = null;

		public RichTextBoxWriter Writer
		{
			get { return _writer; }
			set { _writer = value; }
		}
		Database _db;

		public FOutputCode()
		{
			InitializeComponent();
			_writer = new RichTextBoxWriter(this._Result_RichTextBox);
		}

		public FOutputCode(KeyValuePair<string, string> code)
			: this()
		{
			this.Text = code.Key;
			_Result_RichTextBox.Text = code.Value;
			toolStripButton1.Visible = false;
		}

		public FOutputCode(string s)
			: this()
		{
			_Result_RichTextBox.Text = s;
			toolStripButton1.Visible = false;
		}

		public FOutputCode(Database db, string s)
			: this(s)
		{
			_db = db;
			toolStripButton1.Visible = true;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			using (new WaitCursor(this))
			{
				_db.ExecuteNonQuery(_Result_RichTextBox.Text);

				MessageBox.Show("Finished!");
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();
			Clipboard.SetText(_Result_RichTextBox.Text);
		}



		#region �����ط�����װ

		/// <summary>
		/// ���һ�β�ɫ���֣�ָ��ǰ��ɫ��
		/// </summary>
		public void Write(Color fc, string s, params object[] args)
		{
			_writer.Write(fc, s, args);
		}

		/// <summary>
		/// ���һ�β�ɫ���֣���Ĭ��ǰ��ɫ��
		/// </summary>
		public void Write(string s, params object[] args)
		{
			_writer.Write(s, args);
		}

		/// <summary>
		/// ���һ�β�ɫ���֣�ָ��ǰ��ɫ��ĩβ�����з���
		/// </summary>
		public void WriteLine(Color fc, string s, params object[] args)
		{
			_writer.WriteLine(fc, s, args);
		}
		/// <summary>
		/// ���һ�β�ɫ���֣���Ĭ��ǰ��ɫ��ĩβ�����з���
		/// </summary>
		public void WriteLine(string s, params object[] args)
		{
			_writer.WriteLine(s, args);
		}

		/// <summary>
		/// ���һ������
		/// </summary>
		public void WriteLine()
		{
			_writer.WriteLine();
		}

		/// <summary>
		/// ��� n ������
		/// </summary>
		/// <param name="numOfNewLine"></param>
		public void WriteLine(int n)
		{
			_writer.WriteLine(n);
		}

		/// <summary>
		/// ��� RichTextBox ����ʾ
		/// </summary>
		public void Clear()
		{
			_writer.Clear();
		}

		#endregion
	}
}