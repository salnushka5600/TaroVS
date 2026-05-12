using System.Windows;
using TaroVS.ViewModels;

namespace TaroVS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true &&
                loginWindow.CurrentUser != null)
            {
                var mainWindow = new MainWindow();

                mainWindow.DataContext =
                    new MainViewModel(loginWindow.CurrentUser);

                MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}