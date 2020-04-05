using System;

namespace tmp
{
    public struct PointB
    {
        public byte X { get; private set; }
        public byte Y { get; private set; }
        public byte Z { get; private set; }
        public static readonly PointB Default = new PointB(0, 0, 0);

        public PointB(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int GetDistance(PointB other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }

        public PointB Add(PointB other)
        {
            return new PointB((byte) (X + other.X), (byte) (Y + other.Y), (byte) (Z + other.Z));
        }

        public static implicit operator PointI(PointB point)
        {
            return new PointI(point.X, point.Y, point.Z);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}