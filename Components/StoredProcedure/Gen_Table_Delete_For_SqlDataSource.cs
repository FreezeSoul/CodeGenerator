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
    public class Gen_Table_Delete_For_SqlDataSource : IGenComponent
    {
        #region Init

        public Gen_Table_Delete_For_SqlDataSource()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_Delete_For_SqlDataSource");
            this._properties.Add(GenProperties.Caption, "删除单条(用于SqlDataSource)");
            this._properties.Add(GenProperties.Group, "生成过程脚本_DELETE类");
            this._properties.Add(GenProperties.Tips, "生成根据主键删除单行数据(符合SqlDataSource参数要求)的存储过程脚本");
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
            Table t = (Table)sqlElements[0];

            List<Column> pks = Utils.GetPrimaryKeyColumns(t);

            return !(pks.Count == 0);
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
                gr.Message = "无法为没有主键字段的表生成该过程！";
                return gr;
            }

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            sb.Append(@"
-- 针对 表 " + t.ToString() + @"
-- 根据新旧主键值删除一行数据（这是为 SqlDataSource 的删除方法准备的）
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Delete_ForSqlDataSource] (");
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    " + (i > 0 ? ", " : "  ") + Utils.FormatString("@" + cn, Utils.GetParmDeclareStr(c), "= NULL", 40, 40));
            }
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    , " + Utils.FormatString("@Original_" + cn, Utils.GetParmDeclareStr(c), 30));
            }
            sb.Append(@"
) AS
BEGIN
    SET NOCOUNT ON;
");
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    IF @" + cn + @" IS NULL
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Delete|Required." + c.Name + @" " + cn + @"不能为空', 11, 1); RETURN -1;
    END;
");
            }
            sb.Append(@"
/*
    --prepare trans & error
    DECLARE @TranStarted bit; SET @TranStarted = 0; IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @TranStarted = 1 END;
*/


    DELETE
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
    IF @@ERROR <> 0 OR @@ROWCOUNT = 0
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Delete|Failed 数据删除失败', 11, 1); RETURN -1;  -- GOTO Cleanup;
    END


/*
    --cleanup trans
    IF @TranStarted = 1 COMMIT TRANSACTION; RETURN 0;
Cleanup:
    IF @TranStarted = 1 ROLLBACK TRANSACTION; RETURN -1;
*/

    RETURN 0;

END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 表 " + t.ToString() + @"
根据新旧主键值删除一行数据（这是为 SqlDataSource 的删除方法准备的）' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Delete_ForSqlDataSource'

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
