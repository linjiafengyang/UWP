using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            // 加入两个用来测试的item
            this.allItems.Add(new Models.TodoItem("123", "123", DateTime.Now.Date));
            this.allItems.Add(new Models.TodoItem("456", "456", DateTime.Now.Date));
        }

        public void AddTodoItem(string title, string description, System.DateTimeOffset set_day)
        {
            this.allItems.Add(new Models.TodoItem(title, description, set_day));// 直接调用Add方法
        }

        public void RemoveTodoItem()
        {
            // DIY
            this.allItems.Remove(this.SelectedItem);// 直接调用Remove方法
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, System.DateTimeOffset set_day)
        {
            // DIY
            // 获取TodoItem的下标
            var index = this.allItems.IndexOf(this.selectedItem);
            // 设置title，description，day
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.day = set_day;
            // 删掉原来的TodoItem
            this.allItems.Remove(this.SelectedItem);
            // 在原位置插入更新update后的TodoItem
            this.allItems.Insert(index, this.selectedItem);
            // set selectedItem to null after update
            this.selectedItem = null;
        }
    }
}
