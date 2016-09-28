using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_XtraFormList : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_XtraFormList";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"查询列表";
            }
        }
        public override string PropertyTips
        {
            get
            {
                return @"使用DevExpress控件";
            }
        }
        public override bool IsEnabled
        {
            get
            {
                return true;
            }
        }
        public override Dictionary<string, string> TemplateOutputs
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"TableName_List_XtraUserControl.tt","{0}_List_XtraUserControl.cs"},
                    {"TableName_List_XtraUserControl.Designer.tt","{0}_List_XtraUserControl.Designer.cs"},
                    {"TableName_List_XtraUserControl.resx.tt","{0}_List_XtraUserControl.resx"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Table; }
        }
    }
}
