using System;
using System.Numerics;

namespace tmp.Infrastructure.SimpleMath
{
    public class Basis
    {
        public PointF O;
        public Vector3 I;
        public Vector3 J;
        public Vector3 K;
        public static Basis UnitBasis => new Basis(PointF.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public Basis(PointF center, Vector3 i, Vector3 j, Vector3 k)
        {
            O = center;
            I = i;
            J = j;
            K = k;
        }

        public Basis Scale(float scaleX, float scaleY, float scaleZ)
        {
            return new Basis(O, scaleX * I, scaleY * J, scaleZ * K);
        }

        public Basis Shift(Vector3 offset)
        {
            return new Basis(O.Add(offset), I, J, K);
        }

        public Basis Normalized()
        {
            return new Basis(O, Vector3.Normalize(I), Vector3.Normalize(J), Vector3.Normalize(K));
        }
    }
}