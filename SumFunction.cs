using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    public class SumFunction : IFunction
    {
        public string Name => "Sum";
        public double Evaluate(double[] v)
        {
            double sum = 0.0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
            }
            return sum;
        }
    }
}
