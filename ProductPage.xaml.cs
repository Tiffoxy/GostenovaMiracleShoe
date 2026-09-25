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

namespace GostenovaMiracleShow
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private int PageSize = 10;
        private List<Product> CurrentPageLIst = new List<Product>();
        private int currentPage = 1;
        private List<Product> _filtredProducts;
        private Client _currentUser;//для сохранения текущего юзера
        public ProductPage(Client user)
        {
            InitializeComponent();
            _currentUser = user;
            var currentProduct = GostenovaMiracleShowEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProduct;
            if (user != null)
            {
                FIOTB.Text = "Вы авторизованы как " + user.FirstName + " " +
                    user.LastName + " " + user.Patrynomic;
                switch (user.RoleID)
                {
                    case 1:
                        RoleTB.Text = "Роль: Администратор"; break;
                    case 2:
                        RoleTB.Text = "Роль: Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Роль: Клиент"; break;
                }
            }
            else
            {
                FIOTB.Text = "Вы вошли гость";
                RoleTB.Text = "Роль: Гость";
            }
            ComboCateghory.SelectedIndex = 0;
            ComboBoh.SelectedIndex = 0;
            UpdateProduct();
        }
        private void ChangePage()
        {
            PageListBox.Items.Clear();
            int totalPage = (_filtredProducts.Count + PageSize - 1) / PageSize;
            if (totalPage == 0) totalPage = 1;

            for (int i = 0; i <= totalPage; i++)
            {
                PageListBox.Items.Add(i);
            }
            PageListBox.SelectedItem = currentPage;

            var productPage = _filtredProducts
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            ProductListView.ItemsSource = productPage;
        }

        private void UpdateProduct()
        {
            var currentProduct = GostenovaMiracleShowEntities.GetContext().Product.ToList();

            if (!string.IsNullOrEmpty(TBSearch.Text))
            {
                string searchText = TBSearch.Text.ToLower();
                searchText = searchText.Replace("Ё", "Е").Replace("ё", "е");
                currentProduct = currentProduct.Where(p =>
                (p.ProductName != null && p.ProductName.ToLower().Contains(searchText)) ||
                (p.Category != null && p.CategoryTitle.ToLower().Contains(searchText)) ||
                (p.Manufacture != null && p.ManufactureTitle.ToLower().Contains(searchText)) ||
                (p.SubCategory != null && p.SubCategoryTitle.ToLower().Contains(searchText))
                ).ToList();

            }

            if (ComboBoh.SelectedIndex > 0)
            {
                string sortType = (ComboBoh.SelectedItem as TextBlock).Text;

                switch (sortType)
                {
                    case "по нименованию от А до я":
                        currentProduct = currentProduct.OrderBy(p => p.ProductName).ToList();
                        break;
                    case "по производителю по убыванию":
                        currentProduct = currentProduct.OrderBy(p => p.ManufactureTitle).ToList();
                        break;
                    case "По возрастнаию цены":
                        currentProduct = currentProduct.OrderBy(p => p.Price).ToList();
                        break;
                    case "По убыванию цены":
                        currentProduct = currentProduct.OrderByDescending(p => p.Price).ToList();
                        break;

                }
            }

            if (ComboCateghory.SelectedIndex == 1)
            {
                currentProduct = currentProduct.Where(p => p.CategoryTitle == "Детская обувь").ToList();
            }
            if (ComboCateghory.SelectedIndex == 2)
            {
                currentProduct = currentProduct.Where(p => p.CategoryTitle == "Женская обувь").ToList();
            }
            if (ComboCateghory.SelectedIndex == 3)
            {
                currentProduct = currentProduct.Where(p => p.CategoryTitle == "Мужская обувь").ToList();
            }

            _filtredProducts = currentProduct;
            currentPage = 1;
            ChangePage();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboCateghory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboBoh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void LeftDirBtn_Click(object sender, RoutedEventArgs e)
        {
            int totalPage = (_filtredProducts.Count - 1) / PageSize;
            if (currentPage > 1)
            {
                currentPage--;
                ChangePage();
            }
        }

        private void RightDirBtn_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (_filtredProducts.Count + PageSize - 1) / PageSize;
            if (currentPage < totalPages)
            {
                currentPage++;
                ChangePage();
            }
            
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PageListBox.SelectedItem != null)
            {
                int page = (int)PageListBox.SelectedItem;
                if (page != currentPage)
                {
                    currentPage = page;
                    ChangePage();
                }
            }
        }
    }
}
