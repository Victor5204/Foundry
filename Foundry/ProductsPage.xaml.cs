using Foundry.Data; // Убедитесь, что это правильное пространство имен
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity; // Добавьте эту директиву для использования Include
using System.Data.Entity.Infrastructure;

namespace Foundry
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }


        private void LoadProducts()
        {
            using (var context = new FoundryEntities2()) // Используйте одно и то же имя контекста
            {
                dgProducts.ItemsSource = context.Products.Include("Materials").ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Логика добавления продукции
            NavigationService.Navigate(new AddProductPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Логика редактирования продукции
            if (dgProducts.SelectedItem is Products selectedProduct)
            {
                NavigationService.Navigate(new EditProductPage(selectedProduct));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Логика удаления продукции
            if (dgProducts.SelectedItem is Products selectedProduct)
            {
                using (var context = new FoundryEntities2()) // Используйте одно и то же имя контекста
                {
                    try
                    {
                        var productToDelete = context.Products.Find(selectedProduct.Id);
                        if (productToDelete != null)
                        {
                            // Удаляем связанные данные в таблице OrderDetails
                            var relatedOrderDetails = context.OrderDetails.Where(od => od.ProductId == productToDelete.Id).ToList();
                            foreach (var orderDetail in relatedOrderDetails)
                            {
                                context.OrderDetails.Remove(orderDetail);
                            }

                            context.Products.Remove(productToDelete);
                            context.SaveChanges();
                            LoadProducts();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        var innerException = ex.InnerException;
                        if (innerException != null)
                        {
                            MessageBox.Show($"Ошибка при удалении продукта: {innerException.Message}");
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка при удалении продукта: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления");
            }
        }
    }
}