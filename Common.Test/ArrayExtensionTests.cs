using FluentAssertions;
using Xunit;

namespace Common.Test
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void CanGetRowsCorrectly()
        {
            // Arrange
            var array = new int[3, 3]
            {
                {1,2,3},
                {4,5,6},
                {7,8,9}
            };

            // Act
            int[] row0 = array.GetRow(0);
            int[] row1 = array.GetRow(1);
            int[] row2 = array.GetRow(2);

            // Assert
            row0.Should().BeEquivalentTo(new int[3] { 1, 2, 3 });
            row1.Should().BeEquivalentTo(new int[3] { 4, 5, 6 });
            row2.Should().BeEquivalentTo(new int[3] { 7, 8, 9 });
        }

        [Fact]
        public void CanGetColumnsCorrectly()
        {
            // Arrange
            var array = new int[3, 3]
            {
                {1,2,3},
                {4,5,6},
                {7,8,9}
            };

            // Act
            int[] col0 = array.GetColumn(0);
            int[] col1 = array.GetColumn(1);
            int[] col2 = array.GetColumn(2);

            // Assert
            col0.Should().BeEquivalentTo(new int[3] { 1, 4, 7 });
            col1.Should().BeEquivalentTo(new int[3] { 2, 5, 8 });
            col2.Should().BeEquivalentTo(new int[3] { 3, 6, 9 });
        }
    }
}