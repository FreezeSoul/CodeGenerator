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
	public class Gen_Table_Insert : IGenComponent
	{
		#region Init

		public Gen_Table_Insert()
		{
			this._properties.Add(GenProperties.Name, "Gen_Table_Insert");
			this._properties.Add(GenProperties.Caption, "插入单条");
			this._properties.Add(GenProperties.Group, "生成过程脚本_INSERT类");
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
-- 添加一行数据并返回
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
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Insert|Required." + c.Name + @" 必须填写 " + cc + @"', 11, 1); RETURN -1;
    END;
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
    )
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Insert|Exists.PrimaryKeys 欲插入的主键值已存在', 11, 1); RETURN -1;
    END;
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
    )
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Insert|NotFound." + s1 + @" " + s1 + @" 的值在表：" + Utils.GetCaption(ft) + @" 中未找到', 11, 1); RETURN -1;
    END;
");
			}

			sb.Append(@"

/*
    --prepare trans & error
    DECLARE @TranStarted bit; SET @TranStarted = 0; IF @@TRANCOUNT = 0 BEGIN BEGIN TRANSACTION; SET @TranStarted = 1 END;
*/

");

			if (_db.CompatibilityLevel >= CompatibilityLevel.Version90)
			{
				sb.Append(@"

    INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (");
				for (int i = 0; i < wcs.Count; i++)
				{
					Column c = wcs[i];
					sb.Append(@"
        " + (i > 0 ? ", " : "  ") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@"
    )
    OUTPUT Inserted.*
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
			}
			else
			{

				sb.Append(@"

    INSERT INTO [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] (");
				for (int i = 0; i < wcs.Count; i++)
				{
					Column c = wcs[i];
					sb.Append(@"
        " + (i > 0 ? ", " : "  ") + "[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
				}
				sb.Append(@"
    ) VALUES (");
				for (int i = 0; i < wcs.Count; i++)
				{
					Column c = wcs[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
        " + (i > 0 ? ", " : "  ") + "@" + cn);
				}
				sb.Append(@"
    );
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
					if (c.Identity)
					{
						s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = SCOPE_IDENTITY()";
					}
					else s += @"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn;
				}
				if (s.Length > 0) sb.Append(@"
     WHERE " + s);
			}

			sb.Append(@";
    IF @@ERROR <> 0 OR @@ROWCOUNT = 0
    BEGIN
        RAISERROR ('" + t.Schema + @"." + t.Name + @".Insert|Failed 数据插入失败', 11, 1); RETURN -1;  -- GOTO Cleanup;
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
添加一行数据并返回' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Insert'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_IsSingleLineResult', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Insert'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_Insert'

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
