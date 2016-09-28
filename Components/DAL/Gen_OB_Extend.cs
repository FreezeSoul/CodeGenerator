using System;
using System.Collections.Generic;
using System.Text;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// Data Info 扩展方法生成器。为数据对象类 OO 实例附加 OB 里的部分静态操作方法
	/// </summary>
	public static class Gen_OB_Extend
	{
		public static string Gen(Database db, string ns)
		{
			Server server = db.Parent;
			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> utfs = Utils.GetUserFunctions_TableValue(db);
			List<StoredProcedure> usp = Utils.GetUserStoredProcedures(db);

			StringBuilder sb = new StringBuilder();

			#region Header
			sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace " + ns + @"
{
	/// <summary>
	/// 为数据对象类 DI 附加 OB 里的部分静态操作方法
	/// </summary>
	public static partial class OB_Extend
	{
");
			#endregion

			#region Footer
			sb.Append(@"
	}
}");
			#endregion

			return sb.ToString();
		}
	}
}
