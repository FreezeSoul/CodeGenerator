using System;
using System.Globalization;
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
	public partial class FConnector : Form
	{
		public FConnector()
		{
			InitializeComponent();

			_ServerType_ComboBox.SelectedIndex = 0;
			_Authentication_ComboBox.SelectedIndex = 0;
		}

		private ServerConnection _sc = null;

		public FConnector(ServerConnection sc)
			: this()
		{
			_sc = sc;
		}

		private void FConnector_Load(object sender, EventArgs e)
		{
			_ServerType_ComboBox.SelectedIndex = Properties.Settings.Default.Engine;
			_ServerName_ComboBox.Text = Properties.Settings.Default.Server;
			_Authentication_ComboBox.SelectedIndex = Properties.Settings.Default.IntegratedSecurity ? 0 : 1;
			_Username_ComboBox.Text = Properties.Settings.Default.Username;
			_Password_TextBox.Text = Properties.Settings.Default.Password;
			_RememberPassword_CheckBox.Checked = Properties.Settings.Default.Remember;

			if (!_RememberPassword_CheckBox.Checked) _Password_TextBox.Text = "";
		}

		private void _Authentication_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_Authentication_ComboBox.SelectedIndex == 0)
			{
				_Username_Label.Text = "&Username:";
				_Username_Label.Enabled = _Password_Label.Enabled = _Username_ComboBox.Enabled = _Password_TextBox.Enabled = _RememberPassword_CheckBox.Enabled = false;
				_Username_ComboBox.Text = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
			}
			else
			{
				_Username_Label.Text = "&Login:";
				_Username_ComboBox.Text = "";
				_Username_Label.Enabled = _Password_Label.Enabled = _Username_ComboBox.Enabled = _Password_TextBox.Enabled = _RememberPassword_CheckBox.Enabled = true;
			}
		}

		private void _Connect_Button_Click(object sender, EventArgs e)
		{
			if (_Authentication_ComboBox.SelectedIndex > 0 && _Username_ComboBox.Text.Trim().Length == 0)
			{
				MessageBox.Show("The username can't be empty£¡");
				return;
			}

			Cursor cur = this.Cursor;   //backup

			this.Cursor = Cursors.WaitCursor;
			bool b = false;

			try
			{
				_sc.ServerInstance = _ServerName_ComboBox.Text;
				_sc.ConnectTimeout = 10;
				if (_Authentication_ComboBox.SelectedIndex == 0)
				{
					_sc.LoginSecure = true;
				}
				else
				{
					_sc.LoginSecure = false;
					_sc.Login = _Username_ComboBox.Text;
					_sc.Password = _Password_TextBox.Text;
				}
				_sc.Connect();

				b = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				this.Cursor = cur;
			}

			if (b)
			{
				this.DialogResult = DialogResult.OK;
				Properties.Settings.Default.Engine = _ServerType_ComboBox.SelectedIndex;
				Properties.Settings.Default.Server = _ServerName_ComboBox.Text;
				Properties.Settings.Default.IntegratedSecurity = _Authentication_ComboBox.SelectedIndex == 0;
				Properties.Settings.Default.Username = _Username_ComboBox.Text;
				Properties.Settings.Default.Password = _Password_TextBox.Text;
				Properties.Settings.Default.Remember = _RememberPassword_CheckBox.Checked;
				Properties.Settings.Default.Save();
			}
			// else this.DialogResult = DialogResult.No;
		}

		private void _Cancel_Button_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void _Help_Button_Click(object sender, EventArgs e)
		{

		}

		private void _ServerName_ComboBox_DropDown(object sender, EventArgs e)
		{
			if (_ServerName_ComboBox.Items.Count == 0)
			{
				DataTable dt;

				dt = SmoApplication.EnumAvailableSqlServers(false);
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						_ServerName_ComboBox.Items.Add(dr["Name"]);
					}

					// Set local server as default
					_ServerName_ComboBox.SelectedIndex = _ServerName_ComboBox.FindStringExact(System.Environment.MachineName);

					if (_ServerName_ComboBox.SelectedIndex < 0)
					{
						_ServerName_ComboBox.SelectedIndex = 0;
					}
				}
				else
				{
					MessageBox.Show("sql server was not found!");
				}

			}
		}

	}
}