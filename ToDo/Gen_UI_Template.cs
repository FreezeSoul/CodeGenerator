using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.IO;

namespace CodeGenerator.Components.UI.NeverCleanUp
{
	public static class Gen_UI_Template
	{
		private static string _path = "Output\\";
		private static string _ctrlpath = "Output\\Ctrls\\";
		private static string _htmlpath = "Output\\Templates\\";

		public static void Gen(Table t)
		{
			string tn = Utils.GetEscapeName(t);
			_path = System.IO.Path.Combine(Environment.CurrentDirectory, _path);
			try
			{
				System.IO.Directory.CreateDirectory(_path);
			}
			catch { }

			_ctrlpath = System.IO.Path.Combine(Environment.CurrentDirectory, _ctrlpath);
			try
			{
				System.IO.Directory.CreateDirectory(_ctrlpath);
			}
			catch { }

			_htmlpath = System.IO.Path.Combine(Environment.CurrentDirectory, _htmlpath);
			try
			{
				System.IO.Directory.CreateDirectory(_htmlpath);
			}
			catch { }

			System.Diagnostics.Process.Start("Explorer.exe", _path);


			string fn = System.IO.Path.Combine(_htmlpath, t.Name + ".html");
			using (StreamWriter sw = new StreamWriter(fn, false, Encoding.Unicode))
			{
				List<Column> wcs = Utils.GetWriteableColumns(t);

				sw.Write(@"<html>
<head>
	<title></title>
</head>
<body>
	<table>");
				foreach (Column c in wcs)
				{
					string caption = Utils.GetCaption(c);
					if (string.IsNullOrEmpty(caption)) caption = c.Name;

					sw.Write(@"
		<tr>
			<td>" + caption + @"
			</td>
			<td><input type=""text"" id=""" + c.Name + @""" value=""" + (Utils.CheckIsNumericType(c) ? "0" : "") + @""" />
			</td>
		</tr>");
				}
				sw.Write(@"
		<tr>
			<td colspan=""2""><button id=""提交"" style=""float:right"">提交</button>
			<button id=""公式设定"">公式设定</button><button id=""公式运行"">公式运行</button>
			</td>
		</tr>
	</table>
</body>
</html>
");
				sw.Close();
			}

			fn = System.IO.Path.Combine(_ctrlpath, "C" + t.Name + ".cs");
			using (StreamWriter sw = new StreamWriter(fn, false, Encoding.Unicode))
			{
				sw.Write(@"using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.VisualBasic;

namespace CodeGen.Ctrls
{
	public class C" + t.Name + @"WebBrowser : WebBrowser
	{

		public C" + t.Name + @"WebBrowser()
			: base()
		{
			this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(C" + t.Name + @"WebBrowser_DocumentCompleted);
		}

		protected override void InitLayout()
		{
			base.InitLayout();

			if (DesignMode) return;
			this.Url = new Uri(Application.StartupPath + ""\\Templates\\" + t.Name + @".html"");
		}
");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;

					sw.Write(@"
		private HtmlElement _" + c.Name + @"_HtmlElement;");
				}
				sw.Write(@"

		private HtmlElement _提交_HtmlElement;
		private HtmlElement _公式设定_HtmlElement;
		private HtmlElement _公式运行_HtmlElement;

");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;
					if (Utils.CheckIsNumericType(c))
					{
						sw.Write(@"
		public " + Utils.GetDataType(c) + @" " + c.Name + @"
		{
			get
			{
				string s = _" + c.Name + @"_HtmlElement.GetAttribute(""value"");
				if (Information.IsNumeric(s)) return " + Utils.GetDataType(c) + @".Parse(s);
				else return 0;
			}
			set
			{
				_" + c.Name + @"_HtmlElement.SetAttribute(""value"", value.ToString());
			}
		}");
					}
					else
					{
						sw.Write(@"
		public string " + c.Name + @"
		{
			get
			{
				return _" + c.Name + @"_HtmlElement.GetAttribute(""value"");
			}
			set
			{
				_" + c.Name + @"_HtmlElement.SetAttribute(""value"", value);
			}
		}");
					}
				}

				sw.Write(@"

		public event EventHandler OnLoad = null;
		public event EventHandler On提交Clicked = null;
		public event EventHandler On公式设定Clicked = null;
		public event EventHandler On公式运行Clicked = null;
");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;

					sw.Write(@"
		public event EventHandler On" + c.Name + @"Changed = null;");
				}
				sw.Write(@"

		private void C" + t.Name + @"WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			HtmlDocument hd = this.Document;

			_提交_HtmlElement = hd.All[""提交""];
			_提交_HtmlElement.Click += delegate(object o, HtmlElementEventArgs ex)
			{
				if (On提交Clicked != null) On提交Clicked(o, ex);
			};

			_公式设定_HtmlElement = hd.All[""公式设定""];
			_公式设定_HtmlElement.Click += delegate(object o, HtmlElementEventArgs ex)
			{
				if (On公式设定Clicked != null) On公式设定Clicked(o, ex);
			};

			_公式运行_HtmlElement = hd.All[""公式运行""];
			_公式运行_HtmlElement.Click += delegate(object o, HtmlElementEventArgs ex)
			{
				if (On公式运行Clicked != null) On公式运行Clicked(o, ex);
			};

");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;

					sw.Write(@"
			_" + c.Name + @"_HtmlElement = hd.All[""" + c.Name + @"""];
			_" + c.Name + @"_HtmlElement.AttachEventHandler(""onchange"", delegate(object o, EventArgs ex)
			{
				if (On" + c.Name + @"Changed != null) On" + c.Name + @"Changed(o, ex);
			});");
				}

				sw.Write(@"

			if (OnLoad != null) OnLoad(sender, e);
		}


		CodeDomProvider _cdp = null;
		CompilerParameters _cp = null;
		CompilerResults _cr = null;
		MethodInfo _mi = null;
		Assembly _a = null;
		Type _t = null;
");

				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;

					sw.Write(@"
		FieldInfo _fi" + c.Name + @" = null;");
				}

				sw.Write(@"

		public void SetExpression(string expression)
		{
			_cdp = new VBCodeProvider();
			_cp = new CompilerParameters();

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(@""
Imports System
Imports System.Math
Imports Microsoft.VisualBasic

Public Module Calc
");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;

					sw.Write(@"
	Public " + c.Name + @" As " + (Utils.CheckIsNumericType(c) ? Utils.GetDataType(c) : "string") + @"");
				}

				sw.Write(@"

	Public Sub Run()
		"" + expression + @""
	End Sub
End Module
"");
");
				sw.Write(@"

			_cp.ReferencedAssemblies.Add(""System.dll"");
			_cp.ReferencedAssemblies.Add(""Microsoft.VisualBasic.dll"");
			_cp.GenerateExecutable = false;
			_cp.GenerateInMemory = true;

			CompilerResults _cr = _cdp.CompileAssemblyFromSource(_cp, sb.ToString());

			if (_cr.Errors.HasErrors)
			{
				StringBuilder ssbb = new StringBuilder();
				foreach (CompilerError ce in _cr.Errors)
				{
					ssbb.Append(ce.ErrorText + Environment.NewLine);
				}
				MessageBox.Show(ssbb.ToString());
				return;
			}
			else
			{
				_a = _cr.CompiledAssembly;
				_t = _a.GetType(""Calc"");
				try
				{
					_mi = _t.GetMethod(""Run"", BindingFlags.Static | BindingFlags.Public);");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;
					sw.Write(@"
					_fi" + c.Name + @" = _t.GetField(""" + c.Name + @""", BindingFlags.Static | BindingFlags.Public);");
				}
				sw.Write(@"
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		public void Eval()
		{
			if (_mi == null)
			{
				MessageBox.Show(""请先设置公式再运行！"");
				return;
			}
			try
			{");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;
					sw.Write(@"
				_fi" + c.Name + @".SetValue(null, this." + c.Name + @");");
				}
				sw.Write(@"

				_mi.Invoke(null, new object[0]);
");
				foreach (Column c in t.Columns)
				{
					if (c.Identity || c.Computed) continue;
					sw.Write(@"
				this." + c.Name + @" = (" + (Utils.CheckIsNumericType(c) ? Utils.GetDataType(c) : "string") + @")_fi" + c.Name + @".GetValue(null);");
				}
				sw.Write(@"
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

	}
}
");
				sw.Close();
			}

		}
	}
}

