using Foundry.Data; // Убедитесь, что это правильное пространство имен
using System;
using System.Linq; // Добавьте эту директиву для использования FirstOrDefault
using System.Windows;
using System.Windows.Controls;

namespace Foundry
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            using (var context = new FoundryEntities2())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user != null)
                {
                    NavigationService.Navigate(new ProductsPage());
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }
    }
}