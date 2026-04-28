using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Models
{
    internal class ChangeLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ObjectType { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
    }
}
