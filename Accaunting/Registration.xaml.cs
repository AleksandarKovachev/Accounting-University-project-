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

            ShowUserTypes();
        }

        private void RegisterNewUser(object obj)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            string email = EmailTextBox.Text;
            UserType userType = UserTypeList.SelectedItem as UserType;

            if (isValid(username, Constants.USERNAME) && isValid(password, Constants.PASSWORD) && isValid(email, Constants.EMAIL))
            {
                if(userType == null)
                {
                    System.Windows.MessageBox.Show(String.Format(Constants.FIELD_IS_EMPTY, Constants.USER_TYPE), Constants.MESSAGE_BOX_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    using (var ctx = new UserContext())
                    {
                        User user = new User()
                        {
                            username = username,
                            password = password,
                            email = email,
                            userType_id = userType.id,
                            createDate = DateTime.Now
                        };
                        ctx.Users.Add(user);
                        ctx.SaveChangesAsync();

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

        private void ShowUserTypes()
        {
            using (var ctx = new UserContext())
            {
                UserTypeList.ItemsSource = ctx.UserTypes.ToList();
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

        public string Usertype
        {
            get
            {
                return Constants.USER_TYPE;
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
