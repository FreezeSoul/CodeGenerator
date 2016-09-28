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
	/// ������ Data Command ������һһ��Ӧ�ĵ��÷���
	/// todo: CheckExists,  Merge
	/// </summary>
	public static class Gen_DB_View
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
			/// ����һ�������������
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
			/// �������� TSQL �ִ�����һ�������������
			/// </summary>
			public static int GetCount_Custom(string where)
			{
				object o = SQLHelper.ExecuteScalar(DC." + tn + @".NewCmd_GetCount_Custom(where));
				if (o == DBNull.Value) return 0;
				return (int)o;
			}

			/// <summary>
			/// �����������ʽ���� ����һ�������������
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
			/// ���ݴ����Row������s�����ҷ���һ�����ݲ���䴫��Row��δ�ҵ��򷵻� false
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
					r.ItemArray = dt[0].ItemArray;
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
			/// ���ݴ��������s�����ҷ���һ������ ��һ���֣��Ӵ����ֶ��б���� ����䴫�� row��δ�ҵ��򷵻� null
			/// </summary>
			public static bool FillPart(" + dsn + "." + tn + @"Row r, params DI." + tn + @"[] __cols)
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
					foreach (Column c in v.Columns)
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
			/// ����һ������������ݲ���䴫��DataTable��������Ӱ������
			/// </summary>
			public static int FillAll(" + dsn + @"." + tn + @"DataTable dt)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll());
			}");

				#endregion

				#region FillAll_Custom

				sb.Append(@"
			/// <summary>
			/// ����һ������������ݣ�����ֲ� TSQL ������䴫��DataTable��������Ӱ������
			/// TSQL ��ʽ�� SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] {1}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, string s1, string s2)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom(s1, s2));
			}

			/// <summary>
			/// ����һ������������ݣ����� TSQL WHERE ���֣�����䴫��DataTable��������Ӱ������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, string where)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom("""", "" WHERE "" + where));
			}

			/// <summary>
			/// ����һ������������ݣ����� TSQL WHERE ���ֵı��ʽ���󣩲���䴫��DataTable��������Ӱ������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, OE." + tn + @" exp)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : (""""))));
			}

			/// <summary>
			/// ����һ����� top �����ݣ����� TSQL WHERE ���ֵı��ʽ���󣩲���䴫��DataTable��������Ӱ������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp}
			/// </summary>
			public static int FillAll_Custom(" + dsn + @"." + tn + @"DataTable dt, OE." + tn + @" exp, int top)
			{
				return SQLHelper.ExecuteDataTable(dt, DC." + tn + @".NewCmd_SelectAll_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : (""""))));
			}
");

				#endregion

				#region FillAll_By_ForeignKeys

				if (Utils.GetBaseTable(v) != null)
				{
					Table t = Utils.GetBaseTable(v);
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
			/// ���ݴ�������s�����ҷ���һ�����ݲ���䴫��DataTable��������Ӱ������
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
				return FillAll_Custom(dt, oexp);
			}
		");
						}
					}
				}

				#endregion



				#region FillAllPart

				#endregion

				#region FillAllPart_Custom

				#endregion

				#region FillAllPart_By_ForeignKeys

				#endregion


				#region FillNode

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && Utils.GetBaseTable(v) != null && Utils.CheckIsTree(Utils.GetBaseTable(v)))
				{

				}

				#endregion

				#region FillNodePart

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && Utils.GetBaseTable(v) != null && Utils.CheckIsTree(Utils.GetBaseTable(v)))
				{

				}

				#endregion


				#region FillAllPage

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

				#endregion

				#region FillAllPage_Custom

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

				#endregion

				#region FillAllPage_By_ForeignKeys

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

				#endregion


				#region FillAllPartPage

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

				#endregion

				#region FillAllPartPage_Custom

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

				#endregion

				#region FillAllPartPage_By_ForeignKeys

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{

				}

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
			/// ���ݴ��������s�����ҷ���һ�����ݡ�δ�ҵ��򷵻� null
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
					string vn = Utils.GetEscapeName(v);
					sb.Append(@"
				" + dsn + @"." + vn + @"DataTable dt = new " + dsn + @"." + vn + @"DataTable();
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
			/// ���ݴ���� TSQL WHERE�����ҷ���һ�����ݡ�δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
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
			/// ���ݴ���� TSQL WHERE ���ʽ���󣬲��ҷ���һ�����ݡ�δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp}
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
			/// ���ݴ��������s�����ҷ���һ������ ��һ���֣��Ӵ����ֶ��б���� ����δ�ҵ��򷵻� null
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
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart();");
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
			/// ���ݴ���� TSQL WHERE�����ҷ���һ������ ��һ���֣��Ӵ����ֶ��б���� ����δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
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
			/// ���ݴ���� WHERE ���ֵı��ʽ���󣬲��ҷ���һ������ ��һ���֣��Ӵ����ֶ��б���� ����δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp, params DI." + tn + @"[] __cols}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
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
			/// ����һ�������������
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
			/// ����һ������������� ������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll(DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ������������� �����������޶�
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
			/// ����һ����ͼ���������ݣ�����ֲ� TSQL ��
			/// TSQL ��ʽ�� SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] {1}
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
			/// ����һ����ͼ���������ݣ����� TSQL WHERE ���֣�
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(string where)
			{
				return SelectAll_Custom("""", "" WHERE "" + where);
			}

			/// <summary>
			/// ����һ����ͼ���������ݣ�������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ����ͼ���������ݣ�������������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ����ͼ���������ݣ��������������޶�����
			/// TSQL ��ʽ�� SELECT TOP top * FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top)
			{
				return SelectAll_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}
");

				#endregion

				#region SelectAll_By_ForeignKeys

				if (Utils.GetBaseTable(v) != null)
				{
					Table t = Utils.GetBaseTable(v);
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
			/// ���ݴ�������s�����ҷ���һ������
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
				return SelectAll_Custom(oexp);
			}
			/// <summary>
			/// ���ݴ�������s�����ҷ���һ�����ݣ������������������޶�����
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
				}

				#endregion


				#region SelectAllPart

				sb.Append(@"
			/// <summary>
			/// ����һ������������ݵĲ����ֶ�
			/// TSQL ��ʽ�� SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart(params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart(__cols));
			}

			/// <summary>
			/// ����һ������������ݵĲ����ֶ� ������
			/// TSQL ��ʽ�� SELECT __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart(DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}

			/// <summary>
			/// ����һ������������ݵĲ����ֶ� �������޶�����
			/// TSQL ��ʽ�� SELECT TOP top __cols..... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] ORDER BY {sd}
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
			/// ���ݴ���Ĳ��� TSQL ����һ������������ݵĲ����ֶ�
			/// TSQL ��ʽ�� SELECT {0} __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] {1}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(string s1, string s2, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom(s1, s2, __cols));
			}
			/// <summary>
			/// ���ݴ���� WHERE ���� TSQL ����һ������������ݵĲ����ֶ�
			/// TSQL ��ʽ�� SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(string where, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", "" WHERE "" + where, __cols));
			}
			/// <summary>
			/// ����һ������������ݵĲ����ֶΣ�������
			/// TSQL ��ʽ�� SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")), __cols));
			}
			/// <summary>
			/// ����һ������������ݵĲ����ֶΣ�������������
			/// TSQL ��ʽ�� SELECT __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}
			/// <summary>
			/// ����һ������������ݵĲ����ֶΣ������������������޶�
			/// TSQL ��ʽ�� SELECT TOP top __cols... FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static DataTable SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				return SQLHelper.ExecuteDataTable(DC." + tn + @".NewCmd_SelectAllPart_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols));
			}
");

				#endregion

				#region SelectAllPart_By_ForeignKeys

				if (Utils.GetBaseTable(v) != null)
				{
					Table t = Utils.GetBaseTable(v);
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
			/// ���ݴ�������s�����ҷ���һ������������ݵĲ����ֶ�
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
				return SelectAllPart_Custom(oexp, __cols);
			}
			/// <summary>
			/// ���ݴ�������s�����ҷ���һ������������ݵĲ����ֶΣ������������������޶�����
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
				}

				#endregion


				#region SelectNode

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ��������s�����ҷ���һ���ڵ�����ݡ�������򷵻����и��ڵ�Ϊ�յĽڵ����ݡ�
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
			/// ���ݴ��������s�����ҷ���һ���ڵ㲿���ֶε����ݡ�������򷵻����и��ڵ�Ϊ�յĽڵ����ݡ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static " + dsn + @"." + tn + @"DataTable SelectNodePart(");
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
				" + dsn + @"." + tn + @"DataTable dt = new " + dsn + @"." + tn + @"DataTable();
				SQLHelper.ExecuteDataTable(dt, cmd);
				return dt;
			}
		");
				}

				#endregion


				#region SelectAllPage_Custom

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{
					sb.Append(@"
			/// <summary>
			/// ����һ����Ķ������ݣ������޶�,����TSQL��������ҳ����
			/// TSQL ��ʽ�� 
			/// SELECT {�����ֶ��б�}
			/// FROM (SELECT {prefix} {�����ֶ��б�}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
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
			/// ����һ����Ķ������ݣ�����������������ֻ�������ֶΣ�������ҳ���ֶ�����
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
					if (Utils.GetBaseTable(v) != null)
					{
						Table t = Utils.GetBaseTable(v);
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
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶΣ������������򣬷�ҳ
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
				}

				#endregion


				#region SelectAllPartPage_Custom

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90)
				{
					sb.Append(@"
			/// <summary>
			/// ����һ����Ķ������ݣ������޶�,����TSQL��������ֻ�������ֶΣ�������ҳ����
			/// TSQL ��ʽ�� 
			/// SELECT {__cols}
			/// FROM (SELECT {prefix} {__cols}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(v.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(v.Name) + @"]
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
			/// ����һ����Ķ������ݣ�����������������ֻ�������ֶΣ�������ҳ���ֶ�����
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
					if (Utils.GetBaseTable(v) != null)
					{
						Table t = Utils.GetBaseTable(v);
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
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶΣ������������򣬷�ҳ
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
