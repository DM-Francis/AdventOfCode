using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day11_SpacePolice
{
    public class Program
    {
        static void Main(string[] args)
        {
            var program = GetProgramFromFile();

            var robot = new PaintingRobot(program);

            robot.Start(Color.Black);
            Console.WriteLine(robot.PanelsPainted);
            Render(robot);
        }

        private static IEnumerable<BigInteger> GetProgramFromFile()
        {
            return File.ReadAllText("input.txt").Split(',').Select(c => BigInteger.Parse(c));
        }

        public static void Render(PaintingRobot robot)
        {
            for (int y = robot.MaxY; y >= robot.MinY; y--)
            {
                for (int x = robot.MinX; x <= robot.MaxX; x++)
                {
                    if (robot[x, y] == Color.Black)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write('#');
                    }

                    if (x == robot.MaxX)
                    {
                        Console.Write(Environment.NewLine);
                    }
                }
            }
        }
    }
}
