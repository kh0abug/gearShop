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
using System.Windows.Shapes;

namespace GearShopWPF
{
    /// <summary>
    /// Interaction logic for WindowCategoryCreate.xaml
    /// </summary>
    public partial class WindowCategoryCreate : Window
    {
        private readonly PageCategoryMng pageCategoryMng;
        private readonly ApplicationDbContext _context;
        private Category? Category;
        public WindowCategoryCreate(PageCategoryMng pageCategoryMng, ApplicationDbContext context, Category category)
        {
            InitializeComponent();

            _context = context;
            this.pageCategoryMng = pageCategoryMng;
            this.Category = category;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
            if(Category != null)
            {
                txtCategoryName.Text = Category.Name;
                Title = "Update Category";
                btnCreate.Content = "Update";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ValidateInputs())
                {
                    if (Category == null)
                    {
                        Category addCate = new Category
                        {
                            Name = txtCategoryName.Text
                        };
                        _context.Categories.Add(addCate);
                    }
                    else
                    {
                        Category.Name = txtCategoryName.Text;
                    }
                    _context.SaveChanges();
                    pageCategoryMng.LoadData();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Category name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
