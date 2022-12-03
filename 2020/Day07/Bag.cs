using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Day07
{
    public class Bag
    {
        private readonly List<(string BagColor, int Amount)> _allowedChildren = new();

        public string Color { get; }
        public ReadOnlyCollection<(string BagColor, int Amount)> AllowedChildren { get; }

        public Bag(string input)
        {
            AllowedChildren = new ReadOnlyCollection<(string BagColor, int Amount)>(_allowedChildren);

            string[] inputSplit = input.Split(" bags contain ");

            Color = inputSplit[0];
            string childrenRaw = inputSplit[1].TrimEnd('.');
            string[] children = childrenRaw.Split(", ");

            foreach(string child in children)
            {
                if (child == "no other bags")
                    break;

                int amount = int.Parse(child[0].ToString());
                string color = child[2..].Replace("bags", "").Replace("bag", "").Trim();
                _allowedChildren.Add((color, amount));
            }
        }
    }
}
