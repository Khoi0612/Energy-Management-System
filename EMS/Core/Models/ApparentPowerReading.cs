using System;

namespace EMS.Core.Models
{
    internal class ApparentPowerReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal VA_L1 { get; set; }
        public decimal VA_L2 { get; set; }
        public decimal VA_L3 { get; set; }
    }
}
