/*using System;
using System.Collections.Generic;
using System.Numerics;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Entity
{
    public class Cow : IEntity
    {
        public string Name { get; }
        public int Health { get; }
        public IMover2 Mover { get; }

        public Cow(PointF position)
        {
            Name = "Cow";
            Health = 10;
            var rnd = new Random();
            var direction = new Vector3(rnd.Next(), 0, rnd.Next()) + Vector3.UnitX;
            Mover = new CowMover(position, Vector3.Normalize(direction));
        }
    }

    public class CowMover : EntityMover2
    {
        private readonly PointF size;
        private float yaw;
        private float pitch;
        private readonly Random movementRnd;

        public CowMover(PointF position, Vector3 front) : base(position, front)
        {
            size = new PointF(1.5f, 1.2f, 0.8f);
            movementRnd = new Random();
        }

        public override Geometry Geometry
        {
            get
            {
                var frontXZ = new Vector3(Front.X, 0, Front.Z);
                var rightXZ = new Vector3(Right.X, 0, Right.Z);
                var b = new Basis(Position, frontXZ, Up, rightXZ).Normalized();
                return new Geometry(size.X, size.Y, size.Z, b);
            }
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
        {
            throw new System.NotImplementedException();
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            yaw += (float) movementRnd.NextDouble();
            
        }
    }
}*/