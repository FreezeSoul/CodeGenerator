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
    public class Gen_Table_SelectNode : IGenComponent
    {
        #region Init

        public Gen_Table_SelectNode()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_SelectNode");
            this._properties.Add(GenProperties.Caption, "查询节点（CTE版）");
            this._properties.Add(GenProperties.Group, "生成过程脚本_SELECT类");
            this._properties.Add(GenProperties.Tips, "生成根据表的主键查询表节点数据的存储过程脚本");
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
            return Utils.CheckIsTree(t);
        }

        public GenResult Gen(params object[] sqlElements)
        {
            #region Init

            GenResult gr;
            Table t = (Table)sqlElements[0];

            if (!Utils.CheckIsTree(t))
            {
                gr = new GenResult(GenResultTypes.Message);
                gr.Message = "无法为非树表生成该过程！";
                return gr;
            }

            List<Column> pks = Utils.GetPrimaryKeyColumns(t);

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            foreach (ForeignKey fk in t.ForeignKeys)
            {
                if (fk.ReferencedTable != t.Name || fk.ReferencedTableSchema != t.Schema) continue;
                int equaled = 0;
                foreach (ForeignKeyColumn fkc in fk.Columns)        //判断是否一个外键约束所有字段都是在当前表
                {
                    if (fkc.Parent.Parent == t) equaled++;
                }
                if (equaled == fk.Columns.Count)                    //当前表为树表
                {
                    sb.Append(@"
-- 针对 表 " + t.ToString() + @"
-- 根据主键值返回一个节点的多行数据
CREATE PROCEDURE [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectNode] (");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        sb.Append(@"
    " + (i > 0 ? ", " : "  ") + Utils.FormatString("@" + cn, Utils.GetParmDeclareStr(c), "= NULL", 40, 40));
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
        RAISERROR ('" + t.Schema + @"." + t.Name + @".SelectNode|Required." + c.Name + @" " + cn + @" 不能为空', 11, 1); RETURN -1;
    END;");
                    }
                    sb.Append(@"
    WITH Node(");
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        if (i > 0) sb.Append(@", ");
                        sb.Append(@"[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"]");
                    }
                    sb.Append(@")
    AS
    (
        SELECT ");
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        sb.Append((i > 0 ? @"
             , " : "") + @"[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"]");
                    }
                    sb.Append(@"
          FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"]
         WHERE ");
                    for (int i = 0; i < pks.Count; i++)
                    {
                        Column c = pks[i];
                        string cn = Utils.GetEscapeName(c);
                        if (i > 0) sb.Append(" AND ");
                        sb.Append(@"[" + Utils.GetEscapeSqlObjectName(c.Name) + @"] = @" + cn);
                    }
                    sb.Append(@"
         UNION ALL
        SELECT ");
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        sb.Append((i > 0 ? @"
             , " : "") + @"a.[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"]");
                    }
                    sb.Append(@"
          FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] a
          JOIN Node ON ");
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        if (i > 0) sb.Append(@" AND ");
                        sb.Append(@"a.[" + Utils.GetEscapeSqlObjectName(fkc.Name) + @"] = Node.[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"]");
                    }
                    sb.Append(@"
    )
    SELECT ");
                    for (int i = 0; i < t.Columns.Count; i++)
                    {
                        Column c = t.Columns[i];
                        sb.Append((i > 0 ? @"
         , " : "") + @"a.[" + Utils.GetEscapeSqlObjectName(c.Name) + @"]");
                    }
                    sb.Append(@"
      FROM [" + Utils.GetEscapeSqlObjectName(t.Schema) + @"].[" + Utils.GetEscapeSqlObjectName(t.Name) + @"] a
      JOIN Node ON ");
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        if (i > 0) sb.Append(@" AND ");
                        sb.Append(@"a.[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"] = Node.[" + Utils.GetEscapeSqlObjectName(fkc.ReferencedColumn) + @"]");
                    }
                    sb.Append(@"
END


-- 下面这几行用于生成智能感知代码，以及强类型返回值，请注意同步修改（SP名称，备注，返回值类型）

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对 表 " + t.ToString() + @"
根据主键值返回一个节点的多行数据' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectNode'
EXEC sys.sp_addextendedproperty @name=N'CodeGenSettings_ResultType', @value=N'" + t.ToString() + @"' , @level0type=N'SCHEMA',@level0name=N'" + t.Schema + @"', @level1type=N'PROCEDURE',@level1name=N'usp_" + Utils.GetEscapeSqlObjectName(t.Name) + @"_SelectNode'

");
                    break;
                }
            }

            #endregion

            #region return

            gr = new GenResult(GenResultTypes.CodeSegment);
            gr.CodeSegment = new KeyValuePair<string, string>(this._properties[GenProperties.Tips].ToString(), sb.ToString());
            return gr;

            #endregion
        }

    }
}
