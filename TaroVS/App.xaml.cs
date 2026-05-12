using System.Windows;
using TaroVS.ViewModels;

namespace TaroVS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            if (result == true && loginWindow.CurrentUser != null)
            {
                var mainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(loginWindow.CurrentUser)
                };

                MainWindow = mainWindow;

                ShutdownMode = ShutdownMode.OnMainWindowClose;

                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}