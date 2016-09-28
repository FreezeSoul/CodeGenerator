using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;
using System.Data;

namespace CodeGenerator.Components.BLL
{
    class Gen_Table_Enum : IGenComponent
    {
        #region Init

        public Gen_Table_Enum()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_Enum");
            this._properties.Add(GenProperties.Caption, "根据数据生成 Enum");
            this._properties.Add(GenProperties.Group, "C#");
            this._properties.Add(GenProperties.Tips, "为 Table 里面的数据生成 C# 的枚举代码");
            this._properties.Add(GenProperties.IsEnabled, false);
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
        /// 通过条件：只有一个数字类型主键
        /// </summary>
        public bool Validate(params object[] sqlElements)
        {
            Table t = (Table)sqlElements[0];

            bool b = false;
            List<Column> pks = Utils.GetPrimaryKeyColumns(t);
            if (pks.Count == 1)
            {
                if (Utils.CheckIsNumericType(pks[0])) b = true;
            }
            return b;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];

            List<Column> pks = Utils.GetPrimaryKeyColumns(t);

            if (pks.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有主键字段的表生成该代码！";
                return gr;
            }
            else if (pks.Count > 1)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为多主键字段的表生成该代码！";
                return gr;
            }
            else if (!Utils.CheckIsNumericType(pks[0]))
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为非数字型主键字段的表生成该代码！";
                return gr;
            }

            Column vc = pks[0];
            Column nc = null;

            List<Column> sacs = Utils.GetSearchableColumns(t);
            if (sacs.Count == 0)
            {
                nc = vc;
            }
            else
            {
                nc = sacs[0];
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            string tbn = Utils.GetEscapeSqlObjectName(t.Name);

            sb.Append(@"/// <summary>
/// " + Utils.GetDescription(t) + @"
/// </summary>
public enum " + tbn + @"
{");
            DataSet ds = _db.ExecuteWithResults("SELECT [" + Utils.GetEscapeSqlObjectName(vc.Name) + "], [" + Utils.GetEscapeSqlObjectName(nc.Name) + "] FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + "].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ORDER BY [" + Utils.GetEscapeSqlObjectName(nc.Name) + "]");
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "当前表中没有数据！生成失败！";
                return gr;
            }

            foreach (DataRow c in ds.Tables[0].Rows)
            {
                sb.Append(@"
	" + Utils.GetEscapeName(c[nc.Name].ToString()) + @" = " + c[vc.Name].ToString() + @",");
            }
            sb.Append(@"
}
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
