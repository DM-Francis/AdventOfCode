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

        public void PlotPath(WirePath path, int value)
        {
            var points = path.GetAllPoints();

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
