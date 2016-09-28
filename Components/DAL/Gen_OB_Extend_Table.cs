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
	public static class Gen_OB_Extend_Table
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

			#region Table

			foreach (Table t in uts)
			{
				string tn = Utils.GetEscapeName(t);
				List<Column> pks = Utils.GetPrimaryKeyColumns(t);
				Dictionary<Column, Column> pfcs = Utils.GetTreePKFKColumns(t);

				sb.Append(@"
		#region " + tn + @"
");

				if (pks.Count > 0)
				{
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
		/// <summary>
		/// 将当前对象 添加到数据库，更新数据 并返回 受影响行数。
		/// </summary>
		public static int Insert(this OO." + tn + @" o)
		{
			return OB." + tn + @".Insert(o);
		}
		/// <summary>
		/// 将当前对象 部分字段 添加到数据库，更新数据 并返回 受影响行数。
		/// </summary>
		public static int InsertPart(this OO." + tn + @" o, params DI." + tn + @"[] __cols)
		{
			OO." + tn + @" o2 =  OB." + tn + @".InsertPart(o, __cols);
			if (o2 == null) return 0;
			o2.CopyTo(o);
			return 1;
		}
		/// <summary>
		/// 根据当前对象的字段值，更新一行数据并刷新对象。返回受影响行数。
		/// </summary>
		public static int Update(this OO." + tn + @" o)
		{
			return OB." + tn + @".Update(o);
		}
		/// <summary>
		/// 根据当前对象，更新一行数据并刷新对象。返回受影响行数。
		/// </summary>
		public static int UpdatePart(this OO." + tn + @" o, params DI." + tn + @"[] __cols)
		{
			return OB." + tn + @".UpdatePart(o, __cols);
		}

		/// <summary>
		/// 根据当前对象，新增或更新一行数据并刷新对象。返回受影响行数。
		/// </summary>
		public static int Merge(this OO." + tn + @" o)
		{
			// todo
			return 0;
		}
		/// <summary>
		/// 根据当前对象的主键，删除一行数据。返回受影响行数
		/// </summary>
		public static int Delete(this OO." + tn + @" o)
		{
			return OB." + tn + @".Delete(o);
		}
");
					if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
					{
						sb.Append(@"
		/// <summary>
		/// 根据当前对象的主键，删除一个节点的数据。返回受影响行数
		/// </summary>
		public static int DeleteNode(this OO." + tn + @" o)
		{
			return OB." + tn + @".DeleteNode(o);
		}
");
					}
				}
				else
				{
					sb.Append(@"
		/// <summary>
		/// 将当前对象 添加到数据库 并返回 受影响行数。
		/// </summary>
		public static int Insert(this OO." + tn + @" o)
		{
			return OB." + tn + @".Insert(o);
		}
		/// <summary>
		/// 将当前对象 部分字段 添加到数据库 并返回 受影响行数。
		/// </summary>
		public static int InsertPart(this OO." + tn + @" o, params DI." + tn + @"[] __cols)
		{
			return OB." + tn + @".InsertPart(o, __cols);
		}
");
				}

				sb.Append(@"
		#endregion
");
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
