using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Cashbook
{
    // SplitView点击顶部展开Pane
    class NullableBooleanToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (Boolean)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Boolean)value == true)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
    // 点击返回按钮ListView与返回到的界面相对应
    class FrameToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
            {
                return typeof(ContactInfo);
            }
            else if ((int)value == 1)
            {
                return typeof(cashbook);
            }
            else if ((int)value == 2)
            {
                return typeof(weather);
            }
            else if ((int)value == 3)
            {
                return typeof(phonenumber);
            }
            else if ((int)value == 4)
            {
                return typeof(mediaplayer);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Type)value == typeof(ContactInfo))
            {
                return 0;
            }
            else if ((Type)value == typeof(cashbook))
            {
                return 1;
            }
            else if ((Type)value == typeof(weather))
            {
                return 2;
            }
            else if ((Type)value == typeof(phonenumber))
            {
                return 3;
            }
            else if ((Type)value == typeof(mediaplayer))
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }
    }
    // 播放器相关转换
    class MediaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }

    class DisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).ToString(@"hh\:mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class AudioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)value * 100;
        }
    }
}
