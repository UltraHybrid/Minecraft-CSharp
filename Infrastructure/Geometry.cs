using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Infrastructure
{
    public struct Geometry : IEnumerable<PointF>
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
            return new Geometry(xLength, yLength, zLength, basis.Shift(-(basis.I + basis.K) / 2));
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
            return other.Any(IsInnerPoint);
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