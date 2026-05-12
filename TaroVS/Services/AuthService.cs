using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaroVS.Models;

namespace TaroVS.Services
{
    public class AuthService
    {
        private readonly string _filePath = "Data/users.json";

        public AuthService()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(_filePath))
                CreateDefaultUsers();
        }

        private void CreateDefaultUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Login = "admin",
                    Password = "admin",
                    Role = "Admin"
                },

                new User
                {
                    Id = 2,
                    Login = "client",
                    Password = "client",
                    Role = "Client"
                }
            };

            SaveUsers(users);
        }

        public User? Login(string login, string password)
        {
            var users = LoadUsers();

            return users.FirstOrDefault(u =>
                u.Login == login &&
                u.Password == password);
        }

        private List<User> LoadUsers()
        {
            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<User>>(json)
                   ?? new List<User>();
        }

        private void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(
                users,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_filePath, json);
        }
    }
}