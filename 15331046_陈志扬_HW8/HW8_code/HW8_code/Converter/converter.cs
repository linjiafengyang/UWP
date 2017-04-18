using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HW8_code.Converter
{
    class speedConverter : IValueConverter
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
    class volumeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // 注意这里是value转volume，是除以100
            return (double)value / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)value * 100;
        }
    }
}
