using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LMS.AdoNet.DbBase;
using Microsoft.ApplicationBlocks.Data;
using RFD.LMS.Model.Permission;
using RFD.LMS.Domain.Permission;

namespace RFD.LMS.AdoDao.Permission
{
	public partial class PMS_UserRulesMappingDao : DaoBase, IPMS_UserRulesMappingDao
	{
		
		private const string TableName = @"PMS_UserRulesMapping";
		
		private const string PagingTemplate = @"SELECT  RowIndex ,
																					T.*
																			FROM    ( SELECT    T2.* ,
																								ROW_NUMBER() OVER ( ORDER BY {0} DESC ) AS RowIndex
																					  FROM   ( {1} )  T2
																					) AS T
																			WHERE   T.RowIndex > {2}
																			AND T.RowIndex <= {3}";
		
		public PMS_UserRulesMappingDao()
		{
		}
		
		public bool Exists(Int32 id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("select count(1) from {0}",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","Id"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","Id"),id)
											};
			return Convert.ToInt64(SqlHelper.ExecuteScalar(ReadOnlyConnection, CommandType.Text, strSql.ToString(), sqlParams.ToArray())) > 0;
		}
		
		
		
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(RFD.LMS.Model.Permission.PMS_UserRulesMapping model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("insert into {0}(",TableName));			
															strSql.Append(" UserCode , ");
												strSql.Append(" RuleId , ");
												strSql.Append(" IsDelete , ");
												strSql.Append(" CreateBy , ");
												strSql.Append(" CreateTime , ");
												strSql.Append(" UpdateBy , ");
												strSql.Append(" UpdateTime  ");
									strSql.Append(") values (");			
															strSql.Append(" @UserCode , ");
												strSql.Append(" @RuleId , ");
												strSql.Append(" @IsDelete , ");
												strSql.Append(" @CreateBy , ");
												strSql.Append(" @CreateTime , ");
												strSql.Append(" @UpdateBy , ");
												strSql.Append(" @UpdateTime  ");
									strSql.Append(") ");
			strSql.Append(";select @@IDENTITY");
			 SqlParameter[] parameters = {
																					new SqlParameter(string.Format("@{0}","UserCode"), model.UserCode),
																					new SqlParameter(string.Format("@{0}","RuleId"), model.RuleId),
																					new SqlParameter(string.Format("@{0}","IsDelete"), model.IsDelete),
																					new SqlParameter(string.Format("@{0}","CreateBy"), model.CreateBy),
																					new SqlParameter(string.Format("@{0}","CreateTime"), model.CreateTime),
																					new SqlParameter(string.Format("@{0}","UpdateBy"), model.UpdateBy),
																					new SqlParameter(string.Format("@{0}","UpdateTime"), model.UpdateTime)									};
			object obj = SqlHelper.ExecuteScalar(ReadOnlyConnection, CommandType.Text, strSql.ToString(), parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}		
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(RFD.LMS.Model.Permission.PMS_UserRulesMapping model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("update {0} set ",TableName));
														            
			strSql.Append(" UserCode = @UserCode ,	 ");
										            
			strSql.Append(" RuleId = @RuleId ,	 ");
										            
			strSql.Append(" IsDelete = @IsDelete ,	 ");
										            
			strSql.Append(" CreateBy = @CreateBy ,	 ");
										            
			strSql.Append(" CreateTime = @CreateTime ,	 ");
										            
			strSql.Append(" UpdateBy = @UpdateBy ,	 ");
										            
			strSql.Append(" UpdateTime = @UpdateTime  ");
									
			strSql.Append(string.Format(" where {0} = @{0}","Id"));
			 SqlParameter[] parameters = {
								new SqlParameter(string.Format("@{0}","Id"), model.Id),								new SqlParameter(string.Format("@{0}","UserCode"), model.UserCode),								new SqlParameter(string.Format("@{0}","RuleId"), model.RuleId),								new SqlParameter(string.Format("@{0}","IsDelete"), model.IsDelete),								new SqlParameter(string.Format("@{0}","CreateBy"), model.CreateBy),								new SqlParameter(string.Format("@{0}","CreateTime"), model.CreateTime),								new SqlParameter(string.Format("@{0}","UpdateBy"), model.UpdateBy),								new SqlParameter(string.Format("@{0}","UpdateTime"), model.UpdateTime),						};
			int rows = SqlHelper.ExecuteNonQuery(ReadOnlyConnection, CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(Int32 id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("delete from {0} ",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","Id"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","Id"),id)
											};
			int rows = SqlHelper.ExecuteNonQuery(ReadOnlyConnection, CommandType.Text, strSql.ToString(), sqlParams.ToArray());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public RFD.LMS.Model.Permission.PMS_UserRulesMapping GetModel(Int32 id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id, UserCode, RuleId, IsDelete, CreateBy, CreateTime, UpdateBy, UpdateTime  ");			
			strSql.Append(string.Format("  from {0} ",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","Id"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","Id"),id)
											};
			var model=new RFD.LMS.Model.Permission.PMS_UserRulesMapping();
			DataSet ds= SqlHelper.ExecuteDataset(ReadOnlyConnection,  CommandType.Text,strSql.ToString(),sqlParams.ToArray());
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=Int32.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
																																				model.UserCode= ds.Tables[0].Rows[0]["UserCode"].ToString();
																												if(ds.Tables[0].Rows[0]["RuleId"].ToString()!="")
				{
					model.RuleId=Int32.Parse(ds.Tables[0].Rows[0]["RuleId"].ToString());
				}
																																																if(ds.Tables[0].Rows[0]["IsDelete"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsDelete"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsDelete"].ToString().ToLower()=="true"))
					{
					model.IsDelete= true;
					}
					else
					{
					model.IsDelete= false;
					}
				}
																if(ds.Tables[0].Rows[0]["CreateBy"].ToString()!="")
				{
					model.CreateBy=Int32.Parse(ds.Tables[0].Rows[0]["CreateBy"].ToString());
				}
																																if(ds.Tables[0].Rows[0]["CreateTime"].ToString()!="")
				{
					model.CreateTime=System.DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
				}
																																if(ds.Tables[0].Rows[0]["UpdateBy"].ToString()!="")
				{
					model.UpdateBy=Int32.Parse(ds.Tables[0].Rows[0]["UpdateBy"].ToString());
				}
																																if(ds.Tables[0].Rows[0]["UpdateTime"].ToString()!="")
				{
					model.UpdateTime=System.DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
				}
																														
				return model;
			}
			else
			{
				return null;
			}
		}
		
		
		public int GetDataTableCount(string searchString,Dictionary<string, object> searchParams)
		{
			var sqlStr = string.Format(@"SELECT COUNT(*) FROM {0} {1}",TableName,searchString);
			var sqlParams = new List<SqlParameter>();
			if (searchParams != null)
			{
				searchParams.ToList().ForEach(item =>sqlParams.Add(new SqlParameter(string.Format("@{0}",item.Key), item.Value)));
			}
			var obj = SqlHelper.ExecuteScalar(ReadOnlyConnection, CommandType.Text, sqlStr, sqlParams.ToArray());
			int i = 0;
			if(obj!=null)
			{
				int.TryParse(obj.ToString(),out i);
			}
			return i;
		}
		
		public DataTable GetDataTable(Dictionary<string, object> searchParams)
		{
		    StringBuilder strSql = new StringBuilder();
			strSql.Append("select Id, UserCode, RuleId, IsDelete, CreateBy, CreateTime, UpdateBy, UpdateTime  ");			
			strSql.Append(string.Format("  from {0} ",TableName));
			strSql.Append(" where 1 = 1 ");
			var sqlParams = new List<SqlParameter>();
			if (searchParams != null)
			{
				searchParams.ToList().ForEach(item =>
						{
							strSql.Append(string.Format(" and {0} = @{0}",item.Key));
							sqlParams.Add(new SqlParameter(string.Format("@{0}",item.Key), item.Value));
						});
			}
			return SqlHelper.ExecuteDataset(ReadOnlyConnection,  CommandType.Text,strSql.ToString(),sqlParams.ToArray()).Tables[0];
		}
		
		public DataTable GetDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams)
		{
			var sqlStr = string.Format(@"SELECT * FROM {0} {1} ORDER BY {2} DESC",TableName,searchString,sortColumn);
			var sqlParams = new List<SqlParameter>();
			if (searchParams != null)
			{
				searchParams.ToList().ForEach(item =>sqlParams.Add(new SqlParameter(string.Format("@{0}",item.Key), item.Value)));
			}
			return SqlHelper.ExecuteDataset(ReadOnlyConnection,  CommandType.Text,sqlStr,sqlParams.ToArray()).Tables[0];
		}
		
		public DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams, int rowStart, int rowEnd)
		{
			var sqlStr = string.Format(@"SELECT * FROM {0} {1}", TableName, searchString);

			sqlStr = string.Format(PagingTemplate, sortColumn, sqlStr, rowStart, rowEnd);
			var sqlParams = new List<SqlParameter>();
			if (searchParams != null)
			{
				searchParams.ToList().ForEach(item =>sqlParams.Add(new SqlParameter(string.Format("@{0}",item.Key), item.Value)));
			}
		   return SqlHelper.ExecuteDataset(ReadOnlyConnection,  CommandType.Text,sqlStr,sqlParams.ToArray()).Tables[0];
		}
	}
}
