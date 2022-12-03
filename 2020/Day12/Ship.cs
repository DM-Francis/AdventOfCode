using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Ship
    {
        public Direction Facing { get; private set; }
        public (int North, int East) Position { get; private set; } = (0, 0);
        public (int North, int East) RelativeWaypointPosition { get; private set; }

        public Ship(Direction initialFacing)
        {
            Facing = initialFacing;
        }

        public Ship((int North, int East) relativeWaypointPosition)
        {
            RelativeWaypointPosition = relativeWaypointPosition;
        }

        public void ApplyInstructions(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
            {
                ApplyInstruction(instruction);
            }
        }

        public void ApplyInstructions2(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
            {
                ApplyInstruction2(instruction);
            }
        }

        public void ApplyInstruction(string instruction)
        {
            char action = instruction[0];
            int value = int.Parse(instruction[1..]);

            if (action is ('N' or 'S' or 'E' or 'W' or 'F'))
            {
                Direction moveDirection = action switch
                {
                    'N' => Direction.North,
                    'S' => Direction.South,
                    'E' => Direction.East,
                    'W' => Direction.West,
                    'F' => Facing,
                    _ => throw new InvalidOperationException()
                };

                Position = moveDirection switch
                {
                    Direction.North => (Position.North + value, Position.East),
                    Direction.South => (Position.North - value, Position.East),
                    Direction.East => (Position.North, Position.East + value),
                    Direction.West => (Position.North, Position.East - value),
                    _ => throw new InvalidOperationException()
                };
            }
            else if (action == 'L')
            {
                Facing = (Direction)Modulo((int)Facing - value / 90, 4);
            }
            else if (action == 'R')
            {
                Facing = (Direction)Modulo((int)Facing + value / 90, 4);
            }
        }

        public void ApplyInstruction2(string instruction)
        {
            char action = instruction[0];
            int value = int.Parse(instruction[1..]);

            if (action is ('N' or 'S' or 'E' or 'W'))
            {
                RelativeWaypointPosition = action switch
                {
                    'N' => (RelativeWaypointPosition.North + value, RelativeWaypointPosition.East),
                    'S' => (RelativeWaypointPosition.North - value, RelativeWaypointPosition.East),
                    'E' => (RelativeWaypointPosition.North, RelativeWaypointPosition.East + value),
                    'W' => (RelativeWaypointPosition.North, RelativeWaypointPosition.East - value),
                    _ => throw new InvalidOperationException()
                };
            }            
            else if (action is 'L' or 'R')
            {
                int angle = action == 'L' ? value : Modulo(-value, 360);

                RelativeWaypointPosition = angle switch
                {
                    90 => (RelativeWaypointPosition.East, -RelativeWaypointPosition.North),
                    180 => (-RelativeWaypointPosition.North, -RelativeWaypointPosition.East),
                    270 => (-RelativeWaypointPosition.East, RelativeWaypointPosition.North),
                    _ => throw new InvalidOperationException("Unrecognised angle")
                };
            }
            else if (action is 'F')
            {
                Position = (Position.North + value * RelativeWaypointPosition.North, Position.East + value * RelativeWaypointPosition.East);
            }
        }

        private static int Modulo(int x, int n) => (x % n + n) % n;
    }
}
