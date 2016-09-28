using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.NeverCleanUp
{
	public static class Gen_UI_DetailPanel_Ex
	{
		#region Table

		public static string Gen_Aspx(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
			return sb.ToString();
		}


		#endregion

		#region View

		public static string Gen_Aspx(View t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
			return sb.ToString();
		}



		#endregion

		#region Function

		public static string Gen_Aspx(UserDefinedFunction t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

			sb.Append(@"
<cc:DockPart ID=""_" + tbn + @"_DetailPanel_DockPart"" runat=""server"" Height=""40"" IsClientClose=""False"" Style=""position: absolute; left: 0px; top: 0px; z-index: 101;"" Title=""" + t.Name + @" Row's Detail"" Visible=""False"" Width=""50"" BackColor=""white"">
    <cc:DetailPanel ID=""_" + tbn + @"_DetailPanel"" runat=""server"" CssClass=""DetailPanel"">");
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
			return sb.ToString();
		}

		#endregion




		public static string Gen_Cs(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
			return sb.ToString();
		}



		public static string Gen_Cs(View t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

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
			return sb.ToString();
		}
	}
}
