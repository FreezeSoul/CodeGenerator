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
    public class Gen_View_SelectAll_Page_Custom : IGenComponent
    {
        #region Init

        public Gen_View_SelectAll_Page_Custom()
        {
            this._properties.Add(GenProperties.Name, "Gen_View_SelectAll_Page_Custom");
            this._properties.Add(GenProperties.Caption, "查询所有(翻页,条件)");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成查询视图所有数据(翻页,条件)的存储过程脚本");
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
            if (_db.CompatibilityLevel < CompatibilityLevel.Version90) return false;

            View t = (View)sqlElements[0];
            List<Column> socs = Utils.GetSortableColumns(t);
            return socs.Count > 0;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            View t = (View)sqlElements[0];

            List<Column> scs = Utils.GetSearchableColumns(t);
            List<Column> socs = Utils.GetSortableColumns(t);

            if (socs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有可排序字段的视图生成该过程！";
                return gr;
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            sb.Append(@"
-- 针对 视图 " + t.ToString() + @"
-- 根据填写的查询字串返回数行数据（带分页排序）
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Custom] (
    @WhereString                NVARCHAR(MAX)    = NULL,            -- tsql expression
    @SortExpression             NVARCHAR(MAX)    = NULL,            -- column name
    @SortDirection              INT              = NULL,            -- 0 : asc  1: desc
    @PageSize                   INT              = NULL,            -- page size
    @StartRowIndex              INT              = NULL,            -- current page first row index
    @Count                      INT              = NULL    OUTPUT   -- rows count
) AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SqlStr NVARCHAR(MAX)
    DECLARE @SqlParm NVARCHAR(MAX)
    DECLARE @EndRowIndex INT;

    IF @WhereString IS NULL SET @WhereString = '';
    ELSE IF @WhereString <> '' SET @WhereString = ' WHERE ' + @WhereString;
    IF @SortExpression IS NULL OR @SortExpression = '' SET @SortExpression = '" + socs[0].Name + @"';
    IF @SortDirection IS NULL SET @SortDirection = 0;
    IF @SortDirection = 1 SET @SortExpression = @SortExpression + ' DESC'

    IF @PageSize IS NULL OR @PageSize < 1 SET @PageSize = 20;
    IF @StartRowIndex IS NULL OR @StartRowIndex < 0 SET @StartRowIndex = 0;
    SET @StartRowIndex = @StartRowIndex + 1;
    SET @EndRowIndex = @StartRowIndex + @PageSize - 1;

    SET @SqlStr = 'SELECT @Count = COUNT(*) FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]' + @WhereString;
    SET @SqlParm = '@Count INT OUTPUT';
    EXEC sp_executesql @SqlStr, @SqlParm, @Count OUTPUT;

    SET @SqlStr = '
    WITH T AS
    (
        SELECT ");
            for (int i = 0; i < t.Columns.Count; i++)
            {
                Column c = t.Columns[i];
                sb.Append((i > 0 ? @"
             , " : "") + @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
            }
            sb.Append(@"
             , ROW_NUMBER() OVER (ORDER BY ' + @SortExpression + ') AS ''RowNumber''
          FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] ' + @WhereString + '
    )
    SELECT ");
            for (int i = 0; i < t.Columns.Count; i++)
            {
                Column c = t.Columns[i];
                sb.Append((i > 0 ? @"
         , " : "") + @"T.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
            }
            sb.Append(@"
    FROM T
    WHERE T.RowNumber BETWEEN @StartRowIndex AND @EndRowIndex
    ';
    SET @SqlParm = '@StartRowIndex INT, @EndRowIndex INT';
    EXEC sp_executesql @SqlStr, @SqlParm, @StartRowIndex, @EndRowIndex;

    RETURN 0
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 视图 " + t.ToString() + @"
根据填写的查询字串返回数行数据（带分页排序）' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Custom'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Custom'

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
