using System;
using System.IO;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] instructions = File.ReadAllLines("input.txt");

            var ship = new Ship((1, 10));

            ship.ApplyInstructions2(instructions);

            Console.WriteLine(Math.Abs(ship.Position.East) + Math.Abs(ship.Position.North));
        }
    }
}
