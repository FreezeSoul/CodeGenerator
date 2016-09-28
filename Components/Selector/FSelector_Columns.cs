using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.Selector
{
    public partial class FSelector_Columns : Form
    {
        /*
        示例:
         
        List<Column> cs = new List<Column>();
        using (FSelector_Columns f = new FSelector_Columns(t, ref cs))
        {
            f.ShowDialog();
        }

        MessageBox.Show(cs.Count.ToString());
         
         */






        protected Table _t = null;
        protected List<Column> _cs = null;

        public FSelector_Columns(Table t, ref List<Column> cs)
        {
            InitializeComponent();

            _t = t;
            _cs = cs;
        }

        private void FSelector_Columns_Load(object sender, EventArgs e)
        {
            if (_t != null)
            {
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
                    int i = _DataGridView.Rows.Add(
                        (c.InPrimaryKey ? Properties.Resources.SQL_Key : (c.IsForeignKey ? Properties.Resources.SQL_ForeignKey : Properties.Resources.SQL_Empty)),
                        c.Name, 
                        Utils.GetCaption(c), 
                        Utils.GetDescription(c)
                    );
                    _DataGridView.Rows[i].Tag = c;
                }

            }
            
        }

        private void _Submit_button_Click(object sender, EventArgs e)
        {
            if (_DataGridView.SelectedRows.Count == 0)
            {
                return;
            }
            else
            {
                _cs.Clear();
                foreach (DataGridViewRow dgvr in _DataGridView.SelectedRows)
                {
                    _cs.Add((Column)dgvr.Tag);
                }

                this.Close();
            }
        }

        private void _Cancel_button_Click(object sender, EventArgs e)
        {
            _cs.Clear();
        }


    }
}
