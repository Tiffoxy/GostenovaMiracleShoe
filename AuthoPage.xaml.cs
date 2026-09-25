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
    /// Логика взаимодействия для AuthoPage.xaml
    /// </summary>
    public partial class AuthoPage : Page
    {
        public AuthoPage()
        {
            InitializeComponent();
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вошли как гость");
            Manager.MainFrame.Navigate(new ProductPage(null));
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = TBLogin.Text;
            if (login == "")
            {
                MessageBox.Show("есть пустые поля");
                return;
            }
            Client user = GostenovaMiracleShowEntities.GetContext().Client.ToList()
                .Find(p => p.Login == login);
            if (user != null)
            {
                TBLogin.Text = "";
                MessageBox.Show("Вы успешно вошли в аkaунт");
                Manager.MainFrame.Navigate(new ProductPage(user));                
            }
            else
            {
                MessageBox.Show("неверно введены данные! проверьте еще раз");
                return;
            }
        }
    }
}
