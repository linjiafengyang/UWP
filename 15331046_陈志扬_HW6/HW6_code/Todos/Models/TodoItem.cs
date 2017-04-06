using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.Models
{
    class TodoItem
    {
        private string id;
        public string title { get; set; }
        public string description { get; set; }
        //public BitmapImage image { get; set; }
        //public Uri imageUri { get; set; }
        public bool completed { get; set; }
        //日期字段自己写
        public string date { get; set; }
        // 日期
        public System.DateTimeOffset day;
        // 添加了形参 System.DateTimeOffset set_day，用来表示用户设置的时间
        public TodoItem(string title, string description, System.DateTimeOffset set_day)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.completed = false; //默认为未完成
            this.day = set_day;
            this.date = set_day.ToString();
            
        }
    }
    // 转换器，实现控制Line的Visibility
    class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool myValue = (bool)value;
            if (myValue)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
