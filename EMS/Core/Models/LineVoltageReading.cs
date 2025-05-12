using System;

namespace EMS.Core.Models
{
    internal class LineVoltageReading
    {
        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal V_L12 { get; set; }
        public decimal V_L23 { get; set; }
        public decimal V_L31 { get; set; }
    }
}
