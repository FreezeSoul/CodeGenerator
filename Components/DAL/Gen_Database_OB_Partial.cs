using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;


namespace CodeGenerator.Components.DAL
{
	public class Gen_Database_OB_Partial : IGenComponent
	{
		#region Init

		public Gen_Database_OB_Partial()
		{
			this._properties.Add(GenProperties.Name, "Gen_Database_OB_Partial");
			this._properties.Add(GenProperties.Caption, "3. DAL.OB_Partial 文件生成");
			this._properties.Add(GenProperties.Group, "");
			this._properties.Add(GenProperties.Tips, "生成数据操作DAL中的OB的Partial文件用于放置自己的扩展方法");
		}
		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.Database; }
		}

		#endregion

		#region Misc

		Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
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

		public GenResult Gen(params object[] sqlElements)
		{
			GenResult gr;



			gr = new GenResult(GenResultTypes.File);
			gr.File = new KeyValuePair<string, byte[]>("OB_Partial.cs", Encoding.UTF8.GetBytes(Gen()));
			return gr;
		}

		private string Gen()
		{
			#region Header

			Utils.LoadDatabaseDALGenSettingDS(_db);
			string ns = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;

			string s = "", s1 = "";
			List<StoredProcedure> sps = Utils.GetUserStoredProcedures(_db);
			List<Table> uts = Utils.GetUserTables(_db);
			List<View> uvs = Utils.GetUserViews(_db);
			List<UserDefinedFunction> ufs = Utils.GetUserFunctions(_db);

			StringBuilder sb = new StringBuilder(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace " + ns + @"
{
	/// <summary>
	/// 基于 OO (类实例) 的数据操作静态方法集合类 的 Partial 扩展类
	/// </summary>
	public partial class OB
	{");
			#endregion

			#region Tables

			#region Header

			sb.Append(@"
		#region Tables
");
		#endregion

			foreach (Table t in uts)
			{
				#region Header

				List<Column> pks = Utils.GetPrimaryKeyColumns(t);
				List<Column> wcs = Utils.GetWriteableColumns(t);
				Dictionary<Column, Column> pfcs = Utils.GetTreePKFKColumns(t);
				string tn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tn + @"

		public partial class " + tn + @"
		{
			// 在这里写扩展的静态方法
		}

		#endregion
");
				#endregion
			}

			#region Footer

			sb.Append(@"
		#endregion
");
			#endregion

			#endregion

			#region Views

			#region Header

			sb.Append(@"
		#region Views
");

		#endregion

			foreach (View v in uvs)
			{
				#region Header

				string tn = Utils.GetEscapeName(v);
				List<Column> pks = Utils.GetPrimaryKeyColumns(v);
				Dictionary<Column, Column> pfcs = new Dictionary<Column, Column>();
				if (Utils.GetBaseTable(v) != null) pfcs = Utils.GetTreePKFKColumns(Utils.GetBaseTable(v));

				sb.Append(@"
		#region " + tn + @"

		public partial class " + tn + @"
		{
			// 在这里写扩展的静态方法
		}

		#endregion
");
				#endregion
			}

			#region Footer

			sb.Append(@"
		#endregion
");
			#endregion

			#endregion

			//todo TableFunctions

			#region Footer

			sb.Append(@"
	}
}
");
			return sb.ToString();

			#endregion
		}
	}
}
