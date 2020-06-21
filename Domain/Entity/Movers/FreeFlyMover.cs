using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class FreeFlyMover : EntityMover
    {
        private readonly PointF size;
        public float Speed { get; private set; }
        public override Geometry Geometry { get; }

        private float yaw;
        private float Yaw
        {
            get => yaw;
            set => yaw = value % 360;
        }

        private float pitch;
        private float Pitch
        {
            get => pitch;
            set
            {
                if (value > 179f)
                    pitch = 179f;
                else if (value < 1f)
                    pitch = 1f;
                else pitch = value;
            }
        }
        
        public FreeFlyMover(PointF position, Vector3 front, PointF size) : base(position, front)
        {
            this.size = size;
            pitch = 90;
            yaw = 0;
            Speed = 15f;
        }

        public override void Move(Piece piece, IReadOnlyList<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = Vector3.Normalize(new Vector3(Front.X, 0, Front.Z));
            var resultMove = Vector3.Zero;
            foreach (var direction in directions)
            {
                resultMove += direction switch
                {
                    Direction.Forward => frontXZ,
                    Direction.Back => -frontXZ,
                    Direction.Right => Right,
                    Direction.Left => -Right,
                    Direction.Up => Up,
                    Direction.Down => -Up,
                };
            }

            if (!resultMove.Equals(Vector3.Zero))
                Position = Position.Add(distance * Vector3.Normalize(resultMove));
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Right = Vector3.Normalize(Vector3.Cross(Front, Up));
        }
    }
}