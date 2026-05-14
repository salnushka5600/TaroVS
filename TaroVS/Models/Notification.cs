using System;

namespace TaroVS.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Title { get; set; } = "";

        public string Message { get; set; } = "";

        public string Type { get; set; } = "";
    }
}