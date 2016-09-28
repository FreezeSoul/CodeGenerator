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
	public partial class PMS_RuleTypeDao : DaoBase, IPMS_RuleTypeDao
	{
		
		private const string TableName = @"PMS_RuleType";
		
		private const string PagingTemplate = @"SELECT  RowIndex ,
																					T.*
																			FROM    ( SELECT    T2.* ,
																								ROW_NUMBER() OVER ( ORDER BY {0} DESC ) AS RowIndex
																					  FROM   ( {1} )  T2
																					) AS T
																			WHERE   T.RowIndex > {2}
																			AND T.RowIndex <= {3}";
		
		public PMS_RuleTypeDao()
		{
		}
		
		public bool Exists(Int32 ruletypeid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("select count(1) from {0}",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","RuleTypeId"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","RuleTypeId"),ruletypeid)
											};
			return Convert.ToInt64(SqlHelper.ExecuteScalar(ReadOnlyConnection, CommandType.Text, strSql.ToString(), sqlParams.ToArray())) > 0;
		}
		
		
		
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(RFD.LMS.Model.Permission.PMS_RuleType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("insert into {0}(",TableName));			
															strSql.Append(" RuleTypeKey , ");
												strSql.Append(" RuleTypeName , ");
												strSql.Append(" SourceType , ");
												strSql.Append(" SourceName , ");
												strSql.Append(" SubSysKey , ");
												strSql.Append(" IsDelete , ");
												strSql.Append(" CreatBy , ");
												strSql.Append(" CreateTime , ");
												strSql.Append(" UpdateBy , ");
												strSql.Append(" UpdateTime  ");
									strSql.Append(") values (");			
															strSql.Append(" @RuleTypeKey , ");
												strSql.Append(" @RuleTypeName , ");
												strSql.Append(" @SourceType , ");
												strSql.Append(" @SourceName , ");
												strSql.Append(" @SubSysKey , ");
												strSql.Append(" @IsDelete , ");
												strSql.Append(" @CreatBy , ");
												strSql.Append(" @CreateTime , ");
												strSql.Append(" @UpdateBy , ");
												strSql.Append(" @UpdateTime  ");
									strSql.Append(") ");
			strSql.Append(";select @@IDENTITY");
			 SqlParameter[] parameters = {
																					new SqlParameter(string.Format("@{0}","RuleTypeKey"), model.RuleTypeKey),
																					new SqlParameter(string.Format("@{0}","RuleTypeName"), model.RuleTypeName),
																					new SqlParameter(string.Format("@{0}","SourceType"), model.SourceType),
																					new SqlParameter(string.Format("@{0}","SourceName"), model.SourceName),
																					new SqlParameter(string.Format("@{0}","SubSysKey"), model.SubSysKey),
																					new SqlParameter(string.Format("@{0}","IsDelete"), model.IsDelete),
																					new SqlParameter(string.Format("@{0}","CreatBy"), model.CreatBy),
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
		public bool Update(RFD.LMS.Model.Permission.PMS_RuleType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("update {0} set ",TableName));
														            
			strSql.Append(" RuleTypeKey = @RuleTypeKey ,	 ");
										            
			strSql.Append(" RuleTypeName = @RuleTypeName ,	 ");
										            
			strSql.Append(" SourceType = @SourceType ,	 ");
										            
			strSql.Append(" SourceName = @SourceName ,	 ");
										            
			strSql.Append(" SubSysKey = @SubSysKey ,	 ");
										            
			strSql.Append(" IsDelete = @IsDelete ,	 ");
										            
			strSql.Append(" CreatBy = @CreatBy ,	 ");
										            
			strSql.Append(" CreateTime = @CreateTime ,	 ");
										            
			strSql.Append(" UpdateBy = @UpdateBy ,	 ");
										            
			strSql.Append(" UpdateTime = @UpdateTime  ");
									
			strSql.Append(string.Format(" where {0} = @{0}","RuleTypeId"));
			 SqlParameter[] parameters = {
																	new SqlParameter(string.Format("@{0}","RuleTypeKey"), model.RuleTypeKey),														new SqlParameter(string.Format("@{0}","RuleTypeName"), model.RuleTypeName),														new SqlParameter(string.Format("@{0}","SourceType"), model.SourceType),														new SqlParameter(string.Format("@{0}","SourceName"), model.SourceName),														new SqlParameter(string.Format("@{0}","SubSysKey"), model.SubSysKey),														new SqlParameter(string.Format("@{0}","IsDelete"), model.IsDelete),														new SqlParameter(string.Format("@{0}","CreatBy"), model.CreatBy),														new SqlParameter(string.Format("@{0}","CreateTime"), model.CreateTime),														new SqlParameter(string.Format("@{0}","UpdateBy"), model.UpdateBy),														new SqlParameter(string.Format("@{0}","UpdateTime"), model.UpdateTime)									};
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
		public bool Delete(Int32 ruletypeid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append(string.Format("delete from {0} ",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","RuleTypeId"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","RuleTypeId"),ruletypeid)
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
		public RFD.LMS.Model.Permission.PMS_RuleType GetModel(Int32 ruletypeid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select RuleTypeId, RuleTypeKey, RuleTypeName, SourceType, SourceName, SubSysKey, IsDelete, CreatBy, CreateTime, UpdateBy, UpdateTime  ");			
			strSql.Append(string.Format("  from {0} ",TableName));
			strSql.Append(string.Format(" where {0} = @{0}","RuleTypeId"));
			var sqlParams = new List<SqlParameter>()
											{
												new SqlParameter(string.Format("@{0}","RuleTypeId"),ruletypeid)
											};
			var model=new RFD.LMS.Model.Permission.PMS_RuleType();
			DataSet ds= SqlHelper.ExecuteDataset(ReadOnlyConnection,  CommandType.Text,strSql.ToString(),sqlParams.ToArray());
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["RuleTypeId"].ToString()!="")
				{
					model.RuleTypeId=Int32.Parse(ds.Tables[0].Rows[0]["RuleTypeId"].ToString());
				}
																																				model.RuleTypeKey= ds.Tables[0].Rows[0]["RuleTypeKey"].ToString();
																																model.RuleTypeName= ds.Tables[0].Rows[0]["RuleTypeName"].ToString();
																												if(ds.Tables[0].Rows[0]["SourceType"].ToString()!="")
				{
					model.SourceType=Int32.Parse(ds.Tables[0].Rows[0]["SourceType"].ToString());
				}
																																				model.SourceName= ds.Tables[0].Rows[0]["SourceName"].ToString();
																																model.SubSysKey= ds.Tables[0].Rows[0]["SubSysKey"].ToString();
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
																if(ds.Tables[0].Rows[0]["CreatBy"].ToString()!="")
				{
					model.CreatBy=Int32.Parse(ds.Tables[0].Rows[0]["CreatBy"].ToString());
				}
																																																								if(ds.Tables[0].Rows[0]["UpdateBy"].ToString()!="")
				{
					model.UpdateBy=Int32.Parse(ds.Tables[0].Rows[0]["UpdateBy"].ToString());
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
			strSql.Append("select RuleTypeId, RuleTypeKey, RuleTypeName, SourceType, SourceName, SubSysKey, IsDelete, CreatBy, CreateTime, UpdateBy, UpdateTime  ");			
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
