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
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            User? user = _authService.Login(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            CurrentUser = user;
            DialogResult = true;
        }
    }
}