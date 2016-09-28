using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.Collections;

namespace CodeGenerator.Components.DAL
{
	public partial class FGen_Database_DAL_Config : Form
	{
		public FGen_Database_DAL_Config(Database db)
		{
			InitializeComponent();

			_db = db;
		}
		public FGen_Database_DAL_Config(Database db, int schemeID):this(db)
		{
			_currentSchemeID = schemeID;
		}

		int _currentSchemeID = 1;
		Database _db;

		private void FGen_Database_DAL_Config_Load(object sender, EventArgs e)
		{
			// 载入方案

			Utils.LoadDatabaseDALGenSettingDS(_db, _currentSchemeID);

			// 初始化显示

			this._namespace_textBox.Text = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;
			this._isSupportSchema_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema;

			this._IsSupportDS_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDS;
			this._IsSupportDB_Table_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table;
			this._IsSupportDB_View_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View;
			this._IsSupportDB_Function_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function;
			this._IsSupportDB_SP_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP;

			this._IsSupportOO_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOO;
			this._isSupportWCF_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF;
			this._IsSupportOB_Table_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table;
			this._IsSupportOB_View_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View;
			this._IsSupportOB_Function_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function;
			this._IsSupportOB_SP_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP;
			this._IsSupportOB_Extend_checkBox.Checked = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Extend;

		}

		private void _submit_button_Click(object sender, EventArgs e)
		{
			// 从控件读取设置参数

			Utils._CurrrentDALGenSetting_CurrentScheme.Namespace = this._namespace_textBox.Text;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema = this._isSupportSchema_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF = this._isSupportWCF_checkBox.Checked;

			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDS = this._IsSupportDS_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Table = this._IsSupportDB_Table_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_View = this._IsSupportDB_View_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_Function = this._IsSupportDB_Function_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportDB_SP = this._IsSupportDB_SP_checkBox.Checked;

			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOO = this._IsSupportOO_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Table = this._IsSupportOB_Table_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_View = this._IsSupportOB_View_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Function = this._IsSupportOB_Function_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_SP = this._IsSupportOB_SP_checkBox.Checked;
			Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportOB_Extend = this._IsSupportOB_Extend_checkBox.Checked;

			// 保存默认方案 1

			Utils.SaveDatabaseDALGenSettingDS(_db);

			this.DialogResult = DialogResult.OK;
		}

		private void _IsSupportDS_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			_DBGen_groupBox.Enabled = (sender as CheckBox).Checked;
			if (!_DBGen_groupBox.Enabled)
			{
				_IsSupportDB_Table_checkBox.Checked = false;
				_IsSupportDB_Function_checkBox.Checked = false;
				_IsSupportDB_View_checkBox.Checked = false;
				_IsSupportDB_SP_checkBox.Checked = false;
			}
		}

		private void _IsSupportOO_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			_isSupportWCF_checkBox.Enabled = _OBGen_groupBox.Enabled = (sender as CheckBox).Checked;
			if (!_isSupportWCF_checkBox.Enabled) _isSupportWCF_checkBox.Checked = false;
			if (!_OBGen_groupBox.Enabled)
			{
				_IsSupportOB_Extend_checkBox.Checked = false;
				_IsSupportOB_Table_checkBox.Checked = false;
				_IsSupportOB_Function_checkBox.Checked = false;
				_IsSupportOB_View_checkBox.Checked = false;
				_IsSupportOB_SP_checkBox.Checked = false;
			}
		}

		private void _IsSupportOB_Table_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			_IsSupportOB_Extend_checkBox.Enabled = _IsSupportOB_Table_checkBox.Checked || _IsSupportOB_View_checkBox.Checked;
			if (!_IsSupportOB_Extend_checkBox.Enabled) _IsSupportOB_Extend_checkBox.Checked = false;
		}

		private void _IsSupportOB_View_checkBox_CheckedChanged(object sender, EventArgs e)
		{
			_IsSupportOB_Extend_checkBox.Enabled = _IsSupportOB_Table_checkBox.Checked || _IsSupportOB_View_checkBox.Checked;
			if (!_IsSupportOB_Extend_checkBox.Enabled) _IsSupportOB_Extend_checkBox.Checked = false;
		}
	}
}
