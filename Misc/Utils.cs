using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using CodeGenerator.Misc;


namespace CodeGenerator
{
    /// <summary>
    /// 生成器工具类 提供各种生成代码的辅助方法
    /// </summary>
    public static class Utils
    {
        #region Const Strings

        public const string EP_DefaultDesc = "MS_Description";
        public const string EP_Settings = "CodeGenSettings_";
        public const string EP_ParmDesc = "CodeGen_ParmDesc_";
        public const string EP_Caption_ = "CodeGen_Caption_";
        public const string EP_Caption = "CodeGen_Caption";
        public const string EP_PrimaryKeys = "CodeGenSettings_PrimaryKeys";
        public const string EP_BaseTableName = "CodeGenSettings_BaseTable";
        public const string EP_BaseTableSchema = "CodeGenSettings_BaseTableSchema";
        public const string EP_MethodName = "CodeGenSettings_MethodName";
        public const string EP_BelongTo = "CodeGenSettings_BelongTo";
        public const string EP_Behavior = "CodeGenSettings_Behavior";
        public const string EP_IsSingleLineResult = "CodeGenSettings_IsSingleLineResult";
        public const string EP_ResultType = "CodeGenSettings_ResultType";
        public const string EP_ResultTypeSchema = "CodeGenSettings_ResultTypeSchema";
        public const string EP_ResultType_Int = "int";
        public const string EP_ResultType_DataSet = "DataSet";
        public const string EP_ResultType_DataTable = "DataTable";
        public const string EP_ResultType_UserDefinedTableType = "UserDefinedTableType";
        public const string EP_ResultType_Object = "Object";
        public const string EP_IsDisplay = "UserDefine_IsDisplay";

        #endregion

        #region Get/Set Description ( Extended Properties )

        /// <summary>
        /// 读数据库对象（库，表，视图，过程，函数，字段，参数）的自定义备注（ key = null 则为返回 MS_Description 的内容 ）
        /// </summary>
        public static string GetDescription(object o)
        {
            return GetDescription(o, null);
        }
        /// <summary>
        /// 读数据库对象（库，表，视图，过程，函数，字段，参数）的自定义备注（ key = null 则为返回 MS_Description 的内容 ）
        /// </summary>
        public static string GetDescription(object o, string key)
        {
            if (string.IsNullOrEmpty(key)) key = Utils.EP_DefaultDesc;

            ExtendedProperty ep = null;
            if (o.GetType() == typeof(Database)) ep = ((Database)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(Table)) ep = ((Table)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(View)) ep = ((View)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(UserDefinedTableType)) ep = ((UserDefinedTableType)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(UserDefinedFunction)) ep = ((UserDefinedFunction)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(StoredProcedure)) ep = ((StoredProcedure)o).ExtendedProperties[key];
            else if (o.GetType() == typeof(Column))
            {
                Column p = (Column)o;
                if (p.Parent.GetType() == typeof(Table))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        ep = p.ExtendedProperties[key];
                    }
                    else ep = ((Table)p.Parent).ExtendedProperties[key + p.Name];
                }
                else if (p.Parent.GetType() == typeof(UserDefinedTableType))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        ep = p.ExtendedProperties[key];
                    }
                    else ep = ((UserDefinedTableType)p.Parent).ExtendedProperties[key + p.Name];
                }
                else if (p.Parent.GetType() == typeof(View))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        ep = ((View)p.Parent).ExtendedProperties[Utils.EP_Settings + p.Name];
                    }
                    else ep = ((View)p.Parent).ExtendedProperties[key + p.Name];
                }
                else if (p.Parent.GetType() == typeof(UserDefinedFunction))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        ep = ((UserDefinedFunction)p.Parent).ExtendedProperties[Utils.EP_Settings + p.Name];
                    }
                    else ep = ((UserDefinedFunction)p.Parent).ExtendedProperties[key + p.Name];
                }
            }
            else if (o.GetType() == typeof(UserDefinedFunctionParameter))
            {
                UserDefinedFunctionParameter p = (UserDefinedFunctionParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    ep = ((UserDefinedFunction)p.Parent).ExtendedProperties[Utils.EP_ParmDesc + p.Name];
                }
                else ep = ((UserDefinedFunction)p.Parent).ExtendedProperties[key + p.Name];
            }
            else if (o.GetType() == typeof(StoredProcedureParameter))
            {
                StoredProcedureParameter p = (StoredProcedureParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    ep = ((StoredProcedure)p.Parent).ExtendedProperties[Utils.EP_ParmDesc + p.Name];
                }
                else ep = ((StoredProcedure)p.Parent).ExtendedProperties[key + p.Name];
            }
            if (ep != null)
            {
                // 分卷合并读取
                string s = ep.Value as string;
                if (s == null) return "";       // for sql2000
                if (s.Length == 3600) return s + GetDescription(o, key + "_");
                else return s;
            }
            return string.Empty;
        }

        /// <summary>
        /// 写数据库对象（库，表，视图，过程，函数，字段，参数）的备注
        /// </summary>
        public static void SetDescription(object o, string value)
        {
            SetDescription(o, null, value);
        }
        /// <summary>
        /// 写数据库对象（库，表，视图，过程，函数，字段，参数）的自定义备注（ key = null 则为设置 MS_Description 的内容 ）
        /// </summary>
        public static void SetDescription(object o, string key, string value)
        {
            if (string.IsNullOrEmpty(key)) key = Utils.EP_DefaultDesc;
            if (value == null) value = "";

            // 分卷存 ( MSSQL 扩展属性有 3600 个字左右的限制）
            if (value.Length > 3600)
            {
                // 存子串
                SetDescription(o, key + "_", value.Substring(3600));
                value = value.Substring(0, 3600);
            }
            else if (value.Length > 0)
            {
                // 删多余的分卷扩展属性 姑且认为最多有 100 个
                for (int i = 1; i < 100; i++)
                {
                    DeleteDescription(o, key + new string('_', i));
                }
            }

            if (o.GetType() == typeof(Database))
            {
                Database p = (Database)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(Table))
            {
                Table p = (Table)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(UserDefinedTableType))
            {
                UserDefinedTableType p = (UserDefinedTableType)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(View))
            {
                View p = (View)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(Column))
            {
                Column p = (Column)o;
                if (p.Parent.GetType() == typeof(Table)
                    || p.Parent.GetType() == typeof(UserDefinedTableType))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        if (p.ExtendedProperties.Contains(key))
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                p.ExtendedProperties[key].Drop();
                                p.Alter();
                            }
                            else if (p.ExtendedProperties[key].Value as string != value)
                            {
                                p.ExtendedProperties[key].Value = value;
                                p.Alter();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                                p.Alter();
                            }
                        }
                    }
                    else
                    {
                        SetDescription(p.Parent, key + p.Name, value);
                    }
                }
                else if (p.Parent.GetType() == typeof(View))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        SetDescription(p.Parent, Utils.EP_Settings + p.Name, value);
                    }
                    else SetDescription(p.Parent, key + p.Name, value);
                }
                else if (p.Parent.GetType() == typeof(UserDefinedFunction))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        SetDescription(p.Parent, Utils.EP_Settings + p.Name, value);
                    }
                    else SetDescription(p.Parent, key + p.Name, value);
                }
            }
            else if (o.GetType() == typeof(UserDefinedFunction))
            {
                UserDefinedFunction p = (UserDefinedFunction)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(UserDefinedFunctionParameter))
            {
                UserDefinedFunctionParameter p = (UserDefinedFunctionParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    SetDescription(p.Parent, Utils.EP_ParmDesc + p.Name, value);
                }
                else SetDescription(p.Parent, key + p.Name, value);
            }
            else if (o.GetType() == typeof(StoredProcedure))
            {
                StoredProcedure p = (StoredProcedure)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties[key].Drop();
                        p.Alter();
                    }
                    else if (p.ExtendedProperties[key].Value as string != value)
                    {
                        p.ExtendedProperties[key].Value = value;
                        p.Alter();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.ExtendedProperties.Add(new ExtendedProperty(p, key, value));
                        p.Alter();
                    }
                }
            }
            else if (o.GetType() == typeof(StoredProcedureParameter))
            {
                StoredProcedureParameter p = (StoredProcedureParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    SetDescription(p.Parent, Utils.EP_ParmDesc + p.Name, value);
                }
                else SetDescription(p.Parent, key + p.Name, value);
            }
        }


        /// <summary>
        /// 删数据库对象（库，表，视图，过程，函数，字段，参数）的自定义备注（ key = null 则为设置 MS_Description 的内容 ）
        /// </summary>
        public static void DeleteDescription(object o, string key)
        {
            if (string.IsNullOrEmpty(key)) key = Utils.EP_DefaultDesc;

            if (o.GetType() == typeof(Database))
            {
                Database p = (Database)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(Table))
            {
                Table p = (Table)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(UserDefinedTableType))
            {
                UserDefinedTableType p = (UserDefinedTableType)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(View))
            {
                View p = (View)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(Column))
            {
                Column p = (Column)o;
                if (p.Parent.GetType() == typeof(Table)
                    || p.Parent.GetType() == typeof(UserDefinedTableType))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        if (p.ExtendedProperties.Contains(key))
                        {
                            p.ExtendedProperties[key].Drop();
                            p.Alter();
                        }
                    }
                    else
                    {
                        DeleteDescription(p.Parent, key + p.Name);
                    }
                }
                else if (p.Parent.GetType() == typeof(View))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        DeleteDescription(p.Parent, Utils.EP_Settings + p.Name);
                    }
                    else DeleteDescription(p.Parent, key + p.Name);
                }
                else if (p.Parent.GetType() == typeof(UserDefinedFunction))
                {
                    if (key == Utils.EP_DefaultDesc)
                    {
                        DeleteDescription(p.Parent, Utils.EP_Settings + p.Name);
                    }
                    else DeleteDescription(p.Parent, key + p.Name);
                }
            }
            else if (o.GetType() == typeof(UserDefinedFunction))
            {
                UserDefinedFunction p = (UserDefinedFunction)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(UserDefinedFunctionParameter))
            {
                UserDefinedFunctionParameter p = (UserDefinedFunctionParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    DeleteDescription(p.Parent, Utils.EP_ParmDesc + p.Name);
                }
                else DeleteDescription(p.Parent, key + p.Name);
            }
            else if (o.GetType() == typeof(StoredProcedure))
            {
                StoredProcedure p = (StoredProcedure)o;
                if (p.ExtendedProperties.Contains(key))
                {
                    p.ExtendedProperties[key].Drop();
                    p.Alter();
                }
            }
            else if (o.GetType() == typeof(StoredProcedureParameter))
            {
                StoredProcedureParameter p = (StoredProcedureParameter)o;
                if (key == Utils.EP_DefaultDesc)
                {
                    DeleteDescription(p.Parent, Utils.EP_ParmDesc + p.Name);
                }
                else DeleteDescription(p.Parent, key + p.Name);
            }
        }


        #endregion


        #region Get/Set Caption

        /// <summary>
        /// 读字段（表，视图，表函数）的 Caption
        /// </summary>
        public static string GetCaption(Column c)
        {
            string s = GetDescription(c, Utils.EP_Caption_);
            if (string.IsNullOrEmpty(s)) return c.Name;
            return s;
        }


        /// <summary>
        /// 写字段（表，视图，表函数）的 Caption
        /// </summary>
        public static void SetCaption(Column c, string value)
        {
            if (c.Name == value) SetDescription(c, Utils.EP_Caption_, string.Empty);
            else SetDescription(c, Utils.EP_Caption_, value);
        }


        /// <summary>
        /// 读数据库对象的 Caption
        /// </summary>
        public static string GetCaption(object o)
        {
            string s = GetDescription(o, Utils.EP_Caption);
            if (string.IsNullOrEmpty(s))
            {
                if (o.GetType() == typeof(Table)) return ((Table)o).Name;
                else if (o.GetType() == typeof(View)) return ((View)o).Name;
                else if (o.GetType() == typeof(UserDefinedTableType)) return ((UserDefinedTableType)o).Name;
                else if (o.GetType() == typeof(UserDefinedFunction)) return ((UserDefinedFunction)o).Name;
                return o.ToString();
            }
            return s;
        }

        /// <summary>
        /// 写数据库对象的 Caption
        /// </summary>
        public static void SetCaption(object o, string value)
        {
            if (o.GetType() == typeof(Table) && value == ((Table)o).Name
                || o.GetType() == typeof(View) && value == ((View)o).Name
                || o.GetType() == typeof(UserDefinedTableType) && value == ((UserDefinedTableType)o).Name
                || o.GetType() == typeof(UserDefinedFunction) && value == ((UserDefinedFunction)o).Name
                || value == o.ToString()) SetDescription(o, Utils.EP_Caption, string.Empty);
            else SetDescription(o, Utils.EP_Caption, value);
        }

        #endregion


        #region Get/Set View's PrimaryKeyColumnNames

        /// <summary>
        /// 返回一个视图的手工指定的主键字段名列表
        /// </summary>
        public static List<string> GetPrimaryKeyColumnNames(View v)
        {
            string s = GetDescription(v, Utils.EP_PrimaryKeys);
            if (s.Length > 0)
            {
                return new List<string>(s.Split(','));
            }
            return new List<string>();
        }

        /// <summary>
        /// 设置一个视图的手工指定的主键字段名列表
        /// </summary>
        public static void SetPrimaryKeyColumnNames(View v, List<string> cns)
        {
            if (cns == null || cns.Count == 0) SetDescription(v, Utils.EP_PrimaryKeys, string.Empty);
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < cns.Count; i++)
                {
                    if (i > 0) sb.Append(@",");
                    sb.Append(cns[i]);
                }
                SetDescription(v, Utils.EP_PrimaryKeys, sb.ToString());
            }
        }


        /// <summary>
        /// 返回一个视图的手工指定的主键字段列表
        /// </summary>
        public static List<Column> GetPrimaryKeyColumns(View v)
        {
            string s = GetDescription(v, Utils.EP_PrimaryKeys);
            if (s.Length > 0)
            {
                List<string> ss = new List<string>(s.Split(','));
                List<Column> cs = new List<Column>();
                foreach (Column c in v.Columns) if (ss.Contains(c.Name)) cs.Add(c);
                return cs;
            }
            return new List<Column>();
        }

        /// <summary>
        /// 设置一个视图的手工指定的主键字段列表
        /// </summary>
        public static void SetPrimaryKeyColumns(View v, List<Column> cs)
        {
            if (cs == null || cs.Count == 0) SetDescription(v, Utils.EP_PrimaryKeys, string.Empty);
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < cs.Count; i++)
                {
                    if (i > 0) sb.Append(@",");
                    sb.Append(cs[i].Name);
                }
                SetDescription(v, Utils.EP_PrimaryKeys, sb.ToString());
            }
        }

        #endregion

        #region Get/Set View's Base Table

        /// <summary>
        /// 返回一个视图所指向的基表，没有返回空
        /// </summary>
        public static Table GetBaseTable(View v)
        {
            string n = Utils.GetDescription(v, Utils.EP_BaseTableName);
            if (string.IsNullOrEmpty(n)) return null;
            string s = Utils.GetDescription(v, Utils.EP_BaseTableSchema);
            if (string.IsNullOrEmpty(s)) return v.Parent.Tables[n];
            return v.Parent.Tables[n, s];
        }

        /// <summary>
        /// 返回一个视图所指向的基表名，没有返回 空
        /// </summary>
        public static string GetBaseTableName(View v)
        {
            return Utils.GetDescription(v, Utils.EP_BaseTableName);
        }
        /// <summary>
        /// 返回一个视图所指向的基表架构名，没有返回 空
        /// </summary>
        public static string GetBaseTableSchema(View v)
        {
            return Utils.GetDescription(v, Utils.EP_BaseTableSchema);
        }

        /// <summary>
        /// 设置一个视图的基表
        /// </summary>
        public static void SetBaseTable(View v, string tn, string schema)
        {
            if (string.IsNullOrEmpty(tn) || tn == "None")
            {
                Utils.SetDescription(v, Utils.EP_BaseTableName, string.Empty);
                Utils.SetDescription(v, Utils.EP_BaseTableSchema, string.Empty);
            }
            else
            {
                Utils.SetDescription(v, Utils.EP_BaseTableName, tn);
                Utils.SetDescription(v, Utils.EP_BaseTableSchema, schema);
            }
        }

        #endregion


        #region Get/Set SP/Func's MethodName

        /// <summary>
        /// 取存储过程的方法名
        /// </summary>
        public static string GetMethodName(StoredProcedure sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_MethodName);
            if (string.IsNullOrEmpty(s)) return GetEscapeName(sp.Name);
            return s;
        }

        /// <summary>
        /// 设存储过程的方法名
        /// </summary>
        public static void SetMethodName(StoredProcedure sp, string value)
        {
            if (string.IsNullOrEmpty(value) || value == GetEscapeName(sp.Name)) Utils.SetDescription(sp, Utils.EP_MethodName, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_MethodName, value);
        }


        /// <summary>
        /// 取函数的方法名
        /// </summary>
        public static string GetMethodName(UserDefinedFunction sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_MethodName);
            if (string.IsNullOrEmpty(s)) return GetEscapeName(sp.Name);
            return s;
        }

        /// <summary>
        /// 设函数的方法名
        /// </summary>
        public static void SetMethodName(UserDefinedFunction sp, string value)
        {
            if (string.IsNullOrEmpty(value) || value == GetEscapeName(sp.Name)) Utils.SetDescription(sp, Utils.EP_MethodName, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_MethodName, value);
        }

        #endregion

        #region Get/Set SP/Func's BelongTo

        /// <summary>
        /// 取存储过程的归属表
        /// </summary>
        public static string GetBelongTo(StoredProcedure sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_BelongTo);
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s;
        }

        /// <summary>
        /// 设存储过程的归属表
        /// </summary>
        public static void SetBelongTo(StoredProcedure sp, string value)
        {
            Utils.SetDescription(sp, Utils.EP_BelongTo, value);
        }


        /// <summary>
        /// 取函数的归属表
        /// </summary>
        public static string GetBelongTo(UserDefinedFunction sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_BelongTo);
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s;
        }

        /// <summary>
        /// 设函数的归属表
        /// </summary>
        public static void SetBelongTo(UserDefinedFunction sp, string value)
        {
            Utils.SetDescription(sp, Utils.EP_BelongTo, value);
        }

        #endregion

        #region Get/Set SP/Func's Behavior

        /// <summary>
        /// 取存储过程的数据操作行为
        /// </summary>
        public static string GetBehavior(StoredProcedure sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_Behavior);
            if (string.IsNullOrEmpty(s)) return "None";
            return s;
        }

        /// <summary>
        /// 设存储过程的数据操作行为
        /// </summary>
        public static void SetBehavior(StoredProcedure sp, string value)
        {
            if (string.IsNullOrEmpty(value) || value == "None") Utils.SetDescription(sp, Utils.EP_Behavior, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_Behavior, value);
        }


        /// <summary>
        /// 取函数的数据操作行为
        /// </summary>
        public static string GetBehavior(UserDefinedFunction sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_Behavior);
            if (string.IsNullOrEmpty(s)) return "None";
            return s;
        }

        /// <summary>
        /// 设函数的数据操作行为
        /// </summary>
        public static void SetBehavior(UserDefinedFunction sp, string value)
        {
            if (string.IsNullOrEmpty(value) || value == "None") Utils.SetDescription(sp, Utils.EP_Behavior, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_Behavior, value);
        }

        #endregion

        #region Get/Set SP/Func's ResultType

        /// <summary>
        /// 取存储过程的返回结果类型
        /// </summary>
        public static string GetResultType(StoredProcedure sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_ResultType);
            if (string.IsNullOrEmpty(s)) return Utils.EP_ResultType_Int;
            return s;
        }

        /// <summary>
        /// 设存储过程的返回结果类型
        /// </summary>
        public static void SetResultType(StoredProcedure sp, string value)
        {
            if (value == Utils.EP_ResultType_Int) Utils.SetDescription(sp, Utils.EP_ResultType, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_ResultType, value);
        }


        /// <summary>
        /// 取函数的返回结果类型
        /// </summary>
        public static string GetResultType(UserDefinedFunction sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_ResultType);
            if (string.IsNullOrEmpty(s)) return Utils.EP_ResultType_Int;
            return s;
        }

        /// <summary>
        /// 设函数的返回结果类型
        /// </summary>
        public static void SetResultType(UserDefinedFunction sp, string value)
        {
            if (value == Utils.EP_ResultType_Int) Utils.SetDescription(sp, Utils.EP_ResultType, string.Empty);
            else Utils.SetDescription(sp, Utils.EP_ResultType, value);
        }



        /// <summary>
        /// 取存储过程的返回结果类型所属Schema
        /// </summary>
        public static string GetResultTypeSchema(StoredProcedure sp)
        {
            return Utils.GetDescription(sp, Utils.EP_ResultTypeSchema);
        }

        /// <summary>
        /// 设存储过程的返回结果类型所属Schema
        /// </summary>
        public static void SetResultTypeSchema(StoredProcedure sp, string value)
        {
            Utils.SetDescription(sp, Utils.EP_ResultTypeSchema, value);
        }


        /// <summary>
        /// 取函数的返回结果类型所属Schema
        /// </summary>
        public static string GetResultTypeSchema(UserDefinedFunction sp)
        {
            return Utils.GetDescription(sp, Utils.EP_ResultTypeSchema);
        }

        /// <summary>
        /// 设函数的返回结果类型所属Schema
        /// </summary>
        public static void SetResultTypeSchema(UserDefinedFunction sp, string value)
        {
            Utils.SetDescription(sp, Utils.EP_ResultTypeSchema, value);
        }

        #endregion

        #region Get/Set SP/Func's IsSingleLineResult

        /// <summary>
        /// 取存储过程的返回结果类型是否为单行
        /// </summary>
        public static bool GetIsSingleLineResult(StoredProcedure sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_IsSingleLineResult);
            if (string.IsNullOrEmpty(s)) return false;
            return bool.Parse(s);
        }

        /// <summary>
        /// 设存储过程的返回结果类型是否为单行
        /// </summary>
        public static void SetIsSingleLineResult(StoredProcedure sp, bool value)
        {
            if (value) Utils.SetDescription(sp, Utils.EP_IsSingleLineResult, value.ToString());
            else Utils.SetDescription(sp, Utils.EP_IsSingleLineResult, string.Empty);
        }


        /// <summary>
        /// 取函数的返回结果类型是否为单行
        /// </summary>
        public static bool GetIsSingleLineResult(UserDefinedFunction sp)
        {
            string s = Utils.GetDescription(sp, Utils.EP_IsSingleLineResult);
            if (string.IsNullOrEmpty(s)) return false;
            return bool.Parse(s);
        }

        /// <summary>
        /// 设函数的返回结果类型是否为单行
        /// </summary>
        public static void SetIsSingleLineResult(UserDefinedFunction sp, bool value)
        {
            if (value) Utils.SetDescription(sp, Utils.EP_IsSingleLineResult, value.ToString());
            else Utils.SetDescription(sp, Utils.EP_IsSingleLineResult, string.Empty);
        }

        #endregion

        #region GetDatabase

        /// <summary>
        /// 返回数据对象所在的 Database 引用
        /// </summary>
        public static Database GetDatabase(object o)
        {
            if (o.GetType() == typeof(Database)) return (Database)o;
            else if (o.GetType() == typeof(Column))
            {
                Column p = (Column)o;
                if (p.Parent.GetType() == typeof(Table))
                {
                    return ((Table)p.Parent).Parent;
                }
                else if (p.Parent.GetType() == typeof(UserDefinedTableType))
                {
                    return ((UserDefinedTableType)p.Parent).Parent;
                }
                else if (p.Parent.GetType() == typeof(View))
                {
                    return ((View)p.Parent).Parent;
                }
                else if (p.Parent.GetType() == typeof(UserDefinedFunction))
                {
                    return ((UserDefinedFunction)p.Parent).Parent;
                }
            }
            else if (o.GetType() == typeof(StoredProcedureParameter)) return ((StoredProcedureParameter)o).Parent.Parent;
            else if (o.GetType() == typeof(UserDefinedFunctionParameter)) return ((UserDefinedFunctionParameter)o).Parent.Parent;
            else if (o.GetType() == typeof(UserDefinedFunction)) return ((UserDefinedFunction)o).Parent;

            throw new Exception("未处理的数据类型");
        }

        #endregion

        #region GetSqlDataType

        /// <summary>
        /// 根据 systemtype 等字串形式的数据类型描述，返回相应的 SqlDataType 枚举
        /// </summary>
        /// <param name="s">数据类型名</param>
        /// <param name="maxLength">用于判断是否返回 nvarcharMax, varbinaryMax, varcharMax</param>
        public static SqlDataType GetSqlDataType(string s, int maxLength)
        {
            switch (s)
            {
                case "bigint":
                    return SqlDataType.BigInt;
                case "decimal":
                    return SqlDataType.Decimal;
                case "int":
                    return SqlDataType.Int;
                case "numeric":
                    return SqlDataType.Decimal;
                case "smallint":
                    return SqlDataType.SmallInt;
                case "money":
                    return SqlDataType.Money;
                case "tinyint":
                    return SqlDataType.TinyInt;
                case "smallmoney":
                    return SqlDataType.SmallMoney;
                case "bit":
                    return SqlDataType.Bit;
                case "float":
                    return SqlDataType.Float;
                case "real":
                    return SqlDataType.Real;
                case "datetime":
                    return SqlDataType.DateTime;
                case "smalldatetime":
                    return SqlDataType.SmallDateTime;
                case "char":
                    return SqlDataType.Char;
                case "text":
                    return SqlDataType.Text;
                case "varchar":
                    if (maxLength == -1) return SqlDataType.VarCharMax;
                    return SqlDataType.VarChar;
                case "nchar":
                    return SqlDataType.NChar;
                case "ntext":
                    return SqlDataType.NText;
                case "nvarchar":
                    if (maxLength == -1) return SqlDataType.NVarCharMax;
                    return SqlDataType.NVarChar;
                case "binary":
                    return SqlDataType.Binary;
                case "image":
                    return SqlDataType.Image;
                case "varbinary":
                    if (maxLength == -1) return SqlDataType.VarBinaryMax;
                    return SqlDataType.VarBinary;
                case "uniqueidentifier":
                    return SqlDataType.UniqueIdentifier;
                case "timestamp":
                    return SqlDataType.Timestamp;


                // todo: 这些数据类型名不一定对

                case "sql_variant":
                    return SqlDataType.Variant;

                case "userdefineddatatype":
                    return SqlDataType.UserDefinedDataType;
                case "userdefinedtype":
                    return SqlDataType.UserDefinedType;
                case "userdefinedtabletype":
                    return SqlDataType.UserDefinedTableType;

                case "datetime2":
                    return SqlDataType.DateTime2;
                case "datetimeoffset":
                    return SqlDataType.DateTimeOffset;
                case "date":
                    return SqlDataType.Date;
                case "time":
                    return SqlDataType.Time;

                case "xml":
                    return SqlDataType.Xml;

            }
            throw new Exception("未处理的数据类型:" + s.ToString());
        }

        #endregion

        #region GetSummary

        /// <summary>
        /// 返回为 代码段的 summary 部分而格式化的备注输出格式。每一行的前面带三个斜杠
        /// todo: 内容转义
        /// </summary>
        public static string GetSummary(object o, int numTabs)
        {
            return GetSummary(o, string.Empty, numTabs);
        }

        /// <summary>
        /// 用来保存一串 Tab 符
        /// </summary>
        public const string Tabs = "																																																								";

        /// <summary>
        /// 返回为 代码段的 summary 部分而格式化的备注输出格式。每一行的前面带三个斜杠。内容最后面附加一些字串，前面可空 numTabs 个 Tab 符
        /// todo: 内容转义
        /// </summary>
        public static string GetSummary(object o, string attach, int numTabs)
        {
            string str = GetDescription(o) + attach;
            string tabs = Tabs.Substring(0, numTabs);
            if (string.IsNullOrEmpty(str))
            {
                return @"
" + tabs + @"/// <summary>
" + tabs + @"/// 
" + tabs + @"/// </summary>";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
" + tabs + @"/// <summary>");
            using (TextReader tr = new StringReader(str))
            {
                while (true)
                {
                    string s = tr.ReadLine();
                    if (s == null) break;
                    if (s.Contains("--"))
                    {
                        if (s.StartsWith("-- ============================")) continue;
                    }
                    sb.Append(@"
" + tabs + @"/// " + s);
                }
            }
            sb.Append(@"
" + tabs + @"/// </summary>");
            return sb.ToString();
        }

        #endregion

        #region GetDataType

        /// <summary>
        /// 根据用户自定义数据类型返回一般类型
        /// </summary>
        public static DataType GetDataType(UserDefinedDataType udt)
        {
            SqlDataType sdt = GetSqlDataType(udt.SystemType, udt.MaxLength);
            DataType t = new DataType(sdt);
            t.MaximumLength = udt.MaxLength;
            t.NumericPrecision = udt.NumericPrecision;
            t.NumericScale = udt.NumericScale;
            // udt.Nullable
            return t;
        }


        /// <summary>
        /// 返回一个字段数据类型所对应的 C# 对象类型
        /// </summary>
        public static string GetDataType(Column c)
        {
            return GetDataType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 返回一个过程参数数据类型所对应的 C# 对象类型
        /// </summary>
        public static string GetDataType(StoredProcedureParameter p)
        {
            return GetDataType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个函数参数数据类型所对应的 C# 对象类型
        /// </summary>
        public static string GetDataType(UserDefinedFunctionParameter p)
        {
            return GetDataType(GetDatabase(p), p.DataType);
        }
        public static string GetDataType(UserDefinedFunction f)
        {
            return GetDataType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 返回一个SQL数据类型所对应的 C# 对象类型
        /// </summary>
        public static string GetDataType(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.Bit:
                    return "bool";
                case SqlDataType.TinyInt:
                    return "byte";
                case SqlDataType.SmallInt:
                    return "short";
                case SqlDataType.Int:
                    return "int";
                case SqlDataType.BigInt:
                    return "System.Int64";
                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return "decimal";

                case SqlDataType.Float:
                    return "double";

                case SqlDataType.Real:
                    return "float";
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Time:
                case SqlDataType.Date:
                    return "System.DateTime";
                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return "string";
                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Timestamp:
                    return "byte[]";
                case SqlDataType.UniqueIdentifier:
                    return "System.Guid";

                case SqlDataType.UserDefinedDataType:
                    return GetDataType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:

                default:
                    return "object";
            }
        }


        #endregion

        #region GetDataReaderMethod

        /// <summary>
        /// 根据数据类型返回在 DataReader 的字段读取中所用的方法名
        /// </summary>
        public static string GetDataReaderMethod(Column c)
        {
            return GetDataReaderMethod(GetDatabase(c), c.DataType);
        }
        public static string GetDataReaderMethod(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.Bit:
                    return "GetBoolean";
                case SqlDataType.TinyInt:
                    return "GetByte";
                case SqlDataType.SmallInt:
                    return "GetInt16";
                case SqlDataType.Int:
                    return "GetInt32";
                case SqlDataType.BigInt:
                    return "GetInt64";
                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return "GetDecimal";
                case SqlDataType.Float:
                    return "GetDouble";
                case SqlDataType.Real:
                    return "GetFloat";
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Date:
                case SqlDataType.Time:
                    return "GetDateTime";
                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return "GetString";

                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Timestamp:
                    return "GetSqlBinary";				// GetSqlBinary 方法还须在后加 .Value

                case SqlDataType.UniqueIdentifier:
                    return "GetGuid";

                case SqlDataType.UserDefinedDataType:
                    return GetDataReaderMethod(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:

                // todo: hierachyid, geography, ...

                default:
                    return "GetValue";

            }
        }


        #endregion

        #region GetNullableDataType

        /// <summary>
        /// 返回一个字段数据类型所对应的 C# 对象类型（可空类型）
        /// </summary>
        public static string GetNullableDataType(Column c)
        {
            return GetNullableDataType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 返回一个标值函数返回值类型所对应的 C# 对象类型（可空类型）
        /// </summary>
        public static string GetNullableDataType(UserDefinedFunction f)
        {
            return GetNullableDataType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 返回一个函数参数数据类型所对应的 C# 对象类型（可空类型）
        /// </summary>
        public static string GetNullableDataType(UserDefinedFunctionParameter p)
        {
            return GetNullableDataType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个过程参数数据类型所对应的 C# 对象类型（可空类型）
        /// </summary>
        public static string GetNullableDataType(StoredProcedureParameter p)
        {
            return GetNullableDataType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个字段数据类型所对应的 C# 对象类型（可空类型）
        /// </summary>
        public static string GetNullableDataType(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.Bit:
                    return "bool?";
                case SqlDataType.TinyInt:
                    return "byte?";
                case SqlDataType.SmallInt:
                    return "short?";
                case SqlDataType.Int:
                    return "int?";
                case SqlDataType.BigInt:
                    return "Int64?";
                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return "decimal?";
                case SqlDataType.Float:
                    return "double?";
                case SqlDataType.Real:
                    return "float?";
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Date:
                case SqlDataType.Time:
                    return "System.DateTime?";
                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return "string";
                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Timestamp:
                    return "byte[]";
                case SqlDataType.UniqueIdentifier:
                    return "System.Guid?";

                case SqlDataType.UserDefinedDataType:
                    return GetNullableDataType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:

                // todo: hierachyid, geography, ...

                default:
                    return "object";
            }
        }

        #endregion

        #region GetObjectDataSourceParameterDataType


        /// <summary>
        /// 返回一个字段数据类型所对应的 C# ObjectDataSource 对象参数的数据类型
        /// </summary>
        public static string GetObjectDataSourceParameterDataType(Column c)
        {
            return GetObjectDataSourceParameterDataType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 返回一个标值函数返回值类型所对应的 C# ObjectDataSource 对象参数的数据类型
        /// </summary>
        public static string GetObjectDataSourceParameterDataType(UserDefinedFunction f)
        {
            return GetObjectDataSourceParameterDataType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 返回一个函数参数数据类型所对应的 C# ObjectDataSource 对象参数的数据类型
        /// </summary>
        public static string GetObjectDataSourceParameterDataType(UserDefinedFunctionParameter p)
        {
            return GetObjectDataSourceParameterDataType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个过程参数数据类型所对应的 C# ObjectDataSource 对象参数的数据类型
        /// </summary>
        public static string GetObjectDataSourceParameterDataType(StoredProcedureParameter p)
        {
            return GetObjectDataSourceParameterDataType(GetDatabase(p), p.DataType);
        }

        public static string GetObjectDataSourceParameterDataType(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.SmallInt:
                    return "short";
                case SqlDataType.Int:
                    return "int";
                case SqlDataType.BigInt:
                    return "System.Int64";
                case SqlDataType.Decimal:
                case SqlDataType.Numeric:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return "decimal";
                case SqlDataType.TinyInt:
                    return "byte";
                case SqlDataType.Float:
                    return "double";
                case SqlDataType.Real:
                    return "float";
                case SqlDataType.Bit:
                    return "bool";
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Date:
                case SqlDataType.Time:
                    return "System.DateTime";
                case SqlDataType.UserDefinedTableType:
                    return "System.Data.DataTable";
                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Timestamp:
                    return "byte[]";
                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.UniqueIdentifier:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return "string";

                case SqlDataType.UserDefinedDataType:
                    return GetObjectDataSourceParameterDataType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:

                // todo: hierachyid, geography, ...

                default:
                    return "object";
            }
        }



        /// <summary>
        /// 根据字段数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetObjectDataSourceParameterDefaultValue(Column c)
        {
            return GetObjectDataSourceParameterDefaultValue(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 根据过程参数数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetObjectDataSourceParameterDefaultValue(StoredProcedureParameter p)
        {
            return GetObjectDataSourceParameterDefaultValue(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 根据函数参数数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetObjectDataSourceParameterDefaultValue(UserDefinedFunctionParameter p)
        {
            return GetObjectDataSourceParameterDefaultValue(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 根据数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetObjectDataSourceParameterDefaultValue(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.BigInt:
                case SqlDataType.Decimal:
                case SqlDataType.Int:
                case SqlDataType.Numeric:
                case SqlDataType.SmallInt:
                case SqlDataType.Money:
                case SqlDataType.TinyInt:
                case SqlDataType.SmallMoney:
                case SqlDataType.Float:
                case SqlDataType.Real:
                    return "0";
                case SqlDataType.Bit:
                    return "false";
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                    return "1900-1-1";
                case SqlDataType.UserDefinedTableType:
                    return "null";
                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.Binary:
                case SqlDataType.UniqueIdentifier:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:


                case SqlDataType.UserDefinedDataType:
                    return GetObjectDataSourceParameterDefaultValue(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:

                // todo: hierachyid, geography, ...

                default:
                    return string.Empty;
            }
        }

        #endregion

        #region GetSqlDbType

        /// <summary>
        /// 返回一个字段数据类型所对应的 DbType
        /// </summary>
        public static string GetSqlDbType(Column c)
        {
            return GetSqlDbType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 返回一个过程参数数据类型所对应的 DbType
        /// </summary>
        public static string GetSqlDbType(StoredProcedureParameter p)
        {
            return GetSqlDbType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个函数参数数据类型所对应的 SqlDbType
        /// </summary>
        public static string GetSqlDbType(UserDefinedFunctionParameter p)
        {
            return GetSqlDbType(GetDatabase(p), p.DataType);
        }
        public static string GetSqlDbType(Database db, DataType dt)
        {

            switch (dt.SqlDataType)
            {
                case SqlDataType.BigInt:
                    return "System.Data.SqlDbType.BigInt";
                case SqlDataType.Decimal:
                    return "System.Data.SqlDbType.Decimal";
                case SqlDataType.Int:
                    return "System.Data.SqlDbType.Int";
                case SqlDataType.Numeric:
                    return "System.Data.SqlDbType.Decimal";
                case SqlDataType.SmallInt:
                    return "System.Data.SqlDbType.SmallInt";
                case SqlDataType.Money:
                    return "System.Data.SqlDbType.Money";
                case SqlDataType.TinyInt:
                    return "System.Data.SqlDbType.TinyInt";
                case SqlDataType.SmallMoney:
                    return "System.Data.SqlDbType.SmallMoney";
                case SqlDataType.Bit:
                    return "System.Data.SqlDbType.Bit";
                case SqlDataType.Float:
                    return "System.Data.SqlDbType.Float";
                case SqlDataType.Real:
                    return "System.Data.SqlDbType.Real";
                case SqlDataType.DateTime:
                    return "System.Data.SqlDbType.DateTime";
                case SqlDataType.SmallDateTime:
                    return "System.Data.SqlDbType.SmallDateTime";
                case SqlDataType.Char:
                    return "System.Data.SqlDbType.Char";
                case SqlDataType.Text:
                    return "System.Data.SqlDbType.Text";
                case SqlDataType.VarChar:
                case SqlDataType.VarCharMax:
                    return "System.Data.SqlDbType.VarChar";
                case SqlDataType.NChar:
                    return "System.Data.SqlDbType.NChar";
                case SqlDataType.NText:
                    return "System.Data.SqlDbType.NText";
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                    return "System.Data.SqlDbType.NVarChar";
                case SqlDataType.Binary:
                    return "System.Data.SqlDbType.Binary";
                case SqlDataType.Image:
                    return "System.Data.SqlDbType.Image";
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                    return "System.Data.SqlDbType.VarBinary";
                case SqlDataType.UniqueIdentifier:
                    return "System.Data.SqlDbType.UniqueIdentifier";

                case SqlDataType.UserDefinedDataType:
                    return GetSqlDbType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.UserDefinedType:
                    return "System.Data.SqlDbType.Udt";

                case SqlDataType.UserDefinedTableType:
                    return "System.Data.SqlDbType.Structured";

                case SqlDataType.DateTime2:
                    return "System.Data.SqlDbType.DateTime2";
                case SqlDataType.DateTimeOffset:
                    return "System.Data.SqlDbType.DateTimeOffset";
                case SqlDataType.Date:
                    return "System.Data.SqlDbType.Date";
                case SqlDataType.Time:
                    return "System.Data.SqlDbType.Time";

                case SqlDataType.Xml:
                    return "System.Data.SqlDbType.Xml";

                case SqlDataType.Timestamp:
                    return "System.Data.SqlDbType.Timestamp";

                default:
                    return "System.Data.SqlDbType.Variant";
            }
        }

        #endregion



        #region GetDbTypeLength

        /// <summary>
        /// 返回一个字段数据类型所对应的 SqlDb 对象类型的声明长度
        /// </summary>
        public static string GetDbTypeLength(Column c)
        {
            return GetDbTypeLength(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 返回一个过程参数数据类型所对应的 SqlDb 对象类型的声明长度
        /// </summary>
        public static string GetDbTypeLength(StoredProcedureParameter p)
        {
            return GetDbTypeLength(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个函数参数数据类型所对应的 SqlDb 对象类型的声明长度
        /// </summary>
        public static string GetDbTypeLength(UserDefinedFunctionParameter p)
        {
            return GetDbTypeLength(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 返回一个SQL数据类型所对应的 SqlDb 对象类型的声明长度
        /// </summary>
        public static string GetDbTypeLength(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.UserDefinedDataType:
                    return GetDbTypeLength(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
                case SqlDataType.Decimal:
                case SqlDataType.Char:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NVarChar:
                case SqlDataType.Timestamp:
                    return dt.MaximumLength.ToString();
                case SqlDataType.Text:
                case SqlDataType.NText:
                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Xml:
                    return int.MaxValue.ToString();
                case SqlDataType.UniqueIdentifier:
                case SqlDataType.BigInt:
                case SqlDataType.Int:
                case SqlDataType.Numeric:
                case SqlDataType.SmallInt:
                case SqlDataType.Money:
                case SqlDataType.TinyInt:
                case SqlDataType.SmallMoney:
                case SqlDataType.Bit:
                case SqlDataType.Float:
                case SqlDataType.Real:
                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Geography:
                case SqlDataType.Geometry:
                case SqlDataType.HierarchyId:
                case SqlDataType.UserDefinedType:

                default:
                    return "0";
            }
        }

        #endregion

        #region GetDefaultValue

        /// <summary>
        /// 根据字段数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetDefaultValue(Column c)
        {
            return GetDefaultValue(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 根据过程参数数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetDefaultValue(StoredProcedureParameter p)
        {
            return GetDefaultValue(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 根据函数参数数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetDefaultValue(UserDefinedFunctionParameter p)
        {
            return GetDefaultValue(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 根据数据类型返回 C# 默认值代码段
        /// </summary>
        public static string GetDefaultValue(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.UserDefinedDataType:
                    return GetDefaultValue(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));

                case SqlDataType.Bit:
                    return "false";

                case SqlDataType.BigInt:
                case SqlDataType.Decimal:
                case SqlDataType.Int:
                case SqlDataType.Numeric:
                case SqlDataType.SmallInt:
                case SqlDataType.Money:
                case SqlDataType.TinyInt:
                case SqlDataType.SmallMoney:
                case SqlDataType.Float:
                case SqlDataType.Real:
                    return "0";

                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                    return "System.DateTime.Now";

                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                case SqlDataType.Timestamp:
                    return "new byte[] { }";

                case SqlDataType.UniqueIdentifier:
                    return "System.Guid.Empty";

                case SqlDataType.Geography:
                case SqlDataType.Geometry:
                case SqlDataType.UserDefinedTableType:
                case SqlDataType.HierarchyId:
                    return "null";

                case SqlDataType.Char:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                default:
                    return "\"\"";
            }
        }

        #endregion

        #region Get SP's ParmDeclareStr

        /// <summary>
        /// 根据字段数据类型取过程/函数参数声明字串
        /// </summary>
        public static string GetParmDeclareStr(Column c)
        {
            switch (c.DataType.SqlDataType)
            {
                case SqlDataType.Int:
                case SqlDataType.BigInt:
                case SqlDataType.Numeric:
                case SqlDataType.SmallInt:
                case SqlDataType.Money:
                case SqlDataType.TinyInt:
                case SqlDataType.SmallMoney:
                case SqlDataType.Bit:
                case SqlDataType.Float:
                case SqlDataType.Real:
                case SqlDataType.Text:
                case SqlDataType.NText:
                case SqlDataType.Image:
                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.Timestamp:
                case SqlDataType.UniqueIdentifier:
                case SqlDataType.UserDefinedTableType:
                case SqlDataType.UserDefinedDataType:
                case SqlDataType.UserDefinedType:
                case SqlDataType.Geography:
                case SqlDataType.Geometry:
                case SqlDataType.HierarchyId:
                case SqlDataType.Xml:
                case SqlDataType.Variant:
                    return c.DataType.Name.ToUpper();

                case SqlDataType.Decimal:
                    return c.DataType.Name.ToUpper() + " (" + c.DataType.NumericPrecision.ToString() + "," + c.DataType.NumericScale.ToString() + ")";

                default:
                    return c.DataType.Name.ToUpper() + "(" + (c.DataType.MaximumLength == -1 ? "MAX" : c.DataType.MaximumLength.ToString()) + ")";
            }
        }

        #endregion


        #region Check Type

        /// <summary>
        /// 判断是否为字串或日期类型（拼字串时需要加引号）
        /// </summary>
        public static bool CheckNeedQuote(Column c)
        {
            return Utils.CheckIsStringType(c) || Utils.CheckIsDateTimeType(c);
        }

        /// <summary>
        /// 判断并返回字段数据类型是否为“值类型”
        /// </summary>
        public static bool CheckIsValueType(Column c)
        {
            return CheckIsValueType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断并返回字段数据类型是否为“值类型”
        /// </summary>
        public static bool CheckIsValueType(Database db, DataType dt)
        {
            switch (dt.SqlDataType)
            {
                case SqlDataType.UserDefinedDataType:
                    return CheckIsValueType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
                case SqlDataType.BigInt:
                case SqlDataType.Decimal:
                case SqlDataType.Int:
                case SqlDataType.Numeric:
                case SqlDataType.SmallInt:
                case SqlDataType.Money:
                case SqlDataType.TinyInt:
                case SqlDataType.SmallMoney:
                case SqlDataType.Bit:
                case SqlDataType.Float:
                case SqlDataType.Real:
                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.DateTime2:
                case SqlDataType.DateTimeOffset:
                case SqlDataType.UniqueIdentifier:
                    return true;

                default:
                    return false;
            }
        }


        /// <summary>
        /// 判断一个字段的数据类型是否为 “字串类”
        /// </summary>
        public static bool CheckIsStringType(Column c)
        {
            return CheckIsStringType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “字串类”
        /// </summary>
        public static bool CheckIsStringType(UserDefinedFunctionParameter p)
        {
            return CheckIsStringType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “字串类”
        /// </summary>
        public static bool CheckIsStringType(StoredProcedureParameter p)
        {
            return CheckIsStringType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “字串类”
        /// </summary>
        public static bool CheckIsStringType(UserDefinedFunction f)
        {
            return CheckIsStringType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “字串类”
        /// </summary>
        public static bool CheckIsStringType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsStringType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.Char
                    || sdt == SqlDataType.Text
                    || sdt == SqlDataType.VarChar
                    || sdt == SqlDataType.NChar
                    || sdt == SqlDataType.NText
                    || sdt == SqlDataType.NVarChar
                    || sdt == SqlDataType.NVarCharMax
                    || sdt == SqlDataType.VarCharMax
                    || sdt == SqlDataType.Xml);

            // todo: sql08 hierachyid
        }



        /// <summary>
        /// 判断一个字段的数据类型是否为 “日期类”
        /// </summary>
        public static bool CheckIsDateTimeType(Column c)
        {
            return CheckIsDateTimeType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “日期类”
        /// </summary>
        public static bool CheckIsDateTimeType(UserDefinedFunctionParameter p)
        {
            return CheckIsDateTimeType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “日期类”
        /// </summary>
        public static bool CheckIsDateTimeType(StoredProcedureParameter p)
        {
            return CheckIsDateTimeType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “日期类”
        /// </summary>
        public static bool CheckIsDateTimeType(UserDefinedFunction f)
        {
            return CheckIsDateTimeType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “日期类”
        /// </summary>
        public static bool CheckIsDateTimeType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsDateTimeType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.DateTimeOffset
                    || sdt == SqlDataType.DateTime2
                    || sdt == SqlDataType.DateTime
                    || sdt == SqlDataType.Date
                    || sdt == SqlDataType.Time);
        }



        /// <summary>
        /// 判断一个字段的数据类型是否为 “Guid 类”
        /// </summary>
        public static bool CheckIsGuidType(Column c)
        {
            return CheckIsGuidType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “Guid 类”
        /// </summary>
        public static bool CheckIsGuidType(UserDefinedFunctionParameter p)
        {
            return CheckIsGuidType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “Guid 类”
        /// </summary>
        public static bool CheckIsGuidType(StoredProcedureParameter p)
        {
            return CheckIsGuidType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “Guid 类”
        /// </summary>
        public static bool CheckIsGuidType(UserDefinedFunction f)
        {
            return CheckIsGuidType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “Guid 类”
        /// </summary>
        public static bool CheckIsGuidType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsGuidType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.UniqueIdentifier);
        }


        /// <summary>
        /// 判断一个字段的数据类型是否为 “Boolean 类”
        /// </summary>
        public static bool CheckIsBooleanType(Column c)
        {
            return CheckIsBooleanType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “Boolean 类”
        /// </summary>
        public static bool CheckIsBooleanType(UserDefinedFunctionParameter p)
        {
            return CheckIsBooleanType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “Boolean 类”
        /// </summary>
        public static bool CheckIsBooleanType(StoredProcedureParameter p)
        {
            return CheckIsBooleanType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “Boolean 类”
        /// </summary>
        public static bool CheckIsBooleanType(UserDefinedFunction f)
        {
            return CheckIsBooleanType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “Boolean 类”
        /// </summary>
        public static bool CheckIsBooleanType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsGuidType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.Bit);
        }




        /// <summary>
        /// 判断一个字段的数据类型是否为 “数字 类”
        /// </summary>
        public static bool CheckIsNumericType(Column c)
        {
            return CheckIsNumericType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “数字 类”
        /// </summary>
        public static bool CheckIsNumericType(UserDefinedFunctionParameter p)
        {
            return CheckIsNumericType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “数字 类”
        /// </summary>
        public static bool CheckIsNumericType(StoredProcedureParameter p)
        {
            return CheckIsNumericType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “数字 类”
        /// </summary>
        public static bool CheckIsNumericType(UserDefinedFunction f)
        {
            return CheckIsNumericType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “数字类”
        /// </summary>
        public static bool CheckIsNumericType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsGuidType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.BigInt
                || sdt == SqlDataType.Decimal
                || sdt == SqlDataType.Float
                || sdt == SqlDataType.Int
                || sdt == SqlDataType.Money
                || sdt == SqlDataType.Numeric
                || sdt == SqlDataType.Real
                || sdt == SqlDataType.SmallInt
                || sdt == SqlDataType.SmallMoney
                || sdt == SqlDataType.TinyInt);
        }




        /// <summary>
        /// 判断一个字段的数据类型是否为 “Binary 类”
        /// </summary>
        public static bool CheckIsBinaryType(Column c)
        {
            return CheckIsBinaryType(GetDatabase(c), c.DataType);
        }
        /// <summary>
        /// 判断一个函数的数据类型是否为 “Binary 类”
        /// </summary>
        public static bool CheckIsBinaryType(UserDefinedFunctionParameter p)
        {
            return CheckIsBinaryType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个过程的数据类型是否为 “Binary 类”
        /// </summary>
        public static bool CheckIsBinaryType(StoredProcedureParameter p)
        {
            return CheckIsBinaryType(GetDatabase(p), p.DataType);
        }
        /// <summary>
        /// 判断一个函数返回值的数据类型是否为 “Binary 类”
        /// </summary>
        public static bool CheckIsBinaryType(UserDefinedFunction f)
        {
            return CheckIsBinaryType(GetDatabase(f), f.DataType);
        }
        /// <summary>
        /// 判断一个数据类型是否为 “Binary 类”
        /// </summary>
        public static bool CheckIsBinaryType(Database db, DataType dt)
        {
            SqlDataType sdt = dt.SqlDataType;
            if (sdt == SqlDataType.UserDefinedDataType)
                return CheckIsGuidType(db, GetDataType(db.UserDefinedDataTypes[dt.Name, dt.Schema]));
            return (sdt == SqlDataType.Image
                    || sdt == SqlDataType.Binary
                    || sdt == SqlDataType.Timestamp
                    || sdt == SqlDataType.VarBinaryMax
                    || sdt == SqlDataType.VarBinary);
        }



        ///// <summary>
        ///// 判断一个数据类型是否为 “Nullable<> 类”  (值类)
        ///// </summary>
        //public static bool CheckIsNullableType(DataType sdt)
        //{
        //    return 
        //}

        /// <summary>
        /// 判断一个字串是否为 C# 的关键字
        /// http://msdn2.microsoft.com/en-us/library/x53a06bb.aspx
        /// </summary>
        public static bool CheckIsKeywords(string s)
        {
            s = s.ToLower();
            return s == "abstract" ||
                s == "event" ||
                s == "new" ||
                s == "struct" ||
                s == "as" ||
                s == "explicit" ||
                s == "null" ||
                s == "switch" ||
                s == "bas" ||
                s == "extern" ||
                s == "object" ||
                s == "this" ||

                s == "boolean" ||
                s == "false" ||
                s == "operator" ||
                s == "throw" ||

                s == "break" ||
                s == "finally" ||
                s == "out" ||
                s == "true" ||

                s == "byte" ||
                s == "fixed" ||
                s == "override" ||
                s == "try" ||

                s == "case" ||
                s == "float" ||
                s == "params" ||
                s == "typeof" ||

                s == "catch" ||
                s == "for" ||
                s == "private" ||
                s == "uint" ||

                s == "char" ||
                s == "foreach" ||
                s == "protected" ||
                s == "ulong" ||

                s == "checked" ||
                s == "goto" ||
                s == "public" ||
                s == "unchecked" ||

                s == "class" ||
                s == "if" ||
                s == "readonly" ||
                s == "unsafe" ||

                s == "const" ||
                s == "implicit" ||
                s == "ref" ||
                s == "ushort" ||

                s == "continue" ||
                s == "in" ||
                s == "return" ||
                s == "using" ||

                s == "decimal" ||
                s == "int32" ||
                s == "sbyte" ||
                s == "virtual" ||

                s == "default" ||
                s == "interface" ||
                s == "sealed" ||
                s == "volatile" ||

                s == "delegate" ||
                s == "public" ||
                s == "int16" ||
                s == "void" ||

                s == "do" ||
                s == "is" ||
                s == "sizeof" ||
                s == "while" ||

                s == "double" ||
                s == "lock" ||
                s == "stackalloc" ||


                s == "else" ||
                s == "int64" ||
                s == "static" ||


                s == "enum" ||
                s == "namespace" ||
                s == "string";
        }

        /// <summary>
        /// 查一个视图是否为 树表（它的基表符合外键指向自己的条件）
        /// </summary>
        public static bool CheckIsTree(View v)
        {
            return CheckIsTree(GetBaseTable(v));
        }


        /// <summary>
        /// 查一个表是否为 树表（符合外键指向自己的条件）
        /// </summary>
        public static bool CheckIsTree(Table t)
        {
            if (t == null) return false;
            List<Column> pks = Utils.GetPrimaryKeyColumns(t);
            if (pks == null || pks.Count == 0)		//没有主键？
            {
                return false;
            }

            if (t.ForeignKeys.Count == 0)
            {
                return false;
            }

            foreach (ForeignKey fk in t.ForeignKeys)
            {
                if (fk.ReferencedTable != t.Name || fk.ReferencedTableSchema != t.Schema) continue;
                int equaled = 0;
                foreach (ForeignKeyColumn fkc in fk.Columns)		//判断是否一个外键约束所有字段都是在当前表
                {
                    if (fkc.Parent.Parent == t) equaled++;
                }
                if (equaled == fk.Columns.Count)					//当前表为树表
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查 命令行 路径名 是否需要加引号
        /// </summary>
        public static bool CheckNeedQuote(string s)
        {
            return s.Contains(" ") || s.Contains("&") || s.Contains("(") || s.Contains(")") || s.Contains("[") ||
                    s.Contains("]") || s.Contains("{") || s.Contains("}") || s.Contains("^") || s.Contains("=") ||
                    s.Contains(";") || s.Contains("!") || s.Contains("'") || s.Contains("+") || s.Contains(",") ||
                    s.Contains("`") || s.Contains("~");
        }


        /// <summary>
        /// 判断一个表是否已设置成一个视图的基表
        /// </summary>
        public static bool CheckIsBaseTable(View v, Table t)
        {
            return Utils.GetBaseTableName(v) == t.Name && Utils.GetBaseTableSchema(v) == t.Schema;
        }
        /// <summary>
        /// 判断一个表是否已设置成一个视图的基表
        /// </summary>
        public static bool CheckIsBaseTable(Table t, View v)
        {
            return Utils.GetBaseTableName(v) == t.Name && Utils.GetBaseTableSchema(v) == t.Schema;
        }

        /// <summary>
        /// 判断一个表是否有资格成一个视图的基表
        /// </summary>
        public static bool CheckCanbeBaseTable(View v, Table t)
        {
            //检查 t 是否为 v 的字段子集
            bool isContain = true;
            foreach (Column c in t.Columns)
            {
                if (!v.Columns.Contains(c.Name))
                {
                    isContain = false;
                    break;
                }
            }
            return isContain;
        }
        /// <summary>
        /// 判断一个表是否有资格成一个视图的基表
        /// </summary>
        public static bool CheckCanbeBaseTable(Table t, View v)
        {
            return CheckCanbeBaseTable(v, t);
        }



        /// <summary>
        /// 检查一个字段是否为所属表的外键字段之一
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool CheckIsForeignKey(Column c)
        {
            Table t = (Table)c.Parent;
            foreach (ForeignKey fk in t.ForeignKeys)
            {
                foreach (ForeignKeyColumn fkc in fk.Columns)
                {
                    Column o = t.Columns[fkc.Name];
                    if (c == o) return true;
                }
            }
            return false;
        }


        

        #endregion

        #region Escape

        /// <summary>
        /// 是否为代码中的方法名，类名等生成架构名前缀（生成前手工初始化）
        /// </summary>
        public static string SchemaSplitter = null;

        /// <summary>
        /// 取转义后的字段名称（类名，属性名，参数名等）字串（去空格，过滤类，类成员名中的非法字符，处理和 C# 数据类型同名的字段名：前面加 _）
        /// </summary>
        public static string GetEscapeName(object o)
        {
            if (o.GetType() == typeof(Column))
            {
                Column c = (Column)o;
                if (c.Parent.GetType() == typeof(Table))
                {
                    Table t = (Table)c.Parent;
                    return GetEscapeName(c.Name) + (t.Name == c.Name ? "_" : "");
                }
                else if (c.Parent.GetType() == typeof(UserDefinedTableType))
                {
                    UserDefinedTableType t = (UserDefinedTableType)c.Parent;
                    return GetEscapeName(c.Name) + (t.Name == c.Name ? "_" : "");
                }
                else if (c.Parent.GetType() == typeof(View))
                {
                    View t = (View)c.Parent;
                    return GetEscapeName(c.Name) + (t.Name == c.Name ? "_" : "");
                }
                else if (c.Parent.GetType() == typeof(UserDefinedFunction))
                {
                    UserDefinedFunction t = (UserDefinedFunction)c.Parent;
                    return GetEscapeName(c.Name) + (t.Name == c.Name ? "_" : "");
                }
            }
            else if (o.GetType() == typeof(Table))
            {
                Table t = (Table)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(UserDefinedTableType))
            {
                UserDefinedTableType t = (UserDefinedTableType)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(View))
            {
                View t = (View)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(UserDefinedFunction))
            {
                UserDefinedFunction t = (UserDefinedFunction)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(UserDefinedFunctionParameter))
            {
                UserDefinedFunctionParameter t = (UserDefinedFunctionParameter)o;
                return GetEscapeName(t.Name.Substring(1));
            }
            else if (o.GetType() == typeof(StoredProcedure))
            {
                StoredProcedure t = (StoredProcedure)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(StoredProcedureParameter))
            {
                StoredProcedureParameter t = (StoredProcedureParameter)o;
                return GetEscapeName(t.Name.Substring(1));
            }
            else if (o.GetType() == typeof(ForeignKeyColumn))
            {
                ForeignKeyColumn t = (ForeignKeyColumn)o;
                return GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(ForeignKey))
            {
                ForeignKey t = (ForeignKey)o;
                return GetEscapeName(t.Name);
            }
            else if (o.GetType() == typeof(DataType))
            {
                DataType t = (DataType)o;
                return (!string.IsNullOrEmpty(SchemaSplitter) ? (GetEscapeName(t.Schema) + SchemaSplitter) : "") + GetEscapeName(t.Name);
            }

            throw new Exception("未处理的数据类型");
        }

        /// <summary>
        /// 取转义后的名称（类名，属性名，参数名等）字串（去空格，过滤类，类成员名中的非法字符，处理和 C# 数据类型同名的字段名：前面加 _）
        /// </summary>
        public static string GetEscapeName(string s)
        {
            s = s.Trim();
            if (CheckIsKeywords(s)) return "_" + s;
            if (s[0] >= '0' && s[0] <= '9') s = "_" + s;
            return s.Replace(' ', '_')
                .Replace(',', '_')
                .Replace('.', '_')
                .Replace(';', '_')
                .Replace(':', '_')
                .Replace('~', '_')
                .Replace('(', '_')
                .Replace(')', '_')
                .Replace('#', '_')
                .Replace('\\', '_')
                .Replace('/', '_')
                .Replace('=', '_')
                .Replace('>', '_')
                .Replace('<', '_')
                .Replace('+', '_')
                .Replace('-', '_')
                .Replace('*', '_')
                .Replace('%', '_')
                .Replace('&', '_')
                .Replace('|', '_')
                .Replace('^', '_')
                .Replace('\'', '_')
                .Replace('"', '_')
                .Replace('[', '_')
                .Replace(']', '_')
                .Replace('!', '_')
                .Replace('@', '_')
                .Replace('$', '_');
        }

        /// <summary>
        /// 取转义后的位于 RowFilter 中的字段名字串（）
        /// </summary>
        public static string GetEscapeRowFilterName(string s)
        {
            if (s.Contains("~")
                || s.Contains("(")
                || s.Contains(")")
                || s.Contains("#")
                || s.Contains("\\")
                || s.Contains("/")
                || s.Contains("=")
                || s.Contains(">")
                || s.Contains("<")
                || s.Contains("+")
                || s.Contains("-")
                || s.Contains("*")
                || s.Contains("%")
                || s.Contains("&")
                || s.Contains("|")
                || s.Contains("^")
                || s.Contains("\'")
                || s.Contains("\"")
                || s.Contains("[")
                || s.Contains("]")
                || s.Contains(" ")
                || s.Contains(",")
                || s.Contains(".")
                || s.Contains(";")
                || s.Contains(":")
                )
            {
                return "[" + s.Replace("]", @"\]") + "]";
            }
            return s;
        }

        /// <summary>
        /// 取转义后的数据库对象名（标识符）
        /// </summary>
        public static string GetEscapeSqlObjectName(string s)
        {
            return s.Replace("]", @"]]");
        }


        #endregion

        #region FormatString

        /// <summary>
        /// 包含空格的一个字串
        /// </summary>
        public const string Spaces = "                                                                                                                                                      ";

        /// <summary>
        /// 将 s1 格式化为 lenOfs1 字长（加空格）
        /// </summary>
        public static string FormatString(string s1, string s2, int lenOfs1)
        {
            int s1len = System.Text.Encoding.Default.GetByteCount(s1);
            if (s1len < lenOfs1) return s1 + Spaces.Substring(0, lenOfs1 - s1len) + s2;
            return s1 + " " + s2;
        }

        /// <summary>
        /// 将 s1, s2 格式化为 lenOfs1 lenOfs2 字长（加空格）
        /// </summary>
        public static string FormatString(string s1, string s2, string s3, int lenOfs1, int lenOfs2)
        {
            int s1len = System.Text.Encoding.Default.GetByteCount(s1);
            int s2len = System.Text.Encoding.Default.GetByteCount(s2);
            if (s1len < lenOfs1) s1 += Spaces.Substring(0, lenOfs1 - s1len);
            else s1 += " ";
            if (s2len < lenOfs2) s2 += Spaces.Substring(0, lenOfs2 - s2len);
            else s2 += " ";
            return s1 + s2 + s3;
        }

        #endregion


        #region DAL_GEN_Config


        /// <summary>
        /// 指向当前数据库的 DAL 生成配置
        /// </summary>
        public static DS _CurrrentDALGenSetting = null;

        /// <summary>
        /// 指向当前数据库的 DAL 生成配置 里面的 当前配置方案
        /// </summary>
        public static DS.SchemesRow _CurrrentDALGenSetting_CurrentScheme = null;


        /// <summary>
        /// 为 DS 实例填充 DAL 生成配置 初始数据
        /// </summary>
        public static void FillDatabaseDALGenSettingDS(Database db, DS ds)
        {
            string s = Utils.GetDescription(db, "CodeGenSettings_DALGen");
            if (!string.IsNullOrEmpty(s)) ds.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(s)));

            DS.TypeNamesRow t_Tables;
            DS.TypeNamesRow t_Views;
            DS.TypeNamesRow t_StoredProcedures;
            DS.TypeNamesRow t_Functions;

            // 从当前 ds 中查找默认数据
            t_Tables = ds.TypeNames.FindByTypeName("Tables");
            t_Views = ds.TypeNames.FindByTypeName("Views");
            t_StoredProcedures = ds.TypeNames.FindByTypeName("StoredProcedures");
            t_Functions = ds.TypeNames.FindByTypeName("Functions");

            // 未找到则填充
            if (t_Tables == null) t_Tables = ds.TypeNames.AddTypeNamesRow("Tables");
            if (t_Views == null) t_Views = ds.TypeNames.AddTypeNamesRow("Views");
            if (t_StoredProcedures == null) t_StoredProcedures = ds.TypeNames.AddTypeNamesRow("StoredProcedures");
            if (t_Functions == null) t_Functions = ds.TypeNames.AddTypeNamesRow("Functions");

            DS.SchemesRow s_Default;
            s_Default = ds.Schemes.FindBySchemesID(1);
            if (s_Default == null)
            {
                string ns = Utils.GetDescription(db, "CodeGenSettings_DefaultNamespace");
                if (string.IsNullOrEmpty(ns)) ns = "DAL";

                bool isGenWCFAttribute = false;
                s = Utils.GetDescription(db, "CodeGenSettings_IsGenWCFAttribute");
                if (!string.IsNullOrEmpty(s)) isGenWCFAttribute = bool.Parse(s);

                bool isGenSchemaSupport = false;
                s = Utils.GetDescription(db, "CodeGenSettings_SchemaHandleMode");
                if (s == "_") isGenSchemaSupport = true;

                s_Default = ds.Schemes.AddSchemesRow(ns, "默认的生成方案", isGenSchemaSupport, isGenWCFAttribute, false, true, false, false, false, false, true, true, true, true, false, 1, "默认");

                s = Utils.GetDescription(db, "CodeGenSettings_Filters");
                if (string.IsNullOrEmpty(s))
                {
                    ds.SchemesFilters.AddSchemesFiltersRow(t_Tables, true, ".", s_Default, "默认允许所有用户表", "");
                    ds.SchemesFilters.AddSchemesFiltersRow(t_Views, true, ".", s_Default, "默认允许所有用户视图", "");
                    ds.SchemesFilters.AddSchemesFiltersRow(t_StoredProcedures, true, ".", s_Default, "默认允许所有用户存储过程", "");
                    ds.SchemesFilters.AddSchemesFiltersRow(t_Functions, true, ".", s_Default, "默认允许所有用户函数", "");

                    ds.SchemesFilters.AddSchemesFiltersRow(t_Tables, false, "^aspnet_.", s_Default, "默认屏蔽 membership 相关表", "dbo");
                    ds.SchemesFilters.AddSchemesFiltersRow(t_Views, false, "^vw_aspnet_.", s_Default, "默认屏蔽 membership 相关视图", "dbo");
                    ds.SchemesFilters.AddSchemesFiltersRow(t_StoredProcedures, false, "^aspnet_.", s_Default, "默认屏蔽 membership 相关过程", "dbo");
                }
                else
                {
                    try
                    {
                        ds.Filters.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(s)));
                    }
                    catch { }
                    foreach (DS.FiltersRow r in ds.Filters)
                    {
                        ds.SchemesFilters.AddSchemesFiltersRow(r.TypeNamesRow, r.IsAllow, r.FilterString, s_Default, r.Memo, "");
                    }
                    ds.Filters.Clear();
                }
            }

            ds.AcceptChanges();
        }

        /// <summary>
        /// 载入 DAL 生成设置到当前上下文
        /// </summary>
        public static void LoadDatabaseDALGenSettingDS(Database db)
        {
            _CurrrentDALGenSetting = new DS();
            FillDatabaseDALGenSettingDS(db, _CurrrentDALGenSetting);
            _CurrrentDALGenSetting_CurrentScheme = _CurrrentDALGenSetting.Schemes.FindBySchemesID(1);
            SchemaSplitter = _CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? "_" : null;
        }
        /// <summary>
        /// 载入 DAL 生成设置到当前上下文 指定配置方案编号
        /// </summary>
        public static void LoadDatabaseDALGenSettingDS(Database db, int schemeID)
        {
            _CurrrentDALGenSetting = new DS();
            FillDatabaseDALGenSettingDS(db, _CurrrentDALGenSetting);
            _CurrrentDALGenSetting_CurrentScheme = _CurrrentDALGenSetting.Schemes.FindBySchemesID(schemeID);
            SchemaSplitter = _CurrrentDALGenSetting_CurrentScheme.IsSupportSchema ? "_" : null;
        }
        /// <summary>
        /// 将当前上下文中的 DAL 生成设置保存到数据库
        /// </summary>
        public static void SaveDatabaseDALGenSettingDS(Database db)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            _CurrrentDALGenSetting.AcceptChanges();
            _CurrrentDALGenSetting.WriteXml(sw);
            Utils.SetDescription(db, "CodeGenSettings_DALGen", sb.ToString());
        }
        /// <summary>
        /// 将指定的 DAL 生成设置保存到数据库
        /// </summary>
        public static void SaveDatabaseDALGenSettingDS(Database db, DS ds)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ds.AcceptChanges();
            ds.WriteXml(sw);
            Utils.SetDescription(db, "CodeGenSettings_DALGen", sb.ToString());
        }



        #endregion

        #region Get Database Objects ( User's Tables, Functions, StoredProcedures, Columns )

        /// <summary>
        /// 返回树表的 主外键 字典（如果没有返回 0 长度字典）
        /// </summary>
        public static Dictionary<Column, Column> GetTreePKFKColumns(Table t)
        {
            Dictionary<Column, Column> ccs = new Dictionary<Column, Column>();
            foreach (ForeignKey fk in t.ForeignKeys)
            {
                if (fk.ReferencedTable != t.Name || fk.ReferencedTableSchema != t.Schema) continue;
                int equaled = 0;
                foreach (ForeignKeyColumn fkc in fk.Columns)		//判断是否一个外键约束所有字段都是在当前表
                {
                    if (fkc.Parent.Parent == t) equaled++;
                }
                if (equaled == fk.Columns.Count)					//当前表为树表
                {
                    for (int i = 0; i < fk.Columns.Count; i++)
                    {
                        ForeignKeyColumn fkc = fk.Columns[i];
                        Column f = t.Columns[fkc.Name];
                        Column p = t.Columns[fkc.ReferencedColumn];
                        ccs.Add(p, f);
                    }
                    return ccs;
                }
            }
            return ccs;
        }

        /// <summary>
        /// 返回一个表的主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetPrimaryKeyColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个表的非主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetNonPrimaryKeyColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (!c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个表里的自增字段（如果没有返回空）
        /// </summary>
        public static Column GetIdentityColumn(Table t)
        {
            foreach (Column c in t.Columns)
            {
                if (c.Identity) return c;
            }
            return null;
        }

        /// <summary>
        /// 返回一个视图的非主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetNonPrimaryKeyColumns(View v)
        {
            List<Column> pkcs = GetPrimaryKeyColumns(v);
            List<Column> cs = new List<Column>();
            foreach (Column c in v.Columns)
            {
                if (!pkcs.Contains(c)) cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个表值函数的主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetPrimaryKeyColumns(UserDefinedFunction f)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in f.Columns)
            {
                if (c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个表值函数的非主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetNonPrimaryKeyColumns(UserDefinedFunction f)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in f.Columns)
            {
                if (!c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个用户自定义表类型的主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetPrimaryKeyColumns(UserDefinedTableType t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个用户自定义表类型的非主键集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetNonPrimaryKeyColumns(UserDefinedTableType t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (!c.InPrimaryKey) cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个表中的可写字段集合 （排除计算列，自增列，Timestamp 列）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetWriteableColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.Computed || c.Identity || c.DataType.SqlDataType == SqlDataType.Timestamp) continue;		// || c.RowGuidCol
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个视图中的可写字段集合 （判断基表的可写情况从而返回，基表里没有认为可写）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetWriteableColumns(View v)
        {
            Table t = GetBaseTable(v);
            if (t != null)
            {
                List<Column> cs = new List<Column>();
                foreach (Column c in t.Columns)
                {
                    if (c.Computed || c.Identity || c.DataType.SqlDataType == SqlDataType.Timestamp) continue;		// || c.RowGuidCol
                    cs.Add(v.Columns[c.Name]);
                }
                foreach (Column c in v.Columns)
                {
                    if (!cs.Exists(new Predicate<Column>(delegate(Column o) { return o.Name == c.Name; })))
                    {
                        cs.Add(c);
                    }
                }
                return cs;
            }
            else
            {
                List<Column> cs = new List<Column>();
                foreach (Column c in v.Columns)
                {
                    cs.Add(c);
                }
                return cs;
            }
        }

        /// <summary>
        /// 返回一个表值函数中的可写字段集合 （暂定所有列可写 除了 Timestamp ）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetWriteableColumns(UserDefinedFunction t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.DataType.SqlDataType == SqlDataType.Timestamp) continue;
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个用户自定义表类型中的可写字段集合 （排除计算列，自增列，Timestamp）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetWriteableColumns(UserDefinedTableType t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.Computed || c.Identity || c.DataType.SqlDataType == SqlDataType.Timestamp) continue;		// || c.RowGuidCol
                cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个表中的必写字段集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetMustWriteColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.Identity || c.Computed || c.Nullable || c.DefaultConstraint != null) continue;
                cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个用户自定义表类型中的必写字段集合（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetMustWriteColumns(UserDefinedTableType t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (c.Identity || c.Computed || c.Nullable || c.DefaultConstraint != null) continue;
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个表中的可排序字段集合 （排除二进制，图片，文本等类型列）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSortableColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (Utils.CheckIsBinaryType(c)) continue;
                cs.Add(c);
            }
            return cs;
        }

        /// <summary>
        /// 返回一个表函数结果中的可排序字段集合 （排除二进制，图片，文本等类型列）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSortableColumns(UserDefinedFunction t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (Utils.CheckIsBinaryType(c)) continue;
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个视图中的可排序字段集合 （排除二进制，图片，文本等类型列）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSortableColumns(View v)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in v.Columns)
            {
                if (Utils.CheckIsBinaryType(c)) continue;
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个用户自定义表类型结果中的可排序字段集合 （排除二进制，图片，文本等类型列）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSortableColumns(UserDefinedTableType t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (Utils.CheckIsBinaryType(c)) continue;
                cs.Add(c);
            }
            return cs;
        }


        /// <summary>
        /// 返回一个表中的可模糊查询字段集合 （字串类）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSearchableColumns(Table t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (CheckIsStringType(c))
                {
                    cs.Add(c);
                }
            }
            return cs;
        }
        /// <summary>
        /// 返回一个表值函数结果中的可模糊查询字段集合 （字串类）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSearchableColumns(UserDefinedFunction t)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in t.Columns)
            {
                if (CheckIsStringType(c))
                {
                    cs.Add(c);
                }
            }
            return cs;
        }

        /// <summary>
        /// 返回一个视图中的可模糊查询字段集合 （字串类）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSearchableColumns(View v)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in v.Columns)
            {
                if (CheckIsStringType(c))
                {
                    cs.Add(c);
                }
            }
            return cs;
        }

        /// <summary>
        /// 返回一个 用户自定义表类型 中的可模糊查询字段集合 （字串类）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Column> GetSearchableColumns(UserDefinedTableType v)
        {
            List<Column> cs = new List<Column>();
            foreach (Column c in v.Columns)
            {
                if (CheckIsStringType(c))
                {
                    cs.Add(c);
                }
            }
            return cs;
        }













        /// <summary>
        /// 返回用户表（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Table> GetUserTables(Database db, DS ds)
        {
            DS.SchemesFiltersRow[] rs = (DS.SchemesFiltersRow[])ds.SchemesFilters.Select("TypeName = 'Tables' AND SchemesID = " + _CurrrentDALGenSetting_CurrentScheme.SchemesID.ToString(), "SortOrder", DataViewRowState.CurrentRows);

            List<Table> os = new List<Table>();
            foreach (Table o in db.Tables)
            {
                if (o.IsSystemObject)			// 如果是系统对象, 则去查找是否存在完全匹配的过滤信息
                {
                    bool b = true;
                    foreach (DS.SchemesFiltersRow r in rs)
                    {
                        if (("^" + o.Name + "$").Equals(r.FilterString, StringComparison.CurrentCultureIgnoreCase) && o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase) && r.IsAllow)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (b) continue;
                }

                foreach (DS.SchemesFiltersRow r in rs)
                {
                    try
                    {
                        if (Regex.IsMatch(o.Name, r.FilterString, RegexOptions.IgnoreCase) && (r.Schema == "" || o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            if (r.IsAllow)
                            {
                                if (!os.Contains(o)) os.Add(o);
                            }
                            else if (os.Contains(o)) os.Remove(o);
                        }
                    }
                    catch { }
                }
            }
            return os;
        }

        /// <summary>
        /// 返回用户表（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Table> GetUserTables(Database db)
        {
            if (_CurrrentDALGenSetting == null) LoadDatabaseDALGenSettingDS(db);
            return GetUserTables(db, _CurrrentDALGenSetting);
        }

        /// <summary>
        /// 返回用户过程（如果没有返回 0 长度列表）
        /// </summary>
        public static List<StoredProcedure> GetUserStoredProcedures(Database db, DS ds)
        {
            DS.SchemesFiltersRow[] rs = (DS.SchemesFiltersRow[])ds.SchemesFilters.Select("TypeName = 'StoredProcedures' AND SchemesID = " + _CurrrentDALGenSetting_CurrentScheme.SchemesID.ToString(), "SortOrder", DataViewRowState.CurrentRows);

            List<StoredProcedure> os = new List<StoredProcedure>();
            foreach (StoredProcedure o in db.StoredProcedures)
            {
                if (o.IsSystemObject)			// 如果是系统对象, 则去查找是否存在完全匹配的过滤信息
                {
                    bool b = true;
                    foreach (DS.SchemesFiltersRow r in rs)
                    {
                        if (("^" + o.Name + "$").Equals(r.FilterString, StringComparison.CurrentCultureIgnoreCase) && o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase) && r.IsAllow)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (b) continue;
                }


                foreach (DS.SchemesFiltersRow r in rs)
                {
                    try
                    {
                        if (Regex.IsMatch(o.Name, r.FilterString, RegexOptions.IgnoreCase) && (r.Schema == "" || o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            if (r.IsAllow)
                            {
                                if (!os.Contains(o)) os.Add(o);
                            }
                            else if (os.Contains(o)) os.Remove(o);
                        }
                    }
                    catch { }
                }
            }
            return os;
        }


        /// <summary>
        /// 返回用户过程（如果没有返回 0 长度列表）
        /// </summary>
        public static List<StoredProcedure> GetUserStoredProcedures(Database db)
        {
            if (_CurrrentDALGenSetting == null) LoadDatabaseDALGenSettingDS(db);
            return GetUserStoredProcedures(db, _CurrrentDALGenSetting);
        }

        /// <summary>
        /// 返回用户函数（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedFunction> GetUserFunctions(Database db, DS ds)
        {
            DS.SchemesFiltersRow[] rs = (DS.SchemesFiltersRow[])ds.SchemesFilters.Select("TypeName = 'Functions' AND SchemesID = " + _CurrrentDALGenSetting_CurrentScheme.SchemesID.ToString(), "SortOrder", DataViewRowState.CurrentRows);

            List<UserDefinedFunction> os = new List<UserDefinedFunction>();
            foreach (UserDefinedFunction o in db.UserDefinedFunctions)
            {
                if (o.IsSystemObject)			// 如果是系统对象, 则去查找是否存在完全匹配的过滤信息
                {
                    bool b = true;
                    foreach (DS.SchemesFiltersRow r in rs)
                    {
                        if (("^" + o.Name + "$").Equals(r.FilterString, StringComparison.CurrentCultureIgnoreCase) && o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase) && r.IsAllow)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (b) continue;
                }


                foreach (DS.SchemesFiltersRow r in rs)
                {
                    try
                    {
                        if (Regex.IsMatch(o.Name, r.FilterString, RegexOptions.IgnoreCase) && (r.Schema == "" || o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            if (r.IsAllow)
                            {
                                if (!os.Contains(o)) os.Add(o);
                            }
                            else if (os.Contains(o)) os.Remove(o);
                        }
                    }
                    catch { }
                }
            }
            return os;
        }

        /// <summary>
        /// 返回用户函数（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedFunction> GetUserFunctions(Database db)
        {
            if (_CurrrentDALGenSetting == null) LoadDatabaseDALGenSettingDS(db);
            return GetUserFunctions(db, _CurrrentDALGenSetting);
        }

        /// <summary>
        /// 返回用户函数（表值）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedFunction> GetUserFunctions_TableValue(Database db)
        {
            List<UserDefinedFunction> os = new List<UserDefinedFunction>();
            foreach (UserDefinedFunction o in GetUserFunctions(db))
            {
                if (o.FunctionType == UserDefinedFunctionType.Table || o.FunctionType == UserDefinedFunctionType.Inline) os.Add(o);
            }
            return os;
        }

        /// <summary>
        /// 返回用户函数（标值）（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedFunction> GetUserFunctions_ScalarValue(Database db)
        {
            List<UserDefinedFunction> os = new List<UserDefinedFunction>();
            foreach (UserDefinedFunction o in GetUserFunctions(db))
            {
                if (o.FunctionType == UserDefinedFunctionType.Scalar) os.Add(o);
            }
            return os;
        }


        /// <summary>
        /// 返回用户视图（如果没有返回 0 长度列表）
        /// </summary>
        public static List<View> GetUserViews(Database db, DS ds)
        {
            DS.SchemesFiltersRow[] rs = (DS.SchemesFiltersRow[])ds.SchemesFilters.Select("TypeName = 'Views' AND SchemesID = " + _CurrrentDALGenSetting_CurrentScheme.SchemesID.ToString(), "SortOrder", DataViewRowState.CurrentRows);

            List<View> os = new List<View>();
            foreach (View o in db.Views)
            {
                if (o.IsSystemObject)			// 如果是系统对象, 则去查找是否存在完全匹配的过滤信息
                {
                    bool b = true;
                    foreach (DS.SchemesFiltersRow r in rs)
                    {
                        if (("^" + o.Name + "$").Equals(r.FilterString, StringComparison.CurrentCultureIgnoreCase) && o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase) && r.IsAllow)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (b) continue;
                }


                foreach (DS.SchemesFiltersRow r in rs)
                {
                    try
                    {
                        if (Regex.IsMatch(o.Name, r.FilterString, RegexOptions.IgnoreCase) && (r.Schema == "" || o.Schema.Equals(r.Schema, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            if (r.IsAllow)
                            {
                                if (!os.Contains(o)) os.Add(o);
                            }
                            else if (os.Contains(o)) os.Remove(o);
                        }
                    }
                    catch { }
                }
            }
            return os;
        }

        /// <summary>
        /// 返回用户视图（如果没有返回 0 长度列表）
        /// </summary>
        public static List<View> GetUserViews(Database db)
        {
            if (_CurrrentDALGenSetting == null) LoadDatabaseDALGenSettingDS(db);
            return GetUserViews(db, _CurrrentDALGenSetting);
        }

        /// <summary>
        /// 返回用户定义表类型（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedTableType> GetUserDefinedTableTypes(Database db)
        {
            List<UserDefinedTableType> os = new List<UserDefinedTableType>();
            if (db.CompatibilityLevel >= CompatibilityLevel.Version100)
            {
                foreach (UserDefinedTableType o in db.UserDefinedTableTypes)
                {
                    if (o.IsUserDefined) os.Add(o);
                }
            }
            return os;
        }


        /// <summary>
        /// 返回用户定义数据（别名）类型（如果没有返回 0 长度列表）
        /// </summary>
        public static List<UserDefinedDataType> GetUserDefinedDataTypes(Database db)
        {
            List<UserDefinedDataType> os = new List<UserDefinedDataType>();
            foreach (UserDefinedDataType o in db.UserDefinedDataTypes)
            {
                os.Add(o);
            }
            return os;
        }

        /// <summary>
        /// 返回用户定义架构列表（如果没有返回 0 长度列表）
        /// </summary>
        public static List<Schema> GetUserSchemas(Database db)
        {
            List<Schema> ss = new List<Schema>();
            foreach (Schema s in db.Schemas)
            {
                if (s.Name == "db_accessadmin"
                    || s.Name == "db_backupoperator"
                    || s.Name == "db_datareader"
                    || s.Name == "db_datawriter"
                    || s.Name == "db_ddladmin"
                    || s.Name == "db_denydatareader"
                    || s.Name == "db_denydatawriter"
                    || s.Name == "db_owner"
                    || s.Name == "db_securityadmin"
                    || s.Name == "dbo"
                    || s.Name == "guest"
                    || s.Name == "INFORMATION_SCHEMA"
                    || s.Name == "sys")
                    continue;
                ss.Add(s);
            }
            return ss;
        }


        /// <summary>
        /// 返回表的最适合于显示的字段（优先级：第一个限长字段，第一个不限长字段，第一个主键字段）
        /// todo: 继续完善
        /// </summary>
        public static Column GetDisplayColumn(Table t)
        {
            foreach (Column c in t.Columns)
            {
                if (CheckIsStringType(c) && (c.DataType.MaximumLength > 0 && c.DataType.MaximumLength < 4000)) return c;
            }
            foreach (Column c in t.Columns)
            {
                if (CheckIsStringType(c)) return c;
            }
            foreach (Column c in t.Columns)
            {
                if (c.InPrimaryKey) return c;
            }

            return t.Columns[0];
        }



        /// <summary>
        /// 返回一个数据对象的扩展属性集合
        /// </summary>
        public static List<ExtendedProperty> GetExtendedProperties(Database db)
        {
            List<ExtendedProperty> result = new List<ExtendedProperty>();
            foreach (ExtendedProperty ep in db.ExtendedProperties) result.Add(ep);
            return result;
        }

        /// <summary>
        /// 返回一个数据对象的扩展属性集合
        /// </summary>
        public static List<ExtendedProperty> GetExtendedProperties(Table t)
        {
            List<ExtendedProperty> result = new List<ExtendedProperty>();
            foreach (ExtendedProperty ep in t.ExtendedProperties) result.Add(ep);
            return result;
        }

        #endregion


    }
}
