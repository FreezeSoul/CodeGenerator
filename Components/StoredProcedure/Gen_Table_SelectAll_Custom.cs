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
    public class Gen_Table_SelectAll_Custom : IGenComponent
    {
        #region Init

        public Gen_Table_SelectAll_Custom()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_SelectAll_Custom");
            this._properties.Add(GenProperties.Caption, "查询所有(带条件)");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成查询表所有数据带条件的存储过程脚本");
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

        public bool Validate(params object[] sqlElements)
        {
            return true;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen
            string strLen = (_db.CompatibilityLevel >= CompatibilityLevel.Version90) ? "MAX" : "4000";
            sb.Append(@"
-- 针对 表 " + t.ToString() + @"
-- 根据填写的查询字串返回数行数据
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Custom] (
    @WhereString            NVARCHAR(" + strLen + @")        = NULL
) AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SqlStr NVARCHAR(" + strLen + @")

    IF @WhereString IS NULL SET @WhereString = '';
    ELSE IF @WhereString <> '' SET @WhereString = ' WHERE ' + @WhereString;

    SET @SqlStr = '
    SELECT ");
            for (int i = 0; i < t.Columns.Count; i++)
            {
                Column c = t.Columns[i];
                sb.Append((i > 0 ? @"
         , " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
            }
            sb.Append(@"
      FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ' + @WhereString;
    EXEC sp_executesql @SqlStr;
    
    RETURN 0
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 表 " + t.ToString() + @"
根据填写的查询字串返回数行数据' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Custom'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Custom'

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
