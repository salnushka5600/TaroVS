using System;
using System.Collections.Generic;
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
        public string Status { get; set; } = "Новый";
        public string Payment { get; set; } = "Карта";
        public string Delivery { get; set; } = "Самовывоз";
        public decimal Total { get; set; }
    }
}
