using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Day3_CrossedWires
{
    public readonly struct Point : IEquatable<Point>
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public static Point LeftOf(Point p, int distance) => new Point(p.X - distance, p.Y);

        public static Point RightOf(Point p, int distance) => new Point(p.X + distance, p.Y);

        public static Point UpFrom(Point p, int distance) => new Point(p.X, p.Y + distance);

        public static Point DownFrom(Point p, int distance) => new Point(p.X, p.Y - distance);

        public override bool Equals(object obj) => obj is Point point && Equals(point);
        public bool Equals([AllowNull] Point other) => X == other.X && Y == other.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
