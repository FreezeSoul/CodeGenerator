using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_EntityBLL : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_EntityBLL";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"EntityBLL层";
            }
        }
        public override string PropertyTips
        {
            get
            {
                return @"使用EntityFramework";
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
                    {"TableNameEntityBLL.tt","{0}EntityBLL.cs"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Table; }
        }
    }
}
