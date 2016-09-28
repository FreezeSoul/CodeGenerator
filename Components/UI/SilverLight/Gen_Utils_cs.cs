using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.Components.UI.SilverLight
{
	public static class Gen_Utils_cs
	{
		public static string Gen()
		{
			return @"    
      public class DgWindows
	   {
		/// <summary>
		/// this ModalHost is Silverlight Widndow
		/// Writer ： 李茂
		/// The Date of  Write :2008/11/20
		/// </summary>
        public class ModalHost : UserControl
		{
			#region  Variable
			Canvas _LayoutRoot;
			UserControl _oUserControl;
			Size _oHostSize;

			public ProgressBar oBar;
			public Storyboard oTimer;
			public TextBlock myTextBloc;
			public Random x = new Random();


			bool _bMouseCapturing = false;
			Point _oLastMousePos = new Point();

			#endregion

			#region Property
			public UserControl ChildControl
			{
				get
				{
					return _oUserControl;
				}
			}
			#endregion

			#region Structure


			public ModalHost(UserControl _UserControl)
			{
				_oUserControl = _UserControl;

				_LayoutRoot = new Canvas();
				_LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));

				oTimer = new Storyboard();
				oTimer.Duration = new Duration(new TimeSpan(10));
				oTimer.Begin();

				StackPanel stackPanel = new StackPanel();
				stackPanel.Orientation = Orientation.Vertical;
				oBar = new ProgressBar();
				oBar.Foreground = new SolidColorBrush(Colors.Cyan);
				oBar.Background = GetLinearBrush(Colors.Black,Colors.White);
				oBar.BorderBrush = new SolidColorBrush(Colors.Blue);
				oBar.Maximum = 10;
				oBar.Width = 200;
				oBar.Height = 15;
				oBar.Margin = new Thickness(15);

				myTextBloc = new TextBlock();
				myTextBloc.Margin = new Thickness(15, 15, 0, 0);

				stackPanel.Children.Add(oBar);
				stackPanel.Children.Add(myTextBloc);

				_UserControl.SetValue(UserControl.ContentProperty, stackPanel);
			
				Canvas.SetLeft(_UserControl, 600);
				Canvas.SetTop(_UserControl, 400);

				_LayoutRoot.Children.Add(_UserControl);

				Content = _LayoutRoot;

				Application.Current.Host.Content.Resized += OnResized;

				Loaded += OnLoaded;
			}
			#endregion


			public void Close()
			{
				Application.Current.Host.Content.Resized -= OnResized;
				Loaded -= OnLoaded;
				_oUserControl.MouseLeftButtonDown -= OnMouseLeftButtonDown;
				_oUserControl.MouseLeftButtonUp -= OnMouseLeftButtonUp;
				_oUserControl.MouseMove -= OnMouseMove;
			}

			/// <summary>
			/// Create Windows Size ，the Windows set Left and Top of Canvas
			/// </summary>
			private void CreateWindow()
			{
				if (_oUserControl != null && !double.IsNaN(_oUserControl.Width) && !double.IsNaN(_LayoutRoot.Width))
					_oUserControl.SetValue(Canvas.LeftProperty, (_LayoutRoot.Width - _oUserControl.Width) / 2);

				if (_oUserControl != null && !double.IsNaN(_oUserControl.Height) && !double.IsNaN(_LayoutRoot.Height))
					_oUserControl.SetValue(Canvas.TopProperty, (_LayoutRoot.Height - _oUserControl.Height) / 2);
			}

			#region Loaded
			void OnLoaded(object sender, RoutedEventArgs e)
			{
				_oUserControl.MouseLeftButtonDown += OnMouseLeftButtonDown;
				_oUserControl.MouseLeftButtonUp += OnMouseLeftButtonUp;
				_oUserControl.MouseMove += OnMouseMove;

				_oUserControl.Focus();
				_oUserControl.TabNavigation = KeyboardNavigationMode.Cycle;

				OnResized(null, EventArgs.Empty);

				CreateWindow();

			}

			private static LinearGradientBrush GetLinearBrush(Color startColor, Color endColor)
			{
				LinearGradientBrush brush = new LinearGradientBrush();
				GradientStop colorStop1 = new GradientStop();
				colorStop1.Color = startColor;
				GradientStop colorStop2 = new GradientStop();
				colorStop2.Color = endColor;
				colorStop2.Offset = 1;
				brush.StartPoint = new System.Windows.Point(0.5, 1);
				brush.EndPoint = new System.Windows.Point(0.5, 0);
				brush.GradientStops = new GradientStopCollection { colorStop1, colorStop2 };
				return brush;
			}

			#endregion

			#region Resized
			void OnResized(object sender, EventArgs e)
			{
				_oHostSize.Width = Application.Current.Host.Content.ActualWidth;
				_oHostSize.Height = Application.Current.Host.Content.ActualHeight;

				_LayoutRoot.Width = _oHostSize.Width;
				_LayoutRoot.Height = _oHostSize.Height;

				CreateWindow();
			}
			#endregion


			#region Dragging Code
			void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
			{
				_bMouseCapturing = _oUserControl.CaptureMouse();
				_oLastMousePos = e.GetPosition(null);
			}

			void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
			{
				_oUserControl.ReleaseMouseCapture();
				_oLastMousePos = new Point();
				_bMouseCapturing = false;
			}

			void OnMouseMove(object sender, MouseEventArgs e)
			{
				if (_bMouseCapturing)
				{
					Point _oCurrentMousePos = e.GetPosition(null);

					double dblX = (double)_oUserControl.GetValue(Canvas.LeftProperty)
						+ _oCurrentMousePos.X - _oLastMousePos.X;

					double dblY = (double)_oUserControl.GetValue(Canvas.TopProperty)
						+ _oCurrentMousePos.Y - _oLastMousePos.Y;

					if (dblX > 0 && dblX + _oUserControl.ActualWidth < _oHostSize.Width)
						_oUserControl.SetValue(Canvas.LeftProperty, dblX);

					if (dblY > 0 && dblY + _oUserControl.ActualWidth < _oHostSize.Height)
						_oUserControl.SetValue(Canvas.TopProperty, dblY);

					_oLastMousePos = _oCurrentMousePos;

				}
			}

			#endregion

		}


		#region Modal handlers
		Popup _oPopup = null;
	    ModalHost _ModalHost;


		private void oBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_ModalHost.myTextBloc.Text = ""处理中:  "" + e.NewValue.ToString() + ""0%"";
		}

		public void oTimer_Completed(object sender, EventArgs e)
		{
			int period = _ModalHost.x.Next(1, 2) * 1;

			if (_ModalHost.oBar.Value < _ModalHost.oBar.Maximum)
			{
				System.Threading.Thread.CurrentThread.Join(period);

				_ModalHost.oBar.Value += 1;

				_ModalHost.oTimer.Begin();

			}

			if (_ModalHost.oBar.Value >= _ModalHost.oBar.Maximum)
			{
				_ModalHost.myTextBloc.Text = ""处理完成"";
				this.HideModal();
			}
		}

		public bool ShowModal()
		{
			_ModalHost = new ModalHost(new UserControl());
			_ModalHost.oBar.ValueChanged+=new RoutedPropertyChangedEventHandler<double>(oBar_ValueChanged);
			_ModalHost.oTimer.Completed +=new EventHandler(oTimer_Completed);
			_oPopup = new Popup();
			_oPopup.Child = _ModalHost;
			_oPopup.IsOpen = true;
			return true;
		}

		public UserControl HideModal()
		{
			if (_oPopup != null)
			{
				_oPopup.IsOpen = false;
				_oPopup.Child = null;
				_oPopup = null;
			}
			if (_ModalHost != null)
			{
				UserControl oRet = _ModalHost.ChildControl;
				_ModalHost.Close();
				return oRet;
			}
			else
				return null;
		}
		#endregion

	}

	public static class Utils
	{
		public static class Window
		{

			public static void Confirm(object message, EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs> handler)
			{
				Telerik.Windows.Controls.RadWindow.Confirm(message, handler);
			}
			public static void Prompt(object message, EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs> handler)
			{
				Telerik.Windows.Controls.RadWindow.Prompt(message, handler);
			}
			public static void Alert(object message)
			{
				Telerik.Windows.Controls.RadWindow.Alert(message);
			}
			public static void Alert(object message, EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs> handler)
			{
				Telerik.Windows.Controls.RadWindow.Alert(message, handler);
			}
			public static void ShowModail(double width, double height, string title, UIElement content)
			{
				Create(width, height, title, content).ShowDialog();
			}
			public static Telerik.Windows.Controls.RadWindow Create(double width, double height, string title, UIElement content)
			{
				return new Telerik.Windows.Controls.RadWindow
				{
					Width = width,
					Height = height,
					Header = title,
					Content = content,
					WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation.CenterScreen
				};
			}
		}

		public static class Clipboard
		{
			const string HostNoClipboard = ""The clipboard isn't available in the current host."";
			const string ClipboardFailure = ""The text couldn't be copied into the clipboard."";

			/// <summary>  
			/// Write to the clipboard (Internet Explorer-only)  
			/// </summary>  
			public static void SetText(string text)
			{
				// document.window.clipboardData.setData(format, data);  
				var clipboardData = (ScriptObject)HtmlPage.Window.GetProperty(""clipboardData"");
				if (clipboardData != null)
				{
					bool success = (bool)clipboardData.Invoke(""setData"", ""text"", text);
					if (!success)
					{
						HtmlPage.Window.Alert(ClipboardFailure);
					}
				}
				else
				{
					HtmlPage.Window.Alert(HostNoClipboard);
				}

			}
		}

		public static class DataGrid
		{
			/// <summary>
			/// 为 DataGrid 附加点击单元格复制到 Clipboard 和 ToolTip 的功能
			/// </summary>
			public static void AttachToolTipAndClipboardCopy(System.Windows.Controls.DataGrid dg)
			{
				dg.LoadingRow += (sender1, e1) =>
				{
					e1.Row.Loaded += (sender2, e2) =>
					{
						var r = sender2 as DataGridRow;
						foreach (DataGridColumn dgc in dg.Columns)
						{
							var o = dgc.GetCellContent(r);

							if (o is TextBlock)
							{
								var tb = o as TextBlock;
								tb.TextWrapping = TextWrapping.NoWrap;

								tb.MouseLeftButtonDown += (sender3, e3) =>
								{
									Utils.Clipboard.SetText((sender3 as TextBlock).Text);
								};

								var tt = new ToolTip();
								tt.Content = tb.Text;
								ToolTipService.SetToolTip(o as TextBlock, tt);
							}
						}
					};
				};
			}
		}
	}
";
		}
	}

    public static class Gen_Utils_custom
    {
        public static string Gen()
        {
            return @"
  <ResourceDictionary 
  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""    
  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
  xmlns:vsm=""clr-namespace:System.Windows;assembly=System.Windows""
  xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data""  
  xmlns:localprimitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
  xmlns:Bus_SL_NameSpace=""clr-namespace:Bus_SL"">


    <Style TargetType=""Bus_SL_NameSpace:DataGridColumnHead"">
        <Setter Property=""Foreground"" Value=""#FF444444"" />
        <Setter Property=""HorizontalContentAlignment"" Value=""Center"" />
        <Setter Property=""VerticalContentAlignment"" Value=""Center"" />
        <Setter Property=""FontSize"" Value=""10.5"" />
        <Setter Property=""FontWeight"" Value=""Bold"" />
        <Setter Property=""IsTabStop"" Value=""False"" />
        <Setter Property=""SeparatorBrush"" Value=""#FFDFE3E6"" />
        <Setter Property=""Padding"" Value=""4,4,5,4"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""localprimitives:DataGridColumnHeader"">
                    <Grid Name=""Root"">
                         <Grid.Resources>
                            <Storyboard x:Name=""TranslateSoreIcon"">
                                <DoubleAnimation Storyboard.TargetName=""SortIcon"" Storyboard.TargetProperty=""Opacity"" Duration=""0"" To=""1.0"" />
                                <DoubleAnimation Storyboard.TargetName=""SortIconTransform"" Storyboard.TargetProperty=""ScaleY"" Duration=""0"" To=""-1""/>
                            </Storyboard>
                            <Storyboard x:Name=""UnTranslateSoreIcon"">
                                <DoubleAnimation Storyboard.TargetName=""SortIcon"" Storyboard.TargetProperty=""Opacity"" Duration=""0"" To=""1.0""/>
                                <DoubleAnimation Storyboard.TargetName=""SortIconTransform"" Storyboard.TargetProperty=""ScaleY"" Duration=""0"" To=""1""/>
                            </Storyboard>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:0.1"" />
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal"" />
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <ColorAnimationUsingKeyFrames BeginTime=""0"" Duration=""0"" Storyboard.TargetName=""BackgroundRectangle"" Storyboard.TargetProperty=""(Shape.Fill).(SolidColorBrush.Color)"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FF448DCA""/>
                                        </ColorAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames BeginTime=""0"" Duration=""0"" Storyboard.TargetName=""BackgroundGradient"" Storyboard.TargetProperty=""(Shape.Fill).(GradientBrush.GradientStops)[3].(GradientStop.Color)"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#7FFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames BeginTime=""0"" Duration=""0"" Storyboard.TargetName=""BackgroundGradient"" Storyboard.TargetProperty=""(Shape.Fill).(GradientBrush.GradientStops)[2].(GradientStop.Color)"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#CCFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames BeginTime=""0"" Duration=""0"" Storyboard.TargetName=""BackgroundGradient"" Storyboard.TargetProperty=""(Shape.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Color)"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#F2FFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>

                        <Grid.RowDefinitions>
                            <RowDefinition Height=""*"" />
                            <RowDefinition Height=""*"" />
                            <RowDefinition Height=""Auto"" />
                        </Grid.RowDefinitions>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""Auto"" />
                            <ColumnDefinition Width=""*"" />
                            <ColumnDefinition Width=""Auto"" />
                        </Grid.ColumnDefinitions>

                        <Rectangle x:Name=""BackgroundRectangle"" Stretch=""Fill"" Fill=""#FF1F3B53"" Grid.ColumnSpan=""2"" Grid.RowSpan=""2""/>

                        <Rectangle x:Name=""BackgroundGradient"" Stretch=""Fill"" Grid.ColumnSpan=""2"" Grid.RowSpan=""2"">
                            <Rectangle.Fill>
                                <LinearGradientBrush StartPoint="".7,0"" EndPoint="".7,1"">
                                    <GradientStop Color=""#FFFFFFFF"" Offset=""0.015"" />
                                    <GradientStop Color=""#F9FFFFFF"" Offset=""0.375"" />
                                    <GradientStop Color=""#E5FFFFFF"" Offset=""0.6"" />
                                    <GradientStop Color=""#C6FFFFFF"" Offset=""1"" />
                                </LinearGradientBrush>
                            </Rectangle.Fill>
                        </Rectangle>

                        <ContentPresenter
                        Grid.RowSpan=""2""
                        Content=""{TemplateBinding Content}""
                        Cursor=""{TemplateBinding Cursor}""
                        HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                        VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                        Margin=""{TemplateBinding Padding}""/>

                        <Rectangle Name=""VerticalSeparator"" Grid.RowSpan=""2"" Grid.Column=""2"" Width=""1"" VerticalAlignment=""Stretch"" Fill=""{TemplateBinding SeparatorBrush}"" Visibility=""{TemplateBinding SeparatorVisibility}"" />

                        <Path Grid.RowSpan=""2"" Name=""SortIcon"" RenderTransformOrigin="".5,.5"" HorizontalAlignment=""Left"" VerticalAlignment=""Center"" Opacity=""0"" Grid.Column=""1"" Stretch=""Uniform"" Width=""8"" Data=""F1 M -5.215,6.099L 5.215,6.099L 0,0L -5.215,6.099 Z "">
                            <Path.Fill>
                                <SolidColorBrush Color=""#FF444444"" />
                            </Path.Fill>
                            <Path.RenderTransform>
                                <TransformGroup>
                                    <ScaleTransform x:Name=""SortIconTransform"" ScaleX=""1"" ScaleY=""1"" />
                                </TransformGroup>
                            </Path.RenderTransform>
                        </Path>

                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    </ResourceDictionary>  


    /// <summary>
    /// 
    ///  创建一个Custom Control
    ///  主要是修改DataGrid 每一列的Header 的Style 
    ///  Header,自定义样式,每一个Header 包括Header,和排序按钮,显示列设置
    ///  
    /// </summary>
    public class DataGridColumnHead:DataGridColumnHeader
    {
        private Storyboard _translateSortIcon;
		private bool _isTranslateSort;
		private cc_Accusation u;
		Path path;

        #region 构超函数
        public DataGridColumnHead():base()
        {
            this.DefaultStyleKey = typeof(DataGridColumnHead);
            this.Loaded += new RoutedEventHandler(DataGridColumnHead_Loaded);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(path_MouseLeftButtonDown);
        }
        #endregion 

        #region 方法

        /// <summary>
        ///  初始化要显示控件内容
        /// </summary>
        private void InitaizeUIElement()
        {

            GetVisualTreeHelperChildren();
        }

        
        /// <summary>
        /// /通过VisualTree ,查找整个DataGrid,然后找到以后,将他返回 
        /// </summary>
        /// <returns>DataGrid 对象</returns>
        private void GetVisualTreeHelperChildren()
        {
            Grid obj = (Grid)VisualTreeHelper.GetParent(this);
			DataGridColumnHeader objh = (DataGridColumnHeader)VisualTreeHelper.GetParent(obj);
			DataGridColumnHeadersPresenter obj1 = (DataGridColumnHeadersPresenter)VisualTreeHelper.GetParent(objh);
			Grid obj2 = (Grid)VisualTreeHelper.GetParent(obj1);
			Border obj3 = (Border)VisualTreeHelper.GetParent(obj2);
			DataGrid obj4 = (DataGrid)VisualTreeHelper.GetParent(obj3);
			Grid g = (Grid)VisualTreeHelper.GetParent(obj4);
			if (VisualTreeHelper.GetParent(g) is cc_Accusation)
			{
				u = (cc_Accusation)VisualTreeHelper.GetParent(g);
			}
        }

        #endregion

        #region 重载函数
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }      
        #endregion 


        #region 事件
        void DataGridColumnHead_Loaded(object sender, RoutedEventArgs e)
		{
			InitaizeUIElement();
			path = (Path)this.GetTemplateChild(""SortIcon"");
			path.MouseLeftButtonDown += new MouseButtonEventHandler(path_MouseLeftButtonDown);
		}

		void path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Grid obj = (Grid)VisualTreeHelper.GetParent(this);
			DataGridColumnHeader objh = (DataGridColumnHeader)VisualTreeHelper.GetParent(obj);
			DataGridColumnHeadersPresenter obj1 = (DataGridColumnHeadersPresenter)VisualTreeHelper.GetParent(objh);
			Grid obj2 = (Grid)VisualTreeHelper.GetParent(obj1);
			Border obj3 = (Border)VisualTreeHelper.GetParent(obj2);
			DataGrid obj4 = (DataGrid)VisualTreeHelper.GetParent(obj3);
			foreach (DataGridColumn dt in obj4.Columns)
			{
				if (dt.Header != null)
				{
					if (dt.Header.Equals(objh.Content))
					{
						xxxxx.index = obj4.Columns.IndexOf(dt);
						
					}
				}
			}
			if (!_isTranslateSort)
			{
				_translateSortIcon = (Storyboard)this.GetTemplateChild(""TranslateSoreIcon"");
				_translateSortIcon.Begin();
				_isTranslateSort = true;
				u.xxxxx_GetData(cc_Accusation.index);
			}
			else
			{
				_translateSortIcon = (Storyboard)this.GetTemplateChild(""UnTranslateSoreIcon"");
				_translateSortIcon.Begin();
				_isTranslateSort = false;
				u.xxxxxxxx_GetData(-cc_Accusation.index);
			}
			path.Opacity = 0;
		}

        #endregion 
     }
	";
        }
    }
}
