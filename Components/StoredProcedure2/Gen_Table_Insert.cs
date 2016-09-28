using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.StoredProdcedure2
{
    public class Gen_Table_Insert : IGenComponent
    {
        #region Init

        public Gen_Table_Insert()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_Insert");
            this._properties.Add(GenProperties.Caption, "插入单条");
            this._properties.Add(GenProperties.Group, "生成过程脚本_INSERT类（RETURN带错误返回）");
            this._properties.Add(GenProperties.Tips, "生成插入单条数据的存储过程");
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
            List<Column> wcs = Utils.GetWriteableColumns(t);

            return !(wcs.Count == 0 || pks.Count == 0);
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];


            List<Column> pks = Utils.GetPrimaryKeyColumns(t);
            List<Column> wcs = Utils.GetWriteableColumns(t);
            List<Column> mwcs = Utils.GetMustWriteColumns(t);

            if (wcs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有可写入字段的表生成该过程！";
                return gr;
            }
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
-- 添加一行数据
-- 操作成功返回 受影响行数; 失败返回
-- -1: 主键为空
-- -2: 主键冲突
-- -3: 外键无效
-- -4: 添加失败
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Insert] (");
            for (int i = 0; i < wcs.Count; i++)
            {
                Column c = wcs[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    " + (i > 0 ? ", " : "  ") + Utils.FormatString("@" + cn, Utils.GetParmDeclareStr(c), "= NULL", 40, 40));
            }
            sb.Append(@"
) AS
BEGIN
    SET NOCOUNT ON;
");
            //判断必填字段是否填写了空值
            foreach (Column c in wcs)
            {
                if (c.Nullable) continue;
                string cn = Utils.GetEscapeName(c);
                string cc = Utils.GetCaption(c);
                if (mwcs.Contains(c))
                {
                    sb.Append(@"
    IF @" + cn + @" IS NULL" + (Utils.CheckIsStringType(c) ? ("-- OR LEN(@" + cn + @") = 0") : ("")) + @"
        RETURN -1;
");
                }
                else
                {
                    sb.Append(@"
    IF @" + cn + @" IS NULL SET @" + cn + @" = " + c.DefaultConstraint.Text + @";
");
                }
            }

            //判断主键重复
            //判断是否存在自增主键
            bool hasIdentityCol = false;
            foreach (Column c in pks)
            {
                if (c.Identity)
                {
                    hasIdentityCol = true;
                    break;
                }
            }
            if (!hasIdentityCol)
            {
                sb.Append(@"
    IF EXISTS (
       SELECT 1 FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
--         WITH (TABLOCK, HOLDLOCK)
        WHERE ");
                for (int i = 0; i < pks.Count; i++)
                {
                    Column c = pks[i];
                    string cn = Utils.GetEscapeName(c);
                    string cc = Utils.GetCaption(c);
                    if (i > 0) sb.Append(@" AND ");
                    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
                }
                sb.Append(@"
    ) RETURN -2;
");
            }

            //判断外键字段是否在外键表中存在
            foreach (ForeignKey fk in t.ForeignKeys)
            {
                Table ft = t.Parent.Tables[fk.ReferencedTable, fk.ReferencedTableSchema];
                sb.Append(@"
    IF NOT EXISTS (
        SELECT 1 FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(ft.Name) + @"]
         WHERE ");
                string s1 = "";
                for (int i = 0; i < fk.Columns.Count; i++)
                {
                    ForeignKeyColumn fkc = fk.Columns[i];
                    Column c = t.Columns[fkc.Name];
                    string cn = Utils.GetEscapeName(c);
                    string cc = Utils.GetCaption(c);

                    if (i > 0) sb.Append(@" AND ");
                    sb.Append("(" + (c.Nullable ? (" @" + cn + @" IS NULL OR ") : "") + @"[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"] = @" + cn + @")");
                    if (i > 0) s1 += "，";
                    s1 += Utils.GetCaption(t.Columns[fkc.Name]);
                }
                sb.Append(@"
    ) RETURN -3;
");
            }

            sb.Append(@"

/*
    --prepare trans & error
    DECLARE @TranStarted bit, @ReturnValue int;
    SELECT @TranStarted = 0, @ReturnValue = 0;
    IF @@TRANCOUNT = 0 
    BEGIN
        BEGIN TRANSACTION;
        SET @TranStarted = 1
    END;
*/

    DECLARE @ERROR INT, @ROWCOUNT INT;

    INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (");
            for (int i = 0; i < wcs.Count; i++)
            {
                Column c = wcs[i];
                sb.Append(@"
        " + (i > 0 ? ", " : "  ") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
            }
            sb.Append(@"
    )
--    OUTPUT Inserted.*
    VALUES (");
            for (int i = 0; i < wcs.Count; i++)
            {
                Column c = wcs[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
        " + (i > 0 ? ", " : "  ") + "@" + cn);
            }
            sb.Append(@"
    );");
                sb.Append(@"
    SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT;
    IF @ERROR <> 0 OR @ROWCOUNT = 0
    BEGIN
/*
        @ReturnValue = -4;
        GOTO Cleanup;
*/
        RETURN -4;
    END

/*
    @ReturnValue = @ROWCOUNT;
    GOTO Cleanup;
*/

    RETURN @ROWCOUNT;

/*
    --cleanup trans
    IF @TranStarted = 1 COMMIT TRANSACTION;
    RETURN @ReturnValue;
Cleanup:
    IF @TranStarted = 1 ROLLBACK TRANSACTION;
    RETURN @ReturnValue;
*/
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 表 " + t.ToString() + @"
添加一行数据
操作成功返回 受影响行数; 失败返回
-1: 主键为空
-2: 主键冲突
-3: 外键无效
-4: 添加失败' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Insert'
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
