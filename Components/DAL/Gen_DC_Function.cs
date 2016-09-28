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
    public static class Gen_DC_Function
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

            #region Functions

            sb.Append(@"
		#region User Defined Functions
");
            foreach (UserDefinedFunction f in ufs)
            {

                string tbn = Utils.GetEscapeName(f);
                sb.Append(@"
		private static SqlCommand _" + tbn + @"_cmd = null;
        private static object _" + tbn + @"_cmd_sync = new object();
		public static SqlCommand NewCmd_" + tbn + @"()
		{
		    if (_" + tbn + @"_cmd != null) return _" + tbn + @"_cmd.Clone();
            lock(_" + tbn + @"_cmd_sync)
            {");
                if (f.FunctionType == UserDefinedFunctionType.Table || f.FunctionType == UserDefinedFunctionType.Inline)
                {
                    sb.Append(@"
			    _" + tbn + @"_cmd = new SqlCommand(""SELECT * FROM [" + Utils.GetEscapeSqlObjectName(f.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(f.Name) + @"](");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        UserDefinedFunctionParameter p = f.Parameters[i];
                        string pn = Utils.GetEscapeName(p);
                        if (i > 0) sb.Append(", ");
                        sb.Append("@" + pn);
                    }

                    sb.Append(@")"");");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        UserDefinedFunctionParameter p = f.Parameters[i];
                        string pn = Utils.GetEscapeName(p);
                        sb.Append(@"
			    _" + tbn + @"_cmd.Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");

                    }

                    sb.Append(@"
			    return _" + tbn + @"_cmd.Clone();");
                }

                if (f.FunctionType != UserDefinedFunctionType.Table && f.FunctionType != UserDefinedFunctionType.Inline)
                {
                    sb.Append(@"
			    _" + tbn + @"_cmd = new SqlCommand(""SELECT [" + Utils.GetEscapeSqlObjectName(f.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(f.Name) + @"](");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        UserDefinedFunctionParameter p = f.Parameters[i];
                        string pn = Utils.GetEscapeName(p);
                        if (i > 0) sb.Append(", ");
                        sb.Append("@" + pn);
                    }

                    sb.Append(@")"");");
                    for (int i = 0; i < f.Parameters.Count; i++)
                    {
                        UserDefinedFunctionParameter p = f.Parameters[i];
                        string pn = Utils.GetEscapeName(p);
                        sb.Append(@"
			    _" + tbn + @"_cmd.Parameters.Add(new SqlParameter(""" + pn + @""", " + Utils.GetSqlDbType(p) + @", " + p.DataType.MaximumLength.ToString() + @", ParameterDirection.Input, false, " + p.DataType.NumericPrecision.ToString() + @", " + p.DataType.NumericScale.ToString() + @", """ + pn + @""", DataRowVersion.Current, null));");

                    }

                    sb.Append(@"
			    return _" + tbn + @"_cmd.Clone();");

                }

                sb.Append(@"
            }
		}");


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