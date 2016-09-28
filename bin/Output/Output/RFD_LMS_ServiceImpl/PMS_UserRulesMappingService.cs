using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RFD.LMS.Domain.Permission;
using RFD.LMS.Service.Permission;

namespace RFD.LMS.ServiceImpl.Permission
{
	public partial class PMS_UserRulesMappingService:IPMS_UserRulesMappingService
	{
		
		private IPMS_UserRulesMappingDao _pMS_UserRulesMappingDao;
		
        public PMS_UserRulesMappingService()
        { }
		
		/// <summary>
        /// 是否存在该记录
        /// </summary>
		public bool Exists(Int32 id)
		{
			return _pMS_UserRulesMappingDao.Exists(id);
		}
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(RFD.LMS.Model.Permission.PMS_UserRulesMapping model)
		{
			return _pMS_UserRulesMappingDao.Add(model);
		}
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(RFD.LMS.Model.Permission.PMS_UserRulesMapping model)
		{
			return _pMS_UserRulesMappingDao.Update(model);
		}
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(Int32 id)
		{
			return _pMS_UserRulesMappingDao.Delete(id);
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public RFD.LMS.Model.Permission.PMS_UserRulesMapping GetModel(Int32 id)
		{
			return _pMS_UserRulesMappingDao.GetModel(id);
		}
		
		public int GetDataTableCount(string searchString,Dictionary<string, object> searchParams)
		{
			return _pMS_UserRulesMappingDao.GetDataTableCount(searchString,searchParams);
		}
		
		public DataTable GetDataTable(Dictionary<string, object> searchParams)
		{
			return _pMS_UserRulesMappingDao.GetDataTable(searchParams);
		}
		
		public DataTable GetDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams)
		{
			return _pMS_UserRulesMappingDao.GetDataTable(searchString,sortColumn,searchParams);
		}
		
		public DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams, int rowStart, int rowEnd)
		{
			return _pMS_UserRulesMappingDao.GetPageDataTable(searchString,sortColumn,searchParams, rowStart, rowEnd);
		}
	}
}
