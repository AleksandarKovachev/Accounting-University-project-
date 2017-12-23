using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class MainWindow : Window
    {
        private void Overview_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;
        }

        public string Header
        {
            get
            {
                return Constants.HEADER;
            }
            set { }
        }

        public string Welcome
        {
            get
            {
                return Constants.WELCOME;
            }
            set { }
        }
    }
}
