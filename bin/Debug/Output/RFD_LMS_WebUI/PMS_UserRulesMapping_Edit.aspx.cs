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
    public partial class PMS_UserRulesMapping_Edit : PageBase
    {

        private IPMS_UserRulesMappingService service = ServiceLocator.GetService<IPMS_UserRulesMappingService>();

        #region Property
        private string Id
        {
            get
            {
                return Request.Params["Id"] != null ? Request.Params["Id"].ToString() : string.Empty;
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
            if (Id != string.Empty)
            {
                var model = service.GetModel(Int32.Parse(Id));
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
                if (Id != string.Empty)
                {
                    var key =Int32.Parse(Id);
                    service.Update(new PMS_UserRulesMapping()
                    {
                       //TODO:初始化修改数据
                    });
                }
                else
                {
                   	service.Add(new PMS_UserRulesMapping()
                    {
                       //TODO:初始化修改数据
                    });
                }
				Alert("保存成功!");
            }
            catch (Exception)
            {
                Alert("存储失败，请重试!");
            }
        }
    }
}
