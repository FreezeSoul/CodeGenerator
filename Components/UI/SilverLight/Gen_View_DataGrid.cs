using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
	class Gen_View_DataGrid : IGenComponent
	{
		#region Init

		public Gen_View_DataGrid()
		{
			this._properties.Add(GenProperties.Name, "Gen_View_DataGrid");
			this._properties.Add(GenProperties.Caption, "DataGrid");
			this._properties.Add(GenProperties.Group, "SilverLight");
			this._properties.Add(GenProperties.Tips, "为 View 生成 SilverLight 的数据行列视图 UI 相关代码");
            this._properties.Add(GenProperties.IsEnabled, false);
        }
		public SqlElementTypes TargetSqlElementType
		{
			get { return SqlElementTypes.View; }
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
			View t = (View)sqlElements[0];

			return Utils.GetPrimaryKeyColumns(t).Count > 0;
		}

		public GenResult Gen(params object[] sqlElements)
		{
			#region Init

			GenResult gr;
			View t = (View)sqlElements[0];

			List<Column> pks = Utils.GetPrimaryKeyColumns(t);

			if (pks.Count == 0)
			{
				gr = new GenResult(GenResultTypes.Message);
				gr.Message = "无法为没有主键字段的视图生成该UI代码！";
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
	xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data"" 
	xmlns:localprimitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
    xmlns:dataGridColumnHeadTemplate=""clr-namespace:ColumnHeadTemplate""
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
                            <dataGridColumnHeadTemplate:DataGridColumnHead Content=""{TemplateBinding Content}"" />
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
			<RowDefinition Height=""22"" />
			<RowDefinition Height=""5"" />
			<RowDefinition Height=""*"" />
			<RowDefinition Height=""5"" />
			<RowDefinition Height=""22"" />
		</Grid.RowDefinitions>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width=""*"" />
			<ColumnDefinition Width=""*"" />
			<ColumnDefinition Width=""10"" />
			<ColumnDefinition Width=""0.2*""/>
		</Grid.ColumnDefinitions>
			<!-- " + tbn + @" 查询 -->");
            sb.Append(@"
            <Border Grid.ColumnSpean=""2"">
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
            sb.Append("<!-- " + tbn + @" DataGrid -->
			<data:DataGrid x:Name=""_" + tbn + @"_DataGrid"" Grid.Row=""2"" Grid.ColumnSpan=""2"" Style=""{StaticResource DataGrid}"" RowStyle=""{StaticResource DataGridRows}"" AutoGenerateColumns=""False"" SelectionMode=""Single"" HeadersVisibility=""All"" CanUserSortColumns=""False"" SelectionChanged=""_" + tbn + @"_DataGrid_SelectionChanged"" SizeChanged=""_" + tbn + @"_DataGrid_SizeChanged"">
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
				<TextBox x:Name=""_" + tbn + @"_Pager_PageIndex_TextBox"" TextChanged=""_" + tbn + @"_Pager_PageIndex_TextBox_TextChanged"" Style=""{StaticResource DataGridPagerTextBoxes}""></TextBox>
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
                <HyperlinkButton x:Name=""cc_" + tbn + @"_Toolbar_处理结果_未处理_Button"" Content=""未处理""   Click=""_" + tbn + @"_Toolbar_处理结果_未处理_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""cc_" + tbn +@"_Toolbar_处理结果_已处理_Button"" Content=""已处理""   Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click"" ></HyperlinkButton>         
                <HyperlinkButton x:Name=""cc_" + tbn +@"_Toolbar_处理结果_正在处理_Button"" Content=""正在处理""  Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click""></HyperlinkButton>
            </StackPanel>
            <StackPanel Margin=""0,22,0,0"" >
                <StackPanel.Background>
                    <LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
                        <GradientStop Color=""#FFD6D6D6"" Offset=""0.326""/>
                        <GradientStop Color=""#FFFFFFFF"" Offset=""0.33""/>
                    </LinearGradientBrush>
                </StackPanel.Background>
                <TextBlock Text=""勾选行操作"" Foreground=""#FF000000"" Height=""25"" FontSize=""20"" HorizontalAlignment=""Left"" ></TextBlock>
                <HyperlinkButton x:Name=""cc_" + tbn + @"_Most_Toolbar_处理结果_未处理_Button"" Content=""未处理""   Click=""_" + tbn + @"_Toolbar_处理结果_未处理_Click""></HyperlinkButton>
                <HyperlinkButton x:Name=""cc_" + tbn +@"_Most_Toolbar_处理结果_已处理_Button"" Content=""已处理""   Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click"" ></HyperlinkButton>         
                <HyperlinkButton x:Name=""cc_" + tbn +@"_Most_Toolbar_处理结果_正在处理_Button"" Content=""正在处理""  Click=""_" + tbn + @"_Toolbar_处理结果_已处理_Click""></HyperlinkButton>
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

        DgWindows _dgWindiows;

        public Page()
		{
			InitializeComponent();

			// 注册载入事件
			this.Loaded += new RoutedEventHandler(Page_Loaded);

			// 扩展到全屏
			this.Width = this.Height = double.NaN;


			// 隐藏多选 CheckBox
			this._" + tbn + @"_DataGrid.Columns[0].Visibility = Visibility.Collapsed;

			// 隐藏 ToolbarRight 的 Submit Button
			this._" + tbn + @"_ToolbarRight_Submit_Button.Visibility = Visibility.Collapsed;

			// 初始化下拉排序 ComboBox 的显示
			this._" + tbn + @"_ToolbarRight_Sort_ComboBox.SelectedIndex = 0;

            // 令整个 DataGrid 只读
			this._" + tbn + @"_DataGrid.IsReadOnly = true;

			// 初始化 DataGrid 显示行数
			this." + tbn + @"_PageSize = 10;


			// 初始化按钮状态
			this." + tbn + @"_EnsurePagerState();
		}

		#endregion

		#region " + tbn + @" Service Client & Methods

//		/// <summary>
//		/// Service Client Instance
//		/// </summary>
//		private ServiceReference1.Service1Client _sc = new ServiceReference1.Service1Client();

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
//			// 注册数据取回完毕事件
//			this._sc.Get" + tbn + @"RowsCompleted += new EventHandler<ServiceReference1.Get" + tbn + @"RowsCompletedEventArgs>(sc_Get" + tbn + @"RowsCompleted);
//
//			" + tbn + @"_GetData();
		}

//		private void sc_Get" + tbn + @"RowsCompleted(object sender, ServiceReference1.Get" + tbn + @"RowsCompletedEventArgs e)
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
		private void " + tbn + @"_GetData()
		{
//			int sortOrder = this._" + tbn + @"_ToolbarRight_Sort_ComboBox_GetSelectedSortOrder();
//			this._sc.Get" + tbn + @"RowsAsync(this." + tbn + @"_PageIndex, this." + tbn + @"_PageSize, sortOrder);
		}

		#endregion

		#region _" + tbn + @"_HeaderCheckBox

		private CheckBox _" + tbn + @"_HeaderCheckBox;

		private void _" + tbn + @"_HeaderCheckBox_Loaded(object sender, RoutedEventArgs e)
		{
			this._" + tbn + @"_HeaderCheckBox = (CheckBox)sender;
		}

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

		private void _" + tbn + @"_RowCheckBox_Loaded(object sender, RoutedEventArgs e)
		{
			this._" + tbn + @"_RowCheckBoxList.Add((CheckBox)sender);
		}

		private void _" + tbn + @"_RowCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			// var o = (sender as CheckBox).Tag as ServiceReference1.xxxxxxxxxxxxxxxxx;
			// todo
		}

		private void _" + tbn + @"_RowCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			// var o = (sender as CheckBox).Tag as ServiceReference1.xxxxxxxxxxxxxxxxx;
			// todo
		}

		#endregion

		#region _" + tbn + @"_Toolbar


        private void _cc" + tbn +@"_Toolbar_处理结果_未处理_Click(object sender, RoutedEventArgs e)
		{
		    _dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_未处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" + tbn + @"_GetData();
 		}

		private void _cc"+tbn +@"_Toolbar_处理结果_已处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_已处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" + tbn + @"_GetData();
		}

		private void _cc"+tbn+@"_Toolbar_处理结果_正在处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_正在处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" +tbn+ @"_GetData();
		}



        private void _cc" + tbn + @"_Most_Toolbar_处理结果_未处理_Click(object sender, RoutedEventArgs e)
		{
		    _dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_未处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" + tbn + @"_GetData();
 		}

		private void _cc" + tbn + @"_Most_Toolbar_处理结果_已处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_已处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" + tbn + @"_GetData();
		}

		private void _cc" + tbn + @"_Most_Toolbar_处理结果_正在处理_Click(object sender, RoutedEventArgs e)
		{
			_dgWindiows = new DgWindows();
			_dgWindiows.ShowModal();
			//_sc.cc_玩家举报_处理状态_设为_正在处理Async(SelectItem_OOcc_玩家举报.玩家举报编号, App.AdminID);
			cc" + tbn + @"_GetData();
		}



//		private int _" + tbn + @"_ToolbarRight_Sort_ComboBox_GetSelectedSortOrder()
//		{
//			ComboBoxItem cbi = _" + tbn + @"_ToolbarRight_Sort_ComboBox.SelectedItem as ComboBoxItem;
//			if (cbi == null) return 1;
//			return int.Parse(cbi.Tag as string);
//		}
//
//		private void _" + tbn + @"_ToolbarRight_Sort_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//		{
//			" + tbn + @"_GetData();
//
//			// todo: init ui codes here
//		}
//
//		private void _" + tbn + @"_Toolbar_Refresh_Button_Click(object sender, RoutedEventArgs e)
//		{
//			" + tbn + @"_GetData();
//
//			// todo: init ui codes here
//		}
//		private void _" + tbn + @"_Toolbar_View_Button_Click(object sender, RoutedEventArgs e)
//		{
//			// todo: popup detail window & show selected row
//		}
//
//		private void _" + tbn + @"_Toolbar_Insert_Button_Click(object sender, RoutedEventArgs e)
//		{
//			// todo: popup detail window & let user input data & save
//		}
//
//		private void _" + tbn + @"_Toolbar_Edit_Button_Click(object sender, RoutedEventArgs e)
//		{
//			// todo: popup detail window & edit selected row & save
//		}
//
//		private void _" + tbn + @"_Toolbar_Delete_Button_Click(object sender, RoutedEventArgs e)
//		{
//			// todo: popup confirm window, if result is True, delete row & get data & refresh display
//		}
//
//		private void _" + tbn + @"_ToolbarRight_Submit_Button_Click(object sender, RoutedEventArgs e)
//		{
//			// todo: scan _" + tbn + @"_RowCheckBoxList & submit
//		}


		#endregion

		#region _" + tbn + @"_DataGrid

		private void _" + tbn + @"_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// todo: write some code here if needed
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

			this." + tbn + @"_GetData();
		}

		private void _" + tbn + @"_Pager_Previous_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex--;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData();
		}

		private void _" + tbn + @"_Pager_Next_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex++;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData();
		}

		private void _" + tbn + @"_Pager_Last_Button_Click(object sender, RoutedEventArgs e)
		{
			this." + tbn + @"_PageIndex = int.MaxValue;
			this." + tbn + @"_EnsurePagerState();

			this." + tbn + @"_GetData();
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

				this." + tbn + @"_GetData();
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

		#endregion
");

			string result_cs = sb.ToString();

			#endregion

			#region Gen Utils_CS

			sb.Remove(0, sb.Length);

			sb.Append(Gen_Utils_cs.Gen());

			string result_utils_cs = sb.ToString();

			#endregion

			#region return

			gr = new GenResult(GenResultTypes.CodeSegments);
			gr.CodeSegments = new List<KeyValuePair<string, string>>();
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML Import:", result_import));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid Style:", result_style));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid XAML:", result_xaml));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL DataGrid CS:", result_cs));
			gr.CodeSegments.Add(new KeyValuePair<string, string>("SL Utils CS:", result_utils_cs));
			return gr;

			#endregion
		}
	}
}
