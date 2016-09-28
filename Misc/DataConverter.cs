using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CodeGenerator
{
	/// <summary>
	/// 包含各种各种数据类型转换处理方法
	/// </summary>
	public static class DataConverter
	{
		#region 字串 IP 转换为 uint IP

		/// <summary>
		/// 字串 IP 转换为 uint IP
		/// </summary>
		public static uint IPStr2Num(string ipString)
		{
			string[] ip = ipString.Split('.');
			if (ip.Length != 4) throw new Exception("Invalid IP Address!");
			uint num = 0;
			for (int i = 0; i < ip.Length; i++)
			{
				num <<= 8;
				num |= uint.Parse(ip[i]);
			}
			return num;
		}
		#endregion

		#region Object 转换为 Int? 型

		/// <summary>
		/// Object 转换为 Int? 型
		/// </summary>
		public static int? Object2Int(object o)
		{
			try
			{
				if (o == null) return null;
				return Convert.ToInt32(o.ToString());
			}
			catch (Exception)
			{
				return null;
			}
		}

		#endregion

		#region 将一个字串转换为 rtf 码

		/// <summary>
		/// 将一个字串转换为 rtf 码
		/// </summary>
		/// <param name="s">字串</param>
		/// <param name="c">颜色</param>
		/// <returns>转换后的 rtf 代码</returns>
		public static string Str2Rtf(string s, Color c)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf1\ansi{\colortbl ;\red" + c.R.ToString() + @"\green" + c.G.ToString() + @"\blue" + c.B.ToString() + @";}\f0\cf1 ");

			foreach (char ch in s)
			{
				int asc = Microsoft.VisualBasic.Strings.Asc(ch);
				if (asc > 127)
				{
					Int16ToByte itb = new Int16ToByte();
					itb.I = (Int16)asc;
					sb.Append(@"\'" + itb.B1.ToString("x") + @"\'" + itb.B2.ToString("x"));
				}
				else
				{
					if (ch == '\\')
					{
						sb.Append("\\\\");
					}
					else
					{
						sb.Append(ch);
					}
				}
			}
			sb.Append(@"}");
			return sb.Replace(Environment.NewLine, @"\par").ToString();
		}

		#endregion

		#region Struct, 用于将 int16 拆分为两个 byte
		/// <summary>
		/// 用于将 int16 拆分为两个 byte
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct Int16ToByte
		{
			[FieldOffset(0)]
			public Int16 I;
			[FieldOffset(0)]
			public Byte B1;
			[FieldOffset(1)]
			public Byte B2;
		}

		#endregion

	}
}
