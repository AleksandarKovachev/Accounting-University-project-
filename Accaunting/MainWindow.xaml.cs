﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Accaunting
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private Property loggedUser;
        private List<string> typeList;
        private List<string> categoryList;
        private List<ExpenseCategory> expenseCategories;
        private List<ProfitCategory> profitCategories;
        private List<string> allList;
        private List<Activity> activities;
        private List<Activity> profitActivities;
        private List<Activity> expenseActivities;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            using (var ctx = new UserContext())
            {
                loggedUser = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
                if(loggedUser.value == null)
                {
                    new Login().Show();
                    this.Close();
                }

                expenseCategories = new List<ExpenseCategory>(ctx.ExpenseCategories.ToList());
                profitCategories = new List<ProfitCategory>(ctx.ProfitCategories.ToList());

                var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
                var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();

                LineGraph.Plot(x, y);

                activities = new List<Activity>();
                profitActivities = new List<Activity>();
                expenseActivities = new List<Activity>();

                foreach (Profit profit in ctx.Profits.OrderBy(p => p.date))
                {
                    ProfitCategory profitCategory = ctx.ProfitCategories.Where(p => p.id == profit.category_id).SingleOrDefault();
                    profitActivities.Add(new Activity() { dateTime = profit.date, amount = profit.amount, type = Constants.PROFIT, category = profitCategory.name });
                }

                foreach (Expense expense in ctx.Expenses.OrderBy(e => e.date))
                {
                    ExpenseCategory expenseCategory = ctx.ExpenseCategories.Where(e => e.id == expense.category_id).SingleOrDefault();
                    expenseActivities.Add(new Activity() { dateTime = expense.date, amount = expense.amount, type = Constants.EXPENSE, category = expenseCategory.name });
                }

                activities.AddRange(profitActivities);
                activities.AddRange(expenseActivities);
                activities = activities.OrderBy(a => a.dateTime).ToList();
            }

            typeList = new List<string>()
            {
                Constants.ALL_DATA,
                Constants.PROFIT,
                Constants.EXPENSE
            };

            allList = new List<string>()
            {
                Constants.ALL_DATA
            };

            categoryList = allList;

            SelectedType = Constants.ALL_DATA;
            CategoryComboBox.SelectedIndex = 0;
        }
        
        private void ActivityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActivityTypeComboBox.SelectedItem.Equals(Constants.PROFIT))
            {
                activities = profitActivities;
                PropChanged("Activities");
            } else if (ActivityTypeComboBox.SelectedItem.Equals(Constants.EXPENSE))
            {
                activities = expenseActivities;
                PropChanged("Activities");
            } else
            {
                activities.Clear();
                activities.AddRange(profitActivities);
                activities.AddRange(expenseActivities);
                activities = activities.OrderBy(a => a.dateTime).ToList();
                PropChanged("Activities");
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedType.Equals(Constants.PROFIT))
            {
                categoryList = profitCategories.ConvertAll(p => p.name);
                categoryList.Add(Constants.ALL_DATA);
                PropChanged("CategoryList");
            }
            else if (SelectedType.Equals(Constants.EXPENSE))
            {
                categoryList = expenseCategories.ConvertAll(p => p.name);
                categoryList.Add(Constants.ALL_DATA);
                PropChanged("CategoryList");
            }
            else
            {
                categoryList = allList;
                PropChanged("CategoryList");
            }
        }

        public List<Activity> Activities
        {
            get { return activities; }
            set { activities = value; }
        }

        public List<string> TypeList
        {
            get { return typeList; }
            set { typeList = value; }
        }

        public List<string> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value;
                PropChanged("CategoryList"); }
        }

        public string SelectedType { get; set; }

        public string Header
        {
            get
            {
                return Constants.HEADER;
            }
            set { }
        }

        public string TypeText
        {
            get
            {
                return Constants.TYPE;
            }
            set { }
        }

        public string CategoryText
        {
            get
            {
                return Constants.CATEGORY;
            }
            set { }
        }

        public string ShowDataButton
        {
            get
            {
                return Constants.SHOW;
            }
            set { }
        }

        public string Activity
        {
            get
            {
                return Constants.Activity;
            }
            set { }
        }

        public string Welcome
        {
            get
            {
                return string.Format(Constants.WELCOME, loggedUser.value);
            }
            set {}
        }

        public void PropChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
