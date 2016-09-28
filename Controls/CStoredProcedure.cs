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
    public partial class CStoredProcedure : UserControl
    {
        private StoredProcedure _sp;
        private Database _db;

        public bool IsSingleLineResult
        {
            get
            {
                return Utils.GetIsSingleLineResult(_sp);
            }
            set
            {
                Utils.SetIsSingleLineResult(_sp, value);
            }
        }

        public string MethodName
        {
            get
            {
                return Utils.GetMethodName(_sp);
            }
            set
            {
                Utils.SetMethodName(_sp, value);
            }
        }

        public string Behavior
        {
            get
            {
                return Utils.GetBehavior(_sp);
            }
            set
            {
                Utils.SetBehavior(_sp, value);
            }
        }


        public string Description
        {
            get
            {
                return Utils.GetDescription(_sp);
            }
            set
            {
                Utils.SetDescription(_sp, value);
            }
        }


        public string ResultType
        {
            get
            {
                return Utils.GetResultType(_sp);
            }
            set
            {
                Utils.SetResultType(_sp, value);
            }
        }

        public string ResultTypeSchema
        {
            get
            {
                return Utils.GetResultTypeSchema(_sp);
            }
            set
            {
                Utils.SetResultTypeSchema(_sp, value);
            }
        }


        public CStoredProcedure(StoredProcedure sp)
        {
            _sp = sp;
            _db = sp.Parent;

            InitializeComponent();
        }

        private void CStoredProcedure_Load(object sender, EventArgs e)
        {
            _Behavior_comboBox.BeginUpdate();
            _Behavior_comboBox.Items.Add("None");
            _Behavior_comboBox.Items.Add("Select");
            _Behavior_comboBox.Items.Add("Insert");
            _Behavior_comboBox.Items.Add("Update");
            _Behavior_comboBox.Items.Add("Delete");


            _SPName_TextBox.Text = _sp.ToString();
            _SPHead_richTextBox.Text = _sp.TextHeader;
            _SPBody_richTextBox.Text = _sp.TextBody;

            _Desc_richTextBox.Text = this.Description;
            if (string.IsNullOrEmpty(this.MethodName))
                _MethodName_textBox.Text = Utils.GetEscapeName(_sp);
            else _MethodName_textBox.Text = this.MethodName;
            _MethodName_textBox.Text = this.MethodName;
            _SingleLine_checkBox.Checked = this.IsSingleLineResult;

            _ResultType_comboBox.BeginUpdate();

            _ResultType_comboBox.Items.Add(Utils.EP_ResultType_Int);
            _ResultType_comboBox.Items.Add(Utils.EP_ResultType_Object);
            _ResultType_comboBox.Items.Add(Utils.EP_ResultType_DataSet);
            _ResultType_comboBox.Items.Add(Utils.EP_ResultType_DataTable);


            List<Table> uts = Utils.GetUserTables(_db);
            List<Microsoft.SqlServer.Management.Smo.View> uvs = Utils.GetUserViews(_db);
            List<UserDefinedTableType> udtts = Utils.GetUserDefinedTableTypes(_db);

            if (string.IsNullOrEmpty(this.ResultType))
                _ResultType_comboBox.SelectedIndex = 0;
            else if (string.IsNullOrEmpty(this.ResultTypeSchema))
                _ResultType_comboBox.SelectedIndex = _ResultType_comboBox.FindString(this.ResultType);

            int idx = 4;
            foreach (Table o in uts)
            {
                _ResultType_comboBox.Items.Add(o);
                if (o.Name == this.ResultType && o.Schema == this.ResultTypeSchema) _ResultType_comboBox.SelectedIndex = idx;
                idx++;
            }
            foreach (Microsoft.SqlServer.Management.Smo.View o in uvs)
            {
                _ResultType_comboBox.Items.Add(o);
                if (o.Name == this.ResultType && o.Schema == this.ResultTypeSchema) _ResultType_comboBox.SelectedIndex = idx;
                idx++;
            }
            foreach (UserDefinedTableType o in udtts)
            {
                _ResultType_comboBox.Items.Add(o);
                if (o.Name == this.ResultType && o.Schema == this.ResultTypeSchema) _ResultType_comboBox.SelectedIndex = idx;
                idx++;
            }


            if (string.IsNullOrEmpty(this.Behavior))
            {
                _Behavior_comboBox.SelectedIndex = 0;
            }
            else _Behavior_comboBox.SelectedIndex = _Behavior_comboBox.FindString(this.Behavior);

            _ResultType_comboBox.EndUpdate();
            _Behavior_comboBox.EndUpdate();


            _ResultType_comboBox.SelectedIndexChanged += new EventHandler(_ResultType_comboBox_SelectedIndexChanged);
            _Behavior_comboBox.SelectedIndexChanged += new EventHandler(_Behavor_comboBox_SelectedIndexChanged);


            foreach (StoredProcedureParameter p in _sp.Parameters)
            {
                dS.Parms.AddParmsRow(p.Name, p.IsOutputParameter, Utils.GetDescription(p), p.DataType.Name, p.DataType.MaximumLength, p.DataType.NumericPrecision, p.DataType.NumericScale, p.DefaultValue);
            }

            dS.Parms.ParmsRowChanged += new DS.ParmsRowChangeEventHandler(Parms_ParmsRowChanged);
        }

        void _Behavor_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Behavior = _Behavior_comboBox.SelectedItem.ToString();
        }


        void Parms_ParmsRowChanged(object sender, DS.ParmsRowChangeEvent e)
        {
            Utils.SetDescription(_sp.Parameters[e.Row.Name], e.Row.Desc);
        }

        private void _Memo_textBox_TextChanged(object sender, EventArgs e)
        {
            this.Description = _Desc_richTextBox.Text;
        }

        private void _ResultType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SingleLine_checkBox.Enabled = _ResultType_comboBox.SelectedIndex > 3;
            if (!_SingleLine_checkBox.Enabled) _SingleLine_checkBox.Checked = false;

            Type t = _ResultType_comboBox.SelectedItem.GetType();
            if (t.Name == "String")
            {
                this.ResultType = _ResultType_comboBox.SelectedItem.ToString();
                this.ResultTypeSchema = "";
            }
            else if (t.Name == "Table")
            {
                var o = (Table)_ResultType_comboBox.SelectedItem;
                this.ResultType = o.Name;
                this.ResultTypeSchema = o.Schema;
            }
            else if (t.Name == "View")
            {
                var o = (Microsoft.SqlServer.Management.Smo.View)_ResultType_comboBox.SelectedItem;
                this.ResultType = o.Name;
                this.ResultTypeSchema = o.Schema;
            }
            else if (t.Name == "UserDefinedTableType")
            {
                var o = (UserDefinedTableType)_ResultType_comboBox.SelectedItem;
                this.ResultType = o.Name;
                this.ResultTypeSchema = o.Schema;
            }
        }

        private void _MethodName_textBox_TextChanged(object sender, EventArgs e)
        {
            //todo: 判断如果该过程属于某表，则方法名不应和表自带方法相冲
            //如果方法名和对象同名，存空值

            if (_MethodName_textBox.Text == Utils.GetEscapeName(_sp)) this.MethodName = null;
            else this.MethodName = _MethodName_textBox.Text;
        }

        private void _SingleLine_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.IsSingleLineResult = _SingleLine_checkBox.Checked;
        }

        private void _dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dataGridView.Rows.Count) return;
            if (_dataGridView.Columns[e.ColumnIndex].Name == "Desc")
            {
                StoredProcedureParameter p = _sp.Parameters[_dataGridView.Rows[e.RowIndex].Cells[0].Value as string];
                string memo = _dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
                Utils.SetDescription(p, memo);
            }
        }

        private void _Extract_button_Click(object sender, EventArgs e)
        {
            using (TextReader tr = new StringReader(_sp.TextHeader))
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
