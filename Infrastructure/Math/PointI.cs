using System;

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

        public int GetDistance(PointI other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }

        public PointI Add(PointI other)
        {
            return new PointI(X + other.X, Y + other.Y, Z + other.Z);
        }

        public static explicit operator PointB(PointI point)
        {
            return new PointB((byte) point.X, (byte) point.Y, (byte) point.Z);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}