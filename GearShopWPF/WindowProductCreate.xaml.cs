using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GearShopWPF
{
    /// <summary>
    /// Interaction logic for WindowProductCreate.xaml
    /// </summary>
    public partial class WindowProductCreate : Window
    {
        private readonly PageProductMng pageProductMng;
        private readonly ApplicationDbContext _context;
        private Product? product;

        public WindowProductCreate(PageProductMng pageProductMng, ApplicationDbContext context, Product product)
        {
            InitializeComponent();
            this.pageProductMng = pageProductMng;
            _context = context;
            this.product = product;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxMCategory.ItemsSource = await _context.Categories.ToListAsync();
            comboBoxMCategory.DisplayMemberPath = "Name";
            comboBoxMCategory.SelectedValuePath = "Id";
            if (product != null)
            {
                txtBoxProductName.Text = product.Name;
                txtDescription.Text = product.Description;
                txtPrice.Text = product.Price.ToString();
                txtStock.Text = product.Stock.ToString();
                txtImg.Text = product.ImgUrl;
                comboBoxMCategory.SelectedValue = product.CategoryId;
                Title = "Update Product";
                btnCreate.Content = "Update";
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    var cateSelect = (Category)comboBoxMCategory.SelectedItem;
                    if (product == null)
                    {
                        Product addProd = new Product
                        {
                            Name = txtBoxProductName.Text,
                            CategoryId = cateSelect.Id,
                            Description = txtDescription.Text,
                            Price = double.Parse(txtPrice.Text),
                            Stock = int.Parse(txtStock.Text),
                            ImgUrl = txtImg.Text
                        };
                        _context.Products.Add(addProd);
                        await _context.SaveChangesAsync();
                        MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        product.Name = txtBoxProductName.Text;
                        product.CategoryId = cateSelect.Id;
                        product.Description = txtDescription.Text;
                        product.Price = double.Parse(txtPrice.Text);
                        product.Stock = int.Parse(txtStock.Text);
                        product.ImgUrl = txtImg.Text;
                        _context.Products.Update(product);
                        await _context.SaveChangesAsync();
                        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    pageProductMng.LoadData();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtBoxProductName.Text))
            {
                MessageBox.Show("Product name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (comboBoxMCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(txtPrice.Text, out double price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtImg.Text))
            {
                MessageBox.Show("Image URL cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
