using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
	class Gen_Table_DataGrid : IGenComponent
	{
		#region Init

		public Gen_Table_DataGrid()
		{
			this._properties.Add(GenProperties.Name, "Gen_Table_DataGrid");
			this._properties.Add(GenProperties.Caption, "DataGrid");
			this._properties.Add(GenProperties.Group, "SilverLight");
			this._properties.Add(GenProperties.Tips, "为 Table 生成 SilverLight 的数据行列表 UI 相关代码");
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
			#region Init

			GenResult gr;
			Table t = (Table)sqlElements[0];

			List<Column> pks = Utils.GetPrimaryKeyColumns(t);

			if (pks.Count == 0)
			{
				gr = new GenResult(GenResultTypes.Message);
				gr.Message = "无法为没有主键字段的表生成该UI代码！";
				return gr;
			}

			List<Column> wcs = Utils.GetWriteableColumns(t);
			List<Column> socs = Utils.GetSortableColumns(t);
			List<Column> sacs = Utils.GetSearchableColumns(t);

			string tbn = Utils.GetEscapeName(t);

			StringBuilder sb = new StringBuilder();

			#endregion

			#region Gen Import


            sb.Remove(0, sb.Length);

            sb.Append(@"
    xmlns:controls=""clr-namespace:Microsoft.Windows.Controls;assembly=Microsoft.Windows.Controls""
    xmlns:basics=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls""
	xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data"" 
	xmlns:localprimitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
");

            string result_import = sb.ToString();

            #endregion

            #region Gen Style

            sb.Remove(0, sb.Length);

            sb.Append(@"
	<UserControl.Resources>

		<!-- DataGrid Controls Container -->

		<Style x:Key=""DataGridControlsContainer"" TargetType=""Grid"">
			<Setter Property=""Margin"" Value=""5 5 5 5"" />
		</Style>

		<!-- DataGrid Toolbar -->

		<Style x:Key=""DataGridToolbar"" TargetType=""StackPanel"">
		</Style>

		<Style x:Key=""DataGridToolBarButtons"" TargetType=""Button"">
			<Setter Property=""Padding"" Value=""15 0 15 0"" />
			<Setter Property=""Margin"" Value=""0 0 5 0"" />
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""Height"" Value=""22"" />
		</Style>

		<Style x:Key=""DataGridToolbarTextBlocks"" TargetType=""TextBlock"">
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""VerticalAlignment"" Value=""Center"" />
			<Setter Property=""Margin"" Value=""0 0 2 0"" />
		</Style>

		<!-- DataGrid Toolbar Right -->

		<Style x:Key=""DataGridToolbarRight"" TargetType=""StackPanel"">
		</Style>

		<Style x:Key=""DataGridToolBarRightButtons"" TargetType=""Button"">
			<Setter Property=""Padding"" Value=""15 0 15 0"" />
			<Setter Property=""Margin"" Value=""5 0 0 0"" />
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""Height"" Value=""22"" />
		</Style>

		<Style x:Key=""DataGridToolbarRightTextBlocks"" TargetType=""TextBlock"">
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""VerticalAlignment"" Value=""Center"" />
			<Setter Property=""Margin"" Value=""2 0 0 0"" />
		</Style>

		<Style x:Key=""DataGridToolbarRightComboBoxes"" TargetType=""ComboBox"">
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""VerticalAlignment"" Value=""Center"" />
			<Setter Property=""Margin"" Value=""2 0 0 0"" />
		</Style>


		<!-- DataGrid -->

		<Style x:Key=""DataGrid"" TargetType=""data:DataGrid"">
		</Style>

		<Style x:Key=""DataGridRows"" TargetType=""data:DataGridRow"">
			<!-- <Setter Property=""Height"" Value=""25"" /> --->
		</Style>

		<Style x:Key=""DataGridColumnHeaders"" TargetType=""localprimitives:DataGridColumnHeader"">
        <Setter Property=""Template"">
                <Setter.Value>
                    <ControlTemplate TargetType=""localprimitives:DataGridColumnHeader"">
                       <Grid>
                            <this:DataGridColumnHead Content=""{TemplateBinding Content}"" />
                        </Grid>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
		</Style>

		<!-- DataGrid Pager -->

		<Style x:Key=""DataGridPager"" TargetType=""StackPanel"">
		</Style>

		<Style x:Key=""DataGridPagerButtons"" TargetType=""Button"">
			<Setter Property=""Padding"" Value=""15 0 15 0"" />
			<Setter Property=""Margin"" Value=""0 0 5 0"" />
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""Height"" Value=""22"" />
		</Style>

		<Style x:Key=""DataGridPagerTextBlocks"" TargetType=""TextBlock"">
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""VerticalAlignment"" Value=""Center"" />
			<Setter Property=""Margin"" Value=""0 0 2 0"" />
		</Style>

		<Style x:Key=""DataGridPagerTextBoxes"" TargetType=""TextBox"">
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""VerticalAlignment"" Value=""Center"" />
			<Setter Property=""Margin"" Value=""0 0 5 0"" />
			<Setter Property=""FontSize"" Value=""12"" />
			<Setter Property=""Height"" Value=""22"" />
			<Setter Property=""Width"" Value=""35"" />
			<Setter Property=""BorderBrush"">
				<Setter.Value>
					<SolidColorBrush Color=""Silver""></SolidColorBrush>
				</Setter.Value>
			</Setter>
		</Style>

	</UserControl.Resources>
");

            string result_style = sb.ToString();

            #endregion

            #region Gen XAML

            sb.Remove(0, sb.Length);

            sb.Append(@"
		<!-- " + tbn + @" -->
		<Grid Style=""{StaticResource DataGridControlsContainer}"">
         <Grid.RowDefinitions>
			<RowDefinition Height=""*"" />
			<RowDefinition Height=""5"" />
			<RowDefinition Height=""*"" />
			<RowDefinition Height=""5"" />
			<RowDefinition Height=""22"" />
		</Grid.RowDefinitions>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width=""*"" />
			<ColumnDefinition Width=""*"" />
			<ColumnDefinition Width=""10"" />
			<ColumnDefinition Width=""200""/>
		</Grid.ColumnDefinitions>
			<!-- " + tbn + @" 查询 -->");
            sb.Append(@"
              <!-- cc_玩家举报 查询 -->
             <controls:WrapPanel x:Name=""ManualTextWrapping"" Orientation=""Horizontal"" Grid.ColumnSpan=""2"" >");
  
            foreach (Column c in t.Columns)
            {
                string cn = Utils.GetEscapeName(c);
                string caption = Utils.GetCaption(c);
                if (Utils.CheckIsStringType(c))
                {
                    sb.Append(@"
                       <StackPanel Margin=""0,5,5,0"" Orientation=""Horizontal"" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					      <TextBlock Height=""Auto"" Width=""100"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                          <TextBox Height=""Auto"" Width=""150"" x:Name=""" + cn + @""" TextWrapping=""Wrap""/>
                       </StackPanel>");
                }
                else if(Utils.CheckIsNumericType(c))
                {
                    sb.Append(@"
                       <StackPanel Margin=""0,5,5,0"" Orientation=""Horizontal"" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					      <TextBlock Height=""Auto"" Width=""100"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                          <ComboBox x:Name=""" + cn + @"""  HorizontalAlignment=""Left"" Width=""150"" />
                          <TextBox x:Name=""" + cn + @"""  HorizontalAlignment=""Left"" Width=""150"" />
                       </StackPanel>");
                    
                }
                else if (Utils.CheckIsDateTimeType(c))  
                {
                     sb.Append(@"
                       <StackPanel Margin=""0,5,5,0"" Orientation=""Horizontal"" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					      <TextBlock Height=""Auto"" Width=""100"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                          <basics:DatePicker  x:Name=""" + cn + @""" HorizontalAlignment=""Left"" Width=""150"" />
                       </StackPanel>");
                }
                else if (Utils.CheckIsGuidType(c))
                {
                }
                else if (Utils.CheckIsBooleanType(c))
                {
                    sb.Append(@"
                       <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"">");
                    sb.Append(@"    
					     <TextBlock Height=""Auto"" Width=""100"" Margin=""0,0,5,0"" Text=""" + caption + @":""/>
                         <CheckBox Height=""Auto"" Width=""Auto"" x:Name=""" + cn + @"""/>
                       </StackPanel>");
                }
                else if (Utils.CheckIsBinaryType(c))
                {
                    // todo
                }
            }
            sb.Append(@" 
                <StackPanel Grid.ColumnSpan=""1"" Grid.RowSpan=""3"" Grid.Column=""4"">
                    <Button Content=""提交查询""  ></Button>
                </StackPanel>");
            sb.Append(@"</controls:WrapPanel>
           ");
            sb.Append("<!-- " + tbn + @" DataGrid -->
			<data:DataGrid x:Name=""_" + tbn + @"_DataGrid"" Grid.Row=""2"" RowBackground=""Cornsilk"" AlternatingRowBackground=""LemonChiffon"" Grid.ColumnSpan=""2"" Style=""{StaticResource DataGrid}"" RowStyle=""{StaticResource DataGridRows}"" AutoGenerateColumns=""False"" SelectionMode=""Single"" HeadersVisibility=""All"" CanUserSortColumns=""False""  KeyDown=""DataGrid_KeyDown""  SelectionChanged=""_" + tbn + @"_DataGrid_SelectionChanged"" SizeChanged=""_" + tbn + @"_DataGrid_SizeChanged"">
				<data:DataGrid.Columns>

					<data:DataGridTemplateColumn CanUserSort=""False"">
						<data:DataGridTemplateColumn.HeaderStyle>
							<Style TargetType=""localprimitives:DataGridColumnHeader"">
								<Setter Property=""Template"">
									<Setter.Value>
										<ControlTemplate>
											<CheckBox Margin=""3 4 0 0"" IsThreeState=""False"" Loaded=""_" + tbn + @"_HeaderCheckBox_Loaded"" Checked=""_" + tbn + @"_HeaderCheckBox_Checked"" Unchecked=""_" + tbn + @"_HeaderCheckBox_Unchecked"" />
										</ControlTemplate>
									</Setter.Value>
								</Setter>
							</Style>
						</data:DataGridTemplateColumn.HeaderStyle>
						<data:DataGridTemplateColumn.CellTemplate>
							<DataTemplate>
								<StackPanel Orientation=""Horizontal"">
									<CheckBox Tag=""{Binding}"" Margin=""3 4 0 0"" IsThreeState=""False"" Loaded=""_" + tbn + @"_RowCheckBox_Loaded"" Checked=""_" + tbn + @"_RowCheckBox_Checked"" Unchecked=""_" + tbn + @"_RowCheckBox_Checked"" />
								</StackPanel>
							</DataTemplate>
						</data:DataGridTemplateColumn.CellTemplate>
					</data:DataGridTemplateColumn>

					<!-- 注: Column.Width 相关的值可以为 SizeToHeader, SizeToCells, Auto, n -->
	");
            foreach (Column c in t.Columns)
            {
                string cn = Utils.GetEscapeName(c);
                string caption = Utils.GetCaption(c);
                //if (string.IsNullOrEmpty(caption) || caption.Trim().Length == 0) caption = c.Name;
                //string rdonly = wcs.Contains(c) ? "" : @" ReadOnly=""True""";
                //string sort = socs.Contains(c) ? (@" SortExpression=""" + cn + @"""") : "";

                if (Utils.CheckIsStringType(c) || Utils.CheckIsNumericType(c) || Utils.CheckIsDateTimeType(c) || Utils.CheckIsGuidType(c))
                {
                    sb.Append(@"
					<data:DataGridTextColumn HeaderStyle=""{StaticResource DataGridColumnHeaders}"" CanUserSort=""False"" Header=""" + caption + @""" Width=""Auto"" MinWidth=""20"" Binding=""{Binding Path=" + cn + @"}"" />");
                }
                else if (Utils.CheckIsBooleanType(c))
                {
                    sb.Append(@"
					<data:DataGridCheckBoxColumn HeaderStyle=""{StaticResource DataGridColumnHeaders}"" CanUserSort=""False"" Header=""" + caption + @""" Width=""Auto"" Binding=""{Binding Path=" + cn + @"}""");
                    if (c.Nullable)
                        sb.Append(@" IsThreeState=""True""");
                    sb.Append(@" />");
                }
                else if (Utils.CheckIsBinaryType(c))
                {
                    // todo
                }
            }
            sb.Append(@"
				</data:DataGrid.Columns>
			</data:DataGrid>

			<!-- " + tbn + @" Pager -->
			
			<StackPanel x:Name=""_" + tbn + @"_Pager_StackPanel"" Grid.Row=""4"" Grid.ColumnSpan=""2"" Style=""{StaticResource DataGridPager}"" Orientation=""Horizontal"" Height=""Auto"">
				<Button x:Name=""_" + tbn + @"_Pager_First_Button"" Content=""翻至首页"" Click=""_" + tbn + @"_Pager_First_Button_Click"" Style=""{StaticResource DataGridPagerButtons}""></Button>
				<Button x:Name=""_" + tbn + @"_Pager_Previous_Button"" Content=""上一页"" Click=""_" + tbn + @"_Pager_Previous_Button_Click"" Style=""{StaticResource DataGridPagerButtons}""></Button>
				<Button x:Name=""_" + tbn + @"_Pager_Next_Button"" Content=""下一页"" Click=""_" + tbn + @"_Pager_Next_Button_Click"" Style=""{StaticResource DataGridPagerButtons}""></Button>
				<Button x:Name=""_" + tbn + @"_Pager_Last_Button"" Content=""翻至尾页"" Click=""_" + tbn + @"_Pager_Last_Button_Click"" Style=""{StaticResource DataGridPagerButtons}""></Button>
				<TextBlock x:Name=""_" + tbn + @"_Pager_PageIndex_TextBlock"" Text=""页码:"" Style=""{StaticResource DataGridPagerTextBlocks}"" />
				<TextBox x:Name=""_" + tbn + @"_Pager_PageIndex_TextBox"" Style=""{StaticResource DataGridPagerTextBoxes}""></TextBox>
				<TextBlock x:Name=""_" + tbn + @"_Pager_PageCount_TextBlock"" Text=""总页数:"" Style=""{StaticResource DataGridPagerTextBlocks}"" />
				<TextBox x:Name=""_" + tbn + @"_Pager_PageCount_TextBox"" IsReadOnly=""True"" Style=""{StaticResource DataGridPagerTextBoxes}""></TextBox>
				<TextBlock x:Name=""_" + tbn + @"_Pager_RowCount_TextBlock"" Text=""总行数:"" Style=""{StaticResource DataGridPagerTextBlocks}"" />
				<TextBox x:Name=""_" + tbn + @"_Pager_RowCount_TextBox"" IsReadOnly=""True"" Style=""{StaticResource DataGridPagerTextBoxes}""></TextBox>
			</StackPanel>

          <StackPanel Grid.Column=""3"" Grid.RowSpan=""5"" Width=""500"" Height=""Auto"" Grid.ColumnSpan=""1"" >
            <StackPanel>
            	<StackPanel.Background>
            		<LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
            			<GradientStop Color=""#FFD6D6D6"" Offset=""0.326""/>
            			<GradientStop Color=""#FFFFFFFF"" Offset=""0.33""/>
            		</LinearGradientBrush>
            	</StackPanel.Background>
                <TextBlock Text=""当前行操作"" Foreground=""#FF000000"" Height=""25"" FontSize=""20"" HorizontalAlignment=""Left"" ></TextBlock>
                <HyperlinkButton x:Name=""_" + tbn + @"_Toolbar_处理结果_拒绝_Button"" Content=""拒绝""   Click=""_" + tbn + @"_Toolbar_处理结果_拒绝_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""_" + tbn + @"_Toolbar_处理结果_未处理_Button"" Content=""未处理""   Click=""_" + tbn + @"_Toolbar_处理结果_未处理_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""_" + tbn + @"_Toolbar_处理结果_已处理_Button"" Content=""已处理""   Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click"" ></HyperlinkButton>         
                <HyperlinkButton x:Name=""_" + tbn + @"_Toolbar_处理结果_正在处理_Button"" Content=""正在处理""  Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click""></HyperlinkButton>
                <HyperlinkButton x:Name="""+tbn+@"_Toolbar_处理结果_填写处理结果_Button"" Content=""填写处理结果"" Click=""_"+tbn+@"_Toolbar_处理结果_填写处理结果_Button_Click""></HyperlinkButton>
                <HyperlinkButton x:Name="""+tbn+@"_Toolbar_处理结果_查看历史变更日志_Button"" Content=""查看历史变更日志"" Click=""_"+tbn+@"_Toolbar_处理结果_查看历史变更日志_Button_Click""></HyperlinkButton>
            </StackPanel>
            <StackPanel Margin=""0,22,0,0"" x:Name=""MostCheck_StackPanel"" Visibility=""Collapsed"">
                <StackPanel.Background>
                    <LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
                        <GradientStop Color=""#FFD6D6D6"" Offset=""0.326""/>
                        <GradientStop Color=""#FFFFFFFF"" Offset=""0.33""/>
                    </LinearGradientBrush>
                </StackPanel.Background>
                <TextBlock Text=""勾选行操作"" Foreground=""#FF000000"" Height=""25"" FontSize=""20"" HorizontalAlignment=""Left"" ></TextBlock>
                <HyperlinkButton x:Name=""_" + tbn + @"_Most_Toolbar_处理结果_拒绝_Button"" Content=""拒绝""   Click=""_" + tbn + @"_Most_Toolbar_处理结果_拒绝_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""_" + tbn + @"_Most_Toolbar_处理结果_未处理_Button"" Content=""未处理""   Click=""_" + tbn + @"_Most_Toolbar_处理结果_未处理_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""_" + tbn + @"_Most_Toolbar_处理结果_已处理_Button"" Content=""已处理""   Click=""_" + tbn + @"_Most_Toolbar_处理结果_已处理_Click"" ></HyperlinkButton>         
                <HyperlinkButton x:Name=""_" + tbn + @"_Most_Toolbar_处理结果_正在处理_Button"" Content=""正在处理""  Click=""_" + tbn + @"_Most_Toolbar_处理结果_已处理_Click""></HyperlinkButton>
            </StackPanel>
        </StackPanel>
		<basics:GridSplitter Margin=""0,0,0,0"" Grid.Column=""2"" Grid.Row=""0"" Grid.RowSpan=""5"" HorizontalAlignment=""Stretch"" Grid.ColumnSpan=""1""  Background=""#FFBDBDBD"" HorizontalContentAlignment=""Stretch"" VerticalContentAlignment=""Stretch"" Cursor=""None""/>

		</Grid>");


            string result_xaml = sb.ToString();

            #endregion

            #region Gen CS

            sb.Remove(0, sb.Length);


            sb.Append(@"
		#region Constructor
//		/// <summary>
//		/// Service Client Instance
//		/// </summary>
//		private SR_Admin.Service_AdminClient _sc = new SR_Admin.Service_AdminClient();
		DataGrid _" + tbn + @"_处理结果变更日志_DataGrid;
		DataGrid _" + tbn + @"_处理状态变更日志_DataGrid;
		Telerik.Windows.Controls.RadWindow radWindow;
		Telerik.Windows.Controls.RadWindow changeWindow; 
		TextBox resultContext;
        DgWindows _dgWindiows;
		public static int index = 1;
		public Bus_SL.SR_Admin.OO" + tbn+@" SelectItem_OO"+tbn+@"
		{
			get;
			set;
		}


        public Page()
		{
			InitializeComponent();

			// 注册载入事件
			this.Loaded += new RoutedEventHandler(Page_Loaded);

			// 扩展到全屏
			this.Width = this.Height = double.NaN;

			ManualTextWrapping.Height = double.NaN;

            // 令整个 DataGrid 只读
			this._" + tbn + @"_DataGrid.IsReadOnly = true;

			// 初始化 DataGrid 显示行数
			this." + tbn + @"_PageSize = 10;
            
            this._"+tbn+@"_Pager_PageIndex_TextBox.TextChanged +=new TextChangedEventHandler(_"+tbn+@"_Pager_PageIndex_TextBox_TextChanged);

			// 初始化按钮状态
			this." + tbn + @"_EnsurePagerState();
		}

		#endregion

		#region " + tbn + @" Service Client & Methods



		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
//			// 注册数据取回完毕事件
//			this._sc." + tbn + @"_获取多条Completed += new EventHandler<ServiceReference1.Get" + tbn + @"RowsCompletedEventArgs>(sc_Get" + tbn + @"RowsCompleted);
//          this._sc.cc_处理状态_获取多条Completed +=new EventHandler<Bus_SL.SR_Admin.cc_处理状态_获取多条CompletedEventArgs>(_sc_cc_处理状态_获取多条Completed);
			this._sc." + tbn + @"_处理状态_修改Completed += new EventHandler<SR_Admin." + tbn + @"_处理状态_修改CompletedEventArgs>(_sc_" + tbn + @"_处理结果_修改Completed);
			
//			" + tbn + @"_GetData(1);
		}

//      void _sc_" + tbn + @"_处理结果_修改Completed(object sender, SR_Admin." + tbn + @"_处理状态_修改CompletedEventArgs e)
//		{
//			if(e.Result != null )
//			{
//				Utils.Window.Alert(e.Result);
//			}
//		}
//
//
//		void _sc_cc_处理状态_获取多条Completed(object sender, Bus_SL.SR_Admin.cc_处理状态_获取多条CompletedEventArgs e)
//		{
//			// 如果返回过程没中断, 结果没问题显示
//			if (!e.Cancelled && e.Error == null)
//			{
//				处理状态编号.Items.Clear();
//				foreach (var o in e.Result)
//				{
//					ComboBoxItem c = new ComboBoxItem();
//					c.Tag = o.处理状态编号;
//					c.Content = o.状态名;
//					处理状态编号.Items.Add(c);
//				}
//				处理状态编号.SelectedIndex = 0;
//			}
//		}

//		private void sc_Get" + tbn + @"RowsCompleted(object sender, ServiceReference1." + tbn + @"_获取多条CompletedEventArgs e)
//		{
//			// 如果返回过程没中断, 结果没问题就 bind 显示
//			if (!e.Cancelled && e.Error == null)
//			{
//				this." + tbn + @"_RowCount = e.Result.Count;
//				" + tbn + @"_EnsurePagerState();
//				this._" + tbn + @"_DataGrid.ItemsSource = e.Result.Rows;
//			}
//			else
//			{
//				this." + tbn + @"_RowCount = 0;
//				" + tbn + @"_EnsurePagerState();
//				this._" + tbn + @"_DataGrid.ItemsSource = null;
//				Utils.Window.Alert(e.Error.Message);
//			}
//		}

		/// <summary>
		/// 开始取回视图 " + tbn + @" 的数据
		/// </summary>
		private void " + tbn + @"_GetData(int sortOrder)
		{
//			this._sc." + tbn + @"_获取多条Async(this." + tbn + @"_PageIndex, this." + tbn + @"_PageSize, sortOrder);
//          this._sc.cc_处理状态_获取多条Async();
		}

		#endregion

		#region _" + tbn + @"_HeaderCheckBox

		private void _" + tbn + @"_HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				cb.IsChecked = (sender as CheckBox).IsChecked;
			}
		}

		private void _" + tbn + @"_HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				cb.IsChecked = (sender as CheckBox).IsChecked;
			}
		}

		#endregion

		#region _" + tbn + @"_RowCheckBoxList

		private List<CheckBox>  _" + tbn + @"_RowCheckBoxList = new List<CheckBox>();

		private void CheckBox_Loaded(object sender, RoutedEventArgs e)
		{
			this._" + tbn + @"_RowCheckBoxList.Add((CheckBox)sender);
		}

		private void _" + tbn + @"_RowCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox c in _"+tbn+@"_RowCheckBoxList)
			{
				if (c == (CheckBox)sender)
				{
					c.IsChecked = ((CheckBox)sender).IsChecked;
				}
			}
			" + tbn + @"_EnsureStackPanelState();
		}

		private void _" + tbn + @"_RowCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox c in _" + tbn + @"_RowCheckBoxList)
			{
				if (c == (CheckBox)sender)
				{
					c.IsChecked = ((CheckBox)sender).IsChecked;
				}
			}
			" + tbn + @"_EnsureStackPanelState();
		}

		#endregion

		#region _" + tbn + @"_Toolbar

        private void _" + tbn + @"_Toolbar_处理结果_拒绝_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			_sc." + tbn + @"_处理状态_修改Async(SelectItem_OO" + tbn + @".xxxx, App.AdminID, 1);
			" + tbn + @"_GetData(index);
		}


		private void _" + tbn + @"_Toolbar_处理结果_未处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			_sc." + tbn + @"_处理状态_修改Async(SelectItem_OO" + tbn + @".xxx, App.AdminID, 0);
			" + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Toolbar_处理结果_已处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			_sc." + tbn + @"_处理状态_修改Async(SelectItem_OO" + tbn + @".xxx, App.AdminID, 3);
			" + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Toolbar_处理结果_正在处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			_sc." + tbn + @"_处理状态_修改Async(SelectItem_OO" + tbn + @".xxxx, App.AdminID,2);
			" + tbn + @"_GetData(index);
		}

        private void _" + tbn + @"_Most_Toolbar_处理结果_未处理_Click(object sender, RoutedEventArgs e)
		{
		    _dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				if (cb.IsChecked == true)
				{
					_sc." + tbn + @"_处理状态_修改Async(((Bus_SL.SR_Admin.OO" + tbn + @")cb.Tag).xxxx, App.AdminID, 0);
				}
			}
			" + tbn + @"_GetData(index);
 		}

		private void _" + tbn + @"_Most_Toolbar_处理结果_拒绝_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				if (cb.IsChecked == true)
				{
					_sc." + tbn + @"_处理状态_修改Async(((Bus_SL.SR_Admin.OO" + tbn + @")cb.Tag).xxxx, App.AdminID, 1);
				}
			}
			" + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Most_Toolbar_处理结果_已处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				if (cb.IsChecked == true)
				{
					_sc." + tbn + @"_处理状态_修改Async(((Bus_SL.SR_Admin.OO" + tbn + @")cb.Tag).xxxx, App.AdminID, 3);
				}
			}
			" + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"Most_Toolbar_处理结果_正在处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			foreach (CheckBox cb in this._" + tbn + @"_RowCheckBoxList)
			{
				if (cb.IsChecked == true)
				{
					_sc." + tbn + @"_处理状态_修改Async(((Bus_SL.SR_Admin.OO" + tbn + @")cb.Tag).xxxx, App.AdminID, 2);
				}
			}
			" + tbn + @"_GetData(index);
		}

		/// <summary>
		/// 提交查询
		/// </summary>
		private void _" + tbn + @"_ToolbarRight_Submit_Button_Click(object sender, RoutedEventArgs e)
		{ }

       
		private void _" + tbn + @"_Toolbar_处理结果_填写处理结果_Button_Click(object sender, RoutedEventArgs e)
		{
			StackPanel stackPanel= new StackPanel();
			TextBlock textblock = new TextBlock ();
			textblock.Text =""处理结果:"";

		    resultContext = new TextBox();
			resultContext.TextWrapping = TextWrapping.Wrap;
			resultContext.Width = 400;
			resultContext.Height = 400;

			Button  btn  = new Button ();
			btn.Width = 100;		
			btn .Content = ""确认"";
			btn.Click += new RoutedEventHandler(btn_Click);

			stackPanel.Children.Add(textblock);
			stackPanel.Children.Add(resultContext);
			stackPanel.Children.Add(btn);

		    radWindow = Utils.Window.Create(500, 550, ""玩家xxx处理界面"", stackPanel);
			radWindow.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;
			radWindow.ShowDialog();
		}

		void btn_Click(object sender, RoutedEventArgs e)
		{
			_sc." + tbn + @"_处理结果_设为Async(SelectItem_OO" + tbn + @".xxxx, App.AdminID, resultContext.Text);
			radWindow.Close();
		}
		#endregion

		#region _" + tbn + @"_DataGrid

		private void _" + tbn + @"_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != 0)
			{
				SelectItem_OO" + tbn + @"= (Bus_SL.SR_Admin.OO" + tbn + @")e.AddedItems[0];
			}
		}
       
    	private void _" + tbn + @"_Toolbar_处理结果_查看历史变更日志_Button_Click(object sender, RoutedEventArgs e)
 		{

			Grid _grid = new Grid();

			RowDefinition R1 = new RowDefinition();
			R1.Height = new GridLength(20);
			RowDefinition R2 = new RowDefinition();
			R2.Height = new GridLength(670);
			ColumnDefinition C1 = new ColumnDefinition();
			C1.Width = new GridLength(450);
			ColumnDefinition C2 = new ColumnDefinition();
			C2.Width = new GridLength(450);
			_grid.RowDefinitions.Add(R1);
			_grid.RowDefinitions.Add(R2);
			_grid.ColumnDefinitions.Add(C1);
			_grid.ColumnDefinitions.Add(C2);

			Button CloseWindow = new Button();
			CloseWindow.Content = ""关闭界面"";

			_" + tbn + @"_处理状态变更日志_DataGrid = new DataGrid();
			_" + tbn + @"_处理状态变更日志_DataGrid.AutoGenerateColumns = false;
			_" + tbn + @"_处理状态变更日志_DataGrid.SelectionMode = DataGridSelectionMode.Single;
			_" + tbn + @"_处理状态变更日志_DataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
			Grid.SetRow(_" + tbn + @"_处理状态变更日志_DataGrid, 1);

			DataGridTextColumn dgt1 = new DataGridTextColumn();
			dgt1.IsReadOnly = true;
			dgt1.Header = ""XXXX"";
			dgt1.Width = DataGridLength.Auto;
			dgt1.MinWidth = 20;
			Binding b1 = new Binding(""XXX"");
			dgt1.Binding = b1;

			DataGridTextColumn dgt2 = new DataGridTextColumn();
			dgt2.IsReadOnly = true;
			dgt2.Header = ""处理状态编号"";
			dgt2.Width = DataGridLength.Auto;
			dgt2.MinWidth = 20;
			Binding b2 = new Binding(""处理状态编号"");
			b2.Converter = new StateValueConverter();
			dgt2.Binding = b2;


			DataGridTextColumn dgt3 = new DataGridTextColumn();
			dgt3.IsReadOnly = true;
			dgt3.Header = ""日志编号"";
			dgt3.Width = DataGridLength.Auto;
			dgt3.MinWidth = 20;
			Binding b3 = new Binding(""日志编号"");
			dgt3.Binding = b3;

			DataGridTextColumn dgt4 = new DataGridTextColumn();
			dgt4.IsReadOnly = false;
			dgt4.Header = ""修改人编号"";
			dgt4.Width = DataGridLength.Auto;
			dgt4.MinWidth = 20;
			Binding b4 = new Binding(""修改人编号"");
			dgt4.Binding = b4;

			DataGridTextColumn dgt5 = new DataGridTextColumn();
			dgt5.IsReadOnly = true;
			dgt5.Header = ""修改时间"";
			dgt5.Width = DataGridLength.Auto;
			dgt5.MinWidth = 20;
			Binding b5 = new Binding(""修改时间"");
			dgt5.Binding = b5;
			_" + tbn + @"_处理状态变更日志_DataGrid.Columns.Add(dgt1);
			_" + tbn + @"_处理状态变更日志_DataGrid.Columns.Add(dgt2);
			_" + tbn + @"_处理状态变更日志_DataGrid.Columns.Add(dgt3);
			_" + tbn + @"_处理状态变更日志_DataGrid.Columns.Add(dgt4);
			_" + tbn + @"_处理状态变更日志_DataGrid.Columns.Add(dgt5);

			_" + tbn + @"_处理结果变更日志_DataGrid = new DataGrid();
			_" + tbn + @"_处理结果变更日志_DataGrid.AutoGenerateColumns = false;
			_" + tbn + @"_处理结果变更日志_DataGrid.SelectionMode = DataGridSelectionMode.Single;
			_" + tbn + @"_处理结果变更日志_DataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
			Grid.SetRow(_" + tbn + @"_处理结果变更日志_DataGrid, 1);
			Grid.SetColumn(_" + tbn + @"_处理结果变更日志_DataGrid, 1);

			DataGridTextColumn dgt6 = new DataGridTextColumn();
			dgt6.IsReadOnly = true;
			dgt6.Header = ""xxx"";
			dgt6.Width = DataGridLength.Auto;
			dgt6.MinWidth = 20;
			Binding b6 = new Binding(""xxxx"");
			dgt6.Binding = b6;


			DataGridTextColumn dgt7 = new DataGridTextColumn();
			dgt7.IsReadOnly = true;
			dgt7.Header = ""处理结果"";
			dgt7.Width = DataGridLength.Auto;
			dgt7.MinWidth = 20;
			Binding b7 = new Binding(""处理结果"");
			dgt7.Binding = b7;

			DataGridTextColumn dgt8 = new DataGridTextColumn();
			dgt8.IsReadOnly = true;
			dgt8.Header = ""日志编号"";
			dgt8.Width = DataGridLength.Auto;
			dgt8.MinWidth = 20;
			Binding b8 = new Binding(""日志编号"");
			dgt8.Binding =b8;

			DataGridTextColumn dgt9 = new DataGridTextColumn();
			dgt9.IsReadOnly = true;
			dgt9.Header = ""修改人编号"";
			dgt9.Width = DataGridLength.Auto;
			dgt9.MinWidth = 20;
			Binding b9 = new Binding(""修改人编号"");
			dgt9.Binding = b9;

			DataGridTextColumn dgt10 = new DataGridTextColumn();
			dgt10.IsReadOnly = true;
			dgt10.Header = ""修改时间"";
			dgt10.Width = DataGridLength.Auto;
			dgt10.MinWidth = 20;
			Binding b10 = new Binding(""修改时间"");
			dgt10.Binding = b10;
			_" + tbn + @"_处理结果变更日志_DataGrid.Columns.Add(dgt6);
			_" + tbn + @"_处理结果变更日志_DataGrid.Columns.Add(dgt7);
			_" + tbn + @"_处理结果变更日志_DataGrid.Columns.Add(dgt8);
			_" + tbn + @"_处理结果变更日志_DataGrid.Columns.Add(dgt9);
			_" + tbn + @"_处理结果变更日志_DataGrid.Columns.Add(dgt10);


			_grid.Children.Add(CloseWindow);
			_grid.Children.Add(_" + tbn + @"_处理状态变更日志_DataGrid);
			_grid.Children.Add(_" + tbn + @"_处理结果变更日志_DataGrid);

			this._sc." + tbn + @"_处理状态变更日志_获取多条Completed += new EventHandler<" + tbn + @"_处理状态变更日志_获取多条CompletedEventArgs>(_sc_" + tbn + @"_处理状态变更日志_获取多条Completed);
			this._sc." + tbn + @"_处理结果变更日志_获取多条Completed += new EventHandler<" + tbn + @"_处理结果变更日志_获取多条CompletedEventArgs>(_sc_" + tbn + @"_处理结果变更日志_获取多条Completed);
			_sc." + tbn + @"_处理状态变更日志_获取多条Async(SelectItem_OO" + tbn + @".XXX);
			_sc." + tbn + @"_处理结果变更日志_获取多条Async(SelectItem_OO" + tbn + @"XXX);

			changeWindow = Utils.Window.Create(900, 700, ""XXX查看历史界面"", _grid);
			changeWindow.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;
			changeWindow.ShowDialog();
			CloseWindow.Click += new RoutedEventHandler(CloseWindow_Click);
		}


		void _sc_" + tbn + @"_处理状态变更日志_获取多条Completed(object sender," + tbn + @"_处理状态变更日志_获取多条CompletedEventArgs e)
		{
		   _" + tbn + @"_处理状态变更日志_DataGrid.ItemsSource = e.Result;
		}

		void _sc_" + tbn + @"_处理结果变更日志_获取多条Completed(object sender," + tbn + @"_处理结果变更日志_获取多条CompletedEventArgs e)
		{
		   _" + tbn + @"_处理结果变更日志_DataGrid.ItemsSource = e.Result;
		}

		void CloseWindow_Click(object sender, RoutedEventArgs e)
		{
			changeWindow.Hide();
			changeWindow.Close();
		}
        /// <summary>
		/// 当选中行,通过键盘空格控制勾选该行
		/// </summary>
		private void DataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
              DataGrid dg =(DataGrid)sender;
			  foreach (CheckBox c in _" + tbn + @"_RowCheckBoxList)
			  {
				  if (c.DataContext == dg.SelectedItem)
				  {
					  c.IsChecked = !c.IsChecked; 
				  }
			  }
			}
		}
		#endregion

		#region _" + tbn + @"_Pager

		public int " + tbn + @"_PageSize { get; set; }
		public int " + tbn + @"_PageCount { get; set; }
		public int " + tbn + @"_RowCount { get; set; }
		public int " + tbn + @"_PageIndex { get; set; }

		private void _" + tbn + @"_Pager_First_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex = 0;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Pager_Previous_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex--;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Pager_Next_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex++;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Pager_Last_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex = int.MaxValue;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData(index);
		}

		private void _" + tbn + @"_Pager_PageIndex_TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			int previousIndex = this." + tbn + @"_PageIndex;

			TextBox tb = (TextBox)sender;
			try
			{
				this." + tbn + @"_PageIndex = int.Parse(tb.Text) - 1;
			}
			catch
			{
				tb.Text = (previousIndex + 1).ToString();
			}

			if (previousIndex != this." + tbn + @"_PageIndex)
			{
				this." + tbn + @"_EnsurePagerState();

				this." + tbn + @"_GetData(index);
			}
		}

		private void _" + tbn + @"_DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// this." + tbn + @"_PageSize = ((int)e.NewSize.Height - 25) / 25;
		}


		public void " + tbn + @"_EnsurePagerState()
		{
			if (this." + tbn + @"_PageSize < 1) this." + tbn + @"_PageSize = 1;

			this." + tbn + @"_PageCount = this." + tbn + @"_RowCount / this." + tbn + @"_PageSize;

			if (this." + tbn + @"_RowCount % this." + tbn + @"_PageSize > 0) this." + tbn + @"_PageCount++;

			if (this." + tbn + @"_PageIndex >= this." + tbn + @"_PageCount)
			{
				this." + tbn + @"_PageIndex = this." + tbn + @"_PageCount - 1;
			}
			if (this." + tbn + @"_PageIndex < 0) this." + tbn + @"_PageIndex = 0;

			this._" + tbn + @"_Pager_PageCount_TextBox.Text = this." + tbn + @"_PageCount.ToString();
			this._" + tbn + @"_Pager_RowCount_TextBox.Text = this." + tbn + @"_RowCount.ToString();
			this._" + tbn + @"_Pager_PageIndex_TextBox.Text = (this." + tbn + @"_PageIndex + 1).ToString();

			this._" + tbn + @"_Pager_First_Button.IsEnabled = false;
			this._" + tbn + @"_Pager_Previous_Button.IsEnabled = false;
			this._" + tbn + @"_Pager_Next_Button.IsEnabled = false;
			this._" + tbn + @"_Pager_Last_Button.IsEnabled = false;

			if (this." + tbn + @"_PageIndex > 0)
			{
				this._" + tbn + @"_Pager_First_Button.IsEnabled = true;
				this._" + tbn + @"_Pager_Previous_Button.IsEnabled = true;
			}

			if (this." + tbn + @"_PageIndex < this." + tbn + @"_PageCount - 1)
			{
				this._" + tbn + @"_Pager_Next_Button.IsEnabled = true;
				this._" + tbn + @"_Pager_Last_Button.IsEnabled = true;
			}
		}

        public void " + tbn + @"_EnsureStackPanelState()
		{
			bool isVisibility = false;
			foreach (CheckBox c in this._" + tbn + @"_RowCheckBoxList)
			{
				isVisibility |= (bool)c.IsChecked;
            }
			if (isVisibility == true)
			{
				MostCheck_StackPanel.Visibility = Visibility.Visible;
			}
			else
			{
				MostCheck_StackPanel.Visibility = Visibility.Collapsed;
			}

		}

		#endregion
");

            string result_cs = sb.ToString();

            #endregion

            #region Gen Utils_CS

            sb.Remove(0, sb.Length);

            sb.Append(Gen_Utils_cs.Gen());

            string result_utils_cs = sb.ToString();

			#endregion

            #region Gen_Utils_Custom 
            sb.Remove(0,sb.Length);

            sb.Append(Gen_Utils_custom.Gen());

            string result_utils_custom =sb.ToString();
            #endregion 

			#region return

			gr = new GenResult(GenResultTypes.CodeSegments);
			gr.CodeSegments = new List<KeyValuePair<string, string>>();
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML Import:", result_import));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid Style:", result_style));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML:", result_xaml));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid CS:", result_cs));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL Utils CS:", result_utils_cs));
            gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid Custom Control", result_utils_custom));
			return gr;

			#endregion
		}
	}
}
