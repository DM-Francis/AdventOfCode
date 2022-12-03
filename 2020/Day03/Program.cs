using System;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            long[] treeCounts = new long[]
            {
                CountTrees(1, 1, input),
                CountTrees(3, 1, input),
                CountTrees(5, 1, input),
                CountTrees(7, 1, input),
                CountTrees(1, 2, input),
            };

            Console.WriteLine(treeCounts.Aggregate((a,b) => a * b));
        }

        public static int CountTrees(int rightMoves, int downMoves, string[] map)
        {
            // Start top left
            int x = 0;

            int treeCount = 0;
            int width = map[0].Length;

            for (int y = 0; y < map.Length; y += downMoves)
            {
                if (map[y][x % width] == '#')
                    treeCount++;

                x += rightMoves;
            }

            return treeCount;
        }
    }
}
