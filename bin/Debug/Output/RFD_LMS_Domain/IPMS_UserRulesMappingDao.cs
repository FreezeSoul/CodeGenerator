using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RFD.LMS.Model.Permission;

namespace RFD.LMS.Domain.Permission
{
	public partial interface IPMS_UserRulesMappingDao
	{
		/// <summary>
        /// 是否存在该记录
        /// </summary>
		bool Exists(Int32 id);
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(RFD.LMS.Model.Permission.PMS_UserRulesMapping model);
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(RFD.LMS.Model.Permission.PMS_UserRulesMapping model);
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(Int32 id);
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		RFD.LMS.Model.Permission.PMS_UserRulesMapping GetModel(Int32 id);
		
		int GetDataTableCount(string searchString,Dictionary<string, object> searchParams);
		
		DataTable GetDataTable(Dictionary<string, object> searchParams);
		
		DataTable GetDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams);
		
		DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams, int rowStart, int rowEnd);
	}
}
