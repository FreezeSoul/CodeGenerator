using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Components.T4
{
    public class Gen_RFD_LMS : Gen_T4TemplateTableBase
    {
        public override string PropertyName
        {
            get
            {
                return @"Gen_RFD_LMS";
            }
        }
        public override string PropertyCaption
        {
            get
            {
                return @"Gen_RFD_LMS多层";
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
                    {"RFD_LMS_Dao.tt","RFD_LMS_AdoDao\\{0}Dao.cs"},
                    {"RFD_LMS_Dao_Partial.tt","RFD_LMS_AdoDao\\{0}Dao_Partial.cs"},
                    {"RFD_LMS_Domain.tt","RFD_LMS_Domain\\I{0}Dao.cs"},
                    {"RFD_LMS_Domain_Partial.tt","RFD_LMS_Domain\\I{0}Dao_Partial.cs"},
                    {"RFD_LMS_Service.tt","RFD_LMS_Service\\I{0}Service.cs"},
                    {"RFD_LMS_Service_Partial.tt","RFD_LMS_Service\\I{0}Service_Partial.cs"},
                    {"RFD_LMS_ServiceImpl.tt","RFD_LMS_ServiceImpl\\{0}Service.cs"},
                    {"RFD_LMS_ServiceImpl_Partial.tt","RFD_LMS_ServiceImpl\\{0}Service_Partial.cs"},
                    {"RFD_LMS_WebUI_List.tt","RFD_LMS_WebUI\\{0}_List.aspx"},
                    {"RFD_LMS_WebUI_List_Cs.tt","RFD_LMS_WebUI\\{0}_List.aspx.cs"},
                    {"RFD_LMS_WebUI_List_Designer.tt","RFD_LMS_WebUI\\{0}_List.aspx.Designer.cs"},
                    {"RFD_LMS_WebUI_Edit.tt","RFD_LMS_WebUI\\{0}_Edit.aspx"},
                    {"RFD_LMS_WebUI_Edit_Cs.tt","RFD_LMS_WebUI\\{0}_Edit.aspx.cs"},
                    {"RFD_LMS_WebUI_Edit_Designer.tt","RFD_LMS_WebUI\\{0}_Edit.aspx.Designer.cs"},
                    {"RFD_LMS_Model.tt","RFD_LMS_Model\\{0}.cs"}
                };
            }
        }
        public override SqlElementTypes TargetSqlElementType
        {
            get { return SqlElementTypes.Table; }
        }
    }
}
