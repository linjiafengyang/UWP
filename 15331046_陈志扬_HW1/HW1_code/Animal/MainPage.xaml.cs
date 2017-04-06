using System;
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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Animal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate string AnimalSaying(object sender, myEventArgs e);//声明一个委托
        private event AnimalSaying Say;//委托声明一个事件
        private int times = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        interface Animal
        {
            //方法
            string saying(object sender, myEventArgs e);
            //属性
            int A { get; set; }
        }

        class cat : Animal
        {
            TextBlock word;
            private int a;

            public cat(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)//定义方法
            {
                this.word.Text += "Cat: I am a cat.\n";//修改cat的输出文字
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        class dog : Animal
        {
            TextBlock word;
            private int a;

            public dog(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)//定义方法
            {
                this.word.Text += "Dog: I am a dog.\n";//修改dog的输出文字
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }

        class pig : Animal
        {
            TextBlock word;
            private int a;

            public pig(TextBlock w)
            {
                this.word = w;
            }
            public string saying(object sender, myEventArgs e)//定义方法
            {
                this.word.Text += "Pig: I am a pig.\n";//修改pig的输出文字
                return "";
            }
            public int A
            {
                get { return a; }
                set { this.a = value; }
            }
        }
        private cat c;
        private dog d;
        private pig p;
        //当用户点击speak按钮时对应的操作，实现随机发言
        private void Button_ClickofSpeak(object sender, RoutedEventArgs e)
        {
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            //生成随机数，产生0、1、2的随机数，分别对应cat、dog、pig
            Random number = new Random();
            int num = number.Next(3);
            //注册事件
            if (num == 0) Say += new AnimalSaying(c.saying);//对应cat
            if (num == 1) Say += new AnimalSaying(d.saying);//对应dog
            if (num == 2) Say += new AnimalSaying(p.saying);//对应pig
            //执行事件
            Say(this, new myEventArgs(times++));  //事件中传递参数times
            //删除事件
            if (num == 0) Say -= new AnimalSaying(c.saying);
            if (num == 1) Say -= new AnimalSaying(d.saying);
            if (num == 2) Say -= new AnimalSaying(p.saying);
        }
        //当用户点击OK按钮对应的操作，实现指定发言
        private void Button_ClickofOK(object sender, RoutedEventArgs e)
        {
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            //对应cat
            if (name.Text == "cat")
            {
                Say += new AnimalSaying(c.saying);//添加事件
                Say(this, new myEventArgs(times++));//执行事件
                Say -= new AnimalSaying(c.saying);//删除事件
            }
            //对应dog
            else if (name.Text == "dog") {
                Say += new AnimalSaying(d.saying);
                Say(this, new myEventArgs(times++));
                Say -= new AnimalSaying(d.saying);
            }
            //对应pig
            else if (name.Text == "pig") {
                Say += new AnimalSaying(p.saying);
                Say(this, new myEventArgs(times++));
                Say -= new AnimalSaying(p.saying);
            }
            name.Text = "";
            
        }
        //自定义一个Eventargs传递事件参数
        class myEventArgs : EventArgs
        {
            public int t = 0;
            public myEventArgs(int tt)
            {
                this.t = tt;
            }
        }
    }
}
