using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.Services
{
    public static class GeometricsService
    {
        private static double degreesToRadians = (Math.PI / 180);

        public static Point GetPointInCircleCoordinates(double circleRadius, double angle)
        {
            return new Point
            {
                X = circleRadius * Math.Cos(angle * degreesToRadians),
                Y = circleRadius * Math.Sin(angle * degreesToRadians)
            };
        }

        public static double CalculateTrianglePerimeter(Point coordinatesPointOne, Point coordinatesPointTwo, Point coordinatesPointsThree)
        {

            if (PointsAreInLine(coordinatesPointOne, coordinatesPointTwo, coordinatesPointsThree).PointsAreInLine)
            {
                return 0;
            }

            // Side A: coordinatesPointOne -> coordinatesPointTwo
            // Side B: coordinatesPointOne -> coordinatesPointsThree
            // Side C: coordinatesPointTwo -> coordinatesPointsThree
            double sideASize = Math.Sqrt(Math.Pow(coordinatesPointTwo.X - coordinatesPointOne.X, 2) + Math.Pow(coordinatesPointTwo.Y - coordinatesPointOne.Y, 2));
            double sideBSize = Math.Sqrt(Math.Pow(coordinatesPointsThree.X - coordinatesPointOne.X, 2) + Math.Pow(coordinatesPointsThree.Y - coordinatesPointOne.Y, 2));
            double sideCSize = Math.Sqrt(Math.Pow(coordinatesPointsThree.X - coordinatesPointTwo.X, 2) + Math.Pow(coordinatesPointsThree.Y - coordinatesPointTwo.Y, 2));

            return sideASize + sideBSize + sideCSize;
        }

        public static bool PointIsInsideTriangle(Point point, Point coordinatesPointOne, Point coordinatesPointTwo, Point coordinatesPointsThree)
        {
            // This is determined using baricentric coordinates

            var s = coordinatesPointOne.Y * coordinatesPointsThree.X - coordinatesPointOne.X * coordinatesPointsThree.Y + (coordinatesPointsThree.Y - coordinatesPointOne.Y) * point.X + (coordinatesPointOne.X - coordinatesPointsThree.X) * point.Y;
            var t = coordinatesPointOne.X * coordinatesPointTwo.Y - coordinatesPointOne.Y * coordinatesPointTwo.X + (coordinatesPointOne.Y - coordinatesPointTwo.Y) * point.X + (coordinatesPointTwo.X - coordinatesPointOne.X) * point.Y;

            if ((s < 0) != (t < 0))
                return false;

            var A = -coordinatesPointTwo.Y * coordinatesPointsThree.X + coordinatesPointOne.Y * (coordinatesPointsThree.X - coordinatesPointTwo.X) + coordinatesPointOne.X * (coordinatesPointTwo.Y - coordinatesPointsThree.Y) + coordinatesPointTwo.X * coordinatesPointsThree.Y;

            return A < 0 ?
                    (s <= 0 && s + t >= A) :
                    (s >= 0 && s + t <= A);
        }

        public static PointsInLine PointsAreInLine(Point coordinatesPointOne, Point coordinatesPointTwo, Point coordinatesPointsThree)
        {
            bool pointsAreInLine = (coordinatesPointOne.X == coordinatesPointTwo.X && coordinatesPointTwo.X == coordinatesPointsThree.X);
            bool lineCrossPointZero = pointsAreInLine && (coordinatesPointOne.X == 0);
            pointsAreInLine |= (coordinatesPointOne.Y == coordinatesPointTwo.Y && coordinatesPointTwo.Y == coordinatesPointsThree.Y);
            lineCrossPointZero |= (pointsAreInLine && (coordinatesPointOne.Y == 0));

            if (!pointsAreInLine)
            {
                var sloap1 = (coordinatesPointTwo.Y - coordinatesPointOne.Y) / (coordinatesPointTwo.X - coordinatesPointOne.X);
                var sloap2 = (coordinatesPointsThree.Y - coordinatesPointOne.Y) / (coordinatesPointsThree.X - coordinatesPointOne.X);

                pointsAreInLine = (sloap1 == sloap2);

                if (pointsAreInLine)
                {
                    var sloap0 = (coordinatesPointOne.Y - 0) / (coordinatesPointOne.X - 0);
                    sloap1 = (coordinatesPointTwo.Y - 0) / (coordinatesPointTwo.X - 0);
                    sloap2 = (coordinatesPointsThree.Y - 0) / (coordinatesPointsThree.X - 0);

                    lineCrossPointZero = (sloap0 == sloap1 && sloap1 == sloap2);
                }                
            }

            return new PointsInLine
            {
                PointsAreInLine = pointsAreInLine,
                LineCrossPointZero = lineCrossPointZero
            };
        }
    }
}
