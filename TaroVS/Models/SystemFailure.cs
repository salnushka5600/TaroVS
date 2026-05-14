using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Models
{
    public class SystemFailure
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Module { get; set; } = "";

        public string Description { get; set; } = "";

        public string Criticality { get; set; } = "";
    }
}
