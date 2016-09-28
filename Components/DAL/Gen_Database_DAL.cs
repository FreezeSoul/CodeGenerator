using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;


namespace CodeGenerator.Components.DAL
{
	public class Gen_Database_DAL : IGenComponent
	{
		#region Init

		public Gen_Database_DAL()
		{
			this._properties.Add(GenProperties.Name, "Gen_Database_DAL");
			this._properties.Add(GenProperties.Caption, "2. DAL 层生成");
			this._properties.Add(GenProperties.Group, "");
			this._properties.Add(GenProperties.Tips, "生成数据操作DAL(数据集,类声明,静态方法)");
		}
		public Gen_Database_DAL(int schemeID)
		{
			this._currentSchemeID = schemeID;
			this._isPopupConfigForm = false;
		}

		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.Database; }
		}

		#endregion

		#region Misc

		protected Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
		public Dictionary<GenProperties, object> Properties
		{
			get
			{
				return this._properties;
			}
		}

		public event System.ComponentModel.CancelEventHandler OnProcessing;

		private Server _server;
		public Server Server
		{
			set { _server = value; }
		}

		private Database _db;
		public Database Database
		{
			set { _db = value; }
		}

		#endregion

		public bool Validate(params object[] sqlElements)
		{
			return true;
		}

		public bool _isPopupConfigForm = true;
		public int _currentSchemeID = 1;

		public GenResult Gen(params object[] sqlElements)
		{
			GenResult gr;

			if (_isPopupConfigForm)
			{
				DialogResult dr;

				using (FGen_Database_DAL_Config cfg = new FGen_Database_DAL_Config(_db, _currentSchemeID))
				{
					dr = cfg.ShowDialog();
				}

				if (dr != DialogResult.OK)
				{
					gr = new GenResult(GenResultTypes.Message);
					gr.Message = null;
					return gr;
				}
			}
			Utils.LoadDatabaseDALGenSettingDS(_db, _currentSchemeID);

			string ns = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;

			// 只要有任意的数据操作方法需要生成， 即生成 DC 对象根
			bool isSupportDC = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function ||
				Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP;

			// 只要有 DB, OB 其中之一需要生成， 即生成相应 DC 对象
			bool isSupportDC_Table = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table || Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table;
			bool isSupportDC_View = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View || Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View;
			bool isSupportDC_Function = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function || Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function;
			bool isSupportDC_SP = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP || Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP;


			gr = new GenResult(GenResultTypes.Files);
			gr.Files = new List<KeyValuePair<string, byte[]>>();

			using (FOutputText fw = new FOutputText("Gening...Plz Wait...", "", 350, 500, true))
			{
                fw.Text = "Generate Information";
				fw.Show();
				fw.Activate();

                fw.Write("Analysing");
				fw.Write(Color.Blue, "Prefetch");
				fw.WriteLine("...");

				#region Prefetch

				ScriptingOptions option = new ScriptingOptions();
				option.ExtendedProperties = true;
				try
				{
					_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.StoredProcedure), option);
				}
				catch { }
				try
				{
					_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.UserDefinedFunction), option);
				}
				catch { }
				try
				{
					_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.View), option);
				}
				catch { }
				try
				{
					_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.Table), option);
				}
				catch { }
				try
				{
					_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.UserDefinedType), option);
				}
				catch { }
				if (_db.CompatibilityLevel >= CompatibilityLevel.Version100)
				{
					try
					{
						_db.PrefetchObjects(typeof(Microsoft.SqlServer.Management.Smo.UserDefinedTableType), option);
					}
					catch { }
				}

				#endregion

				fw.WriteLine(Color.OrangeRed, "Done!");


				fw.Write("Generating");
				fw.Write(Color.Blue, " Database Informatin");
				fw.WriteLine("...");

				gr.Files.Add(new KeyValuePair<string, byte[]>("DI.cs", Encoding.UTF8.GetBytes(
					Gen_DI.Gen(_db, ns, "DS2")
				)));

				fw.WriteLine(Color.OrangeRed, "Done!");


				if (_db.CompatibilityLevel >= CompatibilityLevel.Version100)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "UserDefinedTableType Information");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("DI2.cs", Encoding.UTF8.GetBytes(
						Gen_DI2.Gen(_db, ns, "DI2")
					)));

					fw.WriteLine(Color.OrangeRed, "Done!");



                    fw.Write("Generating");
                    fw.Write(Color.Blue, "UserDefinedTableType DataSet Declare");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("DS2.cs", Encoding.UTF8.GetBytes(
						Gen_DS2.Gen(_db, ns, "DS2")
					)));

					fw.WriteLine(Color.OrangeRed, "Done!");



                    fw.Write("Generating");
                    fw.Write(Color.Blue, "UserDefinedTableType Object Class Declare");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("OO2.cs", Encoding.UTF8.GetBytes(
						Gen_OO2.Gen(_db, ns, Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF, "DS2")
					)));

					fw.WriteLine(Color.OrangeRed, "Done!");
				}


				if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDS)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "DataSet");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("DS.cs", Encoding.UTF8.GetBytes(
						Gen_DS.Gen(_db, ns, "DS")
					)));

					fw.WriteLine(Color.OrangeRed, "Done!");
				}


				if (isSupportDC)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "Data Command");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("DC.cs", Encoding.UTF8.GetBytes(
						Gen_DC.Gen(_db, ns)
					)));

					if (isSupportDC_Table)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DC_Table.cs", Encoding.UTF8.GetBytes(
							Gen_DC_Table.Gen(_db, ns)
						)));
					}
					if (isSupportDC_View)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DC_View.cs", Encoding.UTF8.GetBytes(
							Gen_DC_View.Gen(_db, ns)
						)));
					}
					if (isSupportDC_Function)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DC_Function.cs", Encoding.UTF8.GetBytes(
							Gen_DC_Function.Gen(_db, ns)
						)));
					}
					if (isSupportDC_SP)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DC_StoredProcedure.cs", Encoding.UTF8.GetBytes(
							Gen_DC_StoredProcedure.Gen(_db, ns)
						)));
					}

					fw.WriteLine(Color.OrangeRed, "Done!");
				}

				if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "DataSet Business");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("DB.cs", Encoding.UTF8.GetBytes(
						Gen_DB.Gen(_db, ns, "DS", "DS2")
					)));

					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DB_Table.cs", Encoding.UTF8.GetBytes(
							Gen_DB_Table.Gen(_db, ns, "DS", "DS2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DB_View.cs", Encoding.UTF8.GetBytes(
							Gen_DB_View.Gen(_db, ns, "DS", "DS2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DB_Function.cs", Encoding.UTF8.GetBytes(
							Gen_DB_Function.Gen(_db, ns, "DS", "DS2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("DB_StoredProcedure.cs", Encoding.UTF8.GetBytes(
							Gen_DB_StoredProcedure.Gen(_db, ns, "DS", "DS2")
						)));
					}

					fw.WriteLine(Color.OrangeRed, "Done!");
				}

				if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOO)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "Object Class Declare");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("OO.cs", Encoding.UTF8.GetBytes(
						Gen_OO.Gen(_db, ns, Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF)
					)));

					fw.WriteLine(Color.OrangeRed, "Done!");
				}

				if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function ||
					Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP)
				{
                    fw.Write("Generating");
					fw.Write(Color.Blue, "Object Business");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("OB.cs", Encoding.UTF8.GetBytes(
						Gen_OB.Gen(_db, ns, "OO2")
					)));

					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_Table.cs", Encoding.UTF8.GetBytes(
							Gen_OB_Table.Gen(_db, ns, "OO2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_View.cs", Encoding.UTF8.GetBytes(
							Gen_OB_View.Gen(_db, ns, "OO2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_Function.cs", Encoding.UTF8.GetBytes(
							Gen_OB_Function.Gen(_db, ns, "OO2")
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_StoredProcedure.cs", Encoding.UTF8.GetBytes(
							Gen_OB_StoredProcedure.Gen(_db, ns, "OO2")
						)));
					}

					fw.WriteLine(Color.OrangeRed, "Done!");
				}


				if (isSupportDC)
				{

                    fw.Write("Generating");
					fw.Write(Color.Blue, "Object Expression");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("OE.cs", Encoding.UTF8.GetBytes(
						Gen_OE.Gen(_db, ns)
					)));
					if (isSupportDC_Table)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OE_Table.cs", Encoding.UTF8.GetBytes(
							Gen_OE_Table.Gen(_db, ns)
						)));
					}
					if (isSupportDC_View)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OE_View.cs", Encoding.UTF8.GetBytes(
							Gen_OE_View.Gen(_db, ns)
						)));
					}
					if (isSupportDC_Function)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OE_Function.cs", Encoding.UTF8.GetBytes(
							Gen_OE_Function.Gen(_db, ns)
						)));
					}

					fw.WriteLine(Color.OrangeRed, "Done!");
				}

				if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Extend)
				{

                    fw.Write("Generating");
					fw.Write(Color.Blue, "Object Business Extension（ .net 3.5+　）");
					fw.WriteLine("...");

					gr.Files.Add(new KeyValuePair<string, byte[]>("OB_Extend.cs", Encoding.UTF8.GetBytes(
						Gen_OB_Extend.Gen(_db, ns)
					)));
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_Extend_Table.cs", Encoding.UTF8.GetBytes(
							Gen_OB_Extend_Table.Gen(_db, ns)
						)));
					}
					if (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View)
					{
						gr.Files.Add(new KeyValuePair<string, byte[]>("OB_Extend_View.cs", Encoding.UTF8.GetBytes(
							Gen_OB_Extend_View.Gen(_db, ns)
						)));
					}

					fw.WriteLine(Color.OrangeRed, "Done!");
				}

                fw.Write("Generating");
				fw.Write(Color.Blue, "SQLHelper");
				fw.WriteLine("...");


				gr.Files.Add(new KeyValuePair<string, byte[]>("SQLHelper.cs", Encoding.UTF8.GetBytes(
					Gen_SQLHelper.Gen(ns)
				)));

				fw.WriteLine(Color.OrangeRed, "Done!");

				fw.WriteLine();
				fw.WriteLine(Color.Red, "All Done!");
			}

			//gr = new GenResult(GenResultTypes.CodeSegment);
			//gr.CodeSegment = new KeyValuePair<string, string>(this.Tips, t.Name + " gen finished!");
			return gr;
		}

	}
}
