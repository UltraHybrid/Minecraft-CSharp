using System;

namespace tmp.TrialVersion
{
    public class Point3
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public static readonly Point3 Default = new Point3(0, 0, 0);

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int GetDistance(Point3 other)
        {
            return Math.Max(Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)), Math.Abs(Z - other.Z));
        }
    }
}