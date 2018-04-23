using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Matek
    {
        public Matek() { }
        public static int LNKO(int a, int b)
        {

            if (a < 0) a = -a;
            if (b < 0) b = -b;
            int t;
            while (b > 0)
            {
                t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        public static int LKKT(int a, int b)
        {
            int temp = a > b ? a : b;
            int counter = 1;
            while (!(temp % a == 0 && temp % b == 0))
            {
                temp = temp * counter;
                counter++;
            }
            return temp;
        }
    }
}
