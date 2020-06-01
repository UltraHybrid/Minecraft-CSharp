using System;

namespace tmp
{
    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static readonly Vector Default = new Vector(0, 0, 0);

        public float Length => (float) Math.Sqrt(X * X + Y * Y + Z * Z);

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector Normalize()
        {
            var length = Length;
            return length == 0 ? Default : new Vector(X / length, Y / length, Z / length);
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
            var x = -v.X;
            var y = -v.Y;
            var z = -v.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            var x = v1.X - v2.X;
            var y = v1.Y - v2.Y;
            var z = v1.Z - v2.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator *(Vector v, float coefficient) 
        {
            return coefficient* v;
        }

        public static Vector operator *(float coefficient, Vector v)
        {
            var x = v.X * coefficient;
            var y = v.Y * coefficient;
            var z = v.Z * coefficient;
            return new Vector(x, y, z);
        }

        public static explicit operator PointI(Vector v)
        {
            return new PointI((int)v.X, (int)v.Y, (int)v.Z);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector);
        }

        public bool Equals(Vector other) 
        {
            if (other is null)
                return false;

            return ReferenceEquals(other, this) ||
                other.X == X && other.Y == Y && other.Z == Z;       
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}