using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator
{
	public partial class CUserTableType : UserControl
	{
		public CUserTableType(UserDefinedTableType t)
		{
			InitializeComponent();
			_t = t;
		}

		private UserDefinedTableType _t;


		private void CTable_Load(object sender, EventArgs e)
		{
			_Caption_TextBox.Text = Utils.GetCaption(_t);
			_TableName_TextBox.Text = _t.Name;
			_Scheme_TextBox.Text = _t.Schema;
			_CreateTime_TextBox.Text = _t.CreateDate.ToString();
			_Desc_TextBox.Text = Utils.GetDescription(_t);


			//todo: 多字段组合主键 的表达
			//todo: 多字段组合唯一索引 的表达

			List<string> ucns = new List<string>();
			foreach (Index idx in _t.Indexes)
			{
				//idx.IsUnique
				foreach (IndexedColumn idxc in idx.IndexedColumns)
				{
					ucns.Add(idxc.Name);
				}
			}

			foreach (Column c in _t.Columns)
			{
				int i = _DataGridView.Rows.Add((c.InPrimaryKey ? Properties.Resources.SQL_Key : (c.IsForeignKey ? Properties.Resources.SQL_ForeignKey : Properties.Resources.SQL_Empty)), c.Name, Utils.GetCaption(c), Utils.GetDescription(c), c.DataType.Name, c.DataType.MaximumLength, c.Nullable, c.InPrimaryKey || ucns.Contains(c.Name), c.Computed);
				_DataGridView.Rows[i].Tag = c;
			}
		}

		private void _DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.RowIndex >= _DataGridView.Rows.Count) return;
			if (_DataGridView.Columns[e.ColumnIndex].Name == "_Caption")
			{
				Column c = (Column)_DataGridView.Rows[e.RowIndex].Tag;
				string caption = _DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
				if (caption == null) caption = "";
				Utils.SetCaption(c, caption);
			}
			else if (_DataGridView.Columns[e.ColumnIndex].Name == "_Memo")
			{
				Column c = (Column)_DataGridView.Rows[e.RowIndex].Tag;
				string memo = _DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
				if (memo == null) memo = "";
				Utils.SetDescription(c, memo);
			}
		}

		private void _Desc_TextBox_TextChanged(object sender, EventArgs e)
		{
			Utils.SetDescription(_t, _Desc_TextBox.Text);
		}

		private void _Caption_TextBox_TextChanged(object sender, EventArgs e)
		{
			Utils.SetCaption(_t, _Caption_TextBox.Text);
		}


	}
}
