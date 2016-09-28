using System;
using System.Collections.Generic;
using System.Text;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;
using System.Diagnostics;

namespace CodeGenerator.Components.DAL
{
    /// <summary>
    /// Data Command 类生成器。根据数据库的结构生成相应的 Command 对象
    /// todo: 对无主键表的判断和处理
    /// </summary>
    public static class Gen_DC_StoredProcedure
    {
        public static string Gen(Database db, string ns)
        {
            #region Header

            Server server = db.Parent;
            List<Table> uts = Utils.GetUserTables(db);
            List<View> uvs = Utils.GetUserViews(db);
            List<StoredProcedure> sps = Utils.GetUserStoredProcedures(db);
            List<UserDefinedFunction> ufs = Utils.GetUserFunctions(db);
            string s = "";

            StringBuilder sb = new StringBuilder();

            sb.Append(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace " + ns + @"
{
	public static partial class DC
	{");

            #endregion

            #region SPs

            sb.Append(@"
		#region Stored Procedures
");

            foreach (StoredProcedure sp in sps)
            {
                string spn = Utils.GetEscapeName(sp);
                sb.Append(@"
		private static SqlCommand _" + spn + @" = null;
        private static object _" + spn + @"_sync = new object();
		public static SqlCommand NewCmd_" + spn + @"()
		{
			if (_" + spn + @" != null) return _" + spn + @".Clone();
            lock(_" + spn + @"_sync)
            {
			    _" + spn + @" = new SqlCommand(""[" + Utils.GetEscapeSqlObjectName(sp.Schema) + "].[" + Utils.GetEscapeSqlObjectName(sp.Name) + @"]"");
			    _" + spn + @".CommandType = CommandType.StoredProcedure;
			    _" + spn + @".Parameters.Add(new SqlParameter(""RETURN_VALUE"", System.Data.SqlDbType.Int, 0, ParameterDirection.ReturnValue, false, 0, 0, null, DataRowVersion.Current, null));");
                    foreach (StoredProcedureParameter p in sp.Parameters)
                    {
                        string pn = Utils.GetEscapeName(p);
                        if (p.IsOutputParameter)
                        {
                            sb.Append(@"
			    _" + spn + @".Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.InputOutput, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");
                        }
                        else
                        {
                            if (pn.StartsWith("Original_"))
                            {
                                sb.Append(@"
			    _" + spn + @".Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Original, null));");
                            }
                            else sb.Append(@"
			    _" + spn + @".Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");
                        }

                    }
                    sb.Append(@"
			    return _" + spn + @".Clone();
            }
		}
		public static SqlCommand NewCmd_" + spn + @"(DI." + spn + @"Parameters p)
		{
			SqlCommand cmd = new SqlCommand(""[" + Utils.GetEscapeSqlObjectName(sp.Schema) + "].[" + Utils.GetEscapeSqlObjectName(sp.Name) + @"]"");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter(""RETURN_VALUE"", System.Data.SqlDbType.Int, 0, ParameterDirection.ReturnValue, false, 0, 0, null, DataRowVersion.Current, null));");
                foreach (StoredProcedureParameter p in sp.Parameters)
                {
                    string pn = Utils.GetEscapeName(p);
                    if (p.IsOutputParameter)
                    {
                        sb.Append(@"
			cmd.Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.InputOutput, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");
                    }
                    else
                    {
                        sb.Append(@"
			if (p.CheckIs" + pn + @"Changed())
			{");
                        if (pn.StartsWith("Original_"))
                        {
                            sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Original, null));");
                        }
                        else sb.Append(@"
				cmd.Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");

                        sb.Append(@"
			}");
                    }

                }
                sb.Append(@"
			return cmd;
		}
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