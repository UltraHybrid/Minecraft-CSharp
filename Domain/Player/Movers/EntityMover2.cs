using System;
using System.Collections.Generic;
using System.Numerics;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public abstract class EntityMover2 : IMover2
    {
        public PointF Position { get; protected set; }
        public Vector3 Front { get; protected set; }
        protected Vector3 Right { get; set;}
        protected Vector3 Up { get; set; }
        public abstract Geometry Geometry { get; }

        public EntityMover2(PointF position, Vector3 front)
        {
            Position = position;
            Front = Vector3.Normalize(front);
            Up = Vector3.UnitY;
            Right = Vector3.Cross(Front, Up);
            Console.WriteLine("Front " + Front);
            Console.WriteLine("Up " + Up);
            Console.WriteLine("Right " + Right);
        }

        protected Vector3 Convert2Cartesian(float alpha, float betta)
        {
            /*var z = (float) (Math.Cos(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            var y = (float) Math.Sin(betta * Math.PI / 180);
            var x = (float) (-Math.Sin(alpha * Math.PI / 180) * Math.Cos(betta * Math.PI / 180));
            return new Vector3(x, y, z);*/
            var alphaRadians = alpha * Math.PI / 180;
            var bettaRadians = betta * Math.PI / 180;
            var x = (float) (Math.Cos(alphaRadians) * Math.Sin(bettaRadians));
            var y = (float) (Math.Cos(bettaRadians));
            var z = (float) (Math.Sin(alphaRadians) * Math.Sin(bettaRadians));
            return new Vector3(x, y, z);
        }

        public abstract void Move(Piece piece, IEnumerable<Direction> directions, float time);
        public abstract void Rotate(float deltaYaw, float deltaPitch);
    }
}