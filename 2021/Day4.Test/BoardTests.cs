using Common;
using FluentAssertions;
using Xunit;

namespace Day4.Test
{
    public class BoardTests
    {
        [Fact]
        public void IsCreatedProperly()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            // Act
            var board = new Board(inputData);

            // Assert
            board.Numbers.GetLength(0).Should().Be(3);
            board.Numbers.GetLength(1).Should().Be(3);

            board.Numbers.GetRow(1).Should().BeEquivalentTo(new int[3] { 4, 5, 6 });
            board.Numbers.GetColumn(2).Should().BeEquivalentTo(new int[3] { 3, 6, 9 });
        }

        [Fact]
        public void MarksNumbersCorrectly()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board = new Board(inputData);

            // Act
            board.Mark(7);
            board.Mark(5);
            board.Mark(3);

            // Assert
            board.Marked.Should().BeEquivalentTo(new bool[3, 3]
            {
                { false, false, true },
                { false, true, false },
                { true, false, false },
            });
        }

        [Fact]
        public void CorrectlyDeterminesIfTheBoardHasWon()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board1 = new Board(inputData);
            var board2 = new Board(inputData);

            board1.Mark(1);
            board1.Mark(2);
            board1.Mark(3);

            board2.Mark(7);

            // Act
            bool board1hasWon = board1.HasWon();
            bool board2hasWon = board2.HasWon();

            // Assert
            board1hasWon.Should().BeTrue();
            board2hasWon.Should().BeFalse();
        }

        [Fact]
        public void CorrectlyIdentifiesTheWinningRow()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board = new Board(inputData);
            board.Mark(4);
            board.Mark(5);
            board.Mark(6);

            // Act
            int[]? winningNumbers = board.GetWinningNumbers();

            // Assert
            winningNumbers.Should().NotBeNull();
            winningNumbers.Should().BeEquivalentTo(new int[] { 4, 5, 6 });
        }

        [Fact]
        public void CorrectlyIdentifiesTheWinningColumn()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board = new Board(inputData);
            board.Mark(3);
            board.Mark(6);
            board.Mark(9);

            // Act
            int[]? winningNumbers = board.GetWinningNumbers();

            // Assert
            winningNumbers.Should().NotBeNull();
            winningNumbers.Should().BeEquivalentTo(new int[] { 3, 6, 9 });
        }

        [Fact]
        public void GetWinningNumbersReturnsNullIfBoardHasNotWon()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board = new Board(inputData);
            board.Mark(3);

            // Act
            int[]? winningNumbers = board.GetWinningNumbers();

            // Assert
            winningNumbers.Should().BeNull();
        }

        [Fact]
        public void CanHandleMultipleSpacesBetweenNumbers()
        {
            // Arrange
            var inputData = new string[]
            {
                " 1  2  3",
                "10 34 99",
                "89  5  4"
            };

            // Act
            var board = new Board(inputData);

            // Assert
            board.Numbers.GetLength(0).Should().Be(3);
            board.Numbers.GetLength(1).Should().Be(3);

            board.Numbers.GetRow(1).Should().BeEquivalentTo(new int[3] { 10, 34, 99 });
            board.Numbers.GetColumn(2).Should().BeEquivalentTo(new int[3] { 3, 99, 4 });
        }

        [Fact]
        public void GetAllUnmarkedNumbersWorksCorrectly()
        {
            // Arrange
            var inputData = new string[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9"
            };

            var board = new Board(inputData);
            board.Mark(3);
            board.Mark(9);
            board.Mark(5);

            // Act
            var unmarkedNumbers = board.GetAllUnmarkedNumbers();

            // Assert
            unmarkedNumbers.Should().BeEquivalentTo(new[] { 1, 2, 4, 6, 7, 8 });
        }
    }
}