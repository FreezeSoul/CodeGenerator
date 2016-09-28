using System;
using System.Collections.Generic;
using System.Text;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.GenSchema
{
	public class Gen_Database_SchemaManage : IGenComponent
	{
		#region Init

		public Gen_Database_SchemaManage()
		{
			this._properties.Add(GenProperties.Name, "Gen_Database_SchemaManage");
			this._properties.Add(GenProperties.Caption, "4. 多配置生成管理器");
			this._properties.Add(GenProperties.Group, "");
			this._properties.Add(GenProperties.Tips, "用于针对同一个数据库的多种过滤和生成方案的生成配置管理");
		}
		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.Database; }
		}

		#endregion

		#region Misc

		protected Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
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
			// todo:popup manage form
			using (FGen_Database_SchemaManage f = new FGen_Database_SchemaManage(_db))
			{
				f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				f.ShowDialog();
			}

			return null;
		}
	}
}
