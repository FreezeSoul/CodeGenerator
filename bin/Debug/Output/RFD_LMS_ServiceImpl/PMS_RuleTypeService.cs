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
	public partial class PMS_RuleTypeService:IPMS_RuleTypeService
	{
		
		private IPMS_RuleTypeDao _pMS_RuleTypeDao;
		
        public PMS_RuleTypeService()
        { }
		
		/// <summary>
        /// 是否存在该记录
        /// </summary>
		public bool Exists(Int32 ruletypeid)
		{
			return _pMS_RuleTypeDao.Exists(ruletypeid);
		}
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(RFD.LMS.Model.Permission.PMS_RuleType model)
		{
			return _pMS_RuleTypeDao.Add(model);
		}
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(RFD.LMS.Model.Permission.PMS_RuleType model)
		{
			return _pMS_RuleTypeDao.Update(model);
		}
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(Int32 ruletypeid)
		{
			return _pMS_RuleTypeDao.Delete(ruletypeid);
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public RFD.LMS.Model.Permission.PMS_RuleType GetModel(Int32 ruletypeid)
		{
			return _pMS_RuleTypeDao.GetModel(ruletypeid);
		}
		
		public int GetDataTableCount(string searchString,Dictionary<string, object> searchParams)
		{
			return _pMS_RuleTypeDao.GetDataTableCount(searchString,searchParams);
		}
		
		public DataTable GetDataTable(Dictionary<string, object> searchParams)
		{
			return _pMS_RuleTypeDao.GetDataTable(searchParams);
		}
		
		public DataTable GetDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams)
		{
			return _pMS_RuleTypeDao.GetDataTable(searchString,sortColumn,searchParams);
		}
		
		public DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams, int rowStart, int rowEnd)
		{
			return _pMS_RuleTypeDao.GetPageDataTable(searchString,sortColumn,searchParams, rowStart, rowEnd);
		}
	}
}
