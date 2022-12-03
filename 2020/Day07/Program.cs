using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputStrings = File.ReadAllLines("input.txt");

            var bags = inputStrings.Select(s => new Bag(s)).ToList();

            var validColors = new HashSet<string>();
            CheckBagsThatCanContainColor("shiny gold", bags, validColors);

            Console.WriteLine(validColors.Count);

            int bagsWithinGold = CountBagsWithinColoredBag("shiny gold", bags);

            Console.WriteLine(bagsWithinGold);
        }

        private static void CheckBagsThatCanContainColor(string bagColor, IReadOnlyCollection<Bag> allBags, ICollection<string> validColors)
        {
            var validBags = allBags.Where(b => b.AllowedChildren.Select(b => b.BagColor).Contains(bagColor));

            foreach(Bag bag in validBags)
            {
                validColors.Add(bag.Color);
                CheckBagsThatCanContainColor(bag.Color, allBags, validColors);
            }
        }

        private static int CountBagsWithinColoredBag(string bagColor, IReadOnlyCollection<Bag> allBags)
        {
            Bag currentBag = allBags.Single(b => b.Color == bagColor);

            int innerBags = 0;
            foreach(var child in currentBag.AllowedChildren)
            {
                innerBags += child.Amount;
                innerBags += child.Amount * CountBagsWithinColoredBag(child.BagColor, allBags);
            }

            return innerBags;
        }
    }
}
