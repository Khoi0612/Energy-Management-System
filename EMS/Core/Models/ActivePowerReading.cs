using System;

namespace EMS.Core.Models
{
    internal class ActivePowerReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal P_L1 { get; set; }
        public decimal P_L2 { get; set; }
        public decimal P_L3 { get; set; }
    }
}
