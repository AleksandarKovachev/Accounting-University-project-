using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class Login : Window
    {

        private ICommand registration;
        private ICommand login;

        public Login()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            registration = new RelayCommand(ShowRegistrationWindow, param => true);
            login = new RelayCommand(LoginUser, param => true);
        }

        private void LoginUser(object obj)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            if (isValid(username, Constants.USERNAME) && isValid(password, Constants.PASSWORD))
            {
                using (var ctx = new UserContext())
                {
                    User user = (from x in ctx.Users
                             where x.username == username &&
                             x.password == password
                             select x).SingleOrDefault();
                    if(user == null)
                    {
                        System.Windows.MessageBox.Show(String.Format(Constants.MISSING_USER, username), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                    } else
                    {
                        Property property = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
                        property.value = username;
                        ctx.Properties.Attach(property);
                        ctx.Entry(property).State = EntityState.Modified;
                        ctx.SaveChangesAsync();

                        new MainWindow().Show();
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

        private void ShowRegistrationWindow(object obj)
        {
            new Registration().Show();
            this.Close();
        }

        public ICommand Registration
        {
            get
            {
                return registration;
            }
            set
            {
                registration = value;
            }
        }

        public ICommand LoginBtn
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
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

        public string Username
        {
            get
            {
                return Constants.USERNAME;
            }
            set { }
        }

        public string Password
        {
            get
            {
                return Constants.PASSWORD;
            }
            set { }
        }

        public string LoginBtnText
        {
            get
            {
                return Constants.LOGIN;
            }
            set { }
        }

        public string RegistrationBtnText
        {
            get
            {
                return Constants.REGISTRATION;
            }
            set { }
        }
    }
}
