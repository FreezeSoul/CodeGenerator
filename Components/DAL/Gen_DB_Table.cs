using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;
using System.Diagnostics;

namespace CodeGenerator.Components.DAL
{
    /// <summary>
    /// 生成与 Data Command 生成物一一对应的调用方法
    /// todo: CheckExists,  Merge
    /// </summary>
    public static class Gen_DB_Table
    {

        public static string Gen(Database db)
        {
            return Gen(db, "DAL", "DS", "DS2");
        }

        public static string Gen(Database db, string ns, string dsn, string dsn2)
        {
            #region Header

            Server server = db.Parent;
            string s = "";
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
	public static partial class DB
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
		[System.ComponentModel.DataObjectAttribute(true)]
		public static partial class " + tn + @"
		{
");
                #endregion


                #region GetCount

                #region Header
                sb.Append(@"
			#region GetCount
");
            #endregion


                #region GetCount

                sb.Append(@"
			/// <summary>
			/// 返回一个表的数据行数
			/// </summary>
			public static int GetCount()
			{
				object o = SQLHelper.ExecuteScalar(DC." + tn + @".NewCmd_GetCount());
				if (o == DBNull.Value) return 0;
				return (int)o;
			}
		");

                #endregion

                #region GetCount_Custom

                sb.Append(@"
			/// <summary>
			/// 返回一个表的数据行数，根据条件 TSQL 字串
			/// </summary>
			public static int GetCount_Custom(string where)
			{
				object o = SQLHelper.ExecuteScalar(DC." + tn + @".NewCmd_GetCount_Custom(where));
				if (o == DBNull.Value) return 0;
				return (int)o;
			}

			/// <summary>
			/// 返回一个表的数据行数，带条件
			/// </summary>
			public static int GetCount_Custom(OE." + tn + @" exp)
			{
				object o = SQLHelper.ExecuteScalar(DC." + tn + @".NewCmd_GetCount_Custom(exp == null ? """" : exp.ToString()));
				if (o == DBNull.Value) return 0;
				return (int)o;
			}
		");

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Fill

                #region Header
                sb.Append(@"
			#region Fill
");
            #endregion


                #region Fill

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的Row的主键s，查找返回一行数据并填充传入Row。未找到则返回 false
			/// </summary>
			public static bool Fill(" + dsn + "." + tn + @"Row r)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = r." + cn + @";");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0)
				{
					r.ItemArray = dt.Rows[0].ItemArray;
					return true;
				}
				return false;
			}
		");
                }

                #endregion

                #region FillPart

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，查找返回一行数据 的一部分（视传入字段列表而定 ）填充传入 row。未找到则返回 null
			/// </summary>
			public static bool SelectPart(" + dsn + "." + tn + @"Row r, params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart(__cols);");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = r." + cn + @";");
                    }
                    sb.Append(@"
				DataTable dt = SQLHelper.ExecuteDataTable(cmd);
				if (dt.Rows.Count > 0)
				{
					DataRow row = dt.Rows[0];
                    int idx = 0;
");
                    foreach (Column c in t.Columns)
                    {
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
					if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                    {
                        r[""" + cn + @"""] = row[""" + cn + @"""];
                        idx++;
                    }
");
                    }
                    sb.Append(@"
					return true;
				}
				return false;
			}
		");
                }

                #endregion


                #region FillAll

                sb.Append(@"
			/// <summary>
			/// 返回一个表的所有数据并填充传入DataTable，返回受影响行数
			/// </summary>
			public static int FillAll(" + dsn + @"." + tn + @"DataTable dt)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll());
			}");

                #endregion

                #region FillAll_Custom

                sb.Append(@"
			/// <summary>
			/// 返回一个表的所有数据（传入局部 TSQL ）并填充传入DataTable，返回受影响行数
			/// TSQL 格式： SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, string s1, string s2)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom(s1, s2));
			}

			/// <summary>
			/// 返回一个表的所有数据（传入 TSQL WHERE 部分）并填充传入DataTable，返回受影响行数
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, string where)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom("""", "" WHERE "" + where));
			}

			/// <summary>
			/// 返回一个表的所有数据（传入 TSQL WHERE 部分的表达式对象）并填充传入DataTable，返回受影响行数
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, OE." + tn + @" exp)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : (""""))));
			}
");

                #endregion

                #region FillAll_By_ForeignKeys

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

                        sb.Append(@"
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据并填充传入DataTable，返回受影响行数
			/// </summary>
			public static int FillAll_By_" + s + @"(" + dsn + @"." + tn + @"DataTable dt, ");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll_By_" + s + @"();");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }

                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteDataTable(dt, cmd);
			}
		");
                    }
                }

                #endregion


                #region never finish yet

                #region FillAllPart

                #endregion

                #region FillAllPart_Custom

                #endregion

                #region FillAllPart_By_ForeignKeys

                #endregion


                #region FillNode

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
                {
                    // todo
                }

                #endregion

                #region FillNodePart

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
                {
                    // todo
                }

                #endregion


                #region FillAllPage

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion

                #region FillAllPage_Custom

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion

                #region FillAllPage_By_ForeignKeys

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion


                #region FillAllPartPage

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion

                #region FillAllPartPage_Custom

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion

                #region FillAllPartPage_By_ForeignKeys

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    // todo
                }

                #endregion

                #endregion

                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Select Single Line

                #region Header
                sb.Append(@"
			#region Select Single Line
");
            #endregion


                #region Select

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，查找返回一行数据。未找到则返回 null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"Row Select(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}
		");
                }

                #endregion

                #region Select_Custom

                sb.Append(@"
			/// <summary>
			/// 根据传入的 TSQL WHERE，查找返回一行数据。未找到则返回 null
			/// TSQL 格式： SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"Row Select_Custom(string where)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select_Custom(where);
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}

			/// <summary>
			/// 根据传入的 WHERE 部分的表达式对象，查找返回一行数据。未找到则返回 null
			/// TSQL 格式： SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"Row Select_Custom(OE." + tn + @" exp)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select_Custom(exp.ToString());
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}
");

                #endregion

                #region SelectPart

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，查找返回一行数据 的一部分（视传入字段列表而定 ）。未找到则返回 null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataRow SelectPart(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@", params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart(__cols);");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				DataTable dt = SQLHelper.ExecuteDataTable(cmd);
				if (dt.Rows.Count > 0) return dt.Rows[0];
				return null;
			}
		");
                }

                #endregion

                #region SelectPart_Custom

                sb.Append(@"
			/// <summary>
			/// 根据传入的 TSQL WHERE，查找返回一行数据 的一部分（视传入字段列表而定 ）。未找到则返回 null
			/// TSQL 格式： SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataRow SelectPart_Custom(string where, params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart_Custom(where, __cols);
				DataTable dt = SQLHelper.ExecuteDataTable(cmd);
				if (dt.Rows.Count > 0) return dt.Rows[0];
				return null;
			}

			/// <summary>
			/// 根据传入的 WHERE 部分的表达式对象，查找返回一行数据 的一部分（视传入字段列表而定 ）。未找到则返回 null
			/// TSQL 格式： SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp, params DI." + tn + @"[] __cols}
			/// </summary>
			public static DataRow SelectPart_Custom(OE." + tn + @" exp, params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart_Custom(exp.ToString(), __cols);
				DataTable dt = SQLHelper.ExecuteDataTable(cmd);
				if (dt.Rows.Count > 0) return dt.Rows[0];
				return null;
			}
");

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Select Multiply Line

                #region Header
                sb.Append(@"
			#region Select Multiply Line
");
            #endregion


                #region SelectAll

                sb.Append(@"
			/// <summary>
			/// 返回一个表的所有数据
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll()
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll();
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}
			/// <summary>
			/// 返回一个表的所有数据 带排序
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll(DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")));
			}
			/// <summary>
			/// 返回一个表的所有数据 带排序，限定行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll(DI." + tn + @"SortDictionary sd, int top)
			{
				return SelectAll_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")));
			}
");

                #endregion

                #region SelectAll_Custom

                sb.Append(@"
			/// <summary>
			/// 返回一个表的所有数据（传入局部 TSQL ）
			/// TSQL 格式： SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(string s1, string s2)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll_Custom(s1, s2);
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}

			/// <summary>
			/// 返回一个表的所有数据（传入 TSQL WHERE 部分）
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(string where)
			{
				return SelectAll_Custom("""", "" WHERE "" + where);
			}

			/// <summary>
			/// 返回一个表的所有数据（传入 TSQL WHERE 部分的表达式对象）
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")));
			}

			/// <summary>
			/// 返回一个表的所有数据 带排序（传入 TSQL WHERE 部分的表达式对象）
			/// TSQL 格式： SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}

			/// <summary>
			/// 返回一个表的所有数据 带排序，行数限定（传入 TSQL WHERE 部分的表达式对象）
			/// TSQL 格式： SELECT TOP top * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top)
			{
				return SelectAll_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
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

                        sb.Append(@"
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_By_" + s + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll_By_" + s + @"();");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }

                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据，带额外条件，排序，限定行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_By_" + s + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@", OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top)
			{
				OE." + tn + @" oexp = new OE." + tn + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            string quote = Utils.CheckNeedQuote(c) ? "'" : "";
                            if (i > 0) sb.Append(@" + "" AND "" + ");
                            if (c.Nullable)
                            {
                                if (Utils.CheckIsStringType(c))
                                {
                                    sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @"""))");
                                }
                                else
                                {
                                    sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @"""))");
                                }
                            }
                            else
                            {
                                if (Utils.CheckIsStringType(c))
                                {
                                    sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @""")");
                                }
                                else
                                {
                                    sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @""")");
                                }
                            }
                        }
                        sb.Append(@");
				return SelectAll_Custom(oexp.And(exp), sd, top);
			}
");
                    }
                }

                #endregion


                #region SelectAllPart

                sb.Append(@"
			/// <summary>
			/// 返回一个表的所有数据的部分字段
			/// TSQL 格式： SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart(params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart(__cols));
			}

			/// <summary>
			/// 返回一个表的所有数据的部分字段 带排序
			/// TSQL 格式： SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ORDER BY {.........}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart(DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}

			/// <summary>
			/// 返回一个表的所有数据的部分字段 带排序，行数限定
			/// TSQL 格式： SELECT TOP top __cols..... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ORDER BY {.........}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart(DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}
");

                #endregion

                #region SelectAllPart_Custom

                sb.Append(@"
			/// <summary>
			/// 根据传入的部分 TSQL 返回一个表的所有数据的部分字段
			/// TSQL 格式： SELECT {0} __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(string s1, string s2, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom(s1, s2, __cols));
			}
			/// <summary>
			/// 根据传入的 WHERE 部分 TSQL 返回一个表的所有数据的部分字段
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(string where, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", "" WHERE "" + where, __cols));
			}
			/// <summary>
			/// 根据传入的 WHERE 部分 表达式对象 返回一个表的所有数据的部分字段
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")), __cols));
			}

			/// <summary>
			/// 根据传入的 WHERE 部分 表达式对象 返回一个表的所有数据的部分字段 带排序
			/// TSQL 格式： SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}

			/// <summary>
			/// 根据传入的 WHERE 部分 表达式对象 返回一个表的所有数据的部分字段 带排序，行数限定
			/// TSQL 格式： SELECT TOP top __cols... FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols));
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

                        sb.Append(@"
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据的部分字段
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_By_" + s + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@", params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPart_By_" + s + @"(__cols);");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }

                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteDataTable(cmd);
			}

			/// <summary>
			/// 根据传入的外键s，查找返回一组数据的部分字段，带条件，排序，行数限定
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_By_" + s + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@", OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				OE." + tn + @" oexp = new OE." + tn + @"(");
                        for (int i = 0; i < fk.Columns.Count; i++)
                        {
                            Column c = t.Columns[fk.Columns[i].Name];
                            string cn = Utils.GetEscapeName(c);
                            string quote = Utils.CheckNeedQuote(c) ? "'" : "";
                            if (i > 0) sb.Append(@" + "" AND "" + ");
                            if (c.Nullable)
                            {
                                if (Utils.CheckIsStringType(c))
                                {
                                    sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @"""))");
                                }
                                else
                                {
                                    sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @"""))");
                                }
                            }
                            else
                            {
                                if (Utils.CheckIsStringType(c))
                                {
                                    sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @""")");
                                }
                                else
                                {
                                    sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @""")");
                                }
                            }
                        }
                        sb.Append(@");
				return SelectAllPart_Custom(oexp.And(exp), sd, top, __cols);
			}
");
                    }
                }

                #endregion


                #region SelectNode

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，查找返回一个节点的数据。传入空则返回所有父节点为空的节点数据。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectNode(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectNode();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}
		");
                }

                #endregion

                #region SelectNodePart

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，查找返回一个节点部分字段的数据。传入空则返回所有父节点为空的节点数据。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectNodePart(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@", params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectNodePart(__cols);");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteDataTable(cmd);
			}
		");
                }

                #endregion



                #region SelectAllPage_Custom

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    sb.Append(@"
			/// <summary>
			/// 返回一个表的多行数据（传入限定,条件TSQL）（带分页排序）
			/// TSQL 格式： 
			/// SELECT {所有字段列表}
			/// FROM (SELECT {prefix} {所有字段列表}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAllPage_Custom(string prefix, string where, DI." + tn + @"SortDictionary sortDict, int rowIndexStart, int pageSize)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPage_Custom(prefix, string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where), sortDict);
				cmd.Parameters[""__RN_BEGIN""].Value = rowIndexStart + 1;
				cmd.Parameters[""__RN_END""].Value = rowIndexStart + pageSize;
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}
			/// <summary>
			/// 返回一个表的多行数据（传入条件）（对象只含部分字段）（带分页单字段排序）
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAllPage_Custom(OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize)
			{
                return SelectAllPage_Custom("""", (exp == null ? """" : exp.ToString()), new DI." + tn + @"SortDictionary(sortCol, isAsc), rowIndexStart, pageSize);
            }
");
                }

                #endregion

                #region SelectAllPage_By_ForeignKeys

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {

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


                            sb.Append(@"
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据的部分字段，带条件，排序，分页
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAllPage_By_" + s + @"(");
                            for (int i = 0; i < fk.Columns.Count; i++)
                            {
                                Column c = t.Columns[fk.Columns[i].Name];
                                string cn = Utils.GetEscapeName(c);
                                if (i > 0) sb.Append(@", ");
                                if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                                else sb.Append(Utils.GetDataType(c) + " " + cn);
                            }
                            sb.Append(@", OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize)
			{
				OE." + tn + @" oexp = new OE." + tn + @"(");
                            for (int i = 0; i < fk.Columns.Count; i++)
                            {
                                Column c = t.Columns[fk.Columns[i].Name];
                                string cn = Utils.GetEscapeName(c);
                                string quote = Utils.CheckNeedQuote(c) ? "'" : "";
                                if (i > 0) sb.Append(@" + "" AND "" + ");
                                if (c.Nullable)
                                {
                                    if (Utils.CheckIsStringType(c))
                                    {
                                        sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @"""))");
                                    }
                                    else
                                    {
                                        sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @"""))");
                                    }
                                }
                                else
                                {
                                    if (Utils.CheckIsStringType(c))
                                    {
                                        sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @""")");
                                    }
                                    else
                                    {
                                        sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @""")");
                                    }
                                }
                            }
                            sb.Append(@");
				return SelectAllPage_Custom("""", (exp == null ? oexp.ToString() : oexp.And(exp).ToString()), new DI." + tn + @"SortDictionary(sortCol, isAsc), rowIndexStart, pageSize);
			}
");
                        }
                    }
                }

                #endregion


                #region SelectAllPartPage_Custom

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {
                    sb.Append(@"
			/// <summary>
			/// 返回一个表的多行数据（传入限定,条件TSQL）（对象只含部分字段）（带分页排序）
			/// TSQL 格式： 
			/// SELECT {__cols}
			/// FROM (SELECT {prefix} {__cols}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPartPage_Custom(string prefix, string where, DI." + tn + @"SortDictionary sortDict, int rowIndexStart, int pageSize, params DI." + tn + @"[] __cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPartPage_Custom(prefix, string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where), sortDict, __cols);
				cmd.Parameters[""__RN_BEGIN""].Value = rowIndexStart + 1;
				cmd.Parameters[""__RN_END""].Value = rowIndexStart + pageSize;
                return SQLHelper.ExecuteDataTable(cmd);
			}
			/// <summary>
			/// 返回一个表的多行数据（传入条件）（对象只含部分字段）（带分页单字段排序）
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPartPage_Custom(OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize, params DI." + tn + @"[] __cols)
			{
                return SelectAllPartPage_Custom("""", (exp == null ? """" : exp.ToString()), new DI." + tn + @"SortDictionary(sortCol, isAsc), rowIndexStart, pageSize, __cols);
            }
");
                }

                #endregion

                #region SelectAllPartPage_By_ForeignKeys

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
                {

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


                            sb.Append(@"
			/// <summary>
			/// 根据传入的外键s，查找返回一组数据的部分字段，带条件，排序，分页
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPartPage_By_" + s + @"(");
                            for (int i = 0; i < fk.Columns.Count; i++)
                            {
                                Column c = t.Columns[fk.Columns[i].Name];
                                string cn = Utils.GetEscapeName(c);
                                if (i > 0) sb.Append(@", ");
                                if (c.Nullable) sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                                else sb.Append(Utils.GetDataType(c) + " " + cn);
                            }
                            sb.Append(@", OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize, params DI." + tn + @"[] __cols)
			{
				OE." + tn + @" oexp = new OE." + tn + @"(");
                            for (int i = 0; i < fk.Columns.Count; i++)
                            {
                                Column c = t.Columns[fk.Columns[i].Name];
                                string cn = Utils.GetEscapeName(c);
                                string quote = Utils.CheckNeedQuote(c) ? "'" : "";
                                if (i > 0) sb.Append(@" + "" AND "" + ");
                                if (c.Nullable)
                                {
                                    if (Utils.CheckIsStringType(c))
                                    {
                                        sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @"""))");
                                    }
                                    else
                                    {
                                        sb.Append(@"(" + cn + @" == null ? ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"" : (""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @"""))");
                                    }
                                }
                                else
                                {
                                    if (Utils.CheckIsStringType(c))
                                    {
                                        sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + SQLHelper.EscapeEqual(" + cn + @") + """ + quote + @""")");
                                    }
                                    else
                                    {
                                        sb.Append(@"(""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = " + quote + @""" + " + cn + @".ToString() + """ + quote + @""")");
                                    }
                                }
                            }
                            sb.Append(@");
				return SelectAllPartPage_Custom("""", oexp.And(exp).ToString(), new DI." + tn + @"SortDictionary(sortCol, isAsc), rowIndexStart, pageSize, __cols);
			}
");
                        }
                    }
                }

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Delete

                #region Header
                sb.Append(@"
			#region Delete
");
            #endregion


                #region Delete

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，删除一行数据。返回受影响行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int Delete(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Delete();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// 根据传入的 Original_主键s，删除一行数据。返回受影响行数（主要为 ObjectDataSource 服务）
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
			public static int Delete(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " Original_" + cn);
                    }
                    sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Delete();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = Original_" + cn + @";");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// 根据传入的数据行对象，删除一行数据。返回受影响行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int Delete(" + dsn + "." + tn + @"Row r)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Delete();");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = r[""" + cn + @""", DataRowVersion.Original];");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}");

                    //看哪些视图以当前表作为“基表”
                    foreach (View v in uvs)
                    {
                        if (Utils.CheckIsBaseTable(v, t))
                        {
                            string vn = Utils.GetEscapeName(v);
                            sb.Append(@"
			/// <summary>
			/// 根据传入的数据行对象，删除一行数据。返回受影响行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int Delete(" + dsn + "." + vn + @"Row r)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Delete();");
                            for (int i = 0; i < pks.Count; i++)
                            {
                                Column c = pks[i];
                                string cn = Utils.GetEscapeName(c);
                                sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = r[""" + cn + @""", DataRowVersion.Original];");
                            }
                            sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}");

                        }
                    }
                }

                #endregion

                #region DeleteAll_Custom

                sb.Append(@"
			/// <summary>
			/// 根据传入的 TSQL WHERE 删除数行记录
			/// TSQL 格式： DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteAll_Custom(string where)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteAll_Custom("""", string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where));
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// 根据传入的 TSQL WHERE 部分的表达式对象 删除数行记录
			/// TSQL 格式： DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteAll_Custom(OE." + tn + @" exp)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")));
				return SQLHelper.ExecuteNonQuery(cmd);
			}
");

                #endregion


                #region DeleteNode

                if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
                {
                    if (pks.Count > 0)
                    {
                        sb.Append(@"
			/// <summary>
			/// 根据传入的主键s，删除一个节点的数据。返回受影响行数
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteNode(");
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteNode();");
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// 根据传入的 Original_主键s，删除一个节点的数据。返回受影响行数（主要服务于 ObjectDataSource)
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteNode(");
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@", ");
                            sb.Append(Utils.GetDataType(c) + " Original_" + cn);
                        }
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteNode();");
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = Original_" + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}
");
                    }
                }

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Insert

                #region Header
                sb.Append(@"
			#region Insert
");
            #endregion


                #region Insert

                if (wcs.Count > 0)
                {
                    if (pks.Count > 0)
                    {
                        sb.Append(@"
			/// <summary>
			/// 根据传入的字段值添加到数据库并返回 Row 对象。失败返回 null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
			public static " + dsn + @"." + tn + @"Row Insert(");
                        s = "";
                        foreach (Column c in wcs)
                        {
                            if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                            string cn = Utils.GetEscapeName(c);
                            if (s.Length > 0) s += @", ";
                            if (c.Nullable) s += Utils.GetNullableDataType(c) + " " + cn;
                            else s += Utils.GetDataType(c) + " " + cn;
                        }
                        sb.Append(s);
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Insert();");
                        foreach (Column c in wcs)
                        {
                            if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }
                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}

			/// <summary>
			/// 根据传入的 Row 添加到数据库，刷新原 Row 并返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int Insert(" + dsn + "." + tn + @"Row r)
			{
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				dt.LoadDataRow(r.ItemArray, true).SetAdded();
				int i = SQLHelper.UpdateData(dt, DC." + tn + @".NewCmd_Insert(), null, null, null);
				if (i > 0)
				{
					r.EnforceReadOnly(false);
					r.ItemArray = dt.Rows[0].ItemArray;
					r.EnforceReadOnly(false);
					if (r.RowState != DataRowState.Detached) r.AcceptChanges();
				}
				return i;
			}");


                        //看哪些视图以当前表作为“基表”
                        foreach (View v in uvs)
                        {
                            if (Utils.CheckIsBaseTable(v, t))
                            {
                                string vn = Utils.GetEscapeName(v);
                                sb.Append(@"
			/// <summary>
			/// 根据传入的 Row 添加到数据库，返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int Insert(" + dsn + "." + vn + @"Row r)
			{
				" + dsn + @"." + vn + @"DataTable dt = new " + dsn + @"." + vn + @"DataTable();
				dt.ImportRow(r);
				dt[0].SetAdded();
				return SQLHelper.UpdateData(dt, DC." + tn + @".NewCmd_Insert(), null, null, null);
			}");

                            }
                        }

                    }
                    else
                    {
                        sb.Append(@"
			/// <summary>
			/// 根据传入的字段值添加到数据库。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int Insert(");
                        s = "";
                        foreach (Column c in wcs)
                        {
                            if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                            string cn = Utils.GetEscapeName(c);
                            if (s.Length > 0) s += @", ";
                            if (c.Nullable) s += Utils.GetNullableDataType(c) + " " + cn;
                            else s += Utils.GetDataType(c) + " " + cn;
                        }
                        sb.Append(s);
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Insert();");
                        foreach (Column c in wcs)
                        {
                            if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }

                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}");
                    }

                }

                #endregion

                #region InsertPart

                if (pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的字段部分键值对添加到数据库并返回 Row 对象。失败返回 null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static " + dsn + @"." + tn + @"Row InsertPart(Dictionary<DI." + tn + @", object> cvs)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_InsertPart(cvs);");
                    foreach (Column c in wcs)
                    {
                        if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (cvs.ContainsKey(DI." + tn + @"." + cn + @"))
				{");
                        if (c.Nullable)
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
                        }
                        sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = cvs[DI." + tn + @"." + cn + @"];
				}
");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}

			/// <summary>
			/// 根据传入的行和对应的更新字段列表添加到数据库并返回 Row 对象。失败返回 null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static " + dsn + @"." + tn + @"Row InsertPart(" + dsn + @"." + tn + @"Row row, params DI." + tn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				SqlCommand cmd = DC." + tn + @".NewCmd_InsertPart(__cols);");
                    foreach (Column c in wcs)
                    {
                        if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
				{");
                        if (c.Nullable)
                        {
                            sb.Append(@"
					if (row.Is" + cn + @"Null()) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
					if (row." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
                        }
                        sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = row." + cn + @";

                    idx++;
				}
");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				if (dt.Rows.Count > 0) return dt[0];
				return null;
			}

		");
                }
                else
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的字段部分键值对添加到数据库。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int InsertPart(Dictionary<DI." + tn + @", object> cvs)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_InsertPart(cvs);");
                    foreach (Column c in wcs)
                    {
                        if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (cvs.ContainsKey(DI." + tn + @"." + cn + @"))
				{");
                        if (c.Nullable)
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
                        }
                        sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = cvs[DI." + tn + @"." + cn + @"];
				}
");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// 根据传入的行和对应的更新字段列表添加到数据库。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int InsertPart(" + dsn + @"." + tn + @"Row row, params DI." + tn + @"[] __cols)
			{
				Array.Sort(__cols);
                int idx = 0;
				SqlCommand cmd = DC." + tn + @".NewCmd_InsertPart(__cols);");
                    foreach (Column c in wcs)
                    {
                        if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
				{");
                        if (c.Nullable)
                        {
                            sb.Append(@"
					if (row.Is" + cn + @"Null()) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
					if (row." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
                        }
                        sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = row." + cn + @";

                    idx++;
				}
");
                    }
                    sb.Append(@"
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				return SQLHelper.ExecuteNonQuery(cmd);
			}

		");
                }

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion

                #region Update

                #region Header
                sb.Append(@"
			#region Update
");
            #endregion


                #region Update

                if (wcs.Count > 0 && pks.Count > 0)
                {
                    sb.Append(@"
			/// <summary>
			/// 根据传入的字段值，更新一行数据。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int Update(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        sb.Append(Utils.GetDataType(c) + " Original_" + cn);
                    }
                    for (int i = 0; i < wcs.Count; i++)
                    {
                        Column c = wcs[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@", ");
                        if (c.Nullable)
                            sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                        else
                            sb.Append(Utils.GetDataType(c) + " " + cn);
                    }
                    sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Update();");
                    foreach (Column c in pks)
                    {
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = Original_" + cn + @";");
                    }
                    foreach (Column c in wcs)
                    {
                        string cn = Utils.GetEscapeName(c);
                        if (c.Nullable)
                        {
                            sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                        }
                        sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}
");
                    bool hasWriteablePK = false;
                    foreach (Column c in pks)
                    {
                        if (wcs.Contains(c))
                        {
                            hasWriteablePK = true;
                            break;
                        }
                    }
                    if (hasWriteablePK)
                    {
                        sb.Append(@"

			/// <summary>
			/// 根据传入的字段值，更新一行数据。返回受影响行数。（主键不可更改型，主要为 ObjectDataSource 服务）	// todo 刷新原数据行
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
			public static int Update(");
                        for (int i = 0; i < pks.Count; i++)
                        {
                            Column c = pks[i];
                            string cn = Utils.GetEscapeName(c);
                            if (i > 0) sb.Append(@", ");
                            sb.Append(Utils.GetDataType(c) + " Original_" + cn);
                        }
                        for (int i = 0; i < wcs.Count; i++)
                        {
                            Column c = wcs[i];
                            if (pks.Contains(c)) continue;

                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@", ");
                            if (c.Nullable)
                                sb.Append(Utils.GetNullableDataType(c) + " " + cn);
                            else
                                sb.Append(Utils.GetDataType(c) + " " + cn);
                        }
                        sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Update();");
                        foreach (Column c in pks)
                        {
                            string cn = Utils.GetEscapeName(c);
                            sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = Original_" + cn + @";
				cmd.Parameters[""" + cn + @"""].Value = Original_" + cn + @";");
                        }
                        foreach (Column c in wcs)
                        {
                            if (pks.Contains(c)) continue;
                            string cn = Utils.GetEscapeName(c);
                            if (c.Nullable)
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
                            }
                            else if (Utils.CheckIsStringType(c))
                            {
                                sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
                            }
                            sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
                        }
                        sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}
");
                    }
                    sb.Append(@"

			/// <summary>
			/// 根据传入的字段列表，更新一行数据。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int Update(" + dsn + @"." + tn + @"Row r)
			{
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				" + dsn + @"." + tn + @"Row dr = (" + dsn + @"." + tn + @"Row)dt.LoadDataRow(r.ItemArray_Original, true);
				dr.EnforceReadOnly(false);
				dr.ItemArray = r.ItemArray;
				dr.EnforceReadOnly(true);
				int i = SQLHelper.UpdateData(dr, null, null, DC." + tn + @".NewCmd_Update(), null);
				r.EnforceReadOnly(false);
				r.ItemArray = dr.ItemArray;
				r.EnforceReadOnly(true);
				if (r.RowState != DataRowState.Detached) r.AcceptChanges();
				return i;
			}");


                    //看哪些视图以当前表作为“基表”
                    foreach (View v in uvs)
                    {
                        string vn = Utils.GetEscapeName(v);
                        if (Utils.CheckIsBaseTable(v, t))
                        {
                            sb.Append(@"

			/// <summary>
			/// 根据传入的 Row 更新一行数据。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int Update(" + dsn + @"." + vn + @"Row r)
			{
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				dt.ImportRow(r);
				dt[0].SetModified();
				return SQLHelper.UpdateData(dt, null, null, DC." + tn + @".NewCmd_Update(), null);
			}");

                        }
                    }
                }

                #endregion

                #region UpdatePart

                if (wcs.Count > 0 && pks.Count > 0)
                {
                    sb.Append(@"

			/// <summary>
			/// 根据传入的字段列表，选择性局部更新一行数据。返回受影响行数。
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int UpdatePart(");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(@", ");
                        if (c.Nullable)
                            sb.Append(Utils.GetNullableDataType(c) + " Original_" + cn);
                        else
                            sb.Append(Utils.GetDataType(c) + " Original_" + cn);
                    }
                    sb.Append(@", Dictionary<DI." + tn + @", object> cvs)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_UpdatePart(cvs);");
                    foreach (Column c in pks)
                    {
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = Original_" + cn + @";");
                    }
                    foreach (Column c in wcs)
                    {
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
				if (cvs.ContainsKey(DI." + tn + @"." + cn + @"))
				{");
                        if (c.Nullable)
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
                        }
                        else if (Utils.CheckIsStringType(c))
                        {
                            sb.Append(@"
					if (cvs[DI." + tn + @"." + cn + @"] == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
                        }

                        sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = cvs[DI." + tn + @"." + cn + @"];
				}");
                    }
                    sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}


			/// <summary>
			/// 根据传入的字段列表，选择性局部更新一行数据
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int UpdatePart(" + dsn + @"." + tn + @"Row r, params DI." + tn + @"[] cols)
			{
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				" + dsn + @"." + tn + @"Row dr = (" + dsn + @"." + tn + @"Row)dt.LoadDataRow(r.ItemArray_Original, true);
				dr.EnforceReadOnly(false);
				dr.ItemArray = r.ItemArray;
				dr.EnforceReadOnly(true);
				int i = SQLHelper.UpdateData(dr, null, null, DC." + tn + @".NewCmd_UpdatePart(cols), null);
				r.EnforceReadOnly(false);
				r.ItemArray = dr.ItemArray;
				r.EnforceReadOnly(true);
				if (r.RowState != DataRowState.Detached) r.AcceptChanges();
				return i;
			}
		");
                }

                #endregion


                #region Footer
                sb.Append(@"
			#endregion
");
                #endregion

                #endregion


                #region Footer

                sb.Append(@"
		}
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
