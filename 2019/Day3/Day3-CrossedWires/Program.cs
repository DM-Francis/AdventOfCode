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
            PartTwo();            
        }

        private static void PartTwo()
        {
            // Steps:
            // 1. Create paths from input
            // 2. Create grid to place paths on
            // 3. Plot paths on grid
            // 4. Find intersections of paths
            // 5. Find indexes of each intersection on both paths
            // 6. Get the smallest sum of indexes

            var paths = GetPathsFromFile();
            var path1 = paths[0];
            var path2 = paths[1];

            List<Point> interceptionPoints = GetInterceptionPoints(path1, path2);

            var path1Points = path1.GetAllPoints();
            var path2Points = path2.GetAllPoints();

            var steps = (from ip in interceptionPoints
                         where !ip.Equals(new Point(0, 0))
                         select new
                         {
                             Path1Steps = GetIndex(ip, path1Points),
                             Path2Steps = GetIndex(ip, path2Points)
                         }).ToList();

            int minCombinedSteps = steps.Select(s => s.Path1Steps + s.Path2Steps).Min();

            Console.WriteLine(steps.Select(s => s.Path1Steps + s.Path2Steps).Min());
        }

        private static int GetIndex<T>(T item, List<T> list) where T : IEquatable<T>
        {
            return list.FindIndex(i => i.Equals(item));
        }

        private static void PartOne()
        {
            // Steps:
            // 1. Create paths from input
            // 2. Create grid to place paths on
            // 3. Plot paths on grid
            // 4. Find intersections of paths
            // 5. Find closest intersection to the origin

            var paths = GetPathsFromFile();
            var path1 = paths[0];
            var path2 = paths[1];

            List<Point> interceptionPoints = GetInterceptionPoints(path1, path2);

            int shortestDistance = (from p in interceptionPoints
                                    where !p.Equals(new Point(0, 0))
                                    select Math.Abs(p.X) + Math.Abs(p.Y)).Min();

            Console.WriteLine(shortestDistance);
        }

        private static List<WirePath> GetPathsFromFile()
        {
            return File.ReadAllLines("input.txt").Select(rp => WirePath.FromString(rp)).ToList();
        }

        private static List<Point> GetInterceptionPoints(WirePath path1, WirePath path2)
        {
            var bothPaths = new List<WirePath> { path1, path2 };
            var grid = new CircuitGrid();

            grid.PlotPath(path1, 1);
            grid.PlotPath(path2, 2);

            List<Point> interceptionPoints = (from gv in grid.AllPoints()
                                              where gv.values.Count == 2
                                              select gv.point).ToList();
            return interceptionPoints;
        }
    }
}
