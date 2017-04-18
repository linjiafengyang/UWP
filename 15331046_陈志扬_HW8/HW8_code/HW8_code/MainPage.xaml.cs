using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW8_code
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public bool ismusic = true;// 表示是音频文件
        public bool isplay = false;// 表示音频文件是否播放
        public bool ispause = false;// 表示音频文件是否暂停
        public bool isstop = false;// 表示音频文件是否停止
        private void playClicked(object sender, RoutedEventArgs e)
        {
            media.Play();
            isplay = true;
            // 音频文件启动动画效果
            if (ismusic)
            {
                // 音频文件的状态是从暂停转到播放的，恢复图片动画
                if (ispause)
                {
                    // 处理：点击停止再点击暂停再点击播放动画不选转问题
                    // 因为停止时Resume还是停在那里不动的
                    if (isstop)
                    {
                        storyboard.Begin();
                        isstop = false;
                    }
                    else
                        storyboard.Resume();
                }
                // 是刚播放的，启动动画效果
                else
                {
                    storyboard.Begin();
                }
                storyboard1.Begin();
            }
            else
            {
                storyboard.Stop();
                storyboard1.Stop();
            }
        }
        // 点击暂停，暂停播放，暂停动画
        private void pauseClicked(object sender, RoutedEventArgs e)
        {
            media.Pause();
            // 处理：刚运行时不点播放然而先点暂停按钮的bug
            if (isplay)
            {
                if (ismusic)
                {
                    ispause = true;
                    storyboard.Pause();
                    storyboard1.Stop();
                }
            }

        }
        // 点击停止，停止播放，停止动画
        private void stopClicked(object sender, RoutedEventArgs e)
        {
            media.Stop();
            // 处理：刚运行时不点播放然而先点停止按钮的bug
            if (isplay)
            {
                if (ismusic)
                {
                    isstop = true;
                    storyboard.Stop();
                    storyboard1.Stop();
                }
            }
        }
        // 选取本地文件播放
        private async void selectFileClicked(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            // 可选的文件格式
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wma");
            picker.FileTypeFilter.Add(".m4a");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mkv");
            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".avi");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                // 自动播放
                media.SetSource(fileStream, file.FileType);
                media.AutoPlay = true;
                // 判断选择的文件是否是音频文件
                // 是，启动动画效果
                if (isMusic(file.FileType))
                {
                    ismusic = true;
                    isplay = true;
                    storyboard.Begin();
                    storyboard1.Begin();
                }
                else
                {
                    // 从音频文件切换到视频文件也要停止动画
                    ismusic = false;
                    storyboard.Stop();
                    storyboard1.Stop();
                }
            }
        }
        // 判断文件类型，mp3/m4a/wma为音频文件
        private bool isMusic(string fileType)
        {
            return fileType == ".mp3" || fileType == ".m4a" || fileType == ".wma";
        }
        // 当前窗口大小发生变化注册一个事件，主要来处理退出全屏底部CommandBar的可见性问题
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SizeChanged += windowSizeChanged;
        }
        private void windowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // 获取应用程序是否在全屏下运行
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isfull = view.IsFullScreenMode;
            if (!isfull)
            {
                commandBar.Opacity = 100;// 否，显示底部CommandBar,同时显示进度条和时间长度（绑定）
                fullscreen.Label = "全屏";
            }
        }
        // 点击进入全屏模式
        private void fullScreenClicked(object sender, RoutedEventArgs e)
        {
            
            // 获取应用程序是否在全屏下运行
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isfull = view.IsFullScreenMode;
            if (isfull)
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();// 进入全屏
            }
            else
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();// 进入全屏
            }
        }

        // 点击Volumn按钮，显示Slider
        private void showVolumnClicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null) FlyoutBase.ShowAttachedFlyout(element);
        }

        private void mediaOpened(object sender, RoutedEventArgs e)
        {
            // 获取当前打开的媒体文件的持续时间（以总秒数来算）
            speed.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            // 以 时:分:秒 的格式显示媒体文件的总时间
            mediaTimeSpan.Text = ((int)speed.Maximum / 3600).ToString() + ":"
                + ((int)speed.Maximum % 3600 / 60).ToString() + ":"
                + (speed.Maximum % 60).ToString().Substring(0, 2);

            speed.Value = 0;
        }
        private void mediaEnded(object sender, RoutedEventArgs e)
        {
            // 媒体文件播放结束后设置继续播放该文件，即循环播放
            media.Play();
            // 动画先停止，再重新开始旋转
            if (ismusic)
            {
                storyboard.Stop();
                storyboard.Begin();
                storyboard1.Stop();
                storyboard1.Begin();
            }
        }
        private void mediaFailed(object sender, RoutedEventArgs e)
        {
            var i = new MessageDialog("无法播放该文件！").ShowAsync();
        }
        // 处理鼠标移到底部时的可见性
        private void commandBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // 全屏状态下移动鼠标到底部CommandBar可显示出来，
            // 同时进度条和时间长度也显示出来（绑定）
            commandBar.Opacity = 100;

        }
        // 处理鼠标移出底部时的可见性
        private void commandBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // 获取应用程序是否在全屏下运行
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isfull = view.IsFullScreenMode;
            // 全屏状态下移动鼠标离开CommandBar可隐藏出来
            // 同时进度条和时间长度也隐藏起来（绑定）
            if (isfull)
            {
                commandBar.Opacity = 0;
                fullscreen.Label = "退出全屏";
            }
        }
    }
}
