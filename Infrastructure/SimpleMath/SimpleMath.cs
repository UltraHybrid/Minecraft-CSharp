namespace tmp.Infrastructure.SimpleMath
{
    public static class SimpleMath
    {
        public static PointB AsPointB(this PointI point)
        {
            return new PointB((byte) point.X, (byte) point.Y, (byte) point.Z);
        }

        public static PointI AsPointI(this PointB point)
        {
            return new PointI(point.X, point.Y, point.Z);
        }

        public static Vector AsVector(this PointI point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        public static PointF Add(this PointF point, Vector vector)
        {
            return new PointF(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        public static PointI AsPointI(this Vector vector)
        {
            return new PointI((int) vector.X, (int) vector.Y, (int) vector.Z);
        }
    }
}