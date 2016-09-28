using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.BLL
{
    public class Gen_Table_UserDefinedTableType : IGenComponent
    {
        #region Init

        public Gen_Table_UserDefinedTableType()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_UserDefinedTableType");
            this._properties.Add(GenProperties.Caption, "生成 用户定义表类型 创建脚本");
            this._properties.Add(GenProperties.Group, "生成TSQL脚本");
            this._properties.Add(GenProperties.Tips, "根据 Table 结构生成 用户定义表类型 创建脚本");
        }
        public SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Table; }
        }

        #endregion

        #region Misc

        Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
        public Dictionary<GenProperties, object> Properties
        {
            get
            {
                return this._properties;
            }
        }

        public event System.ComponentModel.CancelEventHandler OnProcessing;

        private Server _server;
        public Server Server
        {
            set { _server = value; }
        }

        private Database _db;
        public Database Database
        {
            set { _db = value; }
        }

        #endregion

        /// <summary>
        /// 通过条件：无条件通过
        /// </summary>
        public bool Validate(params object[] sqlElements)
        {
            return true;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];

            List<Column> pkcs = Utils.GetPrimaryKeyColumns(t);

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            string sn = Utils.GetEscapeSqlObjectName(t.Schema);
            string tn = "udtt_" + Utils.GetEscapeSqlObjectName(t.Name);

            sb.Append(@"
CREATE TYPE [" + sn + @"].[" + tn + @"] AS TABLE(");
            for (int i = 0; i < t.Columns.Count; i++)
            {
                Column c = t.Columns[i];
                string cn = Utils.GetEscapeSqlObjectName(c.Name);
                string dn = Utils.GetParmDeclareStr(c);
                sb.Append(@"
	[" + cn + @"] " + dn + @" NOT NULL" + (i < t.Columns.Count - 1 ? "," : ""));
            }

            if (pkcs.Count > 0)
            {
                sb.Append(@"
	PRIMARY KEY CLUSTERED 
    (");
                for (int i = 0; i < pkcs.Count; i++)
                {
                    Column c = pkcs[i];
                    string cn = Utils.GetEscapeSqlObjectName(c.Name);
                    sb.Append(@"
        [" + cn + @"] ASC" + (i < pkcs.Count - 1 ? "," : ""));
                }
                sb.Append(@"
    ) WITH ( IGNORE_DUP_KEY = OFF )");
            }
            sb.Append(@"
)
");

            #endregion

            #region return

            gr = new GenResult(GenResultTypes.CodeSegment);
            gr.CodeSegment = new KeyValuePair<string, string>(this._properties[GenProperties.Tips].ToString(), sb.ToString());
            return gr;

            #endregion
        }
    }
}
