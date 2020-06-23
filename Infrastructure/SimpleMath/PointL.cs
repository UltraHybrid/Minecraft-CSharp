using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftSharp.Infrastructure.SimpleMath
{
    public class PointL
    {
        public long X { get; }
        public long Y { get; }
        public long Z { get; }
        public static readonly PointL Zero = new PointL(0, 0, 0);

        public PointL(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // ReSharper disable once InconsistentNaming
        public static PointL CreateXZ(long x, long z)
        {
            return new PointL(x, 0, z);
        }

        public long GetDistance(PointL other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }

        public PointL Add(PointL other)
        {
            return new PointL(X + other.X, Y + other.Y, Z + other.Z);
        }

        public static PointL operator -(PointL point)
        {
            return new PointL(-point.X, -point.Y, -point.Z);
        }

        public override string ToString()
        {
            return "PointL(" + X + ", " + Y + ", " + Z + ")";
        }

        public IEnumerable<PointL> GetXzNeighbours()
        {
            return GetFlatXzNeighbours(1).Concat(GetFlatXzNeighbours(2));
        }

        public IEnumerable<PointL> GetNeighbours()
        {
            yield return Add(new PointL(0, 0, -1));
            yield return Add(new PointL(-1, 0, 0));
            yield return Add(new PointL(0, 1, 0));
            yield return Add(new PointL(0, -1, 0));
            yield return Add(new PointL(0, 0, 1));
            yield return Add(new PointL(1, 0, 0));
        }

        public IEnumerable<PointL> GetCubicNeighbourhood(int radius)
        {
            for (var i = -radius; i <= radius; i++)
            for (var k = -radius; k <= radius; k++)
                yield return Add(new PointL(i, -radius, k));

            for (var j = -radius + 1; j <= radius - 1; j++)
            {
                for (var i = -radius; i <= radius; i += 2 * radius)
                for (var k = -radius; k <= radius; k++)
                    yield return Add(new PointL(i, j, k));

                for (var k = -radius; k <= radius; k += 2 * radius)
                for (var i = -radius + 1; i <= radius - 1; i++)
                    yield return Add(new PointL(i, j, k));
            }

            for (var i = -radius; i <= radius; i++)
            for (var k = -radius; k <= radius; k++)
                yield return Add(new PointL(i, radius, k));
        }

        private IEnumerable<PointL> GetFlatXzNeighbours(int totalDifference)
        {
            for (var dx = -1; dx <= 1; dx++)
            for (var dz = -1; dz <= 1; dz++)
                if (totalDifference == Math.Abs(dx) + Math.Abs(dz))
                    yield return new PointL(X + dx, Y, Z + dz);
        }
    }
}