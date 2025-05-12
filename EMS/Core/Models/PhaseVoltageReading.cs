using System;

namespace EMS.Core.Models
{
    internal class PhaseVoltageReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal V_L1 { get; set; }
        public decimal V_L2 { get; set; }
        public decimal V_L3 { get; set; }

    }
}
