using System;
using System.Collections.Generic;

namespace tmp
{
    public sealed class FreeFlyMover : EntityMover
    {
        public override Vector Left { get; set; }
        public override Vector Up { get; set; }
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

        public FreeFlyMover(Vector position, Vector front) : base(position, front)
        {
            Speed = 15f;
            Front = front;
            Left = Vector.Cross(new Vector(0, 1, 0), front).Normalize();
            Up = Vector.Cross(Front, Left).Normalize();
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = new Vector(Front.X, 0, Front.Z).Normalize();
            var resultMove = Vector.Default;
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
            
            Position += distance * (resultMove.Normalize());
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Left = Vector.Cross(Up, Front).Normalize();
        }
    }
}