using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
    public static class Gen_Database_Default_CS
    {
        public static List<KeyValuePair<string, byte[]>> Gen(Database db, string ns)
        {
            List<KeyValuePair<string, byte[]>> _Cs_KeyValue = new List<KeyValuePair<string, byte[]>>();
            _Cs_KeyValue.Add(new KeyValuePair<string, byte[]>("DefaultControl.cs", Encoding.UTF8.GetBytes(Gen_Default(ns,"DefaultControl"))));
            _Cs_KeyValue.Add(new KeyValuePair<string,byte[]>("WelcomeControl.cs",Encoding.UTF8.GetBytes(Gen_Default(ns,"WelcomeControl"))));
            return _Cs_KeyValue;
        }

        private static string Gen_Default(string ns,string ClassName)
        {
            #region 生成CS代码
            StringBuilder sb = new StringBuilder(@"
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
            public partial class " + ClassName + @": UserControl
            {
               RadWindow radWindows;
               public Brush PreferredBackground
               {
                    get { return Root.Background; }
                    set { Root.Background = value; }
               } 
               public" + ClassName+ @"()
               {
                  InitializeComponent();
    
                  WelcomeControl oControl = new WelcomeControl();
                  radWindows = new RadWindow();
                  radWindows.PreviewHidden += new EventHandler<WindowPreviewHiddenEventArgs>(radWindows_PreviewHidden);
                  radWindows.PreviewClosed+=new EventHandler<WindowPreviewClosedEventArgs>(radWindows_PreviewClosed);
                  radWindows.Hidden += new RoutedEventHandler(radWindows_Hidden);
                  radWindows.HorizontalAlignment = HorizontalAlignment.Center;
                  radWindows.VerticalAlignment = VerticalAlignment.Center;
                  radWindows.Background = GetLinearBrush(Colors.Black, Colors.White);
                  radWindows.ResizeMode = ResizeMode.NoResize;
                  radWindows.Content = oControl;
                  radWindows.ShowDialog();
                  this._T1_TreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(_T1_TreeView_SelectedItemChanged);

               }"
            );
            #endregion

            #region 事件
            sb.Append(@" 
            #region Event
            

            void _T1_TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
            {
               
            }
            private void radWindows_Hidden(object sender, RoutedEventArgs e)
            {
              (sender as RadWindow).ShowDialog();
            }
            ///<summary>
            /// RadWindows Preview Hidden
            ///</summary>
            private void radWindows_PreviewHidden(object sender, WindowPreviewHiddenEventArgs e)
            {
              if(MessageBox.Show(""是否要关闭窗体？"", ""提示"", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
              {
                radWindows.Close();
              }
            }

            ///<summary>
            /// RadWindows Preview Closed
            ///</summary>
            private void radWindows_PreviewClosed(object sender, WindowPreviewClosedEventArgs e)
            {
               System.Windows.Browser.HtmlPage.Window.Eval(""window.close()"");
            }
    
            /// <summary>
            ///  handler for ChangeSkin
            /// </summary>
            private void ChangeSkin(object sender, ExecutedRoutedEventArgs e)
            {
             RadMenuItem _item =(RadMenuItem)sender;
             this.PreferredBackground =  GetLinearBrush(Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 7, 15, 19));
             string _uri = @" + ns+";component/Skins/+"+@"_item.Tag.TosString();
             Uri uri = new Uri(_uri, UriKind.Relative);
             ImplicitStyleManager.SetResourceDictionaryUri(this.Root, uri);
             ImplicitStyleManager.SetApplyMode(Root, ImplicitStylesApplyMode.Auto);
             ImplicitStyleManager.Apply(Root);
             InitializeComponent();
             }"
            );
            #endregion 

            #region 方法
            sb.Append(@"
            /// <summary>
            ///  Ways for LinearGradientBrush 
            /// </summary>
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
  
            ///<summary>
            /// 初始化控件数据
            /// </summary>
            private  void Initaize()
            {
              
            }
            ");
            #endregion

            #region 
            sb.Append(@"
             
       }
    }"
            );
            #endregion 
            return sb.ToString();
            #endregion
        }

        private static string Gen_Login_UserConrol(string ns, string ClassName)
        {
            #region 生成CS代码
            StringBuilder sb = new StringBuilder(@"
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
            public partial class " + ClassName + @": UserControl
            {      
               public" + ClassName + @"()
               {
                  InitializeComponent();
               }"
            );
            #endregion

            #region 事件
            sb.Append(@" 
            #region Event
            ");
            #endregion

            #region 方法
            sb.Append(@"
            /// <summary>
            ///  Ways for LinearGradientBrush 
            /// </summary>
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
  
            ///<summary>
            /// 初始化控件数据
            /// </summary>
            private  void Initaize()
            {
             
            }
            ");
            #endregion
            #region
            sb.Append(@"
             
       }
    }"
            );
            #endregion
            return sb.ToString();
            #endregion

        }

    }
}
