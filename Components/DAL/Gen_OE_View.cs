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
	public static class Gen_OE_View
	{
		public static string Gen(Database db, string ns)
		{
			#region Header

			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> utfs = Utils.GetUserFunctions_TableValue(db);

			StringBuilder sb = new StringBuilder();

			sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace " + ns + @"
{
	/// <summary>
	/// ��ѯ���ʽ����������
	/// </summary>
	public static partial class OE
	{
");
			#endregion

			#region View

			sb.Append(@"
		#region Views
");

			foreach (View t in uvs)
			{
				string tbn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tbn + @"
");

				sb.Append(Utils.GetSummary(t, 2));
				sb.Append(@"
		public partial class " + tbn + @" : __Exp
		{
			#region Exp

			public " + tbn + @" And(" + tbn + @" subExp)
			{
				if (subExp == null) return this;
				__IsAndEffect = true;
				__Nodes.Add(subExp);
				return this;
			}
			public " + tbn + @" Or(" + tbn + @" subExp)
			{
				if (subExp == null) return this;
				__IsAndEffect = false;
				__Nodes.Add(subExp);
				return this;
			}


			public new DI." + tbn + @" __Column
			{
				get { return (DI." + tbn + @")base.__Column; }
				set { base.__Column = value; }
			}

			/// <summary>
			/// �����Զ�����ʽ
			/// </summary>
			public " + tbn + @"(string customExp) : base(0, 0, customExp) { }
			/// <summary>
			/// ������������ĳ�ֶεı��ʽ
			/// </summary>
			public " + tbn + @"(DI." + tbn + @" column, object value) : base(column, SQLHelper.Operators.Equal, value) { }
			/// <summary>
			/// ����ĳ�ֶε�ĳ������ı��ʽ
			/// </summary>
			public " + tbn + @"(DI." + tbn + @" column, SQLHelper.Operators operate, object value) : base(column, operate, value) { }

			/// <summary>
			/// �����ʽת��Ϊ TSQL ��ѯ���ʽ�ִ�
			/// </summary>
			public override string ToString()
			{
				string s = null;
				if(this.__Column == 0 && base.__Operate == SQLHelper.Operators.Custom)
				{
					s = (string)base.__Value;	   //for full custom expression support
				}
				else switch (this.__Column)
				{
");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					string typename = c.Nullable ? Utils.GetNullableDataType(c) : Utils.GetDataType(c);

					sb.Append(@"
					case DI." + tbn + @"." + cn + @":
						if (base.__Operate == SQLHelper.Operators.Custom) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]"" + (string)base.__Value;
						else if (base.__Operate == SQLHelper.Operators.In || base.__Operate == SQLHelper.Operators.NotIn) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" ("" + SQLHelper.Combine<" + typename + @">((IEnumerable<" + typename + @">)base.__Value) + "")"";");
					if (c.Nullable)
					{
						sb.Append(@"
						else if (base.__Value == null && base.__Operate == SQLHelper.Operators.Equal) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NULL"";
						else if (base.__Value == null && base.__Operate == SQLHelper.Operators.NotEqual) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IS NOT NULL"";");
						if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
						else if (base.__Operate == SQLHelper.Operators.Like) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE '%"" + SQLHelper.EscapeLike((string)base.__Value) + ""%'"";
						else if (base.__Operate == SQLHelper.Operators.CustomLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";
						else if (base.__Operate == SQLHelper.Operators.NotLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE '%"" + SQLHelper.EscapeLike((string)base.__Value) + ""%'"";
						else if (base.__Operate == SQLHelper.Operators.CustomNotLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";");
						}
						else if (Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" '"" + base.__Value.ToString() + ""'"";");
						}
						else if (Utils.CheckIsBooleanType(c))
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" "" + (((Boolean?)base.__Value).Value ? ""1"" : ""0"");");
						}
						else
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" "" + base.__Value.ToString();");
						}
					}
					else
					{
						if (Utils.CheckIsStringType(c))
						{
							sb.Append(@"
						else if (base.__Operate == SQLHelper.Operators.Like) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE '%"" + SQLHelper.EscapeLike((string)base.__Value) + ""%'"";
						else if (base.__Operate == SQLHelper.Operators.CustomLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";
						else if (base.__Operate == SQLHelper.Operators.NotLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE '%"" + SQLHelper.EscapeLike((string)base.__Value) + ""%'"";
						else if (base.__Operate == SQLHelper.Operators.CustomNotLike) s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" '"" + SQLHelper.EscapeEqual((string)base.__Value) + ""'"";");
						}
						else if (Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" '"" + base.__Value.ToString() + ""'"";");
						}
						else if (Utils.CheckIsBooleanType(c))
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" "" + ((bool)base.__Value ? ""1"" : ""0"");");
						}
						else
						{
							sb.Append(@"
						else s = ""[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] "" + SQLHelper.GetOperater(base.__Operate) + "" "" + base.__Value.ToString();");
						}
					}
					sb.Append(@"
						break;
");
				}
				sb.Append(@"
				}
				if (this.__Nodes != null && this.__Nodes.Count > 0)
				{
					s = ""("" + s ;
					foreach (" + tbn + @" __node in this.__Nodes)
					{
						s += "" "" + (this.__IsAndEffect ? ""AND"" : ""OR"") + "" "" + __node.ToString();
					}
					s += "")"";
				}
				return s;
			}

			#endregion

");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					string typename = c.Nullable ? Utils.GetNullableDataType(c) : Utils.GetDataType(c);
					sb.Append(@"
			#region " + c.Name + @"");

					sb.Append(Utils.GetSummary(c, 3));

					sb.Append(@"
			public partial class " + cn + @"
			{
");
					switch (typename)
					{
						case "bool":
						case "bool?":
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ�����ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] {s}
				/// </summary>
				public static " + tbn + @" Custom(string s)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", 0, s);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = {value}
				/// </summary>
				public static " + tbn + @" Equal(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.Equal, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <> {value}
				/// </summary>
				public static " + tbn + @" NotEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotEqual, " + cn + @");
				}
");
							break;

						case "System.Guid?":
						case "System.Guid":
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ�����ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] {s}
				/// </summary>
				public static " + tbn + @" Custom(string s)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", 0, s);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = {value}
				/// </summary>
				public static " + tbn + @" Equal(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.Equal, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <> {value}
				/// </summary>
				public static " + tbn + @" NotEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotEqual, " + cn + @");
				}
");

							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.In, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(params " + typename + @"[] list)
				{
					return In(new List<" + typename + @">(list));
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotIn, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(params " + typename + @"[] list)
				{
					return NotIn(new List<" + typename + @">(list));
				}
");
							break;

						case "byte":
						case "Byte?":

						case "short":
						case "short?":

						case "int":
						case "int?":

						case "System.Int64":
						case "Int64?":

						case "float?":
						case "float":

						case "decimal":
						case "decimal?":

						case "System.DateTime?":
						case "System.DateTime":

							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ�����ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] {s}
				/// </summary>
				public static " + tbn + @" Custom(string s)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", 0, s);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = {value}
				/// </summary>
				public static " + tbn + @" Equal(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.Equal, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <> {value}
				/// </summary>
				public static " + tbn + @" NotEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotEqual, " + cn + @");
				}
");

							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� С�� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] < {value}
				/// </summary>
				public static " + tbn + @" LessThan(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LessThan, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� С���ҵ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <= {value}
				/// </summary>
				public static " + tbn + @" LessEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LessEqual, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] > {value}
				/// </summary>
				public static " + tbn + @" LargerThan(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LargerThan, " + cn + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �����ҵ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] >= {value}
				/// </summary>
				public static " + tbn + @" LargerEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LargerEqual, " + cn + @");
				}
	");
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.In, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(params " + typename + @"[] list)
				{
					return In(new List<" + typename + @">(list));
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotIn, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(params " + typename + @"[] list)
				{
					return NotIn(new List<" + typename + @">(list));
				}
");

							break;

						case "string":
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ�����ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] {s}
				/// </summary>
				public static " + tbn + @" Custom(string s)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", 0, s);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = {value}
				/// </summary>
				public static " + tbn + @" Equal(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.Equal, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <> {value}
				/// </summary>
				public static " + tbn + @" NotEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotEqual, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
");
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE '% ? %'
				/// </summary>
				public static " + tbn + @" Like(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.Like, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ������� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE ' ? '
				/// </summary>
				public static " + tbn + @" LikeEx(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.CustomLike, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE '% ? %'
				/// </summary>
				public static " + tbn + @" NotLike(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotLike, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �Զ��岻���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT LIKE ' ? '
				/// </summary>
				public static " + tbn + @" NotLikeEx(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.CustomNotLike, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
");

							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� С�� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] < {value}
				/// </summary>
				public static " + tbn + @" LessThan(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LessThan, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� С���ҵ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] <= {value}
				/// </summary>
				public static " + tbn + @" LessEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LessEqual, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] > {value}
				/// </summary>
				public static " + tbn + @" LargerThan(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LargerThan, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� �����ҵ��� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] >= {value}
				/// </summary>
				public static " + tbn + @" LargerEqual(" + typename + @" " + cn + @")
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.LargerEqual, " + cn + (c.Nullable ? "" : @" ?? """"") + @");
				}
	");
							sb.Append(@"
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.In, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ���� �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" In(params " + typename + @"[] list)
				{
					return In(new List<" + typename + @">(list));
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(IEnumerable<" + typename + @"> list)
				{
					return new " + tbn + @"(DI." + tbn + @"." + cn + @", SQLHelper.Operators.NotIn, list);
				}
				/// <summary>
				/// ���� �ֶ� " + c.Name + @" �� ������ �жϱ��ʽ��
				/// ���ɸ�ʽΪ  [" + Utils.GetEscapeSqlObjectName(c.Name) + @"] NOT IN ( ??,??,??,.... )
				/// </summary>
				public static " + tbn + @" NotIn(params " + typename + @"[] list)
				{
					return NotIn(new List<" + typename + @">(list));
				}
");

							break;
						case "byte[]": break;   //todo
						default:
							break;			  //todo
					}
					sb.Append(@"
			}
			#endregion
");
				}
				sb.Append(@"
		}");

				sb.Append(@"
		#endregion
");
			}

			sb.Append(@"
		#endregion
");

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
