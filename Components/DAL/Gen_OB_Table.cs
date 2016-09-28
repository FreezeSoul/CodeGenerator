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
	/// ������ Data Command ������һһ��Ӧ�ĵ��÷���
	/// todo: CheckExists,  Merge
	/// </summary>
	public static class Gen_OB_Table
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
	public static partial class OB
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
			/// ����һ����������������������� TSQL �ִ�
			/// </summary>
			public static int GetCount_Custom(string where)
			{
				object o = SQLHelper.ExecuteScalar(DC." + tn + @".NewCmd_GetCount_Custom(where));
				if (o == DBNull.Value) return 0;
				return (int)o;
			}

			/// <summary>
			/// ����һ���������������������
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
			/// ���ݴ���Ķ��������s�����ҷ���һ�����ݲ���䴫�����δ�ҵ��򷵻� false
			/// </summary>
			public static bool Fill(OO." + tn + @" o)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select();");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";");
					}
					sb.Append(@"
				bool b = false;
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"
						o." + cn + @" = sdr.IsDBNull(" + istr + @") ? null : " + s1 + ";");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						o." + cn + @" = sdr.GetSqlBinary(" + istr + @").Value;");
							}
							else
								sb.Append(@"
						o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @");");
						}
					}
					sb.Append(@"
						b = true;
					}
				}
				return b;
			}
");
				}

				#endregion

				#region FillPart

				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ��������s�����ҷ���һ�����ݣ�����ֻ�������ֶΣ�����봫�����δ�ҵ��򷵻� null
			/// </summary>
			public static bool FillPart(OO." + tn + @" o, params DI." + tn + @"[] __cols)
			{
				Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart(__cols);");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
							}
							else
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
						}
					}
					sb.Append(@"
						return true;
					}
				}
				return false;
			}
");
				}

				#endregion


				#region FillAll

				sb.Append(@"
			/// <summary>
			/// ȡһ������������� �������һ�� Collection ( ������ͬ���滻ԭֵ����ͬ������ )��������Ӱ������
			/// </summary>
			public static int FillAll(OO." + tn + @"Collection oc)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll();
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string istr = i.ToString();
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
						sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
						}
						else
							sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
					}
				}
				sb.Append(@"));
					}
				}
				oc.Combine(os);
				return 0;
			}");

				#endregion

				#region FillAll_Custom

				sb.Append(@"
			/// <summary>
			/// ����һ����Ķ������ݣ�����ֲ� TSQL �� ������� Collection ���� ( ������ͬ���滻ԭֵ����ͬ������ )��������Ӱ������
			/// TSQL ��ʽ�� SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			public static int FillAll_Custom(OO." + tn + @"Collection oc, string s1, string s2)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll_Custom(s1, s2);
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string istr = i.ToString();
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
						sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
						}
						else
							sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
					}
				}
				sb.Append(@"));
					}
				}
				oc.Combine(os);
				return 0;
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���֣� ������� Collection ���� ( ������ͬ���滻ԭֵ����ͬ������ )��������Ӱ������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			public static int FillAll_Custom(OO." + tn + @"Collection oc, string where)
			{
				return FillAll_Custom(oc, """", "" WHERE "" + where);
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���ֵı��ʽ���� ������� Collection ���� ( ������ͬ���滻ԭֵ����ͬ������ )��������Ӱ������
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			public static int FillAll_Custom(OO." + tn + @"Collection oc, OE." + tn + @" exp)
			{
				return FillAll_Custom(oc, """", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")));
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
			/// ���ݴ�������s�����ҷ���һ������ ������� Collection ���� ( ������ͬ���滻ԭֵ����ͬ������ )��������Ӱ������
			/// </summary>
			public static int FillAll_By_" + s + @"(OO." + tn + @"Collection oc, ");
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
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
						for (int i = 0; i < t.Columns.Count; i++)
						{
							Column c = t.Columns[i];
							string istr = i.ToString();
							if (i > 0) sb.Append(@", ");
							if (c.Nullable)
							{
								s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
								sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
							}
							else
							{
								if (Utils.CheckIsBinaryType(c))
								{
									sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
								}
								else
									sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
							}
						}
						sb.Append(@"));
					}
				}
				oc.Combine(os);
				return 0;
			}
		");
					}
				}

				#endregion


				#region never finish yet

				#region FillNode

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{

				}

				#endregion

				#region FillNodePart

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
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
			public static OO." + tn + @" Select(");
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
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						if (i > 0) sb.Append(@", ");
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
							}
							else
								sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
						}
					}
					sb.Append(@");
					}
				}
				return null;
			}

			/// <summary>
			/// ���ݴ�����������󣬲��ҷ���һ�����ݡ�δ�ҵ��򷵻� null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" Select(OO." + tn + @".PrimaryKeys pk)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select();");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = pk." + cn + @";");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						if (i > 0) sb.Append(@", ");
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
							}
							else
								sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
						}
					}
					sb.Append(@");
					}
				}
				return null;
			}
");
				}

				#endregion

				#region Select_Custom

				sb.Append(@"
			/// <summary>
			/// ���ݴ���� TSQL WHERE�����ҷ���һ�����ݡ�δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" Select_Custom(string where)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Select_Custom(where);
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string istr = i.ToString();
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
						sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
						}
						else
							sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
					}
				}
				sb.Append(@");
					}
				}
				return null;
			}
			/// <summary>
			/// ���ݴ���� TSQL WHERE���ʽ���󣬲��ҷ���һ�����ݡ�δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" Select_Custom(OE." + tn + @" exp)
			{
				return Select_Custom(exp.ToString());
			}
		");

				#endregion

				#region SelectPart

				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ��������s�����ҷ���һ�����ݣ�����ֻ�������ֶΣ���δ�ҵ��򷵻� null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" SelectPart(");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(Utils.GetDataType(c) + " " + cn);
					}
					sb.Append(@", params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart(__cols);");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
							}
							else
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
						}
					}
					sb.Append(@"
						return o;
					}
				}
				return null;
			}

			/// <summary>
			/// ���ݴ�����������󣬲��ҷ���һ�����ݣ�����ֻ�������ֶΣ���δ�ҵ��򷵻� null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" SelectPart(OO." + tn + @".PrimaryKeys pk, params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart(__cols);");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = pk." + cn + @";");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
                            s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
							}
							else
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
						}
					}
					sb.Append(@"
						return o;
					}
				}
				return null;
			}
");
				}

				#endregion

				#region SelectPart_Custom

				sb.Append(@"
			/// <summary>
			/// ���ݴ���� TSQL WHERE�����ҷ���һ�����ݣ�����ֻ�������ֶΣ���δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" SelectPart_Custom(string where, params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectPart_Custom(where, __cols);
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
						sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
						}
						else
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
					}
				}
				sb.Append(@"
						return o;
					}
				}
				return null;
			}
			/// <summary>
			/// ���ݴ���� TSQL WHERE���ʽ���󣬲��ҷ���һ�����ݣ�����ֻ�������ֶΣ���δ�ҵ��򷵻� null
			/// TSQL ��ʽ�� SELECT TOP 1 * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @" SelectPart_Custom(OE." + tn + @" exp, params DI." + tn + @"[] __cols)
			{
				return SelectPart_Custom(exp.ToString(), __cols);
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
			public static OO." + tn + @"Collection SelectAll()
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll();
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string istr = i.ToString();
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
						sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
						}
						else
							sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
					}
				}
				sb.Append(@"));
					}
				}
				return os;
			}

			/// <summary>
			/// ����һ������������� ������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll(DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ����Ķ������� �������޶�����
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll(DI." + tn + @"SortDictionary sd, int top)
			{
				return SelectAll_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}
");

				#endregion

				#region SelectAll_Custom

				sb.Append(@"
			/// <summary>
			/// ����һ����Ķ������ݣ�����ֲ� TSQL ��
			/// TSQL ��ʽ�� SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_Custom(string s1, string s2)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAll_Custom(s1, s2);
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string istr = i.ToString();
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
						sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
						}
						else
							sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
					}
				}
				sb.Append(@"));
					}
				}
				return os;
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���֣�
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_Custom(string where)
			{
				return SelectAll_Custom("""", "" WHERE "" + where);
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���ֵı��ʽ����
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_Custom(OE." + tn + @" exp)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���ֵı��ʽ���󣬴����򣬣� 
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd)
			{
				return SelectAll_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")));
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���ֵı��ʽ���󣬴������޶������� 
			/// TSQL ��ʽ�� SELECT TOP top * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top)
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
			/// ���ݴ�������s�����ҷ���һ������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_By_" + s + @"(");
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
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
						for (int i = 0; i < t.Columns.Count; i++)
						{
							Column c = t.Columns[i];
							string istr = i.ToString();
							if (i > 0) sb.Append(@", ");
							if (c.Nullable)
							{
								s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
								sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
							}
							else
							{
								if (Utils.CheckIsBinaryType(c))
								{
									sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
								}
								else
									sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
							}
						}
						sb.Append(@"));
					}
				}
				return os;
			}

			/// <summary>
			/// ���ݴ�������s�����ҷ���һ�����ݣ������������������޶�����
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAll_By_" + s + @"(");
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
			/// ����һ������������ݣ�����ֻ�������ֶΣ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart(params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPart(__cols);
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
						sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
						}
						else
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
					}
				}
				sb.Append(@"
						os.Add(o);
					}
				}
				return os;
			}

			/// <summary>
			/// ����һ������������� ������ ������ֻ�������ֶΣ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart(DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom("""", (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")), __cols);
			}

			/// <summary>
			/// ����һ����Ķ������� �����������޶� ������ֻ�������ֶΣ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart(DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (sd != null ? (""ORDER BY "" + sd.ToString()) : ("""")), __cols);
			}
");

				#endregion

				#region SelectAllPart_Custom

				sb.Append(@"
			/// <summary>
			/// ����һ����Ķ������ݣ�����ֲ� TSQL ��������ֻ�������ֶΣ�
			/// TSQL ��ʽ�� SELECT {0} * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] {1}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_Custom(string s1, string s2, params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPart_Custom(s1, s2, __cols);
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
						sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
					}
					else
					{
						if (Utils.CheckIsBinaryType(c))
						{
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
						}
						else
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
					}
				}
				sb.Append(@"
						os.Add(o);
					}
				}
				return os;
			}

			/// <summary>
			/// ����һ����Ķ������ݣ����� TSQL WHERE ���֣�������ֻ�������ֶΣ�
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_Custom(string where, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom("""", "" WHERE "" + where, __cols);
			}

			/// <summary>
			/// ����һ����Ķ������ݣ�������������ֻ�������ֶΣ�
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_Custom(OE." + tn + @" exp, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")), __cols);
			}

			/// <summary>
			/// ����һ����Ķ������ݣ������������򣨶���ֻ�������ֶΣ�
			/// TSQL ��ʽ�� SELECT * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom("""", (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols);
			}

			/// <summary>
			/// ����һ����Ķ������ݣ��������������޶�����������ֻ�������ֶΣ�
			/// TSQL ��ʽ�� SELECT TOP top * FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp} ORDER BY {sd}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_Custom(OE." + tn + @" exp, DI." + tn + @"SortDictionary sd, int top, params DI." + tn + @"[] __cols)
			{
				return SelectAllPart_Custom((top >= 0 ? (""TOP "" + top.ToString()): """"), (exp != null ? ("" WHERE "" + exp.ToString()) : ("""")) + (sd != null ? ("" ORDER BY "" + sd.ToString()) : ("""")), __cols);
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
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_By_" + s + @"(");
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
                Array.Sort(__cols);
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
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
						for (int i = 0; i < t.Columns.Count; i++)
						{
							Column c = t.Columns[i];
							string cn = Utils.GetEscapeName(c);
							if (c.Nullable)
							{
								s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
							}
							else
							{
								if (Utils.CheckIsBinaryType(c))
								{
									sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
								}
								else
									sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
							}
						}
						sb.Append(@"
						os.Add(o);
					}
				}
				return os;
			}

			/// <summary>
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶΣ������������������޶�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPart_By_" + s + @"(");
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
			/// ���ݴ��������s�����ҷ���һ���ڵ�����ݡ�������򷵻����и��ڵ�Ϊ�յĽڵ����ݡ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectNode(");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(Utils.GetNullableDataType(c) + " " + cn);
					}
					sb.Append(@")
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectNode();");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
					}
					sb.Append(@"
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						os.Add(new OO." + tn + @"(");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						if (i > 0) sb.Append(@", ");
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
							}
							else
								sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
						}
					}
					sb.Append(@"));
					}
				}
				return os;
			}
");
				}

				#endregion

				#region SelectNodePart

				if (db.CompatibilityLevel >= CompatibilityLevel.Version90 && pfcs.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ��������s�����ҷ���һ���ڵ�Ĳ����ֶε����ݡ�������򷵻����и��ڵ�Ϊ�յĽڵ����ݡ�
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectNodePart(");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(Utils.GetNullableDataType(c) + " " + cn);
					}
					sb.Append(@", params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectNodePart(__cols);");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (" + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else cmd.Parameters[""" + cn + @"""].Value = " + cn + @";");
					}
					sb.Append(@"
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
							}
							else
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
						}
					}
					sb.Append(@"
						os.Add(o);
					}
				}
				return os;
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
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPage_Custom(string prefix, string where, DI." + tn + @"SortDictionary sortDict, int rowIndexStart, int pageSize)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPage_Custom(prefix, string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where), sortDict);
				cmd.Parameters[""__RN_BEGIN""].Value = rowIndexStart + 1;
				cmd.Parameters[""__RN_END""].Value = rowIndexStart + pageSize;
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						OO." + tn + @" o = new OO." + tn + @"();");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						string istr = i.ToString();
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"
						o." + cn + @" = sdr.IsDBNull(" + istr + @") ? null : " + s1 + @";");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						o." + cn + @" = sdr.GetSqlBinary(" + istr + @").Value;");
							}
							else
								sb.Append(@"
						o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @");");
						}
					}
					sb.Append(@"
						os.Add(o);
					}
				}
				return os;
			}
			/// <summary>
			/// ����һ����Ķ������ݣ�����������������ֻ�������ֶΣ�������ҳ���ֶ�����
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPage_Custom(OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize)
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
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶΣ������������򣬷�ҳ
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPage_By_" + s + @"(");
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
			/// ����һ����Ķ������ݣ������޶�,����TSQL��������ֻ�������ֶΣ�������ҳ����
			/// TSQL ��ʽ�� 
			/// SELECT {__cols}
			/// FROM (SELECT {prefix} {__cols}, ROW_NUMBER() OVER (ORDER BY {sortDict}) AS __RowNumber
			/// 		FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
			/// 		WHERE {where}
			/// 	 ) A
			/// WHERE RowNumber BETWEEN @__RN_BEGIN AND @__RN_END
			/// ORDER BY __RowNumber
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPartPage_Custom(string prefix, string where, DI." + tn + @"SortDictionary sortDict, int rowIndexStart, int pageSize, params DI." + tn + @"[] __cols)
			{
                Array.Sort(__cols);
				SqlCommand cmd = DC." + tn + @".NewCmd_SelectAllPartPage_Custom(prefix, string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where), sortDict, __cols);
				cmd.Parameters[""__RN_BEGIN""].Value = rowIndexStart + 1;
				cmd.Parameters[""__RN_END""].Value = rowIndexStart + pageSize;
				OO." + tn + @"Collection os = new OO." + tn + @"Collection();
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
                        int idx = 0;
						OO." + tn + @" o = new OO." + tn + @"();");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? (@"sdr.GetSqlBinary(idx).Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(idx))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(idx)"));
							sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @")
                        {
                            o." + cn + @" = sdr.IsDBNull(idx) ? null : " + s1 + @";
                            idx++;
                        }");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
							{
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr.GetSqlBinary(idx++).Value;");
							}
							else
								sb.Append(@"
						if (idx < __cols.Length && __cols[idx] == DI." + tn + @"." + cn + @") o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(idx++);");
						}
					}
					sb.Append(@"
						os.Add(o);
					}
				}
				return os;
			}
			/// <summary>
			/// ����һ����Ķ������ݣ�����������������ֻ�������ֶΣ�������ҳ���ֶ�����
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPartPage_Custom(OE." + tn + @" exp, DI." + tn + @" sortCol, bool isAsc, int rowIndexStart, int pageSize, params DI." + tn + @"[] __cols)
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
			/// ���ݴ�������s�����ҷ���һ�����ݵĲ����ֶΣ������������򣬷�ҳ
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
			public static OO." + tn + @"Collection SelectAllPartPage_By_" + s + @"(");
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
			/// ���ݴ��������s��ɾ��һ�����ݡ�������Ӱ������
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
			/// ���ݴ���� Original_����s��ɾ��һ�����ݡ�������Ӱ����������ҪΪ ObjectDataSource ����
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
			/// ���ݴ�������ݶ���ɾ��һ�����ݡ�������Ӱ������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
			public static int Delete(OO." + tn + @" o)
			{
				return Delete(o.GetPrimaryKeys());
			}

			/// <summary>
			/// ���ݴ�������ݶ������������ɾ��һ�����ݡ�������Ӱ������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int Delete(OO." + tn + @".PrimaryKeys pk)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Delete();");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = pk." + cn + @";");
					}
					sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}");
				}

				#endregion

				#region DeleteAll_Custom

				sb.Append(@"
			/// <summary>
			/// ���ݴ���� TSQL WHERE ɾ�����м�¼
			/// TSQL ��ʽ�� DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {where}
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteAll_Custom(string where)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteAll_Custom("""", string.IsNullOrEmpty(where) ? """" : ("" WHERE "" + where));
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// ���ݴ���� TSQL WHERE ���ֵı��ʽ���� ɾ�����м�¼
			/// TSQL ��ʽ�� DELETE FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] WHERE {exp}
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
			/// ���ݴ��������s��ɾ��һ���ڵ�����ݡ�������Ӱ������
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
			/// ���ݴ���� ��������s��ɾ��һ���ڵ�����ݡ�������Ӱ����������Ҫ������ ObjectDataSource)
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete)]
			public static int DeleteNode(OO." + tn + @" o)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_DeleteNode();");
						for (int i = 0; i < pks.Count; i++)
						{
							Column c = pks[i];
							string cn = Utils.GetEscapeName(c);
							sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = o." + cn + @";");
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
			/// ���ݴ�����ֶ�ֵ��ӵ����ݿⲢ����  ����ʧ�ܷ��� null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static OO." + tn + @" Insert(");
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
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
						for (int i = 0; i < t.Columns.Count; i++)
						{
							Column c = t.Columns[i];
							string istr = i.ToString();
							if (i > 0) sb.Append(@", ");
							if (c.Nullable)
							{
								s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
								sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
							}
							else
							{
								if (Utils.CheckIsBinaryType(c))
								{
									sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
								}
								else
									sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
							}
						}
						sb.Append(@");
					}
				}
				return null;
			}

			/// <summary>
			/// ���ݴ���� ���� ��ӵ����ݿ⣬ˢ��ԭ ���� ��������Ӱ��������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
			public static int Insert(OO." + tn + @" o)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Insert();");
						foreach (Column c in wcs)
						{
							if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
							string cn = Utils.GetEscapeName(c);
							if (c.Nullable)
							{
								sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
							}
							else if (Utils.CheckIsStringType(c))
							{
								sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
							}

							sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";");
						}
						sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{");
						for (int i = 0; i < t.Columns.Count; i++)
						{
							Column c = t.Columns[i];
							string cn = Utils.GetEscapeName(c);
							string istr = i.ToString();
							if (c.Nullable)
							{
								s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
								sb.Append(@"
						o." + cn + @" = sdr.IsDBNull(" + istr + @") ? null : " + s1 + @";");
							}
							else
							{
								if (Utils.CheckIsBinaryType(c))
								{
									sb.Append(@"
						o." + cn + @" = sdr.GetSqlBinary(" + istr + @").Value;");
								}
								else
									sb.Append(@"
						o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @");");
							}
						}
						sb.Append(@"
						return 1;
					}
				}
				return 0;
			}");
					}
					else
					{
						sb.Append(@"
			/// <summary>
			/// ���ݴ�����ֶ�ֵ��ӵ����ݿ⡣������Ӱ��������
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
			}

			/// <summary>
			/// ���ݴ���� ���� ��ӵ����ݿ⣬������Ӱ��������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int Insert(OO." + tn + @" o)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Insert();");
						foreach (Column c in wcs)
						{
							if (pks.Contains(c) && c.DataType.SqlDataType == SqlDataType.UniqueIdentifier && c.DefaultConstraint != null) continue;
							string cn = Utils.GetEscapeName(c);
							if (c.Nullable)
							{
								sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
							}
							else if (Utils.CheckIsStringType(c))
							{
								sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
							}

							sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";");
						}
						sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}
");
					}

				}

				#endregion

				#region InsertPart

				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ�����ֶβ��ּ�ֵ����ӵ����ݿⲢ����  ����ʧ�ܷ��� null
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static OO." + tn + @" InsertPart(Dictionary<DI." + tn + @", object> cvs)
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
				}");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						if (i > 0) sb.Append(@", ");
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
								sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
							else
								sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
						}
					}
					sb.Append(@");
					}
				}
				return null;
			}

			/// <summary>
			/// ���ݴ���Ķ�����ֶ��б���Ӳ����ֶε����ݿ⡣�������ݿ��иö��������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static OO." + tn + @" InsertPart(OO." + tn + @" o, params DI." + tn + @"[] __cols)
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
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
						}
						else if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
						}
						sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";

                    idx++;
				}");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{
						return new OO." + tn + @"(");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string istr = i.ToString();
						if (i > 0) sb.Append(@", ");
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"sdr.IsDBNull(" + istr + @") ? null : " + s1);
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
								sb.Append(@"sdr.GetSqlBinary(" + istr + @").Value");
							else
								sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")");
						}
					}
					sb.Append(@");
					}
				}
				return null;
			}
		");
				}
				else
				{
					sb.Append(@"
			/// <summary>
			/// ���ݴ�����ֶβ��ּ�ֵ����ӵ����ݿ⡣������Ӱ��������
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
				}");
					}
					sb.Append(@"
				return SQLHelper.ExecuteNonQuery(cmd);
			}

			/// <summary>
			/// ���ݴ���Ķ�����ֶ��б���Ӳ����ֶε����ݿ⡣������Ӱ��������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert)]
			public static int InsertPart(OO." + tn + @" o, params DI." + tn + @"[] __cols)
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
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
						}
						else if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
						}
						sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";

                    idx++;
				}");
					}
					sb.Append(@"
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
			/// ���ݴ�����ֶ�ֵ������һ�����ݡ�������Ӱ��������
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


			/// <summary>
			/// ���ݴ���Ķ��󣬸���һ�����ݲ�ˢ�¶��󡣷�����Ӱ��������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
			public static int Update(OO." + tn + @" o)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_Update();");
					foreach (Column c in pks)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = o." + cn + @";");
					}
					foreach (Column c in wcs)
					{
						string cn = Utils.GetEscapeName(c);
						if (c.Nullable)
						{
							sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
				else");
						}
						else if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
				if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
				else");
						}
						sb.Append(@"
				cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						string istr = i.ToString();
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"
						o." + cn + @" = sdr.IsDBNull(" + istr + @") ? null : " + s1 + @";");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
								sb.Append(@"
						o." + cn + @" = sdr.GetSqlBinary(" + istr + @").Value;");
							else
								sb.Append(@"
						o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @");");
						}
					}
					sb.Append(@"
						return 1;
					}
				}
				return 0;
			}");
				}

				#endregion

				#region UpdatePart

				if (wcs.Count > 0 && pks.Count > 0)
				{
					sb.Append(@"

			/// <summary>
			/// ���ݴ�����ֶ��б�ѡ���Ծֲ�����һ�����ݡ�������Ӱ��������
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
			/// ���ݴ�����ֶ��б�ѡ���Ծֲ����¶�������
			/// </summary>
			[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update)]
			public static int UpdatePart(OO." + tn + @" o, params DI." + tn + @"[] cols)
			{
				SqlCommand cmd = DC." + tn + @".NewCmd_UpdatePart(cols);");
					foreach (Column c in pks)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				cmd.Parameters[""Original_" + cn + @"""].Value = o." + cn + @";");
					}
					sb.Append(@"
				List<DI." + tn + @"> cs = new List<DI." + tn + @">(cols);");
					foreach (Column c in wcs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				if (cs.Contains(DI." + tn + @"." + cn + @"))
				{");
						if (c.Nullable)
						{
							sb.Append(@"
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = DBNull.Value;
					else");
						}
						else if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
					if (o." + cn + @" == null) cmd.Parameters[""" + cn + @"""].Value = """";
					else");
						}
						sb.Append(@"
					cmd.Parameters[""" + cn + @"""].Value = o." + cn + @";
				}");
					}
					sb.Append(@"
				using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
				{
					while(sdr.Read())
					{");
					for (int i = 0; i < t.Columns.Count; i++)
					{
						Column c = t.Columns[i];
						string cn = Utils.GetEscapeName(c);
						string istr = i.ToString();
						if (c.Nullable)
						{
							s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + istr + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @")"));
							sb.Append(@"
						o." + cn + @" = sdr.IsDBNull(" + istr + @") ? null : " + s1 + @";");
						}
						else
						{
							if (Utils.CheckIsBinaryType(c))
								sb.Append(@"
						o." + cn + @" = sdr.GetSqlBinary(" + istr + @").Value;");
							else
								sb.Append(@"
						o." + cn + @" = sdr." + Utils.GetDataReaderMethod(c) + @"(" + istr + @");");
						}
					}
					sb.Append(@"
						return 1;
					}
				}
				return 0;

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
