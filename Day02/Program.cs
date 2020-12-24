using System;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputStrings = File.ReadAllLines("input.txt");

            var policies = inputStrings.Select(i => new PasswordWithPolicy2(i));

            int validCount = policies.Count(p => p.IsValid());

            Console.WriteLine(validCount);
        }
    }
}
