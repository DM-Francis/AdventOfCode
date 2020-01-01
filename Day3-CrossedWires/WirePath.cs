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

        public List<Point> GetPoints()
        {
            var outputList = new List<Point>(_pathPoints);
            outputList.Reverse();

            return outputList;
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
