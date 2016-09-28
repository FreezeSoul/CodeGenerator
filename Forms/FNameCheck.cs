using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;


namespace CodeGenerator
{
	public partial class FNameCheck : Form
	{
		public FNameCheck(Database db)
		{
			InitializeComponent();
			_db = db;
		}

		private Database _db;
	}
}