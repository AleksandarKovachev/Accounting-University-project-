using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
        private List<Activity> profitExpenses;
        private List<Activity> profitActivities;
        private List<Activity> expenseActivities;
        private List<string> periodList;
        private ICommand showDataButton;
        private User user;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            using (var ctx = new UserContext())
            {
                loggedUser = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
                if (loggedUser.value == null)
                {
                    new Login().Show();
                    this.Close();
                }

                user = ctx.Users.Where(u => u.username.Equals(loggedUser.value)).SingleOrDefault();
                expenseCategories = new List<ExpenseCategory>(ctx.ExpenseCategories.ToList());
                profitCategories = new List<ProfitCategory>(ctx.ProfitCategories.ToList());

                activities = new List<Activity>();
                profitActivities = new List<Activity>();
                expenseActivities = new List<Activity>();

                foreach (Profit profit in ctx.Profits.Where(p => p.user_id == user.id).OrderBy(p => p.date))
                {
                    ProfitCategory profitCategory = ctx.ProfitCategories.Where(p => p.id == profit.category_id).SingleOrDefault();
                    profitActivities.Add(new Activity() { dateTime = profit.date, amount = profit.amount, type = Constants.PROFIT, category = profitCategory.name });
                }

                foreach (Expense expense in ctx.Expenses.Where(e => e.user_id == user.id).OrderBy(e => e.date))
                {
                    ExpenseCategory expenseCategory = ctx.ExpenseCategories.Where(e => e.id == expense.category_id).SingleOrDefault();
                    expenseActivities.Add(new Activity() { dateTime = expense.date, amount = expense.amount, type = Constants.EXPENSE, category = expenseCategory.name });
                }

                activities.AddRange(profitActivities);
                activities.AddRange(expenseActivities);
                activities = activities.OrderBy(a => a.dateTime).ToList();
                profitExpenses = activities;
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

            periodList = new List<string>()
            {
                Constants.ALL_DATA,
                Constants.PERIOD_DAY,
                Constants.PERIOD_MONTH,
                Constants.PERIOD_YEAR
            };

            categoryList = allList;

            SelectedType = Constants.ALL_DATA;
            SelectedCategory = Constants.ALL_DATA;
            SelectedPeriod = Constants.ALL_DATA;

            chartData(activities, activities);

            showDataButton = new RelayCommand(ShowData, p => true);
        }

        private void ShowData(object obj)
        {
            lines.Children.Clear();

            if (SelectedType.Equals(Constants.PROFIT))
            {
                if (!SelectedCategory.Equals(Constants.ALL_DATA))
                {
                    List<Activity> profits = profitActivities.Where(p => p.category.Equals(SelectedCategory)).ToList();
                    profits = filterPeriod(profits);
                    chartData(profits, null);
                }
                else
                {
                    chartData(filterPeriod(profitActivities), null);
                }
            } else if (SelectedType.Equals(Constants.EXPENSE)) {
                if (!SelectedCategory.Equals(Constants.ALL_DATA))
                {
                    List<Activity> expenses = expenseActivities.Where(p => p.category.Equals(SelectedCategory)).ToList();
                    expenses = filterPeriod(expenses);
                    chartData(null, expenses);
                }
                else
                {
                    chartData(null, filterPeriod(expenseActivities));
                }
            } else
            {
                List<Activity> filteredProfitExpenses = filterPeriod(profitExpenses);
                chartData(filteredProfitExpenses, filteredProfitExpenses);
            }
        }

        private List<Activity> filterPeriod(List<Activity> activities)
        {
            if (SelectedPeriod.Equals(Constants.PERIOD_DAY))
            {
                return activities.Where(p => p.dateTime.Day == DateTime.Now.Day).ToList();
            }
            else if (SelectedPeriod.Equals(Constants.PERIOD_MONTH))
            {
                return activities.Where(p => p.dateTime.Month == DateTime.Now.Month).ToList();
            }
            else if (SelectedPeriod.Equals(Constants.PERIOD_YEAR))
            {
                return activities.Where(p => p.dateTime.Year == DateTime.Now.Year).ToList();
            }
            return activities;
        }

        private void chartData(List<Activity> profits, List<Activity> expenses)
        {
            if (profits != null)
            {
                List<double> profitX = new List<double>();
                List<double> profitY = new List<double>();
                foreach (Activity activity in profits.Where(a => a.type.Equals(Constants.PROFIT)))
                {
                    profitX.Add(activity.dateTime.AddMinutes(-1).ToOADate());
                    profitY.Add(activity.amount);
                }

                var profitLine = new LineGraph();
                lines.Children.Add(profitLine);
                profitLine.Stroke = new SolidColorBrush(Colors.Blue);
                profitLine.StrokeThickness = 2;
                profitLine.Plot(profitX, profitY);
            }

            if (expenses != null)
            {
                List<double> expenseX = new List<double>();
                List<double> expenseY = new List<double>();
                foreach (Activity activity in expenses.Where(a => a.type.Equals(Constants.EXPENSE)))
                {
                    expenseX.Add(activity.dateTime.AddMinutes(-1).ToOADate());
                    expenseY.Add(activity.amount);
                }

                var expenseLine = new LineGraph();
                lines.Children.Add(expenseLine);
                expenseLine.Stroke = new SolidColorBrush(Colors.Red);
                expenseLine.StrokeThickness = 2;
                expenseLine.Plot(expenseX, expenseY);
            }
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
            CategoryComboBox.SelectedItem = Constants.ALL_DATA;
        }

        public ICommand ShowDataButton
        {
            get { return showDataButton; }
            set { showDataButton = value; }
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

        public List<string> PeriodList
        {
            get { return periodList; }
            set { periodList = value; }
        }

        public string SelectedType { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedPeriod { get; set; }

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

        public string PeriodText
        {
            get
            {
                return Constants.PERIOD;
            }
            set { }
        }

        public string ShowDataButtonText
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

    public class VisibilityToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
