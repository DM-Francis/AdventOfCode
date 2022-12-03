using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public static class BitArrayExtensions
    {
        public static int ToInt32(this BitArray bitArray)
        {
            if (bitArray.Count > 32)
                throw new InvalidOperationException("Bit array is too long to be converted to an Int32");

            int value = 0;
            for (int i = 0; i < bitArray.Count; i++)
            {
                value = (value << 1) | Convert.ToInt32(bitArray[i]);
            }

            return value;
        }
    }
}
