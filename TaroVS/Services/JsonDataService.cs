using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaroVS.Models;

namespace TaroVS.Services
{
    public class JsonDataService
    {
        private readonly AppConfig _config;

        public JsonDataService(AppConfig config)
        {
            _config = config;

            if (!Directory.Exists(_config.DataFolder))
                Directory.CreateDirectory(_config.DataFolder);

            if (!Directory.Exists(_config.BackupFolder))
                Directory.CreateDirectory(_config.BackupFolder);
        }

        private string ProductsFile => Path.Combine(_config.DataFolder, "products.json");
        private string CustomersFile => Path.Combine(_config.DataFolder, "customers.json");
        private string OrdersFile => Path.Combine(_config.DataFolder, "orders.json");
        private string ChangesFile => Path.Combine(_config.DataFolder, "changes.json");

        public void SaveProducts(List<Product> products)
        {
            SaveToFile(ProductsFile, products);
        }

        public void SaveCustomers(List<Customer> customers)
        {
            SaveToFile(CustomersFile, customers);
        }

        public void SaveOrders(List<Order> orders)
        {
            SaveToFile(OrdersFile, orders);
        }

        public void SaveChanges(List<ChangeLogEntry> changes)
        {
            SaveToFile(ChangesFile, changes);
        }

        public List<Product> LoadProducts()
        {
            return LoadFromFile<Product>(ProductsFile);
        }

        public List<Customer> LoadCustomers()
        {
            return LoadFromFile<Customer>(CustomersFile);
        }

        public List<Order> LoadOrders()
        {
            return LoadFromFile<Order>(OrdersFile);
        }

        public List<ChangeLogEntry> LoadChanges()
        {
            return LoadFromFile<ChangeLogEntry>(ChangesFile);
        }

        private void SaveToFile<T>(string path, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }

        private List<T> LoadFromFile<T>(string path)
        {
            if (!File.Exists(path))
                return new List<T>();

            var json = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<List<T>>(json);
            return result ?? new List<T>();
        }
    }
}