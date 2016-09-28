using System;
using System.Collections.Generic;
using System.Text;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;
using System.Diagnostics;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// Data Command 类生成器。根据数据库的结构生成相应的 Command 对象
	/// todo: 对无主键表的判断和处理
	/// </summary>
	public static class Gen_DC_Table
	{
		public static string Gen(Database db, string ns)
		{
			#region Header

			Server server = db.Parent;
			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<StoredProcedure> sps = Utils.GetUserStoredProcedures(db);
			List<UserDefinedFunction> ufs = Utils.GetUserFunctions(db);
			string s = "";

			StringBuilder sb = new StringBuilder();

			sb.Append(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace " + ns + @"
{
	public static partial class DC
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
				List<Column> socs = Utils.GetSortableColumns(t);
				Dictionary<Column, Column> pfcs = Utils.GetTreePKFKColumns(t);

				string tbn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tbn + @"

		public static partial class " + tbn + @"
		{
");
		#endregion


				#region GetCount

				sb.Append(@"
			private static SqlCommand _count_cmd = null;
            private static object _count_cmd_sync = new object();
			/// <summary>
			/// 返回 查询数据条数 Command 对象
			/// TSQL 格式： SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_GetCount()
			{
				if (_count_cmd != null) return _count_cmd.Clone();
                lock(_count_cmd_sync)
                {
				    _count_cmd = new SqlCommand(""SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]"");
				    return _count_cmd.Clone();
                }
			}
");
				#endregion

				#region GetCount_Custom

				sb.Append(@"
			/// <summary>
			/// 返回 根据条件查询数据条数 Command 对象
			/// TSQL 格式： SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			public static SqlCommand NewCmd_GetCount_Custom(string where)
			{
				return new SqlCommand(""SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]"" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
			}
");
				#endregion


				#region Select


				if (pks.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _select_cmd = null;
            private static object _select_cmd_sync = new object();
			/// <summary>
			/// 返回 查询单条数据 Command 对象
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_Select()
			{
				if (_select_cmd != null) return _select_cmd.Clone();
                lock(_select_cmd_sync)
                {
				    _select_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ");
					    for (int i = 0; i < pks.Count; i++)
					    {
						    Column c = pks[i];
						    string cn = Utils.GetEscapeName(c);
						    if (i > 0) sb.Append(@" AND ");
						    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
					    }
					    sb.Append(@""");");
					    for (int i = 0; i < pks.Count; i++)
					    {
						    Column c = pks[i];
						    string cn = Utils.GetEscapeName(c);
                            string len = c.DataType.NumericScale.ToString();
                            if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                            sb.Append(@"
				    _select_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					    }
					    sb.Append(@"
				    return _select_cmd.Clone();
                }
			}
");
				}

				#endregion

				#region Select_Custom


				sb.Append(@"
			/// <summary>
			/// 返回 查询单条数据 Command 对象
			/// TSQL 格式： SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ({传入的条件})
			/// </summary>
			public static SqlCommand NewCmd_Select_Custom(string where)
			{
				return new SqlCommand(""SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]"" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
			}
");

				#endregion

				#region SelectPart


				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// 返回 查询单条数据 部分字段的 Command 对象
			/// TSQL 格式： SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_SelectPart(params DI." + tbn + @"[] __cols)
			{
                Array.Sort(__cols);
                int idx = 0;
				SqlCommand cmd = new SqlCommand();
				StringBuilder sb = new StringBuilder(""SELECT "");
");
					foreach (Column c in t.Columns)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
                    sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
					}
					sb.Append(@"
				sb.Append(@"" FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @" WHERE ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" AND ");
						sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
					}
					sb.Append(@""");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0"; 
						sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					}
					sb.Append(@"
				cmd.CommandText = sb.ToString();
				return cmd;
			}
");
				}

				#endregion

				#region SelectPart_Custom


				sb.Append(@"
			/// <summary>
			/// 返回 查询单条数据 部分字段的 Command 对象
			/// TSQL 格式： SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ({传入的条件})
			/// </summary>
			public static SqlCommand NewCmd_SelectPart_Custom(string where, params DI." + tbn + @"[] __cols)
			{
				SqlCommand cmd = new SqlCommand();
				StringBuilder sb = new StringBuilder(""SELECT TOP 1 "");
                Array.Sort(__cols);
                int idx = 0;
");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
				}
				sb.Append(@"
				sb.Append(@"" FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @""" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
				cmd.CommandText = sb.ToString();
				return cmd;
			}
");

				#endregion


				#region SelectAll


				sb.Append(@"
			private static SqlCommand _selectall_cmd = null;
            private static object _selectall_cmd_sync = new object();
			/// <summary>
			/// 返回 查询所有数据 Command 对象
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_SelectAll()
			{
				if (_selectall_cmd != null) return _selectall_cmd.Clone();
                lock(_selectall_cmd_sync)
                {
				    _selectall_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]"");
				    return _selectall_cmd.Clone();
                }
			}
");

				#endregion

				#region SelectAll_Custom


				sb.Append(@"
			/// <summary>
			/// 返回传入部分 TSQL 的 查询所有数据 Command 对象
			/// TSQL 格式： SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			public static SqlCommand NewCmd_SelectAll_Custom(string s1, string s2)
			{
				return new SqlCommand(""SELECT "" + s1 + @"" * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] "" + s2);
			}
");

				#endregion

				#region SelectAll_By_ForeignKeys

				if (t.ForeignKeys.Count > 0)
				{
					List<string> gened = new List<string>();
					foreach (ForeignKey fk in t.ForeignKeys)
					{
						s = "";
						for (int i = 0; i < fk.Columns.Count; i++)
						{
							if (i > 0) s += "_";
							s += Utils.GetEscapeName(fk.Columns[i]);
						}

						if (gened.Contains(s)) continue;
						gened.Add(s);

						string fkn = Utils.GetEscapeName(fk);

						sb.Append(@"
			private static SqlCommand _selectall_by_" + fkn + @"_cmd = null;
            private static object _selectall_by_" + fkn + @"_cmd_sync = new object();
			/// <summary>
			/// 返回 查询所有数据 Command 对象
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (外键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_SelectAll_By_" + s + @"()
			{
				if (_selectall_by_" + fkn + @"_cmd != null) return _selectall_by_" + fkn + @"_cmd.Clone();
                lock(_selectall_by_" + fkn + @"_cmd_sync)
                {
				    _selectall_by_" + fkn + @"_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (");
						    for (int i = 0; i < fk.Columns.Count; i++)
						    {
							    Column c = t.Columns[fk.Columns[i].Name];
							    string cn = Utils.GetEscapeName(c);
							    if (i > 0) sb.Append(@" AND ");
							    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
						    }
						    if (t.Columns[fk.Columns[0].Name].Nullable)
						    {
							    sb.Append(@") OR (");
							    for (int i = 0; i < fk.Columns.Count; i++)
							    {
								    Column c = t.Columns[fk.Columns[i].Name];
								    string cn = Utils.GetEscapeName(c);
								    if (i > 0) sb.Append(@" AND ");
								    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL AND @" + cn + @" IS NULL");
							    }
							    sb.Append(@")"");");
						    }
						    else sb.Append(@")"");");
						    for (int i = 0; i < fk.Columns.Count; i++)
						    {
							    Column c = t.Columns[fk.Columns[i].Name];
							    string cn = Utils.GetEscapeName(c);
                                string len = c.DataType.NumericScale.ToString();
                                if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                                sb.Append(@"
				    _selectall_by_" + fkn + @"_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
						    }
						    sb.Append(@"
				    return _selectall_by_" + fkn + @"_cmd.Clone();
                }
			}
");
					}
				}

				#endregion


				#region SelectAllPart

				sb.Append(@"
			/// <summary>
			/// 返回 查询所有数据的部分字段的 Command 对象
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPart(params DI." + tbn + @"[] __cols)
			{
                Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder(""SELECT "");
");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
				}
				sb.Append(@"
				sb.Append(@"" FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @""");
				return new SqlCommand(sb.ToString());
			}
");

				#endregion

				#region SelectAllPart_Custom


				sb.Append(@"
			/// <summary>
			/// 返回传入部分 TSQL 的 查询所有数据的部分字段的 Command 对象
			/// TSQL 格式： SELECT {0} __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPart_Custom(string s1, string s2, params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder(""SELECT "" + s1 + "" "");
");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
				}
				sb.Append(@"
				sb.Append(@"" FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @" "" + s2);
				return new SqlCommand(sb.ToString());
			}
");

				#endregion

				#region SelectAllPart_By_ForeignKeys

				if (t.ForeignKeys.Count > 0)
				{
					List<string> gened = new List<string>();
					foreach (ForeignKey fk in t.ForeignKeys)
					{
						s = "";
						for (int i = 0; i < fk.Columns.Count; i++)
						{
							if (i > 0) s += "_";
							s += Utils.GetEscapeName(fk.Columns[i]);
						}

						if (gened.Contains(s)) continue;
						gened.Add(s);

						string fkn = Utils.GetEscapeName(fk);

						sb.Append(@"
			/// <summary>
			/// 返回 查询所有数据 Command 对象
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (外键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPart_By_" + s + @"(params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder(""SELECT "");
");
						foreach (Column c in t.Columns)
						{
							string cn = Utils.GetEscapeName(c);
							sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
						}
						sb.Append(@"
				sb.Append(@"" FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @" WHERE (");
						for (int i = 0; i < fk.Columns.Count; i++)
						{
							Column c = t.Columns[fk.Columns[i].Name];
							string cn = Utils.GetEscapeName(c);
							if (i > 0) sb.Append(@" AND ");
							sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
						}
						if (t.Columns[fk.Columns[0].Name].Nullable)
						{
							sb.Append(@") OR (");
							for (int i = 0; i < fk.Columns.Count; i++)
							{
								Column c = t.Columns[fk.Columns[i].Name];
								string cn = Utils.GetEscapeName(c);
								if (i > 0) sb.Append(@" AND ");
								sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL AND @" + cn + @" IS NULL");
							}
							sb.Append(@")"");");
						}
						else sb.Append(@")"");");
						sb.Append(@"
				SqlCommand cmd = new SqlCommand(sb.ToString());");
						for (int i = 0; i < fk.Columns.Count; i++)
						{
							Column c = t.Columns[fk.Columns[i].Name];
							string cn = Utils.GetEscapeName(c);
                            string len = c.DataType.NumericScale.ToString();
                            if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                            sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
						}
						sb.Append(@"
				return cmd;
			}
");
					}
				}

				#endregion


				#region SelectNode

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _selectnode_cmd = null;
            private static object _selectnode_cmd_sync = new object();
			/// <summary>
			/// 返回 查询节点数据的 Command 对象
			/// TSQL 格式： 
			/// WITH T AS (
			///		SELECT 主键s FROM 表 WHERE (外键s IS NULL AND @主键s IS NULL) OR (主键s = @主键s)
			///		UNION ALL
			///		SELECT 主键s FROM 表 JOIN T ON 表.外键s = T.主键s
			///	)
			/// SELECT 表.* FROM 表 JOIN T ON 表.主键s = T.主键s
			/// </summary>
			public static SqlCommand NewCmd_SelectNode()
			{
				if (_selectnode_cmd != null) return _selectnode_cmd.Clone();
                lock(_selectnode_cmd_sync)
                {
				    _selectnode_cmd = new SqlCommand(@""
WITH T AS (
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (");
					bool isFirst = true;
					foreach (KeyValuePair<Column, Column> kv in pfcs)
					{
						if (!isFirst) sb.Append(@" AND ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] IS NULL AND @" + Utils.GetEscapeName(kv.Key) + " IS NULL");
						isFirst = false;
					}
					sb.Append(@") OR (");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@" AND ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + "] = @" + Utils.GetEscapeName(c));
					}
					sb.Append(@")
	UNION ALL
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] A JOIN T ON ");
					isFirst = true;
					foreach (KeyValuePair<Column, Column> kv in pfcs)
					{
						if (!isFirst) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] = T.[" + Utils.GetEscapeSqlObjectName(kv.Key.Name) + "]");
						isFirst = false;
					}
					sb.Append(@"
)
SELECT A.* FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] A JOIN T ON ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + "] = T.[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@"
"");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				    _selectnode_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					}
					sb.Append(@"
				    return _selectnode_cmd.Clone();
                }
			}
");
				}

				#endregion

				#region SelectNodePart

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// 返回 查询节点数据部分字段的 Command 对象
			/// TSQL 格式： 
			/// WITH T AS (
			///		SELECT 主键s FROM 表 WHERE (外键s IS NULL AND @主键s IS NULL) OR (主键s = @主键s)
			///		UNION ALL
			///		SELECT 主键s FROM 表 JOIN T ON 表.外键s = T.主键s
			///	)
			/// SELECT 表.__cols FROM 表 JOIN T ON 表.主键s = T.主键s
			/// </summary>
			public static SqlCommand NewCmd_SelectNodePart(params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder();
");
					foreach (Column c in t.Columns)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""A.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
					}
					sb.Append(@"
				if (sb.Length == 0) sb.Append(""*"");
				SqlCommand cmd = new SqlCommand(@""
WITH T AS (
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (");
					bool isFirst = true;
					foreach (KeyValuePair<Column, Column> kv in pfcs)
					{
						if (!isFirst) sb.Append(@" AND ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] IS NULL AND @" + Utils.GetEscapeName(kv.Key) + " IS NULL");
						isFirst = false;
					}
					sb.Append(@") OR (");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@" AND ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + "] = @" + Utils.GetEscapeName(c));
					}
					sb.Append(@")
	UNION ALL
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] A JOIN T ON ");
					isFirst = true;
					foreach (KeyValuePair<Column, Column> kv in pfcs)
					{
						if (!isFirst) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] = T.[" + Utils.GetEscapeSqlObjectName(kv.Key.Name) + "]");
						isFirst = false;
					}
					sb.Append(@"
)
SELECT "" + sb.ToString() + @"" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] A JOIN T ON ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + "] = T.[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@"
"");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					}
					sb.Append(@"
				return cmd;
			}
");
				}

				#endregion


				#region SelectAllPage_Custom


				sb.Append(@"
			/// <summary>
			/// 返回 传入条件查询所有数据带分页排序的 Command 对象
			/// TSQL 格式： 
			/// SELECT {所有字段列表}
			/// FROM (SELECT {prefix} {{所有字段列表}}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPage_Custom(string prefix, string where, DI." + tbn + @"SortDictionary sortDict)
			{
				bool isFirstOB = true;
				StringBuilder sb_obs = new StringBuilder();
");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				if (sortDict.ContainsKey(DI." + tbn + @"." + cn + @"))
				{
					sb_obs.Append((isFirstOB ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (sortDict[DI." + tbn + @"." + cn + @"] ? """" : "" DESC""));
					isFirstOB = false;
				}
");
				}
				sb.Append(@"
				StringBuilder sb = new StringBuilder(""SELECT "" + prefix + "" ");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append((i > 0 ? ", " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@""");
				sb.Append(@"" FROM (SELECT ");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append((i > 0 ? ", " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@", ROW_NUMBER() OVER (ORDER BY "");
				sb.Append(sb_obs);
				sb.Append(@"") as __RowNumber FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @" "" + where);
				sb.Append(@"") a WHERE __RowNumber BETWEEN @__RN_BEGIN AND @__RN_END ORDER BY __RowNumber"");
				SqlCommand cmd = new SqlCommand(sb.ToString());
				cmd.Parameters.Add(new SqlParameter(""__RN_BEGIN"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				cmd.Parameters.Add(new SqlParameter(""__RN_END"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				return cmd;
			}
");

				#endregion

				#region SelectAllPartPage_Custom


				sb.Append(@"
			/// <summary>
			/// 返回 传入条件查询所有数据部分字段带分页排序的 Command 对象
			/// TSQL 格式： 
			/// SELECT {__cols}
			/// FROM (SELECT {prefix} {__cols}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPartPage_Custom(string prefix, string where, DI." + tbn + @"SortDictionary sortDict, params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				bool isFirstOB = true;
				StringBuilder sb_cols = new StringBuilder();
				StringBuilder sb_obs = new StringBuilder();
");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb_cols.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
				if (sortDict.ContainsKey(DI." + tbn + @"." + cn + @"))
				{
					sb_obs.Append((isFirstOB ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (sortDict[DI." + tbn + @"." + cn + @"] ? """" : "" DESC""));
					isFirstOB = false;
				}
");
				}
				sb.Append(@"
				StringBuilder sb = new StringBuilder(""SELECT "" + prefix + "" "");
				sb.Append(sb_cols);
				sb.Append(@"" FROM (SELECT "");
				sb.Append(sb_cols);
				sb.Append(@"", ROW_NUMBER() OVER (ORDER BY "");
				sb.Append(sb_obs);
				sb.Append(@"") as __RowNumber FROM " + ("[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]") + @" "" + where);
				sb.Append(@"") a WHERE __RowNumber BETWEEN @__RN_BEGIN AND @__RN_END ORDER BY __RowNumber"");
				SqlCommand cmd = new SqlCommand(sb.ToString());
				cmd.Parameters.Add(new SqlParameter(""__RN_BEGIN"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				cmd.Parameters.Add(new SqlParameter(""__RN_END"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				return cmd;
			}
");

				#endregion


				#region Delete


				if (pks.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _delete_cmd = null;
            private static object _delete_cmd_sync = new object();
			/// <summary>
			/// 返回 删除单条数据 Command 对象
			/// TSQL 格式： DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_Delete()
			{
				if (_delete_cmd != null) return _delete_cmd.Clone();
                lock(_delete_cmd_sync)
                {
    				_delete_cmd = new SqlCommand(""DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" AND ");
						sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
					}
					sb.Append(@""");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
	    			_delete_cmd.Parameters.Add(new SqlParameter(""Original_" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Original, null));");
					}
					sb.Append(@"
		    		return _delete_cmd.Clone();
                }
			}
");
				}

				#endregion

				#region DeleteAll_Custom



				sb.Append(@"
			/// <summary>
			/// 返回传入部分 TSQL 的 删除所有数据 Command 对象
			/// TSQL 格式： DELETE {0} FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			public static SqlCommand NewCmd_DeleteAll_Custom(string s1, string s2)
			{
				return new SqlCommand(""DELETE "" + s1 + @"" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]"" + s2);
			}
");
				#endregion

				#region DeleteNode

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _deletenode_cmd = null;
            private static object _deletenode_cmd_sync = new object();
			/// <summary>
			/// 返回 删除整个节点数据的 Command 对象
			/// TSQL 格式： 
			/// WITH T AS (
			///		SELECT 主键s FROM 表 WHERE 主键s = @主键s
			///		UNION ALL
			///		SELECT 主键s FROM 表 JOIN T ON 表.外键s = T.主键s
			///	)
			/// DELETE FROM 表 WHERE 主键s IN (SELECT 主键s FROM T)
			/// </summary>
			public static SqlCommand NewCmd_DeleteNode()
			{
				if (_selectnode_cmd != null) return _deletenode_cmd.Clone();
                lock(_deletenode_cmd_sync)
                {
    				_deletenode_cmd = new SqlCommand(@""
WITH T AS (
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE (");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@" AND ");
						sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + Utils.GetEscapeName(c));
					}
					sb.Append(@")
	UNION ALL
	SELECT ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						if (i > 0) sb.Append(@", ");
						sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + "]");
					}
					sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] A JOIN T ON ");
					bool isFirst = true;
					foreach (KeyValuePair<Column, Column> kv in pfcs)
					{
						if (!isFirst) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] = T.[" + Utils.GetEscapeSqlObjectName(kv.Key.Name) + "]");
						isFirst = false;
					}
					sb.Append(@"
)
DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
WHERE ");
					if (pks.Count > 1)
					{
						sb.Append(@"EXISTS (SELECT 1 FROM T WHERE ");
						for (int i = 0; i < pks.Count; i++)
						{
							Column c = pks[i];
							if (i > 0) sb.Append(@" AND ");
							sb.Append(@"[" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"].[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = Node.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
						}
						sb.Append(@")");
					}
					else
					{
                        sb.Append(@"[" + Utils.GetEscapeSqlObjectName(pks[0].Name) + @"] IN (SELECT [" + Utils.GetEscapeSqlObjectName(pks[0].Name) + @"] FROM T)");
					}
					sb.Append(@"
"");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				    _deletenode_cmd.Parameters.Add(new SqlParameter(""Original_" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Original, null));");
					}
					sb.Append(@"
				    return _deletenode_cmd.Clone();
                }
			}
");
				}

				#endregion


				#region Insert


				if (wcs.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _insert_cmd = null;
            private static object _insert_cmd_sync = new object();
			/// <summary>
			/// 返回 插入单条数据 Command 对象
			/// TSQL 格式： INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (...所有可写字段...) VALUES (.......); SELECT .... FROM ... WHERE 主键s = @主键s
			/// </summary>
			public static SqlCommand NewCmd_Insert()
			{
				if (_insert_cmd != null) return _insert_cmd.Clone();
                lock(_insert_cmd_sync)
                {
				    _insert_cmd = new SqlCommand(""");
                    if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && t.Triggers.Count == 0)
					{
                        foreach (Column c in pks)
                        {
                            if (c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null)
                            {
                                string cn = Utils.GetEscapeName(c);
                                sb.Append(@"DECLARE @" + cn + @" uniqueidentifier;SET @" + cn + @" = newid();");
                            }
                        }
						sb.Append(@"INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (");
						for (int i = 0; i < wcs.Count; i++)
						{
							Column c = wcs[i];
							sb.Append((i > 0 ? ", " : "") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
						}
						sb.Append(@") OUTPUT Inserted.* VALUES (");
						for (int i = 0; i < wcs.Count; i++)
						{
							Column c = wcs[i];
							string cn = Utils.GetEscapeName(c);
							sb.Append((i > 0 ? ", " : "") + "@" + cn);
						}
						sb.Append(@");");
					}
					else
					{
						foreach (Column c in pks)
						{
							if (c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null)
							{
								string cn = Utils.GetEscapeName(c);
								sb.Append(@"DECLARE @" + cn + @" uniqueidentifier;SET @" + cn + @" = newid();");
							}
						}
						sb.Append(@"INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (");
						for (int i = 0; i < wcs.Count; i++)
						{
							Column c = wcs[i];
							sb.Append((i > 0 ? ", " : "") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
						}
						sb.Append(@") VALUES (");
						for (int i = 0; i < wcs.Count; i++)
						{
							Column c = wcs[i];
							string cn = Utils.GetEscapeName(c);
							sb.Append((i > 0 ? ", " : "") + "@" + cn);
						}
						sb.Append(@");");
						if (pks.Count > 0)
						{
							sb.Append(@" SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ");
							for (int i = 0; i < pks.Count; i++)
							{
								Column c = pks[i];
								string cn = Utils.GetEscapeName(c);
								if (i > 0) sb.Append(@" AND ");
								if (c.Identity)
								{
									sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = SCOPE_IDENTITY()");
								}
								else sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
							}
						}
					}
					sb.Append(@""");");
					foreach (Column c in wcs)
					{
						if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				    _insert_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					}
					sb.Append(@"
				    return _insert_cmd.Clone();
                }
			}
");
				}

				#endregion

				#region InsertPart


				sb.Append(@"
			/// <summary>
			/// 返回传入字段列表 的 个别字段插入 Command 对象
			/// TSQL 格式： INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (...传入字段s...) VALUES (...传入值s...); SELECT .... FROM ... WHERE 主键s = @主键s
			/// </summary>
			public static SqlCommand NewCmd_InsertPart(IList<DI." + tbn + @"> cols)
			{
				bool isFirst = true;
				SqlCommand cmd = new SqlCommand();
				StringBuilder sb = new StringBuilder(""");

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && t.Triggers.Count == 0)
				{
                    s = "";
                    foreach (Column c in pks)
                    {
                        if (c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null)
                        {
                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@"DECLARE @" + cn + @" uniqueidentifier;SET @" + cn + @" = newid();");

                            s += @"
				sb.Append((isFirst ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
				sb2.Append((isFirst ? """" : "", "") + ""@" + cn + @""");
				isFirst = false;
";
                        }
                    }

					sb.Append(@"INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ("");
				StringBuilder sb2 = new StringBuilder();");

                    sb.Append(s);

					foreach (Column c in wcs)
					{
                        if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				if (cols.Contains(DI." + tbn + @"." + cn + @"))
				{
					cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));
					sb.Append((isFirst ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
					sb2.Append((isFirst ? """" : "", "") + ""@" + cn + @""");
					isFirst = false;
				}");
					}
					sb.Append(@"
				sb.Append("") OUTPUT INSERTED.* VALUES ("");
				sb.Append(sb2);
				sb.Append(@"");"");");

				}
				else
				{

					s = "";
					foreach (Column c in pks)
					{
						if (c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null)
						{
							string cn = Utils.GetEscapeName(c);
							sb.Append(@"DECLARE @" + cn + @" uniqueidentifier;SET @" + cn + @" = newid();");

							s += @"
				sb.Append((isFirst ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
				sb2.Append((isFirst ? """" : "", "") + ""@" + cn + @""");
				isFirst = false;
";
						}
					}
					sb.Append(@"INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ("");
				StringBuilder sb2 = new StringBuilder();");

					sb.Append(s);

					foreach (Column c in wcs)
					{
						if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				if (cols.Contains(DI." + tbn + @"." + cn + @"))
				{
					cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));
					sb.Append((isFirst ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
					sb2.Append((isFirst ? """" : "", "") + ""@" + cn + @""");
					isFirst = false;
				}");
					}
					sb.Append(@"
				sb.Append("") VALUES ("");
				sb.Append(sb2);
				sb.Append(@"");"");");
					if (pks.Count > 0)
					{
						sb.Append(@"
				sb.Append(@"" SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ");
						for (int i = 0; i < pks.Count; i++)
						{
							Column c = pks[i];
							string cn = Utils.GetEscapeName(c);
							if (i > 0) sb.Append(@" AND ");
							if (c.Identity)
							{
								sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = SCOPE_IDENTITY()");
							}
							else sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
						}
						sb.Append(@";"");");
					}

				}


				sb.Append(@"
				cmd.CommandText = sb.ToString();
				return cmd;
			}
			public static SqlCommand NewCmd_InsertPart(IDictionary<DI." + tbn + @", object> cvs)
			{
				List<DI." + tbn + @"> cols = new List<DI." + tbn + @">(cvs.Keys);
				return NewCmd_InsertPart(cols);
			}
			public static SqlCommand NewCmd_InsertPart(params DI." + tbn + @"[] cols)
			{
				List<DI." + tbn + @"> collist = new List<DI." + tbn + @">(cols);
				return NewCmd_InsertPart(collist);
			}

	");

				#endregion


				#region Update


				if (wcs.Count > 0 && pks.Count > 0)
				{
					sb.Append(@"
			private static SqlCommand _update_cmd = null;
            private static object _update_cmd_sync = new object();
			/// <summary>
			/// 返回 所有可更新字段更新 Command 对象
			/// TSQL 格式： UPDATE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] SET 字段s = ... WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_Update()
			{
				if (_update_cmd != null) return _update_cmd.Clone();
                lock(_update_cmd_sync)
                {
				    _update_cmd = new SqlCommand(""UPDATE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] SET ");
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append((i > 0 ? ", " : "") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
					}
                    if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && t.Triggers.Count == 0)
						sb.Append(@" OUTPUT Inserted.* WHERE ");
					else
						sb.Append(@" WHERE ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" AND ");
						sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
					}
					if (db.CompatibilityLevel < CompatibilityLevel.Version90 || t.Triggers.Count > 0)
					{
						sb.Append(@"; SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE ");
						for (int i = 0; i < pks.Count; i++)
						{
							Column c = pks[i];
							string cn = Utils.GetEscapeName(c);
							if (i > 0) sb.Append(@" AND ");
							if (wcs.Contains(c)) sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
							else sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
						}
					}
					sb.Append(@""");");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				    _update_cmd.Parameters.Add(new SqlParameter(""Original_" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Original, null));");
					    }
					    foreach (Column c in wcs)
					    {
						    string cn = Utils.GetEscapeName(c);
                            string len = c.DataType.NumericScale.ToString();
                            if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                            sb.Append(@"
				    _update_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					    }
					    sb.Append(@"
				    return _update_cmd.Clone();
                }
			}
	");
				}

				#endregion

				#region UpdatePart



				if (wcs.Count > 0 && pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// 返回传入字段列表 的 个别字段更新 Command 对象
			/// TSQL 格式： UPDATE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] SET 传入字段s = ... WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_UpdatePart(IList<DI." + tbn + @"> cols)
			{
				bool isFirst = true;
				StringBuilder sb = new StringBuilder(""UPDATE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] SET "");");
					foreach (Column c in wcs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (cols.Contains(DI." + tbn + @"." + cn + @"))
				{
					sb.Append((isFirst ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn + @""");
					isFirst = false;
				}");
					}
                    if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && t.Triggers.Count == 0)
						sb.Append(@"
				sb.Append(@"" OUTPUT Inserted.* WHERE ");
					else
						sb.Append(@"
				sb.Append(@"" WHERE ");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" AND ");
						sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
					}
					if (db.CompatibilityLevel < CompatibilityLevel.Version90 || t.Triggers.Count > 0)
					{
						s = "";
						for (int i = 0; i < pks.Count; i++)
						{
							Column c = pks[i];
							string cn = Utils.GetEscapeName(c);
							if (i > 0) s += " AND ";
							if (wcs.Contains(c))
							{
								s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = "" + (cols.Contains(DI." + tbn + @"." + cn + @") ? ""@" + cn + @""" : ""@Original_" + cn + @""") + """;
							}
							else
							{
								s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn;
							}
						}
						if (s.Length > 0)
						{
							sb.Append(@"; SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE " + s);
						}
					}
					sb.Append(@";"");

				SqlCommand cmd = new SqlCommand(sb.ToString());");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""Original_" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Original, null));");
					}
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
                        string len = c.DataType.NumericScale.ToString();
                        if (c.DataType.SqlDataType == SqlDataType.Image) len = "0";
                        sb.Append(@"
				if (cols.Contains(DI." + tbn + @"." + cn + @")) cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + len + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
					}
					sb.Append(@"
				return cmd;
			}
			public static SqlCommand NewCmd_UpdatePart(Dictionary<DI." + tbn + @", object> cvs)
			{
				List<DI." + tbn + @"> cols = new List<DI." + tbn + @">(cvs.Keys);
				return NewCmd_UpdatePart(cols);
			}
			public static SqlCommand NewCmd_UpdatePart(params DI." + tbn + @"[] cols)
			{
				List<DI." + tbn + @"> collist = new List<DI." + tbn + @">(cols);
				return NewCmd_UpdatePart(collist);
			}
	");
				}

				#endregion


				#region Footer

				sb.Append(@"
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