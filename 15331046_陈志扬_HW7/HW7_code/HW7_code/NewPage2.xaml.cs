using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace HW7_code
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage2 : Page
    {
        public NewPage2()
        {
            this.InitializeComponent();
        }
        private void weatherQuery(object sender, RoutedEventArgs e)
        {
            weather.Text = "";
            dressing.Text = "";
            queryAsyncXML(city.Text);
        }
        private void weatherReturn(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        async void queryAsyncXML(string city)
        {
            // 城市输入为空，输出错误信息
            if (city == "")
            {
                var i = new MessageDialog("输入城市不能为空！").ShowAsync();
            }
            else
            {
                string url = "http://v.juhe.cn/weather/index?format=1&cityname=" + city + "&dtype=xml&key=bd807959a479f28b758ba5fb3fc3b207";
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(url);
                // 加载XML文档
                XmlDocument document = new XmlDocument();
                document.LoadXml(result);
                // 解析XML
                XmlNodeList list = document.GetElementsByTagName("reason");
                IXmlNode node = list.Item(0);
                // 判断城市输入是否正确
                if (node.InnerText == "查询不到该城市的信息")
                {
                    var i = new MessageDialog("查询不到该城市的信息！请确保城市输入正确！").ShowAsync();
                }
                else
                {
                    XmlNodeList list1 = document.GetElementsByTagName("temperature");
                    XmlNodeList list2 = document.GetElementsByTagName("weather");
                    XmlNodeList list3 = document.GetElementsByTagName("wind");
                    XmlNodeList list4 = document.GetElementsByTagName("dressing_advice");
                    // 关于这里的第几个问题，要看到请求具体返回的XML才清楚
                    // 下面的注释中第二个其实是指今天，第三个是指明天
                    IXmlNode node1 = list1.Item(1);// 第二个temperature
                    IXmlNode node2 = list2.Item(1);// 第二个weather，
                    IXmlNode node3 = list3.Item(1);// 第二个wind
                    IXmlNode node0 = list4.Item(0);// 第一个dressing_advice
                    IXmlNode node4 = list1.Item(2);// 第二个temperature
                    IXmlNode node5 = list2.Item(2);// 第二个weather
                    IXmlNode node6 = list3.Item(2);// 第二个wind
                    weather.Text = "今" + node1.InnerText + " " + node2.InnerText + " " + node3.InnerText +  "\n" + 
                        "明" + node4.InnerText + " " + node5.InnerText + " " + node6.InnerText;
                    dressing.Text = node0.InnerText;
                }
            }
        }
    }
}
