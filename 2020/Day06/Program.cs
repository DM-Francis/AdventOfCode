using System;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            string rawInput = File.ReadAllText("input.txt");
            string[] rawGroups = rawInput.Split("\n\n");

            int sum = 0;
            foreach(string group in rawGroups)
            {
                var distinctChars = group.Replace("\n", "").Distinct();
                string[] people = group.Trim().Split('\n');

                foreach(char c in distinctChars)
                {
                    if (people.All(p => p.Contains(c)))
                        sum++;
                }
            }

            Console.WriteLine(sum);
        }
    }
}
