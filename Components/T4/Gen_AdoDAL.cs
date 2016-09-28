using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_AdoDAL : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_AdoDAL";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"AdoDAL层";
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
                    {"ViewNameAdoDAL.tt","{0}AdoDAL.cs"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.View; }
        }
    }
}
