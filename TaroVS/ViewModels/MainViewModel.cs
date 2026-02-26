using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TaroVS.Commands;
using TaroVS.Models;

namespace TaroVS.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<Order> Orders { get; } = new();

        private int _productId = 1;
        private int _customerId = 1;
        private int _orderId = 1;

        // Выбор
        private Product? _selectedProduct;
        public Product? SelectedProduct { get => _selectedProduct; set { _selectedProduct = value; OnPropertyChanged(); } }

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer { get => _selectedCustomer; set { _selectedCustomer = value; OnPropertyChanged(); } }

        private Order? _selectedOrder;
        public Order? SelectedOrder { get => _selectedOrder; set { _selectedOrder = value; OnPropertyChanged(); } }

        // Поля ввода: Товары
        public string NewProductName { get; set; } = "";
        public string NewProductCategory { get; set; } = "Таро";
        public string NewProductPublisher { get; set; } = "";
        public decimal NewProductPrice { get; set; } = 2000;
        public int NewProductStock { get; set; } = 1;

        // Поля ввода: Клиенты
        public string NewCustomerName { get; set; } = "";
        public string NewCustomerPhone { get; set; } = "";
        public string NewCustomerEmail { get; set; } = "";

        // Поля ввода: Заказ
        public decimal NewOrderTotal { get; set; } = 0;
        public string NewOrderStatus { get; set; } = "Новый";
        public string NewOrderPayment { get; set; } = "Карта";
        public string NewOrderDelivery { get; set; } = "Самовывоз";

        // Dashboard
        private string _dashboardText = "";
        public string DashboardText { get => _dashboardText; set { _dashboardText = value; OnPropertyChanged(); } }

        // Отчёты
        public DateTime ReportFrom { get; set; } = DateTime.Today.AddDays(-7);
        public DateTime ReportTo { get; set; } = DateTime.Today;
        private string _reportText = "";
        public string ReportText { get => _reportText; set { _reportText = value; OnPropertyChanged(); } }

        // Команды
        public RelayCommand SeedDemoCommand { get; }
        public RelayCommand AddProductCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand AddCustomerCommand { get; }
        public RelayCommand AddOrderCommand { get; }
        public RelayCommand CloseOrderCommand { get; }
        public RelayCommand BuildReportCommand { get; }

        public MainViewModel()
        {
            SeedDemoCommand = new RelayCommand(_ => SeedDemo());

            AddProductCommand = new RelayCommand(_ => AddProduct());
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct != null);

            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            AddOrderCommand = new RelayCommand(_ => AddOrder(), _ => Customers.Count > 0);

            CloseOrderCommand = new RelayCommand(_ => CloseOrder(), _ => SelectedOrder != null);

            BuildReportCommand = new RelayCommand(_ => BuildReport());

            SeedDemo();
        }

        private void SeedDemo()
        {
            Products.Clear();
            Customers.Clear();
            Orders.Clear();

            _productId = 1;
            _customerId = 1;
            _orderId = 1;

            Products.Add(new Product { Id = _productId++, Name = "Rider–Waite Tarot", Category = "Таро", Publisher = "US Games", Price = 2490, Stock = 12 });
            Products.Add(new Product { Id = _productId++, Name = "Thoth Tarot", Category = "Таро", Publisher = "AGM", Price = 3190, Stock = 7 });
            Products.Add(new Product { Id = _productId++, Name = "Oracle of Visions", Category = "Оракул", Publisher = "Blue Angel", Price = 2890, Stock = 5 });

            Customers.Add(new Customer { Id = _customerId++, FullName = "Иванова Мария", Phone = "+7 900 000-00-00", Email = "maria@example.com" });
            Customers.Add(new Customer { Id = _customerId++, FullName = "Петров Артём", Phone = "+7 900 111-11-11", Email = "artem@example.com" });

            Orders.Add(new Order { Id = _orderId++, CreatedAt = DateTime.Now.AddHours(-2), Customer = Customers[0], Status = "Закрыт", Payment = "Карта", Delivery = "Самовывоз", Total = 2490 });
            Orders.Add(new Order { Id = _orderId++, CreatedAt = DateTime.Now.AddMinutes(-40), Customer = Customers[1], Status = "Новый", Payment = "Онлайн", Delivery = "Доставка", Total = 3190 });

            UpdateDashboard();
            BuildReport();
        }

        private void AddProduct()
        {
            if (string.IsNullOrWhiteSpace(NewProductName))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            Products.Add(new Product
            {
                Id = _productId++,
                Name = NewProductName.Trim(),
                Category = NewProductCategory.Trim(),
                Publisher = NewProductPublisher.Trim(),
                Price = NewProductPrice,
                Stock = NewProductStock
            });

            NewProductName = "";
            OnPropertyChanged(nameof(NewProductName));

            UpdateDashboard();
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
            UpdateDashboard();
        }

        private void AddCustomer()
        {
            if (string.IsNullOrWhiteSpace(NewCustomerName))
            {
                MessageBox.Show("Введите ФИО клиента.");
                return;
            }

            Customers.Add(new Customer
            {
                Id = _customerId++,
                FullName = NewCustomerName.Trim(),
                Phone = NewCustomerPhone.Trim(),
                Email = NewCustomerEmail.Trim()
            });

            NewCustomerName = "";
            OnPropertyChanged(nameof(NewCustomerName));

            UpdateDashboard();
        }

        private void AddOrder()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Выберите клиента для заказа.");
                return;
            }

            if (NewOrderTotal <= 0)
            {
                MessageBox.Show("Сумма заказа должна быть больше 0.");
                return;
            }

            Orders.Insert(0, new Order
            {
                Id = _orderId++,
                CreatedAt = DateTime.Now,
                Customer = SelectedCustomer,
                Status = NewOrderStatus,
                Payment = NewOrderPayment,
                Delivery = NewOrderDelivery,
                Total = NewOrderTotal
            });

            NewOrderTotal = 0;
            OnPropertyChanged(nameof(NewOrderTotal));

            UpdateDashboard();
            BuildReport();
        }

        private void CloseOrder()
        {
            if (SelectedOrder == null) return;
            SelectedOrder.Status = "Закрыт";
            // для обновления DataGrid
            OnPropertyChanged(nameof(Orders));
            UpdateDashboard();
            BuildReport();
        }

        private void UpdateDashboard()
        {
            var today = DateTime.Today;
            var todayOrders = Orders.Count(o => o.CreatedAt.Date == today);
            var newOrders = Orders.Count(o => o.Status == "Новый");
            var lowStock = Products.Count(p => p.Stock <= 2);

            DashboardText =
                $"Заказы сегодня: {todayOrders}\n" +
                $"Новые заказы: {newOrders}\n" +
                $"Товаров (всего): {Products.Count}\n" +
                $"Товаров с низким остатком (≤2): {lowStock}\n" +
                $"Клиентов в базе: {Customers.Count}";
        }

        private void BuildReport()
        {
            var from = ReportFrom.Date;
            var to = ReportTo.Date.AddDays(1).AddTicks(-1);

            var closed = Orders.Where(o =>
                o.CreatedAt >= from &&
                o.CreatedAt <= to &&
                (o.Status == "Закрыт" || o.Status == "Отправлен"));

            var count = closed.Count();
            var revenue = closed.Sum(o => o.Total);

            ReportText =
                $"Период: {ReportFrom:dd.MM.yyyy} – {ReportTo:dd.MM.yyyy}\n" +
                $"Заказов (закрыт/отправлен): {count}\n" +
                $"Выручка: {revenue:N2} руб.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
