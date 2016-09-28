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
	/// Data Info �����������������ݿ�Ľṹ������Ӧ�� ���ֶ���ö�� ����
	/// </summary>
	public static class Gen_DI
	{
		public static string Gen(Database db, string ns, string dsn2)
		{
			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> utfs = Utils.GetUserFunctions_TableValue(db);
			List<StoredProcedure> usp = Utils.GetUserStoredProcedures(db);

			StringBuilder sb = new StringBuilder();

			sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace " + ns + @"
{
	/// <summary>
	/// ��Ӧ�����ݿ� " + db.Name + @" �еı���ͼ����ֵ�������ֶνṹö��
	/// </summary>
	public static partial class DI
	{
");
			sb.Append(@"
		#region Tables
");

			foreach (Table t in uts)
			{
				string tbn = Utils.GetEscapeName(t);
				List<Column> socs = Utils.GetSortableColumns(t);

				sb.Append(@"
		#region " + tbn + @"
");

				sb.Append(Utils.GetSummary(t, 2));
				sb.Append(@"
		public enum " + tbn + @"
		{");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 2));
					sb.Append(@"
			" + cn + (i == 0 ? " = 1" : "") + ",");
				}
				sb.Append(@"
		}
");
				sb.Append(Utils.GetSummary(t, " ������� �ֵ�", 2));
				sb.Append(@"
		public class " + tbn + @"SortDictionary : Dictionary<" + tbn + @", bool>
		{
			public " + tbn + @"SortDictionary(" + tbn + @" col) : base()
			{
				this.Add(col, true);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col, bool IsAscending) : base()
			{
				this.Add(col, IsAscending);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2, " + tbn + @" col3, bool IsAscending3) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
				this.Add(col3, IsAscending3);
			}

			/// <summary>
			/// �������ֵ�ת��Ϊ TSQL ��ѯ���ʽ�ִ�
			/// </summary>
			public override string ToString()
			{
				string s = """";
				foreach(KeyValuePair<" + tbn + @", bool> kv in this)
				{
					if (s.Length > 0) s += "", "";");
				foreach (Column c in socs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
					if (kv.Key == " + tbn + @"." + cn + @") s += ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (kv.Value ? """" : "" DESC"");");
				}
				sb.Append(@"
				}
				return s;
			}
		}
");

				sb.Append(Utils.GetSummary(t, " ����", 2));
				sb.Append(@"
		public class " + tbn + @"Collection : List<" + tbn + @">
		{
		}
");

				sb.Append(@"
		#endregion
");
			}

			sb.Append(@"
		#endregion
");


			sb.Append(@"
		#region Views
");


			foreach (View v in uvs)
			{
				string tbn = Utils.GetEscapeName(v);
				List<Column> socs = Utils.GetSortableColumns(v);

				sb.Append(@"
		#region " + tbn + @"
");

				sb.Append(Utils.GetSummary(v, 2));
				sb.Append(@"
		public enum " + tbn + @"
		{");
				for (int i = 0; i < v.Columns.Count; i++)
				{
					Column c = v.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 2));
					sb.Append(@"
			" + cn + (i == 0 ? " = 1" : "") + ",");
				}
				sb.Append(@"
		}
");
				sb.Append(Utils.GetSummary(v, " ������� �ֵ�", 2));
				sb.Append(@"
		public class " + tbn + @"SortDictionary : Dictionary<" + tbn + @", bool>
		{
			public " + tbn + @"SortDictionary(" + tbn + @" col) : base()
			{
				this.Add(col, true);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col, bool IsAscending) : base()
			{
				this.Add(col, IsAscending);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2, " + tbn + @" col3, bool IsAscending3) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
				this.Add(col3, IsAscending3);
			}

			/// <summary>
			/// �������ֵ�ת��Ϊ TSQL ��ѯ���ʽ�ִ�
			/// </summary>
			public override string ToString()
			{
				string s = """";
				foreach(KeyValuePair<" + tbn + @", bool> kv in this)
				{
					if (s.Length > 0) s += "", "";");
				foreach (Column c in socs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
					if (kv.Key == " + tbn + @"." + cn + @") s += ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (kv.Value ? """" : "" DESC"");");
				}
				sb.Append(@"
				}
				return s;
			}
		}
");
				sb.Append(Utils.GetSummary(v, " ����", 2));
				sb.Append(@"
		public class " + tbn + @"Collection : List<" + tbn + @">
		{
		}
");

				sb.Append(@"
		#endregion
");

			}

			sb.Append(@"
		#endregion
");

			sb.Append(@"
		#region User Defined Function Tables
");

			foreach (UserDefinedFunction f in utfs)
			{
				string tbn = Utils.GetEscapeName(f);
				List<Column> socs = Utils.GetSortableColumns(f);

				sb.Append(@"
		#region " + tbn + @"
");

				sb.Append(Utils.GetSummary(f, 2));
				sb.Append(@"
		public enum " + tbn + @"
		{");
				for (int i = 0; i < f.Columns.Count; i++)
				{
					Column c = f.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 2));
					sb.Append(@"
			" + cn + (i == 0 ? " = 1" : "") + ",");
				}
				sb.Append(@"
		}
");
				sb.Append(Utils.GetSummary(f, " ������� �ֵ�", 2));
				sb.Append(@"
		public class " + tbn + @"SortDictionary : Dictionary<" + tbn + @", bool>
		{
			public " + tbn + @"SortDictionary(" + tbn + @" col) : base()
			{
				this.Add(col, true);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col, bool IsAscending) : base()
			{
				this.Add(col, IsAscending);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
			}
			public " + tbn + @"SortDictionary(" + tbn + @" col1, bool IsAscending1, " + tbn + @" col2, bool IsAscending2, " + tbn + @" col3, bool IsAscending3) : base()
			{
				this.Add(col1, IsAscending1);
				this.Add(col2, IsAscending2);
				this.Add(col3, IsAscending3);
			}

			/// <summary>
			/// �������ֵ�ת��Ϊ TSQL ��ѯ���ʽ�ִ�
			/// </summary>
			public override string ToString()
			{
				string s = """";
				foreach(KeyValuePair<" + tbn + @", bool> kv in this)
				{
					if (s.Length > 0) s += "", "";");
				foreach (Column c in socs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
					if (kv.Key == " + tbn + @"." + cn + @") s += ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (kv.Value ? """" : "" DESC"");");
				}
				sb.Append(@"
				}
				return s;
			}
		}
");
				sb.Append(Utils.GetSummary(f, " ����", 2));
				sb.Append(@"
		public class " + tbn + @"Collection : List<" + tbn + @">
		{
		}
");
				sb.Append(@"
		#endregion
");
			}

			sb.Append(@"
		#endregion

		#region Stored Procedures
");

			foreach (StoredProcedure sp in usp)
			{
				string spn = Utils.GetEscapeName(sp);
				sb.Append(@"

		/// <summary>
		/// �����洢���� " + spn + @" �����в�����������
		/// </summary>
		public partial class " + spn + @"Parameters
		{
			protected int _returnValue;
			/// <summary>
			/// ��ȡ�洢������ RETURN ������ֵ
			/// </summary>
			public int _ReturnValue
			{
				get
				{
					return _returnValue;
				}
			}

			/// <summary>
			/// ���ô洢������ RETURN ������ֵ
			/// </summary>
			public void SetReturnValue(int i)
			{
				_returnValue = i;
			}

");
				foreach (StoredProcedureParameter p in sp.Parameters)
				{
					string pn = Utils.GetEscapeName(p);
					string tn;
					if (p.DataType.SqlDataType == SqlDataType.UserDefinedTableType)
					{
						tn = dsn2 + "." + Utils.GetEscapeName(p.DataType) + "DataTable";
					}
					else tn = Utils.GetNullableDataType(p);

					//                    if (p.IsOutputParameter)
					//                        sb.Append(@"
					//			protected bool _Is" + pn + @"Changed = true;
					//			protected " + tn + @" _" + pn + @" = null;");
					//                    else
					sb.Append(@"
			protected bool _Is" + pn + @"Changed = false;
			protected " + tn + @" _" + pn + @";");
					sb.Append(Utils.GetSummary(p, 3));
					sb.Append(@"
			public " + tn + @" " + pn + @"
			{
				get
				{
					return _" + pn + @";
				}
				set
				{
					_" + pn + @" = value;
					_Is" + pn + @"Changed = true;
				}
			}

			/// <summary>
			/// ����һ�� BOOL ֵ�����Ա�ʶ���� " + pn + @" ��ֵ�Ƿ񱻸�����޸�
			/// </summary>
			public bool CheckIs" + pn + @"Changed()
			{
				return _Is" + pn + @"Changed;
			}
");
				}

				sb.Append(@"
		}
");
			}

			sb.Append(@"

		#endregion
	}
}
");
			return sb.ToString();

		}
	}
}
