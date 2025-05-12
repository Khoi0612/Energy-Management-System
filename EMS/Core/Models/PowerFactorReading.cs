using System;

namespace EMS.Core.Models
{
    internal class PowerFactorReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal COS1_L1 { get; set; }
        public decimal COS1_L2 { get; set; }
        public decimal COS1_L3 { get; set; }
    }
}
