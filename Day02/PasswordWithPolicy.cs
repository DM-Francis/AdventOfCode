using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day02
{
    public class PasswordWithPolicy
    {
        public char LimitedChar { get; }
        public int Min { get; }
        public int Max { get; }
        public string Password { get; }

        public PasswordWithPolicy(string input)
        {
            string[] parts = input.Split(' ');

            string minmax = parts[0];
            LimitedChar = parts[1][0];
            Password = parts[2];

            string[] minmaxSplit = minmax.Split('-');
            Min = int.Parse(minmaxSplit[0]);
            Max = int.Parse(minmaxSplit[1]);
        }

        public bool IsValid()
        {
            int charCount = Password.Count(c => c == LimitedChar);
            return charCount >= Min & charCount <= Max;
        }
    }
}
