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
	/// 生成与 Data Command 生成物一一对应的调用方法
	/// todo: CheckExists,  Merge
	/// </summary>
	public static class Gen_OB_Function
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

			#region Functions

			sb.Append(@"
		#region User Defined Functions
");

			foreach (UserDefinedFunction f in ufs)
			{
				string fn = Utils.GetEscapeName(f);

				// 方法名
				string mn = Utils.GetMethodName(f);
				if (string.IsNullOrEmpty(mn)) mn = fn;

				// 架构名
				string sn = Utils.GetEscapeName(f.Schema);

				// 最终方法名
				mn = (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? (sn + "_") : ("")) + mn;






				sb.Append(Utils.GetSummary(f, 2));
				for (int j = 0; j < f.Parameters.Count; j++)
				{
					UserDefinedFunctionParameter p = f.Parameters[j];
					string pn = Utils.GetEscapeName(p);
					string psum = Utils.GetDescription(p);
					if (!string.IsNullOrEmpty(psum)) sb.Append(@"
		/// <param name=""" + pn + @""">" + psum + @"</param>");
				}

				if (f.FunctionType == UserDefinedFunctionType.Table || f.FunctionType == UserDefinedFunctionType.Inline)
				{
					sb.Append(@"
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select)]
		public static OO." + fn + @"Collection " + mn + @"(");
					for (int j = 0; j < f.Parameters.Count; j++)
					{
						UserDefinedFunctionParameter p = f.Parameters[j];
						string pn = Utils.GetEscapeName(p);
						if (j > 0) sb.Append(@", ");
						sb.Append(Utils.GetNullableDataType(p) + " " + pn);
					}
					sb.Append(@")
		{
			SqlCommand cmd = DC.NewCmd_" + fn + @"();");
					for (int i = 0; i < f.Parameters.Count; i++)
					{
						UserDefinedFunctionParameter p = f.Parameters[i];
						string pn = Utils.GetEscapeName(p);
						sb.Append(@"
			if (" + pn + @" == null) cmd.Parameters[""" + pn + @"""].Value = DBNull.Value;
			else cmd.Parameters[""" + pn + @"""].Value = " + pn + ";");

					}

					sb.Append(@"
			OO." + fn + @"Collection os = new OO." + fn + @"Collection();
			using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
			{
				while(sdr.Read())
				{
					os.Add(new OO." + fn + @"(");
					for (int i = 0; i < f.Columns.Count; i++)
					{
						Column c = f.Columns[i];
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
					sb.Append(@"));
				}
			}
			return os;
		}");
				}

				if (f.FunctionType != UserDefinedFunctionType.Table && f.FunctionType != UserDefinedFunctionType.Inline)
				{
					sb.Append(@"
		public static " + Utils.GetNullableDataType(f) + @" " + mn + @"(");
					for (int i = 0; i < f.Parameters.Count; i++)
					{
						UserDefinedFunctionParameter p = f.Parameters[i];
						string pn = Utils.GetEscapeName(p);
						if (i > 0) sb.Append(", ");
						sb.Append(Utils.GetNullableDataType(p) + " " + pn);
					}

					sb.Append(@")
		{
			SqlCommand cmd = DC.NewCmd_" + fn + @"();");
					for (int i = 0; i < f.Parameters.Count; i++)
					{
						UserDefinedFunctionParameter p = f.Parameters[i];
						string pn = Utils.GetEscapeName(p);
						sb.Append(@"
			if (" + pn + @" == null) cmd.Parameters[""" + pn + @"""].Value = DBNull.Value;
			else cmd.Parameters[""" + pn + @"""].Value = " + pn + ";");

					}
					string ntn = Utils.GetNullableDataType(f);
					if (Utils.CheckIsStringType(f))
					{
						sb.Append(@"
			object o = SQLHelper.ExecuteScalar(cmd);
			if(o == DBNull.Value) return null;
			return (string)o;");
					}
                    else if (f.DataType.SqlDataType == SqlDataType.Variant)
                    {
                        sb.Append(@"
			return SQLHelper.ExecuteScalar(cmd);");
                    }
					else
					{
						sb.Append(@"
			object o = SQLHelper.ExecuteScalar(cmd);
			if(o == DBNull.Value) return null;
			return new " + ntn + "((" + Utils.GetDataType(f) + @")o);");
					}
					sb.Append(@"
		}
");

				}
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
