using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaroVS.Models;

namespace TaroVS.Services
{
    public class NotificationStorageService
    {
        private readonly string _filePath = "Data/notifications.json";

        public NotificationStorageService()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<Notification> LoadNotifications()
        {
            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Notification>>(json)
                   ?? new List<Notification>();
        }

        public void SaveNotifications(IEnumerable<Notification> notifications)
        {
            string json = JsonSerializer.Serialize(
                notifications,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_filePath, json);
        }
    }
}