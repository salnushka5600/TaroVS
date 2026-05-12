using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaroVS.Models;

namespace TaroVS.Services
{
    public class OrderStorageService
    {
        private readonly string _filePath = "Data/orders.json";

        public OrderStorageService()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<Order> LoadOrders()
        {
            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Order>>(json)
                   ?? new List<Order>();
        }

        public void SaveOrders(IEnumerable<Order> orders)
        {
            var json = JsonSerializer.Serialize(
                orders,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_filePath, json);
        }
    }
}