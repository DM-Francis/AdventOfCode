using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputStrings = File.ReadAllLines("input.txt");

            var seatIds = new List<int>();

            foreach (string input in inputStrings)
            {
                string rowString = input[..^3];
                string columnString = input[^3..];

                string rowBinaryString = rowString.Replace('F', '0').Replace('B', '1');
                string columnBinaryString = columnString.Replace('L', '0').Replace('R', '1');

                int row = Convert.ToInt32(rowBinaryString, 2);
                int col = Convert.ToInt32(columnBinaryString, 2);

                seatIds.Add(row * 8 + col);
            }

            Console.WriteLine(seatIds.Max());

            for (int i = 0; i < 1024; i++)
            {
                if (seatIds.Contains(i))
                    continue;

                if (seatIds.Contains(i - 1) && seatIds.Contains(i + 1))
                    Console.WriteLine(i);
            }
        }
    }
}
