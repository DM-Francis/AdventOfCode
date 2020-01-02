using Day3_CrossedWires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Day3Tests
{
    public class CircuitGridTests
    {
        //[Fact]
        //public void CircuitGridIsCreatedWithCorrectPoints()
        //{
        //    // Assemble & Act
        //    var grid = new CircuitGrid(minX: -1, maxX: 1, minY: -1, maxY: 1);

        //    // Assert
        //    var allGridValues = grid.AllPoints().ToList();

        //    Assert.Collection(allGridValues.Select(gv => gv.point),
        //        Assertions_ForGridFromMinusOneToOne().ToArray());
        //}

        private IEnumerable<Action<Point>> Assertions_ForGridFromMinusOneToOne()
        {            
            foreach(var expectedPoint in AllPointsBetweenMinusOneAndOne())
            {
                yield return p => Assert.Equal(expectedPoint, p);
            }
        }

        private IEnumerable<Point> AllPointsBetweenMinusOneAndOne()
        {
            yield return new Point(0, 0);
            yield return new Point(0, 1);
            yield return new Point(0, -1);
            yield return new Point(1, 0);
            yield return new Point(1, 1);
            yield return new Point(1, -1);
            yield return new Point(-1, 0);
            yield return new Point(-1, 1);
            yield return new Point(-1, -1);
        }
    }
}
