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
	/// ǿ�������ݼ� DataSet �������� (�� vs ��������)
	/// todo: TableAdapter ������
	/// </summary>
	public static partial class Gen_DS
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <param name="db">��</param>
		/// <param name="ns">�����ռ�</param>
		/// <param name="dsn">DS��</param>
		/// <returns></returns>
		public static string Gen(Database db, string ns, string dsn)
		{
			#region Header

			StringBuilder sb = new StringBuilder();

			List<Table> uts = Utils.GetUserTables(db);
			List<View> uvs = Utils.GetUserViews(db);
			List<UserDefinedFunction> ufs = Utils.GetUserFunctions_TableValue(db);

			#endregion

			#region DataSet

			#region using

			sb.Append(@"using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
");
			#endregion

			#region DataSet class Header

			sb.Append(@"
namespace " + ns + @"
{
	/// <summary>
	/// �����ݿ�ṹ��Ӧ��ǿ�������ݼ���������ͼ����ֵ�����Ķ���
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
	[Serializable()]
	[System.ComponentModel.DesignerCategoryAttribute(""code"")]
	[System.ComponentModel.ToolboxItem(true)]
	[System.Xml.Serialization.XmlSchemaProviderAttribute(""GetTypedDataSetSchema"")]
	[System.Xml.Serialization.XmlRootAttribute(""" + dsn + @""")]
	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.DataSet"")]
	public partial class " + dsn + @" : System.Data.DataSet {
");
			#endregion

			#region static util methods

			sb.Append(@"

		/// <summary>
		/// ���������Ƚ����� Row �Ƿ���ȡ�Ҫ������ r1 ���ڱ�����������ж��塣
		/// </summary>
		public static bool CompareRow(DataRow r1, DataRow r2)
		{
			DataTable t1 = r1.Table, t2 = r2.Table;
			DataColumn[] pk1 = t1.PrimaryKey;
			if (pk1 == null || pk1.Length == 0 || t2.Columns.Count < pk1.Length) return false;
			foreach (DataColumn c1 in pk1)
			{
				DataColumn c2 = t2.Columns[c1.ColumnName];
				if (c2 == null || c1.DataType != c2.DataType || r2.IsNull(c2) || r1[c1] != r2[c2]) return false;
			}
			return true;
		}

		/// <summary>
		/// ���� r �������ֶ��� dt ��� Rows �в��� Row ������
		/// </summary>
		public static DataRow FindRow(DataTable dt, DataRow r)
		{
			if (dt == null || dt.Rows.Count == 0) return null;
			DataTable t = r.Table;
			DataColumn[] pk = t.PrimaryKey;
			if (pk == null || pk.Length == 0 || dt.Columns.Count < pk.Length) return null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow r2 = dt.Rows[i];
				if (CompareRow(r, r2)) return r2;
			}
			return null;
		}

");
			#endregion

			#region private xxxDataTable, DataRelation declare

			foreach (Table t in uts)
			{
				string tbn = Utils.GetEscapeName(t);

				sb.Append(@"
		private " + tbn + @"DataTable table" + tbn + @";");
			}
			foreach (View t in uvs)
			{
				string tbn = Utils.GetEscapeName(t);

				sb.Append(@"
		private " + tbn + @"DataTable table" + tbn + @";");
			}
			foreach (UserDefinedFunction t in ufs)
			{
				string tbn = Utils.GetEscapeName(t);

				sb.Append(@"
		private " + tbn + @"DataTable table" + tbn + @";");
			}

			//todo: relation generate
			//	private System.Data.DataRelation relationFK_����_����;

			#endregion

			#region constructor

			sb.Append(@"
		
		private System.Data.SchemaSerializationMode _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public " + dsn + @"() {
			this.BeginInit();
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			base.Tables.CollectionChanged += schemaChangedHandler;
			base.Relations.CollectionChanged += schemaChangedHandler;
			this.EndInit();
		}
");
			#endregion

			#region XmlSchema

			sb.Append(@"
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected " + dsn + @"(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
				base(info, context, false) {
			if ((this.IsBinarySerialized(info, context) == true)) {
				this.InitVars(false);
				System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
				this.Tables.CollectionChanged += schemaChangedHandler1;
				this.Relations.CollectionChanged += schemaChangedHandler1;
				return;
			}
			string strSchema = ((string)(info.GetValue(""XmlSchema"", typeof(string))));
			if ((this.DetermineSchemaSerializationMode(info, context) == System.Data.SchemaSerializationMode.IncludeSchema)) {
				System.Data.DataSet ds = new System.Data.DataSet();
				ds.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
");

			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			sb.Append(@"
				this.DataSetName = ds.DataSetName;
				this.Prefix = ds.Prefix;
				this.Namespace = ds.Namespace;
				this.Locale = ds.Locale;
				this.CaseSensitive = ds.CaseSensitive;
				this.EnforceConstraints = ds.EnforceConstraints;
				this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
				this.InitVars();
			}
			else {
				this.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
			}
			this.GetSerializationData(info, context);
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			base.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}
		
");

			#endregion

			#region Table, View, TableFunction Get Properties

			foreach (Table t in uts)
			{
				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public " + tbn + @"DataTable " + tbn + @" {
			get {
				return this.table" + tbn + @";
			}
		}");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public " + tbn + @"DataTable " + tbn + @" {
			get {
				return this.table" + tbn + @";
			}
		}");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public " + tbn + @"DataTable " + tbn + @" {
			get {
				return this.table" + tbn + @";
			}
		}");
			}

			#endregion

			#region SchemaSerializationMode

			sb.Append(@"
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.BrowsableAttribute(true)]
		[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
		public override System.Data.SchemaSerializationMode SchemaSerializationMode {
			get {
				return this._schemaSerializationMode;
			}
			set {
				this._schemaSerializationMode = value;
			}
		}
");
			#endregion

			#region Tables, Relations Get Properties

			sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public new System.Data.DataTableCollection Tables {
			get {
				return base.Tables;
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public new System.Data.DataRelationCollection Relations {
			get {
				return base.Relations;
			}
		}
");
			#endregion

			#region InitializeDerivedDataSet

			sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected override void InitializeDerivedDataSet() {
			this.BeginInit();
			this.InitClass();
			this.EndInit();
		}
");
			#endregion

			#region Clone

			sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public override System.Data.DataSet Clone() {
			" + dsn + @" cln = ((" + dsn + @")(base.Clone()));
			cln.InitVars();
			cln.SchemaSerializationMode = this.SchemaSerializationMode;
			return cln;
		}
");
			#endregion

			#region Serialize Flags, ReadXmlSerializable, GetSchemaSerializable

			sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected override bool ShouldSerializeTables() {
			return false;
		}
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected override bool ShouldSerializeRelations() {
			return false;
		}

		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected override void ReadXmlSerializable(System.Xml.XmlReader reader) {
			if ((this.DetermineSchemaSerializationMode(reader) == System.Data.SchemaSerializationMode.IncludeSchema)) {
				this.Reset();
				System.Data.DataSet ds = new System.Data.DataSet();
				ds.ReadXml(reader);");
			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
				if ((ds.Tables[""" + t.Name + @"""] != null)) {
					base.Tables.Add(new " + tbn + @"DataTable(ds.Tables[""" + t.Name + @"""]));
				}");
			}
			sb.Append(@"
				this.DataSetName = ds.DataSetName;
				this.Prefix = ds.Prefix;
				this.Namespace = ds.Namespace;
				this.Locale = ds.Locale;
				this.CaseSensitive = ds.CaseSensitive;
				this.EnforceConstraints = ds.EnforceConstraints;
				this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
				this.InitVars();
			}
			else {
				this.ReadXml(reader);
				this.InitVars();
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			this.WriteXmlSchema(new System.Xml.XmlTextWriter(stream, null));
			stream.Position = 0;
			return System.Xml.Schema.XmlSchema.Read(new System.Xml.XmlTextReader(stream), null);
		}
");
			#endregion

			#region InitVars

			sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		internal void InitVars() {
			this.InitVars(true);
		}
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		internal void InitVars(bool initTable) {");
			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = ((" + tbn + @"DataTable)(base.Tables[""" + t.Name + @"""]));
			if ((initTable == true)) {
				if ((this.table" + tbn + @" != null)) {
					this.table" + tbn + @".InitVars();
				}
			}");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = ((" + tbn + @"DataTable)(base.Tables[""" + t.Name + @"""]));
			if ((initTable == true)) {
				if ((this.table" + tbn + @" != null)) {
					this.table" + tbn + @".InitVars();
				}
			}");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = ((" + tbn + @"DataTable)(base.Tables[""" + t.Name + @"""]));
			if ((initTable == true)) {
				if ((this.table" + tbn + @" != null)) {
					this.table" + tbn + @".InitVars();
				}
			}");
			}
			//todo: relation
			//		this.relationFK_����_���� = this.Relations[""FK_����_����""];
			sb.Append(@"
		}
");

			#endregion

			#region InitClass

			//������Ĭ������ CaseSensitive = true ��ֹ�����ݿ�����������ͬЧ��

			sb.Append(@"
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		private void InitClass() {
			this.CaseSensitive = true;
			this.DataSetName = """ + dsn + @""";
			this.Prefix = """";
			this.Namespace = ""http://tempuri.org/" + dsn + @".xsd"";
			this.EnforceConstraints = true;
			this.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;");
			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = new " + tbn + @"DataTable();
			base.Tables.Add(this.table" + tbn + @");");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = new " + tbn + @"DataTable();
			base.Tables.Add(this.table" + tbn + @");");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
			this.table" + tbn + @" = new " + tbn + @"DataTable();
			base.Tables.Add(this.table" + tbn + @");");
			}
			//todo: relation
			/*
		this.relationFK_����_���� = new System.Data.DataRelation(""FK_����_����"", new System.Data.DataColumn[] {
		this.table����.������Column}, new System.Data.DataColumn[] {
		this.table����.��������Column}, false);
		this.Relations.Add(this.relationFK_����_����);
			 */
			sb.Append(@"
		}
");

			#endregion

			#region private ShouldSerialize

			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		private bool ShouldSerialize" + tbn + @"() {
			return false;
		}");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		private bool ShouldSerialize" + tbn + @"() {
			return false;
		}");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		private bool ShouldSerialize" + tbn + @"() {
			return false;
		}");
			}
			#endregion

			#region SchemaChanged, GetTypedDataSetSchema

			sb.Append(@"
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
			if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
				this.InitVars();
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public static System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet xs) {
			" + dsn + @" ds = new " + dsn + @"();
			System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
			System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
			xs.Add(ds.GetSchemaSerializable());
			System.Xml.Schema.XmlSchemaAny any = new System.Xml.Schema.XmlSchemaAny();
			any.Namespace = ds.Namespace;
			sequence.Items.Add(any);
			type.Particle = sequence;
			return type;
		}
	");
			#endregion

			#region delegate RowChangeEventHandler

			foreach (Table t in uts)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		public delegate void " + tbn + @"RowChangeEventHandler(object sender, " + tbn + @"RowChangeEvent e);");
			}
			foreach (View t in uvs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		public delegate void " + tbn + @"RowChangeEventHandler(object sender, " + tbn + @"RowChangeEvent e);");
			}
			foreach (UserDefinedFunction t in ufs)
			{

				string tbn = Utils.GetEscapeName(t);
				sb.Append(@"
		public delegate void " + tbn + @"RowChangeEventHandler(object sender, " + tbn + @"RowChangeEvent e);");
			}
			sb.Append(@"

");
			#endregion

			#region Strong Type DataView, DataTable, Row, RowChangeEvent declares

			#region Tables

			foreach (Table t in uts)
			{
				string tbn = Utils.GetEscapeName(t);
				List<Column> wcs = Utils.GetWriteableColumns(t);


				#region Strong Type DataView

				//����ǿ���͵� DataView �Ķ���

				sb.Append(@"
		/// <summary>
		/// Strong Type DataView
		/// </summary>
		public partial class " + tbn + @"DataView : System.Data.DataView {
			public " + tbn + @"DataView(" + tbn + @"DataTable dt)
				: base(dt)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter)
				: base(dt, filter, """", DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort)
				: base(dt, filter, sort, DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort, DataViewRowState dvs)
				: base(dt, filter, sort, dvs)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), dvs)
			{

			}

			public new " + tbn + @"Row this[int index]
			{
				get { return (" + tbn + @"Row)base[index].Row; }
			}

			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return (" + tbn + @"DataTable)this.Table; }
			}
		}
");
				#endregion

				#region xxxDataTable Class Body

				sb.Append(Utils.GetSummary(t, 2));

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		[System.Serializable()]
		[System.Xml.Serialization.XmlSchemaProviderAttribute(""GetTypedTableSchema"")]
		public partial class " + tbn + @"DataTable : System.Data.DataTable, System.Collections.IEnumerable {
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			private System.Data.DataColumn column" + cn + @";");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"DataTable() {
				this.CaseSensitive = true;
				this.TableName = """ + t.Name + @""";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"DataTable(System.Data.DataTable table) {
				this.TableName = table.TableName;
				if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
					this.CaseSensitive = table.CaseSensitive;
				}
				if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
					this.Locale = table.Locale;
				}
				if ((table.Namespace != table.DataSet.Namespace)) {
					this.Namespace = table.Namespace;
				}
				this.Prefix = table.Prefix;
				this.MinimumCapacity = table.MinimumCapacity;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected " + tbn + @"DataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
					base(info, context) {
				this.InitVars();
			}
			
");

				//��ȡһ�����е������д������

				sb.Append(@"
			/// <summary>
			/// Get HasError Rows List
			/// </summary>
			public List<" + tbn + @"Row> ErrorRows
			{
				get {
					List<" + tbn + @"Row> rs = new List<" + tbn + @"Row>();
					foreach (" + tbn + @"Row r in this)
					{
						if (!string.IsNullOrEmpty(r.RowError)) rs.Add(r);
					}
					return rs;
				}
			}
");
				//���һ�ű��еĴ����еĴ���

				sb.Append(@"
			/// <summary>
			/// Clear Errors
			/// </summary>
			public void ClearErrors()
			{
				foreach (" + tbn + @"Row r in this)
				{
					if (!string.IsNullOrEmpty(r.RowError)) r.ClearErrors();
				}
			}
");

				//Ϊ������һ���µ���ͼ

				sb.Append(@"
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView()
			{
				return new " + tbn + @"DataView(this);
			}
			/// <summary>
			/// Make a new DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter)
			{
				return new " + tbn + @"DataView(this, filter);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort)
			{
				return new " + tbn + @"DataView(this, filter, sort);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sort, dvs);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending, dvs);
			}
");


				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));


					sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataColumn " + cn + @"Column {
				get {
					return this.column" + cn + @";
				}
			}");
				}

				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[System.ComponentModel.Browsable(false)]
			public int Count {
				get {
					return this.Rows.Count;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row this[int index] {
				get {
					return ((" + tbn + @"Row)(this.Rows[index]));
				}
			}
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanging;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanged;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleting;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleted;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Add" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Add(row);
			}

			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Add" + tbn + @"Row(");

				for (int i = 0; i < wcs.Count; i++)
				{
					Column c = wcs[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						sb.Append(Utils.GetNullableDataType(c) + " " + cn);
					}
					else sb.Append(Utils.GetDataType(c) + " " + cn);
				}
				sb.Append(@") {
				" + tbn + @"Row row" + tbn + @"Row = ((" + tbn + @"Row)(this.NewRow()));
				row" + tbn + @"Row.ItemArray = new object[] {");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (!wcs.Contains(c)) sb.Append("null");
					else sb.Append(cn);
					if (i < t.Columns.Count - 1) sb.Append(@", ");
				}
				sb.Append(@"};
				this.Rows.Add(row" + tbn + @"Row);
				return row" + tbn + @"Row;
			}");

				List<Column> pks = Utils.GetPrimaryKeyColumns(t);
				if (pks.Count > 0)
				{
					sb.Append(@"	 
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row FindBy");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(cn);
					}
					sb.Append(@"(");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(Utils.GetDataType(c) + " " + cn);
					}
					sb.Append(@") {
				return ((" + tbn + @"Row)(this.Rows.Find(new object[] {
						");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@"})));
			}");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public virtual System.Collections.IEnumerator GetEnumerator() {
				return this.Rows.GetEnumerator();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public override System.Data.DataTable Clone() {
				" + tbn + @"DataTable cln = ((" + tbn + @"DataTable)(base.Clone()));
				cln.InitVars();
				return cln;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataTable CreateInstance() {
				return new " + tbn + @"DataTable();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal void InitVars() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			this.column" + cn + @" = base.Columns[""" + c.Name + @"""];");
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			private void InitClass() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				this.column" + cn + @" = new System.Data.DataColumn(""" + c.Name + @""", typeof(" + Utils.GetDataType(c) + @"), null, System.Data.MappingType.Element);");
					if (c.Name != cn)
					{
						sb.Append(@"
                this.column" + cn + @".ExtendedProperties.Add(""Generator_ColumnPropNameInRow"", """ + cn + @""");
                this.column" + cn + @".ExtendedProperties.Add(""Generator_UserColumnName"", """ + c.Name + @""");");
					}
					sb.Append(@"
				base.Columns.Add(this.column" + cn + @");");
				}
				if (pks.Count > 0)
				{
					sb.Append(@"
				this.Constraints.Add(new System.Data.UniqueConstraint(""Constraint1"", new System.Data.DataColumn[] {
							");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(@"this.column" + cn);
					}
					sb.Append(@"}, true));");
				}
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Identity || c.Computed)
					{
						sb.Append(@"
				this.column" + cn + @".ReadOnly = true;");
					}
					if (c.Identity)
					{
						string seed = c.IdentitySeed.ToString();
						string incr = c.IdentityIncrement == 0 ? "1" : c.IdentityIncrement.ToString();
						sb.Append(@"
				this.column" + cn + @".AutoIncrement = true;
				this.column" + cn + @".AutoIncrementSeed = " + seed + @";
				this.column" + cn + @".AutoIncrementStep = " + incr + @";");
					}
					if (!c.Nullable)
					{
						sb.Append(@"
				this.column" + cn + @".AllowDBNull = false;");
					}
					if (pks.Count == 1 && c.InPrimaryKey || c.Identity || c.RowGuidCol)	//todo: check unique index
					{
						sb.Append(@"
				this.column" + cn + @".Unique = true;");
					}
					if (Utils.CheckIsStringType(c))
					{
						sb.Append(@"
				this.column" + cn + @".MaxLength = " + Utils.GetDbTypeLength(c) + @";");
					}
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row New" + tbn + @"Row() {
				return ((" + tbn + @"Row)(this.NewRow()));
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
				return new " + tbn + @"Row(builder);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Type GetRowType() {
				return typeof(" + tbn + @"Row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanged(e);
				if ((this." + tbn + @"RowChanged != null)) {
					this." + tbn + @"RowChanged(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanging(e);
				if ((this." + tbn + @"RowChanging != null)) {
					this." + tbn + @"RowChanging(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleted(e);
				if ((this." + tbn + @"RowDeleted != null)) {
					this." + tbn + @"RowDeleted(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleting(e);
				if ((this." + tbn + @"RowDeleting != null)) {
					this." + tbn + @"RowDeleting(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Remove" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Remove(row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
				System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
				System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
				" + dsn + @" ds = new " + dsn + @"();
				xs.Add(ds.GetSchemaSerializable());
				System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
				any1.Namespace = ""http://www.w3.org/2001/XMLSchema"";
				any1.MinOccurs = new decimal(0);
				any1.MaxOccurs = decimal.MaxValue;
				any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any1);
				System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
				any2.Namespace = ""urn:schemas-microsoft-com:xml-diffgram-v1"";
				any2.MinOccurs = new decimal(1);
				any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any2);
				System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute1.Name = ""namespace"";
				attribute1.FixedValue = ds.Namespace;
				type.Attributes.Add(attribute1);
				System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute2.Name = ""tableTypeName"";
				attribute2.FixedValue = """ + tbn + @"DataTable"";
				type.Attributes.Add(attribute2);
				type.Particle = sequence;
				return type;
			}
		}
");
				#endregion

				#region xxxRow Class Body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public partial class " + tbn + @"Row : System.Data.DataRow {
");

				//Ϊ Row ����һ�����Ʒ���

				sb.Append(@"
			public void CopyTo(DataRow r)
			{
				CopyTo(r, true);
			}
			public void CopyTo(DataRow r, bool isOverrideReadonly)
			{
				DataTable t = r.Table;
				foreach(DataColumn dc in this.Table.Columns)
				{
					DataColumn dc2 = t.Columns[dc.ColumnName];
					if (dc2 != null && dc.DataType == dc2.DataType)
					{
						if (dc2.ReadOnly)
						{
							if (isOverrideReadonly)
							{
								dc2.ReadOnly = false;
								if (dc.AllowDBNull)
								{
									if (IsNull(dc)) r[dc2] = Convert.DBNull;
								}
								else r[dc2] = this[dc];
								dc2.ReadOnly = true;
							}
						}
						else
						{
							if (dc.AllowDBNull)
							{
								if (IsNull(dc)) r[dc2] = Convert.DBNull;
							}
							else r[dc2] = this[dc];
						}
					}
				}
			}
");

				//Ϊ Row ����һ�������������ݲ����صķ���

				sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance()
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				" + tbn + @"Row r = dt.New" + tbn + @"Row();
				return r;
			}
");

				//Ϊ Row ����һ�������������ݲ����ص����ط���

				sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance(DataRow r)
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				dt.ImportRow(r);
				return dt[0];
			}
");

				//Ϊ Row ����һ�������������ݲ����ص����ط���

				if (wcs.Count > 0)
				{

					sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance(");
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				dt.Add" + tbn + @"Row(");
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@");
				return dt[0];
			}");
				}


				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			/// <summary>
			/// Reference to Parent Table : " + tbn + @"DataTable
			/// </summary>
			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return table" + tbn + @"; }
			}
");

				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			public object[] ItemArray_Original
			{
				get
				{
					if (HasVersion(DataRowVersion.Original))
					{
						object[] objArray = new object[this.Table.Columns.Count];
						for (int i = 0; i < objArray.Length; i++)
						{
							objArray[i] = this[i, DataRowVersion.Original];
						}
						return objArray;
					}
					else return ItemArray;
				}
			}

 ");


				//Ϊ Row ���ӿ����ֶ�ֻ�������ķ���

				sb.Append(@"
			/// <summary>
			/// Enable / Disable Column's ReadOnly Properties
			/// </summary>
			public void EnforceReadOnly(bool b)
			{");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Identity || c.Computed)
					{
						sb.Append(@"
				this.table" + tbn + @"." + cn + @"Column.ReadOnly = b;");
					}
				}
				sb.Append(@"
			}

 ");


				//Ϊ Row ���ӱȽ�����ֵ�Ƿ���ȵķ���
				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(" + tbn + @"Row r)
			{
				DataColumn[] pk = this.Table.PrimaryKey;
				foreach(DataColumn dc in pk)
				{
					if (this[dc] != r[dc]) return false;
				}
				return true;
			}

			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(DataRow r)
			{
				return " + dsn + @".CompareRow(this, r);
			}
 ");
				}
				else sb.Append(@"
			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(DataRow r)
			{
				return " + dsn + @".CompareRow(r, this);
			}
 ");

				sb.Append(@"			
			private " + tbn + @"DataTable table" + tbn + @";
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"Row(System.Data.DataRowBuilder rb) : 
					base(rb) {
				this.table" + tbn + @" = ((" + tbn + @"DataTable)(this.Table));
			}
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));

					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					try {
						return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
					}
					catch (System.InvalidCastException e) {
						throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
					}
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				try {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
				}
				catch (System.InvalidCastException e) {
					throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
				}
			}");
					}
					else
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
			}");
					}
				}

				//todo: relation
				/*
					 
						 [System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public " + tbn + @"Row " + tbn + @"RowParent {
			get {
				return ((" + tbn + @"Row)(this.GetParentRow(this.Table.ParentRelations[""FK_" + tbn + @"_" + tbn + @"""])));
			}
			set {
				this.SetParentRow(value, this.Table.ParentRelations[""FK_" + tbn + @"_" + tbn + @"""]);
			}
		}

				 */

				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public bool Is" + cn + @"Null() {
				return this.IsNull(this.table" + tbn + @"." + cn + @"Column);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Set" + cn + @"Null() {
				this[this.table" + tbn + @"." + cn + @"Column] = System.Convert.DBNull;
			}
");
					}
				}

				//todo: relation
				/*
					 
						 [System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public " + tbn + @"Row[] Get" + tbn + @"Rows() {
			return ((" + tbn + @"Row[])(base.GetChildRows(this.Table.ChildRelations[""FK_" + tbn + @"_" + tbn + @"""])));
		}

				 */

				sb.Append(@"
			
		}
");
				#endregion

				#region xxxRowChangeEvent class body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public class " + tbn + @"RowChangeEvent : System.EventArgs {
			
			private " + tbn + @"Row eventRow;
			
			private System.Data.DataRowAction eventAction;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"RowChangeEvent(" + tbn + @"Row row, System.Data.DataRowAction action) {
				this.eventRow = row;
				this.eventAction = action;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Row {
				get {
					return this.eventRow;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataRowAction Action {
				get {
					return this.eventAction;
				}
			}
		}
");
				#endregion
			}

			#endregion

			#region Views

			foreach (View t in uvs)
			{
				string tbn = Utils.GetEscapeName(t);
				List<Column> wcs = Utils.GetWriteableColumns(t);

				#region Strong Type DataView

				//����ǿ���͵� DataView �Ķ���

				sb.Append(@"
		/// <summary>
		/// Strong Type DataView
		/// </summary>
		public partial class " + tbn + @"DataView : System.Data.DataView {
			public " + tbn + @"DataView(" + tbn + @"DataTable dt)
				: base(dt)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter)
				: base(dt, filter, """", DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort)
				: base(dt, filter, sort, DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort, DataViewRowState dvs)
				: base(dt, filter, sort, dvs)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), dvs)
			{

			}

			public new " + tbn + @"Row this[int index]
			{
				get { return (" + tbn + @"Row)base[index].Row; }
			}

			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return (" + tbn + @"DataTable)this.Table; }
			}
		}
");
				#endregion

				#region xxxDataTable Class Body

				sb.Append(Utils.GetSummary(t, 2));

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		[System.Serializable()]
		[System.Xml.Serialization.XmlSchemaProviderAttribute(""GetTypedTableSchema"")]
		public partial class " + tbn + @"DataTable : System.Data.DataTable, System.Collections.IEnumerable {
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			private System.Data.DataColumn column" + cn + @";");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"DataTable() {
				this.CaseSensitive = true;
				this.TableName = """ + t.Name + @""";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"DataTable(System.Data.DataTable table) {
				this.TableName = table.TableName;
				if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
					this.CaseSensitive = table.CaseSensitive;
				}
				if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
					this.Locale = table.Locale;
				}
				if ((table.Namespace != table.DataSet.Namespace)) {
					this.Namespace = table.Namespace;
				}
				this.Prefix = table.Prefix;
				this.MinimumCapacity = table.MinimumCapacity;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected " + tbn + @"DataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
					base(info, context) {
				this.InitVars();
			}
			
");

				//��ȡһ�����е������д������

				sb.Append(@"
			/// <summary>
			/// Get HasError Rows List
			/// </summary>
			public List<" + tbn + @"Row> ErrorRows
			{
				get {
					List<" + tbn + @"Row> rs = new List<" + tbn + @"Row>();
					foreach (" + tbn + @"Row r in this)
					{
						if (!string.IsNullOrEmpty(r.RowError)) rs.Add(r);
					}
					return rs;
				}
			}
");
				//���һ�ű��еĴ����еĴ���

				sb.Append(@"
			/// <summary>
			/// Clear Errors
			/// </summary>
			public void ClearErrors()
			{
				foreach (" + tbn + @"Row r in this)
				{
					if (!string.IsNullOrEmpty(r.RowError)) r.ClearErrors();
				}
			}
");

				//Ϊ������һ���µ���ͼ

				sb.Append(@"
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView()
			{
				return new " + tbn + @"DataView(this);
			}
			/// <summary>
			/// Make a new DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter)
			{
				return new " + tbn + @"DataView(this, filter);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort)
			{
				return new " + tbn + @"DataView(this, filter, sort);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sort, dvs);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending, dvs);
			}
");


				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));


					sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataColumn " + cn + @"Column {
				get {
					return this.column" + cn + @";
				}
			}");
				}

				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[System.ComponentModel.Browsable(false)]
			public int Count {
				get {
					return this.Rows.Count;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row this[int index] {
				get {
					return ((" + tbn + @"Row)(this.Rows[index]));
				}
			}
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanging;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanged;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleting;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleted;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Add" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Add(row);
			}

			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Add" + tbn + @"Row(");

				for (int i = 0; i < wcs.Count; i++)
				{
					Column c = wcs[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						sb.Append(Utils.GetNullableDataType(c) + " " + cn);
					}
					else sb.Append(Utils.GetDataType(c) + " " + cn);
				}
				sb.Append(@") {
				" + tbn + @"Row row" + tbn + @"Row = ((" + tbn + @"Row)(this.NewRow()));
				row" + tbn + @"Row.ItemArray = new object[] {");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (!wcs.Contains(c)) sb.Append("null");
					else sb.Append(cn);
					if (i < t.Columns.Count - 1) sb.Append(@", ");
				}
				sb.Append(@"};
				this.Rows.Add(row" + tbn + @"Row);
				return row" + tbn + @"Row;
			}");

				List<Column> pks = Utils.GetPrimaryKeyColumns(t);
				if (pks.Count > 0)
				{
					sb.Append(@"	 
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row FindBy");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						sb.Append(cn);
					}
					sb.Append(@"(");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(Utils.GetDataType(c) + " " + cn);
					}
					sb.Append(@") {
				return ((" + tbn + @"Row)(this.Rows.Find(new object[] {
						");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@"})));
			}");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public virtual System.Collections.IEnumerator GetEnumerator() {
				return this.Rows.GetEnumerator();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public override System.Data.DataTable Clone() {
				" + tbn + @"DataTable cln = ((" + tbn + @"DataTable)(base.Clone()));
				cln.InitVars();
				return cln;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataTable CreateInstance() {
				return new " + tbn + @"DataTable();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal void InitVars() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			this.column" + cn + @" = base.Columns[""" + c.Name + @"""];");
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			private void InitClass() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				this.column" + cn + @" = new System.Data.DataColumn(""" + c.Name + @""", typeof(" + Utils.GetDataType(c) + @"), null, System.Data.MappingType.Element);");
					if (c.Name != cn)
					{
						sb.Append(@"
                this.column" + cn + @".ExtendedProperties.Add(""Generator_ColumnPropNameInRow"", """ + cn + @""");
                this.column" + cn + @".ExtendedProperties.Add(""Generator_UserColumnName"", """ + c.Name + @""");");
					}
					sb.Append(@"
				base.Columns.Add(this.column" + cn + @");");
				}
				if (pks.Count > 0)
				{
					sb.Append(@"
				this.Constraints.Add(new System.Data.UniqueConstraint(""Constraint1"", new System.Data.DataColumn[] {
							");
					for (int i = 0; i < pks.Count; i++)
					{
						Column c = pks[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(@"this.column" + cn);
					}
					sb.Append(@"}, true));");
				}
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Identity || c.Computed)
					{
						sb.Append(@"
				this.column" + cn + @".ReadOnly = true;");
					}
					if (c.Identity)
					{
						string seed = c.IdentitySeed.ToString();
                        string incr = c.IdentityIncrement == 0 ? "1" : c.IdentityIncrement.ToString();
						sb.Append(@"
				this.column" + cn + @".AutoIncrement = true;
				this.column" + cn + @".AutoIncrementSeed = " + seed + @";
				this.column" + cn + @".AutoIncrementStep = " + incr + @";");
					}
					if (!c.Nullable)
					{
						sb.Append(@"
				this.column" + cn + @".AllowDBNull = false;");
					}
					if (pks.Count == 1 && c.InPrimaryKey || c.Identity || c.RowGuidCol)	//todo: check unique index
					{
						sb.Append(@"
				this.column" + cn + @".Unique = true;");
					}
					if (Utils.CheckIsStringType(c))
					{
						sb.Append(@"
				this.column" + cn + @".MaxLength = " + Utils.GetDbTypeLength(c) + @";");
					}
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row New" + tbn + @"Row() {
				return ((" + tbn + @"Row)(this.NewRow()));
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
				return new " + tbn + @"Row(builder);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Type GetRowType() {
				return typeof(" + tbn + @"Row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanged(e);
				if ((this." + tbn + @"RowChanged != null)) {
					this." + tbn + @"RowChanged(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanging(e);
				if ((this." + tbn + @"RowChanging != null)) {
					this." + tbn + @"RowChanging(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleted(e);
				if ((this." + tbn + @"RowDeleted != null)) {
					this." + tbn + @"RowDeleted(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleting(e);
				if ((this." + tbn + @"RowDeleting != null)) {
					this." + tbn + @"RowDeleting(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Remove" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Remove(row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
				System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
				System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
				" + dsn + @" ds = new " + dsn + @"();
				xs.Add(ds.GetSchemaSerializable());
				System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
				any1.Namespace = ""http://www.w3.org/2001/XMLSchema"";
				any1.MinOccurs = new decimal(0);
				any1.MaxOccurs = decimal.MaxValue;
				any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any1);
				System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
				any2.Namespace = ""urn:schemas-microsoft-com:xml-diffgram-v1"";
				any2.MinOccurs = new decimal(1);
				any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any2);
				System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute1.Name = ""namespace"";
				attribute1.FixedValue = ds.Namespace;
				type.Attributes.Add(attribute1);
				System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute2.Name = ""tableTypeName"";
				attribute2.FixedValue = """ + tbn + @"DataTable"";
				type.Attributes.Add(attribute2);
				type.Particle = sequence;
				return type;
			}
		}
");
				#endregion

				#region xxxRow Class Body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public partial class " + tbn + @"Row : System.Data.DataRow {
");

				//Ϊ Row ����һ�����Ʒ���

				sb.Append(@"
			public void CopyTo(DataRow r)
			{
				CopyTo(r, true);
			}
			public void CopyTo(DataRow r, bool isOverrideReadonly)
			{
				DataTable t = r.Table;
				foreach(DataColumn dc in this.Table.Columns)
				{
					DataColumn dc2 = t.Columns[dc.ColumnName];
					if (dc2 != null && dc.DataType == dc2.DataType)
					{
						if (dc2.ReadOnly)
						{
							if (isOverrideReadonly)
							{
								dc2.ReadOnly = false;
								if (dc.AllowDBNull)
								{
									if (IsNull(dc)) r[dc2] = Convert.DBNull;
								}
								else r[dc2] = this[dc];
								dc2.ReadOnly = true;
							}
						}
						else
						{
							if (dc.AllowDBNull)
							{
								if (IsNull(dc)) r[dc2] = Convert.DBNull;
							}
							else r[dc2] = this[dc];
						}
					}
				}
			}
");

				//Ϊ Row ����һ�������������ݲ����صķ���

				sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance()
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				" + tbn + @"Row r = dt.New" + tbn + @"Row();
				return r;
			}
");

				//Ϊ Row ����һ�������������ݲ����ص����ط���

				sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance(DataRow r)
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				dt.ImportRow(r);
				return dt[0];
			}
");

				//Ϊ Row ����һ�������������ݲ����ص����ط���

				if (wcs.Count > 0)
				{

					sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance(");
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
						string typename;
						if (c.Nullable) typename = Utils.GetNullableDataType(c);
						else typename = Utils.GetDataType(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(typename + " " + cn);
					}
					sb.Append(@")
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				dt.Add" + tbn + @"Row(");
					for (int i = 0; i < wcs.Count; i++)
					{
						Column c = wcs[i];
						string cn = Utils.GetEscapeName(c);
						if (i > 0) sb.Append(@", ");
						sb.Append(cn);
					}
					sb.Append(@");
				return dt[0];
			}");
				}


				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			/// <summary>
			/// Reference to Parent Table : " + tbn + @"DataTable
			/// </summary>
			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return table" + tbn + @"; }
			}
");

				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			public object[] ItemArray_Original
			{
				get
				{
					if (HasVersion(DataRowVersion.Original))
					{
						object[] objArray = new object[this.Table.Columns.Count];
						for (int i = 0; i < objArray.Length; i++)
						{
							objArray[i] = this[i, DataRowVersion.Original];
						}
						return objArray;
					}
					else return ItemArray;
				}
			}

 ");


				//Ϊ Row ���ӿ����ֶ�ֻ�������ķ���

				sb.Append(@"
			/// <summary>
			/// Enable / Disable Column's ReadOnly Properties
			/// </summary>
			public void EnforceReadOnly(bool b)
			{");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Identity || c.Computed)
					{
						sb.Append(@"
				this.table" + tbn + @"." + cn + @"Column.ReadOnly = b;");
					}
				}
				sb.Append(@"
			}

 ");


				//Ϊ Row ���ӱȽ�����ֵ�Ƿ���ȵķ���
				if (pks.Count > 0)
				{
					sb.Append(@"
			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(" + tbn + @"Row r)
			{
				DataColumn[] pk = this.Table.PrimaryKey;
				foreach(DataColumn dc in pk)
				{
					if (this[dc] != r[dc]) return false;
				}
				return true;
			}

			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(DataRow r)
			{
				return " + dsn + @".CompareRow(this, r);
			}
 ");
				}
				else sb.Append(@"
			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(DataRow r)
			{
				return " + dsn + @".CompareRow(r, this);
			}
 ");

				sb.Append(@"			
			private " + tbn + @"DataTable table" + tbn + @";
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"Row(System.Data.DataRowBuilder rb) : 
					base(rb) {
				this.table" + tbn + @" = ((" + tbn + @"DataTable)(this.Table));
			}
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));

					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					try {
						return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
					}
					catch (System.InvalidCastException e) {
						throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
					}
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				try {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
				}
				catch (System.InvalidCastException e) {
					throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
				}
			}");
					}
					else
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
			}");
					}
				}

				//todo: relation
				/*
					 
						 [System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public " + tbn + @"Row " + tbn + @"RowParent {
			get {
				return ((" + tbn + @"Row)(this.GetParentRow(this.Table.ParentRelations[""FK_" + tbn + @"_" + tbn + @"""])));
			}
			set {
				this.SetParentRow(value, this.Table.ParentRelations[""FK_" + tbn + @"_" + tbn + @"""]);
			}
		}

				 */

				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public bool Is" + cn + @"Null() {
				return this.IsNull(this.table" + tbn + @"." + cn + @"Column);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Set" + cn + @"Null() {
				this[this.table" + tbn + @"." + cn + @"Column] = System.Convert.DBNull;
			}
");
					}
				}

				//todo: relation
				/*
					 
						 [System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public " + tbn + @"Row[] Get" + tbn + @"Rows() {
			return ((" + tbn + @"Row[])(base.GetChildRows(this.Table.ChildRelations[""FK_" + tbn + @"_" + tbn + @"""])));
		}

				 */

				sb.Append(@"
			
		}
");
				#endregion

				#region xxxRowChangeEvent class body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public class " + tbn + @"RowChangeEvent : System.EventArgs {
			
			private " + tbn + @"Row eventRow;
			
			private System.Data.DataRowAction eventAction;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"RowChangeEvent(" + tbn + @"Row row, System.Data.DataRowAction action) {
				this.eventRow = row;
				this.eventAction = action;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Row {
				get {
					return this.eventRow;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataRowAction Action {
				get {
					return this.eventAction;
				}
			}
		}
");
				#endregion
			}

			#endregion

			#region Functions

			foreach (UserDefinedFunction t in ufs)
			{
				string tbn = Utils.GetEscapeName(t);

				#region Strong Type DataView

				sb.Append(@"
		/// <summary>
		/// Strong Type DataView
		/// </summary>
		public partial class " + tbn + @"DataView : System.Data.DataView {
			public " + tbn + @"DataView(" + tbn + @"DataTable dt)
				: base(dt)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter)
				: base(dt, filter, """", DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort)
				: base(dt, filter, sort, DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), DataViewRowState.CurrentRows)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, string sort, DataViewRowState dvs)
				: base(dt, filter, sort, dvs)
			{

			}
			public " + tbn + @"DataView(" + tbn + @"DataTable dt, string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
				: base(dt, filter, ""["" + sortColumn.ColumnName.Replace(""]"", ""\\]"") + ""]"" + (isAscending ? """" : "" DESC""), dvs)
			{

			}

			public new " + tbn + @"Row this[int index]
			{
				get { return (" + tbn + @"Row)base[index].Row; }
			}

			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return (" + tbn + @"DataTable)this.Table; }
			}
		}
");
				#endregion

				#region xxxDataTable class body

				sb.Append(Utils.GetSummary(t, 2));

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		[System.Serializable()]
		[System.Xml.Serialization.XmlSchemaProviderAttribute(""GetTypedTableSchema"")]
		public partial class " + tbn + @"DataTable : System.Data.DataTable, System.Collections.IEnumerable {
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			private System.Data.DataColumn column" + cn + @";");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"DataTable() {
				this.CaseSensitive = true;
				this.TableName = """ + t.Name + @""";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"DataTable(System.Data.DataTable table) {
				this.TableName = table.TableName;
				if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
					this.CaseSensitive = table.CaseSensitive;
				}
				if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
					this.Locale = table.Locale;
				}
				if ((table.Namespace != table.DataSet.Namespace)) {
					this.Namespace = table.Namespace;
				}
				this.Prefix = table.Prefix;
				this.MinimumCapacity = table.MinimumCapacity;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected " + tbn + @"DataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
					base(info, context) {
				this.InitVars();
			}
			
");

				//��ȡһ�����е������д������

				sb.Append(@"
			/// <summary>
			/// Get HasError Rows List
			/// </summary>
			public List<" + tbn + @"Row> ErrorRows
			{
				get {
					List<" + tbn + @"Row> rs = new List<" + tbn + @"Row>();
					foreach (" + tbn + @"Row r in this)
					{
						if (!string.IsNullOrEmpty(r.RowError)) rs.Add(r);
					}
					return rs;
				}
			}
");
				//���һ�ű��еĴ����еĴ���

				sb.Append(@"
			/// <summary>
			/// Clear Errors
			/// </summary>
			public void ClearErrors()
			{
				foreach (" + tbn + @"Row r in this)
				{
					if (!string.IsNullOrEmpty(r.RowError)) r.ClearErrors();
				}
			}
");

				//Ϊ������һ���µ���ͼ

				sb.Append(@"
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView()
			{
				return new " + tbn + @"DataView(this);
			}
			/// <summary>
			/// Make a new DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter)
			{
				return new " + tbn + @"DataView(this, filter);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort)
			{
				return new " + tbn + @"DataView(this, filter, sort);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, string sort, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sort, dvs);
			}
			/// <summary>
			/// Make a new " + tbn + @"DataView for this table
			/// </summary>
			public " + tbn + @"DataView NewDataView(string filter, DataColumn sortColumn, bool isAscending, DataViewRowState dvs)
			{
				return new " + tbn + @"DataView(this, filter, sortColumn, isAscending, dvs);
			}
");

				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));

					sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataColumn " + cn + @"Column {
				get {
					return this.column" + cn + @";
				}
			}");
				}
				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[System.ComponentModel.Browsable(false)]
			public int Count {
				get {
					return this.Rows.Count;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row this[int index] {
				get {
					return ((" + tbn + @"Row)(this.Rows[index]));
				}
			}
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanging;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowChanged;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleting;
			
			public event " + tbn + @"RowChangeEventHandler " + tbn + @"RowDeleted;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Add" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Add(row);
			}

			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Add" + tbn + @"Row(");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					if (i > 0) sb.Append(@", ");
					if (c.Nullable)
					{
						sb.Append(Utils.GetNullableDataType(c) + " " + cn);
					}
					else sb.Append(Utils.GetDataType(c) + " " + cn);
				}
				sb.Append(@") {
				" + tbn + @"Row row" + tbn + @"Row = ((" + tbn + @"Row)(this.NewRow()));
				row" + tbn + @"Row.ItemArray = new object[] {");
				for (int i = 0; i < t.Columns.Count; i++)
				{
					Column c = t.Columns[i];
					string cn = Utils.GetEscapeName(c);
					sb.Append(cn);
					if (i < t.Columns.Count - 1) sb.Append(@",");
				}
				sb.Append(@"};
				this.Rows.Add(row" + tbn + @"Row);
				return row" + tbn + @"Row;
			}");

				sb.Append(@"
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public virtual System.Collections.IEnumerator GetEnumerator() {
				return this.Rows.GetEnumerator();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public override System.Data.DataTable Clone() {
				" + tbn + @"DataTable cln = ((" + tbn + @"DataTable)(base.Clone()));
				cln.InitVars();
				return cln;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataTable CreateInstance() {
				return new " + tbn + @"DataTable();
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal void InitVars() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
			this.column" + cn + @" = base.Columns[""" + c.Name + @"""];");
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			private void InitClass() {");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(@"
				this.column" + cn + @" = new System.Data.DataColumn(""" + c.Name + @""", typeof(" + Utils.GetDataType(c) + @"), null, System.Data.MappingType.Element);");
					if (c.Name != cn)
					{
						sb.Append(@"
                this.column" + cn + @".ExtendedProperties.Add(""Generator_ColumnPropNameInRow"", """ + cn + @""");
                this.column" + cn + @".ExtendedProperties.Add(""Generator_UserColumnName"", """ + c.Name + @""");");
					}
					sb.Append(@"
				base.Columns.Add(this.column" + cn + @");");
				}
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (!c.Nullable)
					{
						sb.Append(@"
				this.column" + cn + @".AllowDBNull = false;");
					}
					if (c.Identity || c.RowGuidCol)	//todo: check unique index
					{
						sb.Append(@"
				this.column" + cn + @".Unique = true;");
					}
					if (Utils.CheckIsStringType(c))
					{
						sb.Append(@"
				this.column" + cn + @".MaxLength = " + Utils.GetDbTypeLength(c) + @";");
					}
				}
				sb.Append(@"
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row New" + tbn + @"Row() {
				return ((" + tbn + @"Row)(this.NewRow()));
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
				return new " + tbn + @"Row(builder);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override System.Type GetRowType() {
				return typeof(" + tbn + @"Row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanged(e);
				if ((this." + tbn + @"RowChanged != null)) {
					this." + tbn + @"RowChanged(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
				base.OnRowChanging(e);
				if ((this." + tbn + @"RowChanging != null)) {
					this." + tbn + @"RowChanging(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleted(e);
				if ((this." + tbn + @"RowDeleted != null)) {
					this." + tbn + @"RowDeleted(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
				base.OnRowDeleting(e);
				if ((this." + tbn + @"RowDeleting != null)) {
					this." + tbn + @"RowDeleting(this, new " + tbn + @"RowChangeEvent(((" + tbn + @"Row)(e.Row)), e.Action));
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Remove" + tbn + @"Row(" + tbn + @"Row row) {
				this.Rows.Remove(row);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
				System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
				System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
				" + dsn + @" ds = new " + dsn + @"();
				xs.Add(ds.GetSchemaSerializable());
				System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
				any1.Namespace = ""http://www.w3.org/2001/XMLSchema"";
				any1.MinOccurs = new decimal(0);
				any1.MaxOccurs = decimal.MaxValue;
				any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any1);
				System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
				any2.Namespace = ""urn:schemas-microsoft-com:xml-diffgram-v1"";
				any2.MinOccurs = new decimal(1);
				any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
				sequence.Items.Add(any2);
				System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute1.Name = ""namespace"";
				attribute1.FixedValue = ds.Namespace;
				type.Attributes.Add(attribute1);
				System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
				attribute2.Name = ""tableTypeName"";
				attribute2.FixedValue = """ + tbn + @"DataTable"";
				type.Attributes.Add(attribute2);
				type.Particle = sequence;
				return type;
			}
		}
");
				#endregion

				#region xxxRow class body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public partial class " + tbn + @"Row : System.Data.DataRow {
");

				//Ϊ Row ����һ�����Ʒ���

				sb.Append(@"
			public void CopyTo(DataRow r)
			{
				CopyTo(r, true);
			}
			public void CopyTo(DataRow r, bool isOverrideReadonly)
			{
				DataTable t = r.Table;
				foreach(DataColumn dc in this.Table.Columns)
				{
					DataColumn dc2 = t.Columns[dc.ColumnName];
					if (dc2 != null && dc.DataType == dc2.DataType)
					{
						if (dc2.ReadOnly)
						{
							if (isOverrideReadonly)
							{
								dc2.ReadOnly = false;
								if (dc.AllowDBNull)
								{
									if (IsNull(dc)) r[dc2] = Convert.DBNull;
								}
								else r[dc2] = this[dc];
								dc2.ReadOnly = true;
							}
						}
						else
						{
							if (dc.AllowDBNull)
							{
								if (IsNull(dc)) r[dc2] = Convert.DBNull;
							}
							else r[dc2] = this[dc];
						}
					}
				}
			}
");


				//Ϊ Row ����һ�������������ݲ����صķ���

				sb.Append(@"
			/// <summary>
			/// Create 1 Row in new Table (not added) and return. Copy method: LoadData(thisRow.ItemArray, ..)
			/// </summary>
			public static " + tbn + @"Row CreateInstance()
			{
				" + tbn + @"DataTable dt = new " + tbn + @"DataTable();
				" + tbn + @"Row r = dt.New" + tbn + @"Row();
				return r;
			}
");

				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			/// <summary>
			/// Reference to Parent Table : " + tbn + @"DataTable
			/// </summary>
			public " + tbn + @"DataTable " + tbn + @"DataTable
			{
				get { return table" + tbn + @"; }
			}
");

				//Ϊ Row ����ֱ����������ǿ���ͱ�ķ���

				sb.Append(@"
			public object[] ItemArray_Original
			{
				get
				{
					if (HasVersion(DataRowVersion.Original))
					{
						object[] objArray = new object[this.Table.Columns.Count];
						for (int i = 0; i < objArray.Length; i++)
						{
							objArray[i] = this[i, DataRowVersion.Original];
						}
						return objArray;
					}
					else return ItemArray;
				}
			}

 ");


				//Ϊ Row ���ӿ����ֶ�ֻ�������ķ���

				sb.Append(@"
			/// <summary>
			/// Enable / Disable Column's ReadOnly Properties
			/// </summary>
			public void EnforceReadOnly(bool b)
			{");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Identity || c.Computed)
					{
						sb.Append(@"
				this.table" + tbn + @"." + cn + @"Column.ReadOnly = b;");
					}
				}
				sb.Append(@"
			}

 ");


				//Ϊ Row ���ӱȽ�����ֵ�Ƿ���ȵķ���

				sb.Append(@"
			/// <summary>
			/// Compare Row By Primary Key Column
			/// </summary>
			public bool Compare(DataRow r)
			{
				return " + dsn + @".CompareRow(r, this);
			}
 ");




				sb.Append(@"
			private " + tbn + @"DataTable table" + tbn + @";
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			internal " + tbn + @"Row(System.Data.DataRowBuilder rb) : 
					base(rb) {
				this.table" + tbn + @" = ((" + tbn + @"DataTable)(this.Table));
			}
		");
				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					sb.Append(Utils.GetSummary(c, 3));

					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					try {
						return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
					}
					catch (System.InvalidCastException e) {
						throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
					}
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				try {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
				}
				catch (System.InvalidCastException e) {
					throw new System.Data.StrongTypingException(""The value for column \'" + c.Name + @"\' in table \'" + tbn + @"\' is DBNull."", e);
				}
			}");
					}
					else
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + Utils.GetDataType(c) + @" " + cn + @" {
				get {
					return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column]));
				}
				set {
					this[this.table" + tbn + @"." + cn + @"Column] = value;
				}
			}

			/// <summary>
			/// ȡ�ֶ� " + cn + @" ��ԭʼֵ������еĻ���
			/// </summary>
			public " + Utils.GetDataType(c) + @" GetOriginalValue_" + cn + @"() {
				return ((" + Utils.GetDataType(c) + @")(this[this.table" + tbn + @"." + cn + @"Column, DataRowVersion.Original]));
			}");
					}
				}


				foreach (Column c in t.Columns)
				{
					string cn = Utils.GetEscapeName(c);
					if (c.Nullable)
					{
						sb.Append(@"
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public bool Is" + cn + @"Null() {
				return this.IsNull(this.table" + tbn + @"." + cn + @"Column);
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public void Set" + cn + @"Null() {
				this[this.table" + tbn + @"." + cn + @"Column] = System.Convert.DBNull;
			}
");
					}
				}

				sb.Append(@"
			
		}
");
				#endregion

				#region xxxRowChangeEvent class body

				sb.Append(@"
		[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
		public class " + tbn + @"RowChangeEvent : System.EventArgs {
			
			private " + tbn + @"Row eventRow;
			
			private System.Data.DataRowAction eventAction;
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"RowChangeEvent(" + tbn + @"Row row, System.Data.DataRowAction action) {
				this.eventRow = row;
				this.eventAction = action;
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public " + tbn + @"Row Row {
				get {
					return this.eventRow;
				}
			}
			
			[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			public System.Data.DataRowAction Action {
				get {
					return this.eventAction;
				}
			}
		}
");
				#endregion
			}
			#endregion

			#endregion

			#region DataSet class Footer

			sb.Append(@"
	}");
			#endregion

			#endregion

			#region TableAdapter

			//			foreach (Table t in uts)
			//			{
			//				sb.Append(@"
			//[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Data.Design.TypedDataSetGenerator"", ""2.0.0.0"")]
			//[System.ComponentModel.DesignerCategoryAttribute(""code"")]
			//[System.ComponentModel.ToolboxItem(true)]
			//[System.ComponentModel.DataObjectAttribute(true)]
			//[System.ComponentModel.DesignerAttribute(""Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner"" +
			//	"", Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"")]
			//[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//public partial class " + tbn + @"TableAdapter : System.ComponentModel.Component {
			//	
			//	private System.Data.SqlClient.SqlDataAdapter _adapter;
			//	
			//	private System.Data.SqlClient.SqlConnection _connection;
			//	
			//	private System.Data.SqlClient.SqlCommand[] _commandCollection;
			//	
			//	private bool _clearBeforeFill;
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	public " + tbn + @"TableAdapter() {
			//		this.ClearBeforeFill = true;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	private System.Data.SqlClient.SqlDataAdapter Adapter {
			//		get {
			//			if ((this._adapter == null)) {
			//				this.InitAdapter();
			//			}
			//			return this._adapter;
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	internal System.Data.SqlClient.SqlConnection Connection {
			//		get {
			//			if ((this._connection == null)) {
			//				this.InitConnection();
			//			}
			//			return this._connection;
			//		}
			//		set {
			//			this._connection = value;
			//			if ((this.Adapter.InsertCommand != null)) {
			//				this.Adapter.InsertCommand.Connection = value;
			//			}
			//			if ((this.Adapter.DeleteCommand != null)) {
			//				this.Adapter.DeleteCommand.Connection = value;
			//			}
			//			if ((this.Adapter.UpdateCommand != null)) {
			//				this.Adapter.UpdateCommand.Connection = value;
			//			}
			//			for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1)) {
			//				if ((this.CommandCollection[i] != null)) {
			//					((System.Data.SqlClient.SqlCommand)(this.CommandCollection[i])).Connection = value;
			//				}
			//			}
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	protected System.Data.SqlClient.SqlCommand[] CommandCollection {
			//		get {
			//			if ((this._commandCollection == null)) {
			//				this.InitCommandCollection();
			//			}
			//			return this._commandCollection;
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	public bool ClearBeforeFill {
			//		get {
			//			return this._clearBeforeFill;
			//		}
			//		set {
			//			this._clearBeforeFill = value;
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	private void InitAdapter() {
			//		this._adapter = new System.Data.SqlClient.SqlDataAdapter();
			//		System.Data.Common.DataTableMapping tableMapping = new System.Data.Common.DataTableMapping();
			//		tableMapping.SourceTable = ""Table"";
			//		tableMapping.DataSetTable = """ + t.Name + @""";");
			//				foreach (Column c in t.Columns)
			//				{
			//					sb.Append(@"
			//		tableMapping.ColumnMappings.Add(""" + c.Name + @""", """ + c.Name + @""");");
			//				}

			//				//todo: find delete insert update SP
			//				sb.Append(@"
			//		this._adapter.TableMappings.Add(tableMapping);
			//		this._adapter.DeleteCommand = new System.Data.SqlClient.SqlCommand();
			//		this._adapter.DeleteCommand.Connection = this.Connection;
			//		this._adapter.DeleteCommand.CommandText = ""DELETE FROM [dbo].[����] WHERE (([������] = @Original_������) AND ((@IsNull_�������� = 1 AND "" +
			//			""[��������] IS NULL) OR ([��������] = @Original_��������)) AND ([����] = @Original_����))"";
			//		this._adapter.DeleteCommand.CommandType = System.Data.CommandType.Text;
			//		this._adapter.DeleteCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""������"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//		this._adapter.DeleteCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@IsNull_��������"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Original, true, null, """", """", """"));
			//		this._adapter.DeleteCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_��������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//		this._adapter.DeleteCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_����"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""����"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//		this._adapter.InsertCommand = new System.Data.SqlClient.SqlCommand();
			//		this._adapter.InsertCommand.Connection = this.Connection;
			//		this._adapter.InsertCommand.CommandText = ""INSERT INTO [dbo].[����] ([������], [��������], [��ͼ����], [����]) VALUES (@������, @��������, @��ͼ����, "" +
			//			""@����);\r\nSELECT ������, ��������, ��ͼ����, ���� FROM ���� WHERE (������ = @������)"";
			//		this._adapter.InsertCommand.CommandType = System.Data.CommandType.Text;
			//		this._adapter.InsertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""������"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.InsertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@��������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.InsertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@��ͼ����"", System.Data.SqlDbType.VarBinary, 0, System.Data.ParameterDirection.Input, 0, 0, ""��ͼ����"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.InsertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@����"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""����"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.UpdateCommand = new System.Data.SqlClient.SqlCommand();
			//		this._adapter.UpdateCommand.Connection = this.Connection;
			//		this._adapter.UpdateCommand.CommandText = @""UPDATE [dbo].[����] SET [������] = @������, [��������] = @��������, [��ͼ����] = @��ͼ����, [����] = @���� WHERE (([������] = @Original_������) AND ((@IsNull_�������� = 1 AND [��������] IS NULL) OR ([��������] = @Original_��������)) AND ([����] = @Original_����));
			//SELECT ������, ��������, ��ͼ����, ���� FROM ���� WHERE (������ = @������)"";
			//		this._adapter.UpdateCommand.CommandType = System.Data.CommandType.Text;
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""������"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@��������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@��ͼ����"", System.Data.SqlDbType.VarBinary, 0, System.Data.ParameterDirection.Input, 0, 0, ""��ͼ����"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@����"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""����"", System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""������"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@IsNull_��������"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Original, true, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_��������"", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, 0, 0, ""��������"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//		this._adapter.UpdateCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Original_����"", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, 0, 0, ""����"", System.Data.DataRowVersion.Original, false, null, """", """", """"));
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	private void InitConnection() {
			//		this._connection = new System.Data.SqlClient.SqlConnection();
			//		this._connection.ConnectionString = global::EntLibTest.Properties.Settings.Default.testConnectionString;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	private void InitCommandCollection() {
			//		this._commandCollection = new System.Data.SqlClient.SqlCommand[3];
			//		this._commandCollection[0] = new System.Data.SqlClient.SqlCommand();
			//		this._commandCollection[0].Connection = this.Connection;
			//		this._commandCollection[0].CommandText = ""SELECT ������, ��������, ��ͼ����, ���� FROM dbo.����"";
			//		this._commandCollection[0].CommandType = System.Data.CommandType.Text;
			//		this._commandCollection[1] = new System.Data.SqlClient.SqlCommand();
			//		this._commandCollection[1].Connection = this.Connection;
			//		this._commandCollection[1].CommandText = ""dbo.usp_����_SelectAll"";
			//		this._commandCollection[1].CommandType = System.Data.CommandType.StoredProcedure;
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@RETURN_VALUE"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Keyword"", System.Data.SqlDbType.NVarChar, 2147483647, System.Data.ParameterDirection.Input, 0, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@SortExpression"", System.Data.SqlDbType.NVarChar, 2147483647, System.Data.ParameterDirection.Input, 0, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@SortDirection"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@PageSize"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@StartRowIndex"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[1].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@Count"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.InputOutput, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[2] = new System.Data.SqlClient.SqlCommand();
			//		this._commandCollection[2].Connection = this.Connection;
			//		this._commandCollection[2].CommandText = ""dbo.usp_����_Select"";
			//		this._commandCollection[2].CommandType = System.Data.CommandType.StoredProcedure;
			//		this._commandCollection[2].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@RETURN_VALUE"", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//		this._commandCollection[2].Parameters.Add(new System.Data.SqlClient.SqlParameter(""@������"", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, 0, 0, null, System.Data.DataRowVersion.Current, false, null, """", """", """"));
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Fill, true)]
			//	public virtual int Fill(DataSet2.����DataTable dataTable) {
			//		this.Adapter.SelectCommand = this.CommandCollection[0];
			//		if ((this.ClearBeforeFill == true)) {
			//			dataTable.Clear();
			//		}
			//		int returnValue = this.Adapter.Fill(dataTable);
			//		return returnValue;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
			//	public virtual DataSet2.����DataTable GetData() {
			//		this.Adapter.SelectCommand = this.CommandCollection[0];
			//		DataSet2.����DataTable dataTable = new DataSet2.����DataTable();
			//		this.Adapter.Fill(dataTable);
			//		return dataTable;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Fill, false)]
			//	public virtual int FillByKeyword(DataSet2.����DataTable dataTable, string Keyword, string SortExpression, System.Nullable<int> SortDirection, System.Nullable<int> PageSize, System.Nullable<int> StartRowIndex, ref System.Nullable<int> Count) {
			//		this.Adapter.SelectCommand = this.CommandCollection[1];
			//		if ((Keyword == null)) {
			//			this.Adapter.SelectCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[1].Value = ((string)(Keyword));
			//		}
			//		if ((SortExpression == null)) {
			//			this.Adapter.SelectCommand.Parameters[2].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[2].Value = ((string)(SortExpression));
			//		}
			//		if ((SortDirection.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[3].Value = ((int)(SortDirection.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[3].Value = System.DBNull.Value;
			//		}
			//		if ((PageSize.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[4].Value = ((int)(PageSize.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[4].Value = System.DBNull.Value;
			//		}
			//		if ((StartRowIndex.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[5].Value = ((int)(StartRowIndex.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[5].Value = System.DBNull.Value;
			//		}
			//		if ((Count.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[6].Value = ((int)(Count.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[6].Value = System.DBNull.Value;
			//		}
			//		if ((this.ClearBeforeFill == true)) {
			//			dataTable.Clear();
			//		}
			//		int returnValue = this.Adapter.Fill(dataTable);
			//		if (((this.Adapter.SelectCommand.Parameters[6].Value == null) 
			//					|| (this.Adapter.SelectCommand.Parameters[6].Value.GetType() == typeof(System.DBNull)))) {
			//			Count = new System.Nullable<int>();
			//		}
			//		else {
			//			Count = new System.Nullable<int>(((int)(this.Adapter.SelectCommand.Parameters[6].Value)));
			//		}
			//		return returnValue;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
			//	public virtual DataSet2.����DataTable GetDataByKeyword(string Keyword, string SortExpression, System.Nullable<int> SortDirection, System.Nullable<int> PageSize, System.Nullable<int> StartRowIndex, ref System.Nullable<int> Count) {
			//		this.Adapter.SelectCommand = this.CommandCollection[1];
			//		if ((Keyword == null)) {
			//			this.Adapter.SelectCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[1].Value = ((string)(Keyword));
			//		}
			//		if ((SortExpression == null)) {
			//			this.Adapter.SelectCommand.Parameters[2].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[2].Value = ((string)(SortExpression));
			//		}
			//		if ((SortDirection.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[3].Value = ((int)(SortDirection.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[3].Value = System.DBNull.Value;
			//		}
			//		if ((PageSize.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[4].Value = ((int)(PageSize.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[4].Value = System.DBNull.Value;
			//		}
			//		if ((StartRowIndex.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[5].Value = ((int)(StartRowIndex.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[5].Value = System.DBNull.Value;
			//		}
			//		if ((Count.HasValue == true)) {
			//			this.Adapter.SelectCommand.Parameters[6].Value = ((int)(Count.Value));
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[6].Value = System.DBNull.Value;
			//		}
			//		DataSet2.����DataTable dataTable = new DataSet2.����DataTable();
			//		this.Adapter.Fill(dataTable);
			//		if (((this.Adapter.SelectCommand.Parameters[6].Value == null) 
			//					|| (this.Adapter.SelectCommand.Parameters[6].Value.GetType() == typeof(System.DBNull)))) {
			//			Count = new System.Nullable<int>();
			//		}
			//		else {
			//			Count = new System.Nullable<int>(((int)(this.Adapter.SelectCommand.Parameters[6].Value)));
			//		}
			//		return dataTable;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Fill, false)]
			//	public virtual int FillByPK(DataSet2.����DataTable dataTable, string ������) {
			//		this.Adapter.SelectCommand = this.CommandCollection[2];
			//		if ((������ == null)) {
			//			this.Adapter.SelectCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[1].Value = ((string)(������));
			//		}
			//		if ((this.ClearBeforeFill == true)) {
			//			dataTable.Clear();
			//		}
			//		int returnValue = this.Adapter.Fill(dataTable);
			//		return returnValue;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
			//	public virtual DataSet2.����DataTable GetDataByPK(string ������) {
			//		this.Adapter.SelectCommand = this.CommandCollection[2];
			//		if ((������ == null)) {
			//			this.Adapter.SelectCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.SelectCommand.Parameters[1].Value = ((string)(������));
			//		}
			//		DataSet2.����DataTable dataTable = new DataSet2.����DataTable();
			//		this.Adapter.Fill(dataTable);
			//		return dataTable;
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	public virtual int Update(DataSet2.����DataTable dataTable) {
			//		return this.Adapter.Update(dataTable);
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	public virtual int Update(DataSet2 dataSet) {
			//		return this.Adapter.Update(dataSet, ""����"");
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	public virtual int Update(System.Data.DataRow dataRow) {
			//		return this.Adapter.Update(new System.Data.DataRow[] {
			//					dataRow});
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	public virtual int Update(System.Data.DataRow[] dataRows) {
			//		return this.Adapter.Update(dataRows);
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
			//	public virtual int Delete(string Original_������, string Original_��������, int Original_����) {
			//		if ((Original_������ == null)) {
			//			throw new System.ArgumentNullException(""Original_������"");
			//		}
			//		else {
			//			this.Adapter.DeleteCommand.Parameters[0].Value = ((string)(Original_������));
			//		}
			//		if ((Original_�������� == null)) {
			//			this.Adapter.DeleteCommand.Parameters[1].Value = ((object)(1));
			//			this.Adapter.DeleteCommand.Parameters[2].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.DeleteCommand.Parameters[1].Value = ((object)(0));
			//			this.Adapter.DeleteCommand.Parameters[2].Value = ((string)(Original_��������));
			//		}
			//		this.Adapter.DeleteCommand.Parameters[3].Value = ((int)(Original_����));
			//		System.Data.ConnectionState previousConnectionState = this.Adapter.DeleteCommand.Connection.State;
			//		if (((this.Adapter.DeleteCommand.Connection.State & System.Data.ConnectionState.Open) 
			//					!= System.Data.ConnectionState.Open)) {
			//			this.Adapter.DeleteCommand.Connection.Open();
			//		}
			//		try {
			//			int returnValue = this.Adapter.DeleteCommand.ExecuteNonQuery();
			//			return returnValue;
			//		}
			//		finally {
			//			if ((previousConnectionState == System.Data.ConnectionState.Closed)) {
			//				this.Adapter.DeleteCommand.Connection.Close();
			//			}
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
			//	public virtual int Insert(string ������, string ��������, byte[] ��ͼ����, int ����) {
			//		if ((������ == null)) {
			//			throw new System.ArgumentNullException(""������"");
			//		}
			//		else {
			//			this.Adapter.InsertCommand.Parameters[0].Value = ((string)(������));
			//		}
			//		if ((�������� == null)) {
			//			this.Adapter.InsertCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.InsertCommand.Parameters[1].Value = ((string)(��������));
			//		}
			//		if ((��ͼ���� == null)) {
			//			this.Adapter.InsertCommand.Parameters[2].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.InsertCommand.Parameters[2].Value = ((byte[])(��ͼ����));
			//		}
			//		this.Adapter.InsertCommand.Parameters[3].Value = ((int)(����));
			//		System.Data.ConnectionState previousConnectionState = this.Adapter.InsertCommand.Connection.State;
			//		if (((this.Adapter.InsertCommand.Connection.State & System.Data.ConnectionState.Open) 
			//					!= System.Data.ConnectionState.Open)) {
			//			this.Adapter.InsertCommand.Connection.Open();
			//		}
			//		try {
			//			int returnValue = this.Adapter.InsertCommand.ExecuteNonQuery();
			//			return returnValue;
			//		}
			//		finally {
			//			if ((previousConnectionState == System.Data.ConnectionState.Closed)) {
			//				this.Adapter.InsertCommand.Connection.Close();
			//			}
			//		}
			//	}
			//	
			//	[System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	[System.ComponentModel.Design.HelpKeywordAttribute(""vs.data.TableAdapter"")]
			//	[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
			//	public virtual int Update(string ������, string ��������, byte[] ��ͼ����, int ����, string Original_������, string Original_��������, int Original_����) {
			//		if ((������ == null)) {
			//			throw new System.ArgumentNullException(""������"");
			//		}
			//		else {
			//			this.Adapter.UpdateCommand.Parameters[0].Value = ((string)(������));
			//		}
			//		if ((�������� == null)) {
			//			this.Adapter.UpdateCommand.Parameters[1].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.UpdateCommand.Parameters[1].Value = ((string)(��������));
			//		}
			//		if ((��ͼ���� == null)) {
			//			this.Adapter.UpdateCommand.Parameters[2].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.UpdateCommand.Parameters[2].Value = ((byte[])(��ͼ����));
			//		}
			//		this.Adapter.UpdateCommand.Parameters[3].Value = ((int)(����));
			//		if ((Original_������ == null)) {
			//			throw new System.ArgumentNullException(""Original_������"");
			//		}
			//		else {
			//			this.Adapter.UpdateCommand.Parameters[4].Value = ((string)(Original_������));
			//		}
			//		if ((Original_�������� == null)) {
			//			this.Adapter.UpdateCommand.Parameters[5].Value = ((object)(1));
			//			this.Adapter.UpdateCommand.Parameters[6].Value = System.DBNull.Value;
			//		}
			//		else {
			//			this.Adapter.UpdateCommand.Parameters[5].Value = ((object)(0));
			//			this.Adapter.UpdateCommand.Parameters[6].Value = ((string)(Original_��������));
			//		}
			//		this.Adapter.UpdateCommand.Parameters[7].Value = ((int)(Original_����));
			//		System.Data.ConnectionState previousConnectionState = this.Adapter.UpdateCommand.Connection.State;
			//		if (((this.Adapter.UpdateCommand.Connection.State & System.Data.ConnectionState.Open) 
			//					!= System.Data.ConnectionState.Open)) {
			//			this.Adapter.UpdateCommand.Connection.Open();
			//		}
			//		try {
			//			int returnValue = this.Adapter.UpdateCommand.ExecuteNonQuery();
			//			return returnValue;
			//		}
			//		finally {
			//			if ((previousConnectionState == System.Data.ConnectionState.Closed)) {
			//				this.Adapter.UpdateCommand.Connection.Close();
			//			}
			//		}
			//	}
			//}");
			//			}

			#endregion

			#region Footer

			sb.Append(@"
}
");
			return sb.ToString();

			#endregion
		}
	}
}
