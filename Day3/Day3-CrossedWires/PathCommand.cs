using System;
using System.Collections.Generic;
using System.Text;

namespace Day3_CrossedWires
{
    public class PathCommand
    {
        public Direction Direction { get; }
        public int Distance { get; }

        public PathCommand(string command)
        {
            if (command.Length == 1 || string.IsNullOrEmpty(command))
            {
                throw new ArgumentException($"Invalid command \"{command}\"", nameof(command));
            }

            Direction = GetDirectionFromChar(command[0]);
            Distance = int.Parse(command.Substring(1));
        }

        private static Direction GetDirectionFromChar(char character)
        {
            return character switch
            {
                'L' => Direction.Left,
                'R' => Direction.Right,
                'U' => Direction.Up,
                'D' => Direction.Down,
                _ => throw new ArgumentException($"Unknown direction character '{character}'"),
            };
        }
    }
}
