using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class ExpenseWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand addExpense;
        private IEnumerable<ExpenseCategory> _ExpenseCategories;
        private ICommand closeDialog;

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
                _ExpenseCategories = new ObservableCollection<ExpenseCategory>(ctx.ExpenseCategories.ToList());
            }

            ExpenseUC.UserControl.AddHandler(AddProfitExpenseUC.DialogClose, new RoutedEventHandler(OnDialogClosing));

            closeDialog = new RelayCommand(MainDialogClosing, param => true);
        }

        public ICommand CloseDialog
        {
            get
            {
                return closeDialog;
            }
            set
            {
                closeDialog = value;
            }
        }

        private void MainDialogClosing(object obj)
        {
            if (SelectedCategory == null)
            {
                ExpenseUC.MainDialog.IsOpen = true;
                System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, Constants.FIELD_EXPENSE_CATEGORY), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (isValid(ExpenseUC.UserControl.Date.Text, Constants.FIELD_DATE) && isValid(ExpenseUC.UserControl.Time.Text, Constants.FIELD_TIME) && isValid(ExpenseUC.UserControl.Amount.Text, Constants.AMOUNT))
                {
                    ExpenseUC.MainDialog.IsOpen = false;
                    DateTime date = ExpenseUC.UserControl.Date.SelectedDate.Value.Date;
                    TimeSpan time = ExpenseUC.UserControl.Time.SelectedTime.Value.TimeOfDay;
                    String amount = ExpenseUC.UserControl.Amount.Text;

                    DateTime resultDate = date + time;

                    using (var ctx = new UserContext())
                    {
                        Expense expense = new Expense()
                        {
                            amount = Double.Parse(amount),
                            date = resultDate,
                            category_id = SelectedCategory.id
                        };
                        ctx.Expenses.Add(expense);
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    ExpenseUC.MainDialog.IsOpen = true;
                }
            }
        }

        private bool isValid(string field, string name)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, name), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public ExpenseCategory SelectedCategory { get; set; }

        public IEnumerable<ExpenseCategory> Categories
        {
            get { return _ExpenseCategories; }
            set
            {
                _ExpenseCategories = value;
                PropChanged("Categories");
            }
        }

        private void OnDialogClosing(object sender, RoutedEventArgs e)
        {
            DialogClosingEventArgs eventArgs = (DialogClosingEventArgs)e;
            if (eventArgs.Parameter.Equals(false)) return;

            if (!string.IsNullOrWhiteSpace(ExpenseUC.UserControl.CategoryText.Text))
            {
                using (var ctx = new UserContext())
                {
                    ExpenseCategory category = new ExpenseCategory()
                    {
                        name = ExpenseUC.UserControl.CategoryText.Text
                    };
                    ctx.ExpenseCategories.Add(category);
                    ctx.SaveChanges();
                    _ExpenseCategories = _ExpenseCategories.Concat(new[] { category });
                    PropChanged("Categories");
                }
            }
        }

        public string AddCategoryText
        {
            get
            {
                return Constants.ADD_EXPENSE_CATEGORY;
            }
            set { }
        }

        public string CloseDialogTxt
        {
            get
            {
                return Constants.CLOSE;
            }
            set { }
        }

        public void PropChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string AmountText
        {
            get
            {
                return Constants.AMOUNT;
            }
            set { }
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
