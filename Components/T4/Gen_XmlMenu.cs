using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_XmlMenu : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_XmlMenu";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"XmlData.xml";
            }
        }
        public override string PropertyTips
        {
            get
            {
                return @"生成XML菜单";
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
                    {"XmlMenuGenerator.tt","MenuData.xml"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Database; }
        }
    }
}