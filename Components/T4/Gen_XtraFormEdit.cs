using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_XtraFormEdit : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_XtraFormEdit";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"编辑窗体";
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
                    {"TableName_Edit_XtraUserControl.tt","{0}_Edit_XtraUserControl.cs"},
                    {"TableName_Edit_XtraUserControl.Designer.tt","{0}_Edit_XtraUserControl.Designer.cs"},
                    {"TableName_Edit_XtraUserControl.resx.tt","{0}_Edit_XtraUserControl.resx"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Table; }
        }
    }
}
