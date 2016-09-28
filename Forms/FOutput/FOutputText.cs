using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class FOutputText : Form
	{
		RichTextBoxWriter _writer = null;

		public RichTextBoxWriter Writer
		{
			get { return _writer; }
			set { _writer = value; }
		}

		public FOutputText()
		{
			InitializeComponent();

			_writer = new RichTextBoxWriter(this._richTextBox);
		}

		public FOutputText(string title)
			: this()
		{
			this.Text = title;
		}

		public FOutputText(string title, string text)
			: this(title)
		{
			_richTextBox.Text = text;
		}

		public FOutputText(string title, string text, int width, int height, bool isReadOnly)
			: this(title, text)
		{
			this.Width = width;
			this.Height = height;
			this._richTextBox.ReadOnly = isReadOnly;
		}


		#region 输出相关方法封装

		/// <summary>
		/// 输出一段彩色文字（指定前景色）
		/// </summary>
		public void Write(Color fc, string s, params object[] args)
		{
			_writer.Write(fc, s, args);
		}

		/// <summary>
		/// 输出一段彩色文字（用默认前景色）
		/// </summary>
		public void Write(string s, params object[] args)
		{
			_writer.Write(s, args);
		}

		/// <summary>
		/// 输出一段彩色文字（指定前景色，末尾带换行符）
		/// </summary>
		public void WriteLine(Color fc, string s, params object[] args)
		{
			_writer.WriteLine(fc, s, args);
		}
		/// <summary>
		/// 输出一段彩色文字（用默认前景色，末尾带换行符）
		/// </summary>
		public void WriteLine(string s, params object[] args)
		{
			_writer.WriteLine(s, args);
		}

		/// <summary>
		/// 输出一个空行
		/// </summary>
		public void WriteLine()
		{
			_writer.WriteLine();
		}

		/// <summary>
		/// 输出 n 个空行
		/// </summary>
		/// <param name="numOfNewLine"></param>
		public void WriteLine(int n)
		{
			_writer.WriteLine(n);
		}

		/// <summary>
		/// 清空 RichTextBox 的显示
		/// </summary>
		public void Clear()
		{
			_writer.Clear();
		}

		#endregion
	}
}
