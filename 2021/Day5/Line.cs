using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    public class Line
    {
        public Point Start { get; }
        public Point End { get; }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public bool IsHorizontal => Start.Y == End.Y;
        public bool IsVertical => Start.X == End.X;
        public bool IsDiagonal => !IsHorizontal && !IsVertical;

        public IEnumerable<Point> GetAllPointsOnLine()
        {
            if (IsHorizontal)
            {
                int direction = Math.Sign(End.X - Start.X);
                for (int x = Start.X; x != End.X; x += direction)
                {
                    yield return new Point(x, Start.Y);
                }
            }
            else if (IsVertical)
            {
                int direction = Math.Sign(End.Y - Start.Y);
                for (int y = Start.Y; y != End.Y; y += direction)
                {
                    yield return new Point(Start.X, y);
                }                
            }
            else if (IsDiagonal)
            {
                int xDirection = Math.Sign(End.X - Start.X);
                int yDirection = Math.Sign(End.Y - Start.Y);

                int x = Start.X;
                int y = Start.Y;
                while (x != End.X && y != End.Y)
                {
                    yield return new Point(x, y);
                    x += xDirection;
                    y += yDirection;
                }
            }

            yield return End;
        }
    }
}
