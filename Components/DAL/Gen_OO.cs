using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.DAL
{
	/// <summary>
	/// ���ݱ���ͼ����ֵ����������ʵ��������
	/// </summary>
	public static partial class Gen_OO
	{
		public static string Gen(Database db, string ns, bool isForWCF)
		{
			#region Header

			StringBuilder sb = new StringBuilder();

			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> ufs = Utils.GetUserFunctions_TableValue(db);

			sb.Append(@"using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;");
			if (isForWCF) sb.Append(@"
using System.Runtime.Serialization;");
			sb.Append(@"

namespace " + ns + @"
{
	/// <summary>
	/// �����ݿ�ṹ��Ӧ����ʵ�����伯��������������ͼ����ֵ�����Ķ���
	/// </summary>");
			sb.Append(@"
	public static partial class OO
	{");
			#endregion

			#region Table

			sb.Append(@"
		#region Tables
");

			foreach (Table t in uts)
			{
				List<Column> acs = new List<Column>();
				foreach (Column c in t.Columns) acs.Add(c);

				List<Column> pkcs = Utils.GetPrimaryKeyColumns(t);
				List<Column> ncs = Utils.GetNonPrimaryKeyColumns(t);
				string tn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tn + @"
");

				sb.Append(Utils.GetSummary(t, 2));
				if (isForWCF) sb.Append(@"
		[DataContract]");
				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"
		{");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			#region �������ඨ��

			/// <summary>
			/// ��" + t.Name + @" ����������
			/// </summary>");
					if (isForWCF) sb.Append(@"
			[DataContract]");
					sb.Append(@"
			[Serializable]
			public partial class PrimaryKeys : IEquatable<PrimaryKeys>
			{");
					foreach (Column c in pkcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				protected " + typename + @" __________" + cn + @";");
					}
					foreach (Column c in pkcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(Utils.GetSummary(c, 3));
						if (isForWCF) sb.Append(@"
				[DataMember]");
						sb.Append(@"
				public " + typename + " " + cn + @"
				{
					get { return __________" + cn + @"; }
					set { __________" + cn + @" = value; }
				}");
					}
					sb.Append(@"
				public PrimaryKeys() { }

				public PrimaryKeys(");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
				{");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
					__________" + cn + @" = " + cn + @";");
					}
					sb.Append(@"
				}

				#region IEquatable<PrimaryKeys> Members

				public bool Equals(PrimaryKeys other)
				{
					return ");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" && ");
						sb.Append(@"__________" + cn + @" == other." + cn);
					}
					sb.Append(@";
				}

				#endregion
			}

			/// <summary>
			/// ��" + t.Name + @" ���������༯��
			/// </summary>");
					sb.Append(@"
			[Serializable]
			public partial class PrimaryKeysCollection : List<PrimaryKeys> { }

			#endregion
");
				}
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			#region ˽�г�Ա����ȡ��������������ֵ�Աȷ���

			protected PrimaryKeys __PKs;
			/// <summary>
			/// ��ȡ��" + t.Name + @" ��������ʵ��
			/// </summary>
			public PrimaryKeys GetPrimaryKeys()
			{
				return __PKs;
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @" other)
			{
				return __PKs.Equals(other.GetPrimaryKeys());
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @".PrimaryKeys other)
			{
				return __PKs.Equals(other);
			}");
				}
				else
				{
					sb.Append(@"
			#region ˽�г�Ա
");
				}
				foreach (Column c in ncs)
				{
					string typename;
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					sb.Append(@"
			protected " + typename + @" __________" + cn + @";");
				}
				sb.Append(@"

			#endregion
");
				sb.Append(@"
			#region ���캯��
");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			public " + tn + @"()
			{
				__PKs = new PrimaryKeys();
			}
			public " + tn + @"(PrimaryKeys pk)
			{
				__PKs = pk;
			}");

					sb.Append(@"
			public " + tn + @"(");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{
				__PKs = new PrimaryKeys(");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@");");
					foreach (Column c in ncs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				__________" + cn + @" = " + cn + ";");
					}
					sb.Append(@"
			}");
					if (acs.Count > pkcs.Count)
					{
						sb.Append(@"
			public " + tn + @"(PrimaryKeys __pk");
						foreach (Column c in ncs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@", " + typename + " " + cn);
						}
						sb.Append(@")
			{
				__PKs = __pk;");
						foreach (Column c in ncs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
				__________" + cn + @" = " + cn + ";");
						}
						sb.Append(@"
			}");

					}
				}
				else
				{
					sb.Append(@"
			public " + tn + @"()
			{
			}
			public " + tn + @"(");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				__________" + cn + @" = " + cn + ";");
					}
					sb.Append(@"
			}");
				}
				sb.Append(@"

			#endregion
			
");
				sb.Append(@"

			#region ���Ʒ���

			/// <summary>
			/// ����ǰ�����ֵ���ǵ���һͬ���Ͷ���
			/// </summary>
			public virtual void CopyTo(" + tn + @" __oo)
			{");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
				}
				sb.Append(@"
			}

			/// <summary>
			/// ����һͬ���Ͷ����и���ֵ���ǵ���ǰ����
			/// </summary>
			public virtual void CopyFrom(" + tn + @" __oo)
			{");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				this." + cn + @" = __oo." + cn + @";");
				}
				sb.Append(@"
			}

			/// <summary>
			/// ����һ�ݵ�ǰ�����Ϊһ����ʵ������
			/// </summary>
			public virtual " + tn + @" Copy()
			{
				" + tn + @" __oo = new " + tn + @"();");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
				}
				sb.Append(@"
				return __oo;
			}

			#endregion

			#region �ȽϷ���

			/// <summary>
			/// �Ƚϵ�ǰ����ʹ�������ֵ�Ƿ����
			/// </summary>
			public virtual bool Equals(" + tn + @" __o)
			{
				if (this == __o) return true;
				if (");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
					");
					if (i > 0) sb.Append(@"&& ");
					sb.Append(@"this." + cn + @" == __o." + cn);
				}
				sb.Append(@"
				) return true;
				return false;
			}

			#endregion
");

				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					sb.Append(Utils.GetSummary(c, 3));
					if (isForWCF) sb.Append(@"
			[DataMember]");
					if (pkcs.Contains(c))
					{
						sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __PKs." + cn + @"; }
				set { __PKs." + cn + @" = value; }
			}");
					}
					else sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __________" + cn + @"; }
				set { __________" + cn + @" = value; }
			}");
				}

				sb.Append(@"
		}
");

				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection : List<" + tn + @">
		{
			/// <summary>
			/// ֱ�����������ĳԪ����ֵ��������ĳԪ�ص�ʵ��
			/// </summary>
			public " + tn + @" Add(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(typename + " " + cn);
				}
				sb.Append(@")
			{
				" + tn + @" o = new " + tn + @"(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(cn);
				}
				sb.Append(@");
				this.Add(o);
				return o;
			}

			/// <summary>
			/// �ϲ���һ�����������ǰ���ϣ�������ͬ���滻ԭֵ����ͬ�������� ������Ӱ������
			/// </summary>
			public int Combine(" + tn + @"Collection __os)
			{
				int i = 0;
				foreach (" + tn + @" __o in __os)
				{");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.PrimaryKeyEquals(__o); }));
					if (__oo != null)
					{
						if (!__o.Equals(__oo))
						{
							__o.CopyTo(__oo);
							i++;
						}
					}");
				}
				else
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.Equals(__o); }));
					if (__oo != null)
					{
						__o.CopyTo(__oo);
						i++;
					}");
				}
				sb.Append(@"
					else
					{
						this.Add(__o);
						i++;
					}
				}
				return i;
			}
		}
");
				if (isForWCF) sb.Append(@"
		[DataContract]");
				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection_With_Count
		{");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public int Count;");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public " + tn + @"Collection Rows;
		}
");
				sb.Append(@"
		#endregion
");
			}

			sb.Append(@"
		#endregion
");

		#endregion

			#region View

			sb.Append(@"
		#region Views
");

			foreach (View t in uvs)
			{
				List<Column> acs = new List<Column>();
				foreach (Column c in t.Columns) acs.Add(c);

				List<Column> pkcs = Utils.GetPrimaryKeyColumns(t);
				List<Column> ncs = Utils.GetNonPrimaryKeyColumns(t);
				string tn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tn + @"
");

				sb.Append(Utils.GetSummary(t, 2));

				//ȡ����
				Table bt = Utils.GetBaseTable(t);
				if (bt != null)
				{
					//��������ʣ�µ��ֶ�
					List<Column> vcs = new List<Column>();
					foreach (Column c in acs) if (!bt.Columns.Contains(c.Name)) vcs.Add(c);

					if (isForWCF) sb.Append(@"
		[DataContract]");

					sb.Append(@"
		[Serializable]
		public partial class " + tn + @" : " + Utils.GetEscapeName(bt) + @"
		{
			#region ˽�г�Ա
");
					foreach (Column c in vcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
			protected " + typename + @" __________" + cn + @";");
					}
					sb.Append(@"

			#endregion

			#region ���캯��
");
					sb.Append(@"
			public " + tn + @"() : base()
			{
			}
			public " + tn + @"(");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@") : base(");
					for (int i = 0; i < bt.Columns.Count; i++)
					{
						Column c = bt.Columns[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@")
			{");
					foreach (Column c in vcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				__________" + cn + @" = " + cn + ";");
					}
					sb.Append(@"
			}");

					sb.Append(@"

			#endregion
");
					sb.Append(@"

			#region ���Ʒ���

			/// <summary>
			/// ����ǰ�����ֵ���ǵ���һͬ���Ͷ���
			/// </summary>
			public void CopyTo(" + tn + @" __oo)
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
					}
					sb.Append(@"
			}

			/// <summary>
			/// ����һͬ���Ͷ����и���ֵ���ǵ���ǰ����
			/// </summary>
			public void CopyFrom(" + tn + @" __oo)
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				this." + cn + @" = __oo." + cn + @";");
					}
					sb.Append(@"
			}

			/// <summary>
			/// ����һ�ݵ�ǰ�����Ϊһ����ʵ������
			/// </summary>
			public new " + tn + @" Copy()
			{
				" + tn + @" __oo = new " + tn + @"();");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
					}
					sb.Append(@"
				return __oo;
			}

			#endregion

			#region �ȽϷ���

			/// <summary>
			/// �Ƚϵ�ǰ����ʹ�������ֵ�Ƿ����
			/// </summary>
			public bool Equals(" + tn + @" __o)
			{
				if (this == __o) return true;
				if (");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
					");
						if (i > 0) sb.Append(@"&& ");
						sb.Append(@"this." + cn + @" == __o." + cn);
					}
					sb.Append(@"
				) return true;
				return false;
			}

			#endregion
");

					foreach (Column c in vcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(Utils.GetSummary(c, 3));
						if (isForWCF) sb.Append(@"
			[DataMember]");
						sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __________" + cn + @"; }
				set { __________" + cn + @" = value; }
			}");

					}
					sb.Append(@"
		}
");
				}
				else
				{
					if (isForWCF) sb.Append(@"
		[DataContract]");
					sb.Append(@"
		[Serializable]
		public partial class " + tn + @"
		{");
					if (pkcs.Count > 0)
					{
						sb.Append(@"
			#region �������ඨ��

			/// <summary>
			/// ��" + t.Name + @" ����������
			/// </summary>");
						if (isForWCF) sb.Append(@"
			[DataContract]");
						sb.Append(@"
			[Serializable]
			public partial class PrimaryKeys : IEquatable<PrimaryKeys>
			{");
						foreach (Column c in pkcs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
				protected " + typename + @" __________" + cn + @";");
						}
						foreach (Column c in pkcs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(Utils.GetSummary(c, 4));
							if (isForWCF) sb.Append(@"
				[DataMember]");
							sb.Append(@"
				public " + typename + " " + cn + @"
				{
					get { return __________" + cn + @"; }
					set { __________" + cn + @" = value; }
				}");
						}
						sb.Append(@"
				public PrimaryKeys() { }

				public PrimaryKeys(");
						for (int i = 0; i < pkcs.Count; i++)
						{
							Column c = pkcs[i];
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							if (i > 0) sb.Append(@", ");
							sb.Append(typename + " " + cn);
						}
						sb.Append(@")
				{");
						for (int i = 0; i < pkcs.Count; i++)
						{
							Column c = pkcs[i];
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
					__________" + cn + @" = " + cn + @";");
						}
						sb.Append(@"
				}

				#region IEquatable<PrimaryKeys> Members

				public bool Equals(PrimaryKeys other)
				{
					return ");
						for (int i = 0; i < pkcs.Count; i++)
						{
							Column c = pkcs[i];
							string cn = Utils.GetEscapeName(c);
							if (i > 0) sb.Append(@" && ");
							sb.Append(@"__________" + cn + @" == other." + cn);
						}
						sb.Append(@";
				}

				#endregion
			}

			#endregion
");
					}
					if (pkcs.Count > 0)
					{
						sb.Append(@"
			#region ˽�г�Ա����ȡ��������������ֵ�Աȷ���

			protected PrimaryKeys __PKs;
			/// <summary>
			/// ��ȡ��" + t.Name + @" ��������ʵ��
			/// </summary>
			public PrimaryKeys GetPrimaryKeys()
			{
				return __PKs;
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @" other)
			{
				return __PKs.Equals(other.GetPrimaryKeys());
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @".PrimaryKeys other)
			{
				return __PKs.Equals(other);
			}");
					}
					else
					{
						sb.Append(@"
			#region ˽�г�Ա
");
					}
					foreach (Column c in ncs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
			protected " + typename + @" __________" + cn + @";");
					}
					sb.Append(@"

			#endregion
");
					sb.Append(@"
			#region ���캯��
");
					if (pkcs.Count > 0)
					{

						sb.Append(@"
			public " + tn + @"()
			{
				__PKs = new PrimaryKeys();
			}
			public " + tn + @"(PrimaryKeys pk)
			{
				__PKs = pk;
			}");
						sb.Append(@"
			public " + tn + @"(");
						for (int i = 0; i < acs.Count; i++)
						{
							Column c = acs[i];
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							if (i > 0) sb.Append(@", ");
							sb.Append(typename + " " + cn);
						}
						sb.Append(@")
			{
				__PKs = new PrimaryKeys(");
						for (int i = 0; i < pkcs.Count; i++)
						{
							Column c = pkcs[i];
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							if (i > 0) sb.Append(@", ");
							sb.Append(cn);
						}
						sb.Append(@");");
						foreach (Column c in ncs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
				__________" + cn + @" = " + cn + ";");
						}
						sb.Append(@"
			}");
						if (acs.Count > pkcs.Count)
						{
							sb.Append(@"
			public " + tn + @"(PrimaryKeys __pk");
							foreach (Column c in ncs)
							{
								string cn = Utils.GetEscapeName(c);
								string typename;
								if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
								else typename = Utils.GetDataType(c);
								sb.Append(@", " + typename + " " + cn);
							}
							sb.Append(@")
			{
				__PKs = __pk;");
							foreach (Column c in ncs)
							{
								string cn = Utils.GetEscapeName(c);
								string typename;
								if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
								else typename = Utils.GetDataType(c);
								sb.Append(@"
				__________" + cn + @" = " + cn + ";");
							}
							sb.Append(@"
			}");

						}

					}
					else
					{
						sb.Append(@"
			public " + tn + @"()
			{
			}
			public " + tn + @"(");
						for (int i = 0; i < acs.Count; i++)
						{
							Column c = acs[i];
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							if (i > 0) sb.Append(@", ");
							sb.Append(typename + " " + cn);
						}
						sb.Append(@")
			{");
						foreach (Column c in acs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
				__________" + cn + @" = " + cn + ";");
						}
						sb.Append(@"
			}");
					}
					sb.Append(@"

			#endregion
");

					sb.Append(@"

			#region ���Ʒ���

			/// <summary>
			/// ����ǰ�����ֵ���ǵ���һͬ���Ͷ���
			/// </summary>
			public virtual void CopyTo(" + tn + @" __oo)
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
					}
					sb.Append(@"
			}

			/// <summary>
			/// ����һͬ���Ͷ����и���ֵ���ǵ���ǰ����
			/// </summary>
			public virtual void CopyFrom(" + tn + @" __oo)
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				this." + cn + @" = __oo." + cn + @";");
					}
					sb.Append(@"
			}

			/// <summary>
			/// ����һ�ݵ�ǰ�����Ϊһ����ʵ������
			/// </summary>
			public virtual " + tn + @" Copy()
			{
				" + tn + @" __oo = new " + tn + @"();");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
					}
					sb.Append(@"
				return __oo;
			}

			#endregion

			#region �ȽϷ���

			/// <summary>
			/// �Ƚϵ�ǰ����ʹ�������ֵ�Ƿ����
			/// </summary>
			public virtual bool Equals(" + tn + @" __o)
			{
				if (this == __o) return true;
				if (");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(@"
					");
						if (i > 0) sb.Append(@"&& ");
						sb.Append(@"this." + cn + @" == __o." + cn);
					}
					sb.Append(@"
				) return true;
				return false;
			}

			#endregion
");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable && !pkcs.Contains(c)) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(Utils.GetSummary(c, 3));
						if (isForWCF) sb.Append(@"
			[DataMember]");
						if (pkcs.Contains(c))
						{
							sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __PKs." + cn + @"; }
				set { __PKs." + cn + @" = value; }
			}");
						}
						else sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __________" + cn + @"; }
				set { __________" + cn + @" = value; }
			}");
					}

					sb.Append(@"
		}
");
				}

				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection : List<" + tn + @">
		{
			/// <summary>
			/// ֱ�����������ĳԪ����ֵ��������ĳԪ�ص�ʵ��
			/// </summary>
			public " + tn + @" Add(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(typename + " " + cn);
				}
				sb.Append(@")
			{
				" + tn + @" o = new " + tn + @"(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(cn);
				}
				sb.Append(@");
				this.Add(o);
				return o;
			}

			/// <summary>
			/// �ϲ���һ�����������ǰ���ϣ�������ͬ���滻ԭֵ����ͬ�������� ������Ӱ������
			/// </summary>
			public int Combine(" + tn + @"Collection __os)
			{
				int i = 0;
				foreach (" + tn + @" __o in __os)
				{");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.PrimaryKeyEquals(__o); }));
					if (__oo != null)
					{
						if (!__o.Equals(__oo))
						{
							__o.CopyTo(__oo);
							i++;
						}
					}");
				}
				else
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.Equals(__o); }));
					if (__oo != null)
					{
						__o.CopyTo(__oo);
						i++;
					}");
				}
				sb.Append(@"
					else
					{
						this.Add(__o);
						i++;
					}
				}
				return i;
			}
		}
");
				if (isForWCF) sb.Append(@"
		[DataContract]");
				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection_With_Count
		{");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public int Count;");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public " + tn + @"Collection Rows;
		}
");
				sb.Append(@"
		#endregion
");

			}

			sb.Append(@"
		#endregion
");

		#endregion

			#region FunctionTable

			sb.Append(@"
		#region User Defined Function Tables
");

			foreach (UserDefinedFunction t in ufs)
			{
				List<Column> acs = new List<Column>();
				foreach (Column c in t.Columns) acs.Add(c);

				List<Column> pkcs = Utils.GetPrimaryKeyColumns(t);
				List<Column> ncs = Utils.GetNonPrimaryKeyColumns(t);
				string tn = Utils.GetEscapeName(t);

				sb.Append(@"
		#region " + tn + @"
");

				sb.Append(Utils.GetSummary(t, 2));
				if (isForWCF) sb.Append(@"
		[DataContract]");
				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"
		{");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			#region �������ඨ��

			/// <summary>
			/// ��" + t.Name + @" ����������
			/// </summary>");
					if (isForWCF) sb.Append(@"
			[DataContract]");
					sb.Append(@"
			[Serializable]
			public partial class PrimaryKeys : IEquatable<PrimaryKeys>
			{");
					foreach (Column c in pkcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				protected " + typename + @" __________" + cn + @";");
					}
					foreach (Column c in pkcs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(Utils.GetSummary(c, 4));
						if (isForWCF) sb.Append(@"
				[DataMember]");
						sb.Append(@"
				public " + typename + " " + cn + @"
				{
					get { return __________" + cn + @"; }
					set { __________" + cn + @" = value; }
				}");
					}
					sb.Append(@"
				public PrimaryKeys() { }

				public PrimaryKeys(");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
				{");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
					__________" + cn + @" = " + cn + @";");
					}
					sb.Append(@"
				}

				#region IEquatable<PrimaryKeys> Members

				public bool Equals(PrimaryKeys other)
				{
					return ");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@" && ");
						sb.Append(@"__________" + cn + @" == other." + cn);
					}
					sb.Append(@";
				}

				#endregion
			}

			#endregion
");
				}
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			#region ˽�г�Ա����ȡ��������������ֵ�Աȷ���

			protected PrimaryKeys __PKs;
			/// <summary>
			/// ��ȡ��" + t.Name + @" ��������ʵ��
			/// </summary>
			public PrimaryKeys GetPrimaryKeys()
			{
				return __PKs;
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @" other)
			{
				return __PKs.Equals(other.GetPrimaryKeys());
			}
			/// <summary>
			/// �ж�����һ����ʵ���������Ƿ����
			/// </summary>
			public bool PrimaryKeyEquals(" + tn + @".PrimaryKeys other)
			{
				return __PKs.Equals(other);
			}");
				}
				else
				{
					sb.Append(@"
			#region ˽�г�Ա
");
				}
				foreach (Column c in ncs)
				{
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					sb.Append(@"
			protected " + typename + @" __________" + cn + @";");
				}
				sb.Append(@"

			#endregion
");
				sb.Append(@"
			#region ���캯��
");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
			public " + tn + @"()
			{
				__PKs = new PrimaryKeys();
			}
			public " + tn + @"(PrimaryKeys pk)
			{
				__PKs = pk;
			}");
					sb.Append(@"
			public " + tn + @"(");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{
				__PKs = new PrimaryKeys(");
					for (int i = 0; i < pkcs.Count; i++)
					{
						Column c = pkcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@");");
					foreach (Column c in ncs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				__________" + cn + @" = " + cn + ";");
					}
					sb.Append(@"
			}");
					if (acs.Count > pkcs.Count)
					{
						sb.Append(@"
			public " + tn + @"(PrimaryKeys __pk");
						foreach (Column c in ncs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@", " + typename + " " + cn);
						}
						sb.Append(@")
			{
				__PKs = __pk;");
						foreach (Column c in ncs)
						{
							string cn = Utils.GetEscapeName(c);
							string typename;
							if (c.Nullable) typename = Utils.GetNullableDataType(c);
							else typename = Utils.GetDataType(c);
							sb.Append(@"
				__________" + cn + @" = " + cn + ";");
						}
						sb.Append(@"
			}");

					}
				}
				else
				{
					sb.Append(@"
			public " + tn + @"()
			{
			}
			public " + tn + @"(");
					for (int i = 0; i < acs.Count; i++)
					{
						Column c = acs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{");
					foreach (Column c in acs)
					{
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						sb.Append(@"
				__________" + cn + @" = " + cn + ";");
					}
					sb.Append(@"
			}");
				}
				sb.Append(@"

			#endregion
			
			#region ���Ʒ���

			/// <summary>
			/// ����ǰ�����ֵ���ǵ���һͬ���Ͷ���
			/// </summary>
			public virtual void CopyTo(" + tn + @" __oo)
			{");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
				}
				sb.Append(@"
			}

			/// <summary>
			/// ����һͬ���Ͷ����и���ֵ���ǵ���ǰ����
			/// </summary>
			public virtual void CopyFrom(" + tn + @" __oo)
			{");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				this." + cn + @" = __oo." + cn + @";");
				}
				sb.Append(@"
			}

			/// <summary>
			/// ����һ�ݵ�ǰ�����Ϊһ����ʵ������
			/// </summary>
			public virtual " + tn + @" Copy()
			{
				" + tn + @" __oo = new " + tn + @"();");
				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				__oo." + cn + @" = this." + cn + @";");
				}
				sb.Append(@"
				return __oo;
			}

			#endregion

			#region �ȽϷ���

			/// <summary>
			/// �Ƚϵ�ǰ����ʹ�������ֵ�Ƿ����
			/// </summary>
			public virtual bool Equals(" + tn + @" __o)
			{
				if (this == __o) return true;
				if (");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
					");
					if (i > 0) sb.Append(@"&& ");
					sb.Append(@"this." + cn + @" == __o." + cn);
				}
				sb.Append(@"
				) return true;
				return false;
			}

			#endregion
");

				foreach (Column c in acs)
				{
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					sb.Append(Utils.GetSummary(c, 3));
					if (isForWCF) sb.Append(@"
			[DataMember]");
					if (pkcs.Contains(c))
					{
						sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __PKs." + cn + @"; }
				set { __PKs." + cn + @" = value; }
			}");
					}
					else sb.Append(@"
			public " + typename + " " + cn + @"
			{
				get { return __________" + cn + @"; }
				set { __________" + cn + @" = value; }
			}");
				}

				sb.Append(@"
		}
");

				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection : List<" + tn + @">
		{
			/// <summary>
			/// ֱ�����������ĳԪ����ֵ��������ĳԪ�ص�ʵ��
			/// </summary>
			public " + tn + @" Add(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					string typename;
					if (c.Nullable) typename = Utils.GetNullableDataType(c);
					else typename = Utils.GetDataType(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(typename + " " + cn);
				}
				sb.Append(@")
			{
				" + tn + @" o = new " + tn + @"(");
				for (int i = 0; i < acs.Count; i++)
				{
					Column c = acs[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					sb.Append(cn);
				}
				sb.Append(@");
				this.Add(o);
				return o;
			}

			/// <summary>
			/// �ϲ���һ�����������ǰ���ϣ�������ͬ���滻ԭֵ����ͬ�������� ������Ӱ������
			/// </summary>
			public int Combine(" + tn + @"Collection __os)
			{
				int i = 0;
				foreach (" + tn + @" __o in __os)
				{");
				if (pkcs.Count > 0)
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.PrimaryKeyEquals(__o); }));
					if (__oo != null)
					{
						if (!__o.Equals(__oo))
						{
							__o.CopyTo(__oo);
							i++;
						}
					}");
				}
				else
				{
					sb.Append(@"
					" + tn + @" __oo = this.Find(new Predicate<" + tn + @">(delegate(" + tn + @" _o) { return _o.Equals(__o); }));
					if (__oo != null)
					{
						__o.CopyTo(__oo);
						i++;
					}");
				}
				sb.Append(@"
					else
					{
						this.Add(__o);
						i++;
					}
				}
				return i;
			}
		}
");
				if (isForWCF) sb.Append(@"
		[DataContract]");
				sb.Append(@"
		[Serializable]
		public partial class " + tn + @"Collection_With_Count
		{");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public int Count;");
				if (isForWCF) sb.Append(@"
			[DataMember]");
				sb.Append(@"
			
			public " + tn + @"Collection Rows;
		}
");
				sb.Append(@"
		#endregion
");
			}

			sb.Append(@"
		#endregion
");

		#endregion

			#region Footer

			sb.Append(@"
	}
}");
			return sb.ToString();

			#endregion
		}
	}
}
