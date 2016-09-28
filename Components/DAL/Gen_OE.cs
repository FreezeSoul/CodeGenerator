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
	/// Data Info 类生成器。根据数据库的结构生成相应的 表，字段名枚举 对象
	/// </summary>
	public static class Gen_OE
	{
		public static string Gen(Database db, string ns)
		{
			#region Header

			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> utfs = Utils.GetUserFunctions_TableValue(db);

			StringBuilder sb = new StringBuilder();

			sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace " + ns + @"
{
	/// <summary>
	/// 查询表达式辅助创建类
	/// </summary>
	public static partial class OE
	{
		#region Base Expression Class

		/// <summary>
		/// 表达式基类
		/// </summary>
		public partial class __Exp
		{
			public object __Column;
			public object __Value;
			public SQLHelper.Operators __Operate;
			public bool __IsAndEffect;
			public List<__Exp> __Nodes = null;
			public __Exp And(__Exp subExp)
			{
				if (subExp == null) return this;
				__IsAndEffect = true;
				__Nodes.Add(subExp);
				return this;
			}
			public __Exp Or(__Exp subExp)
			{
				if (subExp == null) return this;
				__IsAndEffect = false;
				__Nodes.Add(subExp);
				return this;
			}

			public __Exp(object column, SQLHelper.Operators operate, object value)
			{
				__Column = column; __Value = value; __Operate = operate; __IsAndEffect = true;
				__Nodes = new List<__Exp>();
			}
		}

		#endregion
");
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
