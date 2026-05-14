using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaroVS.Models;

namespace TaroVS.Services
{
    public class ReliabilityStorageService
    {
        private readonly string _filePath = "Data/failures.json";

        public ReliabilityStorageService()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<SystemFailure> LoadFailures()
        {
            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<SystemFailure>>(json)
                   ?? new List<SystemFailure>();
        }

        public void SaveFailures(IEnumerable<SystemFailure> failures)
        {
            string json = JsonSerializer.Serialize(
                failures,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_filePath, json);
        }
    }
}