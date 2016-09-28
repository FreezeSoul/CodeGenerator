using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LMS.Common.Utility;
using LMS.Util;
using RFD.LMS.Model;
using RFD.LMS.Model.Permission;
using RFD.LMS.Service;
using RFD.LMS.Service.Permission;

namespace RFD.LMS.WebUI.Permission
{
    public partial class PMS_RuleType_Edit : PageBase
    {

        private IPMS_RuleTypeService service = ServiceLocator.GetService<IPMS_RuleTypeService>();

        #region Property
        private string RuleTypeId
        {
            get
            {
                return Request.Params["RuleTypeId"] != null ? Request.Params["RuleTypeId"].ToString() : string.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownList();
                BindEditData();
            }
        }

        private void BindEditData()
        {
            if (RuleTypeId != string.Empty)
            {
                var model = service.GetModel(Int32.Parse(RuleTypeId));
                if (model != null)
                {
					//TODO:绑定数据
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDropDownList()
        {
            
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (RuleTypeId != string.Empty)
                {
                    var key =Int32.Parse(RuleTypeId);
                    service.Update(new PMS_RuleType()
                    {
                       //TODO:初始化修改数据
                    });
                }
                else
                {
                   	service.Add(new PMS_RuleType()
                    {
                       //TODO:初始化修改数据
                    });
                }
            }
            catch (Exception)
            {
                Alert("存储失败，请重试!");
            }
        }
    }
}
