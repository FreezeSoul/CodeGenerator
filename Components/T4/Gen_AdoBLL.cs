using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_AdoBLL : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_AdoBLL";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"AdoBLL层";
            }
        }
        public override string PropertyTips
        {
            get
            {
                return @"使用ADO.NET";
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
                    {"ViewNameAdoBLL.tt","{0}AdoBLL.cs"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.View; }
        }
    }
}
