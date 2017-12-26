﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class ProfitWindow : Window
    {
        private Property loggedUser;
        private ICommand addProfit;

        public ProfitWindow(Property loggedUser)
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;
            this.loggedUser = loggedUser;

            using (var ctx = new UserContext())
            {
                var dateTimes = ctx.Profits.OrderBy(p => p.date).Select(p => p.date).ToArray();
                List<Double> x = new List<double>();
                foreach(DateTime dt in dateTimes)
                {
                    x.Add(dt.AddMinutes(-1).ToOADate());
                }
                var y = ctx.Profits.Select(p => p.amount).ToArray();

                ProfitUC.LineGraph.Plot(x, y);
            }

            addProfit = new RelayCommand(execute: AddProfitInDb, canExecute: param => true);
        }

        private void AddProfitInDb(object obj)
        {

            new AddProfit().Show();
        }

        public ICommand AddBtn
        {
            get
            {
                return addProfit;
            }
            set
            {
                addProfit = value;
            }
        }

        public string Welcome
        {
            get
            {
                return string.Format(Constants.WELCOME, loggedUser.value);
            }
            set { }
        }

        public string Header
        {
            get
            {
                return Constants.PROFIT;
            }
            set { }
        }

        public string StrokeColor
        {
            get
            {
                return "#00FF00";
            }
            set { }
        }
    }
}
