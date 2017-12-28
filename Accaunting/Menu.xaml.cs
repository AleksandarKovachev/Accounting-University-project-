using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            new MainWindow().Show();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void Expense_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            new ExpenseWindow(loggedUser).Show();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
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
