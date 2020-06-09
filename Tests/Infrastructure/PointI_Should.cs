using System.Linq;
using NUnit.Framework;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Tests.Infrastructure
{
    /*[TestFixture]
    public class PointI_Should
    {
        [TestCase(1, 2, 3)]
        public void Init_Should(int x, int y, int z)
        {
            var point = new PointI(x, y, z);
            Assert.AreEqual(new []{ x, y, z }, new [] { point.X, point.Y, point.Z });
        }

        [Test]
        public void DefaultPoint_Should()
        {
            var defaultPoint = PointI.Zero;
            Assert.AreEqual(
                Enumerable.Repeat(0, 3), 
                new [] { defaultPoint.X, defaultPoint.Y, defaultPoint.Z });
        }
        
        [Test]
        public void Distant_Should()
        {
            var point1 = new PointI(1, 1, 1);
            var point3 = new PointI(3, 3, 3);
            var point123 = new PointI(1, 2, 3);
            
            Assert.AreEqual(0, point1.GetDistance(point1));
            
            Assert.AreEqual(2, point1.GetDistance(point3));
            
            Assert.AreEqual(point1.GetDistance(point123), point123.GetDistance(point1));
        }

        [Test]
        public void Add_Should()
        {
            var point1 = new PointI(1, 1, 1);
            var point123 = new PointI(1, 2, 3);
            
            Assert.AreEqual(new PointI(2, 3, 4), point1.Add(point123));
            Assert.AreEqual(point1.Add(point123), point123.Add(point1));
        }
        
        [TestCase(1, 2, 3)]
        [TestCase(12312, 12321, 34342)]
        public void ExplicitConversion_ToPointByte_Should(int x, int y, int z)
        {
            var point = (PointB)new PointI(x, y, z);;
            
            Assert.AreEqual(new [] { (byte) x, (byte) y, (byte) z }, new [] { point.X, point.Y, point.Z });
        }
        
    }*/
}