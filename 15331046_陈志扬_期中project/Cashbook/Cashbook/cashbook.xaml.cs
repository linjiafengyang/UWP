using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
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
    public sealed partial class cashbook : Page
    {
        public cashbook()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.DocumentViewModel();
            payout.Text = ViewModel.payout.ToString();
            income.Text = ViewModel.income.ToString();
            result.Text = ViewModel.result.ToString();
            updateTile();
        }
        ViewModels.DocumentViewModel ViewModel { get; set; }

        private void updateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(File.ReadAllText("tile.xml"));
            foreach (var document in ViewModel.AllDocuments)
            {
                XmlNodeList text = xml.GetElementsByTagName("text");
                for (int i = 0; i < text.Count; i++)
                {
                    text[i].InnerText = document.remarks;
                    i++;
                    text[i].InnerText = document.output_amountOfMoney;
                }
                TileNotification notification = new TileNotification(xml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
        }

        // 从数据库中获取每条消费收入记录，并计算出支出、收入、结余显示在页面上
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(ViewModels.DocumentViewModel))
                {
                    this.ViewModel = (ViewModels.DocumentViewModel)(e.Parameter);
                }
            }
            payout.Text = ViewModel.payout.ToString();
            income.Text = ViewModel.income.ToString();
            result.Text = ViewModel.result.ToString();
        }
        // 点击一个记录跳转到详细信息页面
        private void CashbookListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedDocument = (Models.Document)(e.ClickedItem);
            Frame.Navigate(typeof(cashbookInfo), ViewModel);
        }
        // 点击底部“记一笔”按钮，跳转到新建页面
        private void create_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(cashbookNew), ViewModel);
        }
    }
}
