using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class Registration : Window
    {
        private ICommand login;
        private ICommand registrationBtn;

        public Registration()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            login = new RelayCommand(ShowLoginWindow, param => true);
            registrationBtn = new RelayCommand(RegisterNewUser, param => true);
        }

        private void RegisterNewUser(object obj)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            string email = EmailTextBox.Text;

            if (isValid(username, Constants.USERNAME) && isValid(password, Constants.PASSWORD) && isValid(email, Constants.EMAIL))
            {
                if(username.Length < 4)
                {
                    showMessage(Constants.USERNAME_LENGTH, username);
                    return;
                }

                if (password.Length < 4)
                {
                    showMessage(Constants.PASSWORD_LENGTH, password);
                    return;
                }

                if (!IsValidEmail(email))
                {
                    showMessage(Constants.EMAIL_NOT_VALID, email);
                    return;
                }

                using (var ctx = new UserContext())
                {
                    if (ctx.Users.Where(u => u.username.Equals(username)).SingleOrDefault() != null)
                    {
                        showMessage(Constants.USERNAME_EXISTING, username);
                        return;
                    }

                    if (ctx.Users.Where(u => u.email.Equals(email)).SingleOrDefault() != null)
                    {
                        showMessage(Constants.EMAIL_EXISTING, username);
                        return;
                    }

                    User user = new User()
                    {
                        username = username,
                        password = password,
                        email = email,
                        createDate = DateTime.Now
                    };
                    ctx.Users.Add(user);
                    ctx.SaveChanges();

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

        private bool isValid(string field, string name)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                showMessage(Constants.FIELD_IS_EMPTY, name);
                return false;
            }
            return true;
        }

        private void showMessage(string message, string parameter)
        {
            System.Windows.MessageBox.Show(String.Format(message, parameter), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ShowLoginWindow(object obj)
        {
            new Login().Show();
            this.Close();
        }

        public ICommand RegistrationBtn
        {
            get
            {
                return registrationBtn;
            }
            set
            {
                registrationBtn = value;
            }
        }

        public ICommand Login
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

        public string Email
        {
            get
            {
                return Constants.EMAIL;
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
