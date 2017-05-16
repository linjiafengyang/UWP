using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml.Data;
using SQLitePCL;


namespace Cashbook.ViewModels
{
    class DocumentViewModel
    {
        private ObservableCollection<Models.Document> allDocuments=new ObservableCollection<Models.Document>();
        public ObservableCollection<Models.Document> AllDocuments { get{ return this.allDocuments; } }
        
        public double payout;
        public double income;
        public double result;

        private Models.Document selectedDocument = default(Models.Document);
        //当前被选中的项
        public Models.Document SelectedDocument { get { return selectedDocument; } set { this.selectedDocument = value; } }
        public SQLiteConnection conn;
        //默认构造函数
        public DocumentViewModel()
        {
            conn = new SQLiteConnection("Document.db");
            Models.Document document = null;
            payout = 0;
            income = 0;
            result = 0;
            string sql = @"CREATE TABLE IF NOT EXISTS
                                  Document(id   VARCHAR(150) PRIMARY KEY NOT NULL,
                                        day  VARCHAR(150),
                                        consumptionType VARCHAR(150),
                                        amountOfMoney DOUBLE,
                                        remarks     VARCHAR(150)
                          );";
            using (var statement1 = conn.Prepare(sql))
            {
                statement1.Step();
                using (var statement = conn.Prepare("select id, day, consumptionType, amountOfMoney, remarks from Document"))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        document = new Models.Document((string)statement[0], (string)statement[2], (double)statement[3], (string)statement[4], DateTimeOffset.Parse((string)statement[1]));
                        // 从数据库读取消费收入记录并计算支出、收入、结余
                        if (document.consumptionType == "收入")
                        {
                            income += document.amountOfMoney;
                        } else
                        {
                            payout -= document.amountOfMoney;
                        }
                        this.allDocuments.Add(document);
                    }
                }
            }
            result = income + payout;
        }

        //增加新的记账记录
        public void AddDocument(DateTimeOffset day,string consumptionType,double amountOfMoney,string remarks)
        {
            string newID = Guid.NewGuid().ToString();
            using (var statement = conn.Prepare("insert into Document(id,day,consumptionType,amountOfMoney,remarks) values(?,?,?,?,?);"))
            {
                statement.Bind(1, newID);
                statement.Bind(2, day.ToString());
                statement.Bind(3, consumptionType);
                statement.Bind(4, amountOfMoney);
                statement.Bind(5, remarks);
                statement.Step();
            }
            this.allDocuments.Add(new Models.Document(newID, consumptionType, amountOfMoney, remarks, day));
            if (consumptionType == "收入")
            {
                income += amountOfMoney;
            }
            else
            {
                payout -= amountOfMoney;
            }
            result = income + payout;
        }

        //删除记账记录
        public void RemoveDocument()
        {
            using (var statement = conn.Prepare("DELETE FROM Document WHERE id = ? "))
            {
                statement.Bind(1, this.SelectedDocument.id);
                statement.Step();
            }
            // 删除时收入减少，支出增加，结余
            if (this.SelectedDocument.consumptionType == "收入")
            {
                income -= this.SelectedDocument.amountOfMoney;
            }
            else
            {
                payout += this.SelectedDocument.amountOfMoney;
            }
            result = income + payout;
            this.allDocuments.Remove(this.SelectedDocument);
            this.selectedDocument = null;
        }

        //更新某条记录的消费类型
        public void UpdateConsumptionType(string consumptionType)
        {
            using (var statement = conn.Prepare("UPDATE Document SET consumptionType = ? WHERE id = ?"))
            {
                statement.Bind(1, consumptionType);
                statement.Bind(2, this.SelectedDocument.id);
                statement.Step();
            }
            this.SelectedDocument.consumptionType = consumptionType;
        }

        //更新某条记录的金额
        public void UpdateAmountOfMoney(double amountOfMoney)
        {
            using (var statement = conn.Prepare("UPDATE Document SET amountOfMoney = ? WHERE id = ?"))
            {
                statement.Bind(1, amountOfMoney);
                statement.Bind(2, this.SelectedDocument.id);
                statement.Step();
            }
            this.SelectedDocument.amountOfMoney = amountOfMoney;
        }

        //更新某条备注的注释
        public void UpdateRemarks(string remarks)
        {
            using (var statement = conn.Prepare("UPDATE Document SET remarks = ? WHERE id = ?"))
            {
                statement.Bind(1, remarks);
                statement.Bind(2, this.SelectedDocument.id);
                statement.Step();
            }
            this.SelectedDocument.remarks = remarks;
        }
        //更新日期
        public void UpdateDate(DateTimeOffset date)
        {
            using (var statement = conn.Prepare("UPDATE Document SET day = ? WHERE id = ? "))
            {
                statement.Bind(1, date.ToString());
                statement.Bind(2, this.selectedDocument.id);
                statement.Step();
            }
            this.selectedDocument.day = date;
            this.selectedDocument.dayOfMouth = this.selectedDocument.day.Day.ToString();
            this.selectedDocument.dayOfWeek = this.selectedDocument.day.DayOfWeek.ToString();
        }

        //更新整条记录
        public void UpdateDocument(string consumptionType, double amountOfMoney, string remarks, DateTimeOffset date)
        {
            if (this.SelectedDocument.consumptionType == "收入")
            {
                income -= this.SelectedDocument.amountOfMoney;
            }
            else
            {
                payout += this.SelectedDocument.amountOfMoney;
            }

            UpdateConsumptionType(consumptionType);
            UpdateAmountOfMoney(amountOfMoney);
            UpdateRemarks(remarks);
            UpdateDate(date);

            if (consumptionType == "收入")
            {
                this.SelectedDocument.output_amountOfMoney = "+" + amountOfMoney.ToString();
            }
            else
            {
                this.SelectedDocument.output_amountOfMoney = "-" + amountOfMoney.ToString();
            }

            if (consumptionType == "收入")
            {
                income += amountOfMoney;
            }
            else
            {
                payout -= amountOfMoney;
            }
            result = income + payout;
        }

        //根据filter来搜索储存的记账记录
        public List<Models.Document> SearchDocument(string filter)
        {
            List<Models.Document> result = new List<Models.Document>();
            using (var statement = conn.Prepare("select id,day,consumptionType,amountOfMoney,remarks from Documenet where day like ? or consumptionType like ? or amountOfMoney like ? or remarks like ?"))
            {
                statement.Bind(1, "%" + filter + "%");
                statement.Bind(2, "%" + filter + "%");
                statement.Bind(3, "%" + filter + "%");
                statement.Bind(4, "%" + filter + "%");
                while (SQLiteResult.ROW == statement.Step())
                {
                    result.Add(new Models.Document((string)statement[0], (string)statement[2], (double)statement[3], (string)statement[4], DateTimeOffset.Parse((string)statement[1])));
                }
            }
            return result;
        }
    }
}
