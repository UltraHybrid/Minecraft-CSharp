using System;
using System.Linq;
using NUnit.Framework;

namespace tmp.Tests.Infrastructure
{
    [TestFixture]
    public class Vector_Should
    {
        [Test]
        public void DefaultVector_Should()
        {
            Assert.AreEqual(new Vector(0, 0, 0), Vector.Default);
        }

        [TestCase(0, 0, 0)]
        [TestCase(123, 3124, 4324)]
        [TestCase(0.45f, 0.234f, 43f)]
        public void NormalizeVector_Should(float x, float y, float z)
        {
            var vector = new Vector(x, y, z);
            var normalizeVector = vector.Normalize();

            var delta = 1e-3;
            if (Math.Abs(x) + Math.Abs(y) + Math.Abs(z) > delta)
                Assert.AreEqual(1, normalizeVector.Length, delta);
            else
                Assert.AreEqual(0, normalizeVector.Length, delta);
            Assert.AreEqual(normalizeVector.X * vector.Length, vector.X, delta);
            Assert.AreEqual(normalizeVector.Y * vector.Length, vector.Y, delta);
            Assert.AreEqual(normalizeVector.Z * vector.Length, vector.Z, delta);
        }

        [TestCase(1, 1, 0, 0)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(3, 1, 2, 2)]
        public void CalculateLength_Should(float expectedLength, float x, float y, float z)
        {
            Assert.AreEqual(expectedLength, new Vector(x, y, z).Length);
        }

        [Test]
        public void Multiply_Should()
        {
            var vector1 = new Vector(1, 1, 1);
            var vector123 = new Vector(1, 2, 3);

            Assert.AreEqual(new Vector(3, 3, 3), 3 * vector1);
            Assert.AreEqual(new Vector(2, 4, 6), 2 * vector123);

            Assert.AreEqual(4 * vector123, vector123 * 4);
        }

        [Test]
        public void Cross_Should()
        {
            var v1 = new Vector(1, -1, 1);
            var v2 = new Vector(2, 1, -1);

            var result = Vector.Cross(v1, v2);

            Assert.AreEqual(0, (result - new Vector(0, 3, 3)).Length, 1e-5);

            v1 = new Vector(0.5f , 1, -2);
            v2 = new Vector(0.77f, 33, 0);

            result = Vector.Cross(v1, v2);

            Assert.AreEqual(0, (result - new Vector(66, -1.54f, 15.73f)).Length, 1e-5);
        }

        [Test]
        public void Cross_Should_BePerpendicular()
        { 
            var random = new Random();
            Func<float> getCoordinate = () => (float) (random.NextDouble() - 0.5) * 100;
            Func<Vector, Vector, float> scalar = (v1, v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            var delta = 1e-1;

            for (int i = 0; i < 1e4; i++)
            {
                var v1 = new Vector(getCoordinate(), getCoordinate(), getCoordinate());
                var v2 = new Vector(getCoordinate(), getCoordinate(), getCoordinate());

                var v3 = Vector.Cross(v1, v2);
                Assert.AreEqual(0, scalar(v1, v3), delta);
                Assert.AreEqual(0, scalar(v2, v3), delta);
            }
        }

        [Test]
        public void Minus_Should()
        {
            Assert.AreEqual(new Vector(-1, -2, 3), -new Vector(1, 2, -3));

            Assert.AreEqual(
                new Vector(-1, 1, 6),
                new Vector(5, 5, 5) - new Vector(6, 4, -1)
                );
        }

        [Test]
        public void Add_Should()
        {
            Assert.AreEqual(
                new Vector(-2, 4, 19), 
                new Vector(1, 4, 6) + new Vector(-3, 0, 13)
                );
        }

        [Test]
        public void ExplicitConvert_ToPointInt_Should()
        {
            var v = new Vector(0.54f, 434, 6.8f);

            PointI point = (PointI)v;
            Assert.AreEqual(new PointI(0, 434, 6), point);
        }
    }
}