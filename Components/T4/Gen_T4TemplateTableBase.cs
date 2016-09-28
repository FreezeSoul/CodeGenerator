using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// SMO
using CodeGenerator.Misc;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TextTemplating;

namespace CodeGenerator.Components.T4
{
    public class Gen_T4TemplateTableBase : IGenComponent
    {
        #region Init
        public virtual string PropertyName
        {
            get
            {
                return string.Empty;
            }
        }
        public virtual string PropertyCaption
        {
            get
            {
                return string.Empty;
            }
        }
        public virtual string PropertyTips
        {
            get
            {
                return string.Empty;
            }
        }
        public virtual bool IsEnabled
        {
            get
            {
                return false;
            }
        }
        public virtual Dictionary<string, string> TemplateOutputs
        {
            get
            {
                return new Dictionary<string, string>()
                           {

                           };
            }
        }
        public Gen_T4TemplateTableBase()
        {
            this._properties.Add(GenProperties.Name, this.PropertyName);
            this._properties.Add(GenProperties.Caption, this.PropertyCaption);
            this._properties.Add(GenProperties.Group, "T4TemplateHost");
            this._properties.Add(GenProperties.Tips, this.PropertyTips);
            this._properties.Add(GenProperties.IsEnabled, this.IsEnabled);

        }
        public virtual SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Tables; }
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

        private List<string> selectedColumns = new List<string>();
        #endregion


        public bool Validate(params object[] sqlElements)
        {
            return true;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            Utils.LoadDatabaseDALGenSettingDS(_db);
            var ns = Utils._CurrrentDALGenSetting_CurrentScheme.Namespace;
            var selectedNames = new List<string>();
            if (this.TargetSqlElementType == SqlElementTypes.Database)
            {
                selectedNames.Add(_db.Name);
            }
            if (this.TargetSqlElementType == SqlElementTypes.Tables)
            {
                List<Table> uts = Utils.GetUserTables(_db);
                uts.ForEach(item => selectedNames.Add(item.Name));
            }
            if (this.TargetSqlElementType == SqlElementTypes.Views)
            {
                List<View> uvs = Utils.GetUserViews(_db);
                uvs.ForEach(item => selectedNames.Add(item.Name));
            }
            if (this.TargetSqlElementType == SqlElementTypes.Table || this.TargetSqlElementType == SqlElementTypes.View)
            {
                if (sqlElements == null || sqlElements.Length == 1)
                {
                    if (this.TargetSqlElementType == SqlElementTypes.Table)
                    {
                        selectedNames.Add(((Table)sqlElements[0]).Name);
                        foreach (Column c in ((Table)sqlElements[0]).Columns)
                        {
                            if (!Utils.GetDescription(c, Utils.EP_IsDisplay).ToLower().Equals("false") || c.InPrimaryKey)
                            {
                                selectedColumns.Add(c.Name);
                            }
                        }
                    }
                    if (this.TargetSqlElementType == SqlElementTypes.View)
                    {
                        selectedNames.Add(((View)sqlElements[0]).Name);
                    }
                }
            }
            var gr = new GenResult(GenResultTypes.Files);
            var files = GenT4(selectedNames);
            gr.Files = files.ToList();
            return gr;
        }

        private Dictionary<string, byte[]> GenT4(List<string> selectedNames)
        {
            var files = new Dictionary<string, byte[]>();
            var engine = new Engine();
            var host = new TemplateBaseHost();
            selectedNames.ForEach(name => TemplateOutputs.ToList().ForEach(item =>
                                                                               {
                                                                                   var input = File.ReadAllText(string.Format(System.Windows.Forms.Application.StartupPath + @"\T4Template\{0}", item.Key));
                                                                                   host.TemplateFileValue = string.Format("{0}", item.Key);
                                                                                   host.Session = new TextTemplatingSession {
                                                                                                                                { "SelectedName", name },
                                                                                                                                { "ConnectionString", _server.ConnectionContext.ConnectionString },
                                                                                                                                { "DataBaseName", _db.Name }
                                                                                                                            };
                                                                                   if (selectedColumns.Count > 0)
                                                                                       host.Session.Add("SelectedColumns", selectedColumns);
                                                                                   //CallContext.LogicalSetData("parameter1", parameter);
                                                                                   var output = engine.ProcessTemplate(input, host);
                                                                                   if (host.Errors.HasErrors)
                                                                                       for (int i = 0; i < host.Errors.Count; i++)
                                                                                           output += "\r\n" + host.Errors[i].ErrorText + " 行号" + host.Errors[i].Line;
                                                                                   //var utf8 = new UTF8Encoding(true);
                                                                                   //files.Add(string.Format(item.Value, name), utf8.GetBytes(output));
                                                                                   files.Add(string.Format(item.Value, name), Encoding.UTF8.GetBytes(output));
                                                                               }));
            return files;
        }
    }
}
