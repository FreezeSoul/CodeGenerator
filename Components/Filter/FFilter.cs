using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using System.IO;
using CodeGenerator;
using CodeGenerator.Misc;

namespace CodeGenerator
{
    public partial class FFilter : Form
    {
        int _currentSchemeID = 1;

        public FFilter(Database db)
        {
            InitializeComponent();
            _db = db;
            Utils.FillDatabaseDALGenSettingDS(db, _ds);
            Utils._CurrrentDALGenSetting_CurrentScheme = _ds.Schemes.FindBySchemesID(1);
        }

        public FFilter(Database db, int schemeID)
            : this(db)
        {
            _currentSchemeID = schemeID;
            Utils._CurrrentDALGenSetting_CurrentScheme = _ds.Schemes.FindBySchemesID(schemeID);
        }

        private Database _db;

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveUp_Click(object sender, EventArgs e)
        {
            if (filtersDataGridView.SelectedRows.Count == 0) return;
            DataGridViewRow r = filtersDataGridView.SelectedRows[0];
            int i = r.Index;
            if (i == 0) return;
            DataGridViewRow r_upon = filtersDataGridView.Rows[i - 1];

            Exchange(r.DataBoundItem as DataRowView, r_upon.DataBoundItem as DataRowView);
            filtersDataGridView.Rows[i - 1].Selected = true;
        }

        private void bindingNavigatorMoveDown_Click(object sender, EventArgs e)
        {
            if (filtersDataGridView.SelectedRows.Count == 0) return;
            DataGridViewRow r = filtersDataGridView.SelectedRows[0];
            int i = r.Index;
            if (i >= filtersDataGridView.Rows.Count - 2) return;				// - 2 是因为要算上新增行
            DataGridViewRow r_under = filtersDataGridView.Rows[i + 1];

            Exchange(r.DataBoundItem as DataRowView, r_under.DataBoundItem as DataRowView);
            filtersDataGridView.Rows[i + 1].Selected = true;
        }

        private void Exchange(DataRowView r1, DataRowView r2)
        {
            int i = (int)r1["SortOrder"];
            r1["SortOrder"] = r2["SortOrder"];
            r2["SortOrder"] = i;

            _ds.AcceptChanges();
            typesBindingSource.ResetBindings(false);
            filtersDataGridView.Sort(filtersDataGridView.Columns["dataGridViewTextBoxColumn1"], ListSortDirection.Ascending);
        }

        private void filtersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            filtersDataGridView.CurrentCell = null;

            _ds.AcceptChanges();

            Utils.SaveDatabaseDALGenSettingDS(_db, _ds);
        }

        private void bindingNavigatorTest_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Tables:" + Environment.NewLine);
            foreach (Table t in Utils.GetUserTables(_db, _ds)) sb.Append(t.ToString() + Environment.NewLine);
            MessageBox.Show(sb.ToString());
            sb.Remove(0, sb.Length);
            sb.Append(@"Views:" + Environment.NewLine);
            foreach (Microsoft.SqlServer.Management.Smo.View t in Utils.GetUserViews(_db, _ds)) sb.Append(t.ToString() + Environment.NewLine);
            MessageBox.Show(sb.ToString());
            sb.Remove(0, sb.Length);
            sb.Append(@"StoredProcedures:" + Environment.NewLine);
            foreach (StoredProcedure t in Utils.GetUserStoredProcedures(_db, _ds)) sb.Append(t.ToString() + Environment.NewLine);
            MessageBox.Show(sb.ToString());
            sb.Remove(0, sb.Length);
            sb.Append(@"Functions:" + Environment.NewLine);
            foreach (UserDefinedFunction t in Utils.GetUserFunctions(_db, _ds)) sb.Append(t.ToString() + Environment.NewLine);
            MessageBox.Show(sb.ToString());
        }

        private void FFilter_Load(object sender, EventArgs e)
        {
            _ds.SchemesFilters.SchemesIDColumn.DefaultValue = _currentSchemeID;
            filtersBindingSource.Filter = "SchemesID = " + _currentSchemeID.ToString();
            filtersDataGridView.Sort(filtersDataGridView.Columns["dataGridViewTextBoxColumn1"], ListSortDirection.Ascending);
        }

        private void FFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils._CurrrentDALGenSetting_CurrentScheme = _ds.Schemes.FindBySchemesID(1);
        }
    }
}
