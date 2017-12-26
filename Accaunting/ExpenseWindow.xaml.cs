using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Accaunting
{
    public partial class ExpenseWindow : Window
    {

        private ICommand addExpense;

        private Property loggedUser;
        public ExpenseWindow(Property loggedUser)
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;
            this.loggedUser = loggedUser;

            using (var ctx = new UserContext())
            {
                var dateTimes = ctx.Expenses.OrderBy(p => p.date).Select(p => p.date).ToArray();
                List<Double> x = new List<double>();
                foreach (DateTime dt in dateTimes)
                {
                    x.Add(dt.AddMinutes(-1).ToOADate());
                }
                var y = ctx.Expenses.Select(p => p.amount).ToArray();

                ExpenseUC.LineGraph.Plot(x, y);
            }

            addExpense = new RelayCommand(execute: AddExpenseInDb, canExecute: param => true);
        }

        private void AddExpenseInDb(object obj)
        {
            new AddExpense().Show();
        }

        public ICommand AddBtn
        {
            get
            {
                return addExpense;
            }
            set
            {
                addExpense = value;
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
                return Constants.EXPENSE;
            }
            set { }
        }

        public string StrokeColor
        {
            get
            {
                return "#FF0000";
            }
            set { }
        }
    }
}
