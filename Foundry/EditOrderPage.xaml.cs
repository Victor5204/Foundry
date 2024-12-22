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
using Foundry.Data;

namespace Foundry
{
   
    public partial class EditOrderPage : Page
    {
        private Orders order;

        public EditOrderPage(Orders order)
        {
            InitializeComponent();
            this.order = order;
            LoadUsers();
            LoadOrderData();
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

        private void LoadOrderData()
        {
            cmbUser.SelectedValue = order.UserId;
            dpOrderDate.SelectedDate = order.OrderDate;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            int userId = (int)cmbUser.SelectedValue;
            DateTime orderDate = dpOrderDate.SelectedDate ?? DateTime.Now;

            using (var context = new FoundryEntities2())
            {
                var orderToUpdate = context.Orders.Find(order.Id);
                orderToUpdate.UserId = userId;
                orderToUpdate.OrderDate = orderDate;
                context.SaveChanges();
            }

            NavigationService.Navigate(new OrdersPage());
        }
    }
}
