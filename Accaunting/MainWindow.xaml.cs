using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class MainWindow : Window
    {

        private Property loggedUser;

        private void Overview_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Logout_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var ctx = new UserContext())
            {
                loggedUser.value = null;
                ctx.Properties.Attach(loggedUser);
                ctx.Entry(loggedUser).State = EntityState.Modified;
                ctx.SaveChangesAsync();
                new Login().Show();
                this.Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            using (var ctx = new UserContext())
            {
                loggedUser = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
                if(loggedUser.value == null)
                {
                    new Login().Show();
                    this.Close();
                }
            }
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
                return string.Format(Constants.WELCOME, loggedUser.value);
            }
            set { }
        }
    }
}
