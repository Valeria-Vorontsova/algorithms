using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    public class ConstantFunction : IFunction
    {
        public string Name => "Constant(1)";

        public double Evaluate(double[] v)
        {
            return 1.0;
        }
    }
}
