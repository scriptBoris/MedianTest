using System;
using System.Collections.Generic;
using System.Text;

namespace Worker.Algorithms
{
    internal class Multiply : IAlgorithm
    {
        public float Calculate(params int[] values)
        {
            if (values == null)
                throw new Exception("Bad parameter for algorithm");

            int sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum *= values[i];
            }

            if (sum == 0)
                return 0;
            else
                return sum % 255;
        }
    }
}
