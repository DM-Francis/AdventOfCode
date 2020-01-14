using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day10_MonitoringStation
{
    public class Universe
    {
        private readonly object[,] _map;

        public int Width { get; }
        public int Height { get; }

        public IEnumerable<Asteroid> Asteroids
        {
            get
            {
                foreach(var point in _map)
                {
                    if (point is Asteroid ast)
                    {
                        yield return ast;
                    }
                }
            }
        }

        public object this[int x, int y]
        {
            get => _map[x, y];
        }

        public Universe(int width, int height)
        {
            _map = new object[width, height];
            Width = width;
            Height = height;
        }

        public void AddAsteroidAt(int x, int y)
        {
            _map[x, y] = new Asteroid(this, x, y);
        }

        public Asteroid GetAsteroidAt(int x, int y)
        {
            var obj = _map[x, y];

            if (obj is Asteroid ast)
            {
                return ast;
            }

            return null;
        }

        public IEnumerable<Asteroid> VaporiseAsteroidsFrom(int x, int y)
        {
            while (Asteroids.Count() > 1)
            {
                var visibleAsteroids = GetVisibleAsteroidsInClockwiseOrder(x, y);

                foreach(var asteroid in visibleAsteroids)
                {
                    _map[asteroid.X, asteroid.Y] = null;
                    yield return asteroid;
                }
            }
        }

        public List<Asteroid> GetVisibleAsteroidsInClockwiseOrder(int origin_x, int origin_y)
        {
            var output = new List<Asteroid>();

            var asteroidAngles = GetAsteroidAnglesFrom(origin_x, origin_y);
            var orderedAsteroids = asteroidAngles.OrderBy(kv => kv.Value);

            foreach(var (asteroid, angle) in orderedAsteroids)
            {
                if (asteroid.CanSee(origin_x, origin_y))
                {
                    output.Add(asteroid);
                }
            }

            output.Remove(GetAsteroidAt(origin_x, origin_y));

            return output;
        }

        private Dictionary<Asteroid, double> GetAsteroidAnglesFrom(int x, int y)
        {
            var angles = new Dictionary<Asteroid, double>();

            foreach(var asteroid in Asteroids)
            {
                angles[asteroid] = asteroid.GetAngleAsViewedFrom(x, y);
            }

            return angles;
        }
    }
}
