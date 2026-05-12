using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TaroVS.Commands;
using TaroVS.Models;
using TaroVS.Services;

namespace TaroVS.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly OrderStorageService _orderStorage =
            new OrderStorageService();

        public User CurrentUser { get; set; }

        public bool IsAdmin => CurrentUser != null && CurrentUser.Role == "Admin";
        public bool IsClient => CurrentUser != null && CurrentUser.Role == "Client";

        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Customer> Customers { get; set; } = new();
        public ObservableCollection<Order> Orders { get; set; } = new();
        public ObservableCollection<CartItem> Cart { get; set; } = new();

        public ObservableCollection<string> PaymentMethods { get; set; } = new()
        {
            "Карта",
            "Наличные",
            "Онлайн"
        };

        public ObservableCollection<string> DeliveryMethods { get; set; } = new()
        {
            "Самовывоз",
            "Курьер"
        };

        public ObservableCollection<string> OrderStatuses { get; set; } = new()
        {
            "Новый",
            "В сборке",
            "Отправлен",
            "Закрыт",
            "Отмена"
        };

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                Changed();
            }
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                Changed();
            }
        }

        private CartItem _selectedCartItem;
        public CartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                Changed();
            }
        }

        private string _newProductName = "";
        public string NewProductName
        {
            get => _newProductName;
            set
            {
                _newProductName = value;
                Changed();
            }
        }

        private string _newProductCategory = "Таро";
        public string NewProductCategory
        {
            get => _newProductCategory;
            set
            {
                _newProductCategory = value;
                Changed();
            }
        }

        private string _newProductPublisher = "";
        public string NewProductPublisher
        {
            get => _newProductPublisher;
            set
            {
                _newProductPublisher = value;
                Changed();
            }
        }

        private decimal _newProductPrice = 2500;
        public decimal NewProductPrice
        {
            get => _newProductPrice;
            set
            {
                _newProductPrice = value;
                Changed();
            }
        }

        private int _newProductStock = 1;
        public int NewProductStock
        {
            get => _newProductStock;
            set
            {
                _newProductStock = value;
                Changed();
            }
        }

        private string _clientName = "";
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                Changed();
            }
        }

        private string _clientPhone = "";
        public string ClientPhone
        {
            get => _clientPhone;
            set
            {
                _clientPhone = value;
                Changed();
            }
        }

        private string _clientEmail = "";
        public string ClientEmail
        {
            get => _clientEmail;
            set
            {
                _clientEmail = value;
                Changed();
            }
        }

        private string _clientComment = "";
        public string ClientComment
        {
            get => _clientComment;
            set
            {
                _clientComment = value;
                Changed();
            }
        }

        private string _selectedPayment = "Карта";
        public string SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                _selectedPayment = value;
                Changed();
            }
        }

        private string _selectedDelivery = "Самовывоз";
        public string SelectedDelivery
        {
            get => _selectedDelivery;
            set
            {
                _selectedDelivery = value;
                Changed();
            }
        }

        private string _selectedNextStatus = "В сборке";
        public string SelectedNextStatus
        {
            get => _selectedNextStatus;
            set
            {
                _selectedNextStatus = value;
                Changed();
            }
        }

        private string _dashboardText = "";
        public string DashboardText
        {
            get => _dashboardText;
            set
            {
                _dashboardText = value;
                Changed();
            }
        }

        private string _reportText = "";
        public string ReportText
        {
            get => _reportText;
            set
            {
                _reportText = value;
                Changed();
            }
        }

        public RelayCommand SeedDemoCommand { get; set; }
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand DeleteProductCommand { get; set; }
        public RelayCommand AddToCartCommand { get; set; }
        public RelayCommand RemoveFromCartCommand { get; set; }
        public RelayCommand CreateClientOrderCommand { get; set; }
        public RelayCommand ChangeOrderStatusCommand { get; set; }
        public RelayCommand CancelOrderCommand { get; set; }
        public RelayCommand BuildReportCommand { get; set; }

        private int _productId = 1;
        private int _customerId = 1;
        private int _orderId = 1;

        public MainViewModel()
            : this(new User
            {
                Id = 1,
                Login = "admin",
                Password = "admin",
                Role = "Admin"
            })
        {
        }

        public MainViewModel(User user)
        {
            CurrentUser = user;

            SeedDemoCommand = new RelayCommand(_ => SeedDemo());
            AddProductCommand = new RelayCommand(_ => AddProduct());
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct());
            AddToCartCommand = new RelayCommand(p => AddToCart(p));
            RemoveFromCartCommand = new RelayCommand(_ => RemoveFromCart());
            CreateClientOrderCommand = new RelayCommand(_ => CreateClientOrder());
            ChangeOrderStatusCommand = new RelayCommand(_ => ChangeOrderStatus());
            CancelOrderCommand = new RelayCommand(_ => CancelOrder());
            BuildReportCommand = new RelayCommand(_ => BuildReport());

            SeedDemo();
            LoadSavedOrders();
        }

        private void SeedDemo()
        {
            Products.Clear();
            Customers.Clear();
            Cart.Clear();

            _productId = 1;
            _customerId = 1;
            _orderId = 1;

            Products.Add(new Product
            {
                Id = _productId++,
                Name = "Rider Waite Tarot",
                Category = "Таро",
                Publisher = "US Games",
                Price = 2490,
                Stock = 10
            });

            Products.Add(new Product
            {
                Id = _productId++,
                Name = "Thoth Tarot",
                Category = "Таро",
                Publisher = "AGM",
                Price = 3190,
                Stock = 7
            });

            Products.Add(new Product
            {
                Id = _productId++,
                Name = "Oracle of Visions",
                Category = "Оракул",
                Publisher = "Blue Angel",
                Price = 2890,
                Stock = 5
            });

            Changed(nameof(Products));
            Changed(nameof(Customers));
            Changed(nameof(Cart));

            UpdateDashboard();
            BuildReport();
        }

        private void LoadSavedOrders()
        {
            Orders.Clear();
            Customers.Clear();

            var savedOrders = _orderStorage.LoadOrders();

            foreach (var order in savedOrders)
            {
                Orders.Add(order);

                if (order.Customer != null &&
                    !Customers.Any(c => c.Id == order.Customer.Id))
                {
                    Customers.Add(order.Customer);
                }
            }

            if (Orders.Any())
                _orderId = Orders.Max(o => o.Id) + 1;

            if (Customers.Any())
                _customerId = Customers.Max(c => c.Id) + 1;

            Changed(nameof(Orders));
            Changed(nameof(Customers));

            UpdateDashboard();
            BuildReport();
        }

        private void SaveOrders()
        {
            _orderStorage.SaveOrders(Orders);
        }

        private void AddProduct()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Добавлять товары может только администратор.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewProductName))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            Products.Add(new Product
            {
                Id = _productId++,
                Name = NewProductName,
                Category = NewProductCategory,
                Publisher = NewProductPublisher,
                Price = NewProductPrice,
                Stock = NewProductStock
            });

            NewProductName = "";
            NewProductCategory = "Таро";
            NewProductPublisher = "";
            NewProductPrice = 2500;
            NewProductStock = 1;

            Changed(nameof(Products));
            Changed(nameof(NewProductName));
            Changed(nameof(NewProductCategory));
            Changed(nameof(NewProductPublisher));
            Changed(nameof(NewProductPrice));
            Changed(nameof(NewProductStock));

            UpdateDashboard();
        }

        private void DeleteProduct()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Удалять товары может только администратор.");
                return;
            }

            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            Products.Remove(SelectedProduct);
            SelectedProduct = null;

            Changed(nameof(Products));
            Changed(nameof(SelectedProduct));

            UpdateDashboard();
        }

        private void AddToCart(object parameter)
        {
            Product product = parameter as Product ?? SelectedProduct;

            if (product == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            if (product.Stock <= 0)
            {
                MessageBox.Show("Товара нет в наличии.");
                return;
            }

            var existing = Cart.FirstOrDefault(x =>
                x.Product.Id == product.Id);

            if (existing != null)
            {
                if (existing.Quantity >= product.Stock)
                {
                    MessageBox.Show("Недостаточно товара на складе.");
                    return;
                }

                existing.Quantity++;
                Changed(nameof(Cart));
            }
            else
            {
                Cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }

            Changed(nameof(Cart));
        }

        private void RemoveFromCart()
        {
            if (SelectedCartItem == null)
            {
                MessageBox.Show("Выберите товар в корзине.");
                return;
            }

            Cart.Remove(SelectedCartItem);
            SelectedCartItem = null;

            Changed(nameof(Cart));
            Changed(nameof(SelectedCartItem));
        }

        private void CreateClientOrder()
        {
            if (!Cart.Any())
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ClientName))
            {
                MessageBox.Show("Введите имя клиента.");
                return;
            }

            var customer = new Customer
            {
                Id = _customerId++,
                FullName = ClientName,
                Phone = ClientPhone,
                Email = ClientEmail
            };

            Customers.Add(customer);

            foreach (var item in Cart)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    MessageBox.Show($"Недостаточно товара: {item.Product.Name}");
                    return;
                }

                item.Product.Stock -= item.Quantity;

                Orders.Add(new Order
                {
                    Id = _orderId++,
                    CreatedAt = DateTime.Now,
                    Customer = customer,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Status = "Новый",
                    Payment = SelectedPayment,
                    Delivery = SelectedDelivery,
                    Total = item.Total,
                    Comment = ClientComment
                });
            }

            SaveOrders();

            Cart.Clear();

            ClientName = "";
            ClientPhone = "";
            ClientEmail = "";
            ClientComment = "";

            Changed(nameof(Orders));
            Changed(nameof(Customers));
            Changed(nameof(Cart));
            Changed(nameof(Products));
            Changed(nameof(ClientName));
            Changed(nameof(ClientPhone));
            Changed(nameof(ClientEmail));
            Changed(nameof(ClientComment));

            UpdateDashboard();
            BuildReport();

            MessageBox.Show("Заказ успешно оформлен. Он отправлен администратору.");
        }

        private void ChangeOrderStatus()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Изменять статус заказа может только администратор.");
                return;
            }

            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }

            SelectedOrder.Status = SelectedNextStatus;

            Orders = new ObservableCollection<Order>(Orders);

            SaveOrders();

            Changed(nameof(Orders));

            UpdateDashboard();
            BuildReport();
        }

        private void CancelOrder()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Отменять заказ может только администратор.");
                return;
            }

            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заказ.");
                return;
            }

            if (SelectedOrder.Status == "Отмена")
            {
                MessageBox.Show("Заказ уже отменён.");
                return;
            }

            if (SelectedOrder.Product != null)
            {
                SelectedOrder.Product.Stock += SelectedOrder.Quantity;
            }

            SelectedOrder.Status = "Отмена";

            Orders = new ObservableCollection<Order>(Orders);

            SaveOrders();

            Changed(nameof(Orders));
            Changed(nameof(Products));

            UpdateDashboard();
            BuildReport();

            MessageBox.Show("Заказ отменён.");
        }

        private void UpdateDashboard()
        {
            DashboardText =
                $"Пользователь: {CurrentUser.Login}\n" +
                $"Роль: {CurrentUser.Role}\n" +
                $"Товаров: {Products.Count}\n" +
                $"Клиентов: {Customers.Count}\n" +
                $"Заказов: {Orders.Count}\n" +
                $"Активных заказов: {Orders.Count(x => x.Status != "Закрыт" && x.Status != "Отмена")}";
        }

        private void BuildReport()
        {
            var notCanceledOrders = Orders.Where(x =>
                x.Status != "Отмена");

            var completedOrders = Orders.Where(x =>
                x.Status == "Закрыт" ||
                x.Status == "Отправлен");

            int totalOrders = Orders.Count;
            int newOrders = Orders.Count(x => x.Status == "Новый");
            int activeOrders = Orders.Count(x => x.Status == "Новый" || x.Status == "В сборке");
            int sentOrders = Orders.Count(x => x.Status == "Отправлен");
            int closedOrders = Orders.Count(x => x.Status == "Закрыт");
            int canceledOrders = Orders.Count(x => x.Status == "Отмена");

            decimal totalOrderSum = notCanceledOrders.Sum(x => x.Total);
            decimal completedRevenue = completedOrders.Sum(x => x.Total);

            int totalProductsSold = notCanceledOrders.Sum(x => x.Quantity);

            string topProduct = "нет данных";

            if (notCanceledOrders.Any())
            {
                topProduct = notCanceledOrders
                    .GroupBy(x => x.Product?.Name ?? "Без названия")
                    .Select(g => new
                    {
                        Name = g.Key,
                        Count = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.Count)
                    .First()
                    .Name;
            }

            ReportText =
                "ОТЧЁТ ПО МАГАЗИНУ TARO SHOP\n\n" +
                $"Всего заказов: {totalOrders}\n" +
                $"Новые заказы: {newOrders}\n" +
                $"Активные заказы: {activeOrders}\n" +
                $"Отправленные заказы: {sentOrders}\n" +
                $"Закрытые заказы: {closedOrders}\n" +
                $"Отменённые заказы: {canceledOrders}\n\n" +
                $"Клиентов в базе: {Customers.Count}\n" +
                $"Товаров в каталоге: {Products.Count}\n" +
                $"Товаров в заказах: {totalProductsSold}\n\n" +
                $"Сумма всех неотменённых заказов: {totalOrderSum} руб.\n" +
                $"Выручка по завершённым заказам: {completedRevenue} руб.\n" +
                $"Самый популярный товар: {topProduct}";

            Changed(nameof(ReportText));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Changed([CallerMemberName] string? prop = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(prop));
        }
    }
}