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
        }

        private string ProductsFile =>
            Path.Combine(_config.DataFolder, "products.json");

        private string CustomersFile =>
            Path.Combine(_config.DataFolder, "customers.json");

        private string OrdersFile =>
            Path.Combine(_config.DataFolder, "orders.json");

        private string ChangesFile =>
            Path.Combine(_config.DataFolder, "changes.json");


        public List<Product> LoadProducts()
            => Load<Product>(ProductsFile);

        public List<Customer> LoadCustomers()
            => Load<Customer>(CustomersFile);

        public List<Order> LoadOrders()
            => Load<Order>(OrdersFile);

        public List<ChangeLogEntry> LoadChanges()
            => Load<ChangeLogEntry>(ChangesFile);



        public void SaveProducts(List<Product> data)
            => Save(ProductsFile, data);

        public void SaveCustomers(List<Customer> data)
            => Save(CustomersFile, data);

        public void SaveOrders(List<Order> data)
            => Save(OrdersFile, data);

        public void SaveChanges(List<ChangeLogEntry> data)
            => Save(ChangesFile, data);



        private List<T> Load<T>(string path)
        {
            if (!File.Exists(path))
                return new List<T>();

            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<T>>(json)
                   ?? new List<T>();
        }

        private void Save<T>(string path, List<T> data)
        {
            var json =
                JsonSerializer.Serialize(
                    data,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(path, json);
        }
    }
}