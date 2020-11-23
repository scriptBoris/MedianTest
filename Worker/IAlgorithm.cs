using System;
using System.Collections.Generic;
using System.Text;

namespace Worker
{
    internal interface IAlgorithm
    {
        float Calculate(params int[] values);
    }
}
