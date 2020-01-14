using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10_MonitoringStation
{
    class Program
    {
        static void Main(string[] args)
        {
            var universe = CreateUniverseFromFile();

            int maxDetected = 0;
            Asteroid bestAsteroid = null;

            foreach(var asteroid in universe.Asteroids)
            {
                int visibleAsteriods = asteroid.CountVisibleAsteroids();
                if (visibleAsteriods > maxDetected)
                {
                    maxDetected = visibleAsteriods;
                    bestAsteroid = asteroid;
                }
            }

            Render(universe);
            Console.WriteLine(maxDetected);

            var vaporisedAsteroids = universe.VaporiseAsteroidsFrom(bestAsteroid.X, bestAsteroid.Y).ToList();

            var twoHundredthAsteroid = vaporisedAsteroids[199];

            Console.WriteLine($"({twoHundredthAsteroid.X}, {twoHundredthAsteroid.Y})");
            Console.WriteLine(twoHundredthAsteroid.X * 100 + twoHundredthAsteroid.Y);
        }

        public static void Render(Universe universe)
        {
            for (int y = 0; y < universe.Height; y++)
            {
                for (int x = 0; x < universe.Width; x++)
                {
                    if (universe[x, y] == null)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write('#');
                    }

                    if (x == universe.Width - 1)
                    {
                        Console.Write(Environment.NewLine);
                    }
                }
            }
        }

        public static Universe CreateUniverseFromFile()
        {
            string[] rawData = File.ReadAllLines("input.txt");

            int width = rawData[0].Length;
            int height = rawData.Length;

            var universe = new Universe(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (rawData[y][x] == '#') universe.AddAsteroidAt(x, y);
                }
            }

            return universe;
        }
    }
}
