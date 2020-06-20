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
        private Vector verticalSpeed = new Vector(0, 0, 0);
        private readonly float gravity;
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

        public SurvivalMover(Vector position, Vector front, float hitBotRadius, float hitBoxHeight, float gravity) : base(position, front)
        {
            this.gravity = gravity;
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
            verticalSpeed -= new Vector(0,gravity * time, 0);
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
                        if (Position.Y - (int) Position.Y < 0.01)
                        {
                            var underPoint = new PointI((int) Position.X, (int) Position.Y - 1, (int) Position.Z);
                            if (!HaveVerticalAccess(Position, underPoint, piece, out _))
                                verticalSpeed = new Vector(0, 0.035f, 0);
                        }
                        break;
                    case Direction.Down:
                        break;
                }
            }

            var move = verticalSpeed + distance * (resultMove.Normalize());
            Position += CropMove(move, piece, time);

        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Left = Vector.Cross(Up, Front).Normalize();
        }

        private Vector CropMove(Vector move, Piece piece, float time)
        {
            var newPosition = Position + move;

            newPosition = HorizontalCrop(newPosition, piece, time);
            var oldY = newPosition.Y;
            
            var bottomCoords = new PointI((int) newPosition.X, (int) newPosition.Y, (int) newPosition.Z);
            var upperCoords = new PointI(bottomCoords.X, (int) (newPosition.Y + boxHeight), bottomCoords.Z);

            if (move.Y > 0 && piece.GetItem(upperCoords) != null)
            {
                if (!HaveVerticalAccess(newPosition, upperCoords, piece, out _))
                {
                    newPosition.Y = upperCoords.Y - boxHeight;
                    verticalSpeed.Y = 0;
                }
            }
            else if (move.Y < 0)
            {
                if (!HaveVerticalAccess(newPosition, bottomCoords, piece, out _))
                {
                    newPosition.Y = bottomCoords.Y + 1;
                    verticalSpeed.Y = 0; 
                }
            }

            if (newPosition.Y != oldY)
            {
                newPosition = HorizontalCrop(newPosition, piece, time);
            }
            
            
            
            return newPosition - Position;
        }

        private Vector HorizontalCrop(Vector newPosition, Piece piece, float time)
        {
            for (var y = (int) (newPosition.Y + gravity * time); y < (int) (newPosition.Y + boxHeight); y++)
            {
                var coords = new PointI((int) newPosition.X, y, (int) newPosition.Z);
                foreach (var neighbour in coords.GetNeighbours())
                {
                    if (piece.GetItem(neighbour) == null) continue;
                    var bPos = new Vector(neighbour.X + 0.5f, neighbour.Y + 0.5f, neighbour.Z + 0.5f);
                    for (var i = 0; i < 2; i++)
                    {
                        var xDiff = bPos.X - newPosition.X;
                        var zDiff = bPos.Z - newPosition.Z;
                        var distance = Math.Max(Math.Abs(xDiff), Math.Abs(zDiff));
                        if (distance >= 0.5 + boxRadius) break;
                        if (Math.Abs(xDiff) > Math.Abs(zDiff))
                        {
                            newPosition.X = bPos.X - Math.Sign(xDiff) * (0.5f + boxRadius);
                        }
                        else
                        {
                            newPosition.Z = bPos.Z - Math.Sign(zDiff) * (0.5f + boxRadius);
                        }
                    }
                }
            }

            return newPosition;
        }

        private bool HaveVerticalAccess(Vector newPosition, PointI accessBlockPosition, Piece piece, out int dimension)
        {
            dimension = -1;
            if (piece.GetItem(accessBlockPosition) != null) return false;
            foreach (var neighbour in accessBlockPosition.GetNeighbours())
            {
                if (piece.GetItem(neighbour) == null) continue;
                var blockPos = new Vector(neighbour.X + 0.5f, neighbour.Y + 0.5f, neighbour.Z + 0.5f);
                var distance = Math.Max(Math.Abs(newPosition.X - blockPos.X),
                    Math.Abs(newPosition.Z - blockPos.Z));
                if (distance >= 0.5 + boxRadius) continue;
                dimension = Math.Abs(newPosition.X - blockPos.X) == distance ? 0 : 2;
                return false;
            }
            return true;
        }
    }
}