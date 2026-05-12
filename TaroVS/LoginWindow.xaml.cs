using System.Windows;
using TaroVS.Models;
using TaroVS.Services;

namespace TaroVS
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public User? CurrentUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;

            var user = _authService.Login(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            CurrentUser = user;
            DialogResult = true;
            Close();
        }
    }
}