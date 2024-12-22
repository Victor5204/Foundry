using Foundry.Data;
using Foundry;
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
using System.Data.Entity.Infrastructure;

namespace Foundry
{
    public partial class OrdersPage : Page
    {

        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            using (var context = new FoundryEntities2())
            {
                dgOrders.ItemsSource = context.Orders.Include("Users").ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Логика добавления заказа
            NavigationService.Navigate(new AddOrderPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Логика редактирования заказа
            if (dgOrders.SelectedItem is Orders selectedOrder)
            {
                NavigationService.Navigate(new EditOrderPage(selectedOrder));
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Логика удаления заказа
            if (dgOrders.SelectedItem is Orders selectedOrder)
            {
                using (var context = new FoundryEntities2())
                {
                    try
                    {
                        var orderToDelete = context.Orders.Find(selectedOrder.Id);
                        if (orderToDelete != null)
                        {
                            // Удаляем связанные данные в таблице OrderDetails
                            var relatedOrderDetails = context.OrderDetails.Where(od => od.OrderId == orderToDelete.Id).ToList();
                            foreach (var orderDetail in relatedOrderDetails)
                            {
                                context.OrderDetails.Remove(orderDetail);
                            }

                            context.Orders.Remove(orderToDelete);
                            context.SaveChanges();
                            LoadOrders();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        var innerException = ex.InnerException;
                        if (innerException != null)
                        {
                            MessageBox.Show($"Ошибка при удалении заказа: {innerException.Message}");
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления");
            }
        }
    }
}
