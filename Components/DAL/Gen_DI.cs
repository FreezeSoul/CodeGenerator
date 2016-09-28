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
	/// 对应于数据库 " + db.Name + @" 中的表，视图，表值函数的字段结构枚举
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
				sb.Append(Utils.GetSummary(t, " 排序对象 字典", 2));
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
			/// 将排序字典转化为 TSQL 查询表达式字串
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

				sb.Append(Utils.GetSummary(t, " 集合", 2));
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
				sb.Append(Utils.GetSummary(v, " 排序对象 字典", 2));
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
			/// 将排序字典转化为 TSQL 查询表达式字串
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
				sb.Append(Utils.GetSummary(v, " 集合", 2));
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
				sb.Append(Utils.GetSummary(f, " 排序对象 字典", 2));
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
			/// 将排序字典转化为 TSQL 查询表达式字串
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
				sb.Append(Utils.GetSummary(f, " 集合", 2));
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
		/// 包含存储过程 " + spn + @" 的所有参数的类声明
		/// </summary>
		public partial class " + spn + @"Parameters
		{
			protected int _returnValue;
			/// <summary>
			/// 获取存储过程中 RETURN 的整数值
			/// </summary>
			public int _ReturnValue
			{
				get
				{
					return _returnValue;
				}
			}

			/// <summary>
			/// 设置存储过程中 RETURN 的整数值
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
			/// 返回一个 BOOL 值，用以标识参数 " + pn + @" 的值是否被赋予或修改
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
