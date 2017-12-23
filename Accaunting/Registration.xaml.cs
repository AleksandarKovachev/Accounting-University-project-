using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class Registration : Window
    {
        private ICommand login;

        public Registration()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            login = new RelayCommand(ShowLoginWindow, param => true);
        }

        private void ShowLoginWindow(object obj)
        {
            new Login().Show();
            this.Close();
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
