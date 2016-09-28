using System;
using System.Collections.Generic;
using System.Text;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// �û��Զ������ͽű����� �����ݱ����ͼ������ṹ�õ��û��Զ�������ʹ����ű���
	/// ���ɹ��򣺱���������ȥ���������Ĭ��ֵ�������ɿ�����
	/// </summary>
	public static partial class Gen_UT
	{
		public static string Gen(Table t)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(@"
CREATE TYPE [" + Utils.GetEscapeSqlObjectName(t.Schema) + "].[" + Utils.GetEscapeSqlObjectName(t.Name) + "] AS TABLE");
			foreach (Column c in t.Columns)
			{

			}
			sb.Append(@"");

			return sb.ToString();
		}
		public static string Gen(View t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}
		public static string Gen(UserDefinedFunction t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}
		public static string Gen(UserDefinedTableType t)
		{
			StringBuilder sb = new StringBuilder();

			return sb.ToString();
		}

	}
}
