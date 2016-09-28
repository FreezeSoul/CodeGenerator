using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Windows.Forms;

namespace CodeGenerator.Components.UI.SilverLight
{
	public class Gen_Database_DAL_Default : IGenComponent
	{

		#region Init

		public Gen_Database_DAL_Default()
		{
			this._properties.Add(GenProperties.Name, "Gen_UI_Default");
			this._properties.Add(GenProperties.Caption, "Gen_Silverlight_UI_Default 文件生成");
			this._properties.Add(GenProperties.Group, "");
			this._properties.Add(GenProperties.Tips, "生成Xaml默认界面元素");


			this._properties.Add(GenProperties.IsEnabled, "False");
		}
		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.Database; }
		}

		#endregion

		#region Misc

		Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
		public Dictionary<GenProperties, object> Properties
		{
			get
			{
				return this._properties;
			}
		}

		public event System.ComponentModel.CancelEventHandler OnProcessing;

		private Server _server;
		public Server Server
		{
			set { _server = value; }
		}

		private Database _db;
		public Database Database
		{
			set { _db = value; }
		}

		#endregion

		public bool Validate(params object[] sqlElements)
		{
			return true;
		}

		public GenResult Gen(params object[] sqlElements)
		{
			GenResult gr;

			gr = new GenResult(GenResultTypes.Files);
			gr.Files = new List<KeyValuePair<string, byte[]>>();

			string ns;
			DialogResult dr;
			using (FGen_Database_Config fgs = new FGen_Database_Config(_db))
			{
				dr = fgs.ShowDialog();
			}

			if (dr != DialogResult.OK)
			{
				//gr = new GenResult(GenResultTypes.Message);
				//gr.Message = null;
				//return gr;
			}

			ns = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;
			//isSupportWCF = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportWCF;
			Utils.SchemaSplitter = Utils._CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? "_" : null;

			gr = new GenResult(GenResultTypes.Files);
			gr.Files = new List<KeyValuePair<string, byte[]>>();

			using (FOutputText fw = new FOutputText("代码生成中，请稍后...", "", 350, 500, true))
			{
				fw.Show();
				fw.Activate();

				foreach (KeyValuePair<string, byte[]> key in Gen_Database_Default_XAML.Gen(_db, ns))
				{
					gr.Files.Add(key);
				}

				foreach (KeyValuePair<string, byte[]> keyvalue in Gen_Database_Default_CS.Gen(_db, ns))
				{
					gr.Files.Add(keyvalue);
				}
			}

			return gr;
		}

	}
}
