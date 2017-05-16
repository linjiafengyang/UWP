using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

namespace Cashbook.Models
{
    class Document
    {
        public string id { get; set; }//记录的id，每条记录有一个独一无二的id
        public string consumptionType { get; set; }//消费类型
        public double amountOfMoney { get; set; }//消费金额
        public string remarks { get; set; }//记录备注
        public string output_amountOfMoney { get; set; }
        public System.DateTimeOffset day;//记录的日期
        public string dayOfMouth;// 记录当天是几号
        public string dayOfWeek;// 记录当天星期几

        //利用参数生成新的记录
        public Document(string id,string consumptionType,double amountOfMoney,string remarks,System.DateTimeOffset day)
        {
            this.id = id;
            this.consumptionType = consumptionType;
            // 显示金额（有正负之分）
            if (consumptionType == "收入")
            {
                output_amountOfMoney = "+" + amountOfMoney.ToString();
            } else
            {
                output_amountOfMoney = "-" + amountOfMoney.ToString();
            }
            this.amountOfMoney = amountOfMoney;
            this.remarks = remarks;
            this.day = day;
            this.dayOfMouth = day.Day.ToString();// 获取记录当天是几号
            this.dayOfWeek = day.DayOfWeek.ToString();// 获取记录当天星期几
        }
    }
}
