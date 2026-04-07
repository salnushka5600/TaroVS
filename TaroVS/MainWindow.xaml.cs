using System.Windows;
using TaroVS.Services;
using TaroVS.ViewModels;

namespace TaroVS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var configService = new ConfigService();
            var config = configService.LoadConfig();

            var dataService = new JsonDataService(config);

            DataContext = new MainViewModel(config, dataService);
        }
    }
}