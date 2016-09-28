using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
    public class Gen_Table_UserControl_Complex:IGenComponent
    {
        #region Init

        string ns = "";
        public Gen_Table_UserControl_Complex()
        {
           // this.ns = ns;
            this._properties.Add(GenProperties.Name, "Gen_Table_UserControl_Complex");
            this._properties.Add(GenProperties.Caption, "UserControl");
            this._properties.Add(GenProperties.Group, "SilverLight");
            this._properties.Add(GenProperties.Tips, "为 Table 生成 SilverLight 的数据的相关UserControl");
            this._properties.Add(GenProperties.IsEnabled, false);
        }

		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.Table; }
		}

		#endregion

        #region Misc

        Dictionary<GenProperties, object> _properties = new Dictionary<GenProperties, object>();
        public Dictionary<GenProperties, object> Properties
        {
            get
            {
                return this._properties;
            }
        }

        public event System.ComponentModel.CancelEventHandler OnProcessing;

        private Server _server;
        public Server Server
        {
            set { _server = value; }
        }

        private Database _db;
        public Database Database
        {
            set { _db = value; }
        }

        #endregion

        public bool Validate(params object[] sqlElements)
        {
            Table t = (Table)sqlElements[0];

            return Utils.GetPrimaryKeyColumns(t).Count > 0;
        }

        public GenResult Gen(params object[] sqlElements)
        {
            GenResult gr;

           // gr = new GenResult(GenResultTypes.Files);
            //gr.Files = new List<KeyValuePair<string, byte[]>>();

            Table t = (Table)sqlElements[0];

            #region

            StringBuilder sb = new StringBuilder();

            #endregion


            #region Gen NameSpace

            sb.Remove(0, sb.Length);

            sb.Append(@"<UserControl x:Name =""" + t.Name + @"""
            x:Class=""" + @"Test" + @"." + t.Name + @"""
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
         	xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data""
	        xmlns:primitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
            xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
            xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
            xmlns:theming=""clr-namespace:Microsoft.Windows.Controls.Theming;assembly=Microsoft.Windows.Controls.Theming""
            xmlns:basics=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls""
            xmlns:blacklight=""clr-namespace:Blacklight.Silverlight.Controls;assembly=Blacklight.Silverlight.Controls""
            xmlns:telerik=""clr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Navigation""
            mc:Ignorable=""d"">");

            #endregion

            #region Gen XAML

            #region Root Grid Start
            sb.Append(@"
           <Grid Margin=""0,0,0,0"" x:Name=""Root"" Height=""Auto"" Width=""Auto"">
            <Grid.RowDefinitions>
              <RowDefinition Height =""Auto""/>
              <RowDefinition Height=""Auto"" />
              <RowDefinition Height=""Auto"" />
            </Grid.RowDefinitions>");
            #endregion

            #region Bord DataGrid
            sb.Append(@"
            <!--DataGrid Bord--> 
            <Border Grid.Row=""0"" x:Name =""_border_Grid"">
            </Border>");
            #endregion

            #region Bord Detail
            sb.Append(@"
            <!--Detail Bord -->
            <Border Grid.Row=""1"">
               <Grid>
                 <Grid.ColumnDefinitions>
                    <ColumnDefinition Width=""*"" />
                    <ColumnDefinition Width=""auto"" />
                 </Grid.ColumnDefinitions>
                 <Grid.RowDefinitions>");
            int count = t.Columns.Count/2 +1;
            for (int i = 0; i <count; i++)
            {
                sb.Append(@"
                  <RowDefinition Height =""Auto""/> ");
            }
            sb.Append(@"
                </Grid.RowDefinitions>");
            int rowindex = 0;
            foreach (Column c in t.Columns)
            {
                string cn = Utils.GetEscapeName(c);
                string caption = Utils.GetCaption(c);
                if (Utils.CheckIsStringType(c) || Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
                {
                    sb.Append(@"
                       <StackPanel Margin=""0,5,5,0"" Orientation=""Horizontal"" Grid.ColumnSpan=""1"" Grid.RowSpan=""1"" Grid.Row="""+ rowindex/2+@""" Grid.Column="""+rowindex%2+@""" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					      <TextBlock Height=""Auto"" Width=""60"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                          <TextBox Height=""Auto"" Width=""150"" x:Name=""" + cn + @""" TextWrapping=""Wrap""/>
                       </StackPanel>"); 
                }
                else if (Utils.CheckIsBooleanType(c))
                {
                    sb.Append(@"
                       <StackPanel Orientation=""Horizontal"" Grid.ColumnSpan=""1"" Grid.RowSpan=""1"" Grid.Row=""" + rowindex/2 + @""" Grid.Column=""" + rowindex% 2 + @""" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					     <TextBlock Height=""Auto"" Width=""60"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                         <CheckBox Height=""Auto"" Width=""Auto"" x:Name=""" + cn + @"""/>
                       </StackPanel>"); 
                }
                else if (Utils.CheckIsBinaryType(c))
                {
                    // todo
                }
                rowindex ++;
            }
            sb.Append(@"</Grid>
             </Border>");
            #endregion
            #region DragDockPancel 
            sb.Append(@"
            <Border Grid.Row=""2"">
                <blacklight:DragDockPanelHost x:Namet= ""_dragDockPanel"" />
            </Border>");
            #endregion
            #region Root Grid End
            sb.Append(@"
            </Grid>
     </UserControl>");
            #endregion

            #endregion

            #region Gen CS
            StringBuilder sb_cs = new StringBuilder(@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Net;
            using System.Windows;
            using System.Windows.Controls;
            using System.Windows.Documents;
            using System.Windows.Input;
            using System.Windows.Media;
            using System.Windows.Media.Animation;
            using System.Windows.Shapes;
            using Management.Controls;
            using Management.Controls.Windows;
            using Microsoft.Windows.Controls.Theming;

            namespace " + ns + @"
            {");
            #region 构超函数
            sb.Append(@" 
            public partial class " + ns + @":"+t.Name +@"
            {
               public" + t.Name+ @"()
               {
                  InitializeComponent();
               }"
            );
            #endregion

            #region 事件
            sb.Append(@" 
            #region Event
            

            #endregion 
             ");
            #region 方法
            sb.Append(@"
            public void ChageStyleManager(string url)
            {
              Uri uri = new Uri(url, UriKind.Relative);
              ImplicitStyleManager.SetResourceDictionaryUri(this.Root, uri);
              ImplicitStyleManager.SetApplyMode(Root, ImplicitStylesApplyMode.Auto);
              ImplicitStyleManager.Apply(Root);
            }
            ");
            #endregion
            #region 
            sb.Append(@"
             
       }
    }"
            );
            #endregion 
           
            #endregion
            #endregion

            #region return

            gr = new GenResult(GenResultTypes.CodeSegments);
            gr.CodeSegments = new List<KeyValuePair<string, string>>();
            //gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML Import:", ));
            //gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid Style:", result_style));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML:", sb.ToString()));
            //gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid CS:", result_cs));
            return gr;

            #endregion
        }

    }
}
