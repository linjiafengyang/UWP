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
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }

        private void CheckBoxClicked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                line.Opacity = 100;
            }
            else
            {
                line.Opacity = 0;
            }
            if (checkBox1.IsChecked == true)
            {
                line1.Opacity = 100;
            }
            else
            {
                line1.Opacity = 0;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).IsSuspending;
            if (suspending)
            {
                // Save volatile state in case we get terminated later on
                // then we can restore as if we'd never been gone
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["checkBox"] = checkBox.IsChecked;
                composite["line"] = line.Opacity;
                composite["checkBox1"] = checkBox1.IsChecked;
                composite["line1"] = line1.Opacity;
                ApplicationData.Current.LocalSettings.Values["newpage"] = composite;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                // If this is a new naigation, this is a fresh launch
                // so we can discard any saved state
                ApplicationData.Current.LocalSettings.Values.Remove("newpage");
            }
            else
            {
                // Try to restore state if any, in case we were terminated
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["newpage"] as ApplicationDataCompositeValue;
                    checkBox.IsChecked = (bool)composite["checkBox"];
                    line.Opacity = (double)composite["line"];
                    checkBox1.IsChecked = (bool)composite["checkBox1"];
                    line1.Opacity = (double)composite["line1"];
                    // We're done with it, so remove it
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage");
                }
            }
        }
    }
}
