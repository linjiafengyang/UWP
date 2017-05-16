using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class cashbookNew : Page
    {
        public cashbookNew()
        {
            this.InitializeComponent();
            last = payout;
            payout.Background = new SolidColorBrush(Colors.LightGray);
            consumptionType = "日常";
        }

        ViewModels.DocumentViewModel ViewModel { get; set; }
        Button last;
        string consumptionType;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.DocumentViewModel)(e.Parameter);
        }
        // 点击完成按钮，新建一个记录，跳转回记录页面
        private void complete_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            if (info.Text == "") s = consumptionType;
            else s = info.Text;
            ViewModel.AddDocument(datepicker.Date, consumptionType, Double.Parse(money.Text), s);
            Frame.Navigate(typeof(cashbook), ViewModel);
        }
        // 接下来八个Click事件都是类似的
        // 在该页面下，用户随机点击消费类型的每个按钮，记录下最近一次的点击，并更新消费类型
        // 难点是处理点击按钮前后，上一次点击的按钮呈白色，新点击的按钮呈浅灰色
        private void payout_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "日常";
            payout.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void income_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "收入";
            income.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void traffic_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "交通";
            traffic.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void catering_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "餐饮";
            catering.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void shopping_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "购物";
            shopping.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void entertainment_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "娱乐";
            entertainment.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void travel_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "旅游";
            travel.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void study_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            consumptionType = "学习";
            study.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }
        // 根据TextChanged可以更改“完成”按钮的颜色，形成视觉效果
        private void money_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (money.Text == "") complete.IsEnabled = false;
            else complete.IsEnabled = true;
        }        
    }
}
