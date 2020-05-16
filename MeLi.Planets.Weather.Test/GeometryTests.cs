using MeLi.Planets.Weather.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeLi.Planets.Weather.Test
{
    public class GeometryTests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void GeometricsServiceTests()
        {
            // Points are in line.
            // The point (0, 0) is in the line.

            var PointsAreInLine = GeometricsService.PointsAreInLine(new Point { X = -3, Y = -3 }, new Point { X = 1, Y = 1 }, new Point { X = -7, Y = -7 });

            Assert.IsTrue(PointsAreInLine.PointsAreInLine);
            Assert.IsTrue(PointsAreInLine.LineCrossPointZero);


            // Points are in line.
            // The point (0, 0) is not in the line.

            PointsAreInLine = GeometricsService.PointsAreInLine(new Point { X = -3, Y = -3 }, new Point { X = 1, Y = -3 }, new Point { X = -7, Y = -3 });

            Assert.IsTrue(PointsAreInLine.PointsAreInLine);
            Assert.IsFalse(PointsAreInLine.LineCrossPointZero);


            // Points are in the same coordinates. So, they are in line.
            // The point (0, 0) is not in the line.

            PointsAreInLine = GeometricsService.PointsAreInLine(new Point { X = -3, Y = -3 }, new Point { X = -3, Y = -3 }, new Point { X = -3, Y = -3 });

            Assert.IsTrue(PointsAreInLine.PointsAreInLine);
            Assert.IsFalse(PointsAreInLine.LineCrossPointZero);

            // Points are not in line

            PointsAreInLine = GeometricsService.PointsAreInLine(new Point { X = -3, Y = -3 }, new Point { X = -42, Y = 17 }, new Point { X = 223, Y = 90 });

            Assert.IsFalse(PointsAreInLine.PointsAreInLine);
            Assert.IsFalse(PointsAreInLine.LineCrossPointZero);
        }
    }
}
