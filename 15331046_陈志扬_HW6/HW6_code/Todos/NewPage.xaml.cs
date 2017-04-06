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
using SQLitePCL;

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
            DatePicker.Date = DateTimeOffset.Now;
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
                    // 将新Item插入到数据库中
                    insertTodo(Title.Text, Details.Text, DatePicker.Date.ToString());
                    Frame.Navigate(typeof(MainPage), ViewModel);// 跳转回MainPage
                }
            }
            // 选择了的话进入Update
            else
            {
                UpdateButton_Clicked(sender, e);
            }
        }

        private void insertTodo(string title, string description, string time)
        {
            using (var db = new SQLiteConnection("Todos.db"))
            {
                using (var temp = db.Prepare("INSERT INTO Todo(Title, Description, Time, Picture) VALUES(?, ?, ?, ?)"))
                {
                    temp.Bind(1, title);
                    temp.Bind(2, description);
                    temp.Bind(3, time);
                    temp.Bind(4, DatePicker.Date.ToString("d") + ".jpg");// 将图片根据时间保存下来
                    temp.Step();
                }
            }
        }

        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            // 删除数据库对应的Item
            deleteTodo(ViewModel.SelectedItem.title, ViewModel.SelectedItem.description);
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem();
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
        // 删除数据库对应的Item
        private void deleteTodo(string title, string description)
        {
            using (var db = new SQLiteConnection("Todos.db"))
            {
                using (var statement = db.Prepare("DELETE FROM Todo WHERE Title = ? AND Description = ?"))
                {
                    statement.Bind(1, title);
                    statement.Bind(2, description);
                    statement.Step();
                }
            }
        }

        private void UpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            var tempTitle = ViewModel.SelectedItem.title;
            var tempDetails = ViewModel.SelectedItem.description;
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
                    // 更新数据库对应的Item
                    updateTodo(tempTitle, tempDetails);
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }
        // 更新数据库对应的Item
        private void updateTodo(string title, string description)
        {
            using (var db = new SQLiteConnection("Todos.db"))
            {
                using (var temp = db.Prepare("UPDATE Todo SET Title = ?, Description = ?, Time = ?, Picture = ? WHERE Title = ? AND Description = ?"))
                {
                    temp.Bind(1, Title.Text);
                    temp.Bind(2, Details.Text);
                    // 时间选取器上的日期+当前时间(时分秒)
                    temp.Bind(3, DatePicker.Date.ToString("d") + " " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + " +08:00");
                    temp.Bind(4, DatePicker.Date.ToString("d") + ".jpg");
                    temp.Bind(5, title);
                    temp.Bind(6, description);
                    temp.Step();
                }
            }
        }

    }
}
