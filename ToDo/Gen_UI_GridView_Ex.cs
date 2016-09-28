using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.NeverCleanUp
{
	public static class Gen_UI_GridView_Ex
	{
		public static string Gen_Aspx(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
			return sb.ToString();
		}

		public static string Gen_Aspx(View t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
				string rdonly = @" ReadOnly=""True""";
				string sort = socs.Contains(c) ? (@" SortExpression=""" + cn + @"""") : "";

				sb.Append(@"
			<asp:BoundField DataField=""" + cn + @""" HeaderText=""" + caption + @"""" + rdonly + @"" + sort + @" />");
			}

			string search = "";
			foreach (Column c in sacs)
			{
				if (search.Length > 0) search += " OR ";
				search += @"" + c.Name + @" LIKE '%{0}%'";
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
</cc:DockPart>");
			return sb.ToString();
		}

		/// <summary>
		/// 基本完成。但结果集的二次查询未实现。表值函数的结果的主键定义有待研究
		/// </summary>
		public static string Gen_Aspx(UserDefinedFunction t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

			foreach (UserDefinedFunctionParameter p in t.Parameters)
			{
				string pn = Utils.GetEscapeName(p);
				string datatype = Utils.GetObjectDataSourceParameterDataType(p);
				sb.Append(@"<cc:DockPart ID=""_" + tbn + @"_GridView_DockPart"" runat=""server"" Height=""40"" IsClientClose=""False"" Style=""position: absolute; left: 0px; top: 0px; z-index: 101;"" Title=""" + t.Name + @""" Visible=""True"" Width=""50"" BackColor=""white"">
	<asp:Label runat=""server"" ID=""_" + pn + @"_Label"" Text=""" + p.Name.Substring(1) + @"""></asp:Label>
	<asp:TextBox ID=""_" + pn + @"_TextBox"" runat=""server""></asp:TextBox>");
			}
			sb.Append(@"
	<asp:LinkButton ID=""_refresh_LinkButton"" runat=""server"" Text=""Go"" />
	<asp:GridView ID=""_" + tbn + @"_GridView"" CssClass=""GridView"" runat=""server"" AllowPaging=""True"" AllowSorting=""True"" AutoGenerateColumns=""False"" DataKeyNames=""");
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
				string rdonly = @" ReadOnly=""True""";
				string sort = socs.Contains(c) ? (@" SortExpression=""" + cn + @"""") : "";

				sb.Append(@"
			<asp:BoundField DataField=""" + cn + @""" HeaderText=""" + caption + @"""" + rdonly + @"" + sort + @" />");
			}

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
	<asp:ObjectDataSource ID=""_" + tbn + @"_ObjectDataSource"" runat=""server"" OldValuesParameterFormatString=""original_{0}"" SelectMethod=""" + tbn + @""" TypeName=""DAL.DB"">");
			if (t.Parameters.Count > 0)
			{
				sb.Append(@"
		<SelectParameters>");
				foreach (UserDefinedFunctionParameter p in t.Parameters)
				{
					string pn = Utils.GetEscapeName(p);
					string datatype = Utils.GetObjectDataSourceParameterDataType(p);
					sb.Append(@"
			<asp:ControlParameter ControlID=""_" + pn + @"_TextBox"" DefaultValue=""" + Utils.GetObjectDataSourceParameterDefaultValue(p) + @""" Name=""" + pn + @""" Type=""" + datatype + @""" />");
				}
				sb.Append(@"
		</SelectParameters>");
			}
			sb.Append(@"
	</asp:ObjectDataSource>
</cc:DockPart>");
			return sb.ToString();
		}

		/// <summary>
		/// 没做完
		/// </summary>
		public static string Gen_Aspx(StoredProcedure sp)
		{
			//方法名
			string mn = Utils.GetMethodName(sp);
			//结果集类型（表名或视图名）
			string rt = Utils.GetResultType(sp);
			//返回值是否为单行
			bool isSingleLine = Utils.GetIsSingleLineResult(sp);
			//返回值的描述
			string rtn = (rt == Utils.EP_ResultType_Int
				|| rt == Utils.EP_ResultType_DataSet
				|| rt == Utils.EP_ResultType_DataTable
				|| rt == Utils.EP_ResultType_Object ? rt : ((isSingleLine ? "单行" : "多行") + rt));
			//所属行为
			string behavior = Utils.GetBehavior(sp);
			//返回值对应的视图名
			string vn = "";
			if (string.IsNullOrEmpty(rt))
			{
				rt = Utils.EP_ResultType_DataSet;
			}
			else if (rt.Contains("."))
			{
				vn = rt.Substring(rt.LastIndexOf('.') + 2).TrimEnd(']');
				rt = "DS." + vn + "DataTable";
			}
			string spn = Utils.GetEscapeName(sp);
			if (string.IsNullOrEmpty(mn)) mn = spn;


			View t = sp.Parent.Views[vn];
			if (t == null)
			{
				return "";
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				List<Column> pks = Utils.GetPrimaryKeyColumns(t);
				List<Column> socs = Utils.GetSortableColumns(t);
				List<Column> sacs = Utils.GetSearchableColumns(t);

				string tbn = Utils.GetEscapeName(sp);

				sb.Append(@"<cc:DockPart ID=""_" + tbn + @"_GridView_DockPart"" runat=""server"" Height=""40"" IsClientClose=""True"" Style=""position: absolute; left: 0px; top: 0px; z-index: 101;"" Title=""" + t.Name + @""" Visible=""True"" Width=""50"" BackColor=""white"">
	<cc:GVToolbar ID=""_" + tbn + @"_GVToolbar"" runat=""server"" GridViewID=""_" + tbn + @"_GridView"" />
	<asp:GridView ID=""_" + tbn + @"_GridView"" CssClass=""GridView"" runat=""server"" AllowPaging=""True"" AllowSorting=""True"" AutoGenerateColumns=""False"" DataKeyNames=""");
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
					string rdonly = @" ReadOnly=""True""";
					string sort = socs.Contains(c) ? (@" SortExpression=""" + cn + @"""") : "";

					sb.Append(@"
			<asp:BoundField DataField=""" + cn + @""" HeaderText=""" + caption + @"""" + rdonly + @"" + sort + @" />");
				}

				string search = "";
				foreach (Column c in sacs)
				{
					if (search.Length > 0) search += " OR ";
					search += @"" + c.Name + @" LIKE '%{0}%'";
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
	<asp:ObjectDataSource ID=""_" + tbn + @"_ObjectDataSource"" runat=""server"" OldValuesParameterFormatString=""original_{0}"" SelectMethod=""" + tbn + @""" TypeName=""DAL.DB""" + search + @">");
				if (sp.Parameters.Count > 0)
				{
					sb.Append(@"
		<SelectParameters>");
					foreach (UserDefinedFunctionParameter p in sp.Parameters)
					{
						string pn = Utils.GetEscapeName(p);
						string datatype = Utils.GetDataType(p);
						sb.Append(@"
			<asp:Parameter Name=""" + pn + @""" Type=""" + datatype + @""" />");
					}
					sb.Append(@"
		</SelectParameters>");
				}
				sb.Append(@"
		<FilterParameters>
			<asp:ControlParameter ControlID=""_" + tbn + @"_GVToolbar"" ConvertEmptyStringToNull=""False"" Name=""Keyword"" PropertyName=""Keyword"" Type=""string"" />
		</FilterParameters>
	</asp:ObjectDataSource>
</cc:DockPart>");
				return sb.ToString();
			}
		}

		public static string Gen_CS(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);
			string s = "";

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
			return sb.ToString();
		}

		public static string Gen_CS(View t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);
			string s = "";

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
			return sb.ToString();
		}
	}
}
