using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using TaroVS.Models;
using TaroVS.Commands;
using TaroVS.Services;

namespace TaroVS.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private AppConfig config;
        private JsonDataService json;


        public ObservableCollection<Product> Products { get; set; } = new();

        public ObservableCollection<Customer> Customers { get; set; } = new();

        public ObservableCollection<Order> Orders { get; set; } = new();

        public ObservableCollection<ChangeLogEntry> ChangeLog { get; set; } = new();



        public ObservableCollection<string> OrderStatuses { get; set; } = new()
        {
            "Новый",
            "В сборке",
            "Ожидает оплаты",
            "Готов к выдаче",
            "Отправлен",
            "Закрыт",
            "Отмена"
        };



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


        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                Changed();
            }
        }


        private Product _selectedOrderProduct;
        public Product SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set
            {
                _selectedOrderProduct = value;
                Changed();
            }
        }



        public string SelectedNextStatus { get; set; } = "В сборке";


        public string DashboardText { get; set; }


        private string _reportText;
        public string ReportText
        {
            get => _reportText;
            set
            {
                _reportText = value;
                Changed();
            }
        }



        public int NewOrderQuantity { get; set; } = 1;

        public string NewOrderPayment { get; set; } = "Карта";

        public string NewOrderDelivery { get; set; } = "Самовывоз";

        public string NewOrderComment { get; set; }

        public string NewOrderDocumentPath { get; set; }



        public RelayCommand AddOrderCommand { get; }
        public RelayCommand ChangeOrderStatusCommand { get; }
        public RelayCommand CancelOrderCommand { get; }
        public RelayCommand BuildReportCommand { get; }
        public RelayCommand SaveDataCommand { get; }
        public RelayCommand LoadDataCommand { get; }
        public RelayCommand SeedDemoCommand { get; }



        public MainViewModel()
        {
            config = new AppConfig();

            json = new JsonDataService(config);


            AddOrderCommand =
                new RelayCommand(_ => AddOrder());

            ChangeOrderStatusCommand =
                new RelayCommand(_ => ChangeOrderStatus());

            CancelOrderCommand =
                new RelayCommand(_ => CancelOrder());

            BuildReportCommand =
                new RelayCommand(_ => BuildReport());

            SaveDataCommand =
                new RelayCommand(_ => Save());

            LoadDataCommand =
                new RelayCommand(_ => Load());

            SeedDemoCommand =
                new RelayCommand(_ => SeedDemo());


            SeedDemo();
        }



        private void SeedDemo()
        {
            Products.Clear();
            Customers.Clear();
            Orders.Clear();


            var p1 = new Product
            {
                Id = 1,
                Name = "Rider Waite",
                Category = "Таро",
                Publisher = "US Games",
                Price = 2500,
                Stock = 10
            };

            var p2 = new Product
            {
                Id = 2,
                Name = "Thoth Tarot",
                Category = "Таро",
                Publisher = "AGM",
                Price = 3200,
                Stock = 7
            };


            Products.Add(p1);
            Products.Add(p2);


            var c1 = new Customer
            {
                Id = 1,
                FullName = "Иванова Мария",
                Phone = "999999"
            };

            Customers.Add(c1);


            Orders.Add(
                new Order
                {
                    Id = 1,
                    Customer = c1,
                    Product = p1,
                    Quantity = 1,
                    Status = "Новый",
                    Total = 2500,
                    CreatedAt = DateTime.Now
                });


            UpdateDashboard();

            BuildReport();
        }



        private void AddOrder()
        {
            if (SelectedCustomer == null ||
               SelectedOrderProduct == null)
                return;


            if (SelectedOrderProduct.Stock <
               NewOrderQuantity)
                return;


            SelectedOrderProduct.Stock -=
                NewOrderQuantity;


            Orders.Add(
                new Order
                {
                    Id = Orders.Count + 1,
                    Customer = SelectedCustomer,
                    Product = SelectedOrderProduct,
                    Quantity = NewOrderQuantity,
                    Status = "Новый",
                    Payment = NewOrderPayment,
                    Delivery = NewOrderDelivery,
                    Comment = NewOrderComment,
                    DocumentPath = NewOrderDocumentPath,
                    CreatedAt = DateTime.Now,
                    Total =
                     SelectedOrderProduct.Price *
                     NewOrderQuantity
                });


            Log("Заказ", "Создан");

            Save();

            UpdateDashboard();

            BuildReport();
        }



        private void ChangeOrderStatus()
        {
            if (SelectedOrder == null)
                return;


            string old =
                SelectedOrder.Status;


            SelectedOrder.History.Add(
                new OrderStatusHistory
                {
                    ChangeDate = DateTime.Now,
                    OldStatus = old,
                    NewStatus = SelectedNextStatus,
                    UserName = "Менеджер"
                });


            SelectedOrder.Status =
                SelectedNextStatus;


            Log(
                "Заказ",
                $"Статус {old}->{SelectedNextStatus}"
            );


            Save();

            BuildReport();
        }




        private void CancelOrder()
        {
            if (SelectedOrder == null)
                return;


            SelectedOrder.Product.Stock +=
                SelectedOrder.Quantity;


            SelectedOrder.Status = "Отмена";


            Log("Заказ", "Отменен");


            Save();

            BuildReport();
        }




        private void BuildReport()
        {
            var done =
                Orders.Where(x =>
                    x.Status == "Закрыт" ||
                    x.Status == "Отправлен");


            ReportText =
               $"Заказов: {done.Count()}\n" +
               $"Выручка: {done.Sum(x => x.Total)}";
        }



        private void UpdateDashboard()
        {
            DashboardText =
              $"Товаров: {Products.Count}\n" +
              $"Клиентов: {Customers.Count}\n" +
              $"Заказов: {Orders.Count}";
        }



        private void Log(
            string obj,
            string action)
        {
            ChangeLog.Add(
                new ChangeLogEntry
                {
                    Date = DateTime.Now,
                    ObjectType = obj,
                    Action = action,
                    UserName = "admin"
                });
        }



        private void Save()
        {
            json.SaveProducts(
                Products.ToList());

            json.SaveCustomers(
                Customers.ToList());

            json.SaveOrders(
                Orders.ToList());

            json.SaveChanges(
                ChangeLog.ToList());
        }



        private void Load()
        {
            Products = new ObservableCollection<Product>(
                    json.LoadProducts());

            Customers = new ObservableCollection<Customer>(
                    json.LoadCustomers());

            Orders = new ObservableCollection<Order>(
                    json.LoadOrders());

            ChangeLog =
                new ObservableCollection<ChangeLogEntry>(
                    json.LoadChanges());

            Changed(nameof(Products));
            Changed(nameof(Customers));
            Changed(nameof(Orders));
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void Changed(
         [CallerMemberName] string p = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(p));
        }

    }
}