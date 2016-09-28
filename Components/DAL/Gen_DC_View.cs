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
    public static class Gen_DC_View
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

            #region Views

            #region Header

            sb.Append(@"
		#region Views
");

        #endregion

            foreach (View v in uvs)
            {
                #region Header

                string tbn = Utils.GetEscapeName(v);

                List<Column> pks = Utils.GetPrimaryKeyColumns(v);
                List<Column> socs = Utils.GetSortableColumns(v);
                Dictionary<Column, Column> pfcs = new Dictionary<Column, Column>();
                if (Utils.GetBaseTable(v) != null) pfcs = Utils.GetTreePKFKColumns(Utils.GetBaseTable(v));


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
			/// TSQL 格式： SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_GetCount()
			{
				if (_count_cmd != null) return _count_cmd.Clone();
                lock(_count_cmd_sync)
                {
    				_count_cmd = new SqlCommand(""SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]"");
	    			return _count_cmd.Clone();
                }
			}
");
                #endregion

                #region GetCount_Custom

                sb.Append(@"
			/// <summary>
			/// 返回 根据条件查询数据条数 Command 对象
			/// TSQL 格式： SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
			/// </summary>
			public static SqlCommand NewCmd_GetCount_Custom(string where)
			{
				return new SqlCommand(""SELECT COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]"" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
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
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_Select()
			{
				if (_select_cmd != null) return _select_cmd.Clone();
                lock(_select_cmd_sync)
                {
				    _select_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE ");
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
                            sb.Append(@"
				    _select_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + c.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
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
			/// TSQL 格式： SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE ({传入的条件})
			/// </summary>
			public static SqlCommand NewCmd_Select_Custom(string where)
			{
				return new SqlCommand(""SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]"" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
			}
");

                #endregion


                #region SelectAll

                sb.Append(@"
			private static SqlCommand _selectall_cmd = null;
            private static object _selectall_cmd_sync = new object();
			/// <summary>
			/// 返回 查询所有数据 Command 对象
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_SelectAll()
			{
				if (_selectall_cmd != null) return _selectall_cmd.Clone();
                lock(_selectall_cmd_sync)
                {
				    _selectall_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]"");
				    return _selectall_cmd.Clone();
                }
			}
");

                #endregion

                #region SelectAll_Custom


                sb.Append(@"
			/// <summary>
			/// 返回传入部分 TSQL 的 查询所有数据 Command 对象
			/// TSQL 格式： SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] {1}
			/// </summary>
			public static SqlCommand NewCmd_SelectAll_Custom(string s1, string s2)
			{
				return new SqlCommand(""SELECT "" + s1 + @"" * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]"" + s2);
			}
");

                #endregion


                #region SelectPart


                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 返回 查询单条数据 部分字段的 Command 对象
			/// TSQL 格式： SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE (主键s = ...)
			/// </summary>
			public static SqlCommand NewCmd_SelectPart(params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				SqlCommand cmd = new SqlCommand();
				StringBuilder sb = new StringBuilder(""SELECT "");
");
                    foreach (Column c in v.Columns)
                    {
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
                    }
                    sb.Append(@"
				sb.Append(@"" FROM " + v.ToString() + @" WHERE ");
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
                        sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + c.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
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
			/// TSQL 格式： SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE ({传入的条件})
			/// </summary>
			public static SqlCommand NewCmd_SelectPart_Custom(string where, params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				SqlCommand cmd = new SqlCommand();
				StringBuilder sb = new StringBuilder(""SELECT TOP 1 "");
");
                foreach (Column c in v.Columns)
                {
                    string cn = Utils.GetEscapeName(c);
                    sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
                }
                sb.Append(@"
				sb.Append(@"" FROM " + v.ToString() + @""" + (string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where)));
				cmd.CommandText = sb.ToString();
				return cmd;
			}
");

                #endregion


                #region SelectAllPart

                sb.Append(@"
			/// <summary>
			/// 返回 查询所有数据的部分字段的 Command 对象
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPart(params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder(""SELECT "");
");
                foreach (Column c in v.Columns)
                {
                    string cn = Utils.GetEscapeName(c);
                    sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
                }
                sb.Append(@"
				sb.Append(@"" FROM " + v.ToString() + @""");
				return new SqlCommand(sb.ToString());
			}
");

                #endregion

                #region SelectAllPart_Custom


                sb.Append(@"
			/// <summary>
			/// 返回传入部分 TSQL 的 查询所有数据的部分字段的 Command 对象
			/// TSQL 格式： SELECT {0} __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] {1}
			/// </summary>
			public static SqlCommand NewCmd_SelectAllPart_Custom(string s1, string s2, params DI." + tbn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				StringBuilder sb = new StringBuilder(""SELECT "" + s1 + "" "");
");
                foreach (Column c in v.Columns)
                {
                    string cn = Utils.GetEscapeName(c);
                    sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tbn + @"." + cn + @")
					sb.Append((idx++ == 0 ? """" : "", "") + ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"");
");
                }
                sb.Append(@"
				sb.Append(@"" FROM " + v.ToString() + @" "" + s2);
				return new SqlCommand(sb.ToString());
			}
");

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
                        sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE (");
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
                        sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + Utils.GetEscapeName(c));
                    }
                    sb.Append(@")
	UNION ALL
	SELECT ");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        if (i > 0) sb.Append(@", ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] A JOIN T ON ");
                    isFirst = true;
                    foreach (KeyValuePair<Column, Column> kv in pfcs)
                    {
                        if (!isFirst) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] = T.[" + Utils.GetEscapeName(kv.Key.Name) + "]");
                        isFirst = false;
                    }
                    sb.Append(@"
)
SELECT A.* FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] A JOIN T ON ");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        if (i > 0) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = T.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@"
"");");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				    _selectnode_cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + c.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
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
                    foreach (Column c in v.Columns)
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
                    sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE (");
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
                        sb.Append("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + Utils.GetEscapeName(c));
                    }
                    sb.Append(@")
	UNION ALL
	SELECT ");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        if (i > 0) sb.Append(@", ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@" FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] A JOIN T ON ");
                    isFirst = true;
                    foreach (KeyValuePair<Column, Column> kv in pfcs)
                    {
                        if (!isFirst) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(kv.Value.Name) + "] = T.[" + Utils.GetEscapeName(kv.Key.Name) + "]");
                        isFirst = false;
                    }
                    sb.Append(@"
)
SELECT "" + sb.ToString() + @"" FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] A JOIN T ON ");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        if (i > 0) sb.Append(@" AND ");
                        sb.Append("A.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = T.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@"
"");");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + cn + @""", " + Utils.GetSqlDbType(c) + @", " + c.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + c.DataType.NumericPrecision.ToString() + @", " + c.DataType.NumericScale.ToString() + @", """ + cn + @""", DataRowVersion.Current, null));");
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
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
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
				for (int i = 0; i < v.Columns.Count; i++)
				{
					Column c = v.Columns[i];
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
				for (int i = 0; i < v.Columns.Count; i++)
				{
					Column c = v.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append((i > 0 ? ", " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@""");
				sb.Append(@"" FROM (SELECT ");
				for (int i = 0; i < v.Columns.Count; i++)
				{
					Column c = v.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append((i > 0 ? ", " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@", ROW_NUMBER() OVER (ORDER BY "");
				sb.Append(sb_obs);
				sb.Append(@"") as __RowNumber FROM " + ("[" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]") + @" "" + where);
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
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
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
				foreach (Column c in v.Columns)
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
				sb.Append(@"") as __RowNumber FROM " + ("[" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]") + @" "" + where);
				sb.Append(@"") a WHERE __RowNumber BETWEEN @__RN_BEGIN AND @__RN_END ORDER BY __RowNumber"");
				SqlCommand cmd = new SqlCommand(sb.ToString());
				cmd.Parameters.Add(new SqlParameter(""__RN_BEGIN"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				cmd.Parameters.Add(new SqlParameter(""__RN_END"", System.Data.SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, ""__RN_BEGIN"", DataRowVersion.Current, null));
				return cmd;
			}
");

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