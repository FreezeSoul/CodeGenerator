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
    public class Gen_Table_SelectAll_Page_Blur : IGenComponent
    {
        #region Init

        public Gen_Table_SelectAll_Page_Blur()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_SelectAll_Page_Blur");
            this._properties.Add(GenProperties.Caption, "查询所有(翻页,模糊查询)");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成查询表所有数据(翻页,模糊查询)的存储过程脚本");
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
            if (_db.CompatibilityLevel < CompatibilityLevel.Version90) return false;

            Table t = (Table)sqlElements[0];
            List<Column> scs = Utils.GetSearchableColumns(t);
            List<Column> socs = Utils.GetSortableColumns(t);
            return socs.Count > 0 && scs.Count > 0;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];

            List<Column> scs = Utils.GetSearchableColumns(t);
            List<Column> socs = Utils.GetSortableColumns(t);

            if (socs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有可排序字段的表生成该过程！";
                return gr;
            }
            if (scs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有字符串类型字段的表生成模糊查询过程！";
                return gr;
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            sb.Append(@"
-- 针对 表 " + t.ToString() + @"
-- 根据关键字返回数行数据（带分页排序）
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Blur] (
    @Keyword                        NVARCHAR(max)    = NULL,
    @SortExpression                 NVARCHAR(max)    = NULL,            -- column name
    @SortDirection                  INT              = NULL,            -- 0 : asc  1: desc
    @PageSize                       INT              = NULL,            -- page size
    @StartRowIndex                  INT              = NULL,            -- current page first row index
    @Count                          INT              = NULL    OUTPUT   -- rows count
) AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @EndRowIndex INT;

    IF @Keyword IS NULL SET @Keyword = '%';
    IF @SortExpression IS NULL OR @SortExpression = '' SET @SortExpression = '" + socs[0].Name + @"';
    IF @SortDirection IS NULL SET @SortDirection = 0;

    IF @PageSize IS NULL OR @PageSize < 1 SET @PageSize = 20;
    IF @StartRowIndex IS NULL OR @StartRowIndex < 0 SET @StartRowIndex = 0;
    SET @StartRowIndex = @StartRowIndex + 1;
    SET @EndRowIndex = @StartRowIndex + @PageSize - 1;

    SELECT @Count = COUNT(*)
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

            for (int j = 0; j < scs.Count; j++)
            {
                Column sc = scs[j];
                sb.Append(@"
    " + (j > 0 ? "ELSE " : "") + @"IF @SortExpression = '" + sc.Name + @"'
    BEGIN
        IF @SortDirection = 0
        BEGIN
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
                     , ROW_NUMBER() OVER (ORDER BY [" + Utils.GetEscapeSqlObjectName(sc.Name) + @"]) AS 'RowNumber'
                  FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]");
                s = "";
                for (int i = 0; i < scs.Count; i++)
                {
                    Column c = scs[i];
                    if (i > 0) s += " OR ";
                    s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE @Keyword";
                }
                if (s.Length > 0) sb.Append(@"
                 WHERE " + s);
                sb.Append(@"
            )
            SELECT  ");
                for (int i = 0; i < t.Columns.Count; i++)
                {
                    Column c = t.Columns[i];
                    sb.Append((i > 0 ? @"
                 , " : "") + @"T.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                }
                sb.Append(@"
              FROM T
             WHERE T.RowNumber BETWEEN @StartRowIndex AND @EndRowIndex
        END
        ELSE
        BEGIN
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
                     , ROW_NUMBER() OVER (ORDER BY [" + Utils.GetEscapeSqlObjectName(sc.Name) + @"] DESC) AS 'RowNumber'
                  FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]");
                s = "";
                for (int i = 0; i < scs.Count; i++)
                {
                    Column c = scs[i];
                    if (i > 0) s += " OR ";
                    s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] LIKE @Keyword";
                }
                if (s.Length > 0) sb.Append(@"
                 WHERE " + s);
                sb.Append(@"
            )
            SELECT  ");
                for (int i = 0; i < t.Columns.Count; i++)
                {
                    Column c = t.Columns[i];
                    sb.Append((i > 0 ? @"
                 , " : "") + @"T.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                }
                sb.Append(@"
              FROM T
             WHERE T.RowNumber BETWEEN @StartRowIndex AND @EndRowIndex
        END
    END
");
            }
            sb.Append(@"

    RETURN 0
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 表 " + t.ToString() + @"
根据关键字返回数行数据（带分页排序）' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Blur'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectAll_Page_Blur'

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
