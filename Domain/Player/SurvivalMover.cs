using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public sealed class SurvivalMover : EntityMover
    {
        public override Vector Left { get; set; }
        public override Vector Up { get; set; }
        public override float Speed { get; set; }
        private float pitch;
        private Vector vertical_speed = new Vector(0, 0, 0);
        private const float Gravity = 0.05f;
        private readonly float boxRadius;
        private readonly float boxHeight;

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

        public SurvivalMover(Vector position, Vector front, float hitBotRadius, float hitBoxHeight) : base(position, front)
        {
            Speed = 5f;
            Front = front;
            boxHeight = hitBoxHeight;
            boxRadius = hitBotRadius;
            Left = Vector.Cross(new Vector(0, 1, 0), front).Normalize();
            Up = Vector.Cross(Front, Left).Normalize();
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = new Vector(Front.X, 0, Front.Z).Normalize();
            var resultMove = Vector.Default;
            vertical_speed -= new Vector(0,Gravity * time, 0);
            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        resultMove += frontXZ;
                        break;
                    case Direction.Back:
                        resultMove -= frontXZ;
                        break;
                    case Direction.Right:
                        resultMove -= Left;
                        break;
                    case Direction.Left:
                        resultMove += Left;
                        break;
                    case Direction.Up:
                        vertical_speed = new Vector(0, Gravity / 2, 0);
                        break;
                    case Direction.Down:
                        break;
                }
            }

            var move = vertical_speed + distance * (resultMove.Normalize());
            Position += CropMove(move, piece);

        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Left = Vector.Cross(Up, Front).Normalize();
        }

        private Vector CropMove(Vector move, Piece piece)
        {
            var newPosition = Position + move;
            var coords = new PointI((int) newPosition.X, (int) newPosition.Y, (int) newPosition.Z);
            if (piece.GetItem(coords) != null)
            {
                newPosition.Y = coords.Y + 1;
                vertical_speed.Y = 0;
            }

            return newPosition - Position;
        }
    }
}