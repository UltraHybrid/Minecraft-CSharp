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

        public bool IsIntersect(Line line)
        {
            var pf = Plane.From3Points(Down0, Up0, Down3);
            var pr = Plane.From3Points(Down1, Up1, Up0);
            var pt = Plane.From3Points(Up0, Up1, Up3);
            var pbt = Plane.From3Points(Down0, Down3, Down1);
            var pb = Plane.From3Points(Down1, Down2, Up1);
            var pl = Plane.From3Points(Down3, Down2, Up3);

            var planes = new[] {pf, pr, pt, pbt, pb, pl};
            var squads = new[]
            {
                new Parallelogram(Down3, Down0, Up0, Up3),
                new Parallelogram(Down0, Down1, Up1, Up0),
                new Parallelogram(Up0, Up1, Up2, Up3),
                new Parallelogram(Down0, Down1, Down2, Down3),
                new Parallelogram(Down1, Up1, Up2, Down2),
                new Parallelogram(Down2, Up2, Up3, Down3),
            };

            for (var i = 0; i < 6; i++)
            {
                var v = planes[i].CalculateIntersectionPoint(line);
                if (!v.HasValue)
                    continue;
                if (squads[i].IsInner(v.Value))
                    return true;
            }

            return false;
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