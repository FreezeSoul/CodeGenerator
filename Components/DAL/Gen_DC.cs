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
    public static class Gen_DC
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
	/// <summary>
	/// 生成用于操作数据库的命令对象的静态方法集合类
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
	public static partial class DC
	{");

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