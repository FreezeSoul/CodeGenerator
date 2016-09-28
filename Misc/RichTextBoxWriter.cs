using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CodeGenerator
{
	/// <summary>
	/// 用于方便的往一个 RichTextBox 输出彩色文字
	/// </summary>
	public class RichTextBoxWriter
	{
		#region Properties

		private RichTextBox _richTextBox;
		/// <summary>
		/// 输出用 RichTextBox
		/// </summary>
		public RichTextBox RichTextBox
		{
			get { return _richTextBox; }
		}

		private int _maxLength = 20000;
		/// <summary>
		/// 输出内容限长（超出将清空）
		/// </summary>
		public int MaxLength
		{
			get { return _maxLength; }
		}

		private Color _defaultForegroundColor = Color.Black;
		/// <summary>
		/// 输出文字默认前景色
		/// </summary>
		public Color DefaultForegroundColor
		{
			get { return _defaultForegroundColor; }
			set { _defaultForegroundColor = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 创建一个基于 RichTextBox 的字串输出对象
		/// </summary>
		/// <param name="richTextBox">用于输出的RichTextBox</param>
		/// <param name="bufferLength">字符限长</param>
		public RichTextBoxWriter(RichTextBox richTextBox, int maxLength)
		{
			_richTextBox = richTextBox;
			_maxLength = maxLength;
		}

		/// <summary>
		/// 创建一个基于 RichTextBox 的字串输出对象
		/// </summary>
		/// <param name="richTextBox">用于输出的RichTextBox</param>
		public RichTextBoxWriter(RichTextBox richTextBox)
		{
			_richTextBox = richTextBox;
		}

		/// <summary>
		/// 创建一个基于 RichTextBox 的字串输出对象（带指定默认前景色）
		/// </summary>
		/// <param name="richTextBox">用于输出的RichTextBox</param>
		/// <param name="bufferLength">字符限长</param>
		/// <param name="fg">默认前景色</param>
		public RichTextBoxWriter(RichTextBox richTextBox, int maxLength, Color fg)
		{
			_richTextBox = richTextBox;
			_maxLength = maxLength;
			_defaultForegroundColor = fg;
		}

		#endregion

		#region Methods

		/// <summary>
		/// 输出一段彩色文字（指定前景色）
		/// </summary>
		public void Write(Color fc, string s, params object[] args)
		{
			if (_richTextBox.TextLength > 20000)
			{
				_richTextBox.Text = ".........." + System.Environment.NewLine;
			}
			_richTextBox.Select(_richTextBox.Text.Length, 0);
			string rtf = DataConverter.Str2Rtf(string.Format(s, args), fc);
			_richTextBox.SelectedRtf = rtf;

			Application.DoEvents();
		}

		/// <summary>
		/// 输出一段彩色文字（用默认前景色）
		/// </summary>
		public void Write(string s, params object[] args)
		{
			Write(_defaultForegroundColor, s, args);

			Application.DoEvents();
		}

		/// <summary>
		/// 输出一段彩色文字（指定前景色，末尾带换行符）
		/// </summary>
		public void WriteLine(Color fc, string s, params object[] args)
		{
			Write(fc, s + System.Environment.NewLine, args);

			Application.DoEvents();
		}
		/// <summary>
		/// 输出一段彩色文字（用默认前景色，末尾带换行符）
		/// </summary>
		public void WriteLine(string s, params object[] args)
		{
			WriteLine(_defaultForegroundColor, s, args);

			Application.DoEvents();
		}

		/// <summary>
		/// 输出一个空行
		/// </summary>
		public void WriteLine()
		{
			WriteLine("");
		}

		/// <summary>
		/// 输出 n 个空行
		/// </summary>
		/// <param name="numOfNewLine"></param>
		public void WriteLine(int n)
		{
			StringBuilder sb = new StringBuilder(n*2);
			for (int i = 0; i < n; i++)
			{
				sb.Append(Environment.NewLine);
			}
			Write(sb.ToString());
		}

		/// <summary>
		/// 清空 RichTextBox 的显示
		/// </summary>
		public void Clear()
		{
			_richTextBox.Text = "";
		}

		#endregion
	}
}
