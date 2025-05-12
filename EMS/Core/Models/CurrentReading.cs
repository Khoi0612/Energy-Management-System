using System;

namespace EMS.Core.Models
{
    internal class CurrentReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal I_L1 { get; set; }
        public decimal I_L2 { get; set; }
        public decimal I_L3 { get; set; }
    }
}
