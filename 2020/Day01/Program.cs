using System;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputStrings = File.ReadAllLines("input.txt");
            int[] inputs = inputStrings.Select(s => int.Parse(s)).ToArray();

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    for (int k = 0; k < inputs.Length; k++)
                    {
                        int sum = inputs[i] + inputs[j] + inputs[k];
                        if (sum == 2020)
                        {
                            Console.WriteLine($"{inputs[i]}, {inputs[j]}, {inputs[k]}");
                            Console.WriteLine(inputs[i] * inputs[j] * inputs[k]);
                        }
                    }
                }
            }
        }
    }
}
