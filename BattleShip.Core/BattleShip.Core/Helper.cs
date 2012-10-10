namespace BattleShip.Core
{
    using System;

    public struct CartesianPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public CartesianPoint(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }
    }

    public static class Helper
    {
        public static CartesianPoint FromPolarToCartesian(double radius, double angleInRadians)
        {
            double x = radius * Math.Cos(angleInRadians);
            double y = radius * Math.Sin(angleInRadians);

            return new CartesianPoint(x, y);
        }
    }
}