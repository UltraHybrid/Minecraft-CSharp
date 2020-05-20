using System.Linq;
using NUnit.Framework;
using OpenTK.Graphics.OpenGL;

namespace tmp.Tests.Infrastructure
{
    [TestFixture]
    public class PointB_Should
    {
        [TestCase(1, 2, 3)]
        public void Init_Should(byte x, byte y, byte z)
        {
            var point = new PointB(x, y, z);
            Assert.AreEqual(new []{ x, y, z }, new [] { point.X, point.Y, point.Z });
        }

        [Test]
        public void DefaultPoint_Should()
        {
            var defaultPoint = PointB.Default;
            Assert.AreEqual(
                Enumerable.Repeat(0, 3), 
                new [] { defaultPoint.X, defaultPoint.Y, defaultPoint.Z });
        }
        
        [Test]
        public void Distant_Should()
        {
            var point1 = new PointB(1, 1, 1);
            var point3 = new PointB(3, 3, 3);
            var point123 = new PointB(1, 2, 3);
            
            Assert.AreEqual(0, point1.GetDistance(point1));
            
            Assert.AreEqual(2, point1.GetDistance(point3));
            
            Assert.AreEqual(point1.GetDistance(point123), point123.GetDistance(point1));
        }

        [Test]
        public void Add_Should()
        {
            var point1 = new PointB(1, 1, 1);
            var point123 = new PointB(1, 2, 3);
            
            Assert.AreEqual(new PointB(2, 3, 4), point1.Add(point123));
            
            var pointMax = new PointB(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            
            Assert.AreEqual(new PointB(0, 1, 2), pointMax.Add(point123));
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 3)]
        [TestCase(byte.MaxValue, 0, 0)]
        public void ImplicitConversion_ToPointInt_Should(byte x, byte y, byte z)
        {
            PointI point = new PointB(x, y, z);;
            
            Assert.AreEqual(new [] { x, y, z }, new [] { point.X, point.Y, point.Z });
        }
    }
}