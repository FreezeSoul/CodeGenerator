using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.ASPX
{
    class Gen_Table_DetailPanel : IGenComponent
    {
        #region Init

        public Gen_Table_DetailPanel()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_DetailPanel");
            this._properties.Add(GenProperties.Caption, "DetailPanel");
            this._properties.Add(GenProperties.Group, "ASP.NET");
            this._properties.Add(GenProperties.Tips, "为 Table 生成 asp.net 的 DetailPanel ASPX + CS代码");
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

            StringBuilder sb = new StringBuilder();

            #endregion

            #region Gen

            string tbn = Utils.GetEscapeName(t);


            string s = "";


            sb.Append(@"<cc:DockPart ID=""_" + tbn + @"_GridView_DockPart"" runat=""server"" Height=""40"" IsClientClose=""False"" Style=""position: absolute; left: 0px; top: 0px; z-index: 101;"" Title=""" + t.Name + @""" Visible=""True"" Width=""50"" BackColor=""white"">
	<cc:GVToolbar ID=""_" + tbn + @"_GVToolbar"" runat=""server"" GridViewID=""_" + tbn + @"_GridView"" OnOnDeleteButtonClicked=""_" + tbn + @"_GVToolbar_OnDeleteButtonClicked"" OnOnEditButtonClicked=""_" + tbn + @"_GVToolbar_OnEditButtonClicked"" OnOnInsertButtonClicked=""_" + tbn + @"_GVToolbar_OnInsertButtonClicked"" OnOnRefreshButtonClicked=""_" + tbn + @"_GVToolbar_OnRefreshButtonClicked"" />
	<asp:GridView ID=""_" + tbn + @"_GridView"" CssClass=""GridView"" runat=""server"" AllowPaging=""True"" AllowSorting=""True"" AutoGenerateColumns=""False"" OnSelectedIndexChanged=""_" + tbn + @"_GridView_SelectedIndexChanged"" DataKeyNames=""");
            for (int i = 0; i < pks.Count; i++)
            {
                if (i > 0) sb.Append(@", ");
                sb.Append(pks[i].Name);
            }
            sb.Append(@""" DataSourceID=""_" + tbn + @"_ObjectDataSource"">
		<Columns>
			<asp:CommandField ShowSelectButton=""True"" />");
            foreach (Column c in t.Columns)
            {
                string cn = c.Name;
                string caption = Utils.GetCaption(c);
                if (string.IsNullOrEmpty(caption) || caption.Trim().Length == 0) caption = c.Name;
                string rdonly = wcs.Contains(c) ? "" : @" ReadOnly=""True""";
                string sort = socs.Contains(c) ? (@" SortExpression=""" + cn + @"""") : "";

                sb.Append(@"
			<asp:BoundField DataField=""" + cn + @""" HeaderText=""" + caption + @"""" + rdonly + @"" + sort + @" />");
            }

            string search = "";
            foreach (Column c in sacs)
            {
                string cn = Utils.GetEscapeRowFilterName(c.Name);
                if (search.Length > 0) search += " OR ";
                search += @"" + cn + @" LIKE '%{0}%'";
            }
            if (search.Length > 0) search = @" FilterExpression=""" + search + @"""";

            sb.Append(@"
		</Columns>
		<FooterStyle CssClass=""GridView_Footer"" />
		<EmptyDataRowStyle CssClass=""GridView_Empty"" />
		<RowStyle CssClass=""GridView_Row"" />
		<EditRowStyle CssClass=""GridView_EditRow"" />
		<SelectedRowStyle CssClass=""GridView_SelectedRow"" />
		<PagerStyle CssClass=""GridView_Pager"" />
		<HeaderStyle CssClass=""GridView_Header"" />
		<AlternatingRowStyle CssClass=""GridView_AlternatingRow"" />
	</asp:GridView>
	<asp:ObjectDataSource ID=""_" + tbn + @"_ObjectDataSource"" runat=""server"" OldValuesParameterFormatString=""original_{0}"" SelectMethod=""SelectAll"" TypeName=""DAL.DB+" + tbn + @"""" + search + @">
		<FilterParameters>
			<asp:ControlParameter ControlID=""_" + tbn + @"_GVToolbar"" ConvertEmptyStringToNull=""False"" Name=""Keyword"" PropertyName=""Keyword"" Type=""string"" />
		</FilterParameters>
	</asp:ObjectDataSource>
</cc:DockPart>





");











            sb.Append(@"
<cc:DockPart ID=""_" + tbn + @"_DetailPanel_DockPart"" runat=""server"" Height=""40"" IsClientClose=""False"" Style=""position: absolute; left: 0px; top: 0px; z-index: 101;"" Title=""" + t.Name + @" Row's Detail"" Visible=""False"" Width=""50"" BackColor=""white"">
    <cc:DetailPanel ID=""_" + tbn + @"_DetailPanel"" runat=""server"" CssClass=""DetailPanel"" OnOnCancel=""_" + tbn + @"_DetailPanel_OnCancel"" OnOnDelete=""_" + tbn + @"_DetailPanel_OnDelete"" OnOnInsert=""_" + tbn + @"_DetailPanel_OnInsert"" OnOnUpdate=""_" + tbn + @"_DetailPanel_OnUpdate"">");
            foreach (Column c in t.Columns)
            {
                string cn = Utils.GetEscapeName(c);
                if (c.DataType.SqlDataType == SqlDataType.Bit)
                    sb.Append(@"
        <cc:DetailCheckBox ID=""_" + tbn + "_" + cn + @"_DetailTextBox"" Caption=""" + Utils.GetCaption(c) + @":"" FieldName=""" + c.Name + @""" runat=""server"" />");
                else if (Utils.CheckIsDateTimeType(c))
                    sb.Append(@"
        <cc:DetailDateTimeBox ID=""_" + tbn + "_" + cn + @"_DateTimeBox"" Caption=""" + Utils.GetCaption(c) + @":"" FieldName=""" + c.Name + @""" runat=""server"" />");
                else
                    sb.Append(@"
        <cc:DetailTextBox ID=""_" + tbn + "_" + cn + @"_TextBox"" Caption=""" + Utils.GetCaption(c) + @":"" FieldName=""" + c.Name + @""" runat=""server"" />");
            }
            sb.Append(@"
        <cc:DetailHR ID=""_" + tbn + @"_DetailHR"" runat=""server"" />
        <cc:DetailButtons ID=""_" + tbn + @"_DetailButtons"" runat=""server"" />
		<cc:DetailMessageBox ID=""_" + tbn + @"_DetailMessageBox"" runat=""server"" />
    </cc:DetailPanel>
</cc:DockPart>



");








            //先得到从 GridView 取 Row 的语句
            if (pks.Count > 0)
            {
                s = @"DAL.DS." + tbn + @"Row r = DAL.DB." + tbn + @".Select(";
                for (int i = 0; i < pks.Count; i++)
                {
                    Column c = pks[i];
                    string cn = Utils.GetEscapeName(c);
                    string typename = Utils.GetDataType(c);
                    if (i > 0) s += @", ";
                    s += @"(" + typename + @")_" + tbn + @"_GridView.SelectedDataKey[""" + c.Name + @"""]";
                }
                s += @");";
            }

            sb.Append(@"

#region _" + tbn + @"_GridView

protected DAL.DS." + tbn + @"Row GetSelectedRow_" + tbn + @"_GridView()
{
	" + s + @"
	return r;
}


protected void _" + tbn + @"_GridView_SelectedIndexChanged(object sender, EventArgs e)
{
");
            if (pks.Count > 0)
            {
                sb.Append(@"
	DAL.DS." + tbn + @"Row r = GetSelectedRow_" + tbn + @"_GridView();
	//todo
	_" + tbn + @"_DetailPanel.Init(r, Mender.Web.Controls.DetailStates.View);
	_" + tbn + @"_DetailPanel_DockPart.Show(""view"");
");
            }
            sb.Append(@"
}

protected void _" + tbn + @"_GVToolbar_OnRefreshButtonClicked(object sender, EventArgs e)
{
	_" + tbn + @"_GridView.DataBind();
}

protected void _" + tbn + @"_GVToolbar_OnInsertButtonClicked(object sender, EventArgs e)
{
");
            if (pks.Count > 0)
            {
                sb.Append(@"
	DAL.DS." + tbn + @"Row r = DAL.DS." + tbn + @"Row.CreateInstance(");
                for (int i = 0; i < wcs.Count; i++)
                {
                    Column c = wcs[i];
                    string cn = Utils.GetEscapeName(c);
                    string defaultvalue = Utils.GetDefaultValue(c);
                    if (i > 0) sb.Append(@", ");
                    sb.Append(defaultvalue);
                }
                sb.Append(@");
	//todo
	_" + tbn + @"_DetailPanel.Init(r, Mender.Web.Controls.DetailStates.Insert);
	_" + tbn + @"_DetailPanel_DockPart.Show(""insert"");
");
            }
            sb.Append(@"
}

protected void _" + tbn + @"_GVToolbar_OnDeleteButtonClicked(object sender, EventArgs e)
{
");
            if (pks.Count > 0)
            {
                sb.Append(@"
	DAL.DS." + tbn + @"Row r = GetSelectedRow_" + tbn + @"_GridView();
	//todo
	_" + tbn + @"_DetailPanel.Init(r, Mender.Web.Controls.DetailStates.Delete);
	_" + tbn + @"_DetailPanel_DockPart.Show(""delete"");
");
            }
            sb.Append(@"
}

protected void _" + tbn + @"_GVToolbar_OnEditButtonClicked(object sender, EventArgs e)
{
");
            if (pks.Count > 0)
            {
                sb.Append(@"
	DAL.DS." + tbn + @"Row r = GetSelectedRow_" + tbn + @"_GridView();
	//todo
	_" + tbn + @"_DetailPanel.Init(r, Mender.Web.Controls.DetailStates.Edit);
	_" + tbn + @"_DetailPanel_DockPart.Show(""edit"");
");
            }
            sb.Append(@"
}

#endregion








");












            sb.Append(@"

#region _" + tbn + @"_DetailPanel

protected void _" + tbn + @"_DetailPanel_OnCancel(Mender.Web.Controls.DetailPanel dp)
{
	_" + tbn + @"_DetailPanel_DockPart.Hide();
}

protected void _" + tbn + @"_DetailPanel_OnInsert(Mender.Web.Controls.DetailPanel dp)
{
	DAL.DS." + tbn + @"Row r = (DAL.DS." + tbn + @"Row)dp.DataSource;
	//todo
	DAL.DB." + tbn + @".Insert(r);
	_" + tbn + @"_GridView.DataBind();
}

protected void _" + tbn + @"_DetailPanel_OnDelete(Mender.Web.Controls.DetailPanel dp)
{
	DAL.DS." + tbn + @"Row r = (DAL.DS." + tbn + @"Row)dp.DataSource;
	//todo
	DAL.DB." + tbn + @".Delete(r);
	_" + tbn + @"_GridView.DataBind();
}

protected void _" + tbn + @"_DetailPanel_OnUpdate(Mender.Web.Controls.DetailPanel dp)
{
	DAL.DS." + tbn + @"Row r = (DAL.DS." + tbn + @"Row)dp.DataSource;
	//todo
	DAL.DB." + tbn + @".Update(r);
	_" + tbn + @"_GridView.DataBind();
}

#endregion

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
