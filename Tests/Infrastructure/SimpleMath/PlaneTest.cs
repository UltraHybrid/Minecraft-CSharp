using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using tmp.Infrastructure.SimpleMath;
using Plane = tmp.Infrastructure.SimpleMath.Plane;

namespace tmp.Tests.Infrastructure.SimpleMath
{
    [TestFixture]
    class LineTest
    {
        [Test]
        public void GetPoint_Should()
        {
            var line = new Line(PointF.Zero, Vector3.One);

            Assert.Zero(line.GetPointByParameter(0).GetDistance(PointF.Zero));

            Assert.Zero(line.GetPointByParameter(1).GetDistance(new PointF(1, 1, 1)));

            Assert.Zero(line.GetPointByParameter(-1).GetDistance(new PointF(-1, -1, -1)));
        }
    }

    [TestFixture]
    class ParallelogramTest
    {
        [Test]
        public void IsInner_Should()
        {
            var parallelogram = new Parallelogram(PointF.Zero, new PointF(0, 1, 0), new PointF(1, 1, 0), new PointF(1, 0, 0));

            var random = new Random();
            for (int i = 0; i < 1e6; i++)
            {
                var point = new PointF((float)random.NextDouble(), (float)random.NextDouble(), 0);
                Assert.IsTrue(parallelogram.IsInner(point));
            }
        }
    }

    [TestFixture]
    class PlaneTest
    {
        [Test]
        public void Init_Should()
        {
            var plane = new Plane(Vector3.UnitX, PointF.Zero);
            Assert.AreEqual(Vector3.UnitX, plane.Normal);
            Assert.Zero(plane.D);


            var random = new Random();
            for (int i = 0; i < 1e6; i++)
            {
                var normal = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                normal = Vector3.Normalize(normal);
                plane = new Plane(Vector3.Normalize(normal), normal.AsPointF());

                Assert.AreEqual(normal.Length(), plane.D);
            }
        }

        [Test]
        public void From3Points_Should()
        {
            var plane = Plane.From3Points(Vector3.Zero.AsPointF(), Vector3.UnitZ.AsPointF(), Vector3.UnitX.AsPointF());

            Assert.Zero(plane.D);
            Assert.Zero(Vector3.Cross(Vector3.UnitY, plane.Normal).Length());
        }

        [Test]
        public void CalculateIntersectionPoint_Should()
        {
            var plane = new Plane(Vector3.UnitY, new PointF(0, 1, 0));
            var point = plane.CalculateIntersectionPoint(new Line(PointF.Zero, -Vector3.UnitY));

            #pragma warning disable CS8629 // Тип значения, допускающего NULL, может быть NULL.
            Assert.AreEqual(Vector3.UnitY, point.Value.AsVector());
        }

        [Test]
        public void ContainsPoint_Should()
        {
            var plane = new Plane(Vector3.UnitY, new PointF(0, 1, 0));

            var random = new Random();
            for (int i = 0; i < 1e6; i++)
            {
                var point = new PointF((float)random.NextDouble() * 1000 - 500, 1, (float)random.NextDouble() * 1000 - 500);

                Assert.IsTrue(plane.ContainsPoint(point));
            }

            for (int i = 0; i < 1e6; i++)
            {
                var point = new PointF((float)random.NextDouble() * 1000 - 500, random.Next(2, 100), (float)random.NextDouble() * 1000 - 500);

                Assert.IsFalse(plane.ContainsPoint(point));
            }
        }
    }
}
