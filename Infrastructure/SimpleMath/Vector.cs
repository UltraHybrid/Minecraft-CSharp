using System;
using tmp.Infrastructure.SimpleMath;

namespace tmp
{
    public readonly struct Vector
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public static readonly Vector Zero = new Vector(0, 0, 0);
        public static readonly Vector Unit = new Vector(1, 1, 1);
        public static readonly Vector UnitX = new Vector(1, 0, 0);
        public static readonly Vector UnitY = new Vector(0, 1, 0);
        public static readonly Vector UnitZ = new Vector(0, 0, 1);

        public float Length => (float) Math.Sqrt(X * X + Y * Y + Z * Z);

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector Normalized()
        {
            var length = Length;
            return length == 0 ? Zero : new Vector(X / length, Y / length, Z / length);
        }

        public static float Dot(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector Cross(Vector v1, Vector v2)
        {
            var x = v1.Y * v2.Z - v1.Z * v2.Y;
            var y = v1.Z * v2.X - v1.X * v2.Z;
            var z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector(x, y, z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y, -v.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(float coefficient, Vector v)
        {
            return new Vector(v.X * coefficient, v.Y * coefficient, v.Z * coefficient);
        }

        public static Vector operator *(Vector v, float coefficient)
        {
            return new Vector(v.X * coefficient, v.Y * coefficient, v.Z * coefficient);
        }

        public static Vector operator /(float coefficient, Vector v)
        {
            return new Vector(v.X / coefficient, v.Y / coefficient, v.Z / coefficient);
        }

        public static Vector operator /(Vector v, float coefficient)
        {
            return new Vector(v.X / coefficient, v.Y / coefficient, v.Z / coefficient);
        }

        public override string ToString()
        {
            return $"Vector({X}, {Y}, {Z})";
        }
    }
}