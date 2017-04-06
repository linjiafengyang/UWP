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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using SQLitePCL;
using Todos.Models;
using System.Text;


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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        // 点击Setting按钮中的Edit，宽屏下可以进行修改
        private void UpdateSetting_Clicked(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            var item = ToDoListView.ContainerFromItem(dataContext) as ListViewItem;
            ViewModel.SelectedItem = (Models.TodoItem)(item.Content);
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                Title.Text = ViewModel.SelectedItem.title;
                Details.Text = ViewModel.SelectedItem.description;
                DatePicker.Date = ViewModel.SelectedItem.day;// 加上日期
                CreateButton.Content = "Update";// 对应update
            }
        }
        // 点击Setting中的Delete，直接删除该Item
        private void DeleteSetting_Clicked(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            var item = ToDoListView.ContainerFromItem(dataContext) as ListViewItem;
            ViewModel.SelectedItem = (Models.TodoItem)(item.Content);
            // 删除数据库对应的Item
            deleteTodo(ViewModel.SelectedItem.title, ViewModel.SelectedItem.description);
            DeleteButton_Clicked(sender, e);
        }
        
        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem();
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
        // 点击Search按钮，进行数据库的查询
        private void SearchButton_Clicked(object sender, RoutedEventArgs e)
        {
            // GetTodo获取查询到的Item
            var todos = GetTodo(search.Text);
            // 构建StringBuilder类的一个实例message，用来显示查询到的信息
            StringBuilder message = new StringBuilder();
            if (todos != null)
            {
                if (todos.Count > 0)
                {
                    for (int i = 0; i < todos.Count; i++)
                    {
                        // 将信息追加到message中
                        message.AppendLine("Title: " + todos[i].title + " Description: " + todos[i].description + " Time: " + todos[i].day);
                    }
                    // 弹出MessageDialog，显示信息
                    var j = new MessageDialog(message.ToString()).ShowAsync();
                }
                else
                {
                    var j = new MessageDialog("There is no such Todo in the list.").ShowAsync();
                }
            }
            search.Text = "";
        }
        // 根据一个字符串模糊搜索匹配的Item并返回
        private List<TodoItem> GetTodo(string searchText)
        {
            List<TodoItem> todos = new List<TodoItem>();
            TodoItem todo = null;
            if (ViewModel.AllItems.Count > 0)
            {
                if (searchText != "")
                {
                    using (var db = new SQLiteConnection("Todos.db"))
                    {
                        using (var statement = db.Prepare("SELECT Title, Description, Time FROM Todo WHERE Title LIKE ?"))
                        {
                            statement.Bind(1, "%" + searchText + "%");
                            while (SQLiteResult.ROW == statement.Step())
                            {
                                todo = new TodoItem((string)statement[0], (string)statement[1], DateTimeOffset.Parse(statement[2].ToString()));
                                todos.Add(todo);// 将查询到的todo添加到todos结尾处
                            }
                        }
                    }
                }
            }
            return todos;
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
            DatePicker.Date = DateTimeOffset.Now;
            CreateButton.Content = "Create";
            ViewModel.SelectedItem = null;// 这非常重要，如果没有，会出现只是修改成Create，其实执行的函数还是update
        }
        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
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
                    // Create成功后重置以下信息
                    Title.Text = "";
                    Details.Text = "";
                    DatePicker.Date = DateTimeOffset.Now;
                    CreateButton.Content = "Create";
                    //ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
            else
            {
                UpdateButton_Clicked(sender, e);
            }
        }
        // 将新Item增加到数据库中
        private void insertTodo(string title, string description, string time)
        {
            using (var db = new SQLiteConnection("Todos.db"))
            {
                using (var temp = db.Prepare("INSERT INTO Todo(Title, Description, Time, Picture) VALUES(?, ?, ?, ?)"))
                {
                    temp.Bind(1, title);
                    temp.Bind(2, description);
                    temp.Bind(3, time);
                    temp.Bind(4, DatePicker.Date.ToString("d") + ".jpg");
                    temp.Step();
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
                    Title.Text = "";
                    Details.Text = "";
                    DatePicker.Date = DateTimeOffset.Now;
                    CreateButton.Content = "Create";
                    //ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }
        // 更新数据库中对应的Item
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
