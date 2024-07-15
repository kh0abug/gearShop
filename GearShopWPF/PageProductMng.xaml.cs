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
    /// Interaction logic for PageProductMng.xaml
    /// </summary>
    public partial class PageProductMng : Page
    {
        private readonly ApplicationDbContext _context;
        private Product? product;

        public PageProductMng(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();
            LoadData();
            listView.SelectionChanged += ListView_SelectionChanged;
        }

        public async void LoadData()
        {
            var products = await _context.Products.Include(c=> c.Category).OrderByDescending(p => p.Id).ToListAsync();
            listView.ItemsSource = products;
        }

        public void ClearInputs()
        {
            tbProductId.Text = "";
            tbProductName.Text = "";
            tbCategoryId.Text = "";
            tbDescription.Text = "";
            tbPrice.Text = "";
            tbStock.Text = "";
            listView.UnselectAll();
        }

        private async void Button_Clear(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            await LoadDataAsync();
        }

        private async void Button_Search(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = tbSearch.Text.Trim();
            if (string.IsNullOrEmpty(tbSearch.Text))
            {
                await LoadDataAsync();
            }
            else
            {
                var products = await _context.Products
                                             .Where(p => p.Name.Contains(tbSearch.Text))
                                             .ToListAsync();
                listView.ItemsSource = products;
            }
        }

        private async void Button_Reload(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {
                    // Show confirmation message
                    MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to delete {count} selected product(s)?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Multiple select
                        List<Product> selectedProducts = listView.SelectedItems.Cast<Product>().ToList();
                        foreach (Product selecProd in selectedProducts)
                        {
                            _context.Products.Remove(selecProd);
                        }
                        await _context.SaveChangesAsync();
                        await LoadDataAsync();
                        ClearInputs();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a product to delete", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to delete the product(s): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {
                    // Multiple select
                    List<Product> selectedProducts = listView.SelectedItems.Cast<Product>().ToList();
                    foreach (Product selecProd in selectedProducts)
                    {
                        WindowProductCreate windowProductCreate = new WindowProductCreate(this, _context, selecProd);
                        windowProductCreate.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a product to edit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to edit the member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            WindowProductCreate windowProductCreate = new WindowProductCreate(this, _context, null);
            windowProductCreate.ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                Product selectProduct = (Product)listView.SelectedItem;
                // Set the TextBox values with the selected member's properties
                tbProductId.Text = selectProduct.Id.ToString();
                tbProductName.Text = selectProduct.Name;
                tbCategoryId.Text = selectProduct.CategoryId.ToString();
                tbDescription.Text = selectProduct.Description;
                tbPrice.Text = selectProduct.Price.ToString();
                tbStock.Text = selectProduct.Stock.ToString();

                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                btnClear.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private async Task LoadDataAsync()
        {
            var products = await _context.Products.Include(c => c.Category).OrderByDescending(p => p.Id).ToListAsync();
            listView.ItemsSource = products;
        }
    }
}
