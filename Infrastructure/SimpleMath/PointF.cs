using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp.Infrastructure.SimpleMath
{
    public readonly struct PointF
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public static readonly PointF Zero = new PointF(0, 0, 0);

        public PointF(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // ReSharper disable once InconsistentNaming
        public static PointF CreateXZ(float x, float z)
        {
            return new PointF(x, 0, z);
        }

        public float GetDistance(PointF other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }

        public PointF Add(PointF other)
        {
            return new PointF(X + other.X, Y + other.Y, Z + other.Z);
        }

        public static PointF operator -(PointF point)
        {
            return new PointF(-point.X, -point.Y, -point.Z);
        }

        public override string ToString()
        {
            return "PointF(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}