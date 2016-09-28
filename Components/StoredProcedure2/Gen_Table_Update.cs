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
    public class Gen_Table_UpdatePart : IGenComponent
    {
        #region Init

        public Gen_Table_UpdatePart()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_UpdatePart");
            this._properties.Add(GenProperties.Caption, "更新单条（部分字段）");
            this._properties.Add(GenProperties.Group, "生成过程脚本_UPDATE类（RETURN带错误返回）");
            this._properties.Add(GenProperties.Tips, "生成更新单条数据部分字段的存储过程");
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

            if (wcs == null || wcs.Count == 0)
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为没有可写入字段的表生成该过程！";
                return gr;
            }
            if (pks == null || pks.Count == 0)
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
-- 根据主键的值定位一行数据并修改
-- 操作成功返回 受影响行数, 失败返回
-- -1: 主键为空
-- -2: 主键未找到
-- -3: 为必填字段传入了空值
-- -4: 欲更新的主键值已存在
-- -5: 外键在相关表中未找到
-- -6: 更新失败
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Update] (");
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    " + (i > 0 ? ", " : "  ") + Utils.FormatString("@Original_" + cn, Utils.GetParmDeclareStr(c), 40));
            }
            for (int i = 0; i < wcs.Count; i++)
            {
                Column c = wcs[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    , " + Utils.FormatString("@" + cn, Utils.GetParmDeclareStr(c), "= NULL", 40, 40));
            }
            sb.Append(@"
) AS
BEGIN
    SET NOCOUNT ON;
");
            //判断原始主键必填
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                string cc = Utils.GetCaption(c);
                sb.Append(@"
    IF @Original_" + cn + @" IS NULL RETURN -1;
");
            }

            //判断原始主键必存在
            sb.Append(@"
	-- 这里也可以先不判断原记录是否存在，直接取值，并任意判断一个非空值是否取到
    IF NOT EXISTS (
        SELECT 1 FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
--          WITH (UPDLOCK)
         WHERE ");
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                if (i > 0) sb.Append(@" AND ");
                sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
            }
            sb.Append(@"
    ) RETURN -2;
");

            // 从原数据行读出必填字段原始值并锁定

            // 声明字段原始值
            string s = "";
            foreach (Column c in wcs)
            {
                if (pks.Contains(c)) continue;            // 主键已传入了原始值
                if (c.Nullable) continue;
                string cn = Utils.GetEscapeName(c);
                string cc = Utils.GetCaption(c);
                if (mwcs.Contains(c))
                {
                    sb.Append(@"
    DECLARE @Original_" + cn + @" " + Utils.GetParmDeclareStr(c) + @";");

                    s += (s.Length > 0 ? ", " : "") + "@Original_" + cn + @" = [" + Utils.GetEscapeSqlObjectName(c.Name) + @"]";
                }
            }
            if (s.Length > 0)
            {
                sb.Append(@"
    SELECT " + s + @"
      FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
--      WITH (UPDLOCK)
     WHERE ");
                for (int i = 0; i < pks.Count; i++)
                {
                    Column c = pks[i];
                    string cn = Utils.GetEscapeName(c);
                    if (i > 0) sb.Append(@" AND ");
                    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
                }
                sb.Append(@";");
            }

            //判断必填字段是否填写了空值
            foreach (Column c in wcs)
            {
                if (c.Nullable) continue;
                string cn = Utils.GetEscapeName(c);
                string cc = Utils.GetCaption(c);
                if (mwcs.Contains(c))
                {
                    sb.Append(@"
    IF @" + cn + @" IS NULL SET @" + cn + @" = @Original_" + cn + @";
    -- ELSE IF LEN(@" + cn + @") = 0 RETURN -3;
");
                }
                else
                {
                    sb.Append(@"
    -- IF @" + cn + @" IS NULL SET @" + cn + @" = " + c.DefaultConstraint.Text + @";
");
                }
            }


            //判断新主键值是否和已有主键重复
            if (pks.Count == 1 && pks[0].Identity)
            {
            }
            else
            {
                sb.Append(@"
    IF ");
                bool isFirst = true;
                foreach (Column c in pks)
                {
                    if (c.Identity) continue;
                    string cn = Utils.GetEscapeName(c);
                    string cc = Utils.GetCaption(c);
                    if (!isFirst) sb.Append(@" OR ");
                    sb.Append(@"@Original_" + cn + @" <> @" + cn);
                    isFirst = false;
                }
                sb.Append(@"
    BEGIN
        IF EXISTS (
            SELECT 1 FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
             WHERE ");
                for (int i = 0; i < pks.Count; i++)
                {
                    Column c = pks[i];
                    string cn = Utils.GetEscapeName(c);
                    string cc = Utils.GetCaption(c);
                    if (i > 0) sb.Append(@" AND ");
                    sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn);
                }
                sb.Append(@"
        ) RETURN -4;
    END;
");
                //判断外键字段是否在外键表中存在
                foreach (ForeignKey fk in t.ForeignKeys)
                {
                    Table ft = t.Parent.Tables[fk.ReferencedTable, fk.ReferencedTableSchema];
                    sb.Append(@"
    IF NOT EXISTS (
        SELECT 1 FROM [" + Utils.GetEscapeSqlObjectName(ft.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(ft.Name) + @"]
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
    ) RETURN -5;
");
                }
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

    UPDATE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
       SET ");
            for (int i = 0; i < wcs.Count; i++)
            {
                Column c = wcs[i];
                string cn = Utils.GetEscapeName(c);
                sb.Append((i > 0 ? @"
         , " : "") + Utils.FormatString("[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]", "= @" + cn, 40));
            }
            s = "";
            for (int i = 0; i < pks.Count; i++)
            {
                Column c = pks[i];
                string cn = Utils.GetEscapeName(c);
                if (i > 0) s += " AND ";
                s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @Original_" + cn;
            }
            if (s.Length > 0) sb.Append(@"
     WHERE " + s);
            sb.Append(@";

    SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT;
    IF @ERROR <> 0 OR @ROWCOUNT = 0
    BEGIN
/*
        @ReturnValue = -6;
        GOTO Cleanup;
*/
        RETURN -6;
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
根据主键的值定位一行数据并修改
操作成功返回 受影响行数, 失败返回
-1: 主键为空
-2: 主键未找到
-3: 为必填字段传入了空值
-4: 欲更新的主键值已存在
-5: 外键在相关表中未找到
-6: 更新失败' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_UpdatePart'
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
