using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Cashbook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ContactInfo : Page
    {
        public ContactInfo()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.PersonalInfoViewModel();
        }
        Models.PersonalInfo Model { get; set; }
        ViewModels.PersonalInfoViewModel ViewModel { get; set; }
        public string mail;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(string))
                {
                    mail = (string)(e.Parameter);
                    Model = ViewModel.queryMail(mail);
                    Mail.Text = Model.mail;
                    Nickname.Text = Model.nickname;
                    BitmapImage bi = new BitmapImage();
                    bi.UriSource = Model.imageUri;
                    Picture.ImageSource = bi;
                }

                if (e.Parameter.GetType() == typeof(Models.PersonalInfo))
                {
                    Model = (Models.PersonalInfo)(e.Parameter);
                    ViewModel.AddAccount(Model.nickname, Model.mail, Model.password, Model.imageUri.ToString());
                    Mail.Text = Model.mail;
                    Nickname.Text = Model.nickname;
                    BitmapImage bi = new BitmapImage();
                    bi.UriSource = Model.imageUri;
                    Picture.ImageSource = bi;
                }
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
            {
                var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
                Nickname.Text = (string)composite["nickname"];
                Mail.Text = (string)composite["mail"];
                string s = (string)composite["imageUri"];
                BitmapImage bi = new BitmapImage();
                bi.UriSource = new Uri(s);
                Picture.ImageSource = bi;
                // We're done with it, so remove it
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var composite = new ApplicationDataCompositeValue();
            composite["nickname"] = Nickname.Text;
            composite["mail"] = Mail.Text;
            composite["imageUri"] = Model.imageUri.ToString();
            //var i = new MessageDialog(Model.imageUri.ToString()).ShowAsync();
            ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
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
                Picture.ImageSource = bitmapImage;
            }
        }
        // 点击退出登录按钮回到主界面
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(MainPage), e);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Length < 6 || ChangePassword.Password.Length < 6)
            {
                var j = new MessageDialog("密码格式错误，请重新输入！").ShowAsync();
                return;
            }
            if (Password.Password != ChangePassword.Password)
            {
                var k = new MessageDialog("密码不对应，请重新输入密码！").ShowAsync();
            }
            else
            {
                ViewModel.updateAccount(Mail.Text, Nickname.Text, Password.Password);
                var i = new MessageDialog("修改密码成功！").ShowAsync();
            }
        }
    }
}
