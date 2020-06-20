using System.Numerics;

namespace tmp.Infrastructure.SimpleMath
{
    public static class SimpleMath
    {
        public static PointI AsPointI(this PointB point)
        {
            return new PointI(point.X, point.Y, point.Z);
        }
        
        public static PointL AsPointL(this PointB point)
        {
            return new PointL(point.X, point.Y, point.Z);
        }

        public static PointB AsPointB(this PointI point)
        {
            return new PointB((byte) point.X, (byte) point.Y, (byte) point.Z);
        }

        public static PointF AsPointF(this PointI point)
        {
            return new PointF(point.X, point.Y, point.Z);
        }

        public static Vector3 AsVector(this PointI point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static PointF Add(this PointF point, Vector3 vectorLast)
        {
            return new PointF(point.X + vectorLast.X, point.Y + vectorLast.Y, point.Z + vectorLast.Z);
        }

        public static PointI AsPointI(this PointF point)
        {
            return new PointI((int) point.X, (int) point.Y, (int) point.Z);
        }
        
        public static PointL AsPointL(this PointF point)
        {
            return new PointL((long) point.X, (long) point.Y, (long) point.Z);
        }

        public static Vector3 AsVector(this PointF point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static Vector3 AsVector(this PointL point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static PointI AsPointI(this Vector3 vectorLast)
        {
            return new PointI((int) vectorLast.X, (int) vectorLast.Y, (int) vectorLast.Z);
        }

        public static PointF AsPointF(this Vector3 vectorLast)
        {
            return new PointF(vectorLast.X, vectorLast.Y, vectorLast.Z);
        }
    }
}