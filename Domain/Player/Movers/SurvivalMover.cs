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
                        // TODO: Доделать возможность прыжка
                        if (Position.Y - (int) Position.Y < 0.01)
                        {
                            var underPoint = new PointI((int) Position.X, (int) Position.Y - 1, (int) Position.Z);
                            if (!HaveVerticalAccess(Position, underPoint, piece))
                                verticalSpeed = new Vector(0, 0.035f, 0);
                        }
                        break;
                    case Direction.Down:
                        break;
                }
            }

            var move = verticalSpeed + distance * (resultMove.Normalize());
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
            var bottomCoords = new PointI((int) newPosition.X, (int) newPosition.Y, (int) newPosition.Z);
            var upperCoords = new PointI(bottomCoords.X, (int) (newPosition.Y + boxHeight), bottomCoords.Z);

            if (move.Y > 0 && piece.GetItem(upperCoords) != null)
            {
                if (!HaveVerticalAccess(newPosition, upperCoords, piece))
                {
                    newPosition.Y = upperCoords.Y - boxHeight;
                    verticalSpeed.Y = 0;
                }
            }
            else if (move.Y < 0)
            {
                if (!HaveVerticalAccess(newPosition, bottomCoords, piece))
                {
                    newPosition.Y = bottomCoords.Y + 1;
                    verticalSpeed.Y = 0; 
                }
                
            }

            // bottomCoords = new PointI((int) newPosition.X, (int) newPosition.Y, (int) newPosition.Z);
            // upperCoords = new PointI(bottomCoords.X, (int) (newPosition.Y + boxHeight), bottomCoords.Z);
            //
            // for (var y = bottomCoords.Y; y <= upperCoords.Y; y++)
            // {
            //     var pos = new PointI(bottomCoords.X, y, bottomCoords.Z);
            //     foreach (var neighbour in pos.GetNeighbours())
            //     {
            //         if (piece.GetItem(neighbour) == null) continue;
            //         var blockPos = new Vector(neighbour.X, neighbour.Y, neighbour.Z) + new Vector(0.5f, 0.5f, 0.5f);
            //         for (var i = 0; i < 2; i++)
            //         {
            //             var xDiff = blockPos.X - newPosition.X;
            //             var zDiff = blockPos.Z - newPosition.Z;
            //             var maxDiff = Math.Max(Math.Abs(xDiff), Math.Abs(zDiff));
            //             if (maxDiff >= 0.5 + boxRadius) break;
            //             if (maxDiff == Math.Abs(xDiff))
            //             {
            //                 newPosition.X = blockPos.X - Math.Sign(xDiff) * (float) (0.5 + boxRadius);
            //             }
            //             else
            //             {
            //                 newPosition.Z = blockPos.Z - Math.Sign(zDiff) * (float) (0.5 + boxRadius);
            //             }
            //         }
            //         
            //         
            //     }
            // }

            return newPosition - Position;
        }

        private bool HaveVerticalAccess(Vector newPosition, PointI accessBlockPosition, Piece piece)
        {
            if (piece.GetItem(accessBlockPosition) != null) return false;
            foreach (var neighbour in accessBlockPosition.GetNeighbours())
            {
                if (piece.GetItem(neighbour) == null) continue;
                var blockPos = new Vector(neighbour.X + 0.5f, neighbour.Y + 0.5f, neighbour.Z + 0.5f);
                var distance = Math.Max(Math.Abs(newPosition.X - blockPos.X),
                    Math.Abs(newPosition.Z - blockPos.Z));
                if (distance >= 0.5 + boxRadius) continue;
                return false;
            }
            return true;
        }
    }
}