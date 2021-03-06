﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Accaunting
{
    public partial class AddProfitExpenseUC : UserControl
    {
        public AddProfitExpenseUC()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static readonly RoutedEvent DialogClose = EventManager.RegisterRoutedEvent(
            "Dialog", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddProfitExpenseUC));

        public event RoutedEventHandler Dialog
        {
            add { AddHandler(AddProfitExpenseUC.DialogClose, value); }
            remove { RemoveHandler(AddProfitExpenseUC.DialogClose, value); }
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            RaiseEvent(new DialogClosingEventArgs(eventArgs.Session, eventArgs, AddProfitExpenseUC.DialogClose, this));
        }
    }
}
