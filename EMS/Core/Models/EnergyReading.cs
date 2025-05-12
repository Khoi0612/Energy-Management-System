using System;

namespace EMS.Core.Models
{
    internal class EnergyReading
    {
        //public int ID { get; set; }

        public DateTime Start_Timestamp { get; set; }

        public DateTime End_Timestamp { get; set; }

        public decimal I_L1 { get; set; }
        public decimal I_L2 { get; set; }
        public decimal I_L3 { get; set; }

        public decimal V_L1 { get; set; }
        public decimal V_L2 { get; set; }
        public decimal V_L3 { get; set; }

        public decimal V_L12 { get; set; }
        public decimal V_L23 { get; set; }
        public decimal V_L31 { get; set; }

        public decimal P_L1 { get; set; }
        public decimal P_L2 { get; set; }
        public decimal P_L3 { get; set; }

        public decimal VARQ1_L1 { get; set; }
        public decimal VARQ1_L2 { get; set; }
        public decimal VARQ1_L3 { get; set; }

        public decimal VA_L1 { get; set; }
        public decimal VA_L2 { get; set; }
        public decimal VA_L3 { get; set; }

        public decimal COS1_L1 { get; set; }
        public decimal COS1_L2 { get; set; }
        public decimal COS1_L3 { get; set; }


    }
}
