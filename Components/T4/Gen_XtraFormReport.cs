using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_XtraFormReport : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_XtraFormReport";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"报表查询";
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
                    {"ViewName_Report_XtraUserControl.tt","{0}_Report_XtraUserControl.cs"},
                    {"ViewName_Report_XtraUserControl.Designer.tt","{0}_Report_XtraUserControl.Designer.cs"},
                    {"ViewName_Report_XtraUserControl.resx.tt","{0}_Report_XtraUserControl.resx"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.View; }
        }
    }
}
