using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Infrastructure
{
    public class Geometry : IEnumerable<PointF>
    {
        public PointF Up0;
        public PointF Up1;
        public PointF Up2;
        public PointF Up3;
        public PointF Down0;
        public PointF Down1;
        public PointF Down2;
        public PointF Down3;
        private PointF size;
        private Basis basis;
        public static Geometry Unit => new Geometry(1, 1, 1, Basis.UnitBasis);

        public Geometry(float xLength, float yLength, float zLength, Basis directionBasis)
        {
            basis = directionBasis.Scale(xLength, yLength, zLength);
            size = new PointF(xLength, yLength, zLength);

            Down0 = basis.O;
            Down1 = basis.O.Add(basis.K);
            Down2 = basis.O.Add(basis.I + basis.K);
            Down3 = basis.O.Add(basis.I);

            Up0 = Down0.Add(basis.J);
            Up1 = Down1.Add(basis.J);
            Up2 = Down2.Add(basis.J);
            Up3 = Down3.Add(basis.J);

            Squads = new[]
            {
                new Parallelogram(Down0, Down3, Up3, Up0), // f
                new Parallelogram(Down0, Down1, Up1, Up0), //r
                new Parallelogram(Up0, Up1, Up2, Up3), //t
                new Parallelogram(Down0, Down1, Down2, Down3), //bot
                new Parallelogram(Down1, Down2, Up2, Up1), // b
                new Parallelogram(Down3, Down2, Up2, Up3), // l
            };
        }

        public static Geometry CreateFromCenter(float xLength, float yLength, float zLength, Basis basis)
        {
            return new Geometry(xLength, yLength, zLength,
                basis.Shift(-(xLength * basis.I + zLength * basis.K) / 2));
        }

        public static Geometry CreateFromPosition(PointF pos, float radius, float height)
        {
            var b = Basis.UnitBasis
                .Shift(pos.AsVector());
            return CreateFromCenter(radius * 2, height, radius * 2, b);
        }

        public static Geometry Identity(Basis basis)
        {
            return new Geometry(1, 1, 1, basis);
        }

        public bool IsInnerPoint(PointF point)
        {
            var p = point.Add(-basis.O).Add(-basis.I / 2 - basis.K / 2);
            return 0 <= p.X && p.X <= size.X && 0 <= p.Y && p.Y <= size.Y && 0 <= p.Z && p.Z <= size.Z;
        }

        public bool IsCollision(Geometry other)
        {
            return other.Any(IsInnerPoint) || this.Any(other.IsInnerPoint);
        }

        public readonly Parallelogram[] Squads;

        public List<PointF> GetIntersectPoints(Line line)
        {
            var planes = GetPlanes().ToArray();
            var result = new List<PointF>();
            for (var i = 0; i < Squads.Length; i++)
            {
                var v = planes[i].CalculateIntersectionPoint(line);
                if (!v.HasValue)
                    continue;
                if (Squads[i].IsInner(v.Value))
                    result.Add(v.Value);
            }

            return result;
        }

        public bool IsIntersect(Line line)
        {
            return GetIntersectPoints(line).Count != 0;
        }

        public IEnumerable<Plane> GetPlanes()
        {
            yield return Plane.From3Points(Down0, Up0, Down3); //f
            yield return Plane.From3Points(Down1, Up1, Down0); // r
            yield return Plane.From3Points(Up0, Up1, Up3); // t
            yield return Plane.From3Points(Down0, Down3, Down1); //bot
            yield return Plane.From3Points(Down1, Down2, Up1); // b
            yield return Plane.From3Points(Down3, Up3, Down2); // l
        }


        public IEnumerator<PointF> GetEnumerator()
        {
            yield return Down0;
            yield return Down1;
            yield return Down2;
            yield return Down3;
            yield return Up0;
            yield return Up1;
            yield return Up2;
            yield return Up3;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}