using System;
using System.Collections.Generic;
using System.Numerics;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public sealed class FreeFlyMover : EntityMover
    {
        public override Vector3 Left { get; set; }
        public override Vector3 Up { get; set; }
        public override float Speed { get; set; }
        private float pitch;

        public override float Pitch
        {
            get => pitch;
            set
            {
                pitch = value;
                if (pitch > 89.0f)
                    pitch = 89.0f;
                if (pitch < -89.0f)
                    pitch = -89.0f;
            }
        }

        private float yaw;

        public override float Yaw
        {
            get => yaw;
            set
            {
                yaw = value;
                if (yaw > 360)
                    yaw -= 360;
                if (yaw < 0)
                    yaw += 360;
            }
        }

        public FreeFlyMover(PointF position, Vector3 front) : base(position, front)
        {
            Speed = 15f;
            Front = front;
            Left = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, front));
            Up = Vector3.Normalize(Vector3.Cross(Front, Left));
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
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
                    Direction.Right => -Left,
                    Direction.Left => Left,
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
            Left = Vector3.Normalize(Vector3.Cross(Up, Front));
        }
    }
}