using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace RFD.LMS.Model.Permission
{
	/// <summary>
    /// 规则类型
    /// </summary>
	[Serializable]
	public partial class PMS_RuleType
	{
			/// <summary>
			/// RuleTypeId
	        /// </summary>		
			private Int32 _ruletypeid;
	        public Int32 RuleTypeId
	        {
	            get{ return _ruletypeid; }
	            set{ _ruletypeid = value; }
	        }
            /// <summary>
			/// 规则类型标识
	        /// </summary>		
			private String _ruletypekey;
	        public String RuleTypeKey
	        {
	            get{ return _ruletypekey; }
	            set{ _ruletypekey = value; }
	        }
            /// <summary>
			/// 规则类型名称
	        /// </summary>		
			private String _ruletypename;
	        public String RuleTypeName
	        {
	            get{ return _ruletypename; }
	            set{ _ruletypename = value; }
	        }
            /// <summary>
			/// 数据源类型：0，实现IDataPermissionService的同一应用程序集下的类，1，WCF服务地址
	        /// </summary>		
			private Int32? _sourcetype;
	        public Int32? SourceType
	        {
	            get{ return _sourcetype; }
	            set{ _sourcetype = value; }
	        }
            /// <summary>
			/// 数据源类名或WCF地址
	        /// </summary>		
			private String _sourcename;
	        public String SourceName
	        {
	            get{ return _sourcename; }
	            set{ _sourcename = value; }
	        }
            /// <summary>
			/// 子系统标识
	        /// </summary>		
			private String _subsyskey;
	        public String SubSysKey
	        {
	            get{ return _subsyskey; }
	            set{ _subsyskey = value; }
	        }
            /// <summary>
			/// IsDelete
	        /// </summary>		
			private Boolean? _isdelete;
	        public Boolean? IsDelete
	        {
	            get{ return _isdelete; }
	            set{ _isdelete = value; }
	        }
            /// <summary>
			/// CreatBy
	        /// </summary>		
			private Int32? _creatby;
	        public Int32? CreatBy
	        {
	            get{ return _creatby; }
	            set{ _creatby = value; }
	        }
            /// <summary>
			/// CreateTime
	        /// </summary>		
			private System.DateTime? _createtime;
	        public System.DateTime? CreateTime
	        {
	            get{ return _createtime; }
	            set{ _createtime = value; }
	        }
            /// <summary>
			/// UpdateBy
	        /// </summary>		
			private Int32? _updateby;
	        public Int32? UpdateBy
	        {
	            get{ return _updateby; }
	            set{ _updateby = value; }
	        }
            /// <summary>
			/// UpdateTime
	        /// </summary>		
			private System.DateTime? _updatetime;
	        public System.DateTime? UpdateTime
	        {
	            get{ return _updatetime; }
	            set{ _updatetime = value; }
	        }
            	}
}
