using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Accaunting
{
    public partial class Menu : UserControl
    {

        private Property loggedUser;

        public Menu()
        {
            InitializeComponent();

            using (var ctx = new UserContext())
            {
                loggedUser = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
            }
        }

        private void Overview_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Profit_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            new ProfitWindow(loggedUser).Show();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
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
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }
    }
}
