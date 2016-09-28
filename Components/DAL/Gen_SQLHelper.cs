using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// ¥¥Ω® SQLHelper°°¿‡
	/// </summary>
	public static partial class Gen_SQLHelper
	{
		public static string Gen(string ns)
		{
			string fn = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Documents/SQLHelper.cs");

			using (StreamReader sw = new StreamReader(fn))
			{
				return sw.ReadToEnd().Replace("namespace DAL", "namespace " + ns);	//todo: replace ?
			}
		}
	}
}
