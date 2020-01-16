using Intcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Day11_SpacePolice
{
    public class PaintingRobot
    {
        private readonly IntcodeInterpreter _interpreter;
        private readonly Dictionary<(int X, int Y), Color> _hullColors = new Dictionary<(int, int), Color>();
        private (int X, int Y) _currentPosition = (0, 0);
        private Direction _direction = Direction.Up;

        private OutputType _nextOutputType = OutputType.Color;

        private const Color DefaultColor = Color.Black;

        public int PanelsPainted => _hullColors.Count;

        public int MinX => _hullColors.Keys.Select(p => p.X).Min();
        public int MaxX => _hullColors.Keys.Select(p => p.X).Max();
        public int MinY => _hullColors.Keys.Select(p => p.Y).Min();
        public int MaxY => _hullColors.Keys.Select(p => p.Y).Max();

        public Color this[int X, int Y] => GetColorAt((X, Y));

        public PaintingRobot(IEnumerable<BigInteger> program)
        {
            _interpreter = new IntcodeInterpreter(program, ProvideInput, OnOutput);
        }

        public void Start(Color startingColor = Color.Black)
        {
            _hullColors[(0, 0)] = startingColor;
            _interpreter.Interpret();
        }

        private BigInteger ProvideInput()
        {
            return (int)GetColorAt(_currentPosition);
        }

        private Color GetColorAt((int X, int Y) position)
        {
            if (!_hullColors.TryGetValue(position, out Color color))
            {
                color = DefaultColor;
            }
            return color;
        }

        private void OnOutput(BigInteger output)
        {
            if (_nextOutputType == OutputType.Color)
            {
                PaintCurrentPosition((int)output);
                _nextOutputType = OutputType.Direction;
            }
            else if (_nextOutputType == OutputType.Direction)
            {
                MoveRobot((int)output);
                _nextOutputType = OutputType.Color;
            }
            else
            {
                throw new InvalidOperationException($"OutputType has reached an invalid state: {_nextOutputType}");
            }
        }

        private void PaintCurrentPosition(int colorCode)
        {
            _hullColors[_currentPosition] = (Color)colorCode;
        }

        private void MoveRobot(int moveCommand)
        {
            if (moveCommand == 0)
            {
                RotateLeft();
            }
            else if (moveCommand == 1)
            {
                RotateRight();
            }
            else
            {
                throw new InvalidOperationException($"Unknown move command: {moveCommand}");
            }

            MoveForwards(1);
        }

        private void RotateLeft()
        {
            _direction = _direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                _ => throw new InvalidOperationException($"Unrecognised direction {_direction.ToString()}")
            };
        }
        private void RotateRight()
        {
            _direction = _direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new InvalidOperationException($"Unrecognised direction {_direction.ToString()}")
            };
        }
        private void MoveForwards(int distance)
        {
            _currentPosition = _direction switch
            {
                Direction.Up => (_currentPosition.X, _currentPosition.Y + distance),
                Direction.Down => (_currentPosition.X, _currentPosition.Y - distance),
                Direction.Left => (_currentPosition.X - distance, _currentPosition.Y),
                Direction.Right => (_currentPosition.X + distance, _currentPosition.Y),
                _ => throw new InvalidOperationException($"Unrecognised direction {_direction.ToString()}")
            };
        }
    }
}
