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
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Todos
{
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            
        }

        private void updateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(File.ReadAllText("tile.xml"));
            foreach (var todo in ViewModel.AllItems)
            {
                XmlNodeList text = xml.GetElementsByTagName("text");
                for (int i = 0; i < text.Count; i++)
                {
                    text[i].InnerText = todo.title;
                    //i++;
                    text[i].InnerText = todo.description;
                }
                TileNotification notification = new TileNotification(xml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
        }

        private ViewModels.TodoItemViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //var i = new MessageDialog("Welcome!").ShowAsync();
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

            ViewModel = ((ViewModels.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                CreateButton.Content = "Create";
            }
            else
            {
                Title.Text = ViewModel.SelectedItem.title;
                Details.Text = ViewModel.SelectedItem.description;
                DatePicker.Date = ViewModel.SelectedItem.day;
                CreateButton.Content = "Update";
                // ...
            }
        }

        private async void SelectPicture(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.DecodePixelWidth = 350;
                bitmapImage.DecodePixelHeight = 180;
                await bitmapImage.SetSourceAsync(fileStream);
                background.Source = bitmapImage;
            }
        }
        private void CancelButton_Clicked(object sender, RoutedEventArgs e)
        {
            Title.Text = "";
            Details.Text = "";
            DatePicker.Date = DateTime.Now.Date;
            CreateButton.Content = "Create";
            ViewModel.SelectedItem = null;// 这非常重要，如果没有，会出现只是修改成Create，其实执行的函数还是update
        }
        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
            // check the textbox and datapicker
            // if ok
            // 判断是否选择了TodoItem
            // 没有选择的话进行Create
            if (ViewModel.SelectedItem == null)
            {
                // 同样信息不能为空或者有误
                if (Title.Text == "")
                {
                    var i = new MessageDialog("The title can't be empty!").ShowAsync();
                }
                else if (Details.Text == "")
                {
                    var i = new MessageDialog("The details can't be empty!").ShowAsync();
                }
                else if (DatePicker.Date < DateTime.Now.Date)
                {
                    var i = new MessageDialog("The date before current date is wrong!").ShowAsync();
                }
                else
                {
                    ViewModel.AddTodoItem(Title.Text, Details.Text, DatePicker.Date);// 调用TodoItemViewModel中的AddTodoItem方法
                    Frame.Navigate(typeof(MainPage), ViewModel);// 跳转回MainPage
                }
            }
            // 选择了的话进入Update
            else
            {
                UpdateButton_Clicked(sender, e);
            }
            updateTile();
        }
        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem();
                updateTile();
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                // check then update
                // 同样信息不能为空或者有误
                if (Title.Text == "")
                {
                    var i = new MessageDialog("The title can't be empty!").ShowAsync();
                }
                else if (Details.Text == "")
                {
                    var i = new MessageDialog("The details can't be empty!").ShowAsync();
                }
                else if (DatePicker.Date < DateTime.Now.Date)
                {
                    var i = new MessageDialog("The date before current date is wrong!").ShowAsync();
                }
                // 若全部没修改，则不能update
                else if (Title.Text == ViewModel.SelectedItem.title && Details.Text == ViewModel.SelectedItem.description && DatePicker.Date == ViewModel.SelectedItem.day)
                {
                    var i = new MessageDialog("There is no change! Please update one of them!").ShowAsync();
                }
                else
                {
                    ViewModel.UpdateTodoItem(Title.Text, Details.Text, DatePicker.Date);// 调用TodoItemViewModel中的UpdateTodoItem方法
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }
    }
}
