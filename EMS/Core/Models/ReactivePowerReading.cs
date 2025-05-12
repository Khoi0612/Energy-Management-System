using System;

namespace EMS.Core.Models
{
    internal class ReactivePowerReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal VARQ1_L1 { get; set; }
        public decimal VARQ1_L2 { get; set; }
        public decimal VARQ1_L3 { get; set; }
    }
}
