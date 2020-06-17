using System;
using System.Collections.Generic;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    /*public abstract class EntityMover2 : IMover
    {
        protected Basis basis;
        public Vector_Last Position => basis.O.AsVector();
        public Vector_Last Front => basis.K;
        public Vector_Last Left => basis.I;
        public Vector_Last Up => basis.J;
        public abstract float Speed { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float Yaw { get; set; }

        public EntityMover2(Vector_Last position, Vector_Last front)
        {
            var left = Vector_Last.Cross(Vector_Last.UnitY, front).Normalized();
            var up = Vector_Last.Cross(front, left).Normalized();
            basis = new Basis(position.AsPointF(), front, up, left);
        }

        public abstract void Move(Piece piece, IEnumerable<Direction> directions, float time);

        public abstract void Rotate(float deltaYaw, float deltaPitch);
    }*/
}