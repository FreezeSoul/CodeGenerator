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
	/// 用户自定义类型脚本创建 （根据表或视图或表函数结构得到用户自定义表类型创建脚本）
	/// 生成规则：保留主键，去掉外键，无默认值，保留可空属性
	/// </summary>
	public static partial class Gen_UT
	{
		public static string Gen(Table t)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(@"
CREATE TYPE [" + Utils.GetEscapeSqlObjectName(t.Schema) + "].[" + Utils.GetEscapeSqlObjectName(t.Name) + "] AS TABLE");
			foreach (Column c in t.Columns)
			{

			}
			sb.Append(@"");

			return sb.ToString();
		}
		public static string Gen(View t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}
		public static string Gen(UserDefinedFunction t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}
		public static string Gen(UserDefinedTableType t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}

	}
}
