using System;

namespace MinecraftSharp.Infrastructure.SimpleMath
{
    public readonly struct PointI
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public static readonly PointI Zero = new PointI(0, 0, 0);

        public PointI(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // ReSharper disable once InconsistentNaming
        public static PointI CreateXZ(int x, int z)
        {
            return new PointI(x, 0, z);
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public void Deconstruct(out int x, out int z)
        {
            x = X;
            z = Z;
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

        public override string ToString()
        {
            return "PointI(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}