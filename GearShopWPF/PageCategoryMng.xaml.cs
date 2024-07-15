using Domain.Entities;
using Infrastructure.Persistence;
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

namespace GearShopWPF
{
    /// <summary>
    /// Interaction logic for PageCategoryMng.xaml
    /// </summary>
    public partial class PageCategoryMng : Page
    {
        private readonly ApplicationDbContext _context;
        private Category? category;
        public PageCategoryMng(ApplicationDbContext context)
        {
            _context = context;
            this.category = category;
            InitializeComponent();
         
            LoadData();
            listView.SelectionChanged += ListView_SelectionChanged;
        }

        public void LoadData()
        {
            var categories = _context.Categories.ToList();
            listView.ItemsSource = categories;
        }

        public void ClearInputs()
        {
           tbCategoryId.Text = "";
            tbCategoryName.Text = "";
            tbSearchName.Text = "";
            listView.UnselectAll();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            LoadData();

        }

        private void Button_Delete(object sender, RoutedEventArgs e)
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
                        List<Category> selectedProducts = listView.SelectedItems.Cast<Category>().ToList();
                        foreach (Category selecProd in selectedProducts)
                        {
                            _context.Categories.Remove(selecProd);
                        }
                        _context.SaveChanges();
                        LoadData();
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

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {

                    //mutiple select
                    List<Category> selectedCategory = listView.SelectedItems.Cast<Category>().ToList();
                    foreach (Category selecCate in selectedCategory)
                    {
                        WindowCategoryCreate windowCategoryCreate = new WindowCategoryCreate(this, _context, selecCate);
                        windowCategoryCreate.ShowDialog();
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
            WindowCategoryCreate windowCategoryCreate = new WindowCategoryCreate(this, _context, null);
            windowCategoryCreate.ShowDialog();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            string search = tbSearchName.Text;
            listView.ItemsSource = _context.Categories.Where(c => c.Name.Contains(search)).ToList();

        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            LoadData();

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                Category selectProduct = (Category)listView.SelectedItem;
                // Set the TextBox values with the selected member's properties
                tbCategoryId.Text = selectProduct.Id.ToString();
                tbCategoryName.Text = selectProduct.Name;


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
    }
}
