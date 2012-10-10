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
//            double x = -radius * (1 - 2 * Math.Cos(Math.PI / 2 - angleInRadians) * Math.Sin(Math.PI / 2 - angleInRadians));
//            double y = 2 * radius * Math.Pow(Math.Cos(Math.PI / 2 - angleInRadians), 2);

            double x = radius * Math.Cos(angleInRadians);
            double y = radius * Math.Sin(angleInRadians);

            return new CartesianPoint(x, y);
        }
    }
}