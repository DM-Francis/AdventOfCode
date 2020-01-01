using Day3_CrossedWires.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day3_CrossedWires
{
    public class CircuitGrid
    {
        private readonly Dictionary<Point, List<int>> _gridValues;

        public IReadOnlyCollection<int> this[int x, int y] => _gridValues[new Point(x, y)].AsReadOnly();

        public IReadOnlyCollection<int> this[Point p] => _gridValues[p].AsReadOnly();

        public CircuitGrid()
        {
            _gridValues = new Dictionary<Point, List<int>>();
        }

        public IEnumerable<(Point point, IReadOnlyCollection<int> values)> AllPoints()
        {
            foreach(var gridValue in _gridValues)
            {
                yield return (gridValue.Key, gridValue.Value.AsReadOnly());
            }
        }

        //private void CreateGrid(int minX, int maxX, int minY, int maxY)
        //{
        //    if (minX > maxX || minY > maxY)
        //    {
        //        throw new ArgumentException("Invalid min/max coordinates provided");
        //    }

        //    var xs = Enumerable.Range(minX, maxX - minX + 1);
        //    var ys = Enumerable.Range(minY, maxY - minY + 1);

        //    var points = (from x in xs
        //                  from y in ys
        //                  select new Point(x, y)).ToList();

        //    foreach(var point in points)
        //    {
        //        _gridValues.Add(point, new List<int>());
        //    }
        //}

        public void PlotPath(WirePath path, int value)
        {
            var points = path.GetPoints();

            foreach(var (point, index) in points.WithIndex())
            {
                SetPoint(point, value);
                if (index != 0)
                {
                    var previousPoint = points[index - 1];
                    PlotLineBetweenPoints(point, previousPoint, value);
                }
            }
        }

        private void PlotLineBetweenPoints(Point p1, Point p2, int value)
        {
            if (p1.X == p2.X) // Vertical line
            {
                var ys = Enumerable.Range(Math.Min(p1.Y, p2.Y), Math.Abs(p1.Y - p2.Y) + 1);
                var pointsToPlot = ys.Select(y => new Point(p1.X, y));
                SetPoints(pointsToPlot, value);
            }
            else if (p1.Y == p2.Y) // Horizontal line
            {
                var xs = Enumerable.Range(Math.Min(p1.X, p2.X), Math.Abs(p1.X - p2.X) + 1);
                var pointsToPlot = xs.Select(x => new Point(x, p1.Y));
                SetPoints(pointsToPlot, value);
            }
            else
            {
                throw new ArgumentException("Points provided are not on either the same vertical line or horizontal line");
            }
        }

        private void SetPoints(IEnumerable<Point> points, int value)
        {
            foreach(var point in points)
            {
                SetPoint(point, value);
            }
        }

        private void SetPoint(Point point, int value)
        {
            if (!_gridValues.ContainsKey(point))
            {
                _gridValues[point] = new List<int> { value };
            }
            else
            {
                if (!_gridValues[point].Contains(value))
                {
                    _gridValues[point].Add(value);
                }
            }
        }
    }
}
