using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_DataBaseMetas : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_DataBaseMetas";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"DataBaseMetas.cs";
            }
        }
        public override string PropertyTips
        {
            get
            {
                return @"生成数据库元数据";
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
                    {"TabelMetaGenerator.tt","DataBaseMetas.cs"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Database; }
        }
    }
}
