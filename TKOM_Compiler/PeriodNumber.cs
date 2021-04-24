using System;
using System.Collections.Generic;
using System.Text;

namespace TKOM_Compiler
{
    public class PeriodNumber
    {
        public double DoublePart { get; set; }
        public int Period { get; set; }
    
        public PeriodNumber(double doublepart, int period)
        {
            DoublePart = doublepart;
            Period = period;
        }
        public override string ToString()
        {
                return (DoublePart.ToString().Contains(',')? DoublePart.ToString() : DoublePart.ToString() + ",") + "(" + Period.ToString() + ")";
        }
    }

}
