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
	public static class Gen_OB_Extend_View
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
	public static partial class OB_Extend
	{
");
			#endregion

			#region View

			foreach (View v in uvs)
			{
				string tn = Utils.GetEscapeName(v);
				List<Column> pks = Utils.GetPrimaryKeyColumns(v);

				if (pks.Count > 0)
				{
					sb.Append(@"
		#region " + tn + @"
");

					sb.Append(@"
		/// <summary>
		/// 根据当前对象的主键值，查找并填充所有字段值。未找到则返回 false
		/// </summary>
		public static bool Fill(this OO." + tn + @" o)
		{
			return OB." + tn + @".Fill(o);
		}
		/// <summary>
		/// 根据当前对象的主键值，查找并填充部分字段值。未找到则返回 false
		/// </summary>
		public static bool FillPart(this OO." + tn + @" o, params DI." + tn + @"[] __cols)
		{
			return OB." + tn + @".FillPart(o, __cols);
		}
");

					sb.Append(@"
		#endregion
");
				}
			}

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
