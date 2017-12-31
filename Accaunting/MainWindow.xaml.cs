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
                Constants.PERIOD_WEEK,
                Constants.PERIOD_MONTH,
                Constants.PERIOD_YEAR
            };

            categoryList = allList;

            SelectedType = Constants.ALL_DATA;
            CategoryComboBox.SelectedIndex = 0;

            List<double> profitX = new List<double>();
            List<double> profitY = new List<double>();
            foreach (Activity activity in Activities.Where(a => a.type.Equals(Constants.PROFIT)))
            {
                profitX.Add(activity.dateTime.AddMinutes(-1).ToOADate());
                profitY.Add(activity.amount);
            }

            List<double> expenseX = new List<double>();
            List<double> expenseY = new List<double>();
            foreach (Activity activity in Activities.Where(a => a.type.Equals(Constants.EXPENSE)))
            {
                expenseX.Add(activity.dateTime.AddMinutes(-1).ToOADate());
                expenseY.Add(activity.amount);
            }

            var profitLine = new LineGraph();
            lines.Children.Add(profitLine);
            profitLine.Stroke = new SolidColorBrush(Colors.Blue);
            profitLine.StrokeThickness = 2;
            profitLine.Plot(profitX, profitY);

            var expenseLine = new LineGraph();
            lines.Children.Add(expenseLine);
            expenseLine.Stroke = new SolidColorBrush(Colors.Red);
            expenseLine.StrokeThickness = 2;
            expenseLine.Plot(expenseX, expenseY);

            showDataButton = new RelayCommand(ShowData, p => true);
        }

        private void ShowData(object obj)
        {
            
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
