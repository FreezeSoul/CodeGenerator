using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// 生成与 Data Command 生成物一一对应的调用方法
	/// todo: CheckExists,  Merge
	/// </summary>
	public static class Gen_OB
	{
		public static string Gen(Database db, string ns, string oon2)
		{
			#region Header

			Server server = db.Parent;
			string s = "", s1 = "";
			List<StoredProcedure> sps = Utils.GetUserStoredProcedures(db);
			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> ufs = Utils.GetUserFunctions(db);

			StringBuilder sb = new StringBuilder(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace " + ns + @"
{
	/// <summary>
	/// 基于 OO (类实例) 的数据操作静态方法集合类
	/// </summary>
	[System.ComponentModel.DataObjectAttribute(true)]
	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
	public static partial class OB
	{");
			#endregion

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
