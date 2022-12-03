using System;
using Xunit;

namespace Day07.Test
{
    public class BagTests
    {
        [Fact]
        public void CanParseBagDefinition()
        {
            // Assemble
            string definition = "light red bags contain 1 bright white bag, 2 muted yellow bags.";

            // Act
            var bag = new Bag(definition);

            // Assert
            Assert.Equal("light red", bag.Color);
            Assert.Contains(("bright white", 1), bag.AllowedChildren);
            Assert.Contains(("muted yellow", 2), bag.AllowedChildren);
        }

        [Fact]
        public void CanParseEmptyBag()
        {
            // Assemble
            string definition = "posh plum bags contain no other bags.";

            // Act
            var bag = new Bag(definition);

            // Assert
            Assert.Equal("posh plum", bag.Color);
            Assert.Empty(bag.AllowedChildren);
        }
    }
}
