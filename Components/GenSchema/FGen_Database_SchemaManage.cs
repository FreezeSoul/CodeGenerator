using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.GenSchema
{
	public partial class FGen_Database_SchemaManage : Form
	{
		Database _db;
		public FGen_Database_SchemaManage(Database db)
		{
			_db = db;
			InitializeComponent();
		}

		private void FGen_Database_ExtendSchema_Load(object sender, EventArgs e)
		{
			Utils.LoadDatabaseDALGenSettingDS(_db);

			_Schemes_listBox.DisplayMember = "Name";
			_Schemes_listBox.ValueMember = "SchemesID";
			_Schemes_listBox.DataSource = Utils._CurrrentDALGenSetting.Schemes;
		}

		private void _Filter_button_Click(object sender, EventArgs e)
		{
			using (FFilter f = new FFilter(_db, _currentScheme.SchemesID))
			{
				f.StartPosition = FormStartPosition.CenterScreen;
				f.ShowDialog();
			}
		}

		private void _GenOption_button_Click(object sender, EventArgs e)
		{
			using (Components.DAL.FGen_Database_DAL_Config f = new Components.DAL.FGen_Database_DAL_Config(_db, _currentScheme.SchemesID))
			{
				f.StartPosition = FormStartPosition.CenterScreen;
				f.ShowDialog();
			}

		}

		Misc.DS.SchemesRow _currentScheme = null;
		private void _Schema_listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_IsUpdating) return;

			int idx = 0;
			if (_Schemes_listBox.SelectedIndex > 0) idx = _Schemes_listBox.SelectedIndex;
			_currentScheme = Utils._CurrrentDALGenSetting.Schemes[idx];
			_Name_textBox.Text = _currentScheme.Name;
			_Memo_textBox.Text = _currentScheme.Memo;
		}

		private void _Insert_button_Click(object sender, EventArgs e)
		{
			// 找最大 SchemesID
			int max = 0;
			foreach (Misc.DS.SchemesRow row in Utils._CurrrentDALGenSetting.Schemes) if (row.SchemesID > max) max = row.SchemesID;
			max++;
			Misc.DS.SchemesRow r = Utils._CurrrentDALGenSetting.Schemes.FindBySchemesID(1);
			Misc.DS.SchemesRow newrow = Utils._CurrrentDALGenSetting.Schemes.AddSchemesRow(r.Namespace + "_" + max.ToString(),
				"", r.IsSupportSchema, r.IsSupportWCF, r.IsSupportDS, r.IsSupportOO, r.IsSupportOB_Table, r.IsSupportDB_View,
				r.IsSupportDB_Function, r.IsSupportDB_SP, r.IsSupportOB_Table, r.IsSupportOB_View, r.IsSupportOB_Function,
				r.IsSupportOB_SP, r.IsSupportOB_Extend, max, "方案" + max.ToString());

			// 复制过滤规则
			foreach (Misc.DS.SchemesFiltersRow row in r.GetSchemesFiltersRows())
			{
				Utils._CurrrentDALGenSetting.SchemesFilters.AddSchemesFiltersRow(row.TypeNamesRow, row.IsAllow, row.FilterString, newrow, row.Memo, row.Schema);
			}

			Utils.SaveDatabaseDALGenSettingDS(_db);
		}

		private void _Delete_button_Click(object sender, EventArgs e)
		{
			if (_currentScheme.SchemesID == 1) return;

			Utils._CurrrentDALGenSetting.Schemes.RemoveSchemesRow(_currentScheme);

			_Schema_listBox_SelectedIndexChanged(null, null);

			Utils.SaveDatabaseDALGenSettingDS(_db);
		}

		private bool _IsUpdating = false;

		private void _Save_button_Click(object sender, EventArgs e)
		{
			_IsUpdating = true;
			_currentScheme.Name = _Name_textBox.Text.Trim();
			_currentScheme.Memo = _Memo_textBox.Text.Trim();
			Utils.SaveDatabaseDALGenSettingDS(_db);
			_IsUpdating = false;
		}

		private void _Gen_button_Click(object sender, EventArgs e)
		{
			CodeGenerator.Components.DAL.Gen_Database_DAL gen = new CodeGenerator.Components.DAL.Gen_Database_DAL(_currentScheme.SchemesID);
			gen.Database = _db;
			Program.MainForm.Output(gen.Gen());
		}

	}
}
