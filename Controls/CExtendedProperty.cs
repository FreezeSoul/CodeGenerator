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
    public partial class CExtendedProperty : UserControl
    {
        protected ExtendedProperty _ep = null;

        public CExtendedProperty(ExtendedProperty ep)
        {
            InitializeComponent();

            this._ep = ep;
            this.Load += new EventHandler(CExtendedProperty_Load);
        }

        void CExtendedProperty_Load(object sender, EventArgs e)
        {
            this._Content_richTextBox.Text = this._ep.Value as string;
        }
    }
}
