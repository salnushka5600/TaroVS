using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Models
{
    public class OrderStatusHistory
    {
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public string OldStatus { get; set; } = "";
        public string NewStatus { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}
