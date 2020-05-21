using System;
using System.Collections.Generic;

namespace tmp
{
    public abstract class EntityMover : IMover
    {
        public Vector Position { get; protected set; }
        public Vector Front { get; protected set; }
        public abstract Vector Left { get; set; }
        public abstract Vector Up { get; set; }
        public abstract float Speed { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float Yaw { get; set; }

        public EntityMover(Vector position, Vector front)
        {
            Position = position;
            Front = front;
        }

        public abstract void Move(Piece piece, Direction direction, float time);

        public abstract void Rotate(float deltaYaw, float deltaPitch);

        protected Vector Convert2Cartesian(float alpha, float betta)
        {
            var z = (float) (Math.Cos(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            var y = (float) Math.Sin(betta * Math.PI / 180);
            var x = (float) (-Math.Sin(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            return new Vector(x, y, z);
        }
    }
}