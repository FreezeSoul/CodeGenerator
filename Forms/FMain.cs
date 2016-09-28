using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;

using CodeGenerator.Components;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;
using System.CodeDom.Compiler;
using Microsoft.CSharp;


namespace CodeGenerator
{
	public partial class FMain : Form
	{
		#region Fields

		/// <summary>
		/// 服务器连接对象
		/// </summary>
		private static ServerConnection _serverConnection = new ServerConnection();
		/// <summary>
		/// 服务器对象
		/// </summary>
		private static Server _server = null;
		/// <summary>
		/// 连接服务器的 Form
		/// </summary>
		public static FConnector _connector = new FConnector(_serverConnection);

		/// <summary>
		/// 生成器组件实例集合
		/// </summary>
		public static List<IGenComponent> _gens = new List<IGenComponent>();

		#endregion

		#region 初始化

		public FMain()
		{
			InitializeComponent();

			_TreeView.ImageList = _ImageList;
		}

		/// <summary>
		/// 初始化生成器组件，连接数据库
		/// </summary>
		private void FMain_Load(object sender, EventArgs e)
		{
			// 创建生成器组件实例
			// todo: 弹出一个组件初始化窗口，报告组件载入情况
			InitComponents();

			// 连库
			DialogResult dr = _connector.ShowDialog(this);
			if (dr == DialogResult.OK && _serverConnection.SqlConnectionObject.State == ConnectionState.Open)
			{
				_connector.Hide();

				_server = new Server(_serverConnection);

				if (_server != null)
				{
					#region Set SMO SQL Struct Data Limit

					_server.SetDefaultInitFields(typeof(Database), new String[] { "Name" });

					_server.SetDefaultInitFields(typeof(Table),
						new String[] { "Name", "Schema", "CreateDate", "IsSystemObject" });

					_server.SetDefaultInitFields(typeof(Column),
						new String[] { "Name", "Default", "DataType", "Length", "Nullable", "InPrimaryKey", "IsForeignKey", "Computed" });

					_server.SetDefaultInitFields(typeof(StoredProcedure),
						new String[] { "Name", "Schema", "IsSystemObject" });

					_server.SetDefaultInitFields(typeof(UserDefinedFunction),
						new String[] { "Name", "Schema", "DataType", "IsSystemObject" });

					if (_server.VersionMajor >= 10)
					{
						_server.SetDefaultInitFields(typeof(UserDefinedTableType),
							new String[] { "Name", "Schema" });
					}

					#endregion

					ShowDatabases();
				}
			}
			else
			{
				this.Close();
			}
		}

		/// <summary>
		/// 初始化 TreeView 显示
		/// </summary>
		private void ShowDatabases()
		{
			try
			{
				_TreeView.Nodes.Add("tips: mouse right click the nodes").Tag = "Databases";

				foreach (Database db in _server.Databases)
				{
					if (!db.IsSystemObject && db.IsAccessible)
					{
						TreeNode tn = _TreeView.Nodes.Add(db.Name);
						tn.SelectedImageKey = tn.ImageKey = "SQL_Database.png";
						tn.Tag = db;
					}
				}

				_TreeView.SelectedNode = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// 初始化代码生成组件集合
		/// todo: 检查对像是否具备默认构造函数，是否具备必须的 Property
		/// </summary>
		private void InitComponents()
		{
			// 载入内部组件
			InitComponents(Assembly.GetExecutingAssembly());


			// 载入外部组件 exe 所在文件 Components 目录下面的 *.cs
			// 查找出 *.cs
			string[] files = null;
			try
			{
				files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Components"), "*.cs", SearchOption.AllDirectories);
			}
			catch { }
			if (files == null || files.Length == 0) return;

			// 加载初始化
			CompilerParameters options = new CompilerParameters();

			string path_this = Assembly.GetExecutingAssembly().Location;
			string path_smo = typeof(Microsoft.SqlServer.Management.Smo.Server).Assembly.Location;
			string path_smo_sfc = typeof(Microsoft.SqlServer.Management.Sdk.Sfc.DataProvider).Assembly.Location;
			string path_smo_connectinfo = typeof(Microsoft.SqlServer.Management.Common.ServerConnection).Assembly.Location;
			string path_smo_sqlenum = typeof(Microsoft.SqlServer.Management.Smo.UserDefinedFunctionType).Assembly.Location;

			string path_35 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Reference Assemblies\Microsoft\Framework\v3.5");
			string path_35_core = Path.Combine(path_35, "System.Core.dll");
			string path_35_dsext = Path.Combine(path_35, "System.Data.DataSetExtensions.dll");
			string path_35_xmllinq = Path.Combine(path_35, "System.Xml.Linq.dll");

			options.ReferencedAssemblies.AddRange(new string[]{
				"System.dll"
				, "System.Data.dll"
				, "System.Xml.dll"
				, "System.Windows.Forms.dll"
				, "Microsoft.VisualBasic.dll"
				, path_this
				, path_smo
				, path_smo_sfc
				, path_smo_connectinfo
				, path_smo_sqlenum
				//, path_35_core
				//, path_35_dsext
				//, path_35_xmllinq
			});

			options.CompilerOptions = "/target:library";
			options.GenerateInMemory = true;

			CSharpCodeProvider provider = new CSharpCodeProvider();

			// 载入并报错
			try
			{
				CompilerResults result = provider.CompileAssemblyFromFile(options, files);
				if (result.Errors != null && result.Errors.Count > 0)
				{
					using (FOutputText f = new FOutputText())
					{
						f.Text = "Load components (*.cs) ccurred some error:";
						f.Width = 780;
						f.Height = 550;
						foreach (CompilerError ce in result.Errors)
						{
							f.WriteLine("FileName    = {0}", ce.FileName);
							f.WriteLine("Line        = {0}", ce.Line);
							f.WriteLine("ErrorNumber = {0}", ce.ErrorNumber);
							f.WriteLine("ErrorText   = {0}", ce.ErrorText);
							f.WriteLine("Column      = {0}", ce.Column);
							f.WriteLine("IsWarning   = {0}", ce.IsWarning);
							f.WriteLine(2);
						}
						f.ShowDialog();
						Application.Exit();
					}
				}
				InitComponents(result.CompiledAssembly);

				_gens.Sort(new Comparison<IGenComponent>((a, b) => { return string.Compare(string.Concat(a.Properties[GenProperties.Group], a.Properties[GenProperties.Caption]), string.Concat(b.Properties[GenProperties.Group], b.Properties[GenProperties.Caption])); }));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void InitComponents(Assembly a)
		{
			string interfacename = typeof(IGenComponent).FullName;
			Type[] types = a.GetTypes();
			foreach (Type t in types)		// 找出所有符合 IGenComponent 接口的类声明并创建加入集合
			{
				List<Type> interfaces = new List<Type>(t.GetInterfaces());
				if (interfaces.Exists(delegate(Type type) { return type.FullName == interfacename; }))
				{
					IGenComponent igc = (IGenComponent)a.CreateInstance(t.FullName);
					if (igc.Properties.ContainsKey(GenProperties.IsEnabled) &&
						(igc.Properties[GenProperties.IsEnabled] == "False" || !(bool)igc.Properties[GenProperties.IsEnabled])) continue;
					_gens.Add(igc);
				}
			}
		}


		#endregion

		#region 窗体主要事件方法

		private void FMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_serverConnection != null)
			{
				if (_serverConnection.SqlConnectionObject.State == ConnectionState.Open)
				{
					_serverConnection.Disconnect();
				}
			}

		}

		private void _Refresh_ToolStripButton_Click(object sender, EventArgs e)
		{
			Utils._CurrrentDALGenSetting = null;
			Utils._CurrrentDALGenSetting_CurrentScheme = null;
			_TreeView.Nodes.Clear();
			_SplitContainer.Panel2.Controls.Clear();
			_server = new Server(_server.ConnectionContext);
			ShowDatabases();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FAbout f = new FAbout();
			f.ShowDialog();
			f.Dispose();
		}

		private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FOutputHtml f = new FOutputHtml("关于生成物的简单说明", "Documents/SimpleHelp.htm", 600, 400);
			f.ShowDialog();
			f.Dispose();
		}

		private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		#endregion

		#region 画数据对象树 _TreeView_AfterSelect 事件代码

		private void _TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Cursor csr = null;
			try
			{
				csr = this.Cursor;
				this.Cursor = Cursors.WaitCursor;

				// 清空右边的 detail 类控件显示
				_SplitContainer.Panel2.Controls.Clear();

				TreeNode node = e.Node;
				object tag = node.Tag;
				Type tagType = tag.GetType();

				if (tag == null) node.Tag = node.Text;		// 保持 tag 不空，方便判断

				if (typeof(Database) == tagType && node.Nodes.Count == 0)
				{
					#region Make Empty Sub Tree Nodes

					TreeNode tn;

					tn = node.Nodes.Add("Tables");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "Tables";

					tn = node.Nodes.Add("Views");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "Views";

					tn = node.Nodes.Add("Stored Procedures");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "Stored Procedures";

					tn = node.Nodes.Add("Functions");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "Functions";

					tn = node.Nodes.Add("UserDefinedDataTypes");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "UserDefinedDataTypes";

					Database db = (Database)tag;		// 根据 SQL 的库的版本来决定是否生成某些节点

					if (db.CompatibilityLevel >= CompatibilityLevel.Version100)
					{
						tn = node.Nodes.Add("UserDefinedTableTypes");
						tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
						tn.Tag = "UserDefinedTableTypes";
					}

					if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
					{
						tn = node.Nodes.Add("User Schema");
						tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
						tn.Tag = "User Schema";
					}

					tn = node.Nodes.Add("Extended Properties");
					tn.SelectedImageKey = tn.ImageKey = "SQL_Folder.png";
					tn.Tag = "Extended Properties";

					#endregion
				}
				else if (typeof(Database) == tagType && node.Nodes.Count > 0)
				{
					Database db = (Database)tag;		// 不再重复的生成子节点
				}
				else if (typeof(Table) == tagType)
				{
					Table t = (Table)tag;
					CTable ct = new CTable(t);
					ct.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(ct);
				}
				else if (typeof(UserDefinedTableType) == tagType)
				{
					UserDefinedTableType t = (UserDefinedTableType)tag;
					CUserTableType ct = new CUserTableType(t);
					ct.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(ct);

					// FMain_UI_Binder.MakeByTable(_ToolStrip, t);
				}
				else if (typeof(Microsoft.SqlServer.Management.Smo.View) == tagType)
				{
					Microsoft.SqlServer.Management.Smo.View v = (Microsoft.SqlServer.Management.Smo.View)tag;
					CView cv = new CView(v);
					cv.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(cv);
				}
				else if (typeof(Column) == tagType)
				{
					// todo
				}
				else if (typeof(StoredProcedure) == tagType)
				{
					StoredProcedure sp = (StoredProcedure)tag;
					CStoredProcedure csp = new CStoredProcedure(sp);
					csp.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(csp);
				}
				else if (typeof(UserDefinedFunction) == tagType)
				{
					UserDefinedFunction fun = (UserDefinedFunction)tag;
					CFunction cfun = new CFunction(fun);
					cfun.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(cfun);
				}
				else if (typeof(ExtendedProperty) == tagType)
				{
					ExtendedProperty ep = (ExtendedProperty)tag;
					CExtendedProperty cep = new CExtendedProperty(ep);
					cep.Dock = DockStyle.Fill;
					_SplitContainer.Panel2.Controls.Add(cep);
				}
				else if (typeof(string) == tagType)
				{
					string name = (string)tag;
					if (name == "Tables")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<Table> uts = Utils.GetUserTables(db);
							foreach (Table tbl in uts)
							{
								TreeNode tn = new TreeNode(tbl.Schema + "." + tbl.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_Table.png";
								tn.Tag = tbl;
								node.Nodes.Add(tn);

								foreach (Column c in tbl.Columns)
								{
									TreeNode n = new TreeNode(c.Name);
									n.SelectedImageKey = n.ImageKey = "SQL_Column.png";
									n.Tag = c;
									tn.Nodes.Add(n);
								}

								foreach (ExtendedProperty ep in tbl.ExtendedProperties)
								{
									TreeNode n = new TreeNode(ep.Name);
									n.SelectedImageKey = n.ImageKey = "sql_constrain.png";
									n.Tag = ep;
									tn.Nodes.Add(n);
								}
							}
						}
						// ...
					}
					else if (name == "Views")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<Microsoft.SqlServer.Management.Smo.View> uvs = Utils.GetUserViews(db);
							foreach (Microsoft.SqlServer.Management.Smo.View v in uvs)
							{
								TreeNode tn = new TreeNode(v.Schema + "." + v.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_Table.png";
								tn.Tag = v;
								node.Nodes.Add(tn);

								foreach (Column c in v.Columns)
								{
									TreeNode n = new TreeNode(c.Name);
									n.SelectedImageKey = n.ImageKey = "SQL_Column.png";
									n.Tag = c;
									tn.Nodes.Add(n);
								}

								foreach (ExtendedProperty ep in v.ExtendedProperties)
								{
									TreeNode n = new TreeNode(ep.Name);
									n.SelectedImageKey = n.ImageKey = "sql_constrain.png";
									n.Tag = ep;
									tn.Nodes.Add(n);
								}

							}
						}
					}

					else if (name == "Stored Procedures")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<StoredProcedure> sps = Utils.GetUserStoredProcedures(db);

							foreach (StoredProcedure sp in sps)
							{
								TreeNode tn = new TreeNode(sp.Schema + "." + sp.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_StoredProcedure.png";
								tn.Tag = sp;
								node.Nodes.Add(tn);

								foreach (ExtendedProperty ep in sp.ExtendedProperties)
								{
									TreeNode n = new TreeNode(ep.Name);
									n.SelectedImageKey = n.ImageKey = "sql_constrain.png";
									n.Tag = ep;
									tn.Nodes.Add(n);
								}

							}
						}
					}

					else if (name == "Functions")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<UserDefinedFunction> ufs = Utils.GetUserFunctions(db);

							foreach (UserDefinedFunction fun in ufs)
							{
								TreeNode tn = new TreeNode(fun.Schema + "." + fun.Name);
								if (fun.FunctionType == UserDefinedFunctionType.Table || fun.FunctionType == UserDefinedFunctionType.Inline)
								{
									tn.SelectedImageKey = tn.ImageKey = "SQL_Function_Table.png";
								}
								else
								{
									tn.SelectedImageKey = tn.ImageKey = "SQL_Function_Scale.png";
								}
								tn.Tag = fun;
								node.Nodes.Add(tn);

								foreach (ExtendedProperty ep in fun.ExtendedProperties)
								{
									TreeNode n = new TreeNode(ep.Name);
									n.SelectedImageKey = n.ImageKey = "sql_constrain.png";
									n.Tag = ep;
									tn.Nodes.Add(n);
								}
							}

						}
					}

					else if (name == "UserDefinedDataTypes")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<UserDefinedDataType> uddts = Utils.GetUserDefinedDataTypes(db);

							foreach (UserDefinedDataType o in uddts)
							{
								TreeNode tn = new TreeNode(o.Schema + "." + o.Name + "(" + o.SystemType + " " + o.MaxLength.ToString() + ")");
								tn.SelectedImageKey = tn.ImageKey = "SQL_Function_Scale.png";
								tn.Tag = o;
								node.Nodes.Add(tn);
							}

						}
					}

					else if (name == "UserDefinedTableTypes")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<UserDefinedTableType> utts = Utils.GetUserDefinedTableTypes(db);
							foreach (UserDefinedTableType tbl in utts)
							{
								TreeNode tn = new TreeNode(tbl.Schema + "." + tbl.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_Table.png";
								tn.Tag = tbl;
								node.Nodes.Add(tn);

								foreach (Column c in tbl.Columns)
								{
									TreeNode n = new TreeNode(c.Name);
									n.SelectedImageKey = n.ImageKey = "SQL_Column.png";
									n.Tag = c;
									tn.Nodes.Add(n);
								}

								foreach (ExtendedProperty ep in tbl.ExtendedProperties)
								{
									TreeNode n = new TreeNode(ep.Name);
									n.SelectedImageKey = n.ImageKey = "sql_constrain.png";
									n.Tag = ep;
									tn.Nodes.Add(n);
								}
							}

						}
					}

					else if (name == "User Schema")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<Schema> uss = Utils.GetUserSchemas(db);
							foreach (Schema a in uss)
							{
								TreeNode tn = new TreeNode(a.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_Schema.png";
								tn.Tag = a;
								node.Nodes.Add(tn);
							}
						}
					}

					else if (name == "Extended Properties")
					{
						Database db = (Database)node.Parent.Tag;
						if (node.Nodes.Count == 0)
						{
							List<ExtendedProperty> eps = Utils.GetExtendedProperties(db);
							foreach (ExtendedProperty a in eps)
							{
								TreeNode tn = new TreeNode(a.Name);
								tn.SelectedImageKey = tn.ImageKey = "SQL_Schema.png";
								tn.Tag = a;
								node.Nodes.Add(tn);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = csr;  // Restore the original cursor
			}
		}

		#endregion

		#region 右键菜单相关


		/// <summary>
		/// 创建一个右键上下文菜单项
		/// </summary>
		private void CreateContextMenu(ContextMenu cm, string group, string caption, EventHandler handler)
		{
			if (string.IsNullOrEmpty(group))
			{
				cm.MenuItems.Add(new MenuItem(caption, handler));
			}
			else
			{
				MenuItem pm = null;
				foreach (MenuItem m in cm.MenuItems)
				{
					if (m.Text == group)
					{
						pm = m;
						break;
					}
				}
				if (pm == null)
				{
					pm = new MenuItem(group);
					cm.MenuItems.Add(pm);
				}
				MenuItem mi = new MenuItem(caption, handler);
				pm.MenuItems.Add(mi);
			}
		}

		/// <summary>
		/// 实现鼠标右键上下文菜单
		/// </summary>
		private void _TreeView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (_TreeView.GetNodeAt(e.X, e.Y) != null)
				{
					TreeNode node = _TreeView.SelectedNode = _TreeView.GetNodeAt(e.X, e.Y);

					ContextMenu cm = new ContextMenu();
					object tag = node.Tag;
					Type tagType = tag.GetType();

					if (typeof(Database) == tagType)
					{
						// 找出针对 Database 生成的 IGenComponents
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Database) > 0; });

						// 将 IGenComponent 转为生成上下文菜单
						foreach (IGenComponent gen in gens)
						{
							// 初始化一下 IGenComponent
							IGenComponent g = gen;				// 解决闭包问题
							g.Server = _server;
							g.Database = (Database)tag;

							// 如果通过生成前检查, 则生成上下文菜单, 并绑定生成事件
							if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen());
							});
						}
					}
					else if (typeof(Table) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Table) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((Table)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((Table)tag));
							});
						}
					}
					else if (typeof(UserDefinedTableType) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.UserDefinedTableType) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((UserDefinedTableType)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((UserDefinedTableType)tag));
							});
						}
					}
					else if (typeof(Microsoft.SqlServer.Management.Smo.View) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.View) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((Microsoft.SqlServer.Management.Smo.View)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((Microsoft.SqlServer.Management.Smo.View)tag));
							});
						}
					}
					else if (typeof(Column) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Column) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((Column)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((Column)tag));
							});
						}
					}
					else if (typeof(StoredProcedure) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.StoredProcedure) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((StoredProcedure)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((StoredProcedure)tag));
							});
						}
					}
					else if (typeof(UserDefinedFunction) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.UserDefinedFunction) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((UserDefinedFunction)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((UserDefinedFunction)tag));
							});
						}
					}
					else if (typeof(ExtendedProperty) == tagType)
					{
						List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.ExtendedProperty) > 0; });
						foreach (IGenComponent gen in gens)
						{
							IGenComponent g = gen;
							g.Server = _server;
							g.Database = (Database)node.Parent.Parent.Tag;
							if (g.Validate((ExtendedProperty)tag)) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
							{
								Output(g.Gen((ExtendedProperty)tag));
							});
						}
					}
					else if (typeof(string) == tagType)
					{
						string name = (string)tag;

						if (name == "Databases")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Databases) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = null;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "Tables")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Tables) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = (Database)node.Parent.Tag;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "Views")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.Views) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = (Database)node.Parent.Tag;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "Stored Procedures")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.StoredProcedures) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = (Database)node.Parent.Tag;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "Functions")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.UserDefinedFunctions) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = (Database)node.Parent.Tag;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "UserDefinedDataTypes")
						{
							// todo
						}
						else if (name == "UserDefinedTableTypes")
						{
							List<IGenComponent> gens = _gens.FindAll(delegate(IGenComponent o) { return (int)(o.TargetSqlElementType & SqlElementTypes.UserDefinedTableTypes) > 0; });
							foreach (IGenComponent gen in gens)
							{
								IGenComponent g = gen;
								g.Server = _server;
								g.Database = (Database)node.Parent.Tag;
								if (g.Validate()) CreateContextMenu(cm, (string)g.Properties[GenProperties.Group], (string)g.Properties[GenProperties.Caption], delegate(object s, EventArgs ea)
								{
									Output(g.Gen());
								});
							}
						}
						else if (name == "User Schema")
						{
							// todo
						}
					}

					if (cm.MenuItems.Count > 0)
						_TreeView.ContextMenu = cm;
					else
						_TreeView.ContextMenu = null;
				}
				else
					_TreeView.ContextMenu = null;
			}
		}

		#endregion

		#region 输出结果相关

		/// <summary>
		/// 输出生成结果
		/// </summary>
		public void Output(GenResult result)
		{
			if (result == null)
			{
				// do nothing
			}
			else if (result.GenResultType == GenResultTypes.Message)
			{
				if (result.Message == null) return;
				using (FOutputText f = new FOutputText(result.Message))
				{
					f.ShowDialog();
				}
			}
			else if (result.GenResultType == GenResultTypes.CodeSegment)
			{
				using (FOutputCode f = new FOutputCode(result.CodeSegment))
				{
					f.ShowDialog();
				}
			}
			else if (result.GenResultType == GenResultTypes.CodeSegments)
			{
				using (FOutputCodes f = new FOutputCodes(result.CodeSegments))
				{
					f.ShowDialog();
				}
			}
			else if (result.GenResultType == GenResultTypes.File)
			{
				CleanOutput();

				Output(result.File.Key, result.File.Value);

				PopupOutput();
			}
			else if (result.GenResultType == GenResultTypes.Files)
			{
				CleanOutput();

				foreach (KeyValuePair<string, byte[]> file in result.Files)
				{
					Output(file.Key, file.Value);
				}

				PopupOutput();
			}
		}

		/// <summary>
		/// 清空输出结果
		/// </summary>
		public void CleanOutput()
		{
			string path = Path.Combine(Application.StartupPath, "Output");
			try
			{
				//Directory.Delete(path, true);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				foreach (string file in Directory.GetFiles(path))
				{
					File.Delete(file);
				}
			}
			catch { }
		}

		/// <summary>
		/// 输出生成结果到文件
		/// </summary>
		public void Output(string fn, byte[] fc)
		{
			string path = Path.Combine(Application.StartupPath, "Output");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			fn = Path.Combine(path, fn);
			if (!Directory.Exists(Path.GetDirectoryName(fn)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fn));
			}
			using (FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write))
			{
				fs.Write(fc, 0, fc.Length);
			}
		}

		/// <summary>
		/// 弹出代码输出目录
		/// </summary>
		public void PopupOutput()
		{
			Process.Start("Explorer.exe", Path.Combine(Application.StartupPath, "Output"));
		}

		#endregion
	}
}
