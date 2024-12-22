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
using System.Xml.Linq;


namespace Foundry
{
    public partial class EditProductPage : Page
    {
        private Products product;

        public EditProductPage(Products product)
        {
            InitializeComponent();
            this.product = product;
            LoadMaterials();
            LoadProductData();
        }

 
        private void LoadMaterials()
        {
            using (var context = new FoundryEntities2())
            {
                cmbMaterial.ItemsSource = context.Materials.ToList();
                cmbMaterial.DisplayMemberPath = "Name";
                cmbMaterial.SelectedValuePath = "Id";
            }
        }

        private void LoadProductData()
        {
            txtName.Text = product.Name;
            cmbMaterial.SelectedValue = product.MaterialId;
            txtQuantity.Text = product.Quantity.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            int materialId = (int)cmbMaterial.SelectedValue;
            int quantity = int.Parse(txtQuantity.Text);

            using (var context = new FoundryEntities2())
            {
                var productToUpdate = context.Products.Find(product.Id);
                productToUpdate.Name = name;
                productToUpdate.MaterialId = materialId;
                productToUpdate.Quantity = quantity;
                context.SaveChanges();
            }

            NavigationService.Navigate(new ProductsPage());
        }
    }
}
