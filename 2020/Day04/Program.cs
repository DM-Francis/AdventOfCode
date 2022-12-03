using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string rawInput = File.ReadAllText("input.txt");

            string[] rawPassportData = rawInput.Split("\n\n");

            var passports = rawPassportData.Select(r => new Passport(r));

            int validCount = passports.Count(p => p.IsValidIgnoringCid());
            int fullValidCount = passports.Count(p => p.ValidateAllFields());
            Console.WriteLine(validCount);
            Console.WriteLine(fullValidCount);
        }
    }
}
