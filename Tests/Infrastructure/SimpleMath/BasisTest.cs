using NUnit.Framework;
using System;
using System.Numerics;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Tests.Infrastructure.SimpleMath
{
    [TestFixture]
    class BasisTest
    {
        private readonly Random random = new Random();

        [Test]
        public void UnitBasis_Should_Equals() 
        {
            Assert.Zero(PointF.Zero.GetDistance(Basis.UnitBasis.O));
        }

        private Vector3 GetRandomVector() => 
            new Vector3(
                (float)random.NextDouble() * 1000 - 500, 
                (float)random.NextDouble() * 1000 - 500, 
                (float)random.NextDouble() * 1000 - 500
                );

        [Test]
        public void Shift_Should()
        {
            for (int i = 0; i < 1e6; i++)
            {
                var offset = GetRandomVector();
                var basis = Basis.UnitBasis.Shift(offset);

                Assert.Zero(basis.O.GetDistance(new PointF(offset.X, offset.Y, offset.Z)));
            }
        }
    }
}
