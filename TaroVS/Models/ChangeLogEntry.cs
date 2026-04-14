using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TaroVS.Models
{
    public class ChangeLogEntry
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public string Author { get; set; } = "";
        public string Version { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
