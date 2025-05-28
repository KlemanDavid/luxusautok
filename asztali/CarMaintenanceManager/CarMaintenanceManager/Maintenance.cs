using System;

namespace CarMaintenanceManager
{
    public class Maintenance
    {
        public int      Id        { get; set; }
        public int      CarId     { get; set; }
        public DateTime Date      { get; set; }
        public string   Operation { get; set; } = string.Empty;
        public string   Note      { get; set; } = string.Empty;
    }
}