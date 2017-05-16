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
    public sealed partial class cashbookInfo : Page
    {
        private ViewModels.DocumentViewModel ViewModel;
        Button last;// 用来记录用户最近一次点击的消费类型
        string updateConsumption;

        public cashbookInfo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.DocumentViewModel)(e.Parameter);
            // 跳转到详细页面时显示记录的信息：金额、消费类型（以选中的颜色显示）、时间、备注
            if (ViewModel.SelectedDocument != null)
            {
                money.Text = ViewModel.SelectedDocument.amountOfMoney.ToString();
                datepicker.Date = ViewModel.SelectedDocument.day;
                button_changeColor(ViewModel.SelectedDocument.consumptionType);
                info.Text = ViewModel.SelectedDocument.remarks;
            }
        }
        // 根据记录的消费类型来设置每个按钮的颜色
        private void button_changeColor(string consumptionType)
        {
            if (consumptionType == "日常")
            {
                payout.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "日常";
                last = payout;
            }
            if (consumptionType == "收入")
            {
                income.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "收入";
                last = income;
            }
            if (consumptionType == "交通")
            {
                traffic.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "交通";
                last = traffic;
            }
            if (consumptionType == "餐饮")
            {
                catering.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "餐饮";
                last = catering;
            }
            if (consumptionType == "购物")
            {
                shopping.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "购物";
                last = shopping;
            }
            if (consumptionType == "娱乐")
            {
                entertainment.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "娱乐";
                last = entertainment;
            }
            if (consumptionType == "旅游")
            {
                travel.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "旅游";
                last = travel;
            }
            if (consumptionType == "学习")
            {
                study.Background = new SolidColorBrush(Colors.LightGray);
                updateConsumption = "学习";
                last = study;
            }
        }
        // 点击删除按钮，删除该记录，跳转回记录页面
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDocument != null)
            {
                ViewModel.RemoveDocument();
                Frame.Navigate(typeof(cashbook), ViewModel);
            }
        }
        // 点击完成按钮，更新该记录（若无备注，备注显示为消费类型），跳转回记录页面
        private void complete_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDocument != null)
            {
                string s = "";
                if (info.Text == "") s = updateConsumption;
                else s = info.Text;
                ViewModel.UpdateDocument(updateConsumption, Double.Parse(money.Text), s, datepicker.Date);
                Frame.Navigate(typeof(cashbook), ViewModel);
            }
        }
        // 接下来八个Click事件都是类似的
        // 在该页面下，用户随机点击消费类型的每个按钮，记录下最近一次的点击，并更新消费类型和备注信息
        // 难点是处理点击按钮前后，上一次点击的按钮呈白色，新点击的按钮呈浅灰色
        private void payout_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "日常";
            info.Text = "日常";
            payout.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void income_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "收入";
            info.Text = "收入";
            income.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void traffic_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "交通";
            info.Text = "交通";
            traffic.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void catering_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "餐饮";
            info.Text = "餐饮";
            catering.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void shopping_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "购物";
            info.Text = "购物";
            shopping.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void entertainment_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "娱乐";
            info.Text = "娱乐";
            entertainment.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void travel_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "旅游";
            info.Text = "旅游";
            travel.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }

        private void study_Click(object sender, RoutedEventArgs e)
        {
            last.Background = new SolidColorBrush(Colors.White);
            updateConsumption = "学习";
            info.Text = "学习";
            study.Background = new SolidColorBrush(Colors.LightGray);
            last = (Button)e.OriginalSource;
        }
        // 根据TextChanged可以更改“完成”按钮的颜色，形成视觉效果，且点击无效
        private void money_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (money.Text == "") complete.IsEnabled = false;
            else complete.IsEnabled = true;
        }

    }
}
