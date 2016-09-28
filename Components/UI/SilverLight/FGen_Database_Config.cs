using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
	public partial class FGen_Database_Config : Form
	{
		public FGen_Database_Config(Database db)
		{
			InitializeComponent();
			_db = db;
		}

		Database _db;

		private void FGen_Database_DAL_Config_Load(object sender, EventArgs e)
		{
			Utils.LoadDatabaseDALGenSettingDS(_db);		// 载入默认方案 1
			this._namespace_textBox.Text = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;
			this._isSupportSchema_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema;
			this._isSupportWCF_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF;
		}

		private void _submit_button_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void _namespace_textBox_TextChanged(object sender, EventArgs e)
		{
			Utils._CurrrentDALGenSetting_CurrentScheme.Namespace = this._namespace_textBox.Text;
		}

		private void _isSupportSchema_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema = this._isSupportSchema_checkBox.Checked;
		}

		private void _isSupportWCF_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF = this._isSupportWCF_checkBox.Checked;
		}
	}
}
