using System;

namespace MinecraftSharp.Infrastructure.SimpleMath
{
    public readonly struct PointB
    {
        public byte X { get; }
        public byte Y { get; }
        public byte Z { get; }
        public static readonly PointB Zero = new PointB(0, 0, 0);

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

        public override string ToString()
        {
            return "PointB(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}