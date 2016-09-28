using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.ASPX
{
    class Gen_Table_Detail : IGenComponent
    {
        #region Init

        public Gen_Table_Detail()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_Detail");
            this._properties.Add(GenProperties.Caption, "GridDetail");
            this._properties.Add(GenProperties.Group, "ASP.NET");
            this._properties.Add(GenProperties.Tips, "为 Table 生成 asp.net 的数据行明细 UI 相关代码");
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

        public bool Validate(params object[] sqlElements)
        {
            Table t = (Table)sqlElements[0];

            return Utils.GetPrimaryKeyColumns(t).Count > 0;
        }

        #region Gen ASPX methods

        private string GenTD1(Column c)
        {
            string cn = Utils.GetEscapeName(c);
            string cc = Utils.GetCaption(c);

            return @"
            <asp:Label runat=""server""
                ID=""_" + cn + @"_Label""
                Text=""" + cc + @"""
            />";
        }
        private string GenTD2(Column c)
        {
            string cn = Utils.GetEscapeName(c);
            StringBuilder sb = new StringBuilder();
            if (c.Nullable)
            {
                if (Utils.CheckIsStringType(c))
                {
                    if (c.DataType.MaximumLength >= 100)
                    {
                        sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                TextMode=""MultiLine""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TA""
            />");
                    }
                    else
                    {
                        sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                MaxLength=""" + Utils.GetDbTypeLength(c) + @"""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                    }
                }
                else if (Utils.CheckIsNumericType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                MaxLength=""24""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsBooleanType(c))
                {
                    sb.Append(@"
            <asp:RadioButtonList runat=""server""
                ID=""_" + cn + @"_RadioButtonList""
                RepeatDirection=""Horizontal""
                CssClass=""LI_FIELD_CONTENT_R""
            >
                <asp:ListItem Text=""不知道"" Value="""" Selected=""True"" />
                <asp:ListItem Text=""是"" Value=""1"" />
                <asp:ListItem Text=""否"" Value=""0"" />
            </asp:RadioButtonList>");
                }
                else if (Utils.CheckIsDateTimeType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                MaxLength=""24""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsGuidType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                MaxLength=""36""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsBinaryType(c))
                {
                    // todo
                }
            }
            else
            {
                if (Utils.CheckIsStringType(c))
                {
                    if (c.DataType.MaximumLength >= 100)
                    {
                        sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                TextMode=""MultiLine""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TA""
            />");
                    }
                    else
                    {
                        sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                MaxLength=""" + Utils.GetDbTypeLength(c) + @"""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                    }
                }
                else if (Utils.CheckIsNumericType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                ToolTip=""必填""
                MaxLength=""24""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsBooleanType(c))
                {
                    sb.Append(@"
            <asp:CheckBox runat=""server""
                ID=""_" + cn + @"_CheckBox""
                Text=""是"" 
                CssClass=""LI_FIELD_CONTENT_CB""
            />");
                }
                else if (Utils.CheckIsDateTimeType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                ToolTip=""必填""
                MaxLength=""24""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsGuidType(c))
                {
                    sb.Append(@"
            <asp:TextBox ID=""_" + cn + @"_TextBox"" runat=""server""
                ToolTip=""必填""
                MaxLength=""36""
                Text=""""
                CssClass=""LI_FIELD_CONTENT_TB""
            />");
                }
                else if (Utils.CheckIsBinaryType(c))
                {
                    // todo
                }
            }
            return sb.ToString();
        }
        private string GenTD3(Column c)
        {
            string cn = Utils.GetEscapeName(c);
            string cd = Utils.GetDescription(c);
            return @"
            <asp:Label runat=""server""
                ID=""_" + cn + @"_Warning_Label""
                Text=""" + (c.Nullable ? "" : "*") + @"""
                ForeColor=""Red""
            />
            <asp:Label runat=""server""
                ID=""_" + cn + @"_Message_Label""
                Text=""" + cd + @"""
            />";
        }



        private string GenTD2_DDL(Column c, Table ft, Column fc)
        {
            string cn = Utils.GetEscapeName(c);
            return @"
            <asp:DropDownList ID=""_" + cn + @"_DropDownList"" runat=""server""
                CssClass=""LI_FIELD_CONTENT_DDL""
            />
";
        }




        #endregion

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

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen CSS

            sb.Append(@"

<style type=""text/css"">

    .UL_DETAIL, .LI_FIELD, .LI_BUTTON, .UL_FIELD, .UL_FIELD, .LI_FIELD_LABEL, .LI_FIELD_CONTENT, .LI_FIELD_MESSAGE
    {
        float:left;
        margin:0px;
        list-style:none;
        padding:0px;
    }
    .UL_DETAIL
    {
        font-size:12px;
        width:600px;
    }
    .LI_FIELD
    {
        width:600px;
    }
    .LI_BUTTON
    {
        width:600px;
    }
    .LI_FIELD_LABEL
    {
        width:150px;
    }
    .LI_FIELD_CONTENT
    {
        width:200px;
    }
    .LI_FIELD_MESSAGE
    {
        width:250px;
    }
    .LI_FIELD_CONTENT_TA, .LI_FIELD_CONTENT_TB, .LI_FIELD_CONTENT_CB, .LI_FIELD_CONTENT_DDL
    {
        width:180px;
    }
    .LI_FIELD_CONTENT_TA
    {
        height:60px;
    }
    .LI_FIELD_CONTENT_TB, .LI_FIELD_CONTENT_CB
    {
        height:18px;
    }
    .TABLE_BUTTON
    {
        width:600px;
    }
    .TD_MESSAGE
    {
        width:500px;
    }
    .TD_BUTTON
    {
        text-align:right;
        float:right;
    }

</style>
");

            string result_css = sb.ToString();
            sb.Remove(0, sb.Length);

            #endregion

            #region Gen ASPX

            string tn = Utils.GetEscapeName(t);
            string tc = Utils.GetCaption(t);

            sb.Append(@"
<!-- " + tn + " (" + tc + ")" + @" -->
<ul class=""UL_DETAIL"">");

            foreach (Column c in t.Columns)
            {
                // 如果 c 是外键，则注释
                bool isForeignKey = Utils.CheckIsForeignKey(c);

                sb.Append(isForeignKey ? @"<%--" : "");

                sb.Append(@"
    <li class=""LI_FIELD"">
        <ul class=""UL_FIELD"">
            <li class=""LI_FIELD_LABEL"">" + GenTD1(c) + @"            
            </li>
            <li class=""LI_FIELD_CONTENT"">" + GenTD2(c) + @"            
            </li>
            <li class=""LI_FIELD_MESSAGE"">" + GenTD3(c) + @"            
            </li>
        </ul>
    </li>
");
                sb.Append(isForeignKey ? @"--%>" : "");
            }

            // todo: 多字段外键支持
            foreach (ForeignKey fk in t.ForeignKeys)
            {
                Column c = t.Columns[fk.Columns[0].Name];
                Table ft = _db.Tables[fk.ReferencedTable, fk.ReferencedTableSchema];
                Column fc = ft.Columns[fk.Columns[0].ReferencedColumn];
                sb.Append(@"
    <li class=""LI_FIELD"">
        <ul class=""UL_FIELD"">
            <li class=""LI_FIELD_LABEL"">" + GenTD1(c) + @"            
            </li>
            <li class=""LI_FIELD_CONTENT"">" + GenTD2_DDL(c, ft, fc) + @"            
            </li>
            <li class=""LI_FIELD_MESSAGE"">" + GenTD3(c) + @"            
            </li>
        </ul>
    </li>
");
            }


            sb.Append(@"
    <li class=""LI_BUTTON"">
        <table class=""TABLE_BUTTON"" cellpadding=""0"" cellspacing=""0"" border=""0"">
            <tr>
                <td class=""TD_MESSAGE"">
                    <asp:PlaceHolder runat=""server"" ID=""_Message_PlaceHolder""></asp:PlaceHolder>
                </td>
                <td class=""TD_BUTTON"" valign=""top"">
                    <input type=""button"" runat=""server"" id=""_Submit_Button"" value=""提交"" />
                </td>
            </tr>
        </table>
    </li>
</ul>
");

            string result_aspx = sb.ToString();
            sb.Remove(0, sb.Length);

            #endregion

            #region Gen CS


            sb.Append(@"
protected override void OnInit(EventArgs e)
{
    base.OnInit(e);

    _Submit_Button.ServerClick += new EventHandler(_Submit_Button_ServerClick);
    
    if (!IsPostBack)
    {
        // todo
    }
}

protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // todo: init data here

        var o = new OO." + tn + @"();

        // todo: init data here

        SetData(o);
    }
}

void _Submit_Button_ServerClick(object sender, EventArgs e)
{
    var o = GetData();

    if (HasErrors) return;

    try
    {
        OB." + tn + @".Insert(o);
        EnableControls(false);
        _Submit_Button.Disabled = true;

        OP(""数据保存成功！"");
    }
    catch(Exception ex)
    {
        OP(""数据保存失败..."" + ex.Message);
    }
}

void SetData(OO." + tn + @" o)
{");
            foreach (Column c in t.Columns)
            {
                sb.Append(@"
");
                bool isForeignKey = Utils.CheckIsForeignKey(c);
                string cn = Utils.GetEscapeName(c);

                sb.Append(isForeignKey ? @"
/*
                     " : "");

                if (c.Nullable)
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = o." + cn + @" ?? """";");
                    }
                    else if (Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    if (o." + cn + @" == null) _" + cn + @"_TextBox.Text = """";
    else _" + cn + @"_TextBox.Text = o." + cn + @".ToString();");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    if (o." + cn + @" == null) _" + cn + @"_RadioButtonList.SelectedIndex = 0;
    else _" + cn + @"_RadioButtonList.SelectedIndex = o." + cn + @".value ? 1 : 2;");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                        // todo
                    }
                }
                else
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = o." + cn + @";");
                    }
                    else if (Utils.CheckIsNumericType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = o." + cn + @".ToString();");
                    }
                    else if (Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    if (o." + cn + @" == Guid.Empty) _" + cn + @"_TextBox.Text = """";
    else _" + cn + @"_TextBox.Text = o." + cn + @".ToString();");
                    }
                    else if (Utils.CheckIsDateTimeType(c))
                    {
                        sb.Append(@"
    if (o." + cn + @" == DateTime.MinValue) _" + cn + @"_TextBox.Text = """";
    else _" + cn + @"_TextBox.Text = o." + cn + @".ToString();");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_CheckBox.Checked = o." + cn + @";");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                        // todo
                    }
                }

                sb.Append(isForeignKey ? @"
*/
                     " : "");
            }

            foreach (ForeignKey fk in t.ForeignKeys)
            {
                Column c = t.Columns[fk.Columns[0].Name];
                Table ft = _db.Tables[fk.ReferencedTable, fk.ReferencedTableSchema];
                Column fc = ft.Columns[fk.Columns[0].ReferencedColumn];
                Column dc = Utils.GetDisplayColumn(ft);

                string cn = Utils.GetEscapeName(c);
                string fcn = Utils.GetEscapeName(fc);
                string ftn = Utils.GetEscapeName(ft);
                string dcn = Utils.GetEscapeName(dc);

                sb.Append(@"
    _" + cn + @"_DropDownList.DataSource = OB." + ftn + @".SelectAll();
    _" + cn + @"_DropDownList.DataTextField = DI." + ftn + @"." + dcn + @".ToString();
    _" + cn + @"_DropDownList.DataValueField = DI." + ftn + @"." + fcn + @".ToString();
    _" + cn + @"_DropDownList.DataBind();
");
                if (c.Nullable)
                {
                    sb.Append(@"
    _" + cn + @"_DropDownList.Items.Insert(0, """");
");
                }
                sb.Append(@"
    var " + cn + @"_Value = o." + cn + @" == null ? """" : o." + cn + @".ToString();
    foreach (ListItem item in _" + cn + @"_DropDownList.Items)
    {
        if (item.Value == " + cn + @"_Value)
        {
            item.Selected = true;
            break;
        }
    }
");
            }

            sb.Append(@"

    // todo: more control init here
}


OO." + tn + @" GetData()
{
    var o = new OO." + tn + @"();

");

            foreach (Column c in t.Columns)
            {
                bool isForeignKey = Utils.CheckIsForeignKey(c);
                string cn = Utils.GetEscapeName(c);

                sb.Append(isForeignKey ? @"
/*
                     " : "");

                if (c.Nullable)
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    if (_" + cn + @"_TextBox.Text == """") o." + cn + @" = null;
    else o." + cn + @" = _" + cn + @"_TextBox.Text;");
                    }
                    else if (Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    if (_" + cn + @"_TextBox.Text == """") o." + cn + @" = null;
    else
    {
        try
        {
            o." + cn + @" = " + Utils.GetDataType(c) + @".Parse(_" + cn + @"_TextBox.Text);
            _" + cn + @"_Warning_Label.Text = """";
        }
        catch(Exception ex)
        {
            OP(""必须正确输入" + Utils.GetCaption(c) + @"！"");
            _" + cn + @"_Warning_Label.Text = ""*"";
        }
    }");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    if (_" + cn + @"_RadioButtonList.SelectedValue == """") o." + cn + @" = null;
    else o." + cn + @" = _" + cn + @"_RadioButtonList.SelectedValue == ""1"";
    _" + cn + @"_Warning_Label.Text = """";");
                    }
                    //else if (Utils.CheckIsDateTimeType(c))
                    //{
                    //}
                    else if (Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    if (_" + cn + @"_TextBox.Text == """") o." + cn + @" = null;
    else
    {
        try
        {
            o." + cn + @" = new Guid(_" + cn + @"_TextBox.Text);
            _" + cn + @"_Warning_Label.Text = """";
        }
        catch(Exception ex)
        {
            OP(""必须正确输入" + Utils.GetCaption(c) + @"！"");
            _" + cn + @"_Warning_Label.Text = ""*"";
        }
    }");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                        // todo
                    }
                }
                else
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    // _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    // if (_" + cn + @"_TextBox.Text == """")
    // {
        // OP(""必须正确输入" + Utils.GetCaption(c) + @"！"");
        // _" + cn + @"_Warning_Label.Text = ""*"";
    // }
    // else
    // {
    o." + cn + @" = _" + cn + @"_TextBox.Text;
    _" + cn + @"_Warning_Label.Text = """";
    // }");
                    }
                    else if (Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    try
    {
        o." + cn + @" = " + Utils.GetDataType(c) + @".Parse(_" + cn + @"_TextBox.Text);
        _" + cn + @"_Warning_Label.Text = """";
    }
    catch(Exception ex)
    {
        OP(""必须正确输入" + Utils.GetCaption(c) + @"！"");
        _" + cn + @"_Warning_Label.Text = ""*"";
    }");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = _" + cn + @"_CheckBox.Checked;
    _" + cn + @"_Warning_Label.Text = """";");
                    }
                    //else if (Utils.CheckIsDateTimeType(c))
                    //{
                    //}
                    else if (Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    _" + cn + @"_TextBox.Text = _" + cn + @"_TextBox.Text.Trim();
    try
    {
        o." + cn + @" = new Guid(_" + cn + @"_TextBox.Text);
        _" + cn + @"_Warning_Label.Text = """";
    }
    catch(Exception ex)
    {
        OP(""必须正确输入" + Utils.GetCaption(c) + @"！"");
        _" + cn + @"_Warning_Label.Text = ""*"";
    }");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                        // todo
                    }
                }

                sb.Append(isForeignKey ? @"
*/
                     " : "");

            }

            foreach (ForeignKey fk in t.ForeignKeys)
            {
                Column c = t.Columns[fk.Columns[0].Name];
                Table ft = _db.Tables[fk.ReferencedTable, fk.ReferencedTableSchema];
                Column fc = ft.Columns[fk.Columns[0].ReferencedColumn];
                Column dc = Utils.GetDisplayColumn(ft);

                string cn = Utils.GetEscapeName(c);
                string fcn = Utils.GetEscapeName(fc);
                string ftn = Utils.GetEscapeName(ft);
                string dcn = Utils.GetEscapeName(dc);

                string typename = Utils.GetDataType(c);
                if (c.Nullable)
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = _" + cn + @"_DropDownList.SelectedValue == """" ? null : _" + cn + @"_DropDownList.SelectedValue;
");
                    }
                    else if (Utils.CheckIsNumericType(c) || Utils.CheckIsBooleanType(c) || Utils.CheckIsDateTimeType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = _" + cn + @"_DropDownList.SelectedValue == """" ? null : new " + typename + @"?(" + typename + @".Parse(_" + cn + @"_DropDownList.SelectedValue));
");
                    }
                    else if (Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = _" + cn + @"_DropDownList.SelectedValue == """" ? null : new Guid?(new Guid(_" + cn + @"_DropDownList.SelectedValue));
");
                    }
                    else
                    {
                        throw new Exception("no handle data type");
                    }

                }
                else
                {
                    if (Utils.CheckIsStringType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = _" + cn + @"_DropDownList.SelectedValue;
");
                    }
                    else if (Utils.CheckIsNumericType(c) || Utils.CheckIsBooleanType(c) || Utils.CheckIsDateTimeType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = new " + typename + @"?(" + typename + @".Parse(_" + cn + @"_DropDownList.SelectedValue));
");
                    }
                    else if (Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    o." + cn + @" = new Guid?(new Guid(_" + cn + @"_DropDownList.SelectedValue));
");
                    }
                    else
                    {
                        throw new Exception("no handle data type");
                    }
                }

            }


            sb.Append(@"

    if (!HasErrors)
    {
        // todo: more check here
    }

    return o;
}

int _msgCounter = 0;
void OP(string msg)
{
    _msgCounter++;
    _Message_PlaceHolder.Controls.Add(new Label() { Text = msg, ForeColor = System.Drawing.Color.Red });
    if (_msgCounter % 4 == 0) _Message_PlaceHolder.Controls.Add(new Literal() { Text = ""<br />"" });
}

bool HasErrors
{
    get { return _msgCounter > 0; }
}

void EnableControls(bool b)
{");
            foreach (Column c in t.Columns)
            {
                bool isForeignKey = Utils.CheckIsForeignKey(c);
                sb.Append(isForeignKey ? @"
/*
                     " : "");

                if (c.Nullable)
                {
                    if (Utils.CheckIsStringType(c) || Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    _" + Utils.GetEscapeName(c) + @"_TextBox.Enabled = b;");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    _" + Utils.GetEscapeName(c) + @"_RadioButtonList.Enabled = b;");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                        // todo
                    }
                }
                else
                {
                    if (Utils.CheckIsStringType(c) || Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
                    {
                        sb.Append(@"
    _" + Utils.GetEscapeName(c) + @"_TextBox.Enabled = b;");
                    }
                    else if (Utils.CheckIsBooleanType(c))
                    {
                        sb.Append(@"
    _" + Utils.GetEscapeName(c) + @"_CheckBox.Enabled = b;");
                    }
                    else if (Utils.CheckIsBinaryType(c))
                    {
                    }
                }
                sb.Append(isForeignKey ? @"
*/
                     " : "");

            }

            foreach (ForeignKey fk in t.ForeignKeys)
            {
                Column c = t.Columns[fk.Columns[0].Name];
                string cn = Utils.GetEscapeName(c);
                sb.Append(@"
    _" + cn + @"_DropDownList.Enabled = b;");
            }

            sb.Append(@"
}");

            #endregion

            #region return

            gr = new GenResult(GenResultTypes.CodeSegments);
            gr.CodeSegments = new List<KeyValuePair<string, string>>();
            gr.CodeSegments.Add(new KeyValuePair<string, string>("CSS Style", result_css));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("ASPX Code", result_aspx));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("C# Code", sb.ToString()));
            return gr;

            #endregion
        }



    }
}
