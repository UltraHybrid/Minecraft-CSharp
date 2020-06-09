namespace tmp.Infrastructure.SimpleMath
{
    public class Basis
    {
        public PointF O;
        public Vector I;
        public Vector J;
        public Vector K;
        public static Basis UnitBasis => new Basis(PointF.Zero, Vector.UnitX, Vector.UnitY, Vector.UnitZ);

        public Basis(PointF center, Vector i, Vector j, Vector k)
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

        public Basis Shift(Vector offset)
        {
            return new Basis(O.Add(offset), I, J, K);
        }
    }
}