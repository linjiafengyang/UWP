﻿#pragma checksum "C:\Users\linji\Desktop\现代操作系统应用开发\homework\15331046_陈志扬_HW2\HW2_code\Todos\Todos\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "315CE37CB979B25BCB118E1253DD90B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Todos
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.DeleteAppBarButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 12 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.DeleteAppBarButton).Click += this.AddAppBarButton_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.CheckBox1 = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    #line 61 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.CheckBox1).Click += this.CheckBoxClick;
                    #line default
                }
                break;
            case 4:
                {
                    this.task1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.Line1 = (global::Windows.UI.Xaml.Shapes.Line)(target);
                }
                break;
            case 6:
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element6 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 72 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element6).Click += this.Edit1;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element7 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 73 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element7).Click += this.Delete1;
                    #line default
                }
                break;
            case 8:
                {
                    this.CheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    #line 35 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.CheckBox).Click += this.CheckBoxClick;
                    #line default
                }
                break;
            case 9:
                {
                    this.task = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.Line = (global::Windows.UI.Xaml.Shapes.Line)(target);
                }
                break;
            case 11:
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element11 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 46 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element11).Click += this.Edit;
                    #line default
                }
                break;
            case 12:
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element12 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 47 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element12).Click += this.Delete;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

