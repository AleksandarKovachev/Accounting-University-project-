﻿using System.Windows;
using System.Windows.Input;

namespace Accaunting
{
    public partial class Login : Window
    {

        private ICommand registration;

        public Login()
        {
            InitializeComponent();
            this.DataContext = this;
            this.WindowState = WindowState.Maximized;

            registration = new RelayCommand(ShowRegistrationWindow, param => true);
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

        public string LoginBtn
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