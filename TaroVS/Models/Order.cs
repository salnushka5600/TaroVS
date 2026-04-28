using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Новый";
        public string Payment { get; set; } = "Карта";
        public string Delivery { get; set; } = "Самовывоз";
        public decimal Total { get; set; }
        public string Comment { get; set; } = "";
        public string DocumentPath { get; set; } = "";
        public bool StockReserved { get; set; }

        public ObservableCollection<OrderStatusHistory> History { get; set; } = new();
    }
}
