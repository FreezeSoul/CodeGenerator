using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.ASPX
{
    class Gen_Table_GridView : IGenComponent
    {
        #region Init

        public Gen_Table_GridView()
        {
            this._properties.Add(GenProperties.Name, "Gen_Table_GridView");
            this._properties.Add(GenProperties.Caption, "GridView");
            this._properties.Add(GenProperties.Group, "ASP.NET");
			this._properties.Add(GenProperties.Tips, "为 Table 生成 asp.net 的数据行列表 UI 相关代码");
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

            string tbn = t.Name;

            sb.Append(@"
	<asp:GridView ID=""_" + tbn + @"_GridView"" CssClass=""GridView"" runat=""server"" AllowPaging=""True"" AllowSorting=""True"" AutoGenerateColumns=""False"" DataKeyNames=""");
            for (int i = 0; i < pks.Count; i++)
            {
                if (i > 0) sb.Append(@", ");
                sb.Append(pks[i].Name);
            }
            sb.Append(@""">
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
