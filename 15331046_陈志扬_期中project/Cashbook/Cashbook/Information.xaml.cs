using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Cashbook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Information : Page
    {
        public Information()
        {
            this.InitializeComponent();
            ViewModel = new ViewModels.PersonalInfoViewModel();
            current = this;
        }
        public static Information current;
        ViewModels.PersonalInfoViewModel ViewModel { get; set; }
        Models.PersonalInfo Model;
        public string mail;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(string))
            {
                mail = (string)(e.Parameter);
                frame.Navigate(typeof(ContactInfo), mail);
            }
            if (e.Parameter.GetType() == typeof(Models.PersonalInfo))
            {
                Model = (Models.PersonalInfo)(e.Parameter);
                frame.Navigate(typeof(ContactInfo), Model);
            }
        }
    }
}
