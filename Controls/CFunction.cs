using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using CodeGenerator;
using CodeGenerator.Misc;

namespace CodeGenerator
{
	public partial class CFunction : UserControl
	{
		private UserDefinedFunction _f;
		private Database _db;

		public string MethodName
		{
			get
			{
				return Utils.GetMethodName(_f);
			}
			set
			{
				Utils.SetMethodName(_f, value);
			}
		}

		public string Description
		{
			get
			{
				return Utils.GetDescription(_f);
			}
			set
			{
				Utils.SetDescription(_f, value);
			}
		}

		public string BelongTo
		{
			get
			{
				return Utils.GetBelongTo(_f);
			}
			set
			{
				Utils.SetDescription(_f, value);
			}
		}

		public CFunction(UserDefinedFunction sp)
		{
			_f = sp;
			_db = sp.Parent;

			InitializeComponent();
		}

		private void CFunction_Load(object sender, EventArgs e)
		{
			_SPName_TextBox.Text = _f.ToString();
			_SPHead_richTextBox.Text = _f.TextHeader;
			_SPBody_richTextBox.Text = _f.TextBody;

			_Desc_richTextBox.Text = this.Description;
			if (string.IsNullOrEmpty(this.MethodName))
				_MethodName_textBox.Text = Utils.GetEscapeName(_f);
			else _MethodName_textBox.Text = this.MethodName;
			_MethodName_textBox.Text = this.MethodName;

			foreach (UserDefinedFunctionParameter p in _f.Parameters)
			{
				dS.Parms.AddParmsRow(p.Name, false, Utils.GetDescription(p), p.DataType.Name, p.DataType.MaximumLength, p.DataType.NumericPrecision, p.DataType.NumericScale, p.DefaultValue);
			}

			dS.Parms.ParmsRowChanged += new DS.ParmsRowChangeEventHandler(Parms_ParmsRowChanged);


			if (_f.FunctionType == UserDefinedFunctionType.Table || _f.FunctionType == UserDefinedFunctionType.Inline)
			{
				//todo: 多字段组合主键 的表达
				//todo: 多字段组合唯一索引 的表达

				foreach (Column c in _f.Columns)
				{
					int i = _Result_dataGridView.Rows.Add((c.InPrimaryKey ? Properties.Resources.SQL_Key : (c.IsForeignKey ? Properties.Resources.SQL_ForeignKey : Properties.Resources.SQL_Empty)), c.Name, Utils.GetCaption(c), Utils.GetDescription(c), c.DataType.Name, c.DataType.MaximumLength, c.Nullable, c.InPrimaryKey, c.Computed);
					_Result_dataGridView.Rows[i].Tag = c;
				}

			}
		}

		void Parms_ParmsRowChanged(object sender, DS.ParmsRowChangeEvent e)
		{
			Utils.SetDescription(_f.Parameters[e.Row.Name], e.Row.Desc);
		}

		private void _Memo_textBox_TextChanged(object sender, EventArgs e)
		{
			this.Description = _Desc_richTextBox.Text;
		}

		private void _MethodName_textBox_TextChanged(object sender, EventArgs e)
		{
			//todo: 判断如果该过程属于某表，则方法名不应和表自带方法相冲
			//如果方法名和对象同名，存空值

			if (_MethodName_textBox.Text == Utils.GetEscapeName(_f)) this.MethodName = null;
			else this.MethodName = _MethodName_textBox.Text;
		}

		private void _Result_dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.RowIndex >= _Result_dataGridView.Rows.Count) return;
			if (_Result_dataGridView.Columns[e.ColumnIndex].Name == "_Memo")
			{
				Column c = (Column)_Result_dataGridView.Rows[e.RowIndex].Tag;
				string memo = _Result_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
				Utils.SetDescription(c, memo);
			}
			else if (_Result_dataGridView.Columns[e.ColumnIndex].Name == "_Caption")
			{
				Column c = (Column)_Result_dataGridView.Rows[e.RowIndex].Tag;
				string caption = _Result_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
				if (caption == null) caption = "";
				Utils.SetCaption(c, caption);
			}
		}

		private void _Extract_button_Click(object sender, EventArgs e)
		{
			using (TextReader tr = new StringReader(_f.TextHeader))
			{
				while (true)
				{
					string s = tr.ReadLine();
					if (s == null) break;
					if (s.Contains("--"))
					{
						int idx = s.IndexOf("--", 0);
						s = s.Substring(idx + 2, s.Length - idx - 2).Trim();
						if (s.Length > 0) _Desc_richTextBox.Text += s + Environment.NewLine;
					}
				}
			}
		}
	}
}
