using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    public class MultFunction : IFunction
    {
        public string Name => "Mult";
        public double Evaluate(double[] v)
        {
            double mult = 1.0;
            for (int i = 0; i < v.Length; i++)
            {
                mult *= v[i];
            }
            return mult;
        }
    }
}
