using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3_CrossedWires
{
    class Program
    {
        static void Main(string[] args)
        {
            // Steps:
            // 1. Create paths from input
            // 2. Create grid to place paths on
            // 3. Plot paths on grid
            // 4. Find intersections of paths
            // 5. Find closest intersection to the origin

            string path1Input = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51";
            string path2Input = "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7";

            var paths = GetPathsFromFile();
            var path1 = paths[0];
            var path2 = paths[1];

            Console.WriteLine(CalculateShortestDistanceBetweenPaths(path1, path2));
        }

        private static List<WirePath> GetPathsFromFile()
        {
            return File.ReadAllLines("input.txt").Select(rp => WirePath.FromString(rp)).ToList();
        }

        private static int CalculateShortestDistanceBetweenPaths(WirePath path1, WirePath path2)
        {
            var bothPaths = new List<WirePath> { path1, path2 };
            var grid = new CircuitGrid();

            grid.PlotPath(path1, 1);
            grid.PlotPath(path2, 2);

            List<Point> interceptionPoints = (from gv in grid.AllPoints()
                                              where gv.values.Count == 2
                                              select gv.point).ToList();

            int shortestDistance = (from p in interceptionPoints
                                    where !p.Equals(new Point(0, 0))
                                    select Math.Abs(p.X) + Math.Abs(p.Y)).Min();

            return shortestDistance;
        }
    }
}
