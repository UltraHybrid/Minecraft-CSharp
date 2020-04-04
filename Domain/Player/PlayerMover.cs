using System;
using System.Collections.Generic;

namespace tmp
{
    public class PlayerMover : EntityMover
    {
        public override Vector Right { get; set; }
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

        public PlayerMover(Vector position, Vector front, float speed) : base(position, front)
        {
            Speed = speed;
            Yaw = 90;
            Front = front;
            Right = Vector.Cross(new Vector(0, 1, 0), front).Normalize();
            Up = Vector.Cross(Front, Right).Normalize();
        }

        public override void Move(Direction direction, float time)
        {
            var distance = Speed * time;
            var frontXZ = new Vector(Front.X, 0, Front.Z).Normalize();
            var resultMove = Vector.Default;
            switch (direction)
            {
                case Direction.Forward:
                    resultMove += frontXZ;
                    break;
                case Direction.Back:
                    resultMove -= frontXZ;
                    break;
                case Direction.Right:
                    resultMove -= Right;
                    break;
                case Direction.Left:
                    resultMove += Right;
                    break;
                case Direction.Up:
                    resultMove += Up;
                    break;
                case Direction.Down:
                    resultMove -= Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Position += distance * (resultMove.Normalize());
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Right = Vector.Cross(Up, Front).Normalize();
        }
    }
}