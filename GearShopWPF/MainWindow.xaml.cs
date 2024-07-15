using Infrastructure.Persistence;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;
        public MainWindow(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();
        }


        private void Goto_AdminProductManager(object sender, MouseButtonEventArgs e)
        {
            PageProductMng pageProductMng = new PageProductMng(_context);
            frameAdmin.Content = pageProductMng;
        }
        private void Goto_AdminCategoryManager(object sender, MouseButtonEventArgs e)
        {
            PageCategoryMng pageCategoryMng = new PageCategoryMng(_context);
            frameAdmin.Content = pageCategoryMng;

        }

    }
}