using System;
using System.Collections.Generic;
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
        private readonly AppConfig _config;
        private readonly JsonDataService _dataService;

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<Order> Orders { get; } = new();
        public ObservableCollection<ChangeLogEntry> Changes { get; } = new();

        public ObservableCollection<string> OrderStatuses { get; } = new()
        {
            "Новый",
            "В сборке",
            "Ожидает оплаты",
            "Готов к выдаче",
            "Отправлен",
            "Закрыт",
            "Отмена"
        };

        public ObservableCollection<string> PaymentMethods { get; } = new()
        {
            "Карта",
            "Наличные",
            "Онлайн"
        };

        public ObservableCollection<string> DeliveryMethods { get; } = new()
        {
            "Самовывоз",
            "Доставка"
        };

        private int _productId = 1;
        private int _customerId = 1;
        private int _orderId = 1;
        private int _changeId = 1;

        public RelayCommand SeedDemoCommand { get; }
        public RelayCommand SaveDataCommand { get; }
        public RelayCommand LoadDataCommand { get; }
        public RelayCommand AddProductCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand AddCustomerCommand { get; }
        public RelayCommand AddOrderCommand { get; }
        public RelayCommand ChangeOrderStatusCommand { get; }
        public RelayCommand CancelOrderCommand { get; }
        public RelayCommand BuildReportCommand { get; }
        public RelayCommand AddChangeCommand { get; }

        public MainViewModel(AppConfig config, JsonDataService dataService)
        {
            _config = config;
            _dataService = dataService;

            SeedDemoCommand = new RelayCommand(_ => SeedDemo());
            SaveDataCommand = new RelayCommand(_ => SaveAllData());
            LoadDataCommand = new RelayCommand(_ => LoadAllData());

            AddProductCommand = new RelayCommand(_ => AddProduct());
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct != null);

            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            AddOrderCommand = new RelayCommand(_ => AddOrder(), _ => Customers.Count > 0 && Products.Count > 0);

            ChangeOrderStatusCommand = new RelayCommand(_ => ChangeOrderStatus(), _ => SelectedOrder != null);
            CancelOrderCommand = new RelayCommand(_ => CancelOrder(), _ => SelectedOrder != null);
            
            AddChangeCommand = new RelayCommand(_ => AddChange());

            BuildReportCommand = new RelayCommand(_ => BuildReport());

            LoadAllData();

            if (_config.SeedDemoOnFirstRun && Products.Count == 0 && Customers.Count == 0 && Orders.Count == 0)
            {
                SeedDemo();
            }
        }
       
        private void AddChange()
        {
            if (string.IsNullOrWhiteSpace(NewChangeAuthor))
            {
                MessageBox.Show("Введите автора изменения.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewChangeVersion))
            {
                MessageBox.Show("Введите версию документа.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewChangeDescription))
            {
                MessageBox.Show("Введите описание изменения.");
                return;
            }

            Changes.Insert(0, new ChangeLogEntry
            {
                Id = _changeId++,
                ChangeDate = NewChangeDate,
                Author = NewChangeAuthor.Trim(),
                Version = NewChangeVersion.Trim(),
                Description = NewChangeDescription.Trim()
            });

            NewChangeDate = DateTime.Today;
            NewChangeAuthor = "";
            NewChangeVersion = "";
            NewChangeDescription = "";

            OnPropertyChanged(nameof(NewChangeDate));
            OnPropertyChanged(nameof(NewChangeAuthor));
            OnPropertyChanged(nameof(NewChangeVersion));
            OnPropertyChanged(nameof(NewChangeDescription));

            AutoSave();
        }

        #region Selected Items

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set { _selectedOrder = value; OnPropertyChanged(); }
        }

        private Product? _selectedOrderProduct;
        public Product? SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set { _selectedOrderProduct = value; OnPropertyChanged(); }
        }

        #endregion

        #region Product Fields

        private string _newProductName = "";
        public string NewProductName
        {
            get => _newProductName;
            set { _newProductName = value; OnPropertyChanged(); }
        }

        private string _newProductCategory = "Таро";
        public string NewProductCategory
        {
            get => _newProductCategory;
            set { _newProductCategory = value; OnPropertyChanged(); }
        }

        private string _newProductPublisher = "";
        public string NewProductPublisher
        {
            get => _newProductPublisher;
            set { _newProductPublisher = value; OnPropertyChanged(); }
        }

        private decimal _newProductPrice = 2000;
        public decimal NewProductPrice
        {
            get => _newProductPrice;
            set { _newProductPrice = value; OnPropertyChanged(); }
        }

        private int _newProductStock = 1;
        public int NewProductStock
        {
            get => _newProductStock;
            set { _newProductStock = value; OnPropertyChanged(); }
        }

        #endregion

        #region Customer Fields

        private string _newCustomerName = "";
        public string NewCustomerName
        {
            get => _newCustomerName;
            set { _newCustomerName = value; OnPropertyChanged(); }
        }

        private string _newCustomerPhone = "";
        public string NewCustomerPhone
        {
            get => _newCustomerPhone;
            set { _newCustomerPhone = value; OnPropertyChanged(); }
        }

        private string _newCustomerEmail = "";
        public string NewCustomerEmail
        {
            get => _newCustomerEmail;
            set { _newCustomerEmail = value; OnPropertyChanged(); }
        }

        #endregion

        #region Order Fields

        private int _newOrderQuantity = 1;
        public int NewOrderQuantity
        {
            get => _newOrderQuantity;
            set { _newOrderQuantity = value; OnPropertyChanged(); }
        }

        private string _newOrderPayment = "Карта";
        public string NewOrderPayment
        {
            get => _newOrderPayment;
            set { _newOrderPayment = value; OnPropertyChanged(); }
        }

        private string _newOrderDelivery = "Самовывоз";
        public string NewOrderDelivery
        {
            get => _newOrderDelivery;
            set { _newOrderDelivery = value; OnPropertyChanged(); }
        }

        private string _newOrderComment = "";
        public string NewOrderComment
        {
            get => _newOrderComment;
            set { _newOrderComment = value; OnPropertyChanged(); }
        }

        private string _newOrderDocumentPath = "";
        public string NewOrderDocumentPath
        {
            get => _newOrderDocumentPath;
            set { _newOrderDocumentPath = value; OnPropertyChanged(); }
        }

        private string _selectedNextStatus = "В сборке";
        public string SelectedNextStatus
        {
            get => _selectedNextStatus;
            set { _selectedNextStatus = value; OnPropertyChanged(); }
        }

        #endregion

        #region Dashboard / Reports

        private string _dashboardText = "";
        public string DashboardText
        {
            get => _dashboardText;
            set { _dashboardText = value; OnPropertyChanged(); }
        }

        private DateTime _reportFrom = DateTime.Today.AddDays(-7);
        public DateTime ReportFrom
        {
            get => _reportFrom;
            set { _reportFrom = value; OnPropertyChanged(); }
        }

        private DateTime _reportTo = DateTime.Today;
        public DateTime ReportTo
        {
            get => _reportTo;
            set { _reportTo = value; OnPropertyChanged(); }
        }

        private string _reportText = "";
        public string ReportText
        {
            get => _reportText;
            set { _reportText = value; OnPropertyChanged(); }
        }

        #endregion

        #region Core Methods

        private void SeedDemo()
        {
            Products.Clear();
            Customers.Clear();
            Orders.Clear();

            _productId = 1;
            _customerId = 1;
            _orderId = 1;

            var p1 = new Product { Id = _productId++, Name = "Rider–Waite Tarot", Category = "Таро", Publisher = "US Games", Price = 2490, Stock = 12 };
            var p2 = new Product { Id = _productId++, Name = "Thoth Tarot", Category = "Таро", Publisher = "AGM", Price = 3190, Stock = 7 };
            var p3 = new Product { Id = _productId++, Name = "Oracle of Visions", Category = "Оракул", Publisher = "Blue Angel", Price = 2890, Stock = 5 };

            Products.Add(p1);
            Products.Add(p2);
            Products.Add(p3);

            var c1 = new Customer { Id = _customerId++, FullName = "Иванова Мария", Phone = "+7 900 000-00-00", Email = "maria@example.com" };
            var c2 = new Customer { Id = _customerId++, FullName = "Петров Артём", Phone = "+7 900 111-11-11", Email = "artem@example.com" };

            Customers.Add(c1);
            Customers.Add(c2);

            Orders.Add(new Order
            {
                Id = _orderId++,
                CreatedAt = DateTime.Now.AddHours(-2),
                Customer = c1,
                Product = p1,
                Quantity = 1,
                Status = "Закрыт",
                Payment = "Карта",
                Delivery = "Самовывоз",
                Total = 2490,
                Comment = "Демо-заказ",
                StockReserved = true
            });

            Orders.Add(new Order
            {
                Id = _orderId++,
                CreatedAt = DateTime.Now.AddMinutes(-40),
                Customer = c2,
                Product = p2,
                Quantity = 1,
                Status = "Новый",
                Payment = "Онлайн",
                Delivery = "Доставка",
                Total = 3190,
                Comment = "Демо-онлайн заказ",
                StockReserved = true
            });

            SaveAllData();
            UpdateDashboard();
            BuildReport();
        }

        private DateTime _newChangeDate = DateTime.Today;
        public DateTime NewChangeDate
        {
            get => _newChangeDate;
            set { _newChangeDate = value; OnPropertyChanged(); }
        }

        private string _newChangeAuthor = "";
        public string NewChangeAuthor
        {
            get => _newChangeAuthor;
            set { _newChangeAuthor = value; OnPropertyChanged(); }
        }

        private string _newChangeVersion = "";
        public string NewChangeVersion
        {
            get => _newChangeVersion;
            set { _newChangeVersion = value; OnPropertyChanged(); }
        }

        private string _newChangeDescription = "";
        public string NewChangeDescription
        {
            get => _newChangeDescription;
            set { _newChangeDescription = value; OnPropertyChanged(); }
        }

        private void LoadAllData()
        {
            Products.Clear();
            Customers.Clear();
            Orders.Clear();

            foreach (var p in _dataService.LoadProducts())
                Products.Add(p);

            foreach (var c in _dataService.LoadCustomers())
                Customers.Add(c);

            foreach (var o in _dataService.LoadOrders())
                Orders.Add(o);

            foreach (var ch in _dataService.LoadChanges())
                Changes.Add(ch);

            _productId = Products.Any() ? Products.Max(x => x.Id) + 1 : 1;
            _customerId = Customers.Any() ? Customers.Max(x => x.Id) + 1 : 1;
            _orderId = Orders.Any() ? Orders.Max(x => x.Id) + 1 : 1;
            _changeId = Changes.Any() ? Changes.Max(x => x.Id) + 1 : 1;

            UpdateDashboard();
            BuildReport();
        }

        private void SaveAllData()
        {
            _dataService.SaveProducts(Products.ToList());
            _dataService.SaveCustomers(Customers.ToList());
            _dataService.SaveOrders(Orders.ToList());
            _dataService.SaveChanges(Changes.ToList());
        }

        private void AutoSave()
        {
            if (_config.AutoSave)
                SaveAllData();
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
            NewProductCategory = "Таро";
            NewProductPublisher = "";
            NewProductPrice = 2000;
            NewProductStock = 1;

            OnPropertyChanged(nameof(NewProductName));
            OnPropertyChanged(nameof(NewProductCategory));
            OnPropertyChanged(nameof(NewProductPublisher));
            OnPropertyChanged(nameof(NewProductPrice));
            OnPropertyChanged(nameof(NewProductStock));

            AutoSave();
            UpdateDashboard();
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            Products.Remove(SelectedProduct);
            SelectedProduct = null;

            AutoSave();
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
            NewCustomerPhone = "";
            NewCustomerEmail = "";

            OnPropertyChanged(nameof(NewCustomerName));
            OnPropertyChanged(nameof(NewCustomerPhone));
            OnPropertyChanged(nameof(NewCustomerEmail));

            AutoSave();
            UpdateDashboard();
        }

        private void AddOrder()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Выберите клиента.");
                return;
            }

            if (SelectedOrderProduct == null)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }

            if (NewOrderQuantity <= 0)
            {
                MessageBox.Show("Количество должно быть больше 0.");
                return;
            }

            if (SelectedOrderProduct.Stock < NewOrderQuantity)
            {
                MessageBox.Show("Недостаточно товара на складе.");
                return;
            }

            SelectedOrderProduct.Stock -= NewOrderQuantity;

            var total = SelectedOrderProduct.Price * NewOrderQuantity;

            Orders.Insert(0, new Order
            {
                Id = _orderId++,
                CreatedAt = DateTime.Now,
                Customer = SelectedCustomer,
                Product = SelectedOrderProduct,
                Quantity = NewOrderQuantity,
                Status = "Новый",
                Payment = NewOrderPayment,
                Delivery = NewOrderDelivery,
                Total = total,
                Comment = NewOrderComment,
                DocumentPath = NewOrderDocumentPath,
                StockReserved = true
            });

            NewOrderQuantity = 1;
            NewOrderPayment = "Карта";
            NewOrderDelivery = "Самовывоз";
            NewOrderComment = "";
            NewOrderDocumentPath = "";
            SelectedOrderProduct = null;

            OnPropertyChanged(nameof(NewOrderQuantity));
            OnPropertyChanged(nameof(NewOrderPayment));
            OnPropertyChanged(nameof(NewOrderDelivery));
            OnPropertyChanged(nameof(NewOrderComment));
            OnPropertyChanged(nameof(NewOrderDocumentPath));
            OnPropertyChanged(nameof(SelectedOrderProduct));

            AutoSave();
            UpdateDashboard();
            BuildReport();
        }

        private void ChangeOrderStatus()
        {
            if (SelectedOrder == null) return;

            if (!CanChangeStatus(SelectedOrder.Status, SelectedNextStatus))
            {
                MessageBox.Show($"Переход из статуса «{SelectedOrder.Status}» в статус «{SelectedNextStatus}» недопустим.");
                return;
            }

            SelectedOrder.Status = SelectedNextStatus;

            AutoSave();
            OnPropertyChanged(nameof(Orders));
            UpdateDashboard();
            BuildReport();
        }

        private bool CanChangeStatus(string current, string next)
        {
            var map = new Dictionary<string, List<string>>
            {
                ["Новый"] = new() { "В сборке", "Ожидает оплаты", "Отмена" },
                ["В сборке"] = new() { "Ожидает оплаты", "Готов к выдаче", "Отмена" },
                ["Ожидает оплаты"] = new() { "В сборке", "Готов к выдаче", "Отмена" },
                ["Готов к выдаче"] = new() { "Отправлен", "Закрыт", "Отмена" },
                ["Отправлен"] = new() { "Закрыт" },
                ["Закрыт"] = new(),
                ["Отмена"] = new()
            };

            return map.ContainsKey(current) && map[current].Contains(next);
        }

        private void CancelOrder()
        {
            if (SelectedOrder == null) return;

            if (SelectedOrder.Status == "Отмена")
            {
                MessageBox.Show("Заказ уже отменён.");
                return;
            }

            if (SelectedOrder.Status == "Закрыт")
            {
                MessageBox.Show("Закрытый заказ отменить нельзя.");
                return;
            }

            if (SelectedOrder.StockReserved && SelectedOrder.Product != null)
            {
                SelectedOrder.Product.Stock += SelectedOrder.Quantity;
            }

            SelectedOrder.Status = "Отмена";
            SelectedOrder.StockReserved = false;

            AutoSave();
            OnPropertyChanged(nameof(Orders));
            UpdateDashboard();
            BuildReport();
        }

        private void UpdateDashboard()
        {
            var today = DateTime.Today;
            var todayOrders = Orders.Count(o => o.CreatedAt.Date == today);
            var newOrders = Orders.Count(o => o.Status == "Новый");
            var activeOrders = Orders.Count(o => o.Status != "Закрыт" && o.Status != "Отмена");
            var lowStock = Products.Count(p => p.Stock <= 2);

            DashboardText =
                $"Система: {_config.SystemName}\n" +
                $"Режим хранения: {_config.StorageMode}\n\n" +
                $"Заказы сегодня: {todayOrders}\n" +
                $"Новые заказы: {newOrders}\n" +
                $"Активные заказы: {activeOrders}\n" +
                $"Товаров всего: {Products.Count}\n" +
                $"Товаров с низким остатком (≤2): {lowStock}\n" +
                $"Клиентов в базе: {Customers.Count}";
        }

        private void BuildReport()
        {
            var from = ReportFrom.Date;
            var to = ReportTo.Date.AddDays(1).AddTicks(-1);

            var completedOrders = Orders.Where(o =>
                o.CreatedAt >= from &&
                o.CreatedAt <= to &&
                (o.Status == "Закрыт" || o.Status == "Отправлен"));

            var count = completedOrders.Count();
            var revenue = completedOrders.Sum(o => o.Total);

            var topProducts = completedOrders
                .GroupBy(o => o.Product?.Name ?? "Без товара")
                .Select(g => new { Name = g.Key, Qty = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Qty)
                .Take(3)
                .ToList();

            var topText = topProducts.Any()
                ? string.Join("\n", topProducts.Select(t => $"• {t.Name}: {t.Qty} шт."))
                : "Нет данных";

            ReportText =
                $"Период: {ReportFrom:dd.MM.yyyy} – {ReportTo:dd.MM.yyyy}\n" +
                $"Заказов (закрыт/отправлен): {count}\n" +
                $"Выручка: {revenue:N2} руб.\n\n" +
                $"Топ товаров:\n{topText}";
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}