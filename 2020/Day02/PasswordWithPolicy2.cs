using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day02
{
    public class PasswordWithPolicy2
    {
        public char TargetChar { get; }
        public int Pos1 { get; }
        public int Pos2 { get; }
        public string Password { get; }

        public PasswordWithPolicy2(string input)
        {
            string[] parts = input.Split(' ');

            string positions = parts[0];
            TargetChar = parts[1][0];
            Password = parts[2];

            string[] positionSplit = positions.Split('-');
            Pos1 = int.Parse(positionSplit[0]);
            Pos2 = int.Parse(positionSplit[1]);
        }

        public bool IsValid()
        {
            return (Password[Pos1 - 1] == TargetChar || Password[Pos2 - 1] == TargetChar)
                && (Password[Pos1 - 1] != Password[Pos2 - 1]);
        }
    }
}
