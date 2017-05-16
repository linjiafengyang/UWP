using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Cashbook.ViewModels
{
    class PersonalInfoViewModel
    {
        public SQLiteConnection conn;
        //默认构造函数
        public PersonalInfoViewModel()
        {
            conn = new SQLiteConnection("personalInfo.db");
            Models.PersonalInfo account = null;
            string sql = @"CREATE TABLE IF NOT EXISTS
                                  PersonalInfo(id   VARCHAR(150) PRIMARY KEY NOT NULL,
                                        nickname  VARCHAR(150),
                                        mail VARCHAR(150),
                                        password     VARCHAR(150),
                                        imageUriString      VARCHAR(150)
                          );";
            using (var statement1 = conn.Prepare(sql))
            {
                statement1.Step();
                using (var statement = conn.Prepare("select id, nickname, mail, password, imageUriString from PersonalInfo"))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        account = new Models.PersonalInfo((string)statement[0], (string)statement[1], (string)statement[2], (string)statement[3], (string)statement[4]);
                    }
                }
            }
        }
        // 根据邮箱查询整条记录
        public Models.PersonalInfo queryMail(string mail)
        {
            conn = new SQLiteConnection("personalInfo.db");
            Models.PersonalInfo account = null;
            using (var statement = conn.Prepare("select id, nickname, mail, password, imageUriString from PersonalInfo where mail = ?"))
            {
                statement.Bind(1, mail);
                while (SQLiteResult.ROW == statement.Step())
                {
                    account = new Models.PersonalInfo((string)statement[0], (string)statement[1], (string)statement[2], (string)statement[3], (string)statement[4]);
                }
            }
            return account;
        }
        // 添加账户信息记录
        public void AddAccount(string nickname, string mail, string password, string imageUriString)
        {
            string newID = Guid.NewGuid().ToString();
            using (var statement = conn.Prepare("insert into PersonalInfo(id, nickname, mail, password, imageUriString) values(?,?,?,?,?);"))
            {
                statement.Bind(1, newID);
                statement.Bind(2, nickname);
                statement.Bind(3, mail);
                statement.Bind(4, password);
                statement.Bind(5, imageUriString);
                statement.Step();
            }
        }
        // 更新个人信息记录
        public void updateAccount(string mail, string nickname, string password)
        {
            conn = new SQLiteConnection("personalInfo.db");
            using (var statement = conn.Prepare("UPDATE PersonalInfo SET nickname = ?, password = ? WHERE mail = ? "))
            {
                statement.Bind(1, nickname);
                statement.Bind(2, password);
                statement.Bind(3, mail);
                statement.Step();
            }
        }
        // 通过查询数据库中是否已有该邮箱，若有，返回false，表示注册失败（邮箱不能重复）
        public bool compareMail(string Mail)
        {
            conn = new SQLiteConnection("personalInfo.db");
            using (var statement = conn.Prepare("select mail from PersonalInfo"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    if ((string)statement[0] == Mail)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // 通过匹配邮箱和密码来判断是否可登录
        public bool comparePassword(string Mail, string Password)
        {
            conn = new SQLiteConnection("personalInfo.db");
            using (var statement = conn.Prepare("select mail, password from PersonalInfo"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    if ((string)statement[0] == Mail && (string)statement[1] == Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
