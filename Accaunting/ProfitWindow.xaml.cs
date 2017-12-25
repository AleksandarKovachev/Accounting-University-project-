using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
            var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
            linegraph.Plot(x, y);

            addProfit = new RelayCommand(execute: AddProfitInDbAsync, canExecute: param => true);
        }

        private void AddProfitInDbAsync(object obj)
        {

            new AddProfit().Show();
        }

        public ICommand AddProfit
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
    }
}
