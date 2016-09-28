﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace CodeGenerator.Components.UI.SilverLight
{
    public static class Gen_Database_Default_XAML
    {
        public static List<KeyValuePair<string, byte[]>> Gen(Database db, string ns)
        {
            List<KeyValuePair<string, byte[]>> _Xaml_KeyValue = new List<KeyValuePair<string, byte[]>>();
            _Xaml_KeyValue.Add(new KeyValuePair<string, byte[]>("DefaultControl.xaml", Encoding.UTF8.GetBytes(Gen_Default(ns))));
            _Xaml_KeyValue.Add(new KeyValuePair<string,byte[]>("WelcomeControl.xaml",Encoding.UTF8.GetBytes(Gen_Login_UserControl(ns))));

            //生成Resource 文件
            _Xaml_KeyValue.Add(new KeyValuePair<string, byte[]>("BlueResource.xaml", Encoding.UTF8.GetBytes(Gen_Resource())));
            return _Xaml_KeyValue;
        }

        private static string Gen_Default(string ns)
        {
            #region 生成Xaml文件
            StringBuilder sb = new StringBuilder();

            #region NameSpace Start

            sb.Remove(0, sb.Length);

            sb.Append(@"<UserControl x:Name =""DefaultControl""
            x:Class=" + ns + @".DefaultControl
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
         	xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data""
	        xmlns:primitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
            xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
            xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
            xmlns:theming=""clr-namespace:Microsoft.Windows.Controls.Theming;assembly=Microsoft.Windows.Controls.Theming""
            xmlns:basics=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls""
            xmlns:telerik=""clr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Navigation""
            mc:Ignorable=""d"">");
            #endregion

            #region Root Grid Start
            sb.Append(@"
            <Grid x:Name=""Root"" Height=""Auto"" Width=""Auto"" Opacity=""1"">");
            #endregion
            // 布局控件 
            #region StaticPanel Start
            sb.Append(@"
              <StackPanel x:Name=""LayOutContols"">
             ");
            #endregion

            #region Head Area
            #region Head Start
            sb.Append(@"<!-- Header Area-->
              <Border Height=""50"" BorderThickness=""0,0,0,1"" x:Name=""HeaderBorder"" Padding=""10,0,10,0"">
            ");
            #endregion

            //放在Bord 里面的一个Grid 
            #region Grid Start
            sb.Append(@"
                <Grid>");
            #endregion

            //Grid 里面内容
            #region TextBlock Start
            sb.Append(@"
                    <TextBlock Opacity=""1"" TextWrapping=""Wrap""  VerticalAlignment=""Bottom"" Margin=""0,0,0,5""><Run Text=""后台管理"" /><Run Text=""系统""/></TextBlock>");
            #endregion

            #region Grid End
            sb.Append(@"
                </Grid>");
            #endregion

            #region Head End
            sb.Append(@"
                </Border>"
             );
            #endregion
            #endregion

            #region Menu Area
            #region BorderMenu Start
            sb.Append(@"
                <!-- Menu Area -->
               <Border Margin=""0,5,0,5"" x:Name=""MenuBorder"">");
            #endregion

            #region RadMenu
            sb.Append(@"
              <telerik:RadMenu Orientation=""Horizontal"" >
                <telerik:RadMenuItem Header=""Item 1"">
                        <telerik:RadMenuItem Header=""Item 1.1""  />
                      <telerik:RadMenuItem Header=""Item 1.2"" />
                      <telerik:RadMenuItem Header=""Item 1.3"" />
                   </telerik:RadMenuItem>
                  <telerik:RadMenuItem Header=""Item 2"" />
                    <telerik:RadMenuItem Header=""Item 3"" />
                   <telerik:RadMenuItem Header=""Item 4""  />
               </telerik:RadMenu>");
            #endregion

            #region BorderMenu End
            sb.Append(@"
                 </Bord>");
            #endregion
            #endregion

            #region Detatil Panel
            #region Work Area Start
            sb.Append(@"
               <!--DetailPanel -- >
               <Grid x:Name=""WorkGrid"" >
    		      <Grid.ColumnDefinitions>
    			    <ColumnDefinition Width=""200""/>
                    <ColumnDefinition Width=""1""/>
    			    <ColumnDefinition Width=""*""/>
    	    	  </Grid.ColumnDefinitions>
             ");
            #endregion

            #region TreeView
            sb.Append(@"
              <Border x:Name=""DiagramBorder"" Margin=""0,0,0,0"">
		        <controls:TreeView x:Name=""_T1_TreeView"" Height=""Auto"" HorizontalAlignment=""Stretch"" Margin=""0,0,0,0"" VerticalAlignment=""Stretch"" Width=""Auto""/>
			  </Border>
               ");
            #endregion

            #region UserContol
            sb.Append(@"
                  <Border Grid.Column=""2"">
                     <basics:TabControl x:Name=""SelecttabControl"" Width=""Auto"" Height=""Auto""  Margin=""0,0,0,0"" >
                      </basics:TabControl>
                  </Border>
                  <basics:GridSplitter Grid.Column=""1"" HorizontalAlignment=""Stretch"" Width=""1"" Cursor=""None""/>"
                );
            #endregion

            #region WorkArea End
            sb.Append(@"
               </Grid>     
             ");
            #endregion
            #endregion

            #region StaticPanel End
            sb.Append(@"
               </StackPanel>");
            #endregion

            #region Main Grid End
            sb.Append(@"
            </Grid>
             ");
            #endregion

            #region NameSpace_End
            sb.Append(@"
</UserControl>
            ");
            #endregion
            return sb.ToString();
            #endregion
        }

        private static string Gen_Login_UserControl(string ns)
        {
            #region 生成Xaml文件
            StringBuilder sb = new StringBuilder();

            #region NameSpace_Start

            sb.Remove(0, sb.Length);

            sb.Append(@"<UserControl x:Name =""DefaultControl""
            x:Class=" + ns + @".DefaultControl
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""  
         	xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data"" 
	        xmlns:primitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
            xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
            xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
            xmlns:theming=""clr-namespace:Microsoft.Windows.Controls.Theming;assembly=Microsoft.Windows.Controls.Theming""
            xmlns:basics=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls""
            xmlns:telerik=""clr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Navigation""
            mc:Ignorable=""d"">");
            #endregion

            #region 布局控件
            sb.Append(@"
            <UserControl.Resources>
              <SolidColorBrush x:Key=""""WelcomeBackgroundBrush"""" Color=""""#FF202020"""" />
              <SolidColorBrush x:Key=""""BorderBrush"""" Color=""""#FF747474"""" />
              <SolidColorBrush x:Key=""""WelcomeHeaderFontColor"""" Color=""""#FFE6E6E6"""" />
            </UserControl.Resources>
            <StackPanel >
            <!-- Header -->
            <Border Width=""""300"""" Padding=""""5,0,5,0"""" Opacity=""""0.8"""" x:Name=""""""""Header"""""""" Background=""""""""{StaticResource WelcomeBackgroundBrush}"""" BorderBrush=""{StaticResource BorderBrush}"" BorderThickness=""1,1,1,0"" CornerRadius=""5,5,0,0"" HorizontalAlignment=""Center"" Height=""76"" >
             <Grid>
                <Rectangle  Height=""69""/>
                <TextBlock Padding=""4,0,0,0"" Text=""后台管理系统"" TextWrapping=""Wrap"" Foreground=""{StaticResource WelcomeHeaderFontColor}"" FontSize=""18"" FontWeight=""Bold"" x:Name=""HeaderTextBlock"" d:LayoutOverrides=""VerticalAlignment, Height"" HorizontalAlignment=""Left"" Margin=""0.5,0,0,4"" VerticalAlignment=""Bottom""/>
             </Grid>
           </Border>
           <!-- Content -->
           <Border BorderBrush=""{StaticResource BorderBrush}"" BorderThickness=""1,1,1,1"" Height=""187"">
            <Grid Margin=""0,0,0,0"" x:Name=""ContentGrid"" >
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width=""0.5*""/>
                        <ColumnDefinition Width=""0.5*""/>
                    </Grid.ColumnDefinitions>
                    <Grid.RowDefinitions>
                        <RowDefinition Height=""Auto""/>
                        <RowDefinition Height=""Auto""/>
                        <RowDefinition Height=""Auto""/>
						<RowDefinition Height=""Auto""/>
                    </Grid.RowDefinitions>

                    <StackPanel Grid.Row=""1"" Margin=""11,8,0,0"">
                        <TextBlock Text=""账号"" Foreground=""#FFB5C8D8"" Margin=""1,0,0,15"" />
                        <TextBlock Text=""密码"" Foreground=""#FFB5C8D8"" Margin=""0,0,0,15""/>
						<TextBlock Text= ""验证码"" Foreground = ""#FFB5C8D8"" Margin = ""0,0,0,15"" />
                    </StackPanel>

                  <StackPanel Grid.Row=""1"" Grid.Column=""1"" Margin =""0,8,0,0"">
                     <TextBox TextWrapping=""Wrap"" x:Name=""NameInputTextBox"" HorizontalAlignment=""Left"" Width=""130"" Margin=""5,0,0,5"" TabIndex=""0""/>
                     <PasswordBox x:Name=""InputPasswordBox"" HorizontalAlignment=""Left"" Width=""130"" Margin=""5,0,0,5"" TabIndex=""1""/>
					 <TextBox TextWrapping=""Wrap"" x:Name=""InputTextBox"" HorizontalAlignment=""Left"" Width=""130"" Margin=""5,0,0,5"" TabIndex=""2"" />
                   </StackPanel>

                  <StackPanel Grid.Row=""2"" >
				      <Button Content=""刷新""/>
                   </StackPanel>
				   
				   <StackPanel Grid.Row=""2"" Grid.Column =""1"">
				      <TextBlock Text=""JDDDD"" Margin=""10,0,0,0"" Width=""124"" Padding=""40,0,0,0"" />
				   </StackPanel>

                    <StackPanel Grid.Row=""3"" Margin=""15,0,36,-16"" Orientation=""Horizontal"" VerticalAlignment=""Bottom"" Height=""50"" Grid.ColumnSpan=""2"">
                        <Button Content=""登陆"" x:Name=""AddButton""  TabIndex=""140""  />
                        <Button Content=""退出"" x:Name=""CloseButton"" />
                    </StackPanel>
                </Grid>
             </Border>

            <!-- Footer -->
            <Border Background=""{StaticResource WelcomeBackgroundBrush}"" Height=""35"" x:Name=""Footer"" Opacity=""0.8"" BorderBrush=""{StaticResource BorderBrush}"" BorderThickness=""1,0,1,1"" CornerRadius=""0,0,5,5"">
              <TextBlock x:Name=""VersionLabel"" Margin=""22,0,0,0"" Foreground=""#FFB5C8D8"" Text=""版本 2.0"" HorizontalAlignment=""Left"" VerticalAlignment=""Center""/>
            </Border>

           </StackPanel>
            ");
            #endregion

            #region NameSpace_End
            sb.Append(@"
             </UserControl>  
            ");
            #endregion
            return sb.ToString();
            #endregion
        }

        private static string Gen_Table_UserControl(Table table, string ns)
        {
            return null;
        }

        private static string Gen_Resource()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"
<ResourceDictionary
  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
  xmlns:vsm=""clr-namespace:System.Windows;assembly=System.Windows""
  xmlns:basics=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls""
  xmlns:controls=""clr-namespace:Microsoft.Windows.Controls;assembly=Microsoft.Windows.Controls""
  xmlns:input=""clr-namespace:Microsoft.Windows.Controls;assembly=Microsoft.Windows.Controls.Input""
  xmlns:System_Windows_Controls_Primitives=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls""
  xmlns:data=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data""
  xmlns:System_Windows_Controls_Primitives1=""clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Data""
  xmlns:datavis=""clr-namespace:Microsoft.Windows.Controls.DataVisualization;assembly=Microsoft.Windows.Controls.DataVisualization"">


    <!--SHINY BLUE SETTINGS FOR CHARTS-->
    <LinearGradientBrush x:Key=""ShinyChartOrange"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFFDDBAE"" Offset=""0"" />
        <GradientStop Color=""#FFCE955A"" Offset=""0.185"" />
        <GradientStop Color=""#FFAB7547"" Offset=""0.475"" />
        <GradientStop Color=""#FF704D28"" Offset=""1"" />
    </LinearGradientBrush>


    <LinearGradientBrush x:Key=""ShinyChartYellow"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFFFFF72"" Offset=""0"" />
        <GradientStop Color=""#FFC0C256"" Offset=""0.17"" />
        <GradientStop Color=""#FFCAC64C"" Offset=""0.49"" />
        <GradientStop Color=""#FF8F8832"" Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShinyChartCyan"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFBBF4FD"" Offset=""0"" />
        <GradientStop Color=""#FF62B8BC"" Offset=""0.185"" />
        <GradientStop Color=""#FF5A9399"" Offset=""0.475"" />
        <GradientStop Color=""#FF355E60"" Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShinyChartGreen"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFBAF9DA"" Offset=""0"" />
        <GradientStop Color=""#FF62BC8A"" Offset=""0.185"" />
        <GradientStop Color=""#FF5A996D"" Offset=""0.475"" />
        <GradientStop Color=""#FF35603A"" Offset=""1"" />
    </LinearGradientBrush>


    <LinearGradientBrush x:Key=""ChartBorder"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFBBBBBB"" />
        <GradientStop Color=""#FF737373"" Offset=""0.160"" />
        <GradientStop Color=""#FF646464"" Offset=""0.162"" />
        <GradientStop Color=""#FF000000"" Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShinyBackground"" EndPoint=""0.5,1"" StartPoint=""0.5,0"">
        <GradientStop Color=""#FFBBBBBB"" />
        <GradientStop Color=""#FF000000"" Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""PlotAreaBrush"" EndPoint=""0.5,0.987"" StartPoint=""0.5,0.03"">
        <GradientStop Color=""#FFFFFFFF""/>
        <GradientStop Color=""#FFFFFFFF"" Offset=""1""/>
        <GradientStop Color=""#FEDBDBDB"" Offset=""0.196""/>
        <GradientStop Color=""#FEE7E7E7"" Offset=""0.121""/>
        <GradientStop Color=""#FEF4F4F4"" Offset=""0.058""/>
    </LinearGradientBrush>

    <Color x:Key=""ShinyTopGradientOrange"">#FFFDDBAE</Color>
    <Color x:Key=""ShinyBottomGradientOrange"">#FF704D28</Color>
    <RadialGradientBrush x:Key=""ShinyPieDataPointBrushOrange"">
        <RadialGradientBrush.RelativeTransform>
            <TransformGroup>
                <ScaleTransform CenterX=""0.5"" CenterY=""0.5"" ScaleX=""2.09"" ScaleY=""1.819""/>
                <SkewTransform CenterX=""0.5"" CenterY=""0.5""/>
                <RotateTransform CenterX=""0.5"" CenterY=""0.5""/>
                <TranslateTransform X=""-0.425"" Y=""-0.486""/>
            </TransformGroup>
        </RadialGradientBrush.RelativeTransform>
        <GradientStop Color=""{StaticResource ShinyTopGradientOrange}""/>
        <GradientStop Color=""{StaticResource ShinyBottomGradientOrange}"" Offset=""1""/>
    </RadialGradientBrush>


    <Color x:Key=""ShinyTopGradientYellow"">#FFFFFF72</Color>
    <Color x:Key=""ShinyBottomGradientYellow"">#FF8F8832</Color>
    <RadialGradientBrush x:Key=""ShinyPieDataPointBrushYellow"">
        <RadialGradientBrush.RelativeTransform>
            <TransformGroup>
                <ScaleTransform CenterX=""0.5"" CenterY=""0.5"" ScaleX=""2.09"" ScaleY=""1.819""/>
                <SkewTransform CenterX=""0.5"" CenterY=""0.5""/>
                <RotateTransform CenterX=""0.5"" CenterY=""0.5""/>
                <TranslateTransform X=""-0.425"" Y=""-0.486""/>
            </TransformGroup>
        </RadialGradientBrush.RelativeTransform>
        <GradientStop Color=""{StaticResource ShinyTopGradientYellow}""/>
        <GradientStop Color=""{StaticResource ShinyBottomGradientYellow}"" Offset=""1""/>
    </RadialGradientBrush>

    <Color x:Key=""ShinyTopGradientCyan"">#FFBBF4FD</Color>
    <Color x:Key=""ShinyBottomGradientCyan"">#FF355E60</Color>
    <RadialGradientBrush x:Key=""ShinyPieDataPointBrushCyan"">
        <RadialGradientBrush.RelativeTransform>
            <TransformGroup>
                <ScaleTransform CenterX=""0.5"" CenterY=""0.5"" ScaleX=""2.09"" ScaleY=""1.819""/>
                <SkewTransform CenterX=""0.5"" CenterY=""0.5""/>
                <RotateTransform CenterX=""0.5"" CenterY=""0.5""/>
                <TranslateTransform X=""-0.425"" Y=""-0.486""/>
            </TransformGroup>
        </RadialGradientBrush.RelativeTransform>
        <GradientStop Color=""{StaticResource ShinyTopGradientCyan}""/>
        <GradientStop Color=""{StaticResource ShinyBottomGradientCyan}"" Offset=""1""/>
    </RadialGradientBrush>

    <Color x:Key=""ShinyTopGradientGreen"">#FFBAF9DA</Color>
    <Color x:Key=""ShinyBottomGradientGreen"">#FF35603A</Color>
    <RadialGradientBrush x:Key=""ShinyPieDataPointBrushGreen"">
        <RadialGradientBrush.RelativeTransform>
            <TransformGroup>
                <ScaleTransform CenterX=""0.5"" CenterY=""0.5"" ScaleX=""2.09"" ScaleY=""1.819""/>
                <SkewTransform CenterX=""0.5"" CenterY=""0.5""/>
                <RotateTransform CenterX=""0.5"" CenterY=""0.5""/>
                <TranslateTransform X=""-0.425"" Y=""-0.486""/>
            </TransformGroup>
        </RadialGradientBrush.RelativeTransform>
        <GradientStop Color=""{StaticResource ShinyTopGradientGreen}""/>
        <GradientStop Color=""{StaticResource ShinyBottomGradientGreen}"" Offset=""1""/>
    </RadialGradientBrush>


    <!--SHINY BLUE-->
    <Color x:Key=""TextBrush"">#FF000000</Color>

    <Color x:Key=""NormalBrushGradient1"">#FFBAE4FF</Color>
    <Color x:Key=""NormalBrushGradient2"">#FF398FDF</Color>
    <Color x:Key=""NormalBrushGradient3"">#FF006DD4</Color>
    <Color x:Key=""NormalBrushGradient4"">#FF0A3E69</Color>

    <Color x:Key=""NormalBorderBrushGradient1"">#FFBBBBBB</Color>
    <Color x:Key=""NormalBorderBrushGradient2"">#FF737373</Color>
    <Color x:Key=""NormalBorderBrushGradient3"">#FF646464</Color>
    <Color x:Key=""NormalBorderBrushGradient4"">#FF000000</Color>

    <Color x:Key=""SelectedBackgroundGradient1"">#FFBBBBBB</Color>
    <Color x:Key=""SelectedBackgroundGradient2"">#FF737373</Color>
    <Color x:Key=""SelectedBackgroundGradient3"">#FF646464</Color>
    <Color x:Key=""SelectedBackgroundGradient4"">#FFA1A1A1</Color>

    <Color x:Key=""SliderBorderGradient1"">#FF3F3F3F</Color>
    <Color x:Key=""SliderBorderGradient2"">#FFADADAD</Color>

    <Color x:Key=""ShadeBrushGradient1"">#FF62676A</Color>
    <Color x:Key=""ShadeBrushGradient2"">#FFD1D4D6</Color>
    <Color x:Key=""ShadeBrushGradient3"">#FFFFFFFF</Color>

    <Color x:Key=""WindowBackgroundBrushGradient1"">#FFD1D1D1</Color>
    <Color x:Key=""WindowBackgroundBrushGradient2"">#FF8496AA</Color>


    <LinearGradientBrush x:Key=""NormalBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource NormalBrushGradient1}""
                  Offset=""0"" />
        <GradientStop Color=""{StaticResource NormalBrushGradient2}""
                  Offset=""0.41800001263618469"" />
        <GradientStop Color=""{StaticResource NormalBrushGradient3}""
                  Offset=""0.418"" />
        <GradientStop Color=""{StaticResource NormalBrushGradient4}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""NormalBorderBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource NormalBorderBrushGradient1}"" />
        <GradientStop Color=""{StaticResource NormalBorderBrushGradient2}""
                  Offset=""0.38"" />
        <GradientStop Color=""{StaticResource NormalBorderBrushGradient3}""
                  Offset=""0.384"" />
        <GradientStop Color=""{StaticResource NormalBorderBrushGradient4}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <RadialGradientBrush x:Key=""HoverBrush"">
        <RadialGradientBrush.RelativeTransform>
            <TransformGroup>
                <ScaleTransform CenterX=""0.5""
                        CenterY=""0.5""
                        ScaleX=""1.804""
                        ScaleY=""0.743"" />
                <SkewTransform CenterX=""0.5""
                       CenterY=""0.5"" />
                <RotateTransform CenterX=""0.5""
                         CenterY=""0.5"" />
                <TranslateTransform Y=""0.47999998927116394"" />
            </TransformGroup>
        </RadialGradientBrush.RelativeTransform>
        <GradientStop Color=""#FF98DAFF""
                  Offset=""0.209"" />
        <GradientStop Color=""#0098DAFF""
                  Offset=""1"" />
        <GradientStop Color=""#FFFFFFFF""
                  Offset=""0"" />
    </RadialGradientBrush>

    <LinearGradientBrush x:Key=""CheckIconBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""#FF006CD1"" />
        <GradientStop Color=""#FFA5D6F9""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShadeBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource ShadeBrushGradient2}""
                  Offset=""0"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""0.1"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShadeBrushTop""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource ShadeBrushGradient2}""
                  Offset=""0"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""0.1"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShadeBrushBottom""
                       EndPoint=""0.5,0""
                       StartPoint=""0.5,1"">
        <GradientStop Color=""{StaticResource ShadeBrushGradient2}"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""0.1"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShadeBrushLeft""
                       EndPoint=""1,0.5""
                       StartPoint=""0,0.5"">
        <GradientStop Color=""{StaticResource ShadeBrushGradient2}"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""0.1"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""ShadeBrushRight""
                       EndPoint=""0,0.5""
                       StartPoint=""1,0.5"">
        <GradientStop Color=""{StaticResource ShadeBrushGradient2}"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""0.1"" />
        <GradientStop Color=""{StaticResource ShadeBrushGradient3}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""DisabledBackgroundBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""#FFFFFFFF"" />
        <GradientStop Color=""#FF62676A""
                  Offset=""1"" />
        <GradientStop Color=""#FFD1D4D6""
                  Offset=""0.41800001263618469"" />
        <GradientStop Color=""#FFA9AFB5""
                  Offset=""0.425"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""SelectedBackgroundBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient1}"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient2}""
                  Offset=""0.38"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient3}""
                  Offset=""0.384"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient4}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""SelectedBackgroundBrushVertical""
                       EndPoint=""2.05,0.5""
                       StartPoint=""-0.55,0.5"">
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient1}"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient2}""
                  Offset=""0.37999999523162842"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient3}""
                  Offset=""0.38400000333786011"" />
        <GradientStop Color=""{StaticResource SelectedBackgroundGradient4}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""HorizontalSliderBorderBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource SliderBorderGradient1}"" />
        <GradientStop Color=""{StaticResource SliderBorderGradient2}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""VerticalSliderBorderBrush""
                       EndPoint=""1.35,0.5""
                       StartPoint=""0.6,0.5"">
        <GradientStop Color=""{StaticResource SliderBorderGradient1}"" />
        <GradientStop Color=""{StaticResource SliderBorderGradient2}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <LinearGradientBrush x:Key=""WindowBackgroundBrush""
                       EndPoint=""0.5,1""
                       StartPoint=""0.5,0"">
        <GradientStop Color=""{StaticResource WindowBackgroundBrushGradient1}"" />
        <GradientStop Color=""{StaticResource WindowBackgroundBrushGradient2}""
                  Offset=""1"" />
    </LinearGradientBrush>

    <!--Button-->
    <Style TargetType=""Button"">
        <Setter Property=""Background""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
        <Setter Property=""Padding""
            Value=""3""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""Button"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Pressed""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Pressed""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""MouseOver""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Hover""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""Background""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.7""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Background""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""2,2,2,2"">
                            <Border x:Name=""Hover""
                      Background=""{StaticResource HoverBrush}""
                      CornerRadius=""2,2,2,2""
                      Height=""Auto""
                      Width=""Auto""
                      Opacity=""0""/>
                        </Border>
                        <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                        <Border x:Name=""DisabledVisualElement""
                    IsHitTestVisible=""false""
                    Opacity=""0""
                    Background=""{StaticResource DisabledBackgroundBrush}""
                    CornerRadius=""2,2,2,2""/>
                        <Border x:Name=""FocusVisualElement""
                    IsHitTestVisible=""false""
                    Visibility=""Collapsed""
                    BorderBrush=""{StaticResource HoverBrush}""
                    BorderThickness=""2,2,2,2""
                    CornerRadius=""2,2,2,2""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--Checkbox-->
    <Style  TargetType=""CheckBox"">
        <Setter Property=""Background""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""Foreground""
            Value=""{StaticResource TextBrush}"" />
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Top""/>
        <Setter Property=""Padding""
            Value=""4,1,0,0""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""CheckBox"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Pressed""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Pressed""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""MouseOver""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundOverlay""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""CheckIcon""
                                                   Storyboard.TargetProperty=""(Shape.StrokeThickness)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.3""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""Background""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.7""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""CheckStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Checked""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unchecked""/>
                                    <vsm:VisualTransition From=""Unchecked""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Checked""/>
                                    <vsm:VisualTransition From=""Checked""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Indeterminate""/>
                                    <vsm:VisualTransition From=""Indeterminate""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unchecked""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Checked"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""CheckIcon""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unchecked""/>
                                <vsm:VisualState x:Name=""Indeterminate"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""IndeterminateIcon""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ContentFocusVisualElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""16""/>
                            <ColumnDefinition Width=""*""/>
                        </Grid.ColumnDefinitions>
                        <Grid HorizontalAlignment=""Left""
                  VerticalAlignment=""Top"">
                            <Rectangle Height=""14""
                         Margin=""1""
                         x:Name=""Background""
                         Width=""14""
                         Fill=""{TemplateBinding Background}""
                         Stroke=""{TemplateBinding BorderBrush}""
                         StrokeThickness=""{TemplateBinding BorderThickness}""
                         RadiusX=""1""
                         RadiusY=""1""/>
                            <Rectangle Height=""12""
                         x:Name=""BackgroundOverlay""
                         Width=""12""
                         Opacity=""0""
                         Fill=""{StaticResource HoverBrush}""
                         Stroke=""{x:Null}""
                         StrokeThickness=""1""
                         RadiusX=""1""
                         RadiusY=""1""/>

                            <Path x:Name=""CheckIcon""
                    Stretch=""Fill""
                    Data=""M102.03442,598.79645 L105.22962,597.78918 L106.95686,599.19977 C106.95686,599.19977 113.77958,590.53656 113.77958,590.53656 C113.77958,590.53656 107.40649,603.79431 107.40649,603.79431 z""
                    Opacity=""0""
                    Fill=""#FFFFFFFF""
                    Height=""10""
                    Width=""10.5""
                    Stroke=""{StaticResource CheckIconBrush}""
                    StrokeThickness=""0""/>

                            <Rectangle Height=""9""
                         x:Name=""IndeterminateIcon""
                         Width=""9""
                         Opacity=""0""
                         Fill=""#FFFFFFFF""
                         Stroke=""{StaticResource CheckIconBrush}""
                         RadiusX=""1""
                         RadiusY=""1""/>
                            <Rectangle Height=""14""
                         x:Name=""DisabledVisualElement""
                         Width=""14""
                         Opacity=""0""
                         Fill=""{StaticResource DisabledBackgroundBrush}""
                         RadiusX=""1""
                         RadiusY=""1""/>
                            <Rectangle Height=""16""
                         x:Name=""ContentFocusVisualElement""
                         Width=""16""
                         IsHitTestVisible=""false""
                         Opacity=""0""
                         Stroke=""{StaticResource HoverBrush}""
                         StrokeThickness=""1""
                         RadiusX=""2""
                         RadiusY=""2""/>
                        </Grid>
                        <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Grid.Column=""1""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--RadioButton-->
    <Style TargetType=""RadioButton"">
        <Setter Property=""Background""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""Foreground""
            Value=""{StaticResource TextBrush}"" />
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Top""/>
        <Setter Property=""Padding""
            Value=""4,1,0,0""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""RadioButton"">
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""16""/>
                            <ColumnDefinition Width=""*""/>
                        </Grid.ColumnDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Pressed""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Pressed""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""MouseOver""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundOverlay""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.35""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""Background""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.7""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""CheckStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Checked""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unchecked""/>
                                    <vsm:VisualTransition From=""Unchecked""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Checked""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Checked"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""CheckIcon""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unchecked"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""CheckIcon""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ContentFocusVisualElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid HorizontalAlignment=""Left""
                  VerticalAlignment=""Top"">
                            <Ellipse Height=""14""
                       Margin=""1""
                       x:Name=""Background""
                       Width=""14""
                       Fill=""{StaticResource NormalBrush}""
                       Stroke=""{TemplateBinding BorderBrush}""
                       StrokeThickness=""{TemplateBinding BorderThickness}""/>
                            <Ellipse Height=""12""
                       x:Name=""BackgroundOverlay""
                       Width=""12""
                       Opacity=""0""
                       Fill=""#FFC4DBEE""
                       Stroke=""#00000000""
                       Margin=""2,2,2,2""
                       StrokeThickness=""0""/>
                            <Border HorizontalAlignment=""Center""
                      VerticalAlignment=""Center""
                      Width=""6""
                      Height=""6""
                      UseLayoutRounding=""False""
                      CornerRadius=""1,1,1,1""
                      BorderThickness=""1,1,1,1""
                      Background=""#FFFFFFFF""
                      x:Name=""CheckIcon""
                      BorderBrush=""{StaticResource CheckIconBrush}""
                      Opacity=""0""/>
                            <Ellipse Height=""14""
                       x:Name=""DisabledVisualElement""
                       Width=""14""
                       Opacity=""0""
                       Fill=""{StaticResource DisabledBackgroundBrush}""/>
                            <Ellipse Height=""16""
                       x:Name=""ContentFocusVisualElement""
                       Width=""16""
                       IsHitTestVisible=""false""
                       Opacity=""0""
                       Stroke=""{StaticResource HoverBrush}""
                       StrokeThickness=""1""/>
                        </Grid>
                        <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Grid.Column=""1""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--ScrollBar-->
    <Style  TargetType=""ScrollBar"">
        <Setter Property=""MinWidth""
            Value=""17"" />
        <Setter Property=""MinHeight""
            Value=""17"" />
        <Setter Property=""IsTabStop""
            Value=""False"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ScrollBar"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <ControlTemplate x:Key=""RepeatButtonTemplate""
                               TargetType=""RepeatButton"">
                                <Grid x:Name=""Root""
                      Background=""Transparent"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualState x:Name=""Normal"" />
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""HorizontalIncrementTemplate""
                               TargetType=""RepeatButton"">
                                <Grid x:Name=""Root"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"" />
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>

                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledElement""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.65"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background""
                             Fill=""{StaticResource NormalBrush}""
                             StrokeThickness=""2""
                             RadiusX=""2""
                             RadiusY=""2""
                             Stroke=""{StaticResource NormalBorderBrush}"" />
                                    <Rectangle x:Name=""BackgroundAnimation""
                             Opacity=""0""
                             Fill=""{StaticResource HoverBrush}""
                             Stroke=""{x:Null}""
                             StrokeThickness=""0""
                             RadiusX=""1""
                             RadiusY=""1""
                             Margin=""2,2,2,2"" />
                                    <Path Height=""8""
                        Width=""4""
                        Stretch=""Uniform""
                        Data=""F1 M 511.047,352.682L 511.047,342.252L 517.145,347.467L 511.047,352.682 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FFFFFFFF""
                                       x:Name=""ButtonColor"" />
                                        </Path.Fill>
                                    </Path>
                                    <Rectangle x:Name=""DisabledElement""
                             Opacity=""0""
                             Fill=""{StaticResource DisabledBackgroundBrush}""
                             RadiusX=""3""
                             RadiusY=""3"" />
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""HorizontalDecrementTemplate""
                               TargetType=""RepeatButton"">
                                <Grid x:Name=""Root"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"" />
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledElement""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.65"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background""
                             Fill=""{StaticResource NormalBrush}""
                             StrokeThickness=""2""
                             RadiusX=""2""
                             RadiusY=""2""
                             Stroke=""{StaticResource NormalBorderBrush}"" />
                                    <Rectangle x:Name=""BackgroundMouseOver""
                             Opacity=""0""
                             Fill=""{StaticResource HoverBrush}""
                             Stroke=""{x:Null}""
                             StrokeThickness=""0""
                             RadiusX=""1""
                             RadiusY=""1""
                             Margin=""2,2,2,2"" />
                                    <Path Height=""8""
                        Width=""4""
                        Stretch=""Uniform""
                        Data=""F1 M 110.692,342.252L 110.692,352.682L 104.594,347.467L 110.692,342.252 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FFFFFFFF""
                                       x:Name=""ButtonColor"" />
                                        </Path.Fill>
                                    </Path>
                                    <Rectangle x:Name=""DisabledElement""
                             Opacity=""0""
                             Fill=""{StaticResource DisabledBackgroundBrush}""
                             RadiusX=""3""
                             RadiusY=""3"" />
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""VerticalIncrementTemplate""
                               TargetType=""RepeatButton"">
                                <Grid x:Name=""Root"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"" />
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledElement""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.65"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background""
                             Fill=""{StaticResource NormalBrush}""
                             StrokeThickness=""2""
                             RadiusX=""2""
                             RadiusY=""2""
                             Stroke=""{StaticResource NormalBorderBrush}"" />
                                    <Rectangle x:Name=""BackgroundMouseOver""
                             Opacity=""0""
                             Fill=""{StaticResource HoverBrush}""
                             Stroke=""{x:Null}""
                             RadiusX=""1""
                             RadiusY=""1""
                             StrokeThickness=""0""
                             Margin=""2,2,2,2"" />
                                    <Path Height=""4""
                        Width=""8""
                        Stretch=""Uniform""
                        Data=""F1 M 531.107,321.943L 541.537,321.943L 536.322,328.042L 531.107,321.943 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FFFFFFFF""
                                       x:Name=""ButtonColor"" />
                                        </Path.Fill>
                                    </Path>
                                    <Rectangle x:Name=""DisabledElement""
                             Opacity=""0""
                             Fill=""{StaticResource DisabledBackgroundBrush}""
                             RadiusX=""3""
                             RadiusY=""3"" />
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""VerticalDecrementTemplate""
                               TargetType=""RepeatButton"">
                                <Grid x:Name=""Root"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"" />
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledElement""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0.65"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background""
                             Fill=""{StaticResource NormalBrush}""
                             StrokeThickness=""2""
                             RadiusX=""2""
                             RadiusY=""2""
                             Stroke=""{StaticResource NormalBorderBrush}"" />
                                    <Rectangle x:Name=""BackgroundMouseOver""
                             Fill=""{StaticResource HoverBrush}""
                             Stroke=""{x:Null}""
                             RadiusX=""1""
                             RadiusY=""1""
                             Margin=""2,2,2,2""
                             Opacity=""1"" />
                                    <Path Height=""4""
                        Width=""8""
                        Stretch=""Uniform""
                        Data=""F1 M 541.537,173.589L 531.107,173.589L 536.322,167.49L 541.537,173.589 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FFFFFFFF""
                                       x:Name=""ButtonColor"" />
                                        </Path.Fill>
                                    </Path>
                                    <Rectangle x:Name=""DisabledElement""
                             Opacity=""0""
                             Fill=""{StaticResource DisabledBackgroundBrush}""
                             RadiusX=""3""
                             RadiusY=""3"" />
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""VerticalThumbTemplate""
                               TargetType=""Thumb"">
                                <Grid>
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ThumbVisual""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Grid Margin=""1,0,1,0""
                        x:Name=""ThumbVisual"">
                                        <Rectangle x:Name=""Background""
                               Fill=""{StaticResource NormalBrush}""
                               StrokeThickness=""2""
                               RadiusX=""2""
                               RadiusY=""2""
                               Stroke=""{StaticResource NormalBorderBrush}"" />
                                        <Rectangle x:Name=""BackgroundMouseOver""
                               Fill=""{StaticResource HoverBrush}""
                               Stroke=""{x:Null}""
                               StrokeThickness=""1""
                               RadiusX=""1""
                               RadiusY=""1""
                               Margin=""2,2,2,2"" />
                                    </Grid>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""HorizontalThumbTemplate""
                               TargetType=""Thumb"">
                                <Grid>
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed"" />
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal"" />
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver"" />
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed"" />
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal"" />
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundMouseOver""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""1"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0.7"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ThumbVisual""
                                                         Storyboard.TargetProperty=""Opacity"">
                                                        <SplineDoubleKeyFrame KeyTime=""0:0:0""
                                                  Value=""0"" />
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Grid Margin=""0,1,0,1""
                        x:Name=""ThumbVisual"">
                                        <Rectangle x:Name=""Background""
                               Fill=""{StaticResource NormalBrush}""
                               StrokeThickness=""2""
                               RadiusX=""2""
                               RadiusY=""2""
                               Stroke=""{StaticResource NormalBorderBrush}"" />
                                        <Rectangle x:Name=""BackgroundMouseOver""
                               Opacity=""0""
                               Fill=""{StaticResource HoverBrush}""
                               Stroke=""{x:Null}""
                               StrokeThickness=""0""
                               RadiusX=""1""
                               RadiusY=""1""
                               Margin=""2,2,2,2"" />
                                    </Grid>
                                </Grid>
                            </ControlTemplate>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Normal"" />
                                <vsm:VisualState x:Name=""MouseOver"" />
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Root""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.5"" />
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid x:Name=""HorizontalRoot"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""Auto"" />
                                <ColumnDefinition Width=""Auto"" />
                                <ColumnDefinition Width=""Auto"" />
                                <ColumnDefinition Width=""*"" />
                                <ColumnDefinition Width=""Auto"" />
                            </Grid.ColumnDefinitions>
                            <Rectangle Grid.ColumnSpan=""5""
                         Stroke=""#00000000""
                         StrokeThickness=""1""
                         RadiusX=""3""
                         RadiusY=""3""
                         Fill=""#FFDDDEDF"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource HorizontalDecrementTemplate}""
                            Margin=""1""
                            x:Name=""HorizontalSmallDecrease""
                            Width=""16""
                            Grid.Column=""0""
                            Interval=""50"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""HorizontalLargeDecrease""
                            Width=""0""
                            Grid.Column=""1""
                            Interval=""50"" />
                            <Thumb Background=""{TemplateBinding Background}""
                     Template=""{StaticResource HorizontalThumbTemplate}""
                     MinWidth=""18""
                     x:Name=""HorizontalThumb""
                     Width=""18""
                     Grid.Column=""2"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""HorizontalLargeIncrease""
                            Grid.Column=""3""
                            Interval=""50"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource HorizontalIncrementTemplate}""
                            Margin=""1""
                            x:Name=""HorizontalSmallIncrease""
                            Width=""16""
                            Grid.Column=""4""
                            Interval=""50"" />
                        </Grid>
                        <Grid x:Name=""VerticalRoot""
                  Visibility=""Collapsed"">
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""Auto"" />
                                <RowDefinition Height=""Auto"" />
                                <RowDefinition Height=""Auto"" />
                                <RowDefinition Height=""*"" />
                                <RowDefinition Height=""Auto"" />
                            </Grid.RowDefinitions>
                            <Rectangle Grid.RowSpan=""5""
                         Stroke=""#00000000""
                         StrokeThickness=""1""
                         RadiusX=""3""
                         RadiusY=""3""
                         Fill=""#FFDDDEDF"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource VerticalDecrementTemplate}""
                            Height=""16""
                            Margin=""1""
                            x:Name=""VerticalSmallDecrease""
                            Grid.Row=""0""
                            Interval=""50"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            Height=""0""
                            x:Name=""VerticalLargeDecrease""
                            Grid.Row=""1""
                            Interval=""50"" />
                            <Thumb Template=""{StaticResource VerticalThumbTemplate}""
                     Height=""18""
                     MinHeight=""18""
                     x:Name=""VerticalThumb""
                     Grid.Row=""2"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""VerticalLargeIncrease""
                            Grid.Row=""3""
                            Interval=""50"" />
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource VerticalIncrementTemplate}""
                            Height=""16""
                            Margin=""1""
                            x:Name=""VerticalSmallIncrease""
                            Grid.Row=""4""
                            Interval=""50"" />
                        </Grid>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--ScrollViewer-->
    <Style  TargetType=""ScrollViewer"">
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left"" />
        <Setter Property=""VerticalContentAlignment""
            Value=""Top"" />
        <Setter Property=""VerticalScrollBarVisibility""
            Value=""Visible"" />
        <Setter Property=""Padding""
            Value=""4"" />
        <Setter Property=""BorderThickness""
            Value=""1"" />
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ScrollViewer"">
                    <Border BorderBrush=""{TemplateBinding BorderBrush}""
                  BorderThickness=""{TemplateBinding BorderThickness}""
                  CornerRadius=""2"">
                        <Grid Background=""{TemplateBinding Background}"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""*"" />
                                <ColumnDefinition Width=""Auto"" />
                            </Grid.ColumnDefinitions>
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""*"" />
                                <RowDefinition Height=""Auto"" />
                            </Grid.RowDefinitions>
                            <ScrollContentPresenter Cursor=""{TemplateBinding Cursor}""
                                      Margin=""{TemplateBinding Padding}""
                                      x:Name=""ScrollContentPresenter""
                                      ContentTemplate=""{TemplateBinding ContentTemplate}"" />
                            <Rectangle Grid.Column=""1""
                         Grid.Row=""1""
                         Fill=""#FFE9EEF4"" />
                            <ScrollBar IsTabStop=""False""
                         x:Name=""VerticalScrollBar""
                         Width=""18""
                         Visibility=""{TemplateBinding ComputedVerticalScrollBarVisibility}""
                         Grid.Column=""1""
                         Grid.Row=""0""
                         Orientation=""Vertical""
                         ViewportSize=""{TemplateBinding ViewportHeight}""
                         Maximum=""{TemplateBinding ScrollableHeight}""
                         Minimum=""0""
                         Value=""{TemplateBinding VerticalOffset}""
                         Style=""{StaticResource System.Windows.Controls.Primitives.ScrollBar}"" />
                            <ScrollBar IsTabStop=""False""
                         Height=""18""
                         x:Name=""HorizontalScrollBar""
                         Visibility=""{TemplateBinding ComputedHorizontalScrollBarVisibility}""
                         Grid.Column=""0""
                         Grid.Row=""1""
                         Orientation=""Horizontal""
                         ViewportSize=""{TemplateBinding ViewportWidth}""
                         Maximum=""{TemplateBinding ScrollableWidth}""
                         Minimum=""0""
                         Value=""{TemplateBinding HorizontalOffset}""
                         Style=""{StaticResource System.Windows.Controls.Primitives.ScrollBar}"" />
                        </Grid>
                    </Border>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--Listbox-->
    <Style  TargetType=""ListBox"">
        <Setter Property=""Padding""
            Value=""1""/>
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrush}""/>
        <Setter Property=""Foreground""
            Value=""#FFFFFFff""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Top""/>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""TabNavigation""
            Value=""Once""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ListBox"">
                    <Border BorderBrush=""{TemplateBinding BorderBrush}""
                  BorderThickness=""{TemplateBinding BorderThickness}""
                  CornerRadius=""2""
                  Background=""{TemplateBinding Background}"">
                        <ScrollViewer BorderBrush=""Transparent""
                          BorderThickness=""0""
                          Padding=""{TemplateBinding Padding}""
                          x:Name=""ScrollViewer""
                          Style=""{StaticResource System.Windows.Controls.ScrollViewer}"">
                            <ItemsPresenter/>
                        </ScrollViewer>
                    </Border>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--LisboxItem-->
    <Style TargetType=""ListBoxItem"">
        <Setter Property=""Padding""
            Value=""3""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Top""/>
        <Setter Property=""Background""
            Value=""Transparent""/>
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""TabNavigation""
            Value=""Local""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ListBoxItem"">
                    <Grid Background=""{TemplateBinding Background}""
                Margin=""1,1,1,1"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Unselected""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Selected""/>
                                    <vsm:VisualTransition From=""Selected""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unselected""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unselected""/>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value="".75""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""Background""
                       IsHitTestVisible=""False""
                       Fill=""{StaticResource SelectedBackgroundBrush}""
                       RadiusX=""0""/>
                        <Rectangle x:Name=""SelectedRectangle""
                       IsHitTestVisible=""False""
                       Opacity=""0""
                       Fill=""{StaticResource NormalBrush}""
                       RadiusX=""0""/>
                        <Rectangle x:Name=""HoverRectangle""
                       IsHitTestVisible=""False""
                       Fill=""{StaticResource HoverBrush}""
                       RadiusX=""0""
                       Opacity=""0""/>
                        <ContentPresenter HorizontalAlignment=""Left""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                        <Rectangle x:Name=""FocusVisualElement""
                       Visibility=""Collapsed""
                       Stroke=""{StaticResource HoverBrush}""
                       StrokeThickness=""1""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--TabControl-->
    <Style TargetType=""basics:TabControl"">
        <Setter Property=""IsTabStop""
            Value=""False"" />
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrushTop}"" />
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""BorderThickness""
            Value=""1"" />
        <Setter Property=""Padding""
            Value=""5"" />
        <Setter Property=""HorizontalContentAlignment""
            Value=""Stretch"" />
        <Setter Property=""VerticalContentAlignment""
            Value=""Stretch"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""basics:TabControl"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0"" />
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal"" />
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualTop""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualBottom""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualLeft""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualRight""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid x:Name=""TemplateTop""
                  Visibility=""Collapsed"">
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""Auto"" />
                                <RowDefinition Height=""*"" />
                            </Grid.RowDefinitions>
                            <System_Windows_Controls_Primitives:TabPanel Margin=""2,2,2,-1""
                                                           x:Name=""TabPanelTop""
                                                           Canvas.ZIndex=""1"" />
                            <Border MinHeight=""10""
                      MinWidth=""10""
                      Grid.Row=""1""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""0,0,3,3"">
                                <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                                  HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                  Margin=""{TemplateBinding Padding}""
                                  x:Name=""ContentTop""
                                  VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                            </Border>
                            <Border x:Name=""DisabledVisualTop""
                      IsHitTestVisible=""False""
                      Opacity=""0""
                      Canvas.ZIndex=""1""
                      Grid.Row=""1""
                      Grid.RowSpan=""2""
                      Background=""#8CFFFFFF""
                      CornerRadius=""0,0,3,3"" />
                        </Grid>
                        <Grid x:Name=""TemplateBottom""
                  Visibility=""Collapsed"">
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""*"" />
                                <RowDefinition Height=""Auto"" />
                            </Grid.RowDefinitions>
                            <System_Windows_Controls_Primitives:TabPanel Margin=""2,-1,2,2""
                                                           x:Name=""TabPanelBottom""
                                                           Canvas.ZIndex=""1""
                                                           Grid.Row=""1"" />
                            <Border MinHeight=""10""
                      MinWidth=""10""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""3,3,0,0""
                      Background=""{StaticResource ShadeBrushBottom}"">
                                <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                                  HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                  Margin=""{TemplateBinding Padding}""
                                  x:Name=""ContentBottom""
                                  VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                            </Border>
                            <Border x:Name=""DisabledVisualBottom""
                      IsHitTestVisible=""False""
                      Opacity=""0""
                      Canvas.ZIndex=""1""
                      Background=""#8CFFFFFF""
                      CornerRadius=""3,3,0,0"" />
                        </Grid>
                        <Grid x:Name=""TemplateLeft""
                  Visibility=""Collapsed"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""Auto"" />
                                <ColumnDefinition Width=""*"" />
                            </Grid.ColumnDefinitions>
                            <System_Windows_Controls_Primitives:TabPanel Margin=""2,2,-1,2""
                                                           x:Name=""TabPanelLeft""
                                                           Canvas.ZIndex=""1"" />
                            <Border MinHeight=""10""
                      MinWidth=""10""
                      Grid.Column=""1""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""0,3,3,0""
                      Background=""{StaticResource ShadeBrushLeft}"">
                                <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                                  HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                  Margin=""{TemplateBinding Padding}""
                                  x:Name=""ContentLeft""
                                  VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                            </Border>
                            <Border x:Name=""DisabledVisualLeft""
                      IsHitTestVisible=""False""
                      Opacity=""0""
                      Canvas.ZIndex=""1""
                      Grid.Column=""1""
                      Background=""#8CFFFFFF""
                      CornerRadius=""0,3,3,0"" />
                        </Grid>
                        <Grid x:Name=""TemplateRight""
                  Visibility=""Collapsed"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""*"" />
                                <ColumnDefinition Width=""Auto"" />
                            </Grid.ColumnDefinitions>
                            <System_Windows_Controls_Primitives:TabPanel Margin=""-1,2,2,2""
                                                           x:Name=""TabPanelRight""
                                                           Canvas.ZIndex=""1""
                                                           Grid.Column=""1"" />
                            <Border MinHeight=""10""
                      MinWidth=""10""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""3,0,0,3""
                      Background=""{StaticResource ShadeBrushRight}"">
                                <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                                  HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                  Margin=""{TemplateBinding Padding}""
                                  x:Name=""ContentRight""
                                  VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                            </Border>
                            <Border Margin=""0""
                      x:Name=""DisabledVisualRight""
                      IsHitTestVisible=""False""
                      Opacity=""0""
                      Canvas.ZIndex=""1""
                      Background=""#8CFFFFFF""
                      CornerRadius=""3,0,0,3"" />
                        </Grid>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--TabControlItem-->
    <Style TargetType=""basics:TabItem"">
        <Setter Property=""IsTabStop""
            Value=""False"" />
        <Setter Property=""Background""
            Value=""{StaticResource SelectedBackgroundBrush}"" />
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""BorderThickness""
            Value=""1"" />
        <Setter Property=""Padding""
            Value=""6,2,6,2"" />
        <Setter Property=""HorizontalContentAlignment""
            Value=""Stretch"" />
        <Setter Property=""VerticalContentAlignment""
            Value=""Stretch"" />
        <Setter Property=""MinWidth""
            Value=""5"" />
        <Setter Property=""MinHeight""
            Value=""5"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""basics:TabItem"">
                    <Grid x:Name=""Root"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0"" />
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                        To=""MouseOver"" />
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver"" />
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal"" />
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal"" />
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""0""
                                                   Duration=""00:00:00.001""
                                                   Storyboard.TargetName=""FocusVisualTop""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""0""
                                                   Duration=""00:00:00.001""
                                                   Storyboard.TargetName=""FocusVisualBottom""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""0""
                                                   Duration=""00:00:00.001""
                                                   Storyboard.TargetName=""FocusVisualLeft""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""0""
                                                   Duration=""00:00:00.001""
                                                   Storyboard.TargetName=""FocusVisualRight""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""TopSelectedHoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""TopUnselectedHoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""rectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""BottomUnselectedHoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""rectangle1""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""rectangle2""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""rectangle3""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""rectangle4""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1"" />
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualTopSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualTopUnSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualBottomSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualBottomUnSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualLeftSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualLeftUnSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualRightSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualRightUnSelected""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65"" />
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualState x:Name=""Unselected"">
                                    <Storyboard />
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard />
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualTop""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualBottom""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualLeft""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualRight""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Collapsed"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid x:Name=""TemplateTopSelected""
                  Visibility=""Collapsed""
                  Canvas.ZIndex=""1""
                  Margin=""2,0,2,0"">
                            <Border Margin=""-2,-2,-2,0""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""2,2,2,0""
                      CornerRadius=""3,3,0,0"">
                                <Border BorderBrush=""{x:Null}""
                        CornerRadius=""2.4,2.4,0,0""
                        Background=""{StaticResource NormalBrush}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               x:Name=""TopSelectedHoverRectangle""
                               Opacity=""0"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderTopSelected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border Margin=""-2,-2,-2,0""
                      x:Name=""FocusVisualTop""
                      IsHitTestVisible=""false""
                      Visibility=""Collapsed""
                      BorderBrush=""{StaticResource HoverBrush}""
                      BorderThickness=""2,2,2,0""
                      CornerRadius=""3,3,0,0"" />
                            <Border Margin=""-2,-2,-2,0""
                      x:Name=""DisabledVisualTopSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""3,3,0,0"" />
                        </Grid>
                        <Grid x:Name=""TemplateTopUnselected""
                  Visibility=""Collapsed""
                  Margin=""1,0,1,0"">
                            <Border x:Name=""BorderTop""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""1""
                      CornerRadius=""3,3,0,0"">
                                <Border x:Name=""GradientTop""
                        BorderBrush=""#FFFFFFFF""
                        CornerRadius=""1,1,0,0""
                        Background=""{x:Null}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               x:Name=""TopUnselectedHoverRectangle""
                               Opacity=""0""
                               Height=""Auto""
                               HorizontalAlignment=""Stretch""
                               VerticalAlignment=""Stretch""
                               Width=""Auto"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderTopUnselected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border x:Name=""DisabledVisualTopUnSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""3,3,0,0"" />
                        </Grid>
                        <Grid x:Name=""TemplateBottomSelected""
                  Visibility=""Collapsed""
                  Canvas.ZIndex=""1""
                  Margin=""2,0,2,0"">
                            <Border Margin=""-2,0,-2,-2""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""2,0,2,2""
                      CornerRadius=""0,0,3,3"">
                                <Border BorderBrush=""{x:Null}""
                        CornerRadius=""0,0,2.4,2.4""
                        Background=""{StaticResource NormalBrush}"">
                                    <Grid>
                                        <Rectangle Margin=""0,0,0,0""
                               Fill=""{StaticResource HoverBrush}""
                               Opacity=""0""
                               x:Name=""rectangle"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderBottomSelected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border Margin=""-2,0,-2,-2""
                      x:Name=""FocusVisualBottom""
                      IsHitTestVisible=""false""
                      Visibility=""Collapsed""
                      BorderBrush=""{StaticResource HoverBrush}""
                      BorderThickness=""2,0,2,2""
                      CornerRadius=""0,0,3,3"" />
                            <Border Margin=""-2,0,-2,-2""
                      x:Name=""DisabledVisualBottomSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""0,0,3,3"" />
                        </Grid>
                        <Grid x:Name=""TemplateBottomUnselected""
                  Visibility=""Collapsed""
                  Margin=""1,0,1,0"">
                            <Border x:Name=""BorderBottom""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""1""
                      CornerRadius=""0,0,3,3"">
                                <Border x:Name=""GradientBottom""
                        BorderBrush=""{x:Null}""
                        CornerRadius=""0,0,1,1""
                        Background=""{x:Null}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               x:Name=""BottomUnselectedHoverRectangle""
                               Opacity=""0""
                               Height=""Auto""
                               HorizontalAlignment=""Stretch""
                               VerticalAlignment=""Stretch""
                               Width=""Auto"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderBottomUnselected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border x:Name=""DisabledVisualBottomUnSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""0,0,3,3"" />
                        </Grid>
                        <Grid x:Name=""TemplateLeftSelected""
                  Visibility=""Collapsed""
                  Canvas.ZIndex=""1""
                  Margin=""0,3,0,3"">
                            <Border Margin=""-2,-2,0,-2""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""2,2,0,2""
                      CornerRadius=""3,0,0,3"">
                                <Border BorderBrush=""{x:Null}""
                        BorderThickness=""0,0,0,0""
                        CornerRadius=""1,0,0,1""
                        Background=""{StaticResource NormalBrush}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               Opacity=""0""
                               x:Name=""rectangle1"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderLeftSelected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border Margin=""-2,-2,0,-2""
                      x:Name=""FocusVisualLeft""
                      IsHitTestVisible=""false""
                      Visibility=""Collapsed""
                      BorderBrush=""{StaticResource HoverBrush}""
                      BorderThickness=""2,2,0,2""
                      CornerRadius=""3,0,0,3"" />
                            <Border Margin=""-2,-2,0,-2""
                      x:Name=""DisabledVisualLeftSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""3,0,0,3"" />
                        </Grid>
                        <Grid x:Name=""TemplateLeftUnselected""
                  Visibility=""Collapsed""
                  Margin=""0,1,0,1"">
                            <Border x:Name=""BorderLeft""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""3,0,0,3"">
                                <Border x:Name=""GradientLeft""
                        BorderBrush=""{x:Null}""
                        CornerRadius=""1,0,0,1""
                        Background=""{x:Null}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               Opacity=""0""
                               Height=""Auto""
                               HorizontalAlignment=""Stretch""
                               VerticalAlignment=""Stretch""
                               Width=""Auto""
                               x:Name=""rectangle2"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderLeftUnselected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border x:Name=""DisabledVisualLeftUnSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""3,0,0,3"" />
                        </Grid>
                        <Grid x:Name=""TemplateRightSelected""
                  Visibility=""Collapsed""
                  Canvas.ZIndex=""1""
                  Margin=""0,3,0,3"">
                            <Border Margin=""0,-2,-2,-2""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""0,2,2,2""
                      CornerRadius=""0,3,3,0"">
                                <Border BorderBrush=""{x:Null}""
                        CornerRadius=""0,3,3,0""
                        Background=""{StaticResource NormalBrush}"">
                                    <Grid>
                                        <Rectangle Margin=""-2,0,0,0""
                               Fill=""{StaticResource HoverBrush}""
                               Opacity=""0""
                               x:Name=""rectangle3"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderRightSelected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border Margin=""0,-2,-2,-2""
                      x:Name=""FocusVisualRight""
                      IsHitTestVisible=""false""
                      Visibility=""Collapsed""
                      BorderBrush=""{StaticResource HoverBrush}""
                      BorderThickness=""0,1,1,1""
                      CornerRadius=""0,3,3,0"" />
                            <Border Margin=""0,-2,-2,-2""
                      x:Name=""DisabledVisualRightSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""0,3,3,0"" />
                        </Grid>
                        <Grid x:Name=""TemplateRightUnselected""
                  Visibility=""Collapsed""
                  Margin=""0,1,0,1"">
                            <Border x:Name=""BorderRight""
                      Background=""{TemplateBinding Background}""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""1""
                      CornerRadius=""0,3,3,0"">
                                <Border x:Name=""GradientRight""
                        BorderBrush=""{x:Null}""
                        CornerRadius=""0,1,1,0""
                        Background=""{x:Null}"">
                                    <Grid>
                                        <Rectangle Fill=""{StaticResource HoverBrush}""
                               Opacity=""0""
                               Height=""Auto""
                               VerticalAlignment=""Stretch""
                               x:Name=""rectangle4"" />
                                        <ContentControl FontSize=""{TemplateBinding FontSize}""
                                    Foreground=""{TemplateBinding Foreground}""
                                    IsTabStop=""False""
                                    Cursor=""{TemplateBinding Cursor}""
                                    HorizontalAlignment=""{TemplateBinding HorizontalAlignment}""
                                    Margin=""{TemplateBinding Padding}""
                                    x:Name=""HeaderRightUnselected""
                                    VerticalAlignment=""{TemplateBinding VerticalAlignment}"" />
                                    </Grid>
                                </Border>
                            </Border>
                            <Border x:Name=""DisabledVisualRightUnSelected""
                      IsHitTestVisible=""false""
                      Opacity=""0""
                      Background=""{StaticResource DisabledBackgroundBrush}""
                      CornerRadius=""0,3,3,0"" />
                        </Grid>
                        <Border Margin=""-1""
                    x:Name=""FocusVisualElement""
                    IsHitTestVisible=""false""
                    Visibility=""Collapsed""
                    BorderBrush=""#FF6DBDD1""
                    BorderThickness=""1""
                    CornerRadius=""3,3,0,0"" />
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF"" />
    </Style>
    <!--ComboBoxItem-->
    <Style  TargetType=""ComboBoxItem"">
        <Setter Property=""Padding""
            Value=""3""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Top""/>
        <Setter Property=""Background""
            Value=""Transparent""/>
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""TabNavigation""
            Value=""Local""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ComboBoxItem"">
                    <Grid Background=""{TemplateBinding Background}""
                Margin=""1,1,1,1"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Unselected""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Selected""/>
                                    <vsm:VisualTransition From=""Selected""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unselected""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unselected""/>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""Background""
                       IsHitTestVisible=""False""
                       Fill=""{StaticResource SelectedBackgroundBrush}""
                       RadiusX=""1""
                       RadiusY=""1""/>
                        <Rectangle x:Name=""HoverRectangle""
                       IsHitTestVisible=""False""
                       Opacity=""0""
                       Fill=""{StaticResource HoverBrush}""
                       RadiusX=""1""
                       RadiusY=""1""/>
                        <Rectangle x:Name=""SelectedRectangle""
                       IsHitTestVisible=""False""
                       Opacity=""0""
                       Fill=""{StaticResource NormalBrush}""
                       RadiusX=""1""
                       RadiusY=""1""/>
                        <ContentPresenter HorizontalAlignment=""Left""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                        <Rectangle x:Name=""FocusVisualElement""
                       Visibility=""Collapsed""
                       Stroke=""{StaticResource HoverBrush}""
                       StrokeThickness=""1""
                       RadiusX=""1""
                       RadiusY=""1""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
    </Style>
    <!--ComboBox-->
    <Style  TargetType=""ComboBox"">
        <Setter Property=""Padding""
            Value=""6,2,25,2""/>
        <Setter Property=""Background""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""TabNavigation""
            Value=""Once""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""ItemContainerStyle"" Value=""{StaticResource System.Windows.Controls.ComboBoxItem}""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ComboBox"">
                    <Grid>
                        <Grid.Resources>
                            <Style TargetType=""ToggleButton""
                     x:Name=""comboToggleStyle"">
                                <Setter Property=""Foreground""
                        Value=""#FF000000""/>
                                <Setter Property=""Background""
                        Value=""{StaticResource NormalBrush}""/>
                                <Setter Property=""BorderBrush""
                        Value=""{StaticResource NormalBorderBrush}"" />
                                <Setter Property=""BorderThickness""
                        Value=""2""/>
                                <Setter Property=""Padding""
                        Value=""1""/>
                                <Setter Property=""Template"">
                                    <Setter.Value>
                                        <ControlTemplate TargetType=""ToggleButton"">
                                            <Grid>
                                                <Grid.ColumnDefinitions>
                                                    <ColumnDefinition />
                                                    <ColumnDefinition Width=""20"" />
                                                </Grid.ColumnDefinitions>
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""Pressed""/>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                            <vsm:VisualTransition From=""Pressed""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""Pressed""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""HoverRectangle""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Pressed"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""Background""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""0""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                    <vsm:VisualStateGroup x:Name=""CheckStates"">
                                                        <vsm:VisualState x:Name=""Checked"">
                                                            <Storyboard/>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Unchecked""/>
                                                    </vsm:VisualStateGroup>
                                                    <vsm:VisualStateGroup x:Name=""FocusStates"">
                                                        <vsm:VisualState x:Name=""Focused"">
                                                            <Storyboard>
                                                                <ObjectAnimationUsingKeyFrames Duration=""0""
                                                               Storyboard.TargetName=""FocusVisualElement""
                                                               Storyboard.TargetProperty=""Visibility"">
                                                                    <DiscreteObjectKeyFrame KeyTime=""0""
                                                          Value=""Visible"" />
                                                                </ObjectAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Unfocused""/>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle x:Name=""Background""
                                   Fill=""{TemplateBinding Background}""
                                   Stroke=""{TemplateBinding BorderBrush}""
                                   StrokeThickness=""2""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   Grid.ColumnSpan=""2""/>
                                                <Rectangle x:Name=""HoverRectangle""
                                   Opacity=""0""
                                   Fill=""{StaticResource HoverBrush}""
                                   Stroke=""{x:Null}""
                                   StrokeThickness=""{TemplateBinding BorderThickness}""
                                   RadiusX=""2""
                                   RadiusY=""2""
                                   Grid.ColumnSpan=""2""
                                   Grid.Column=""1""
                                   Margin=""-4,2,2,2""/>
                                                <Rectangle x:Name=""BackgroundOverlay2""
                                   Fill=""#FFFFFFFF""
                                   Stroke=""#00000000""
                                   RadiusX=""2""
                                   RadiusY=""2""
                                   Margin=""1.45000004768372,1,0,1""/>
                                                <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                                          Margin=""{TemplateBinding Padding}""
                                          x:Name=""contentPresenter""
                                          VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                                          Content=""{TemplateBinding Content}""
                                          ContentTemplate=""{TemplateBinding ContentTemplate}""
                                          Grid.Column=""1""
                                          Grid.Row=""9""/>
                                                <Rectangle x:Name=""DisabledVisualElement""
                                   IsHitTestVisible=""false""
                                   Opacity=""0""
                                   Fill=""#A5FFFFFF""
                                   RadiusX=""3""
                                   RadiusY=""3""/>
                                                <Rectangle x:Name=""FocusVisualElement""
                                   IsHitTestVisible=""false""
                                   Visibility=""Collapsed""
                                   Stroke=""{StaticResource NormalBrush}""
                                   StrokeThickness=""1""
                                   RadiusX=""3.5""
                                   RadiusY=""3.5""
                                   Grid.ColumnSpan=""2""/>
                                            </Grid>
                                        </ControlTemplate>
                                    </Setter.Value>
                                </Setter>
                            </Style>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                                <vsm:VisualState x:Name=""FocusedDropDown"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""00:00:00""
                                                   Storyboard.TargetName=""PopupBorder""
                                                   Storyboard.TargetProperty=""(UIElement.Visibility)"">
                                            <DiscreteObjectKeyFrame KeyTime=""00:00:00""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""ContentPresenterBorder"">
                            <Grid>
                                <ToggleButton Background=""{TemplateBinding Background}""
                              BorderBrush=""{TemplateBinding BorderBrush}""
                              BorderThickness=""{TemplateBinding BorderThickness}""
                              HorizontalContentAlignment=""Right""
                              HorizontalAlignment=""Stretch""
                              x:Name=""DropDownToggle""
                              Style=""{StaticResource comboToggleStyle}""
                              VerticalAlignment=""Stretch"">
                                    <Path Height=""4""
                        HorizontalAlignment=""Right""
                        Margin=""0,0,6,0""
                        x:Name=""BtnArrow""
                        Width=""8""
                        Stretch=""Uniform""
                        Data=""F1 M 301.14,-189.041L 311.57,-189.041L 306.355,-182.942L 301.14,-189.041 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FF333333""
                                       x:Name=""BtnArrowColor""/>
                                        </Path.Fill>
                                    </Path>
                                </ToggleButton>
                                <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                                  Margin=""{TemplateBinding Padding}""
                                  x:Name=""ContentPresenter""
                                  VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"">
                                    <TextBlock Text="" ""/>
                                </ContentPresenter>
                            </Grid>
                        </Border>
                        <Rectangle x:Name=""DisabledVisualElement""
                       IsHitTestVisible=""false""
                       Fill=""{StaticResource DisabledBackgroundBrush}""
                       RadiusX=""4""
                       RadiusY=""4""
                       Opacity=""0""
                       StrokeThickness=""0""/>
                        <Rectangle x:Name=""FocusVisualElement""
                       IsHitTestVisible=""false""
                       Opacity=""0""
                       Stroke=""{StaticResource HoverBrush}""
                       StrokeThickness=""1""
                       RadiusX=""3""
                       RadiusY=""3""/>
                        <Popup x:Name=""Popup"">
                            <Border Height=""Auto""
                      HorizontalAlignment=""Stretch""
                      x:Name=""PopupBorder""
                      BorderBrush=""{TemplateBinding BorderBrush}""
                      BorderThickness=""{TemplateBinding BorderThickness}""
                      CornerRadius=""3""
                      Background=""{StaticResource ShadeBrush}"">
                                <ScrollViewer BorderThickness=""0""
                              Padding=""1""
                              x:Name=""ScrollViewer"">
                                    <ItemsPresenter/>
                                </ScrollViewer>
                            </Border>
                        </Popup>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property=""Foreground""
            Value=""#FF000000""/>
    </Style>

    <!--Textbox-->
    <Style TargetType=""TextBox"">
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrush}""/>
        <Setter Property=""Foreground""
            Value=""#FF000000""/>
        <Setter Property=""Padding""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""TextBox"">
                    <Grid >
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""ReadOnly""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Disabled""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverBorder""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.6""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""ReadOnly"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ReadOnlyVisualElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>

                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Border""
                    Opacity=""1""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""2,2,2,2"">
                            <Grid>
                                <Border x:Name=""ReadOnlyVisualElement""
                        Opacity=""0""
                        Background=""#72F7F7F7""/>
                                <Border BorderThickness=""1""
                        CornerRadius=""1,1,1,1"">
                                    <Border.BorderBrush>
                                        <SolidColorBrush Color=""Transparent""
                                     x:Name=""MouseOverColor""/>
                                    </Border.BorderBrush>
                                    <ScrollViewer BorderThickness=""0""
                                IsTabStop=""False""
                                Padding=""{TemplateBinding Padding}""
                                x:Name=""ContentElement""/>
                                </Border>
                            </Grid>
                        </Border>
                        <Border x:Name=""HoverBorder""
                    Opacity=""0""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2,2,2,2""
                    CornerRadius=""2,2,2,2""/>
                        <Border x:Name=""DisabledVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    Background=""#FFFFFFFF""
                    BorderBrush=""#A5F7F7F7""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""2,2,2,2""/>
                        <Border Margin=""1""
                    x:Name=""FocusVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2.1,2.1,2.1,2.1""
                    CornerRadius=""0.2,0.2,0.2,0.2""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--ProgressBar-->
    <Style TargetType=""ProgressBar"">
        <Setter Property=""Foreground""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""Background""
            Value=""{StaticResource SelectedBackgroundBrush}""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""Maximum""
            Value=""100""/>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource DisabledBackgroundBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ProgressBar"">
                    <Grid x:Name=""Root"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Determinate""/>
                                <vsm:VisualState x:Name=""Indeterminate"">
                                    <Storyboard RepeatBehavior=""Forever"">
                                        <ObjectAnimationUsingKeyFrames Duration=""00:00:00""
                                                   Storyboard.TargetName=""IndeterminateRoot""
                                                   Storyboard.TargetProperty=""(UIElement.Visibility)"">
                                            <DiscreteObjectKeyFrame KeyTime=""00:00:00""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""00:00:00""
                                                   Storyboard.TargetName=""DeterminateRoot""
                                                   Storyboard.TargetProperty=""(UIElement.Visibility)"">
                                            <DiscreteObjectKeyFrame KeyTime=""00:00:00""
                                              Value=""Collapsed"" />
                                        </ObjectAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""IndeterminateGradientFill""
                                                   Storyboard.TargetProperty=""(Shape.Fill).(LinearGradientBrush.Transform).(TransformGroup.Children)[0].X"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                            <SplineDoubleKeyFrame KeyTime=""00:00:.5""
                                            Value=""20""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""ProgressBarTrack""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""6,6,6,6""/>
                        <Grid x:Name=""ProgressBarRootGrid"">
                            <Grid x:Name=""IndeterminateRoot""
                    Visibility=""Collapsed"">
                                <Rectangle Margin=""1,1,1,1""
                           x:Name=""IndeterminateGradientFill""
                           Opacity=""0.7""
                           RadiusX=""5""
                           RadiusY=""5""
                           StrokeThickness=""0"">
                                    <Rectangle.Fill>
                                        <LinearGradientBrush EndPoint=""0,1""
                                         StartPoint=""20,1""
                                         MappingMode=""Absolute""
                                         SpreadMethod=""Repeat"">
                                            <LinearGradientBrush.Transform>
                                                <TransformGroup>
                                                    <TranslateTransform X=""0""/>
                                                    <SkewTransform AngleX=""-30""/>
                                                </TransformGroup>
                                            </LinearGradientBrush.Transform>
                                            <GradientStop Color=""#FFFFFFFF""
                                    Offset=""0""/>
                                            <GradientStop Color=""#00FFFFFF""
                                    Offset="".25""/>
                                            <GradientStop Color=""#FFFFFFFF""
                                    Offset=""0.85""/>
                                        </LinearGradientBrush>
                                    </Rectangle.Fill>
                                    <Rectangle.OpacityMask>
                                        <LinearGradientBrush EndPoint=""0.004,0.465""
                                         StartPoint=""0.997,0.422"">
                                            <GradientStop Color=""#00FFFFFF""/>
                                            <GradientStop Color=""#00FFFFFF""
                                    Offset=""1""/>
                                            <GradientStop Color=""#FFFFFFFF""
                                    Offset=""0.486""/>
                                        </LinearGradientBrush>
                                    </Rectangle.OpacityMask>
                                </Rectangle>
                            </Grid>
                            <Grid Margin=""1""
                    x:Name=""DeterminateRoot"">
                                <Rectangle HorizontalAlignment=""Left""
                           Margin=""1,1,1,1""
                           x:Name=""ProgressBarIndicator""
                           Fill=""{TemplateBinding Foreground}""
                           StrokeThickness=""0.5""
                           RadiusX=""5""
                           RadiusY=""5""/>
                            </Grid>
                        </Grid>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--Thumb-->
    <Style  TargetType=""Thumb"">
        <Setter Property=""Background""
            Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""Thumb"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Pressed""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Pressed""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Pressed""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""Background""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.7""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""Base""
                       RadiusX=""5""
                       RadiusY=""5""
                       Fill=""#FF000000""
                       StrokeThickness=""0""/>
                        <Rectangle x:Name=""Background""
                       Fill=""{TemplateBinding Background}""
                       Stroke=""{TemplateBinding BorderBrush}""
                       StrokeThickness=""2""
                       RadiusX=""4""
                       RadiusY=""4""/>
                        <Rectangle x:Name=""BackgroundAnimation""
                       Opacity=""0""
                       Fill=""{StaticResource HoverBrush}""
                       RadiusX=""3""
                       RadiusY=""3""
                       Margin=""2,2,2,2""
                       StrokeThickness=""0""/>
                        <Rectangle x:Name=""DisabledVisualElement""
                       IsHitTestVisible=""false""
                       Opacity=""0""
                       Fill=""{StaticResource DisabledBackgroundBrush}""
                       RadiusX=""5""
                       RadiusY=""5""/>
                        <Rectangle x:Name=""FocusVisualElement""
                       IsHitTestVisible=""false""
                       Visibility=""Collapsed""
                       Stroke=""{StaticResource HoverBrush}""
                       StrokeThickness=""1""
                       RadiusX=""4.5""
                       RadiusY=""4.5""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--Slider-->
    <Style TargetType=""Slider"">
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""Maximum""
            Value=""10""/>
        <Setter Property=""Minimum""
            Value=""0""/>
        <Setter Property=""Value""
            Value=""0""/>
        <Setter Property=""BorderBrush"">
            <Setter.Value>
                <LinearGradientBrush EndPoint=""0.5,1""
                             StartPoint=""0.5,0"">
                    <GradientStop Color=""#FFA3AEB9""
                        Offset=""0""/>
                    <GradientStop Color=""#FF8399A9""
                        Offset=""0.375""/>
                    <GradientStop Color=""#FF718597""
                        Offset=""0.375""/>
                    <GradientStop Color=""#FF617584""
                        Offset=""1""/>
                </LinearGradientBrush>
            </Setter.Value>
        </Setter>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""Slider"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <ControlTemplate x:Key=""RepeatButtonTemplate"">
                                <Grid x:Name=""Root""
                      Opacity=""0""
                      Background=""Transparent""/>
                            </ControlTemplate>
                            <Style x:Key=""SLSliderVerticalThumb""
                     TargetType=""Thumb"">
                                <Setter Property=""Background""
                        Value=""{StaticResource NormalBrush}""/>
                                <Setter Property=""BorderThickness""
                        Value=""1""/>
                                <Setter Property=""IsTabStop""
                        Value=""False""/>
                                <Setter Property=""BorderBrush""
                        Value=""{StaticResource NormalBorderBrush}"" />
                                <Setter Property=""Template"">
                                    <Setter.Value>
                                        <ControlTemplate TargetType=""Thumb"">
                                            <Grid>
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""Pressed""/>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                            <vsm:VisualTransition From=""Pressed""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""Pressed""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                               Storyboard.TargetProperty=""Opacity"">
                                                                    <SplineDoubleKeyFrame KeyTime=""0""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Pressed"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""Background""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""0""
                                                        Value=""0.65""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                    <vsm:VisualStateGroup x:Name=""FocusStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition From=""Focused""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Unfocused""/>
                                                            <vsm:VisualTransition From=""Unfocused""
                                                    GeneratedDuration=""00:00:00""
                                                    To=""Focused""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Focused"">
                                                            <Storyboard>
                                                                <ObjectAnimationUsingKeyFrames Duration=""0""
                                                               Storyboard.TargetName=""FocusVisualElement""
                                                               Storyboard.TargetProperty=""Visibility"">
                                                                    <DiscreteObjectKeyFrame KeyTime=""0""
                                                          Value=""Visible"" />
                                                                </ObjectAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Unfocused""/>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle x:Name=""Base""
                                   RadiusX=""5.1""
                                   RadiusY=""5.1""
                                   Fill=""#FF000000""/>
                                                <Rectangle x:Name=""Background""
                                   Fill=""{TemplateBinding Background}""
                                   Stroke=""{TemplateBinding BorderBrush}""
                                   StrokeThickness=""2""
                                   RadiusX=""4""
                                   RadiusY=""4""/>
                                                <Rectangle Margin=""2,2,2,2""
                                   x:Name=""BackgroundAnimation""
                                   Opacity=""0""
                                   Fill=""{StaticResource HoverBrush}""
                                   RadiusX=""3""
                                   RadiusY=""3""/>
                                                <Rectangle x:Name=""DisabledVisualElement""
                                   IsHitTestVisible=""false""
                                   Opacity=""0""
                                   Fill=""{StaticResource DisabledBackgroundBrush}""
                                   RadiusX=""5""
                                   RadiusY=""5""/>
                                                <Rectangle x:Name=""FocusVisualElement""
                                   IsHitTestVisible=""false""
                                   Visibility=""Collapsed""
                                   Stroke=""{StaticResource HoverBrush}""
                                   StrokeThickness=""1""
                                   RadiusX=""4.5""
                                   RadiusY=""4.5""/>
                                            </Grid>
                                        </ControlTemplate>
                                    </Setter.Value>
                                </Setter>
                            </Style>
                            <Style x:Key=""SLSliderHorizontalThumb""
                     TargetType=""Thumb"">
                                <Setter Property=""Background""
                        Value=""{StaticResource NormalBrush}""/>
                                <Setter Property=""BorderThickness""
                        Value=""2""/>
                                <Setter Property=""IsTabStop""
                        Value=""False""/>
                                <Setter Property=""BorderBrush""
                        Value=""{StaticResource NormalBorderBrush}"" />
                                <Setter Property=""Template"">
                                    <Setter.Value>
                                        <ControlTemplate TargetType=""Thumb"">
                                            <Grid>
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                                    To=""Pressed""/>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                            <vsm:VisualTransition From=""Pressed""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""Pressed""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                               Storyboard.TargetProperty=""Opacity"">
                                                                    <SplineDoubleKeyFrame KeyTime=""0""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Pressed"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""Background""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""0""
                                                        Value=""0.65""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                    <vsm:VisualStateGroup x:Name=""FocusStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition From=""Focused""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Unfocused""/>
                                                            <vsm:VisualTransition From=""Unfocused""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""Focused""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Focused"">
                                                            <Storyboard>
                                                                <ObjectAnimationUsingKeyFrames Duration=""0""
                                                               Storyboard.TargetName=""FocusVisualElement""
                                                               Storyboard.TargetProperty=""Visibility"">
                                                                    <DiscreteObjectKeyFrame KeyTime=""0""
                                                          Value=""Visible"" />
                                                                </ObjectAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Unfocused""/>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle x:Name=""Base""
                                   RadiusX=""5""
                                   RadiusY=""5""
                                   Fill=""#FF000000""
                                   StrokeThickness=""0""/>
                                                <Rectangle x:Name=""Background""
                                   Fill=""{TemplateBinding Background}""
                                   Stroke=""{TemplateBinding BorderBrush}""
                                   StrokeThickness=""2""
                                   RadiusX=""4""
                                   RadiusY=""4""/>
                                                <Rectangle x:Name=""BackgroundAnimation""
                                   Opacity=""0""
                                   Fill=""{StaticResource HoverBrush}""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   Margin=""2,2,2,2""
                                   StrokeThickness=""0""/>
                                                <Rectangle x:Name=""DisabledVisualElement""
                                   IsHitTestVisible=""false""
                                   Opacity=""0""
                                   Fill=""{StaticResource DisabledBackgroundBrush}""
                                   RadiusX=""5""
                                   RadiusY=""5""/>
                                                <Rectangle x:Name=""FocusVisualElement""
                                   IsHitTestVisible=""false""
                                   Visibility=""Collapsed""
                                   Stroke=""{StaticResource HoverBrush}""
                                   StrokeThickness=""1""
                                   RadiusX=""4.5""
                                   RadiusY=""4.5""/>
                                            </Grid>
                                        </ControlTemplate>
                                    </Setter.Value>
                                </Setter>
                            </Style>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Root""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.5""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid x:Name=""HorizontalTemplate""
                  Background=""{TemplateBinding Background}"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""Auto""/>
                                <ColumnDefinition Width=""Auto""/>
                                <ColumnDefinition Width=""*""/>
                            </Grid.ColumnDefinitions>
                            <Rectangle Height=""5""
                         Margin=""5,0,5,0""
                         Grid.Column=""0""
                         Grid.ColumnSpan=""3""
                         Fill=""{StaticResource SelectedBackgroundBrush}""
                         Stroke=""{StaticResource HorizontalSliderBorderBrush}""
                         StrokeThickness=""{TemplateBinding BorderThickness}""/>
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""HorizontalTrackLargeChangeDecreaseRepeatButton""
                            Grid.Column=""0""/>
                            <Thumb IsTabStop=""True""
                     Height=""13""
                     x:Name=""HorizontalThumb""
                     Width=""24""
                     Grid.Column=""1""
                     Style=""{StaticResource SLSliderHorizontalThumb}""
                     Margin=""0,0,0,0""/>
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""HorizontalTrackLargeChangeIncreaseRepeatButton""
                            Grid.Column=""2""/>
                        </Grid>
                        <Grid x:Name=""VerticalTemplate""
                  Visibility=""Collapsed""
                  Background=""{TemplateBinding Background}"">
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""*""/>
                                <RowDefinition Height=""Auto""/>
                                <RowDefinition Height=""Auto""/>
                            </Grid.RowDefinitions>
                            <Rectangle Margin=""0,5,0,5""
                         Width=""5""
                         Grid.Row=""0""
                         Grid.RowSpan=""3""
                         StrokeThickness=""{TemplateBinding BorderThickness}""
                         Fill=""{StaticResource SelectedBackgroundBrushVertical}""
                         Stroke=""{StaticResource VerticalSliderBorderBrush}""/>
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""VerticalTrackLargeChangeDecreaseRepeatButton""
                            Grid.Row=""2""/>
                            <Thumb IsTabStop=""True""
                     Height=""24""
                     x:Name=""VerticalThumb""
                     Width=""13""
                     Grid.Row=""1""
                     Style=""{StaticResource SLSliderVerticalThumb}""/>
                            <RepeatButton IsTabStop=""False""
                            Template=""{StaticResource RepeatButtonTemplate}""
                            x:Name=""VerticalTrackLargeChangeIncreaseRepeatButton""
                            Grid.Row=""0""/>
                        </Grid>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- Calendar DayButton-->

    <Style TargetType=""System_Windows_Controls_Primitives:CalendarDayButton"">
        <Setter Property=""Background"" Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""MinWidth"" Value=""5""/>
        <Setter Property=""MinHeight"" Value=""5""/>
        <Setter Property=""FontSize"" Value=""10""/>
        <Setter Property=""HorizontalContentAlignment"" Value=""Center""/>
        <Setter Property=""VerticalContentAlignment"" Value=""Center""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives:CalendarDayButton"">
                    <Grid x:Name=""Root"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.1""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""0.45""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""0.3""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""NormalText"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value="".35""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unselected""/>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SelectedBackground"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value="".75""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames Storyboard.TargetName=""selectedText"" Storyboard.TargetProperty=""Color"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FFFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""CalendarButtonFocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""CalendarButtonFocused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""DayButtonFocusVisual"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""CalendarButtonUnfocused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""DayButtonFocusVisual"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Collapsed""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""ActiveStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Active""/>
                                <vsm:VisualState x:Name=""Inactive"">
                                    <Storyboard>
                                        <ColorAnimationUsingKeyFrames Storyboard.TargetName=""selectedText"" Storyboard.TargetProperty=""Color"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FF777777""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""DayStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""RegularDay""/>
                                <vsm:VisualState x:Name=""Today"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""TodayBackground"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames Storyboard.TargetName=""selectedText"" Storyboard.TargetProperty=""Color"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FFFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""BlackoutDayStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""NormalDay""/>
                                <vsm:VisualState x:Name=""BlackoutDay"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Blackout"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value="".2""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""TodayBackground"" Opacity=""0"" Fill=""{StaticResource DisabledBackgroundBrush}"" RadiusX=""3"" RadiusY=""3"" Stroke=""{StaticResource NormalBorderBrush}""/>
                        <Rectangle x:Name=""SelectedBackground"" Opacity=""0"" Fill=""{TemplateBinding Background}"" RadiusX=""3"" RadiusY=""3"" Stroke=""{StaticResource NormalBorderBrush}""/>
                        <Rectangle x:Name=""Background"" Opacity=""0"" Fill=""{TemplateBinding Background}"" RadiusX=""3"" RadiusY=""3"" Stroke=""{StaticResource NormalBorderBrush}""/>
                        <ContentControl FontSize=""{TemplateBinding FontSize}"" IsTabStop=""False"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""5,1,5,1"" x:Name=""NormalText"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}"">
                            <ContentControl.Foreground>
                                <SolidColorBrush Color=""#FF333333"" x:Name=""selectedText""/>
                            </ContentControl.Foreground>
                        </ContentControl>
                        <Path HorizontalAlignment=""Stretch"" Margin=""3"" x:Name=""Blackout"" VerticalAlignment=""Stretch"" Opacity=""0"" RenderTransformOrigin=""0.5,0.5"" Fill=""#FF000000"" Stretch=""Fill"" Data=""M8.1772461,11.029181 L10.433105,11.029181 L11.700684,12.801641 L12.973633,11.029181 L15.191895,11.029181 L12.844727,13.999395 L15.21875,17.060919 L12.962891,17.060919 L11.673828,15.256231 L10.352539,17.060919 L8.1396484,17.060919 L10.519043,14.042364 z""/>
                        <Rectangle x:Name=""DayButtonFocusVisual"" IsHitTestVisible=""false"" Visibility=""Collapsed"" Stroke=""{StaticResource HoverBrush}"" RadiusX=""3"" RadiusY=""3""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>


    <!--CalendarItem-->
    <Style TargetType=""System_Windows_Controls_Primitives:CalendarItem"">
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives:CalendarItem"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <SolidColorBrush x:Key=""DisabledColor""
                               Color=""#8CFFFFFF""/>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisual""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.5""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""DisabledVisual""
                                                   Storyboard.TargetProperty=""(UIElement.Visibility)"">
                                            <DiscreteObjectKeyFrame KeyTime=""00:00:00"">
                                                <DiscreteObjectKeyFrame.Value>
                                                    <Visibility>Visible</Visibility>
                                                </DiscreteObjectKeyFrame.Value>
                                            </DiscreteObjectKeyFrame>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border Margin=""0,2,0,2""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""3,3,3,3"">
                            <Border BorderBrush=""{x:Null}""
                      BorderThickness=""2""
                      CornerRadius=""1"">
                                <Grid>
                                    <Grid.Resources>
                                        <ControlTemplate x:Key=""PreviousButtonTemplate""
                                     TargetType=""Button"">
                                            <Grid Cursor=""Hand"">
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""HoverRectangle""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""DisabledVisual""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle Opacity=""1""
                                   Fill=""{StaticResource NormalBrush}""
                                   Stretch=""Fill""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   Stroke=""{StaticResource NormalBorderBrush}""
                                   StrokeThickness=""2""/>
                                                <Rectangle Opacity=""0""
                                   Fill=""{StaticResource HoverBrush}""
                                   Stretch=""Fill""
                                   x:Name=""HoverRectangle""
                                   RadiusX=""2""
                                   RadiusY=""2""
                                   Margin=""2,2,2,2""/>
                                                <Grid>
                                                    <Path Height=""10""
                                HorizontalAlignment=""Center""
                                VerticalAlignment=""Center""
                                Width=""6""
                                Stretch=""Fill""
                                Data=""M288.75,232.25 L288.75,240.625 L283,236.625 z"">
                                                        <Path.Fill>
                                                            <SolidColorBrush Color=""#FFFFFFFF""
                                               x:Name=""TextColor""/>
                                                        </Path.Fill>
                                                    </Path>
                                                </Grid>
                                                <Rectangle Opacity=""0""
                                   Fill=""{StaticResource DisabledBackgroundBrush}""
                                   Stretch=""Fill""
                                   Stroke=""{StaticResource NormalBorderBrush}""
                                   StrokeThickness=""2""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   x:Name=""DisabledVisual""/>
                                            </Grid>
                                        </ControlTemplate>
                                        <ControlTemplate x:Key=""NextButtonTemplate""
                                     TargetType=""Button"">
                                            <Grid Cursor=""Hand"">
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""HoverRectangle""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""DisabledVisual""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle Opacity=""1""
                                   Fill=""{StaticResource NormalBrush}""
                                   Stretch=""Fill""
                                   StrokeThickness=""2""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   Stroke=""{StaticResource NormalBorderBrush}""/>
                                                <Rectangle Opacity=""0""
                                   Fill=""{StaticResource HoverBrush}""
                                   Stretch=""Fill""
                                   x:Name=""HoverRectangle""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   Margin=""2,2,2,2""
                                   StrokeThickness=""0""/>
                                                <Grid>
                                                    <Path Height=""10""
                                HorizontalAlignment=""Center""
                                VerticalAlignment=""Center""
                                Width=""6""
                                Stretch=""Fill""
                                Data=""M282.875,231.875 L282.875,240.375 L288.625,236 z"">
                                                        <Path.Fill>
                                                            <SolidColorBrush Color=""#FFFFFFFF""
                                               x:Name=""TextColor""/>
                                                        </Path.Fill>
                                                    </Path>
                                                </Grid>
                                                <Rectangle Opacity=""0""
                                   Fill=""{StaticResource DisabledBackgroundBrush}""
                                   Stretch=""Fill""
                                   Stroke=""{StaticResource NormalBorderBrush}""
                                   StrokeThickness=""2""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   x:Name=""DisabledVisual""/>
                                            </Grid>
                                        </ControlTemplate>
                                        <ControlTemplate x:Key=""HeaderButtonTemplate""
                                     TargetType=""Button"">
                                            <Grid Cursor=""Hand"">
                                                <vsm:VisualStateManager.VisualStateGroups>
                                                    <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                        <vsm:VisualStateGroup.Transitions>
                                                            <vsm:VisualTransition From=""Normal""
                                                    GeneratedDuration=""00:00:00.3000000""
                                                    To=""MouseOver""/>
                                                            <vsm:VisualTransition From=""MouseOver""
                                                    GeneratedDuration=""00:00:00.5000000""
                                                    To=""Normal""/>
                                                        </vsm:VisualStateGroup.Transitions>
                                                        <vsm:VisualState x:Name=""Normal""/>
                                                        <vsm:VisualState x:Name=""MouseOver"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""HoverRectangle""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""1""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                        <vsm:VisualState x:Name=""Disabled"">
                                                            <Storyboard>
                                                                <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                               Duration=""00:00:00.0010000""
                                                               Storyboard.TargetName=""DisabledVisual""
                                                               Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                                    <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                        Value=""0.7""/>
                                                                </DoubleAnimationUsingKeyFrames>
                                                            </Storyboard>
                                                        </vsm:VisualState>
                                                    </vsm:VisualStateGroup>
                                                </vsm:VisualStateManager.VisualStateGroups>
                                                <Rectangle Fill=""{StaticResource NormalBrush}""
                                   Stroke=""{StaticResource NormalBorderBrush}""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   StrokeThickness=""2""/>
                                                <Rectangle Fill=""{StaticResource HoverBrush}""
                                   Stroke=""{x:Null}""
                                   RadiusX=""2""
                                   RadiusY=""2""
                                   Margin=""2,2,2,2""
                                   Opacity=""0""
                                   x:Name=""HoverRectangle""/>
                                                <ContentControl IsTabStop=""False""
                                        HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                                        x:Name=""buttonContent""
                                        VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                                        Content=""{TemplateBinding Content}""
                                        ContentTemplate=""{TemplateBinding ContentTemplate}""
                                        Margin=""10,3,10,3"">
                                                    <ContentControl.Foreground>
                                                        <SolidColorBrush Color=""#FFFFFFFF""
                                             x:Name=""TextColor""/>
                                                    </ContentControl.Foreground>
                                                </ContentControl>
                                                <Rectangle Fill=""{StaticResource DisabledBackgroundBrush}""
                                   Stroke=""{x:Null}""
                                   RadiusX=""3""
                                   RadiusY=""3""
                                   x:Name=""DisabledVisual""
                                   Opacity=""0""/>
                                            </Grid>
                                        </ControlTemplate>
                                        <DataTemplate x:Name=""DayTitleTemplate"">
                                            <TextBlock HorizontalAlignment=""Center""
                                 Margin=""0,4,0,4""
                                 VerticalAlignment=""Center""
                                 FontSize=""9.5""
                                 FontWeight=""Bold""
                                 Text=""{Binding}""/>
                                        </DataTemplate>
                                    </Grid.Resources>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width=""Auto""/>
                                        <ColumnDefinition Width=""Auto""/>
                                        <ColumnDefinition Width=""Auto""/>
                                    </Grid.ColumnDefinitions>
                                    <Grid.RowDefinitions>
                                        <RowDefinition Height=""Auto""/>
                                        <RowDefinition Height=""*""/>
                                    </Grid.RowDefinitions>
                                    <Button Template=""{StaticResource PreviousButtonTemplate}""
                          Height=""20""
                          HorizontalAlignment=""Left""
                          x:Name=""PreviousButton""
                          Width=""25""
                          Visibility=""Collapsed""/>
                                    <Button FontSize=""10.5""
                          FontWeight=""Bold""
                          Template=""{StaticResource HeaderButtonTemplate}""
                          HorizontalAlignment=""Center""
                          x:Name=""HeaderButton""
                          VerticalAlignment=""Center""
                          Grid.Column=""1""/>
                                    <Button Template=""{StaticResource NextButtonTemplate}""
                          Height=""20""
                          HorizontalAlignment=""Right""
                          x:Name=""NextButton""
                          Width=""25""
                          Visibility=""Collapsed""
                          Grid.Column=""2""/>
                                    <Grid Margin=""6,3,6,6""
                        x:Name=""MonthView""
                        Visibility=""Collapsed""
                        Grid.ColumnSpan=""3""
                        Grid.Row=""1"">
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                        </Grid.ColumnDefinitions>
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                        </Grid.RowDefinitions>
                                    </Grid>
                                    <Grid Margin=""6,3,7,6""
                        x:Name=""YearView""
                        Visibility=""Collapsed""
                        Grid.ColumnSpan=""3""
                        Grid.Row=""1"">
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                            <ColumnDefinition Width=""Auto""/>
                                        </Grid.ColumnDefinitions>
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                            <RowDefinition Height=""Auto""/>
                                        </Grid.RowDefinitions>
                                    </Grid>
                                </Grid>
                            </Border>
                        </Border>
                        <Rectangle Margin=""0,2,0,2""
                       x:Name=""DisabledVisual""
                       Opacity=""0.7""
                       Visibility=""Collapsed""
                       Fill=""#FFFFFFFF""
                       Stretch=""Fill""
                       Stroke=""{x:Null}""
                       StrokeThickness=""0""
                       RadiusX=""2""
                       RadiusY=""2""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- Calendar Button-->

    <Style TargetType=""System_Windows_Controls_Primitives:CalendarButton"">
        <Setter Property=""Background"" Value=""{StaticResource NormalBrush}""/>
        <Setter Property=""MinWidth"" Value=""40""/>
        <Setter Property=""MinHeight"" Value=""42""/>
        <Setter Property=""FontSize"" Value=""10""/>
        <Setter Property=""HorizontalContentAlignment"" Value=""Center""/>
        <Setter Property=""VerticalContentAlignment"" Value=""Center""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives:CalendarButton"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.1""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value="".5""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""0.4""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unselected""/>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SelectedBackground"" Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0"" Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames Storyboard.TargetName=""selectedText"" Storyboard.TargetProperty=""Color"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FFFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""ActiveStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Active""/>
                                <vsm:VisualState x:Name=""Inactive"">
                                    <Storyboard>
                                        <ColorAnimationUsingKeyFrames Storyboard.TargetName=""selectedText"" Storyboard.TargetProperty=""Color"">
                                            <SplineColorKeyFrame KeyTime=""0"" Value=""#FF777777""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""CalendarButtonFocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""CalendarButtonFocused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""CalendarButtonFocusVisual"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""CalendarButtonUnfocused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""CalendarButtonFocusVisual"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Collapsed""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""SelectedBackground"" Opacity=""0"" Fill=""{TemplateBinding Background}"" RadiusX=""3"" RadiusY=""3"" Stroke=""{StaticResource NormalBorderBrush}""/>
                        <Rectangle x:Name=""Background"" Opacity=""0"" Fill=""{TemplateBinding Background}"" RadiusX=""3"" RadiusY=""3"" Stroke=""{StaticResource NormalBorderBrush}""/>
                        <ContentControl FontSize=""{TemplateBinding FontSize}"" IsTabStop=""False"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""1,0,1,1"" x:Name=""NormalText"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}"">
                            <ContentControl.Foreground>
                                <SolidColorBrush Color=""#FF333333"" x:Name=""selectedText""/>
                            </ContentControl.Foreground>
                        </ContentControl>
                        <Rectangle x:Name=""CalendarButtonFocusVisual"" IsHitTestVisible=""false"" Visibility=""Collapsed"" Stroke=""{StaticResource HoverBrush}"" RadiusX=""3"" RadiusY=""3""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!--Calendar-->
    <Style  TargetType=""basics:Calendar"">
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrush}"" />
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""basics:Calendar"">
                    <StackPanel HorizontalAlignment=""Center""
                      x:Name=""Root"">
                        <System_Windows_Controls_Primitives:CalendarItem Background=""{TemplateBinding Background}""
                                                             BorderBrush=""{TemplateBinding BorderBrush}""
                                                             BorderThickness=""{TemplateBinding BorderThickness}""
                                                             x:Name=""CalendarItem""
                                                             Style=""{StaticResource System.Windows.Controls.Primitives.CalendarItem}""/>
                    </StackPanel>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--DatePickerTextBox-->
    <Style  TargetType=""System_Windows_Controls_Primitives:DatePickerTextBox"">
        <Setter Property=""VerticalContentAlignment""
            Value=""Center""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Left""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives:DatePickerTextBox"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <SolidColorBrush x:Key=""WatermarkBrush""
                               Color=""#FFAAAAAA""/>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverBorder""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""WatermarkStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unwatermarked""/>
                                <vsm:VisualState x:Name=""Watermarked"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""ContentElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""Watermark""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                    <vsm:VisualTransition From=""Unfocused""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Focused""/>
                                    <vsm:VisualTransition From=""Focused""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Unfocused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unfocused""/>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Border""
                    Opacity=""1""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""1"">
                            <Grid x:Name=""WatermarkContent""
                    Background=""{TemplateBinding Background}"">
                                <Border x:Name=""ContentElement""
                        Background=""{TemplateBinding Background}""
                        BorderBrush=""#FFFFFFFF""
                        BorderThickness=""1""
                        Padding=""{TemplateBinding Padding}""/>
                                <Border x:Name=""ContentElement2""
                        BorderBrush=""#FFFFFFFF""
                        BorderThickness=""1"">
                                    <ContentControl Background=""{TemplateBinding Background}""
                                  FontSize=""{TemplateBinding FontSize}""
                                  Foreground=""{StaticResource WatermarkBrush}""
                                  HorizontalContentAlignment=""{TemplateBinding HorizontalContentAlignment}""
                                  IsTabStop=""False""
                                  Padding=""2""
                                  VerticalContentAlignment=""{TemplateBinding VerticalContentAlignment}""
                                  HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                                  x:Name=""Watermark""
                                  VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                                  IsHitTestVisible=""False""
                                  Opacity=""0""
                                  Content=""{TemplateBinding Watermark}""/>
                                </Border>
                                <Border x:Name=""FocusVisual""
                        IsHitTestVisible=""False""
                        Opacity=""0""
                        BorderBrush=""#FF6DBDD1""
                        BorderThickness=""{TemplateBinding BorderThickness}""
                        CornerRadius=""1""/>
                            </Grid>
                        </Border>
                        <Border x:Name=""HoverBorder""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2,2,2,2""
                    CornerRadius=""0.8,0.8,0.8,0.8""
                    Opacity=""0""/>
                        <Border x:Name=""DisabledVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    Background=""#FFFFFFFF""
                    BorderBrush=""#A5F7F7F7""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""0.8,0.8,0.8,0.8""/>
                        <Border x:Name=""FocusVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2.4,2.4,2.4,2.4""
                    CornerRadius=""0.8,0.8,0.8,0.8""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--DatePicker-->
    <Style  TargetType=""basics:DatePicker"">
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""Background""
            Value=""#FFFFFFFF""/>
        <Setter Property=""Padding""
            Value=""2""/>
        <Setter Property=""CalendarStyle""
            Value=""{StaticResource System.Windows.Controls.Calendar}""/>

        <Setter Property=""SelectionBackground""
            Value=""#FF444444""/>
        <Setter Property=""BorderBrush"">
            <Setter.Value>
                <LinearGradientBrush EndPoint="".5,0""
                             StartPoint="".5,1"">
                    <GradientStop Color=""#FF617584""
                        Offset=""0""/>
                    <GradientStop Color=""#FF718597""
                        Offset=""0.375""/>
                    <GradientStop Color=""#FF8399A9""
                        Offset=""0.375""/>
                    <GradientStop Color=""#FFA3AEB9""
                        Offset=""1""/>
                </LinearGradientBrush>
            </Setter.Value>
        </Setter>
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""basics:DatePicker"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <SolidColorBrush x:Key=""DisabledBrush""
                               Color=""#8CFFFFFF""/>
                            <ControlTemplate x:Key=""DropDownButtonTemplate""
                               TargetType=""Button"">
                                <Grid>
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                              To=""Pressed""/>
                                                <vsm:VisualTransition From=""Normal""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""Normal""/>
                                                <vsm:VisualTransition From=""Pressed""
                                              GeneratedDuration=""00:00:00.5000000""
                                              To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver""
                                              GeneratedDuration=""00:00:00.3000000""
                                              To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""HoverBorder""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""Background""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Disabled"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                         Duration=""00:00:00.0010000""
                                                         Storyboard.TargetName=""DisabledVisual""
                                                         Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                                  Value=""0.65""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Grid Height=""18""
                        HorizontalAlignment=""Center""
                        Margin=""0""
                        VerticalAlignment=""Center""
                        Width=""19""
                        Background=""#11FFFFFF"">
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width=""20*""/>
                                            <ColumnDefinition Width=""20*""/>
                                            <ColumnDefinition Width=""20*""/>
                                            <ColumnDefinition Width=""20*""/>
                                        </Grid.ColumnDefinitions>
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height=""23*""/>
                                            <RowDefinition Height=""19*""/>
                                            <RowDefinition Height=""19*""/>
                                            <RowDefinition Height=""19*""/>
                                        </Grid.RowDefinitions>
                                        <Border Margin=""-1""
                            x:Name=""Highlight""
                            Opacity=""0""
                            Grid.ColumnSpan=""4""
                            Grid.Row=""0""
                            Grid.RowSpan=""4""
                            BorderBrush=""{StaticResource HoverBrush}""
                            BorderThickness=""1""
                            CornerRadius=""0,0,1,1""/>
                                        <Border Margin=""0,0,0,0""
                            x:Name=""Background""
                            Opacity=""1""
                            Grid.ColumnSpan=""4""
                            Grid.Row=""1""
                            Grid.RowSpan=""3""
                            Background=""{StaticResource DisabledBackgroundBrush}""
                            BorderBrush=""{StaticResource NormalBorderBrush}""
                            BorderThickness=""1""
                            CornerRadius="".5""/>
                                        <Border x:Name=""HoverBorder""
                            BorderBrush=""{x:Null}""
                            BorderThickness=""1""
                            CornerRadius=""0,0,1,1""
                            Grid.ColumnSpan=""4""
                            Grid.RowSpan=""4""
                            Background=""{StaticResource HoverBrush}""
                            Opacity=""0""/>
                                        <Rectangle Grid.ColumnSpan=""4""
                               Grid.RowSpan=""1""
                               StrokeThickness=""1""
                               Fill=""{StaticResource NormalBrush}""
                               Stroke=""{StaticResource NormalBorderBrush}""/>
                                        <Path HorizontalAlignment=""Center""
                          Margin=""4,3,4,3""
                          VerticalAlignment=""Center""
                          RenderTransformOrigin=""0.5,0.5""
                          Grid.Column=""0""
                          Grid.ColumnSpan=""4""
                          Grid.Row=""1""
                          Grid.RowSpan=""3""
                          Fill=""#FF000000""
                          Stretch=""Fill""
                          Data=""M11.426758,8.4305077 L11.749023,8.4305077 L11.749023,16.331387 L10.674805,16.331387 L10.674805,10.299648 L9.0742188,11.298672 L9.0742188,10.294277 C9.4788408,10.090176 9.9094238,9.8090878 10.365967,9.4510155 C10.82251,9.0929432 11.176106,8.7527733 11.426758,8.4305077 z M14.65086,8.4305077 L18.566387,8.4305077 L18.566387,9.3435936 L15.671368,9.3435936 L15.671368,11.255703 C15.936341,11.058764 16.27293,10.960293 16.681133,10.960293 C17.411602,10.960293 17.969301,11.178717 18.354229,11.615566 C18.739157,12.052416 18.931622,12.673672 18.931622,13.479336 C18.931622,15.452317 18.052553,16.438808 16.294415,16.438808 C15.560365,16.438808 14.951641,16.234707 14.468243,15.826504 L14.881817,14.929531 C15.368796,15.326992 15.837872,15.525723 16.289043,15.525723 C17.298809,15.525723 17.803692,14.895514 17.803692,13.635098 C17.803692,12.460618 17.305971,11.873379 16.310528,11.873379 C15.83071,11.873379 15.399232,12.079271 15.016094,12.491055 L14.65086,12.238613 z""/>
                                        <Ellipse Height=""3""
                             HorizontalAlignment=""Center""
                             VerticalAlignment=""Center""
                             Width=""3""
                             Grid.ColumnSpan=""4""
                             Fill=""#FFFFFFFF""
                             StrokeThickness=""0""/>
                                        <Border x:Name=""DisabledVisual""
                            Opacity=""0""
                            Grid.ColumnSpan=""4""
                            Grid.Row=""0""
                            Grid.RowSpan=""4""
                            BorderBrush=""#B2FFFFFF""
                            BorderThickness=""1""
                            CornerRadius=""0,0,.5,.5""
                            Background=""{StaticResource DisabledBackgroundBrush}""/>
                                    </Grid>
                                </Grid>
                            </ControlTemplate>
                        </Grid.Resources>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""*""/>
                            <ColumnDefinition Width=""Auto""/>
                        </Grid.ColumnDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard/>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <System_Windows_Controls_Primitives:DatePickerTextBox Background=""{TemplateBinding Background}""
                                                                  BorderBrush=""{TemplateBinding BorderBrush}""
                                                                  BorderThickness=""{TemplateBinding BorderThickness}""
                                                                  Padding=""{TemplateBinding Padding}""
                                                                  x:Name=""TextBox""
                                                                  Grid.Column=""0""
                                                                  SelectionBackground=""{TemplateBinding SelectionBackground}""
                                                                  Style=""{StaticResource System.Windows.Controls.Primitives.DatePickerTextBox}""/>
                        <Button BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    Foreground=""{TemplateBinding Foreground}""
                    Template=""{StaticResource DropDownButtonTemplate}""
                    Margin=""2,0,2,0""
                    x:Name=""Button""
                    Width=""20""
                    Grid.Column=""1""/>
                        <Grid x:Name=""DisabledVisual""
                  IsHitTestVisible=""False""
                  Opacity=""0""
                  Grid.ColumnSpan=""2"">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""*""/>
                                <ColumnDefinition Width=""Auto""/>
                            </Grid.ColumnDefinitions>
                            <Rectangle Fill=""{StaticResource DisabledBackgroundBrush}""
                         RadiusX=""1""
                         RadiusY=""1""/>
                            <Rectangle Height=""18""
                         Margin=""2,0,2,0""
                         Width=""19""
                         Grid.Column=""1""
                         Fill=""{StaticResource DisabledBackgroundBrush}""
                         RadiusX=""1""
                         RadiusY=""1""/>
                        </Grid>
                        <Popup x:Name=""Popup""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!--ToggleButton-->
    <Style  TargetType=""ToggleButton"">
        <Setter Property=""Background""
            Value=""{StaticResource SelectedBackgroundBrush}""/>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
        <Setter Property=""Padding""
            Value=""3""/>
        <Setter Property=""BorderThickness""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""ToggleButton"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Pressed""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.3""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Pressed"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""BackgroundAnimation""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.5""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.65""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""CheckStates"">
                                <vsm:VisualState x:Name=""Checked"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""CheckedBorder""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unchecked""/>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0""
                                              Value=""Visible"" />
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Background""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""4,4,4,4""
                    Opacity=""0.9""/>
                        <Border x:Name=""CheckedBorder""
                    Opacity=""0""
                    Background=""{StaticResource NormalBrush}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""4""/>
                        <Border x:Name=""BackgroundAnimation""
                    Opacity=""0""
                    Background=""#FFFFFFFF""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""4""/>
                        <ContentPresenter HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              x:Name=""contentPresenter""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                        <Border x:Name=""DisabledVisualElement""
                    IsHitTestVisible=""false""
                    Opacity=""0""
                    Background=""{StaticResource DisabledBackgroundBrush}""
                    CornerRadius=""4""/>
                        <Border x:Name=""FocusVisualElement""
                    IsHitTestVisible=""false""
                    Visibility=""Collapsed""
                    BorderBrush=""{StaticResource HoverBrush}""
                    BorderThickness=""1""
                    CornerRadius=""3""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--PasswordBox-->
    <Style TargetType=""PasswordBox"">
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrush}"" />
        <Setter Property=""Foreground""
            Value=""#FF000000""/>
        <Setter Property=""Padding""
            Value=""2""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush }""  />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""PasswordBox"">
                    <Grid >
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""ReadOnly""/>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:00.1""
                                        To=""Disabled""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverBorder""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""DisabledVisualElement""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.6""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""FocusVisualElement""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Border""
                    Opacity=""1""
                    Background=""{TemplateBinding Background}""
                    BorderBrush=""{TemplateBinding BorderBrush}""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""1"">
                            <Border BorderThickness=""1"">
                                <Border.BorderBrush>
                                    <SolidColorBrush Color=""Transparent""
                                   x:Name=""MouseOverColor""/>
                                </Border.BorderBrush>
                                <Border Margin=""{TemplateBinding Padding}""
                        x:Name=""ContentElement""/>
                            </Border>
                        </Border>
                        <Border x:Name=""HoverBorder""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2,2,2,2""
                    CornerRadius=""2,2,2,2""
                    Opacity=""0""/>
                        <Border x:Name=""DisabledVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    Background=""#FFFFFFFF""
                    BorderBrush=""#A5F7F7F7""
                    BorderThickness=""{TemplateBinding BorderThickness}""
                    CornerRadius=""2,2,2,2""/>
                        <Border Margin=""1""
                    x:Name=""FocusVisualElement""
                    IsHitTestVisible=""False""
                    Opacity=""0""
                    BorderBrush=""{StaticResource NormalBrush}""
                    BorderThickness=""2.1,2.1,2.1,2.1""
                    CornerRadius=""0.2,0.2,0.2,0.2""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!--DataGridRowHeader-->
    <Style TargetType=""System_Windows_Controls_Primitives1:DataGridRowHeader"">
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""SeparatorBrush""
            Value=""#FFFFFFFF""/>
        <Setter Property=""SeparatorVisibility""
            Value=""Collapsed""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives1:DataGridRowHeader"">
                    <Grid x:Name=""Root"">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""Auto""/>
                            <ColumnDefinition Width=""*""/>
                        </Grid.ColumnDefinitions>
                        <Grid.RowDefinitions>
                            <RowDefinition Height=""*""/>
                            <RowDefinition Height=""*""/>
                            <RowDefinition Height=""Auto""/>
                        </Grid.RowDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:0.2""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""CurrentRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""EditingRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""MouseOver CurrentRow Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""CurrentRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""EditingRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Normal CurrentRow"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""CurrentRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""EditingRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Normal Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""CurrentRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""EditingRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Normal EditingRow"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""CurrentRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""EditingRowGlyph""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""grid""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.5""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border Grid.ColumnSpan=""2""
                    Grid.RowSpan=""3""
                    BorderBrush=""#FFFFFFFF""
                    BorderThickness=""1, 0, 1, 0"">
                            <Grid Height=""Auto""
                    Width=""Auto""
                    x:Name=""grid"">
                                <Rectangle Stretch=""Fill""
                           Fill=""{StaticResource SelectedBackgroundBrush}""/>
                                <Rectangle Stretch=""Fill""
                           Fill=""{StaticResource NormalBrush}""
                           x:Name=""SelectedRectangle""
                           Opacity=""0""/>
                                <Rectangle Stretch=""Fill""
                           Fill=""{StaticResource HoverBrush}""
                           x:Name=""HoverRectangle""
                           Opacity=""0""/>
                            </Grid>
                        </Border>
                        <Rectangle Height=""1""
                       HorizontalAlignment=""Stretch""
                       Margin=""1, 0, 1, 0""
                       x:Name=""HorizontalSeparator""
                       Visibility=""{TemplateBinding SeparatorVisibility}""
                       Grid.ColumnSpan=""2""
                       Grid.Row=""2""
                       Fill=""{TemplateBinding SeparatorBrush}""/>
                        <ContentPresenter HorizontalAlignment=""Center""
                              VerticalAlignment=""Center""
                              Grid.Column=""1""
                              Grid.RowSpan=""2""
                              Content=""{TemplateBinding Content}""/>
                        <Path Height=""10""
                  HorizontalAlignment=""Center""
                  Margin=""8,0,8,0""
                  x:Name=""CurrentRowGlyph""
                  VerticalAlignment=""Center""
                  Width=""6""
                  Opacity=""0""
                  Grid.RowSpan=""2""
                  Stretch=""Fill""
                  Data=""F1 M 511.047,352.682L 511.047,342.252L 517.145,347.467L 511.047,352.682 Z "">
                            <Path.Fill>
                                <LinearGradientBrush EndPoint=""0,1.75""
                                     StartPoint=""0,-.15"">
                                    <GradientStop Color=""#FF84E3FF""
                                Offset=""0""/>
                                    <GradientStop Color=""#FF6ABFD8""
                                Offset=""0.5""/>
                                    <GradientStop Color=""#FF5297AB""
                                Offset=""1""/>
                                </LinearGradientBrush>
                            </Path.Fill>
                        </Path>
                        <Path Height=""10""
                  HorizontalAlignment=""Center""
                  Margin=""8,0,8,0""
                  x:Name=""EditingRowGlyph""
                  VerticalAlignment=""Center""
                  Width=""6""
                  Opacity=""0""
                  Grid.RowSpan=""2""
                  Stretch=""Fill""
                  Data=""F1 M 511.047,352.682L 511.047,342.252L 517.145,347.467L 511.047,352.682 Z "">
                            <Path.Fill>
                                <LinearGradientBrush EndPoint=""0,1.75""
                                     StartPoint=""0,-.15"">
                                    <GradientStop Color=""#FF84E3FF""
                                Offset=""0""/>
                                    <GradientStop Color=""#FF6ABFD8""
                                Offset=""0.5""/>
                                    <GradientStop Color=""#FF5297AB""
                                Offset=""1""/>
                                </LinearGradientBrush>
                            </Path.Fill>
                        </Path>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
    </Style>
    <!--DataGridColumnHeader-->
    <Style TargetType=""System_Windows_Controls_Primitives1:DataGridColumnHeader"">
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Center""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Center""/>
        <Setter Property=""FontSize""
            Value=""10.5""/>
        <Setter Property=""FontWeight""
            Value=""Bold""/>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""SeparatorBrush""
            Value=""#FFC9CACA""/>
        <Setter Property=""Padding""
            Value=""4,4,5,4""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""System_Windows_Controls_Primitives1:DataGridColumnHeader"">
                    <Grid x:Name=""Root"">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""Auto""/>
                            <ColumnDefinition Width=""*""/>
                            <ColumnDefinition Width=""Auto""/>
                        </Grid.ColumnDefinitions>
                        <Grid.RowDefinitions>
                            <RowDefinition Height=""*""/>
                            <RowDefinition Height=""*""/>
                            <RowDefinition Height=""Auto""/>
                        </Grid.RowDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:0.1""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SortStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""00:00:0.1""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unsorted""/>
                                <vsm:VisualState x:Name=""SortAscending"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SortIcon""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1.0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""SortDescending"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SortIcon""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""1.0""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""SortIconTransform""
                                                   Storyboard.TargetProperty=""ScaleY"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""-.9""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""BackgroundRectangle""
                       Grid.ColumnSpan=""2""
                       Grid.RowSpan=""2""
                       Fill=""{StaticResource NormalBrush}""
                       Stretch=""Fill""
                       Stroke=""{StaticResource NormalBorderBrush}""
                       StrokeThickness=""2""/>
                        <Rectangle x:Name=""HoverRectangle""
                       Grid.ColumnSpan=""2""
                       Grid.RowSpan=""2""
                       Stretch=""Fill""
                       Fill=""{StaticResource HoverBrush}""
                       Opacity=""0""/>
                        <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                              HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Grid.RowSpan=""2""
                              Content=""{TemplateBinding Content}""/>
                        <Rectangle x:Name=""VerticalSeparator""
                       VerticalAlignment=""Stretch""
                       Width=""1""
                       Visibility=""{TemplateBinding SeparatorVisibility}""
                       Grid.Column=""2""
                       Grid.RowSpan=""2""
                       Fill=""{TemplateBinding SeparatorBrush}""/>
                        <Path HorizontalAlignment=""Left""
                  x:Name=""SortIcon""
                  VerticalAlignment=""Center""
                  Width=""8""
                  Opacity=""0""
                  RenderTransformOrigin="".5,.5""
                  Grid.Column=""1""
                  Grid.RowSpan=""2""
                  Fill=""#FFFFFFFF""
                  Stretch=""Uniform""
                  Data=""F1 M -5.215,6.099L 5.215,6.099L 0,0L -5.215,6.099 Z "">
                            <Path.RenderTransform>
                                <TransformGroup>
                                    <ScaleTransform ScaleX="".9""
                                  ScaleY="".9""
                                  x:Name=""SortIconTransform""/>
                                </TransformGroup>
                            </Path.RenderTransform>
                        </Path>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--DataGridCell-->

    <Style TargetType=""data:DataGridCell"">
        <Setter Property=""Background""
            Value=""Transparent""/>
        <Setter Property=""HorizontalContentAlignment""
            Value=""Stretch""/>
        <Setter Property=""VerticalContentAlignment""
            Value=""Stretch""/>
        <Setter Property=""Cursor""
            Value=""Arrow""/>
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""data:DataGridCell"">
                    <Grid x:Name=""Root""
                Background=""Transparent"">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""*""/>
                            <ColumnDefinition Width=""Auto""/>
                        </Grid.ColumnDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CurrentStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Regular""/>
                                <vsm:VisualState x:Name=""Current"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames Storyboard.TargetName=""FocusVisual""
                                                   Storyboard.TargetProperty=""Opacity"">
                                            <SplineDoubleKeyFrame KeyTime=""0""
                                            Value=""0.8""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle HorizontalAlignment=""Stretch""
                       x:Name=""FocusVisual""
                       VerticalAlignment=""Stretch""
                       IsHitTestVisible=""false""
                       Opacity=""0""
                       Fill=""{StaticResource DisabledBackgroundBrush}""
                       Stroke=""#FF6DBDD1""
                       StrokeThickness=""1""/>
                        <ContentPresenter Cursor=""{TemplateBinding Cursor}""
                              HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}""
                              Margin=""{TemplateBinding Padding}""
                              VerticalAlignment=""{TemplateBinding VerticalContentAlignment}""
                              Content=""{TemplateBinding Content}""
                              ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                        <Rectangle x:Name=""RightGridLine""
                       VerticalAlignment=""Stretch""
                       Width=""1""
                       Grid.Column=""1""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--DataGridRow-->
    <Style TargetType=""data:DataGridRow"">
        <Setter Property=""IsTabStop""
            Value=""False""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""data:DataGridRow"">
                    <System_Windows_Controls_Primitives1:DataGridFrozenGrid x:Name=""Root""
                                                                  Margin=""1,1,1,1"">
                        <System_Windows_Controls_Primitives1:DataGridFrozenGrid.Resources>
                            <Storyboard x:Key=""DetailsVisibleTransition"">
                                <DoubleAnimation Duration=""00:00:0.1""
                                 Storyboard.TargetName=""DetailsPresenter""
                                 Storyboard.TargetProperty=""ContentHeight""/>
                            </Storyboard>
                        </System_Windows_Controls_Primitives1:DataGridFrozenGrid.Resources>
                        <System_Windows_Controls_Primitives1:DataGridFrozenGrid.ColumnDefinitions>
                            <ColumnDefinition Width=""Auto""/>
                            <ColumnDefinition Width=""*""/>
                        </System_Windows_Controls_Primitives1:DataGridFrozenGrid.ColumnDefinitions>
                        <System_Windows_Controls_Primitives1:DataGridFrozenGrid.RowDefinitions>
                            <RowDefinition Height=""*""/>
                            <RowDefinition Height=""Auto""/>
                            <RowDefinition Height=""Auto""/>
                        </System_Windows_Controls_Primitives1:DataGridFrozenGrid.RowDefinitions>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver""/>
                                    <vsm:VisualTransition From=""MouseOver""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""MouseOver Selected""/>
                                    <vsm:VisualTransition From=""MouseOver Selected""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                    <vsm:VisualTransition From=""Normal""
                                        GeneratedDuration=""00:00:00.3000000""
                                        To=""Normal Selected""/>
                                    <vsm:VisualTransition From=""Normal Selected""
                                        GeneratedDuration=""00:00:00.5000000""
                                        To=""Normal""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal"">
                                    <Storyboard/>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Normal AlternatingRow"">
                                    <Storyboard/>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""MouseOver"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Normal Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""MouseOver Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""HoverRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""1""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00""
                                                   Duration=""00:00:00.0010000""
                                                   Storyboard.TargetName=""SelectedRectangle""
                                                   Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00""
                                            Value=""0.6""/>
                                        </DoubleAnimationUsingKeyFrames>

                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Rectangle x:Name=""BackgroundRectangle""
                       Grid.ColumnSpan=""2""
                       Grid.RowSpan=""2""
                       Fill=""{StaticResource SelectedBackgroundBrush}""/>
                        <Rectangle x:Name=""SelectedRectangle""
                       Opacity=""0""
                       Fill=""{StaticResource NormalBrush}""
                       Grid.ColumnSpan=""2""
                       Grid.RowSpan=""2"" />
                        <Rectangle x:Name=""HoverRectangle""
                       Fill=""{StaticResource HoverBrush}""
                       Opacity=""0""
                       Grid.ColumnSpan=""2""
                       Grid.RowSpan=""2""/>
                        <System_Windows_Controls_Primitives1:DataGridRowHeader x:Name=""RowHeader""
                                                                   Grid.RowSpan=""3""
                                                                   System_Windows_Controls_Primitives1:DataGridFrozenGrid.IsFrozen=""True""/>
                        <System_Windows_Controls_Primitives1:DataGridCellsPresenter x:Name=""CellsPresenter""
                                                                        Grid.Column=""1""
                                                                        System_Windows_Controls_Primitives1:DataGridFrozenGrid.IsFrozen=""True""/>
                        <System_Windows_Controls_Primitives1:DataGridDetailsPresenter x:Name=""DetailsPresenter""
                                                                          Grid.Column=""1""
                                                                          Grid.Row=""1""/>
                        <Rectangle Height=""1""
                       HorizontalAlignment=""Stretch""
                       x:Name=""BottomGridLine""
                       Grid.Column=""1""
                       Grid.Row=""2""/>
                    </System_Windows_Controls_Primitives1:DataGridFrozenGrid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <!--DateGrid-->
    <Style TargetType=""data:DataGrid"">
        <Setter Property=""RowBackground""
            Value=""#AAEAEFF4""/>
        <Setter Property=""AlternatingRowBackground""
            Value=""#00FFFFFF""/>
        <Setter Property=""Background""
            Value=""{StaticResource ShadeBrush}""/>
        <Setter Property=""RowHeaderStyle""
            Value=""{StaticResource System.Windows.Controls.Primitives.DataGridRowHeader}""/>


        <Setter Property=""ColumnHeaderStyle""
            Value=""{StaticResource System.Windows.Controls.Primitives.DataGridColumnHeader}""/>

        <Setter Property=""RowStyle""
            Value=""{StaticResource System.Windows.Controls.DataGridRow}""/>
        <Setter Property=""CellStyle""
            Value=""{StaticResource System.Windows.Controls.DataGridCell}""/>

        <Setter Property=""HeadersVisibility""
            Value=""Column""/>
        <Setter Property=""HorizontalScrollBarVisibility""
            Value=""Auto""/>
        <Setter Property=""VerticalScrollBarVisibility""
            Value=""Auto""/>
        <Setter Property=""SelectionMode""
            Value=""Extended""/>
        <Setter Property=""CanUserReorderColumns""
            Value=""True""/>
        <Setter Property=""CanUserResizeColumns""
            Value=""True""/>
        <Setter Property=""CanUserSortColumns""
            Value=""True""/>
        <Setter Property=""AutoGenerateColumns""
            Value=""True""/>
        <Setter Property=""RowDetailsVisibilityMode""
            Value=""VisibleWhenSelected""/>
        <Setter Property=""BorderBrush""
            Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""BorderThickness""
            Value=""1""/>
        <Setter Property=""DragIndicatorStyle"">
            <Setter.Value>
                <Style TargetType=""ContentControl"">
                    <Setter Property=""FontSize""
                  Value=""10.5""/>
                    <Setter Property=""FontWeight""
                  Value=""Bold""/>
                    <Setter Property=""Foreground""
                  Value=""#7FFFFFFF""/>
                    <Setter Property=""Template"">
                        <Setter.Value>
                            <ControlTemplate TargetType=""ContentControl"">
                                <Grid>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width=""Auto""/>
                                        <ColumnDefinition Width=""*""/>
                                        <ColumnDefinition Width=""Auto""/>
                                    </Grid.ColumnDefinitions>
                                    <Grid.RowDefinitions>
                                        <RowDefinition Height=""*""/>
                                        <RowDefinition Height=""*""/>
                                        <RowDefinition Height=""Auto""/>
                                    </Grid.RowDefinitions>
                                    <Rectangle x:Name=""BackgroundRectangle""
                             Grid.ColumnSpan=""2""
                             Grid.RowSpan=""2""
                             Fill=""#66808080""
                             Stretch=""Fill""/>
                                    <Rectangle x:Name=""BackgroundGradient""
                             Opacity=""0""
                             Grid.ColumnSpan=""2""
                             Grid.RowSpan=""2""
                             Stretch=""Fill"">
                                        <Rectangle.Fill>
                                            <LinearGradientBrush EndPoint="".7,1""
                                           StartPoint="".7,0"">
                                                <GradientStop Color=""#FFFFFFFF""
                                      Offset=""0.015""/>
                                                <GradientStop Color=""#F9FFFFFF""
                                      Offset=""0.375""/>
                                                <GradientStop Color=""#E5FFFFFF""
                                      Offset=""0.6""/>
                                                <GradientStop Color=""#C6FFFFFF""
                                      Offset=""1""/>
                                            </LinearGradientBrush>
                                        </Rectangle.Fill>
                                    </Rectangle>
                                    <ContentPresenter Margin=""4,4,5,4""
                                    VerticalAlignment=""Center""/>
                                    <Rectangle VerticalAlignment=""Stretch""
                             Width=""1""
                             Visibility=""Visible""
                             Grid.Column=""2""
                             Grid.RowSpan=""2""
                             Fill=""#FFAAAAAA""/>
                                    <Path HorizontalAlignment=""Left""
                        x:Name=""SortIcon""
                        VerticalAlignment=""Center""
                        Width=""8""
                        Opacity=""0""
                        RenderTransformOrigin="".5,.5""
                        Grid.Column=""1""
                        Fill=""#7FFFFFFF""
                        Stretch=""Uniform""
                        Data=""F1 M -5.215,0.0L 5.215,0.0L 0,6.099L -5.215,0.0 Z "">
                                        <Path.RenderTransform>
                                            <TransformGroup>
                                                <ScaleTransform ScaleX="".9""
                                        ScaleY="".9""/>
                                                <RotateTransform x:Name=""SortIconTransform""/>
                                            </TransformGroup>
                                        </Path.RenderTransform>
                                    </Path>
                                </Grid>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style>
            </Setter.Value>
        </Setter>
        <Setter Property=""DropLocationIndicatorStyle"">
            <Setter.Value>
                <Style TargetType=""Control"">
                    <Setter Property=""Background""
                  Value=""#FF3F4346""/>
                    <Setter Property=""Width""
                  Value=""2""/>
                    <Setter Property=""Template"">
                        <Setter.Value>
                            <ControlTemplate TargetType=""Control"">
                                <Rectangle Height=""{TemplateBinding Height}""
                           Width=""{TemplateBinding Width}""
                           Fill=""{TemplateBinding Background}""/>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style>
            </Setter.Value>
        </Setter>
        <Setter Property=""GridLinesVisibility""
            Value=""Vertical""/>
        <Setter Property=""HorizontalGridLinesBrush""
            Value=""#FFC9CACA""/>
        <Setter Property=""IsTabStop""
            Value=""True""/>
        <Setter Property=""VerticalGridLinesBrush""
            Value=""#FFC9CACA""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""data:DataGrid"">
                    <Border BorderBrush=""{TemplateBinding BorderBrush}""
                  BorderThickness=""{TemplateBinding BorderThickness}""
                  CornerRadius=""2"">
                        <Grid x:Name=""Root""
                  Background=""{TemplateBinding Background}""
                  Margin=""1,1,1,1"">
                            <Grid.Resources>
                                <ControlTemplate x:Key=""TopLeftHeaderTemplate""
                                 TargetType=""System_Windows_Controls_Primitives1:DataGridColumnHeader"">
                                    <Grid x:Name=""Root"">
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height=""*""/>
                                            <RowDefinition Height=""*""/>
                                            <RowDefinition Height=""Auto""/>
                                        </Grid.RowDefinitions>
                                        <Border Grid.RowSpan=""2""
                            Background=""#FF1F3B53""
                            BorderBrush=""#FFC9CACA""
                            BorderThickness=""0,0,1,0"">
                                            <Rectangle Stretch=""Fill""
                                 StrokeThickness=""1"">
                                                <Rectangle.Fill>
                                                    <LinearGradientBrush EndPoint="".7,1""
                                               StartPoint="".7,0"">
                                                        <GradientStop Color=""#FFFFFFFF""
                                          Offset=""0.015""/>
                                                        <GradientStop Color=""#F9FFFFFF""
                                          Offset=""0.375""/>
                                                        <GradientStop Color=""#E5FFFFFF""
                                          Offset=""0.6""/>
                                                        <GradientStop Color=""#C6FFFFFF""
                                          Offset=""1""/>
                                                    </LinearGradientBrush>
                                                </Rectangle.Fill>
                                            </Rectangle>
                                        </Border>
                                        <Rectangle Height=""1""
                               VerticalAlignment=""Bottom""
                               Width=""Auto""
                               Grid.RowSpan=""2""
                               Fill=""{StaticResource NormalBrush}""
                               StrokeThickness=""2""
                               Stroke=""{StaticResource NormalBorderBrush}""/>
                                    </Grid>
                                </ControlTemplate>
                                <ControlTemplate x:Key=""TopRightHeaderTemplate""
                                 TargetType=""System_Windows_Controls_Primitives1:DataGridColumnHeader"">
                                    <Grid >
                                        <Grid.RowDefinitions>
                                            <RowDefinition Height=""*""/>
                                            <RowDefinition Height=""*""/>
                                            <RowDefinition Height=""Auto""/>
                                        </Grid.RowDefinitions>
                                        <Border Grid.RowSpan=""2""
                            Background=""#FF1F3B53""
                            BorderBrush=""#FFC9CACA""
                            BorderThickness=""1,0,0,0"">
                                            <Rectangle Stretch=""Fill""
                                 Fill=""{StaticResource NormalBrush}""
                                 Stroke=""{StaticResource NormalBorderBrush}""
                                 StrokeThickness=""2""/>
                                        </Border>
                                    </Grid>
                                </ControlTemplate>
                            </Grid.Resources>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width=""Auto""/>
                                <ColumnDefinition Width=""*""/>
                                <ColumnDefinition Width=""Auto""/>
                            </Grid.ColumnDefinitions>
                            <Grid.RowDefinitions>
                                <RowDefinition Height=""Auto""/>
                                <RowDefinition Height=""*""/>
                                <RowDefinition Height=""Auto""/>
                            </Grid.RowDefinitions>
                            <System_Windows_Controls_Primitives1:DataGridColumnHeader Template=""{StaticResource TopLeftHeaderTemplate}""
                                                                        x:Name=""TopLeftCornerHeader""
                                                                        Width=""22""/>
                            <System_Windows_Controls_Primitives1:DataGridColumnHeadersPresenter x:Name=""ColumnHeadersPresenter""
                                                                                  Grid.Column=""1""/>
                            <System_Windows_Controls_Primitives1:DataGridColumnHeader Template=""{StaticResource TopRightHeaderTemplate}""
                                                                        x:Name=""TopRightCornerHeader""
                                                                        Grid.Column=""2""/>
                            <Rectangle Height=""1""
                         x:Name=""ColumnHeadersAndRowsSeparator""
                         VerticalAlignment=""Bottom""
                         Width=""Auto""
                         Grid.ColumnSpan=""3""
                         Fill=""#FFDBDCDC""
                         StrokeThickness=""1""/>
                            <System_Windows_Controls_Primitives1:DataGridRowsPresenter x:Name=""RowsPresenter""
                                                                         Grid.ColumnSpan=""2""
                                                                         Grid.Row=""1""/>
                            <Rectangle x:Name=""BottomRightCorner""
                         Grid.Column=""2""
                         Grid.Row=""2""
                         Fill=""#FFE9EEF4""/>
                            <Rectangle x:Name=""BottomLeftCorner""
                         Grid.ColumnSpan=""2""
                         Grid.Row=""2""
                         Fill=""#FFE9EEF4""/>
                            <ScrollBar Margin=""0,-1,-1,-1""
                         x:Name=""VerticalScrollbar""
                         Width=""18""
                         Grid.Column=""2""
                         Grid.Row=""1""
                         Orientation=""Vertical""
                         Style=""{StaticResource System.Windows.Controls.Primitives.ScrollBar}""/>
                            <Grid Grid.Column=""1""
                    Grid.Row=""2"">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width=""Auto""/>
                                    <ColumnDefinition Width=""*""/>
                                </Grid.ColumnDefinitions>
                                <Rectangle x:Name=""FrozenColumnScrollBarSpacer""/>
                                <ScrollBar Height=""18""
                           Margin=""-1,0,-1,-1""
                           x:Name=""HorizontalScrollbar""
                           Grid.Column=""1""
                           Orientation=""Horizontal""
                           Style=""{StaticResource System.Windows.Controls.Primitives.ScrollBar}""/>
                            </Grid>
                        </Grid>
                    </Border>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property=""Foreground""
            Value=""#FFFFFFFF""/>
    </Style>



    <!-- new -->


    <!-- AutoCompleteBox -->
    <Style
    TargetType=""controls:AutoCompleteBox"">
        <Setter Property=""SearchMode"" Value=""StartsWith""/>
        <Setter Property=""Background"" Value=""#FF1F3B53""/>
        <Setter Property=""IsTabStop"" Value=""False""/>
        <Setter Property=""HorizontalContentAlignment"" Value=""Left""/>
        <Setter Property=""TabNavigation"" Value=""Once""/>
        <Setter Property=""BorderBrush"" Value=""{StaticResource NormalBorderBrush}"" />
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""controls:AutoCompleteBox"">
                    <Grid Margin=""{TemplateBinding Padding}"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""PopupStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.1""
                                        To=""PopupOpened""/>
                                    <vsm:VisualTransition GeneratedDuration=""0:0:0.2""
                                        To=""PopupClosed""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""PopupOpened"">
                                    <Storyboard>
                                        <DoubleAnimation Storyboard.TargetName=""PopupBorder""
                                     Storyboard.TargetProperty=""Opacity""
                                     To=""1.0""/>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""PopupClosed"">
                                    <Storyboard>
                                        <DoubleAnimation Storyboard.TargetName=""PopupBorder""
                                     Storyboard.TargetProperty=""Opacity""
                                     To=""0.0""/>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <TextBox Margin=""0""  x:Name=""Text"" IsTabStop=""True"" Style=""{StaticResource System.Windows.Controls.TextBox}"" />
                        <Popup x:Name=""Popup"">
                            <Border HorizontalAlignment=""Stretch"" x:Name=""PopupBorder"" Opacity=""0"" Background=""{StaticResource ShadeBrush}"" BorderThickness=""0"" CornerRadius=""3"">
                                <Border.RenderTransform>
                                    <TranslateTransform X=""1"" Y=""1""/>
                                </Border.RenderTransform>
                                <Border HorizontalAlignment=""Stretch""
                        Opacity=""1.0""
                        BorderBrush=""{TemplateBinding BorderBrush}""
                        BorderThickness=""1""
                        CornerRadius=""3""
                        Padding=""0""
                        Background=""{x:Null}"">
                                    <Border.RenderTransform>
                                        <TransformGroup>
                                            <ScaleTransform/>
                                            <SkewTransform/>
                                            <RotateTransform/>
                                            <TranslateTransform X=""-1""
                                          Y=""-1""/>
                                        </TransformGroup>
                                    </Border.RenderTransform>
                                    <ListBox x:Name=""SelectionAdapter""
                           ScrollViewer.HorizontalScrollBarVisibility=""Auto""
                           ScrollViewer.VerticalScrollBarVisibility=""Auto""
                           ItemTemplate=""{TemplateBinding ItemTemplate}""
                           Background=""{x:Null}""
                           Style=""{StaticResource System.Windows.Controls.ListBox}""
                           />
                                </Border>
                            </Border>
                        </Popup>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- Button Spinner -->
    <Style TargetType=""input:ButtonSpinner"">
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""input:ButtonSpinner"">
                    <Grid>
                        <Grid.Resources>
                            <ControlTemplate x:Key=""IncreaseButton"" TargetType=""RepeatButton"">
                                <Grid>
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Highlight"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"">
                                                            <DiscreteObjectKeyFrame.Value>
                                                                <Visibility>Visible</Visibility>
                                                            </DiscreteObjectKeyFrame.Value>
                                                        </DiscreteObjectKeyFrame>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background"" Fill=""{StaticResource NormalBrush}"" StrokeThickness=""2"" RadiusX=""2"" RadiusY=""2"" Stroke=""{StaticResource NormalBorderBrush}""/>
                                    <Rectangle Margin=""2,2,2,2"" x:Name=""Highlight"" IsHitTestVisible=""false"" Opacity=""0"" Stroke=""{x:Null}"" StrokeThickness=""1"" RadiusX=""1"" RadiusY=""1"" Fill=""{StaticResource HoverBrush}""/>
                                    <Rectangle x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" Stroke=""{StaticResource HoverBrush}"" StrokeThickness=""1"" RadiusX=""2"" RadiusY=""2""/>
                                    <Path Height=""4"" HorizontalAlignment=""Stretch"" Margin=""0,0,0,0"" VerticalAlignment=""Stretch"" Width=""8"" Stretch=""Uniform"" Data=""F1 M 541.537,173.589L 531.107,173.589L 536.322,167.49L 541.537,173.589 Z "" Fill=""#FFFFFFFF""/>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""DecreaseButton"" TargetType=""RepeatButton"">
                                <Grid>
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Highlight"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"">
                                                            <DiscreteObjectKeyFrame.Value>
                                                                <Visibility>Visible</Visibility>
                                                            </DiscreteObjectKeyFrame.Value>
                                                        </DiscreteObjectKeyFrame>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Rectangle x:Name=""Background"" StrokeThickness=""2"" RadiusX=""2"" RadiusY=""2"" Fill=""{StaticResource NormalBrush}"" Stroke=""{StaticResource NormalBorderBrush}""/>
                                    <Rectangle Margin=""2,2,2,2"" x:Name=""Highlight"" IsHitTestVisible=""false"" Opacity=""0"" Stroke=""{x:Null}"" StrokeThickness=""1"" RadiusX=""1"" RadiusY=""1"" Fill=""{StaticResource HoverBrush}""/>
                                    <Rectangle x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" Stroke=""{StaticResource HoverBrush}"" StrokeThickness=""1"" RadiusX=""2"" RadiusY=""2""/>
                                    <Path Height=""4"" HorizontalAlignment=""Stretch"" Margin=""0,0,0,0"" VerticalAlignment=""Stretch"" Width=""8"" Stretch=""Uniform"" Data=""F1 M 531.107,321.943L 541.537,321.943L 536.322,328.042L 531.107,321.943 Z "">
                                        <Path.Fill>
                                            <SolidColorBrush Color=""#FFFFFFFF"" x:Name=""ButtonColor""/>
                                        </Path.Fill>
                                    </Path>
                                </Grid>
                            </ControlTemplate>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""DisabledVisualElement"" Storyboard.TargetProperty=""(UIElement.Opacity)"" To=""1""/>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <StackPanel HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Orientation=""Vertical"">
                            <RepeatButton Height=""10"" HorizontalAlignment=""Left"" x:Name=""IncreaseButton"" VerticalAlignment=""Top"" Width=""15"" IsTabStop=""False"" Template=""{StaticResource IncreaseButton}""/>
                            <ContentPresenter Content=""{TemplateBinding Content}""/>
                            <RepeatButton Height=""10"" HorizontalAlignment=""Left"" x:Name=""DecreaseButton"" VerticalAlignment=""Top"" Width=""15"" IsTabStop=""False"" Template=""{StaticResource DecreaseButton}"" ClickMode=""Press""/>
                        </StackPanel>
                        <Border x:Name=""DisabledVisualElement"" IsHitTestVisible=""false"" Opacity=""0"" Background=""#A5FFFFFF"" CornerRadius=""3""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- NumericUpDown-->
    <Style TargetType=""input:NumericUpDown"">
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""input:NumericUpDown"">
                    <Grid>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""DisabledVisualElement"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.4""/>
                                        </DoubleAnimationUsingKeyFrames>
                                        <ColorAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""DisabledVisualElement"" Storyboard.TargetProperty=""(Border.Background).(SolidColorBrush.Color)"">
                                            <SplineColorKeyFrame KeyTime=""00:00:00"" Value=""#FFFFFFFF""/>
                                        </ColorAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Focused"" GeneratedDuration=""00:00:00.5000000"" To=""Unfocused""/>
                                    <vsm:VisualTransition From=""Unfocused"" GeneratedDuration=""00:00:00.3000000"" To=""Focused""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border Background=""{TemplateBinding Background}"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" Padding=""{TemplateBinding Padding}"">
                            <Grid>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width=""*""/>
                                    <ColumnDefinition Width=""Auto""/>
                                </Grid.ColumnDefinitions>
                                <TextBox Height=""20"" HorizontalAlignment=""Stretch"" MinWidth=""70"" x:Name=""Text"" FontFamily=""{TemplateBinding FontFamily}"" FontSize=""{TemplateBinding FontSize}"" FontStretch=""{TemplateBinding FontStretch}"" FontStyle=""{TemplateBinding FontStyle}"" FontWeight=""{TemplateBinding FontWeight}"" Foreground=""{TemplateBinding Foreground}"" AcceptsReturn=""False"" Text=""{TemplateBinding Value}"" TextAlignment=""Right"" TextWrapping=""NoWrap"" Style=""{StaticResource System.Windows.Controls.TextBox}"" Margin=""0,0,8,0"" />
                                <input:ButtonSpinner x:Name=""Spinner"" Style=""{StaticResource Microsoft.Windows.Controls.ButtonSpinner}"" IsTabStop=""False"" Grid.Column=""1"" Margin=""-11,0,0,0""/>
                            </Grid>
                        </Border>
                        <Border x:Name=""DisabledVisualElement"" IsHitTestVisible=""false"" Opacity=""0"" Background=""#FF000000"" CornerRadius=""4,4,4,4""/>
                        <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""{StaticResource NormalBrush}"" BorderThickness=""1"" CornerRadius=""1,1,1,1""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- Expander -->

    <Style TargetType=""controls:Expander"">
        <Setter Property=""HorizontalContentAlignment"" Value=""Stretch""/>
        <Setter Property=""VerticalContentAlignment"" Value=""Stretch""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""controls:Expander"">
                    <Grid Background=""Transparent"">
                        <Grid.Resources>

                            <Style x:Key=""SLExpanderContentControl"" TargetType=""ContentControl"">
                                <Setter Property=""Foreground"" Value=""#FF000000""/>
                                <Setter Property=""HorizontalContentAlignment"" Value=""Left""/>
                                <Setter Property=""VerticalContentAlignment"" Value=""Top""/>
                                <Setter Property=""Template"">
                                    <Setter.Value>
                                        <ControlTemplate TargetType=""ContentControl"">
                                            <Grid>
                                                <Border Background=""{StaticResource ShadeBrush}"" CornerRadius=""3,3,3,3"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""1,1,1,1""/>
                                                <ContentPresenter Cursor=""{TemplateBinding Cursor}"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""4,4,4,4"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                                            </Grid>
                                        </ControlTemplate>
                                    </Setter.Value>
                                </Setter>
                            </Style>

                            <LinearGradientBrush x:Key=""ExpanderArrowFill"" EndPoint=""0,1"" StartPoint=""0,0"">
                                <GradientStop Color=""White"" Offset=""0""/>
                                <GradientStop Color=""#FFBFBFBF"" Offset=""0.5""/>
                                <GradientStop Color=""#FF878787"" Offset=""1""/>
                            </LinearGradientBrush>
                            <LinearGradientBrush x:Key=""ExpanderArrowHoverFill"" EndPoint=""0,1"" StartPoint=""0,0"">
                                <GradientStop Color=""#FFF0F8FE"" Offset=""0""/>
                                <GradientStop Color=""#FFE0F3FE"" Offset=""0.3""/>
                                <GradientStop Color=""#FF6FA7C5"" Offset=""1""/>
                            </LinearGradientBrush>
                            <LinearGradientBrush x:Key=""ExpanderArrowPressedFill"" EndPoint=""0,1"" StartPoint=""0,0"">
                                <GradientStop Color=""#FFDCF0FA"" Offset=""0""/>
                                <GradientStop Color=""#FFC5E6F7"" Offset=""0.2""/>
                                <GradientStop Color=""#FF5690D0"" Offset=""1""/>
                            </LinearGradientBrush>
                            <ControlTemplate x:Key=""ExpanderDownHeaderTemplate"" TargetType=""ToggleButton"">
                                <Grid Background=""Transparent"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CheckStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Checked"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""arrow"" Storyboard.TargetProperty=""Data"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""M 1,4.5 L 4.5,1 L 8,4.5""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unchecked""/>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                                <vsm:VisualTransition From=""Normal"" GeneratedDuration=""00:00:00.3000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.5000000"" To=""Normal""/>
                                                <vsm:VisualTransition From=""Pressed"" GeneratedDuration=""00:00:00.5000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.3000000"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""HoverBorder"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""FullControlHover"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>

                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>

                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition From=""Focused"" GeneratedDuration=""00:00:00.5000000"" To=""Unfocused""/>
                                                <vsm:VisualTransition From=""Unfocused"" GeneratedDuration=""00:00:00.3000000"" To=""Focused""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Border Padding=""{TemplateBinding Padding}"" CornerRadius=""3,3,3,3"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" Background=""#FFFFFFFF"">
                                        <Grid Background=""Transparent"">
                                            <Grid.ColumnDefinitions>
                                                <ColumnDefinition Width=""19""/>
                                                <ColumnDefinition Width=""*""/>
                                            </Grid.ColumnDefinitions>
                                            <Grid HorizontalAlignment=""Left"" VerticalAlignment=""Top"">
                                                <Border Height=""19"" Width=""19"" Background=""{StaticResource NormalBrush}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" x:Name=""Background""/>
                                                <Border Height=""19"" x:Name=""HoverBorder"" Width=""19"" Background=""{StaticResource HoverBrush}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" Opacity=""0""/>
                                                <Path HorizontalAlignment=""Center"" x:Name=""arrow"" VerticalAlignment=""Center"" StrokeThickness=""2"" Data=""M 1,1.5 L 4.5,5 L 8,1.5"">
                                                    <Path.Stroke>
                                                        <SolidColorBrush Color=""{StaticResource TextBrush}""/>
                                                    </Path.Stroke>
                                                </Path>
                                            </Grid>
                                            <ContentPresenter HorizontalAlignment=""Left"" Margin=""4,0,0,0"" x:Name=""header"" VerticalAlignment=""Center"" Grid.Column=""1"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                                        </Grid>
                                    </Border>
                                    <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""{StaticResource HoverBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3""/>
                                    <Border x:Name=""FullControlHover"" IsHitTestVisible=""false"" Visibility=""Visible"" BorderBrush=""{StaticResource NormalBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3"" Opacity=""0""/>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""ExpanderUpHeaderTemplate"" TargetType=""ToggleButton"">
                                <Grid Background=""Transparent"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CheckStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Checked"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""arrow"" Storyboard.TargetProperty=""Data"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""M 1,4.5 L 4.5,1 L 8,4.5""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unchecked""/>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                                <vsm:VisualTransition From=""Normal"" GeneratedDuration=""00:00:00.3000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.5000000"" To=""Normal""/>
                                                <vsm:VisualTransition From=""Pressed"" GeneratedDuration=""00:00:00.5000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.3000000"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""HoverBorder"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""FullControlHover"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Border Padding=""{TemplateBinding Padding}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" Background=""#FFFFFFFF"">
                                        <Grid Background=""Transparent"">
                                            <Grid.ColumnDefinitions>
                                                <ColumnDefinition Width=""19""/>
                                                <ColumnDefinition Width=""*""/>
                                            </Grid.ColumnDefinitions>
                                            <Grid HorizontalAlignment=""Left"" VerticalAlignment=""Top"">
                                                <Grid.RenderTransform>
                                                    <TransformGroup>
                                                        <TransformGroup.Children>
                                                            <TransformCollection>
                                                                <RotateTransform Angle=""180"" CenterX=""9.5"" CenterY=""9.5""/>
                                                            </TransformCollection>
                                                        </TransformGroup.Children>
                                                    </TransformGroup>
                                                </Grid.RenderTransform>
                                                <Border Height=""19"" Width=""19"" Background=""{StaticResource NormalBrush}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" x:Name=""Background"" RenderTransformOrigin=""0.5,0.5"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""180""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Border Height=""19"" x:Name=""HoverBorder"" Width=""19"" Background=""{StaticResource HoverBrush}"" BorderBrush=""{x:Null}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" RenderTransformOrigin=""0.5,0.5"" Opacity=""0"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""180""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Path HorizontalAlignment=""Center"" x:Name=""arrow"" VerticalAlignment=""Center"" StrokeThickness=""2"" Data=""M 1,1.5 L 4.5,5 L 8,1.5"">
                                                    <Path.Stroke>
                                                        <SolidColorBrush Color=""{StaticResource TextBrush}""/>
                                                    </Path.Stroke>
                                                </Path>
                                            </Grid>
                                            <ContentPresenter HorizontalAlignment=""Left"" Margin=""4,0,0,0"" x:Name=""header"" VerticalAlignment=""Center"" Grid.Column=""1"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                                        </Grid>
                                    </Border>
                                    <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""{StaticResource HoverBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3""/>
                                    <Border x:Name=""FullControlHover"" IsHitTestVisible=""false"" Visibility=""Visible"" BorderBrush=""{StaticResource NormalBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3"" Opacity=""0""/>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""ExpanderLeftHeaderTemplate"" TargetType=""ToggleButton"">
                                <Grid Background=""Transparent"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CheckStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Checked"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""arrow"" Storyboard.TargetProperty=""Data"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""M 1,4.5 L 4.5,1 L 8,4.5""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unchecked""/>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                                <vsm:VisualTransition From=""Normal"" GeneratedDuration=""00:00:00.3000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.5000000"" To=""Normal""/>
                                                <vsm:VisualTransition From=""Pressed"" GeneratedDuration=""00:00:00.5000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.3000000"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""HoverBorder"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""FullControlHover"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Border CornerRadius=""3,3,3,3"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" Padding=""0,2,0,2"" Width=""38"" Background=""#FFFFFFFF"">
                                        <Grid Background=""Transparent"">
                                            <Grid.RowDefinitions>
                                                <RowDefinition Height=""19""/>
                                                <RowDefinition Height=""*""/>
                                            </Grid.RowDefinitions>
                                            <Grid HorizontalAlignment=""Center"" VerticalAlignment=""Top"">
                                                <Grid.RenderTransform>
                                                    <TransformGroup>
                                                        <TransformGroup.Children>
                                                            <TransformCollection>
                                                                <RotateTransform Angle=""90"" CenterX=""9.5"" CenterY=""9.5""/>
                                                            </TransformCollection>
                                                        </TransformGroup.Children>
                                                    </TransformGroup>
                                                </Grid.RenderTransform>
                                                <Border Height=""19"" x:Name=""Background"" Width=""19"" RenderTransformOrigin=""0.5,0.5"" Background=""{StaticResource NormalBrush}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""-90""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Border Height=""19"" x:Name=""HoverBorder"" Width=""19"" RenderTransformOrigin=""0.5,0.5"" Background=""{StaticResource HoverBrush}"" BorderBrush=""{x:Null}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" Opacity=""0"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""-90""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Path HorizontalAlignment=""Center"" x:Name=""arrow"" VerticalAlignment=""Center"" StrokeThickness=""2"" Data=""M 1,1.5 L 4.5,5 L 8,1.5"">
                                                    <Path.Stroke>
                                                        <SolidColorBrush Color=""{StaticResource TextBrush}""/>
                                                    </Path.Stroke>
                                                </Path>
                                            </Grid>
                                        </Grid>
                                    </Border>
                                    <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""{StaticResource HoverBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3""/>
                                    <Border x:Name=""FullControlHover"" IsHitTestVisible=""false"" Visibility=""Visible"" BorderBrush=""{StaticResource NormalBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3"" Opacity=""0""/>
                                    <ContentPresenter x:Name=""header"" VerticalAlignment=""Top"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}"" RenderTransformOrigin=""0.5,0.5"" HorizontalAlignment=""Center"" Margin=""0,42,0,0"" >
                                        <ContentPresenter.RenderTransform>
                                            <TransformGroup>
                                                <ScaleTransform/>
                                                <SkewTransform/>
                                                <RotateTransform Angle=""90""/>
                                                <TranslateTransform/>
                                            </TransformGroup>
                                        </ContentPresenter.RenderTransform>
                                    </ContentPresenter>
                                </Grid>
                            </ControlTemplate>
                            <ControlTemplate x:Key=""ExpanderRightHeaderTemplate"" TargetType=""ToggleButton"">
                                <Grid Background=""Transparent"">
                                    <vsm:VisualStateManager.VisualStateGroups>
                                        <vsm:VisualStateGroup x:Name=""CheckStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Checked"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""arrow"" Storyboard.TargetProperty=""Data"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""M 1,4.5 L 4.5,1 L 8,4.5""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unchecked""/>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""CommonStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition GeneratedDuration=""0""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""MouseOver""/>
                                                <vsm:VisualTransition GeneratedDuration=""00:00:00.1"" To=""Pressed""/>
                                                <vsm:VisualTransition From=""Normal"" GeneratedDuration=""00:00:00.3000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.5000000"" To=""Normal""/>
                                                <vsm:VisualTransition From=""Pressed"" GeneratedDuration=""00:00:00.5000000"" To=""MouseOver""/>
                                                <vsm:VisualTransition From=""MouseOver"" GeneratedDuration=""00:00:00.3000000"" To=""Pressed""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Normal""/>
                                            <vsm:VisualState x:Name=""MouseOver"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""HoverBorder"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""FullControlHover"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""1""/>
                                                    </DoubleAnimationUsingKeyFrames>

                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Pressed"">
                                                <Storyboard>
                                                    <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                        <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.7""/>
                                                    </DoubleAnimationUsingKeyFrames>

                                                </Storyboard>
                                            </vsm:VisualState>
                                        </vsm:VisualStateGroup>
                                        <vsm:VisualStateGroup x:Name=""FocusStates"">
                                            <vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualTransition From=""Focused"" GeneratedDuration=""00:00:00.5000000"" To=""Unfocused""/>
                                                <vsm:VisualTransition From=""Unfocused"" GeneratedDuration=""00:00:00.3000000"" To=""Focused""/>
                                            </vsm:VisualStateGroup.Transitions>
                                            <vsm:VisualState x:Name=""Focused"">
                                                <Storyboard>
                                                    <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                                        <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                                    </ObjectAnimationUsingKeyFrames>
                                                </Storyboard>
                                            </vsm:VisualState>
                                            <vsm:VisualState x:Name=""Unfocused""/>
                                        </vsm:VisualStateGroup>
                                    </vsm:VisualStateManager.VisualStateGroups>
                                    <Border Padding=""{TemplateBinding Padding}"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" Width=""38"" Background=""#FFFFFFFF"">
                                        <Grid Background=""Transparent"">
                                            <Grid.RowDefinitions>
                                                <RowDefinition Height=""19""/>
                                                <RowDefinition Height=""*""/>
                                            </Grid.RowDefinitions>
                                            <Grid HorizontalAlignment=""Center"" VerticalAlignment=""Top"">
                                                <Grid.RenderTransform>
                                                    <TransformGroup>
                                                        <TransformGroup.Children>
                                                            <TransformCollection>
                                                                <RotateTransform Angle=""-90"" CenterX=""9.5"" CenterY=""9.5""/>
                                                            </TransformCollection>
                                                        </TransformGroup.Children>
                                                    </TransformGroup>
                                                </Grid.RenderTransform>
                                                <Border Background=""{StaticResource NormalBrush}"" Width=""19"" Height=""19"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" RenderTransformOrigin=""0.5,0.5"" x:Name=""Background"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""90""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Border Height=""19"" Width=""19"" RenderTransformOrigin=""0.5,0.5"" Background=""{StaticResource HoverBrush}"" BorderBrush=""{x:Null}"" BorderThickness=""2,2,2,2"" CornerRadius=""3,3,3,3"" Opacity=""0"" x:Name=""HoverBorder"">
                                                    <Border.RenderTransform>
                                                        <TransformGroup>
                                                            <ScaleTransform/>
                                                            <SkewTransform/>
                                                            <RotateTransform Angle=""90""/>
                                                            <TranslateTransform/>
                                                        </TransformGroup>
                                                    </Border.RenderTransform>
                                                </Border>
                                                <Path HorizontalAlignment=""Center"" x:Name=""arrow"" VerticalAlignment=""Center"" StrokeThickness=""2"" Data=""M 1,1.5 L 4.5,5 L 8,1.5"">
                                                    <Path.Stroke>
                                                        <SolidColorBrush Color=""{StaticResource TextBrush}""/>
                                                    </Path.Stroke>
                                                </Path>
                                            </Grid>
                                        </Grid>
                                    </Border>
                                    <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""{StaticResource HoverBrush}"" BorderThickness=""1"" CornerRadius=""3""/>
                                    <Border x:Name=""FullControlHover"" IsHitTestVisible=""false"" Visibility=""Visible"" BorderBrush=""{StaticResource NormalBrush}"" BorderThickness=""2,2,2,2"" CornerRadius=""3"" Opacity=""0""/>
                                    <ContentPresenter x:Name=""header"" VerticalAlignment=""Top"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}""  RenderTransformOrigin=""0.5,0.5"" HorizontalAlignment=""Center"" Margin=""0,42,0,0"">
                                        <ContentPresenter.RenderTransform>
                                            <TransformGroup>
                                                <ScaleTransform/>
                                                <SkewTransform/>
                                                <RotateTransform Angle=""-90""/>
                                                <TranslateTransform/>
                                            </TransformGroup>
                                        </ContentPresenter.RenderTransform>
                                    </ContentPresenter>
                                </Grid>
                            </ControlTemplate>
                        </Grid.Resources>
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""DisabledVisualElement"" Storyboard.TargetProperty=""(UIElement.Opacity)"" To=""1""/>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""FocusStates"">
                                <vsm:VisualState x:Name=""Focused"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""FocusVisualElement"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""Unfocused""/>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""ExpansionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Collapsed""/>
                                <vsm:VisualState x:Name=""Expanded"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpandSite"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""ExpandDirectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition GeneratedDuration=""0""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""ExpandDown"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""rd1"" Storyboard.TargetProperty=""Height"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""cd0"" Storyboard.TargetProperty=""Width"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""ExpandUp"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""Template"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""{StaticResource ExpanderUpHeaderTemplate}""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""(Grid.Row)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""1""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpandSite"" Storyboard.TargetProperty=""(Grid.Row)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""0""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""rd0"" Storyboard.TargetProperty=""Height"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""cd0"" Storyboard.TargetProperty=""Width"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""ExpandLeft"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""Template"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""{StaticResource ExpanderLeftHeaderTemplate}""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""(Grid.Column)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""1""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpandSite"" Storyboard.TargetProperty=""(Grid.Row)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""0""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""rd0"" Storyboard.TargetProperty=""Height"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""cd0"" Storyboard.TargetProperty=""Width"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""ExpandRight"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""Template"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""{StaticResource ExpanderRightHeaderTemplate}""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpandSite"" Storyboard.TargetProperty=""(Grid.Row)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""0""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""1"" Storyboard.TargetName=""ExpandSite"" Storyboard.TargetProperty=""(Grid.Column)"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""1""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""rd0"" Storyboard.TargetProperty=""Height"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""cd1"" Storyboard.TargetProperty=""Width"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""*""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Border x:Name=""Background"" Background=""{TemplateBinding Background}"" BorderBrush=""{TemplateBinding BorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" CornerRadius=""3"" Padding=""{TemplateBinding Padding}"">
                            <Grid>
                                <Grid.RowDefinitions>
                                    <RowDefinition Height=""Auto"" x:Name=""rd0""/>
                                    <RowDefinition Height=""Auto"" x:Name=""rd1""/>
                                </Grid.RowDefinitions>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width=""Auto"" x:Name=""cd0""/>
                                    <ColumnDefinition Width=""Auto"" x:Name=""cd1""/>
                                </Grid.ColumnDefinitions>
                                <ToggleButton HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""1"" MinHeight=""0"" MinWidth=""0"" x:Name=""ExpanderButton"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" FontFamily=""{TemplateBinding FontFamily}"" FontSize=""{TemplateBinding FontSize}"" FontStretch=""{TemplateBinding FontStretch}"" FontStyle=""{TemplateBinding FontStyle}"" FontWeight=""{TemplateBinding FontWeight}"" Foreground=""{TemplateBinding Foreground}"" Template=""{StaticResource ExpanderDownHeaderTemplate}"" Grid.Column=""0"" Grid.Row=""0"" Content=""{TemplateBinding Header}"" ContentTemplate=""{TemplateBinding HeaderTemplate}"" IsChecked=""{TemplateBinding IsExpanded}""/>
                                <ContentControl HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" Margin=""1"" MinHeight=""0"" MinWidth=""0"" x:Name=""ExpandSite"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" Visibility=""Collapsed"" FontFamily=""{TemplateBinding FontFamily}"" FontSize=""{TemplateBinding FontSize}"" FontStretch=""{TemplateBinding FontStretch}"" FontStyle=""{TemplateBinding FontStyle}"" FontWeight=""{TemplateBinding FontWeight}"" Foreground=""{TemplateBinding Foreground}"" Grid.Column=""0"" Grid.Row=""1"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}"" Style=""{StaticResource SLExpanderContentControl}""/>
                            </Grid>
                        </Border>
                        <Border x:Name=""DisabledVisualElement"" IsHitTestVisible=""false"" Opacity=""0"" Background=""#A5FFFFFF"" CornerRadius=""3""/>
                        <Border x:Name=""FocusVisualElement"" IsHitTestVisible=""false"" Visibility=""Collapsed"" BorderBrush=""#FF45D6FA"" BorderThickness=""1"" CornerRadius=""3""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- TreeView Item-->

    <Style TargetType=""controls:TreeViewItem"">
        <Setter Property=""IsTabStop"" Value=""True""/>
        <Setter Property=""Padding"" Value=""3""/>
        <Setter Property=""HorizontalContentAlignment"" Value=""Left""/>
        <Setter Property=""VerticalContentAlignment"" Value=""Top""/>
        <Setter Property=""Background"" Value=""Transparent""/>
        <Setter Property=""BorderThickness"" Value=""1""/>
        <Setter Property=""Cursor"" Value=""Arrow""/>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""controls:TreeViewItem"">
                    <Grid Background=""{TemplateBinding Background}"">
                        <vsm:VisualStateManager.VisualStateGroups>
                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                <vsm:VisualState x:Name=""Normal""/>
                                <vsm:VisualState x:Name=""Disabled"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""Header"" Storyboard.TargetProperty=""Foreground"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"">
                                                <DiscreteObjectKeyFrame.Value>
                                                    <SolidColorBrush Color=""#FF999999""/>
                                                </DiscreteObjectKeyFrame.Value>
                                            </DiscreteObjectKeyFrame>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""SelectionStates"">
                                <vsm:VisualStateGroup.Transitions>
                                    <vsm:VisualTransition From=""Unselected"" GeneratedDuration=""00:00:00.3000000"" To=""Selected""/>
                                    <vsm:VisualTransition From=""Selected"" GeneratedDuration=""00:00:00.5000000"" To=""Unselected""/>
                                </vsm:VisualStateGroup.Transitions>
                                <vsm:VisualState x:Name=""Unselected""/>
                                <vsm:VisualState x:Name=""Selected"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""select"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.75""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                                <vsm:VisualState x:Name=""SelectedInactive"">
                                    <Storyboard>
                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""inactive"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                            <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.4""/>
                                        </DoubleAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""HasItemsStates"">
                                <vsm:VisualState x:Name=""HasItems""/>
                                <vsm:VisualState x:Name=""NoItems"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ExpanderButton"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Collapsed""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                            <vsm:VisualStateGroup x:Name=""ExpansionStates"">
                                <vsm:VisualState x:Name=""Collapsed""/>
                                <vsm:VisualState x:Name=""Expanded"">
                                    <Storyboard>
                                        <ObjectAnimationUsingKeyFrames Duration=""0"" Storyboard.TargetName=""ItemsHost"" Storyboard.TargetProperty=""Visibility"">
                                            <DiscreteObjectKeyFrame KeyTime=""0"" Value=""Visible""/>
                                        </ObjectAnimationUsingKeyFrames>
                                    </Storyboard>
                                </vsm:VisualState>
                            </vsm:VisualStateGroup>
                        </vsm:VisualStateManager.VisualStateGroups>
                        <Grid.RowDefinitions>
                            <RowDefinition Height=""Auto""/>
                            <RowDefinition Height=""*""/>
                        </Grid.RowDefinitions>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""15""/>
                            <ColumnDefinition Width=""Auto""/>
                            <ColumnDefinition Width=""*""/>
                        </Grid.ColumnDefinitions>
                        <ToggleButton HorizontalAlignment=""Stretch"" x:Name=""ExpanderButton"" VerticalAlignment=""Stretch"">
                            <ToggleButton.Template>
                                <ControlTemplate TargetType=""ToggleButton"">
                                    <Grid x:Name=""Root"" Background=""Transparent"">
                                        <vsm:VisualStateManager.VisualStateGroups>
                                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                <vsm:VisualState x:Name=""Normal""/>
                                                <vsm:VisualState x:Name=""MouseOver"">
                                                    <Storyboard>
                                                        <ColorAnimation Duration=""0"" Storyboard.TargetName=""UncheckedBrush"" Storyboard.TargetProperty=""Color"" To=""{StaticResource NormalBrushGradient1}""/>
                                                    </Storyboard>
                                                </vsm:VisualState>
                                                <vsm:VisualState x:Name=""Disabled"">
                                                    <Storyboard>
                                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""Root"" Storyboard.TargetProperty=""Opacity"" To="".7""/>
                                                    </Storyboard>
                                                </vsm:VisualState>
                                            </vsm:VisualStateGroup>
                                            <vsm:VisualStateGroup x:Name=""CheckStates"">
                                                <vsm:VisualState x:Name=""Unchecked""/>
                                                <vsm:VisualState x:Name=""Checked"">
                                                    <Storyboard>
                                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""UncheckedVisual"" Storyboard.TargetProperty=""Opacity"" To=""0""/>
                                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""CheckedVisual"" Storyboard.TargetProperty=""Opacity"" To=""1""/>
                                                    </Storyboard>
                                                </vsm:VisualState>
                                            </vsm:VisualStateGroup>
                                        </vsm:VisualStateManager.VisualStateGroups>
                                        <Grid HorizontalAlignment=""Right"" Margin=""2 2 5 2"">
                                            <Path Height=""9"" HorizontalAlignment=""Right"" x:Name=""UncheckedVisual"" VerticalAlignment=""Center"" Width=""6"" Fill=""#FF000000"" StrokeLineJoin=""Miter"" StrokeThickness=""1"" Data=""M 0,0 L 0,9 L 5,4.5 Z"">
                                                <Path.Stroke>
                                                    <SolidColorBrush Color=""#FF989898"" x:Name=""UncheckedBrush""/>
                                                </Path.Stroke>
                                            </Path>
                                            <Path Height=""6"" HorizontalAlignment=""Center"" x:Name=""CheckedVisual"" VerticalAlignment=""Center"" Width=""6"" Opacity=""0"" Fill=""#FF262626"" StrokeLineJoin=""Miter"" Data=""M 6,0 L 6,6 L 0,6 Z""/>
                                        </Grid>
                                    </Grid>
                                </ControlTemplate>
                            </ToggleButton.Template>
                        </ToggleButton>
                        <Rectangle x:Name=""select"" IsHitTestVisible=""False"" Opacity=""0"" Grid.Column=""1"" Fill=""{StaticResource NormalBrush}"" Stroke=""{StaticResource NormalBorderBrush}"" StrokeThickness=""2"" RadiusX=""2"" RadiusY=""2""/>
                        <Rectangle x:Name=""inactive"" IsHitTestVisible=""False"" Opacity=""0"" Grid.Column=""1"" Fill=""{StaticResource NormalBrush}"" Stroke=""{StaticResource NormalBorderBrush}"" StrokeThickness=""1"" RadiusX=""2"" RadiusY=""2""/>
                        <Button Background=""{TemplateBinding Background}"" Foreground=""{TemplateBinding Foreground}"" Cursor=""{TemplateBinding Cursor}"" HorizontalAlignment=""{TemplateBinding HorizontalContentAlignment}"" x:Name=""Header"" VerticalAlignment=""{TemplateBinding VerticalContentAlignment}"" Grid.Column=""1"" Content=""{TemplateBinding Header}"" ContentTemplate=""{TemplateBinding HeaderTemplate}"" ClickMode=""Hover"">
                            <Button.Template>
                                <ControlTemplate TargetType=""Button"">
                                    <Grid Background=""{TemplateBinding Background}"">
                                        <vsm:VisualStateManager.VisualStateGroups>
                                            <vsm:VisualStateGroup x:Name=""CommonStates"">
                                                <vsm:VisualStateGroup.Transitions>
                                                    <vsm:VisualTransition From=""Normal"" GeneratedDuration=""00:00:00.3000000"" To=""Pressed""/>
                                                    <vsm:VisualTransition From=""Pressed"" GeneratedDuration=""00:00:00.5000000"" To=""Normal""/>
                                                </vsm:VisualStateGroup.Transitions>
                                                <vsm:VisualState x:Name=""Normal""/>
                                                <vsm:VisualState x:Name=""Pressed"">
                                                    <Storyboard>
                                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""hover"" Storyboard.TargetProperty=""Opacity"" To="".5""/>
                                                        <DoubleAnimationUsingKeyFrames BeginTime=""00:00:00"" Duration=""00:00:00.0010000"" Storyboard.TargetName=""Background"" Storyboard.TargetProperty=""(UIElement.Opacity)"">
                                                            <SplineDoubleKeyFrame KeyTime=""00:00:00"" Value=""0.5""/>
                                                        </DoubleAnimationUsingKeyFrames>
                                                    </Storyboard>
                                                </vsm:VisualState>
                                                <vsm:VisualState x:Name=""Disabled"">
                                                    <Storyboard>
                                                        <DoubleAnimation Duration=""0"" Storyboard.TargetName=""content"" Storyboard.TargetProperty=""Opacity"" To="".55""/>
                                                    </Storyboard>
                                                </vsm:VisualState>
                                            </vsm:VisualStateGroup>
                                        </vsm:VisualStateManager.VisualStateGroups>
                                        <Rectangle x:Name=""Background"" IsHitTestVisible=""False"" Opacity=""0"" Fill=""{StaticResource NormalBrush}"" Stroke=""{x:Null}"" StrokeThickness=""1"" RadiusX=""2"" RadiusY=""2""/>
                                        <Rectangle x:Name=""hover"" IsHitTestVisible=""False"" Opacity=""0"" Fill=""{StaticResource HoverBrush}"" Stroke=""{x:Null}"" StrokeThickness=""1"" RadiusX=""2"" RadiusY=""2""/>
                                        <ContentPresenter Cursor=""{TemplateBinding Cursor}"" HorizontalAlignment=""Left"" Margin=""{TemplateBinding Padding}"" x:Name=""content"" Content=""{TemplateBinding Content}"" ContentTemplate=""{TemplateBinding ContentTemplate}""/>
                                    </Grid>
                                </ControlTemplate>
                            </Button.Template>
                        </Button>
                        <ItemsPresenter x:Name=""ItemsHost"" Visibility=""Collapsed"" Grid.Column=""1"" Grid.ColumnSpan=""2"" Grid.Row=""1""/>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

    <!-- Tree View -->
    <Style TargetType=""controls:TreeView"">
        <Setter Property=""IsTabStop"" Value=""True""/>
        <Setter Property=""Background"" Value=""{StaticResource ShadeBrush}""/>
        <Setter Property=""Foreground"" Value=""#FF000000""/>
        <Setter Property=""HorizontalContentAlignment"" Value=""Left""/>
        <Setter Property=""VerticalContentAlignment"" Value=""Top""/>
        <Setter Property=""Cursor"" Value=""Arrow""/>
        <Setter Property=""BorderThickness"" Value=""1""/>
        <Setter Property=""Padding"" Value=""1""/>
        <Setter Property=""BorderBrush"">
            <Setter.Value>
                <LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
                    <GradientStop Color=""#FFA3AEB9"" Offset=""0""/>
                    <GradientStop Color=""#FF8399A9"" Offset=""0.375""/>
                    <GradientStop Color=""#FF718597"" Offset=""0.375""/>
                    <GradientStop Color=""#FF617584"" Offset=""1""/>
                </LinearGradientBrush>
            </Setter.Value>
        </Setter>
        <Setter Property=""Template"">
            <Setter.Value>
                <ControlTemplate TargetType=""controls:TreeView"">
                    <Grid x:Name=""Root"">
                        <Grid.Resources>
                            <SolidColorBrush x:Key=""BorderBrush"" Color=""#FF000000""/>
                        </Grid.Resources>
                        <Border x:Name=""Border"" BorderBrush=""{StaticResource NormalBorderBrush}"" BorderThickness=""{TemplateBinding BorderThickness}"" CornerRadius=""2"" Background=""{TemplateBinding Background}"">
                            <ScrollViewer BorderBrush=""Transparent"" BorderThickness=""0"" Padding=""{TemplateBinding Padding}"" Margin=""1"" x:Name=""ScrollViewer"" HorizontalScrollBarVisibility=""Auto"" VerticalScrollBarVisibility=""Auto"" Style=""{StaticResource System.Windows.Controls.ScrollViewer}"">
                                <ItemsPresenter Margin=""5"" x:Name=""TreeItems""/>
                            </ScrollViewer>
                        </Border>
                    </Grid>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>

</ResourceDictionary>");
            return sb.ToString();
        }
    }
}
