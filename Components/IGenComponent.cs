using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

// SMO
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components
{
    /// <summary>
    /// 生成器类须实现这个接口以配合框架调用
    /// </summary>
    public interface IGenComponent
    {
        Dictionary<GenProperties, object> Properties { get; }

        /// <summary>
        /// 本类所针对的目标数据库对象的类型（可以是多种）
        /// </summary>
        SqlElementTypes TargetSqlElementType { get; }

        /// <summary>
        /// 用于设置 Gen 方法中可能会用到的服务器对象
        /// </summary>
        Server Server { set; }

        /// <summary>
        /// 用于设置 Gen 方法中可能会用到的数据库对象
        /// </summary>
        Database Database { set; }

        /// <summary>
        /// 生成代码. 参数类型及其个数应该和 TargetSqlElementType 指示的值相一致
        /// </summary>
        /// <returns>返回 类型:数据 这样的结构</returns>
        GenResult Gen(params object[] sqlElements);

        /// <summary>
        /// 返回当前针对的数据库对象是否具备执行 Gen 的资格. 参数类型及其个数应该和 TargetSqlElementType 指示的值相一致
        /// </summary>
        bool Validate(params object[] sqlElements);

        /// <summary>
        /// 中断生成操作用的委托
        /// </summary>
        event CancelEventHandler OnProcessing;
    }

    /// <summary>
    /// 生成器的参数列表
    /// </summary>
    public enum GenProperties
    {
        /// <summary>
        /// string : 名称标识
        /// </summary>
        Name,
        /// <summary>
        /// string : 菜单显示名
        /// </summary>
        Caption,
        /// <summary>
        /// string : 所属菜单组
        /// </summary>
        Group,
        /// <summary>
        /// string : 文本提示信息
        /// </summary>
        Tips,
		/// <summary>
		/// bool (True/False) : 是否启用当前插件. 默认认为 True
		/// </summary>
		IsEnabled
    }

    /// <summary>
    /// 生成物. 须根据 GenResultType 来填充 同名属性 的值
    /// </summary>
    public class GenResult
    {
        public GenResult(GenResultTypes rt) { _resultType = rt; }

        protected GenResultTypes _resultType;
        public GenResultTypes GenResultType { get { return _resultType; } }

        protected string _message;
        /// <summary>
        /// 获取或设置文本信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        protected KeyValuePair<string, string> _codeSegment;

        /// <summary>
        /// 获取或设置 title + code 
        /// </summary>
        public KeyValuePair<string, string> CodeSegment
        {
            get { return _codeSegment; }
            set { _codeSegment = value; }
        }
        protected List<KeyValuePair<string, string>> _codeSegments;
        /// <summary>
        /// 获取或设置 多组 title + code 
        /// </summary>
        public List<KeyValuePair<string, string>> CodeSegments
        {
            get { return _codeSegments; }
            set { _codeSegments = value; }
        }
        protected KeyValuePair<string, byte[]> _file;
        /// <summary>
        /// 获取或设置 filename + data
        /// </summary>
        public KeyValuePair<string, byte[]> File
        {
            get { return _file; }
            set { _file = value; }
        }
        protected List<KeyValuePair<string, byte[]>> _files;

        /// <summary>
        /// 获取或设置 多组 filename + data
        /// </summary>
        public List<KeyValuePair<string, byte[]>> Files
        {
            get { return _files; }
            set { _files = value; }
        }
    }

    /// <summary>
    /// 生成物类型, 这将会影响框架处理返回值的行为
    /// </summary>
    public enum GenResultTypes
    {
        /// <summary>
        /// 代码段: 对应一个 string
        /// </summary>
        CodeSegment,
        /// <summary>
        /// 多个代码段: 对应多组 KeyValuePair＜string 名称, string 内容＞[] 
        /// </summary>
        CodeSegments,
        /// <summary>
        /// 文件: 对应一个相对 Output 目录的路径 string, 以及文件内容 byte[]
        /// </summary>
        File,
        /// <summary>
        /// 多个文件: 对应多组 KeyValuePair＜string 文件路径, byte[] 文件内容＞[] 
        /// </summary>
        Files,
        /// <summary>
        /// 对应生成过程中被中断或生成错误的情况: string
        /// </summary>
        Message
    }

    /// <summary>
    /// 生成操作可以针对的 SQL 对象枚举
    /// </summary>
    [Flags]
    public enum SqlElementTypes : int
    {
        /// <summary>
        /// 单个库. 参数为 Server, Database
        /// </summary>
        Database = 1,
        /// <summary>
        /// 多个库. 参数为 Server, null
        /// </summary>
        Databases = 2,

        /// <summary>
        /// 单个表. 参数为 Server, Database, Table
        /// </summary>
        Table = 4,
        /// <summary>
        /// 多个表. 参数为 Server, Database
        /// </summary>
        Tables = 8,

        /// <summary>
        /// 单个视图. 参数为 Server, Database, View
        /// </summary>
        View = 16,
        /// <summary>
        /// 多个视图. 参数为 Server, Database
        /// </summary>
        Views = 32,

        /// <summary>
        /// 单个存储过程. 参数为 Server, Database, StoredProcedure
        /// </summary>
        StoredProcedure = 64,
        /// <summary>
        /// 多个存储过程. 参数为 Server, Database
        /// </summary>
        StoredProcedures = 128,

        /// <summary>
        /// 单个函数. 参数为 Server, Database, UserDefinedFunction
        /// </summary>
        UserDefinedFunction = 256,
        /// <summary>
        /// 多个函数. 参数为 Server, Database
        /// </summary>
        UserDefinedFunctions = 1024,

        /// <summary>
        /// 单个自定义表类型. 参数为 Server, Database, UserDefinedTableType
        /// </summary>
        UserDefinedTableType = 2048,
        /// <summary>
        /// 多个自定义表类型. 参数为 Server, Database
        /// </summary>
        UserDefinedTableTypes = 4096,

        /// <summary>
        /// 字段　参数为 Server, Database, Column
        /// </summary>
        Column = 8192,

        /// <summary>
        /// 扩展属性　参数为 Server, Database, ExtendProperty
        /// </summary>
        ExtendedProperty = 16384
    }
}
