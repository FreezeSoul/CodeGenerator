using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LMS.Util;
using RFD.LMS.Model;
using RFD.LMS.Model.Permission;
using RFD.LMS.Service;
using RFD.LMS.Service.Permission;

namespace RFD.LMS.WebUI.Permission
{
    public partial class PMS_RuleType_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuidPage();//页面加载的时候绑定。
                BindDropDownList();
            }
            // 注册分页用户控件事件处理
            if (IsPostBack)
            {
                Pager1.PagerPageChanged += new EventHandler(AspNetPager_PageChanged);
            }
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AspNetPager_PageChanged(object sender, EventArgs e)
        {
            BuidPage();
        }

        /// <summary>
        /// 查询并绑定GridView
        /// </summary>
        private void BuidPage()
        {
            DataTable dataTable = DtSource();
            int tbRowCount = dataTable.Rows.Count;
            if (tbRowCount == 0)
            {
                dataTable.Rows.Add(dataTable.NewRow());
            }
            //分页
            Pager1.RecordCount = dataTable.Rows.Count;
            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = Pager1.PageSize;
            pds.CurrentPageIndex = Pager1.CurrentPageIndex - 1;
            pds.DataSource = dataTable.DefaultView;

            GridView1.DataSource = pds;
            GridView1.DataBind();
            int columnCount = GridView1.Columns.Count;
            //如果数据为空
            if (tbRowCount == 0)
            {
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columnCount;
                GridView1.Rows[0].Cells[0].Text = "没有数据";
                GridView1.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
        }

        private void BindDropDownList()
        {
            
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        private DataTable DtSource()
        {
            //实例化业务层
            var server = ServiceLocator.GetService<IPMS_RuleTypeService>();
            var model = new PMS_RuleType();
			var searchParams = new Dictionary<string, object>();
			//ToDo:查询条件
            DataTable dataTable = server.GetDataTable(searchParams);
            return dataTable;
        }


        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnQuery_Click(object sender, EventArgs e)
        {
            BuidPage();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteById")
            {
                var key = Int32.Parse(e.CommandArgument.ToString());
                var server = ServiceLocator.GetService<IPMS_RuleTypeService>();
                server.Delete(key);
                BuidPage();
                Alert("删除成功！");
            }
        }

    }
}
