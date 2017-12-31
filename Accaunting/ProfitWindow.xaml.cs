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
    public partial class ProfitWindow : Window, INotifyPropertyChanged
    {
        private Property loggedUser;
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<ProfitCategory> _ProfitCategories;
        private ICommand closeDialog;
        private User user;

        public ProfitWindow(Property loggedUser)
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;
            this.loggedUser = loggedUser;

            using (var ctx = new UserContext())
            {
                user = ctx.Users.Where(p => p.username.Equals(loggedUser.value)).SingleOrDefault();

                var dateTimes = ctx.Profits.OrderBy(p => p.date).Select(p => p.date).ToArray();
                List<Double> x = new List<double>();
                foreach(DateTime dt in dateTimes)
                {
                    x.Add(dt.AddMinutes(-1).ToOADate());
                }
                var y = ctx.Profits.Select(p => p.amount).ToArray();

                ProfitUC.LineGraph.Plot(x, y);


                _ProfitCategories = new ObservableCollection<ProfitCategory>(ctx.ProfitCategories.ToList());
            }

            ProfitUC.UserControl.AddHandler(AddProfitExpenseUC.DialogClose, new RoutedEventHandler(CategoryDialogClosing));

            closeDialog = new RelayCommand(MainDialogClosing, param => true);
        }

        private void CategoryDialogClosing(object sender, RoutedEventArgs e)
        {
            DialogClosingEventArgs eventArgs = (DialogClosingEventArgs)e;
            if (eventArgs.Parameter.Equals(false)) return;

            if (!string.IsNullOrWhiteSpace(ProfitUC.UserControl.CategoryText.Text))
            {
                using (var ctx = new UserContext())
                {
                    ProfitCategory category = new ProfitCategory()
                    {
                        name = ProfitUC.UserControl.CategoryText.Text
                    };
                    ctx.ProfitCategories.Add(category);
                    ctx.SaveChanges();
                    _ProfitCategories = _ProfitCategories.Concat(new[] { category });
                    PropChanged("Categories");
                }
            }
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

        private void MainDialogClosing(object sender)
        {
            if (SelectedCategory == null)
            {
                ProfitUC.MainDialog.IsOpen = true;
                System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, Constants.FIELD_PROFIT_CATEGORY), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (isValid(ProfitUC.UserControl.Date.Text, Constants.FIELD_DATE) && isValid(ProfitUC.UserControl.Time.Text, Constants.FIELD_TIME) && isValid(ProfitUC.UserControl.Amount.Text, Constants.AMOUNT))
                {
                    ProfitUC.MainDialog.IsOpen = false;
                    DateTime date = ProfitUC.UserControl.Date.SelectedDate.Value.Date;
                    TimeSpan time = ProfitUC.UserControl.Time.SelectedTime.Value.TimeOfDay;
                    String amount = ProfitUC.UserControl.Amount.Text;

                    DateTime resultDate = date + time;

                    using (var ctx = new UserContext())
                    {
                        Profit profit = new Profit()
                        {
                            amount = Double.Parse(amount),
                            date = resultDate,
                            category_id = SelectedCategory.id,
                            user_id = user.id
                        };
                        ctx.Profits.Add(profit);
                        ctx.SaveChanges();
                    }
                } else
                {
                    ProfitUC.MainDialog.IsOpen = true;
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

        public ProfitCategory SelectedCategory { get; set; }

        public IEnumerable<ProfitCategory> Categories
        {
            get { return _ProfitCategories; }
            set
            {
                _ProfitCategories = value;
                PropChanged("Categories");
            }
        }

        public string CloseDialogTxt
        {
            get
            {
                return Constants.CLOSE;
            }
            set { }
        }

        public string AddProfitExpenseText
        {
            get
            {
                return Constants.ADD_PROFIT;
            }
            set { }
        }

        public string AddCategoryText
        {
            get
            {
                return Constants.ADD_PROFIT_CATEGORY;
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
