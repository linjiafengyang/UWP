using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Cashbook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class phonenumber : Page
    {
        public phonenumber()
        {
            this.InitializeComponent();
        }
        private void phoneQuery(object sender, RoutedEventArgs e)
        {
            location.Text = "";
            runner.Text = "";
            queryAsyncJSON(number.Text);
        }
        async void queryAsyncJSON(string tel)
        {
            // 先判断手机号码是否有输入
            // 否，弹出错误信息
            if (tel == "")
            {
                var i = new MessageDialog("输入手机号码不能为空！").ShowAsync();
            }
            else
            {
                string url = "http://route.showapi.com/6-1?showapi_appid=35441&showapi_sign=4d6fec3edc1248cf865020a73400088a" + "&num=" + tel;
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(url);
                JObject jobject = (JObject)JsonConvert.DeserializeObject(result);
                // 查询成功，进行归属地、运营商的赋值
                if (jobject["showapi_res_body"]["ret_code"].ToString() == "0")
                {
                    location.Text = jobject["showapi_res_body"]["prov"].ToString() + jobject["showapi_res_body"]["city"].ToString();
                    runner.Text = "中国" + jobject["showapi_res_body"]["name"].ToString();
                }
                // 否，输出错误信息
                else
                {
                    var i = new MessageDialog("输入的手机号码信息有误！").ShowAsync();
                }
            }
        }
    }
}
