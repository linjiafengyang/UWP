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
using Windows.UI.Popups;
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
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
        }
        // 点击list，即点击TodoItem
        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            // 当处于窄屏时，跳转到NewPage
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                Title.Text = ViewModel.SelectedItem.title;
                Details.Text = ViewModel.SelectedItem.description;
                DatePicker.Date = ViewModel.SelectedItem.day;// 加上日期
                CreateButton.Content = "Update";// ，因为是点击TodoItem 所以对应update
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // 只能在窄屏下点击+号可以实现跳转
            if (Window.Current.Bounds.Width <= 800)
                Frame.Navigate(typeof(NewPage), ViewModel);
        }

        private void CancelButton_Clicked(object sendet, RoutedEventArgs e)
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
                    // Create成功后重置以下信息
                    Title.Text = "";
                    Details.Text = "";
                    DatePicker.Date = DateTime.Now.Date;
                    CreateButton.Content = "Create";
                    //ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
            // 选择了的话进入Update
            else
            {
                UpdateButton_Clicked(sender, e);
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
                    Title.Text = "";
                    Details.Text = "";
                    DatePicker.Date = DateTime.Now.Date;
                    CreateButton.Content = "Create";
                    //ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }

    }
}
