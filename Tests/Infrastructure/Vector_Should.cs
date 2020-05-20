using System;
using System.Linq;
using NUnit.Framework;

namespace tmp.Tests.Infrastructure
{
    [TestFixture]
    public class Vector_Should
    {
        [TestCase(1, 2, 3)]
        public void Init_Should(float x, float y, float z)
        {
            var vector = new Vector(x, y, z);
            
            Assert.AreEqual(new [] { x, y, z }, GetCoordinates(vector));
        }

        [Test]
        public void DefaultVector_Should()
        {
            Assert.AreEqual(
                Enumerable.Repeat(0f, 3), 
                GetCoordinates(Vector.Default));
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

            Assert.AreEqual(new[] { 3, 3, 3 }, GetCoordinates(3 * vector1));
            Assert.AreEqual(new[] { 2, 4, 6 }, GetCoordinates(2 * vector123));

            Assert.AreEqual(GetCoordinates(4 * vector123), GetCoordinates(vector123 * 4));
        }

        [Test]
        public void Cross_Should()
        {
            //TODO: выяснить что такое крос и написать тест
        }

        [Test]
        public void Minus_Should()
        {
            Assert.AreEqual(
                new[] { -1, -2, 3 }, 
                GetCoordinates(-new Vector(1, 2, -3)));

            Assert.AreEqual(
                new[] { -1, 1, 6 },
                GetCoordinates(new Vector(5, 5, 5) - new Vector(6, 4, -1)));
        }

        [Test]
        public void Add_Should()
        {
            Assert.AreEqual(
                new[] { -2, 4, 19 }, 
                GetCoordinates(new Vector(1, 4, 6) + new Vector(-3, 0, 13)));
        }

        [Test]
        public void ExplicitConvert_ToPointInt_Should()
        {
            var v = new Vector(0.54f, 434, 6.8f);

            PointI point = (PointI)v;
            Assert.AreEqual(new PointI(0, 434, 6), point);
        }

        private float[] GetCoordinates(Vector v)
        {
            return new [] { v.X, v.Y, v.Z }; 
        }
    }
}