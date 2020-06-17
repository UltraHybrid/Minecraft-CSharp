using System;
using System.Collections.Generic;
using System.Numerics;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public abstract class EntityMover : IMover
    {
        public PointF Position { get; protected set; }
        public Vector3 Front { get; protected set; }
        public abstract Vector3 Left { get; set; }
        public abstract Vector3 Up { get; set; }
        public abstract float Speed { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float Yaw { get; set; }

        public EntityMover(PointF position, Vector3 front)
        {
            Position = position;
            Front = front;
        }

        public abstract void Move(Piece piece, IEnumerable<Direction> directions, float time);

        public abstract void Rotate(float deltaYaw, float deltaPitch);

        protected Vector3 Convert2Cartesian(float alpha, float betta)
        {
            var z = (float) (Math.Cos(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            var y = (float) Math.Sin(betta * Math.PI / 180);
            var x = (float) (-Math.Sin(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            return new Vector3(x, y, z);
        }
    }
}