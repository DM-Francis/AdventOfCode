using Day3_CrossedWires.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day3_CrossedWires
{
    public class WirePath
    {
        private readonly Stack<Point> _pathPoints;

        public WirePath()
        {
            _pathPoints = new Stack<Point>();
            _pathPoints.Push(new Point(0, 0));
        }

        public static WirePath FromString(string pathRaw)
        {
            var pathCommands = new List<string>(pathRaw.Split(','));
            var path = new WirePath();
            path.AddRange(pathCommands);

            return path;
        }

        public List<Point> GetCornerPoints()
        {
            var outputList = new List<Point>(_pathPoints);
            outputList.Reverse();

            return outputList;
        }

        public List<Point> GetAllPoints()
        {
            var corners = GetCornerPoints();
            var ouputList = new List<Point>();

            foreach (var (point, index) in corners.WithIndex())
            {
                ouputList.Add(point);
                if (index != corners.Count - 1)
                {
                    var nextPoint = corners[index + 1];
                    AddLineBetweenPoints(point, nextPoint, ouputList);
                }
            }

            return ouputList;
        }

        private void AddLineBetweenPoints(Point p1, Point p2, List<Point> listToAddTo)
        {
            if (p1.X == p2.X) // Vertical line
            {
                var ys = Enumerable.Range(Math.Min(p1.Y, p2.Y) + 1, Math.Abs(p1.Y - p2.Y) - 1);

                int ordering = Math.Sign(p2.Y - p1.Y); // 1 if the line is going upwards, -1 if downwards

                var pointsToAdd = ys.Select(y => new Point(p1.X, y)).OrderBy(p => p.Y * ordering);
                listToAddTo.AddRange(pointsToAdd);
            }
            else if (p1.Y == p2.Y) // Horizontal line
            {
                var xs = Enumerable.Range(Math.Min(p1.X, p2.X) + 1, Math.Abs(p1.X - p2.X) - 1);

                int ordering = Math.Sign(p2.X - p1.X); // 1 if the line is going left, -1 if right

                var pointsToAdd = xs.Select(x => new Point(x, p1.Y)).OrderBy(p => p.X * ordering);
                listToAddTo.AddRange(pointsToAdd);
            }
            else
            {
                throw new ArgumentException("Points provided are not on either the same vertical line or horizontal line");
            }
        }

        public void AddRange(IEnumerable<string> commands)
        {
            foreach (var command in commands)
            {
                Add(command);
            }
        }

        public void AddRange(IEnumerable<PathCommand> commands)
        {
            foreach(var command in commands)
            {
                Add(command);
            }
        }

        public void Add(string command)
        {
            Add(new PathCommand(command));
        }

        public void Add(PathCommand command)
        {
            Point latestPoint = _pathPoints.Peek();

            Point newPoint = command.Direction switch
            {
                Direction.Left => Point.LeftOf(latestPoint, command.Distance),
                Direction.Right => Point.RightOf(latestPoint, command.Distance),
                Direction.Up => Point.UpFrom(latestPoint, command.Distance),
                Direction.Down => Point.DownFrom(latestPoint, command.Distance),
                _ => throw new InvalidOperationException("Unknown direction")
            };

            _pathPoints.Push(newPoint);
        }

        public int GetMinX() => _pathPoints.Select(p => p.X).Min();
        public int GetMaxX() => _pathPoints.Select(p => p.X).Max();
        public int GetMinY() => _pathPoints.Select(p => p.Y).Min();
        public int GetMaxY() => _pathPoints.Select(p => p.Y).Max();
    }
}
