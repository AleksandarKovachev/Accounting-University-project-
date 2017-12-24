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
        public Menu()
        {
            InitializeComponent();
        }

        private void Overview_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Logout_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var ctx = new UserContext())
            {
                Property loggedUser = ctx.Properties.Where(p => p.key == PropertyConstants.LOGGED_USER).SingleOrDefault();
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
