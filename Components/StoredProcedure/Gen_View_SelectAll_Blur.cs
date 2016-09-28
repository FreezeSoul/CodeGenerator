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
    public class Gen_View_SelectAll_Blur : IGenComponent
    {
        #region Init

        public Gen_View_SelectAll_Blur()
        {
            this._properties.Add(GenProperties.Name, "Gen_View_SelectAll_Blur");
            this._properties.Add(GenProperties.Caption, "查询所有(带模糊查询)");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成查询视图所有数据带模糊查询条件的存储过程脚本");
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

            List<Column> scs = Utils.GetSearchableColumns(t);

            return scs.Count > 0;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            View t = (View)sqlElements[0];

            List<Column> scs = Utils.GetSearchableColumns(t);

            if (scs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有字符串类型字段的视图生成模糊查询过程！";
                return gr;
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen
            string strLen = (_db.CompatibilityLevel >= CompatibilityLevel.Version90) ? "MAX" : "4000";
            sb.Append(@"
-- 针对 视图 " + t.ToString() + @"
-- 根据关键字返回数行数据
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Blur] (
       @Keyword             NVARCHAR(" + strLen + @")        = NULL
) AS
BEGIN
    SET NOCOUNT ON;

    IF @Keyword IS NULL OR @Keyword = '' SET @Keyword = '%';
    ELSE SET @Keyword = '%' + @Keyword + '%';

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
            for (int i = 0; i < scs.Count; i++)
            {
                Column c = scs[i];
                if (i > 0) s += " OR ";
                s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE @Keyword";
            }
            if (s.Length > 0) sb.Append(@"
     WHERE " + s);
            sb.Append(@"
    RETURN 0
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 视图 " + t.ToString() + @"
根据关键字返回数行数据' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Blur'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Blur'

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
