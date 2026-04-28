using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "Таро";
        public string Publisher { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; } = 2;
        public bool IsLowStock => Stock <= MinStock;
    }
}
