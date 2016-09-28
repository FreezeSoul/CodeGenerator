using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.ASPX
{
    class Gen_Table_JQGrid : IGenComponent
    {
        // todo: 查询，部分字段返回，多选，多主键支持

        #region Init

        public Gen_Table_JQGrid()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_JQGrid");
            this._properties.Add(GenProperties.Caption, "JQGrid");
            this._properties.Add(GenProperties.Group, "ASP.NET");
            this._properties.Add(GenProperties.Tips, "为 Table 生成 JQGrid 的 JS 代码及 ASHX 后台");
            this._properties.Add(GenProperties.IsEnabled, false);
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

        public string JsEscape(string s)
        {
            return s.Replace(@"'", @"\'").Replace(@"""", @"\""");
        }
        public string CsEscape(string s)
        {
            return s.Replace(@"""", @"""""");
        }

        public bool Validate(params object[] sqlElements)
        {
            Table t = (Table)sqlElements[0];

            return Utils.GetPrimaryKeyColumns(t).Count > 0;
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
                gr.Message = "无法为没有主键字段的表生成该UI代码！";
                return gr;
            }

            List<Column> wcs = Utils.GetWriteableColumns(t);
            List<Column> socs = Utils.GetSortableColumns(t);
            List<Column> sacs = Utils.GetSearchableColumns(t);

            List<Column> ocs = new List<Column>();                      // 实际输出的字段集
            foreach (Column c in t.Columns) if (Utils.CheckIsBinaryType(c)) continue; else ocs.Add(c);      // 过滤掉不输出的字段

            // find pk's index
            int pkcidx = 0;
            for (int i = 0; i < ocs.Count; i++)
            {
                if (ocs[i].InPrimaryKey) { pkcidx = i; break; }
            }
            Column pkc = ocs[pkcidx];
            string pkcn = Utils.GetEscapeName(pkc);


            #endregion

            #region Gen

            string tn = Utils.GetEscapeName(t);


            StringBuilder sb_js = new StringBuilder();
            sb_js.Append(@"
<script type=""text/javascript"">
    $().ready(function() {
        

        $(""#table"").jqGrid({
            pager       : $(""#pager""),

            url         : ""query.ashx"",
            caption     : ""JSON Mapping"",
            sortname    : """ + JsEscape(pkc.Name) + @""",
            sortorder   : ""desc"",
            rowNum      : 10,
            imgpath     : ""css/images"",


            colModel    : [");


            for (int i = 0; i < ocs.Count; i++)
            {
                Column c = ocs[i];

                string caption = Utils.GetCaption(c);
                string cn = JsEscape(c.Name);
                string width = "80";                                       // todo: 根据各种数据类型及其长度来推断出显示宽度
                string align = Utils.CheckIsNumericType(c) ? "center" : "left"; // todo: 视情况判断显示位置 right
                string sortable = socs.Contains(c).ToString().ToLower();

                // todo: 格式化日期，货币显示

                sb_js.Append(@"
	            { label: """ + caption + @""", name: """ + cn + @""", index: """ + cn + @""", width: " + width + @", align: """ + align + @""", sortable: " + sortable + @" }");

                if (i < ocs.Count - 1) sb_js.Append(@",");
            }

            sb_js.Append(@"
            ],
            viewrecords : true,
            datatype    : ""json"",
            jsonReader  : {
	            repeatitems : false,
	            id          : """ + pkcidx.ToString() + @"""
            },
            rowList     : [10, 20, 30],
            height      : ""100%"",
            autowidth   : true

        }).navGrid(""#pager"", {edit: false, add: false, del: false});
    });
</script>








<table id=""table"" class=""scroll"" cellpadding=""0"" cellspacing=""0""></table>
<div id=""pager"" class=""scroll"" style=""text-align:center;""></div>
");
            StringBuilder sb_cs = new StringBuilder();
            sb_cs.Append(@"
var response = context.Response;
var request = context.Request;
response.ContentType = ""text/plain"";


// JQGrid 的固有字段

var pageIndex = int.Parse(request[""page""] ?? ""1"");
var pageSize = int.Parse(request[""rows""] ?? ""10"");
var sortColumn = request[""sidx""] ?? @""" + CsEscape(pkc.Name) + @""";
var sortDirection = request[""sord""];


// 当前表相关字段的过滤查询

var exps = new List<OE." + tn + @">();
var s = """";

");

            foreach (Column c in sacs)
            {
                string cn = Utils.GetEscapeName(c);
                sb_cs.Append(@"
s = request[""" + CsEscape(c.Name) + @"""] ?? """";
if (s != """") exps.Add(OE." + tn + @"." + cn + @".Like(s));
");
            }

            sb_cs.Append(@"

// 拼接表达式

OE." + tn + @" exp = null;
if (exps.Count > 0)
{
    exp = exps[0];
    for (int i = 1; i < exps.Count; i++) exp.And(exps[i]);
}

// 取符合条件的记录数

var rowCount = OB." + tn + @".GetCount_Custom(exp);

// 算页码啥的

var pageCount = 0;
if (rowCount > 0) pageCount = (int)Math.Ceiling((double)rowCount / (double)pageSize);
if (pageIndex > pageCount) pageIndex = pageCount;
var rowIndex = pageSize * pageIndex - pageSize;// +1;

// 取符合条件的，当前需要显示的页的数据

var rows = OB." + tn + @".SelectAllPage_Custom(
    exp,
    (DI." + tn + @")Enum.Parse(typeof(DI." + tn + @"),
    sortColumn),
    sortDirection == ""asc"",
    rowIndex,
    pageSize
);

// 输出 JQGrid 需要的 JSON

response.Write(rows.ToJson(pageIndex, pageCount, rowCount, DI." + tn + @"." + pkcn + @".ToString(), jqGridHelper.DataType.Enhancement));
");



            #region JQGridHelper

            StringBuilder sb_helper = new StringBuilder();
            sb_helper.Append(@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;

/// <summary>
/// 辅助生成 jqgrid 所需的 json 输出格式字串
/// todo: 指定输出字段列表部分输出
/// </summary>
public static class jqGridHelper
{
    /*
            
生成物样式: (不需要空格和换行. 这里只为好看)
         
Basic：(固定主键位置，不支持字段调整显示先后顺序)
{
""page"":""1"",""total"":4,""records"":""22"",""rows"":[
    [""1"",null,""Marc Forster"",""2008"",""Daniel Craig"",""200""],
    [""2"",""Casino Royale"",""Martin Campbell"",""2006"",""Daniel Craig"",""150""],
    [""3"",""Die Another Day"",""Lee Tamahori"",""2002"",""Pierce Brosnan"",""142""],
]
}
         
Standard: (不支持字段调整显示先后顺序)
{
""page"":""1"",""total"":4,""records"":""22"",""rows"":[
    {""id"":1,""cell"":[""1"",null,""Marc Forster"",""2008"",""Daniel Craig"",""200""]},
    {""id"":2,""cell"":[""2"",""Casino Royale"",""Martin Campbell"",""2006"",""Daniel Craig"",""150""]},
    {""id"":3,""cell"":[""3"",""Die Another Day"",""Lee Tamahori"",""2002"",""Pierce Brosnan"",""142""]},
]
}

Enhancement: (固定主键位置，支持字段调整显示先后顺序)
{
""page"":""1"",""total"":2,""records"":""13"",""rows"":[
    {""id"":""1"",""invdate"":null,""name"":""Client 1"",""amount"":""100.00"",""tax"":""20.00"",""total"":""120.00"",""note"":""note 1""},
    {""id"":""2"",""invdate"":""2007-10-03"",""name"":""Client 1"",""amount"":""200.00"",""tax"":""40.00"",""total"":""240.00"",""note"":""note 2""},
    {""id"":""10"",""invdate"":""2007-10-06"",""name"":""Client 2"",""amount"":""100.00"",""tax"":""20.00"",""total"":""120.00"",""note"":null}
]
}

上述生成物中: page 从 1 开始, 表示当前页码; total 表示 一共有多少页; records 表示一共有多少条数据; rows 为数据集
精简版中: 数据格式固定为 id + cell, id 为主键值. cell 为字段值列表.
完整版中: 数据格式为 column name 与 value 形成的键值对. (尚不确定 主键字段  是否必须排第一个)

*/
    /// <summary>
    /// json for jqgrid 的数据输出类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 基础版数据输出，对应的 jsonReader: { repeatitems : true, cell:"""", id: ""0"" }     id 应该是用来说明 主键 位于第几个字段上
        /// </summary>
        Basic,
        /// <summary>
        /// 标准版数据输出，对应的 jsonReader:　无
        /// </summary>
        Standard,
        /// <summary>
        /// 增强版数据输出，支持 jsonMap （调整字段显示先后顺序）  对应的 jsonReader: { repeatitems : false, id: ""0"" }    id 应该是用来说明 主键 位于第几个字段上
        /// </summary>
        Enhancement
    }

    private static string JsonEscape(object o)
    {
        if (o == null) return string.Empty;
        return o.ToString().Replace(""<"", ""&lt;"").Replace("">"", ""&gt;"").Replace(""\"""", ""&quot;"").Replace(""'"", ""’"").Replace(""/"", ""&#47;"").Replace(""\\"", ""&#92;"");
    }


    private static object GetPropertiesAndFieldsValue(object o, MemberInfo mi)
    {
        if (mi.MemberType == MemberTypes.Property)
        {
            return (mi as PropertyInfo).GetValue(o, null);
        }
        else if (mi.MemberType == MemberTypes.Field)
        {
            return (mi as FieldInfo).GetValue(o);
        }
        return null;
    }

    private static List<MemberInfo> GetPropertiesAndFields(Type t)
    {
        var mis = t.GetMembers(BindingFlags.Public | BindingFlags.Instance);
        return (from mi in mis
                where mi.MemberType == MemberTypes.Property || mi.MemberType == MemberTypes.Field
                select mi).ToList();
    }

    /// <summary>
    /// 将 DataTable 转为 jqgrid 所需 json 字串
    /// </summary>
    public static string ToJson(this DataTable dt, int pageIndex, int pageCount, int rowCount, DataType datatype)
    {
        var pk = dt.Columns[0];
        if (dt.PrimaryKey != null && dt.PrimaryKey.Length > 0) pk = dt.PrimaryKey[0];       // todo: 多主键支持

        var sb = new StringBuilder();
        sb.Append(@""{""""page"""":"""""" + pageIndex + @"""""",""""total"""":"" + pageCount + @"",""""records"""":"""""" + rowCount + @"""""",""""rows"""":["");
        var b1 = false;

        foreach (DataRow row in dt.Rows)
        {
            if (b1) sb.Append("","");

            if (datatype == DataType.Basic)
            {
                sb.Append(@""["");
                var b2 = false;
                foreach (DataColumn col in dt.Columns)
                {
                    if (b2) sb.Append(@"","");
                    var value = row.IsNull(col) ? @""""""null"""""" : (@"""""""" + JsonEscape(row[col]) + @"""""""");
                    sb.Append(value);
                    b2 = true;
                }
                sb.Append(@""]"");
            }
            else if (datatype == DataType.Standard)
            {
                var value = row.IsNull(pk) ? @""""""null"""""" : (@"""""""" + JsonEscape(row[pk]) + @"""""""");
                sb.Append(@""{"""""" + JsonEscape(pk.ColumnName) + @"""""":"" + value + @"",""""cell"""":["");
                var b2 = false;
                foreach (DataColumn col in dt.Columns)
                {
                    if (b2) sb.Append(@"","");
                    value = row.IsNull(col) ? @""""""null"""""" : (@"""""""" + JsonEscape(row[col]) + @"""""""");
                    sb.Append(value);
                    b2 = true;
                }
                sb.Append(@""]}"");
            }
            else if (datatype == DataType.Enhancement)
            {
                sb.Append(@""{"");
                var b2 = false;
                foreach (DataColumn col in dt.Columns)
                {
                    if (b2) sb.Append(@"","");
                    var value = row.IsNull(col) ? @""""""null"""""" : (@"""""""" + JsonEscape(row[col]) + @"""""""");
                    sb.Append(@"""""""" + JsonEscape(col.ColumnName) + @"""""":"" + value);
                    b2 = true;
                }
                sb.Append(@""}"");
            }

            b1 = true;
        }
        sb.Append(@""]}"");
        return sb.ToString();
    }

    /// <summary>
    /// 将 数据集合 转为 jqgrid 所需 json 字串
    /// </summary>
    public static string ToJson(this IEnumerable list, int pageIndex, int pageCount, int rowCount, string pkcol, DataType datatype)
    {
        var sb = new StringBuilder();
        sb.Append(@""{""""page"""":"""""" + pageIndex + @"""""",""""total"""":"" + pageCount + @"",""""records"""":"""""" + rowCount + @"""""",""""rows"""":["");

        List<MemberInfo> mis = null;
        MemberInfo pk = null;
        foreach (var o in list)
        {
            if (mis == null)
            {
                mis = GetPropertiesAndFields(o.GetType());
                if (string.IsNullOrEmpty(pkcol)) pk = mis[0];
                pk = mis.Find(m => { return m.Name == pkcol; });
            }
            else
                sb.Append(@"","");

            if (datatype == DataType.Basic)
            {
                sb.Append(@""["");
                var b2 = false;
                foreach (var mi in mis)
                {
                    if (b2) sb.Append(@"","");
                    var val = GetPropertiesAndFieldsValue(o, mi);
                    var value = val == null ? ""null"" : (@"""""""" + JsonEscape(val) + @"""""""");
                    sb.Append(value);
                    b2 = true;
                }
                sb.Append(@""]"");
            }
            else if (datatype == DataType.Standard)
            {
                var val = GetPropertiesAndFieldsValue(o, pk);
                var value = val == null ? ""null"" : (@"""""""" + JsonEscape(val) + @"""""""");
                sb.Append(@""{"""""" + JsonEscape(pk.Name) + @"""""":"" + value + @"",""""cell"""":["");
                var b2 = false;
                foreach (var mi in mis)
                {
                    if (b2) sb.Append(@"","");
                    val = GetPropertiesAndFieldsValue(o, mi);
                    value = val == null ? ""null"" : (@"""""""" + JsonEscape(val) + @"""""""");
                    sb.Append(value);
                    b2 = true;
                }
                sb.Append(@""]}"");
            }
            else if (datatype == DataType.Enhancement)
            {
                sb.Append(@""{"");
                var b2 = false;
                foreach (var mi in mis)
                {
                    if (b2) sb.Append(@"","");
                    var val = GetPropertiesAndFieldsValue(o, mi);
                    var value = val == null ? ""null"" : (@"""""""" + JsonEscape(val) + @"""""""");
                    sb.Append(@"""""""" + JsonEscape(mi.Name) + @"""""":"" + value);
                    b2 = true;
                }
                sb.Append(@""}"");
            }
        }
        sb.Append(@""]}"");
        return sb.ToString();
    }

}
");
            #endregion



            #endregion

            #region return

            gr = new GenResult(GenResultTypes.CodeSegments);
            gr.CodeSegments = new List<KeyValuePair<string, string>>();
            gr.CodeSegments.Add(new KeyValuePair<string, string>("JS & HTML", sb_js.ToString()));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("ASHX C# Code", sb_cs.ToString()));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("JQGrid Helper C# Code", sb_helper.ToString()));
            return gr;

            #endregion
        }
    }
}
