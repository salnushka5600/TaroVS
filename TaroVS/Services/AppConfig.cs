using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaroVS.Services
{
    public class AppConfig
    {
        public string DataFolder { get; set; } = "Data";
        public string BackupFolder { get; set; } = "Backups";
        public bool AutoSave { get; set; } = true;
        public bool SeedDemoOnFirstRun { get; set; } = true;
        public string SystemName { get; set; } = "Taro Shop MVP";
        public string StorageMode { get; set; } = "JSON";
    }
}
