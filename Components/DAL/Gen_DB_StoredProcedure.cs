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
    public static class Gen_DB_StoredProcedure
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
            List<UserDefinedTableType> udtts = Utils.GetUserDefinedTableTypes(db);

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

            #region SPs

            sb.Append(@"
		#region Stored Procedures
");

            foreach (StoredProcedure sp in sps)
            {
                string spn = Utils.GetEscapeName(sp);

                // 方法名
                string mn = Utils.GetMethodName(sp);
                if (string.IsNullOrEmpty(mn)) mn = spn;

                // 架构名
                string sn = Utils.GetEscapeName(sp.Schema);

                // 最终方法名
                mn = (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? (sn + "_") : ("")) + mn;


                // 结果集类型（表名或视图名）
                string rt = Utils.GetResultType(sp);
                // 结果集类型所属 Schema
                string rts = Utils.GetResultTypeSchema(sp);

                // 兼容老的设定（拆分 rt 的内容为 rt + rts）（rts 为空时 rt 的值应为 int, dt, ds, object 之一。。。以 “[” 开头必然为 [schema].[name] 结构）
                if (string.IsNullOrEmpty(rts))
                {
                    if (rt.StartsWith("["))
                    {
                        rts = rt.Substring(1, rt.IndexOf("].[", 0) - 1);
                        rt = rt.Substring(rts.Length + 4, rt.Length - rts.Length - 5);
                    }
                }

                // 返回值是否为单行
                bool isSingleLine = Utils.GetIsSingleLineResult(sp);

                // 返回值的描述（如果不是 int, dt, ds, object 而是指向某个表或表类型，则在前面加上 单行或多行的描述性文字）
                string rtn = isSingleLine ? "单行" : "多行";
                if (!string.IsNullOrEmpty(rts)) rtn = rtn+rt;

                // 所属行为
                string behavior = Utils.GetBehavior(sp);

                // 单行的结果集类型名
                string srt = "";


                if (!string.IsNullOrEmpty(rts))
                {
                    object o = uts.Find(delegate(Table t)
                    {
                        return t.Name == rt && t.Schema == rts;
                    });
                    if (o != null)
                    {
                        string btn = Utils.GetEscapeName((Table)o);
                        rt = dsn + "." + btn + "DataTable";
                        srt = dsn + "." + btn + "Row";
                    }
                    else
                    {
                        o = uvs.Find(delegate(View t)
                        {
                            return t.Name == rt && t.Schema == rts;
                        });
                        if (o != null)
                        {
                            string btn = Utils.GetEscapeName((View)o);
                            rt = dsn + "." + btn + "DataTable";
                            srt = dsn + "." + btn + "Row";
                        }
                        else
                        {
                            o = udtts.Find(delegate(UserDefinedTableType t)
                            {
                                return t.Name == rt && t.Schema == rts;
                            });
                            if (o != null)
                            {
                                string btn = Utils.GetEscapeName((UserDefinedTableType)o);
                                rt = dsn2 + "." + btn + "DataTable";
                                srt = dsn2 + "." + btn + "Row";
                            }
                            else rt = Utils.EP_ResultType_DataTable;    // 如果指向的数据库对象未找到，则将结果设为 DataTable
                        }
                    }
                }







                sb.Append(Utils.GetSummary(sp, 2));
                for (int j = 0; j < sp.Parameters.Count; j++)
                {
                    StoredProcedureParameter p = sp.Parameters[j];
                    string pn = Utils.GetEscapeName(p);
                    string psum = Utils.GetDescription(p);
                    if (!string.IsNullOrEmpty(psum)) sb.Append(@"
		/// <param name=""" + pn + @""">" + psum + @"</param>");
                }
                sb.Append(@"
		/// <returns>" + rtn + @"</returns>");
                if (string.IsNullOrEmpty(behavior) || behavior == "None")
                {
                }
                else
                {
                    sb.Append(@"
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType." + behavior + @")]");
                }
                sb.Append(@"
		public static " + (isSingleLine ? srt : rt) + @" " + mn + @"(");
                for (int j = 0; j < sp.Parameters.Count; j++)
                {
                    StoredProcedureParameter p = sp.Parameters[j];
                    string pn = Utils.GetEscapeName(p);
                    string typename;

                    if (j > 0) sb.Append(@", ");

                    if (p.DataType.SqlDataType == SqlDataType.UserDefinedTableType)
                        typename = dsn2 + "." + Utils.GetEscapeName(p.DataType) + "DataTable";
                    else
                        typename = Utils.GetNullableDataType(p);
                    sb.Append(@" " + (p.IsOutputParameter ? "ref" : "") + @" " + typename + " " + pn);
                }
                sb.Append(@")
		{
			SqlCommand cmd = DC.NewCmd_" + spn + @"();");
                foreach (StoredProcedureParameter p in sp.Parameters)
                {
                    string pn = Utils.GetEscapeName(p);
                    sb.Append(@"
			if (" + pn + @" == null) cmd.Parameters[""" + pn + @"""].Value = DBNull.Value;
			else cmd.Parameters[""" + pn + @"""].Value = " + pn + ";");
                }

                if (rt == Utils.EP_ResultType_Int)
                {
                    sb.Append(@"
			SQLHelper.ExecuteNonQuery(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			" + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) " + pn + @" = null;
			else " + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s)) return 0;
			return int.Parse(s);
		}");
                }
                else if (rt == Utils.EP_ResultType_DataSet)
                {
                    sb.Append(@"
			DataSet ds = SQLHelper.ExecuteDataSet(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			" + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) " + pn + @" = null;
			else " + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			return ds;
		}");
                }
                else if (rt == Utils.EP_ResultType_DataTable)
                {
                    sb.Append(@"
			DataTable dt = SQLHelper.ExecuteDataTable(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			" + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) " + pn + @" = null;
			else " + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			return dt;
		}");
                }
                else if (rt == Utils.EP_ResultType_Object)
                {
                    sb.Append(@"
			Object result = SQLHelper.ExecuteScalar(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			" + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) " + pn + @" = null;
			else " + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			return result;
		}");
                }
                else
                {
                    sb.Append(@"
			" + rt + @" dt = new " + rt + @"();
			SQLHelper.ExecuteDataTable(dt, cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			" + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) " + pn + @" = null;
			else " + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }

                    if (isSingleLine)
                    {
                        sb.Append(@"
			if (dt.Count > 0) return dt[0]; return null;");
                    }
                    else
                    {
                        sb.Append(@"
			return dt;");
                    }
                    sb.Append(@"
		}");
                }









                sb.Append(Utils.GetSummary(sp, 2));
                sb.Append(@"
		/// <returns>" + rtn + @"</returns>");
                sb.Append(@"
		public static " + (isSingleLine ? srt : rt) + @" " + mn + @"(DI." + spn + @"Parameters p)
		{
			SqlCommand cmd = DC.NewCmd_" + spn + @"(p);");
                foreach (StoredProcedureParameter p in sp.Parameters)
                {
                    string pn = Utils.GetEscapeName(p);
                    sb.Append(@"
			if (p.CheckIs" + pn + @"Changed())
			{
				object o = p." + pn + @";
				if (o == null) cmd.Parameters[""" + pn + @"""].Value = DBNull.Value;
				else cmd.Parameters[""" + pn + @"""].Value = o;
			}");
                }

                if (rt == Utils.EP_ResultType_Int)
                {
                    sb.Append(@"
			SQLHelper.ExecuteNonQuery(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			p." + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) p." + pn + @" = null;
			else p." + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s))
			{
				p.SetReturnValue(0);
				return 0;
			}
			else
			{
				p.SetReturnValue(int.Parse(s));
				return p._ReturnValue;
			}
		}");
                }
                else if (rt == Utils.EP_ResultType_DataSet)
                {
                    sb.Append(@"
			DataSet ds = SQLHelper.ExecuteDataSet(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			p." + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) p." + pn + @" = null;
			else p." + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s))
			{
				p.SetReturnValue(0);
			}
			else
			{
				p.SetReturnValue(int.Parse(s));
			}
			return ds;
		}");
                }
                else if (rt == Utils.EP_ResultType_DataTable)
                {
                    sb.Append(@"
			DataTable dt = SQLHelper.ExecuteDataTable(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			p." + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) p." + pn + @" = null;
			else p." + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }

                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s))
			{
				p.SetReturnValue(0);
			}
			else
			{
				p.SetReturnValue(int.Parse(s));
			}
			return dt;
		}");
                }
                else if (rt == Utils.EP_ResultType_Object)
                {
                    sb.Append(@"
			Object result = SQLHelper.ExecuteScalar(cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			p." + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) p." + pn + @" = null;
			else p." + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }

                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s))
			{
				p.SetReturnValue(0);
			}
			else
			{
				p.SetReturnValue(int.Parse(s));
			}
			return result;
		}");
                }
                else
                {
                    sb.Append(@"
			" + rt + @" dt = new " + rt + @"();
			SQLHelper.ExecuteDataTable(dt, cmd);");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        if (!p.IsOutputParameter) continue;
                        string pn = Utils.GetEscapeName(p);
                        string typename = Utils.GetNullableDataType(p);
                        if (Utils.CheckIsStringType(p))
                        {
                            sb.Append(@"
			p." + pn + @" = cmd.Parameters[""" + pn + @"""].Value as string;");
                        }
                        else
                        {
                            sb.Append(@"
			if (cmd.Parameters[""" + pn + @"""].Value == null || cmd.Parameters[""" + pn + @"""].Value == DBNull.Value) p." + pn + @" = null;
			else p." + pn + @" = new " + typename + @"((" + Utils.GetDataType(p) + @")cmd.Parameters[""" + pn + @"""].Value);");
                        }
                    }
                    sb.Append(@"
			string s = cmd.Parameters[""RETURN_VALUE""].Value.ToString();
			if (string.IsNullOrEmpty(s))
			{
				p.SetReturnValue(0);
			}
			else
			{
				p.SetReturnValue(int.Parse(s));
			}
");
                    if (isSingleLine)
                    {
                        sb.Append(@"
			if (dt.Count > 0) return dt[0]; return null;");
                    }
                    else
                    {
                        sb.Append(@"
			return dt;");
                    }
                    sb.Append(@"
		}");
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
