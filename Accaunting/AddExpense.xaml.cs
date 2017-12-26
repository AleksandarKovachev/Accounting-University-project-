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
    public partial class AddExpense : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand addExpense;
        private IEnumerable<ExpenseCategory> _ExpenseCategories;

        public AddExpense()
        {
            InitializeComponent();
            this.DataContext = this;

            using (var ctx = new UserContext())
            {
                _ExpenseCategories = new ObservableCollection<ExpenseCategory>(ctx.ExpenseCategories.ToList());
            }

            addExpense = new RelayCommand(AddExpenseToDb, param => true);

            UserControl.AddHandler(AddProfitExpenseUC.DialogClose, new RoutedEventHandler(OnDialogClosing));
        }

        private void AddExpenseToDb(object obj)
        {
            if (SelectedCategory == null)
            {
                System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, Constants.FIELD_EXPENSE_CATEGORY), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (isValid(UserControl.Date.Text, Constants.FIELD_DATE) && isValid(UserControl.Time.Text, Constants.FIELD_TIME) && isValid(UserControl.Amount.Text, Constants.AMOUNT))
                {
                    DateTime date = UserControl.Date.SelectedDate.Value.Date;
                    TimeSpan time = UserControl.Time.SelectedTime.Value.TimeOfDay;
                    String amount = UserControl.Amount.Text;

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
                        this.Close();
                    }
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

        private void OnDialogClosing(object sender, RoutedEventArgs e)
        {
            DialogClosingEventArgs eventArgs = (DialogClosingEventArgs)e;
            if (eventArgs.Parameter.Equals(false)) return;

            if (!string.IsNullOrWhiteSpace(UserControl.CategoryText.Text))
            {
                using (var ctx = new UserContext())
                {
                    ExpenseCategory category = new ExpenseCategory()
                    {
                        name = UserControl.CategoryText.Text
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
    }
}
