using NUnit.Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp.Tests.Domain.Player
{
    [TestFixture]
    class FreeFlyMover_Should
    {
        private IMover mover;

        [SetUp]
        public void SetUp()
        {
            mover = new FreeFlyMover(Vector.Default, new Vector(0, 0, 1), 1);
        }

        [Test]
        public void Move_Should()
        {
            mover.Move(Direction.Forward, 1);
            Assert.AreEqual(new Vector(0, 0, 1), mover.Position);
        }

        [Test]
        public void Move_Should_AfterRotate()
        {
            mover.Rotate(90, 0);
            mover.Move(Direction.Forward, 2);

            Assert.AreEqual(0, 
                (new Vector(-2, 0, 0) - mover.Position).Length,
                1e-5
                );
        }

        [Test]
        public void Rotate_Should_WhenRotateHorizontal()
        {
            mover.Rotate(90, 0);

            var oldUp = new Vector(mover.Up.X, mover.Up.Y, mover.Up.Z);
            Assert.AreEqual(oldUp, mover.Up);
            Assert.AreEqual(0, (new Vector(0, 0, 1) - mover.Left).Length, 1e-5);
            Assert.AreEqual(Vector.Default, mover.Position);
        }

        [Test]
        public void Rotate_Should_WhenRotateVertical()
        {
            mover.Rotate(0, 45);

            var oldUp = new Vector(mover.Up.X, mover.Up.Y, mover.Up.Z);
            var oldLeft = new Vector(mover.Left.X, mover.Left.Y, mover.Left.Z);

            Assert.AreEqual(oldLeft, mover.Left);
            Assert.AreEqual(oldUp, mover.Up);
            Assert.AreEqual(Vector.Default, mover.Position);
        }
    }
}
