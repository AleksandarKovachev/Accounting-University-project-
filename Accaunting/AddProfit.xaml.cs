using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class AddProfit : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand addProfit;

        public AddProfit()
        {
            InitializeComponent();
            this.DataContext = this;

            using (var ctx = new UserContext())
            {
                _ProfitCategories = new ObservableCollection<ProfitCategory>(ctx.ProfitCategories.ToList());
            }

            addProfit = new RelayCommand(AddProfitToDb, param => true);

            UserControl.AddHandler(AddProfitExpenseUC.DialogClose, new RoutedEventHandler(OnDialogClosing));
        }

        private void AddProfitToDb(object obj)
        {
            ProfitCategory category = SelectedCategory;

            if (category == null)
            {
                System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, Constants.FIELD_PROFIT_CATEGORY), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
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
                        Profit profit = new Profit()
                        {
                            amount = Double.Parse(amount),
                            date = resultDate,
                            category = category
                        };
                        ctx.Profits.Add(profit);
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

        public ProfitCategory SelectedCategory { get; set; }

        private IEnumerable<ProfitCategory> _ProfitCategories;
        public IEnumerable<ProfitCategory> Categories {
            get { return _ProfitCategories; }
            set
            {
                _ProfitCategories = value;
                PropChanged("Categories");
            }
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

        private void OnDialogClosing(object sender, RoutedEventArgs e)
        {
            DialogClosingEventArgs eventArgs = (DialogClosingEventArgs)e;
            if (eventArgs.Parameter.Equals(false)) return;

            if (!string.IsNullOrWhiteSpace(UserControl.CategoryText.Text))
            {
                using (var ctx = new UserContext())
                {
                    ProfitCategory category = new ProfitCategory()
                    {
                        name = UserControl.CategoryText.Text
                    };
                    ctx.ProfitCategories.Add(category);
                    ctx.SaveChanges();
                    _ProfitCategories = _ProfitCategories.Concat(new[] { category });
                    PropChanged("Categories");
                }
            }
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

    }
}
