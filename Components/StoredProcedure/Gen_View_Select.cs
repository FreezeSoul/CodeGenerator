using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.StoredProdcedure
{
    public class Gen_View_Select : IGenComponent
    {
        #region Init

        public Gen_View_Select()
        {
            this._properties.Add(GenProperties.Name, "Gen_View_Select");
            this._properties.Add(GenProperties.Caption, "查询单条");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成根据视图的主键查询视图单条数据的存储过程脚本");
        }
        public SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.View; }
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

        public bool Validate(params object[] sqlElements)
        {
            View t = (View)sqlElements[0];
            List<Column> pks = Utils.GetPrimaryKeyColumns(t);
            return pks.Count > 0;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            View t = (View)sqlElements[0];

            List<Column> pks = Utils.GetPrimaryKeyColumns(t);

            if (pks == null || pks.Count == 0)        //没有主键？
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有主键的视图生成该过程！";
                return gr;
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            sb.Append(@"
-- 针对 视图 " + t.ToString() + @"
-- 根据主键值返回一行数据
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Select] (");
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    " + (i > 0 ? "," : "") + Utils.FormatString("@" + cn, Utils.GetParmDeclareStr(c), 40));
            }
            sb.Append(@"
) AS
BEGIN
    SET NOCOUNT ON;
");
            foreach (Column c in pks)
            {
                string cn = Utils.GetEscapeName(c);
                string cc = Utils.GetCaption(c);
                sb.Append(@"
    IF @" + cn + @" IS NULL
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Select|Required." + c.Name + @" 必须填写 " + cc + @"', 11, 1); RETURN -1;
    END;
");
            }
            sb.Append(@"

    SELECT ");
            for (int i = 0; i < t.Columns.Count; i++)
            {
                Column c = t.Columns[i];
                sb.Append((i > 0 ? @"
         , " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
            }
            sb.Append(@"
      FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]");
            string s = "";
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                if (i > 0) s += " AND ";
                s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn;
            }
            if (s.Length > 0) sb.Append(@"
     WHERE " + s);
            sb.Append(@"

    RETURN 0
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 视图 " + t.ToString() + @"
根据主键值返回一行数据' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Select'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_IsSingleLineResult', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Select'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Select'

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
