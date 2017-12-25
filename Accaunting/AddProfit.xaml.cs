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
        public AddProfit()
        {
            InitializeComponent();
            this.DataContext = this;

            using (var ctx = new UserContext())
            {
                _ProfitCategories = new ObservableCollection<ProfitCategory>(ctx.ProfitCategories.ToList());
            }
        }

        private IEnumerable<ProfitCategory> _ProfitCategories;
        public IEnumerable<ProfitCategory> ProfitCategories {
            get { return _ProfitCategories; }
            set
            {
                _ProfitCategories = value;
                PropChanged("ProfitCategories");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if(!Equals(eventArgs.Parameter, true)) return;

            if (!string.IsNullOrWhiteSpace(ProfitCategoryText.Text))
            {
                using (var ctx = new UserContext())
                {
                    ProfitCategory category = new ProfitCategory()
                    {
                        name = ProfitCategoryText.Text
                    };
                    ctx.ProfitCategories.Add(category);
                    ctx.SaveChanges();
                    _ProfitCategories = _ProfitCategories.Concat(new[] { category });
                    PropChanged("ProfitCategories");
                }
            }
        }

        public string AddProfitCategoryText
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
