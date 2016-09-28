using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;
using System.Data;

namespace CodeGenerator.Components.BLL
{
	class Gen_Database_SetFilter : IGenComponent
	{
		#region Init

		public Gen_Database_SetFilter()
		{
			this._properties.Add(GenProperties.Name, "Gen_Database_SetFilter");
			this._properties.Add(GenProperties.Caption, "1. 设置过滤规则");
			this._properties.Add(GenProperties.Group, "");
			this._properties.Add(GenProperties.Tips, "设置过滤规则以决定哪些数据对象出现在树形列表中或是被生成");
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
			using (FFilter f = new FFilter(_db))
			{
				f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				f.ShowDialog();
			}
			return null;
		}
	}
}
