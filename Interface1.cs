using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    public interface IFunction
    {
        double Evaluate(double[] v);
        string Name { get; }
    }
}
