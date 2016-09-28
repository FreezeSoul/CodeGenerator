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
    public partial class CView : UserControl
    {
        public CView(Microsoft.SqlServer.Management.Smo.View v)
        {
            InitializeComponent();

            _v = v;
        }

        private Microsoft.SqlServer.Management.Smo.View _v;

        private void CView_Load(object sender, EventArgs e)
        {
            _Caption_TextBox.Text = Utils.GetCaption(_v);
            _ViewName_TextBox.Text = _v.Name;
            _Scheme_TextBox.Text = _v.Schema;
            _CreateTime_TextBox.Text = _v.CreateDate.ToString();
            _Desc_TextBox.Text = Utils.GetDescription(_v);

            _BaseTable_comboBox.BeginUpdate();

            _BaseTable_comboBox.Items.Clear();
            _BaseTable_comboBox.Items.Add("None");
            List<Table> uts = Utils.GetUserTables(_v.Parent);
            foreach (Table t in uts)
            {
                if (Utils.CheckCanbeBaseTable(_v, t)) _BaseTable_comboBox.Items.Add(t);
            }


            string baseTableName = Utils.GetBaseTableName(_v);
            string baseTableSchema = Utils.GetBaseTableSchema(_v);

            int i = 0;
            for (i = 0; i < _BaseTable_comboBox.Items.Count; i++)
            {
                Object o = _BaseTable_comboBox.Items[i];
                if (o.GetType() == typeof(Table))
                {
                    Table t = (Table)o;
                    if (t.Name == baseTableName && t.Schema == baseTableSchema)
                    {
                        break;
                    }
                }
            }
            _BaseTable_comboBox.SelectedIndex = i < _BaseTable_comboBox.Items.Count ? i : 0;

            //指定了基表的情况下则不允许再对主键进行指定
            _DataGridView.Columns[1].ReadOnly = (i > 0);

            _BaseTable_comboBox.EndUpdate();

            _BaseTable_comboBox.SelectedIndexChanged += new EventHandler(_BaseTable_comboBox_SelectedIndexChanged);

            DrawGrid();
        }

        private void DrawGrid()
        {
            List<string> pkcns = Utils.GetPrimaryKeyColumnNames(_v);
            _DataGridView.Rows.Clear();
            foreach (Column c in _v.Columns)
            {
                int i = _DataGridView.Rows.Add(c.Name, pkcns.Contains(c.Name), Utils.GetCaption(c), Utils.GetDescription(c), c.DataType.Name, c.DataType.MaximumLength, c.Nullable);
                _DataGridView.Rows[i].Tag = c;
            }

        }

        private void _Desc_TextBox_TextChanged(object sender, EventArgs e)
        {
            Utils.SetDescription(_v, _Desc_TextBox.Text);
        }

        private void _DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _DataGridView.Rows.Count) return;
            if (_DataGridView.Columns[e.ColumnIndex].Name == "_Memo")
            {
                Column c = (Column)_DataGridView.Rows[e.RowIndex].Tag;
                string memo = _DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
                Utils.SetDescription(c, memo);
            }
            else if (_DataGridView.Columns[e.ColumnIndex].Name == "_IsPrimaryKey")
            {
                Column c = (Column)_DataGridView.Rows[e.RowIndex].Tag;
                bool b = (bool)_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                string colName = _DataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                List<string> pkcns = Utils.GetPrimaryKeyColumnNames(_v);
                if (b == false && pkcns.Contains(colName)) pkcns.Remove(colName);
                else if (!pkcns.Contains(colName)) pkcns.Add(colName);

                Utils.SetPrimaryKeyColumnNames(_v, pkcns);
            }
            else if (_DataGridView.Columns[e.ColumnIndex].Name == "_Caption")
            {
                Column c = (Column)_DataGridView.Rows[e.RowIndex].Tag;
                string caption = _DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
                if (caption == null) caption = "";
                Utils.SetCaption(c, caption);
            }
        }

        private void _BaseTable_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object o = _BaseTable_comboBox.SelectedItem;
            if (o.GetType() == typeof(Table))
            {
                Table t = (Table)o;
                Utils.SetBaseTable(_v, t.Name, t.Schema);
                _DataGridView.Columns[1].ReadOnly = true;

                if (t != null)
                {
                    List<Column> pks = Utils.GetPrimaryKeyColumns(t);
                    Utils.SetPrimaryKeyColumns(_v, pks);

                    // 用基表的显示，备注信息填充视图的
                    foreach (Column c in _v.Columns)
                    {
                        Column tc = t.Columns[c.Name];
                        string t_caption = Utils.GetCaption(tc);
                        string t_memo = Utils.GetDescription(tc);

                        string v_caption = Utils.GetCaption(c);
                        string v_memo = Utils.GetDescription(c);

                        if (string.IsNullOrEmpty(v_caption) || v_caption == c.Name)
                        {
                            if (!string.IsNullOrEmpty(t_caption) && t_caption != tc.Name)
                            {
                                Utils.SetCaption(c, t_caption);
                            }
                        }

                        if (string.IsNullOrEmpty(v_memo))
                        {
                            if (!string.IsNullOrEmpty(t_memo))
                            {
                                Utils.SetDescription(c, t_memo);
                            }
                        }
                    }

                    DrawGrid();
                }
            }
            else
            {
                Utils.SetBaseTable(_v, null, null);
                _DataGridView.Columns[1].ReadOnly = false;
            }
        }

        private void _Caption_TextBox_TextChanged(object sender, EventArgs e)
        {
            Utils.SetCaption(_v, _Caption_TextBox.Text);
        }
    }
}
