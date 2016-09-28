using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.NeverCleanUp
{
	public static class Gen_UI_DetailPanel
	{
		#region Table

		public static string Gen(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
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
        <hr />
        <cc:DetailButtons ID=""_" + tbn + @"_DetailButtons"" runat=""server"" />
		<cc:DetailMessageBox ID=""_" + tbn + @"_DetailMessageBox"" runat=""server"" />
    </cc:DetailPanel>
</cc:DockPart>
");
			return sb.ToString();
		}

		public static string GenCS(Table t)
		{
			StringBuilder sb = new StringBuilder();
			List<Column> pks = Utils.GetPrimaryKeyColumns(t);
			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

			sb.Append(@"
");
			return sb.ToString();
		}


		#endregion

		#region View

		public static string Gen(View t)
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
        <hr />
        <cc:DetailButtons ID=""_" + tbn + @"_DetailButtons"" runat=""server"" />
		<cc:DetailMessageBox ID=""_" + tbn + @"_DetailMessageBox"" runat=""server"" />
    </cc:DetailPanel>
</cc:DockPart>
");
			return sb.ToString();
		}
		#endregion

		#region Function

		public static string Gen(UserDefinedFunction t)
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
        <hr />
        <cc:DetailButtons ID=""_" + tbn + @"_DetailButtons"" runat=""server"" />
		<cc:DetailMessageBox ID=""_" + tbn + @"_DetailMessageBox"" runat=""server"" />
    </cc:DetailPanel>
</cc:DockPart>
");
			return sb.ToString();
		}

		#endregion
	}
}
