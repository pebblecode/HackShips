namespace Tests
{
    using System;

    using BattleShip.Core;

    using NUnit.Framework;

    [TestFixture]
    public class HelperTests
    {
        private const double Tolerance = 1e-6;

        [Test]
        public void FromPolarToCartesian_ZeroRadius_ReturnsZeroAndZero()
        {
            var result = Helper.FromPolarToCartesian(0.0, 1.23);

            Assert.AreEqual(0.0, result.X, Tolerance);
            Assert.AreEqual(0.0, result.Y, Tolerance);
        }

        [Test]
        public void FromPolarToCartesian_ZeroAngle_ReturnsRadiusAndZero()
        {
            double radius = 1.23;
            var result = Helper.FromPolarToCartesian(radius, 0.0);

            Assert.AreEqual(radius, result.X, Tolerance);
            Assert.AreEqual(0.0, result.Y, Tolerance);
        }

        [Test]
        public void FromPolarToCartesian_PiAngle_ReturnsMinusRadiusAndZero()
        {
            double radius = 1.23;
            var result = Helper.FromPolarToCartesian(radius, Math.PI);

            Assert.AreEqual(-radius, result.X, Tolerance);
            Assert.AreEqual(0.0, result.Y, Tolerance);
        }

        [Test]
        public void FromPolarToCartesian_HalfPiAngle_ReturnsZeroAndRadius()
        {
            double radius = 1.23;
            var result = Helper.FromPolarToCartesian(radius, Math.PI / 2.0);

            Assert.AreEqual(0.0, result.X, Tolerance);
            Assert.AreEqual(radius, result.Y, Tolerance);
        }

        [Test]
        public void FromPolarToCartesian_270DegreeAngle_ReturnsZeroAndMinusRadius()
        {
            double radius = 1.23;
            var result = Helper.FromPolarToCartesian(radius, 3.0 * Math.PI / 2.0);

            Assert.AreEqual(0.0, result.X, Tolerance);
            Assert.AreEqual(-radius, result.Y, Tolerance);
        }

        [Test]
        public void FromPolarToCartesian_45DegreeAngle_ReturnsRadiusDivRootTwoAndRadiusDivRootTwo()
        {
            double radius = 1.23;
            var result = Helper.FromPolarToCartesian(radius, Math.PI / 4.0);

            Assert.AreEqual(radius / Math.Sqrt(2.0), result.X, Tolerance);
            Assert.AreEqual(radius / Math.Sqrt(2.0), result.Y, Tolerance);
        }
    }
}
