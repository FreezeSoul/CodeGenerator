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
    public static class Gen_OB_StoredProcedure
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
            List<UserDefinedTableType> udtts = Utils.GetUserDefinedTableTypes(db);

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

            #region SPs

            sb.Append(@"
		#region Stored Procedures
");

            foreach (StoredProcedure sp in sps)
            {
                string spn = Utils.GetEscapeName(sp);

                // ������
                string mn = Utils.GetMethodName(sp);
                if (string.IsNullOrEmpty(mn)) mn = spn;

                // �ܹ���
                string sn = Utils.GetEscapeName(sp.Schema);

                // ���շ�����
                mn = (Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? (sn + "_") : ("")) + mn;


                // ��������ͣ���������ͼ����
                string rt = Utils.GetResultType(sp);
                // ������������� Schema
                string rts = Utils.GetResultTypeSchema(sp);

                // �����ϵ��趨����� rt ������Ϊ rt + rts����rts Ϊ��ʱ rt ��ֵӦΪ int, dt, ds, object ֮һ�������� ��[�� ��ͷ��ȻΪ [schema].[name] �ṹ��
                if (string.IsNullOrEmpty(rts))
                {
                    if (rt.StartsWith("["))
                    {
                        rts = rt.Substring(1, rt.IndexOf("].[", 0) - 1);
                        rt = rt.Substring(rts.Length + 4, rt.Length - rts.Length - 5);
                    }
                }

                //����ֵ�Ƿ�Ϊ����
                bool isSingleLine = Utils.GetIsSingleLineResult(sp);

                // ����ֵ��������������� int, dt, ds, object ����ָ��ĳ���������ͣ�����ǰ����� ���л���е����������֣�
                string rtn = isSingleLine ? "����" : "����";
                if (!string.IsNullOrEmpty(rts)) rtn = rtn + rt;

                // ������Ϊ
                string behavior = Utils.GetBehavior(sp);

                // ���еĽ����������
                string srt = rt;

                // backup
                string sqlrt = rt;

                // �� rt תΪ c# result type
                if (!string.IsNullOrEmpty(rts))
                {
                    object o = uts.Find(delegate(Table t)
                    {
                        return t.Name == rt && t.Schema == rts;
                    });
                    if (o != null)
                    {
                        string btn = Utils.GetEscapeName((Table)o);
                        rt = "OO." + btn + "Collection";
                        srt = "OO." + btn;
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
                            rt = "OO." + btn + "Collection";
                            srt = "OO." + btn;
                        }
                        else
                        {
                            o = udtts.Find(delegate(UserDefinedTableType t)
                            {
                                System.Diagnostics.Debug.Print(t.Name + " " + t.Schema);
                                return t.Name == rt && t.Schema == rts;
                            });
                            if (o != null)
                            {

                                string btn = Utils.GetEscapeName((UserDefinedTableType)o);
                                rt = oon2 + "." + btn + "Collection";
                                srt = oon2 + "." + btn;
                            }
                            else rt = Utils.EP_ResultType_DataTable;    // ���ָ������ݿ����δ�ҵ����򽫽����Ϊ DataTable
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
                sb.Append(@"
		public static " + (isSingleLine ? srt : rt) + @" " + mn + @"(");
                for (int j = 0; j < sp.Parameters.Count; j++)
                {
                    StoredProcedureParameter p = sp.Parameters[j];
                    string pn = Utils.GetEscapeName(p);
                    string typename;
                    if (j > 0) sb.Append(@", ");

                    if (p.DataType.SqlDataType == SqlDataType.UserDefinedTableType)
                        typename = oon2 + "." + Utils.GetEscapeName(p.DataType) + "Collection";
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
			else cmd.Parameters[""" + pn + @"""].Value = " + pn + (p.DataType.SqlDataType == SqlDataType.UserDefinedTableType ? ".GetDataTable()" : "") + ";");
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
			" + rt + @" os = new " + rt + @"();
			using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
			{
				while(sdr.Read())
				{
					os.Add(new " + srt + @"(");


                    if (uts.Find(delegate(Table target) { return (target.Name == sqlrt && target.Schema == rts); }) != null)
                    {
                        Table tb = uts.Find(delegate(Table target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }
                    else if (uvs.Find(delegate(View target) { return (target.Name == sqlrt && target.Schema == rts); }) != null)
                    {
                        View tb = uvs.Find(delegate(View target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }
                    else
                    {
                        UserDefinedTableType tb = udtts.Find(delegate(UserDefinedTableType target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }

                    sb.Append(@"));
				}
			}
");
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
			if (os.Count > 0) return os[0]; return null;");
                    }
                    else
                    {
                        sb.Append(@"
			return os;");
                    }
                    sb.Append(@"
		}");

                }










                sb.Append(Utils.GetSummary(sp, 2));
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
			" + rt + @" os = new " + rt + @"();
			using(SqlDataReader sdr = SQLHelper.ExecuteDataReader(cmd))
			{
				while(sdr.Read())
				{
					os.Add(new " + srt + @"(");


                    if (uts.Find(delegate(Table target) { return (target.Name == sqlrt && target.Schema == rts); }) != null)
                    {
                        Table tb = uts.Find(delegate(Table target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }
                    else if (uvs.Find(delegate(View target) { return (target.Name == sqlrt && target.Schema == rts); }) != null)
                    {
                        View tb = uvs.Find(delegate(View target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }
                    else
                    {
                        UserDefinedTableType tb = udtts.Find(delegate(UserDefinedTableType target) { return (target.Name == sqlrt && target.Schema == rts); });
                        for (int j = 0; j < tb.Columns.Count; j++)
                        {
                            Column c = tb.Columns[j];
                            if (j > 0) sb.Append(@", ");
                            if (c.Nullable)
                            {
                                s1 = Utils.CheckIsBinaryType(c) ? ("sdr.GetSqlBinary(" + j + @").Value") : (Utils.CheckIsValueType(c) ? ("new " + Utils.GetNullableDataType(c) + @"(sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @"))") : ("sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")"));
                                sb.Append(@"sdr.IsDBNull(" + j + @") ? null : " + s1);
                            }
                            else
                            {
                                if (Utils.CheckIsBinaryType(c))
                                    sb.Append(@"sdr.GetSqlBinary(" + j + @").Value");
                                else
                                    sb.Append(@"sdr." + Utils.GetDataReaderMethod(c) + @"(" + j + @")");
                            }
                        }
                    }

                    sb.Append(@"));
				}
			}
");
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
			if (os.Count > 0) return os[0]; return null;");
                    }
                    else
                    {
                        sb.Append(@"
			return os;");
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
