using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Day16.Test
{
    public class BitLogicTests
    {
        [Fact]
        public void BitArrayTesting1()
        {
            string testData = "EE00D40C823060";
            byte[] dataBytes = Convert.FromHexString(testData);
            var bitArray = new BitArray(dataBytes);

            bitArray.Length = 32;

            int value = bitArray.ToInt32();

            Assert.Equal(1996499760, value);
        }
    }
}
