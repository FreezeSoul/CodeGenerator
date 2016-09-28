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
    public partial class FSelector_Table : Form
    {
        public Table SelectedTable = null;
        private Database _db = null;
        public FSelector_Table(Database db)
        {
            InitializeComponent();
            _db = db;
        }

        private void FSelector_Table_Load(object sender, EventArgs e)
        {
            foreach (Table t in _db.Tables)
            {
                _DataGridView.Rows.Add(t.Schema, t.Name, Utils.GetCaption(t), Utils.GetDescription(t));
            }
        }

        private void _Submit_button_Click(object sender, EventArgs e)
        {

        }

        private void _Cancel_button_Click(object sender, EventArgs e)
        {

        }
    }
}
