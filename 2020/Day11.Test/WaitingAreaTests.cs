using FluentAssertions;
using System;
using Xunit;

namespace Day11.Test
{
    public class WaitingAreaTests
    {
        [Fact]
        public void WorksForExample1Round()
        {
            // Assemble
            var seatLayout = new string[]
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            var waitingArea = new WaitingArea(seatLayout);

            // Act
            waitingArea.IncrementRoundV2();

            // Assert
            var expectedLayout = new WaitingArea(new string[]
            {
                "#.##.##.##",
                "#######.##",
                "#.#.#..#..",
                "####.##.##",
                "#.##.##.##",
                "#.#####.##",
                "..#.#.....",
                "##########",
                "#.######.#",
                "#.#####.##"
            });

            waitingArea.Seats.Should().BeEquivalentTo(expectedLayout.Seats);
        }

        [Fact]
        public void WorksForExample2Rounds()
        {
            // Assemble
            var seatLayout = new string[]
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            var waitingArea = new WaitingArea(seatLayout);

            // Act
            waitingArea.IncrementRoundV2();
            waitingArea.IncrementRoundV2();

            // Assert
            var expectedLayout = new WaitingArea(new string[]
            {
                "#.LL.LL.L#",
                "#LLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLL#",
                "#.LLLLLL.L",
                "#.LLLLL.L#"
            });

            waitingArea.Seats.Should().BeEquivalentTo(expectedLayout.Seats);
        }
    }
}
