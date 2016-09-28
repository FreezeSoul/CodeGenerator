using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace RFD.LMS.Model.Permission
{
	/// <summary>
    /// PMS_UserRulesMapping
    /// </summary>
	[Serializable]
	public partial class PMS_UserRulesMapping
	{
			/// <summary>
			/// Id
	        /// </summary>		
			private Int32 _id;
	        public Int32 Id
	        {
	            get{ return _id; }
	            set{ _id = value; }
	        }
            /// <summary>
			/// 用户编号
	        /// </summary>		
			private String _usercode;
	        public String UserCode
	        {
	            get{ return _usercode; }
	            set{ _usercode = value; }
	        }
            /// <summary>
			/// 规则ID
	        /// </summary>		
			private Int32? _ruleid;
	        public Int32? RuleId
	        {
	            get{ return _ruleid; }
	            set{ _ruleid = value; }
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
			/// CreateBy
	        /// </summary>		
			private Int32? _createby;
	        public Int32? CreateBy
	        {
	            get{ return _createby; }
	            set{ _createby = value; }
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
