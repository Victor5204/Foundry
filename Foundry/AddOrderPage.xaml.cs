using Foundry.Data;
using System;
using System.Collections.Generic;
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
using Foundry;

namespace Foundry
{
    public partial class AddOrderPage : Page
    {
        public AddOrderPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var context = new FoundryEntities2())
            {
                cmbUser.ItemsSource = context.Users.ToList();
                cmbUser.DisplayMemberPath = "Login";
                cmbUser.SelectedValuePath = "Id";
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            int userId = (int)cmbUser.SelectedValue;
            DateTime orderDate = dpOrderDate.SelectedDate ?? DateTime.Now;

            using (var context = new FoundryEntities2())
            {
                var order = new Orders
                {
                    UserId = userId,
                    OrderDate = orderDate
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }

            NavigationService.Navigate(new OrdersPage());
        }
    }
}
