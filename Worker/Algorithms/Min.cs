using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Worker.Algorithms
{
    internal class Min : IAlgorithm
    {
        public float Calculate(params int[] values)
        {
            if (values == null)
                throw new Exception("Bad parameter for algorithm");

            return values.Min();
        }
    }
}
