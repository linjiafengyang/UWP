using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Media.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Cashbook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class mediaplayer : Windows.UI.Xaml.Controls.Page
    {
        public mediaplayer()
        {
            this.InitializeComponent();
        }

        private int isPlay;
        private int isStart;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SizeChanged += windowSizeChanged;
        }

        private void windowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // 获取应用程序是否在全屏下运行
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isfull = view.IsFullScreenMode;
        }


        private void myMediaElement_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void myMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            isPlay = 0;
            isStart = 0;
            TimelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            IconElement icon = new SymbolIcon(Symbol.Pause);
            PlayButton.Icon = icon;

            if (myMediaElement.IsAudioOnly)
            {
                myMediaElement.Visibility = Visibility.Collapsed;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/cute.jpg", UriKind.Absolute));
                grid.Background = imageBrush;
            }
            else
            {
                myMediaElement.Visibility = Visibility.Visible;
                ImageBrush imageBrush = new ImageBrush();
                grid.Background = imageBrush;
            }

            if (isPlay == 0 && isStart == 0)
            {
                esb1.Begin();
                stick_play.Begin();
                isPlay = 1;
                isStart = 1;
            }
        }

        private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            esb1.Stop();
            if (isPlay == 1)
            {
                stick_play_pause.Begin();
                isPlay = 0;
                isStart = 0;
                IconElement icon = new SymbolIcon(Symbol.Play);
                PlayButton.Icon = icon;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlay == 1)
            {
                myMediaElement.Pause();

                esb1.Pause();
                stick_play_pause.Begin();

                IconElement icon = new SymbolIcon(Symbol.Play);
                PlayButton.Icon = icon;
                isPlay = 0;
            }
            else
            {
                myMediaElement.Play();
                if (isStart == 0)
                {
                    esb1.Begin();
                    isStart = 1;
                }
                else
                {
                    esb1.Resume();
                }
                stick_play.Begin();
                IconElement icon = new SymbolIcon(Symbol.Pause);
                PlayButton.Icon = icon;
                isPlay = 1;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            esb1.Stop();
            if (isPlay == 1)
            {
                stick_play_pause.Begin();
                isPlay = 0;
                isStart = 0;
                IconElement icon = new SymbolIcon(Symbol.Play);
                PlayButton.Icon = icon;
            }
        }

        private async void Pick_Click(object sender, RoutedEventArgs e)
        {
            MediaSource mediaSource;
            FileOpenPicker filePicker = new FileOpenPicker();

            filePicker.FileTypeFilter.Add(".wmv");
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".mkv");
            filePicker.FileTypeFilter.Add(".mp3");

            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                mediaSource = MediaSource.CreateFromStorageFile(file);
                myMediaElement.SetPlaybackSource(mediaSource);
            }

        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();

            bool isInFullScreenMode = view.IsFullScreenMode;

            if (isInFullScreenMode)
            {
                if (!myMediaElement.IsAudioOnly) Grid.SetRowSpan(myMediaElement, 1);
                view.ExitFullScreenMode();
                Information.current.splitview.CompactPaneLength = 48;
            }
            else
            {
                if (!myMediaElement.IsAudioOnly) Grid.SetRowSpan(myMediaElement, 2);
                view.TryEnterFullScreenMode();
                Information.current.splitview.CompactPaneLength = 0;
            }
        }


        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }

        private void TimelineSlider_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TimelineSlider.Opacity = 100;
        }

        private void TimelineSlider_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isfull = view.IsFullScreenMode;
            if (isfull)
                TimelineSlider.Opacity = 0;
            else
                TimelineSlider.Opacity = 100;
        }
    }
}
