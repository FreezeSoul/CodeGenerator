using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeGenerator
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// todo: ��������

			if (args.Length != 0)
			{
				FOutputHtml f = new FOutputHtml("CodeGenerator �����в���ģʽ˵��", "Documents/CmdHelp.htm", 720, 480);
				f.StartPosition = FormStartPosition.CenterScreen;
				f.ShowDialog();
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm = new FMain();
			Application.Run(MainForm);
		}

		public static FMain MainForm = null;
	}
} 