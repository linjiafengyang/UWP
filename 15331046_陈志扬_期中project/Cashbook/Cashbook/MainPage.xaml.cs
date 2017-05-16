using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Cashbook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.PersonalInfoViewModel();
            this.Model = new Models.PersonalInfo(Guid.NewGuid().ToString(), "", "", "", "ms-appx:///Assets/background.jpg");
        }
        ViewModels.PersonalInfoViewModel ViewModel { get; set; }
        Models.PersonalInfo Model { get; set; }
        string mail;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
        }
        // 点击下方登录或者注册按钮，可登录或注册
        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            // 注册，比较注册的邮箱是否已存在
            // 若不存在，则注册成功，数据库添加数据
            if (Sign.Content.ToString() == "注册")
            {
                if (check_Mail.Text == "" && check_Password.Text == "" && check_Nickname.Text == "" && Nickname.Text != "" && Mail.Text != "" && Password.Password != "")
                {
                    if (ViewModel.compareMail(Mail.Text))
                    {
                        Model.addPersonalInfo(Guid.NewGuid().ToString(), Nickname.Text, Mail.Text, Password.Password, "ms-appx:///Assets/background.jpg");
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(Information), Model);
                    }
                    else
                    {
                        var i = new MessageDialog("该邮箱已注册！请选择登录！").ShowAsync();
                    }
                }
                else
                {
                    var i = new MessageDialog("格式错误！请重新注册！").ShowAsync();
                }
            }
            // 登录，比较登录的邮箱和密码是否匹配
            else if (Sign.Content.ToString() == "登录")
            {
                if (check_Mail.Text == "" && check_Password.Text == "" && ViewModel.comparePassword(Mail.Text, Password.Password))
                {
                    //Model.nickname = Nickname.Text;
                    Model.mail = Mail.Text;
                    mail = Mail.Text;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(Information), mail);
                }
                else
                {
                    var i = new MessageDialog("账号或密码错误！请重新登录！").ShowAsync();
                }
            }

        }
        // 点击上方注册按钮，控制显示界面
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Nickname.Text = "";
            Mail.Text = "";
            Password.Password = "";
            check_Nickname.Text = "";
            check_Mail.Text = "";
            check_Password.Text = "";
            Nickname.Visibility = Visibility.Visible;
            check_Nickname.Visibility = Visibility.Visible;
            Grid.SetRow(Mail, 5);
            Grid.SetRow(check_Mail, 6);
            Grid.SetRow(Password, 7);
            Grid.SetRow(check_Password, 8);
            Grid.SetRow(Sign, 9);
            SignUp.BorderThickness = new Thickness(0, 0, 0, 2);
            SignIn.BorderThickness = new Thickness(0);
            SignUp.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
            SignIn.Foreground = new SolidColorBrush(Colors.Gray);
            Sign.Content = "注册";
            Mail.PlaceholderText = "邮箱格式:xx@xx.com";
            Password.PlaceholderText = "密码至少为6位！";
        }
        // 点击上方登录按钮，控制显示界面
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Nickname.Text = "";
            Mail.Text = "";
            Password.Password = "";
            check_Nickname.Text = "";
            check_Mail.Text = "";
            check_Password.Text = "";
            Nickname.Visibility = Visibility.Collapsed;
            check_Nickname.Visibility = Visibility.Collapsed;
            Grid.SetRow(Mail, 3);
            Grid.SetRow(check_Mail, 4);
            Grid.SetRow(Password, 5);
            Grid.SetRow(check_Password, 6);
            Grid.SetRow(Sign, 7);
            SignIn.BorderThickness = new Thickness(0, 0, 0, 2);
            SignUp.BorderThickness = new Thickness(0);
            SignUp.Foreground = new SolidColorBrush(Colors.Gray);
            SignIn.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
            Sign.Content = "登录";
            Mail.PlaceholderText = "邮箱";
            Password.PlaceholderText = "密码";
        }
        // 判断邮箱格式xx@xx.com
        private void Mail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = @".+@.+\.com";
            if (Regex.IsMatch(Mail.Text, s) || Mail.Text == "")
            {
                check_Mail.Text = "";
            } else
            {
                check_Mail.Text = "格式错误！";
            }
        }
        // 判断密码位数
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string s = @".{6,12}";
            if (Regex.IsMatch(Password.Password, s) || Password.Password == "")
            {
                check_Password.Text = "";
            }
            else
            {
                check_Password.Text = "密码至少为6位！";
            }
        }
        // 判断昵称
        private void Nickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Nickname.Text == "")
            {
                check_Nickname.Text = "昵称不能为空！";
            } else
            {
                check_Nickname.Text = "";
            }
        }
    }
}
