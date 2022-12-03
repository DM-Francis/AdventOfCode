using System;
using System.Collections.Generic;
using System.Text;

namespace Day10_MonitoringStation
{
    public class Asteroid
    {
        private readonly Universe _parentUniverse;
        
        public int X { get; }
        public int Y { get; }

        public Asteroid(Universe parentUniverse, int x, int y)
        {
            _parentUniverse = parentUniverse;
            X = x;
            Y = y;
        }

        public bool CanSee(int x, int y)
        {
            int x_diff = x - X;
            int y_diff = y - Y;

            int gcd = NumericHelper.AbsGCD(x_diff, y_diff);

            if (gcd == 1 || gcd == 0) return true;

            int x_unit = x_diff / gcd;
            int y_unit = y_diff / gcd;

            bool canSee = true;
            for (int i = 1; i < gcd; i++)
            {
                int x_check = X + x_unit * i;
                int y_check = Y + y_unit * i;
                if (_parentUniverse[x_check, y_check] != null)
                {
                    return false;
                }
            }

            return canSee;
        }

        public int CountVisibleAsteroids()
        {
            int count = 0;

            foreach(var asteroid in _parentUniverse.Asteroids)
            {
                if (this.CanSee(asteroid.X, asteroid.Y))
                {
                    count++;
                }
            }

            return count - 1; // Ignore self
        }

        public double GetAngleAsViewedFrom(int x, int y)
        {
            int x_diff = X - x;
            int y_diff = y - Y;

            double classicAngle = Math.Atan2(y_diff, x_diff) * 180 / Math.PI;
            double angleFromNorth = (classicAngle * -1 + 90) % 360;

            if (angleFromNorth < 0) return angleFromNorth + 360;
            
            return angleFromNorth;
        }
    }
}
