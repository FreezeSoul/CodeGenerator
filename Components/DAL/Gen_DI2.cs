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
	/// �û��Զ�������� ��ṹ������
	/// </summary>
	public static class Gen_DI2
	{
		public static string Gen(Database db, string ns, string className)
		{
			List<UserDefinedTableType> uts = Utils.GetUserDefinedTableTypes(db);

			StringBuilder sb = new StringBuilder();

			sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace " + ns + @"
{
	/// <summary>
	/// ��Ӧ�����ݿ� " + db.Name + @" �е� �û���������� ���ֶνṹö��
	/// </summary>
	public static partial class " + className + @"
	{
");
			foreach (UserDefinedTableType t in uts)
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
					sb.Append(Utils.GetSummary(c, 3));
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

	}
}
");
			return sb.ToString();

		}
	}
}
