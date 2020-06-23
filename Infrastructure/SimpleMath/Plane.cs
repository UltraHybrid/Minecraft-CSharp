using System;
using System.Numerics;

namespace MinecraftSharp.Infrastructure.SimpleMath
{
    public class Plane
    {
        public readonly Vector3 Normal;
        public readonly float D;

        public Plane(Vector3 normal, PointF point)
        {
            Normal = normal;
            D = -(Vector3.Dot(normal, point.AsVector()));
        }

        public static Plane From3Points(PointF p0, PointF p1, PointF p2)
        {
            var v1 = p1.Add(-p0).AsVector();
            var v2 = p2.Add(-p0).AsVector();
            return new Plane(Vector3.Cross(v1, v2), p0);
        }

        public PointF? CalculateIntersectionPoint(Line line)
        {
            var value = Vector3.Dot(Normal, line.Direction);
            if (value == 0)
                return null;
            var t = -(Vector3.Dot(Normal, line.Point.AsVector()) + D) / value;
            if (t <= 0)
                return null;
            return line.GetPointByParameter(t);
        }
        
        public bool ContainsPoint(PointF point)
        {
            return Math.Abs(Vector3.Dot(Normal, point.AsVector()) + D) <= 0.001f;
        }
    }

    public class Line
    {
        public PointF Point;
        public Vector3 Direction;

        public Line(PointF point, Vector3 direction)
        {
            Point = point;
            Direction = direction;
        }

        public PointF GetPointByParameter(float t)
        {
            return Point.Add(t * Direction);
        }
    }

    public class Parallelogram
    {
        public readonly PointF p0;
        public readonly PointF p1;
        public readonly PointF p2;
        public readonly PointF p3;

        public Parallelogram(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public bool IsInner(PointF point)
        {
            return p0.X <= point.X && point.X <= p2.X &&
                   p0.Y <= point.Y && point.Y <= p2.Y &&
                   p0.Z <= point.Z && point.Z <= p2.Z;
        }
    }
}