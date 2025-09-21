using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms
{
    public static class VectorGenerator
    {
        public static double[] GenerateRandomVector(int n, Random rnd)
        {
            var v = new double[n];
            for (int i = 0; i < n; i++)
                v[i] = rnd.NextDouble() * 1.0; 
            return v;
        }
    }
}
