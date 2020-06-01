using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public struct PointI
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public static readonly PointI Default = new PointI(0, 0, 0);

        public PointI(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static PointI CreateXZ(int x, int z)
        {
            return new PointI(x, 0, z);
        }

        public int GetDistance(PointI other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }

        public PointI Add(PointI other)
        {
            return new PointI(X + other.X, Y + other.Y, Z + other.Z);
        }

        public static PointI operator -(PointI point)
        {
            return new PointI(-point.X, -point.Y, -point.Z);
        }

        public static explicit operator PointB(PointI point)
        {
            return new PointB((byte) point.X, (byte) point.Y, (byte) point.Z);
        }

        public static explicit operator Vector(PointI point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        public IEnumerable<PointI> GetNeighbours()
        {
            return GetFlatNeighbours(1, 2);
        }

        private IEnumerable<PointI> GetFlatNeighbours(params int[] totalDifference)
        {
            for(var dx = -1; dx <= 1; dx++)
            for(var dz = -1; dz <= 1; dz++)
                if (totalDifference.Contains(Math.Abs(dx) + Math.Abs(dz)))
                    yield return new PointI(X + dx, Y, Z + dz);
        }
    }
}