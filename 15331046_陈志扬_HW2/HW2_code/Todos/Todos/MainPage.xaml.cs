using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app backstack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(NewPage));
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true && task.Text != "")
            {
                Line.Visibility = Visibility.Visible;
            }
            else
            {
                Line.Visibility = Visibility.Collapsed;
            }
            if (CheckBox1.IsChecked == true && task1.Text != "")
            {
                Line1.Visibility = Visibility.Visible;
            }
            else
            {
                Line1.Visibility = Visibility.Collapsed;
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (task.Text == "") task.Text = "完成作业";
            Line.Visibility = Visibility.Collapsed;
            CheckBox.IsChecked = false;
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            task.Text = "";
            Line.Visibility = Visibility.Collapsed;
            CheckBox.IsChecked = false;
        }
        private void Edit1(object sender, RoutedEventArgs e)
        {
            if (task1.Text == "") task1.Text = "完成作业";
            Line1.Visibility = Visibility.Collapsed;
            CheckBox1.IsChecked = false;
        }
        private void Delete1(object sender, RoutedEventArgs e)
        {
            task1.Text = "";
            Line1.Visibility = Visibility.Collapsed;
            CheckBox1.IsChecked = false;
        }
    }
}
