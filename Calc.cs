using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCalculator
{
    class Calc
    {
        public List<String> history = new List<string>();

        public bool IsComma;
        public bool IsClickSign;
        public bool IsNum1;
        public bool IsEqually;
        public bool IsX2;

        public double num1;
        public double num2;
        public double summ;

        public int countZnak;

        public char znak;
        public char symbolKey;


        public Calc() {
            IsComma = false;
            IsClickSign = false;
            IsNum1 = false;
            IsEqually = false;
            IsX2 = false;
            countZnak = 0;           
        }
    }
}
