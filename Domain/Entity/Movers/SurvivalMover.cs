using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public class SurvivalMover : EntityMover
    {
        private float yaw;
        private float pitch;
        private float radius;
        private float height;
        private float gravity;
        private float verticalSpeed;
        private Geometry geometry;
        public override Geometry Geometry => geometry;
        public float Speed { get; }

        private float Yaw
        {
            get => yaw;
            set => yaw = value % 360;
        }

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


        public SurvivalMover(float speed, PointF position, Vector3 front, float hitBoxRadius, float hitBoxHeight, float gravity) :
            base(position, front)
        {
            height = hitBoxHeight;
            radius = hitBoxRadius;
            this.gravity = gravity;
            geometry = Geometry.CreateFromPosition(position, radius, height);
            pitch = 90;
            yaw = 0;
            Speed = speed;
        }

        public override void Move(Piece piece, IReadOnlyList<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = Vector3.Normalize(new Vector3(Front.X, 0, Front.Z));
            var resultMove = Vector3.Zero;
            verticalSpeed -= gravity * time;
            foreach (var direction in directions)
            {
                resultMove += direction switch
                {
                    Direction.Forward => frontXZ,
                    Direction.Back => -frontXZ,
                    Direction.Right => Right,
                    Direction.Left => -Right,
                    _ => Vector3.Zero
                };
            }

            if (!resultMove.Equals(Vector3.Zero))
            {
                Position = Position.Add(HorizontalCrop(Position, distance * Vector3.Normalize(resultMove), piece));
            }

            var vertDistance = verticalSpeed * time - gravity * time * time / 2;
            var vertMove = VerticalCrop(Position, new Vector3(0, vertDistance, 0), piece);
            if (vertMove.Equals(Vector3.Zero))
            {
                if (verticalSpeed < 0 && directions.Contains(Direction.Up))
                    verticalSpeed = 6.5f;
                else
                    verticalSpeed = 0;
            } 
            Position = Position.Add(vertMove);
            geometry = Geometry.CreateFromPosition(Position, radius, height);
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Right = Vector3.Normalize(Vector3.Cross(Front, Up));
        }

        private Vector3 HorizontalCrop(PointF position, Vector3 move, Piece piece)
        {
            var croppedMove = move;
            var newPosition = position.Add(move);
            //var newGeometry = Geometry.CreateFromPosition(newPosition, radius, height);
            for (var y = (long) newPosition.Y; y <= (long) (newPosition.Y + height); y++)
            {
                var blockCoords = new PointL((long) newPosition.X, y, (long) newPosition.Z);
                foreach (var neighbour in blockCoords.GetXzNeighbours())
                {
                    if (piece.GetItem(neighbour) == null) continue;
                    var blockPos = neighbour.AsVector() + new Vector3(0.5f);
                    var xDiff = blockPos.X - newPosition.X;
                    var zDiff = blockPos.Z - newPosition.Z;
                    // TODO: 2 строки ниже должны делать тоже самое, но почему-то этого не делают!!!!
                    //if (!newGeometry.IsCollision(Block.GetGeometry(neighbour))) continue;
                    if (Math.Max(Math.Abs(xDiff), Math.Abs(zDiff)) >= 0.5 + radius) continue;
                    if (Math.Abs(xDiff) > Math.Abs(zDiff))
                    {
                        var newAxisPos = blockPos.X - Math.Sign(xDiff) * (0.5 + radius);
                        croppedMove = new Vector3((float) newAxisPos - position.X, croppedMove.Y, croppedMove.Z);
                    }
                    else
                    {
                        var newAxisPos = blockPos.Z - Math.Sign(zDiff) * (0.5 + radius);
                        croppedMove = new Vector3(croppedMove.X, croppedMove.Y, (float) newAxisPos - position.Z);
                    }

                    newPosition = position.Add(croppedMove);
                    //newGeometry = Geometry.CreateFromPosition(newPosition, radius, height);
                }
            }

            return croppedMove;
        }

        private Vector3 VerticalCrop(PointF position, Vector3 move, Piece piece)
        {
            var newPosition = position.Add(move);
            if (move.Y < 0)
            {
                var blockCoords = newPosition.AsPointL();
                if (!HaveVerticalAccess(newPosition, blockCoords, piece))
                {
                    return Vector3.Zero;
                }
            }
            else
            {
                var blockCoords = newPosition.Add(new PointF(0, height, 0)).AsPointL();
                if (!HaveVerticalAccess(newPosition, blockCoords, piece))
                {
                    return Vector3.Zero;
                }
            }

            return move;
        }

        private bool HaveVerticalAccess(PointF newPosition, PointL accessBlockPosition, Piece piece)
        {
            var newGeometry = Geometry.CreateFromPosition(newPosition, radius, height);
            if (piece.GetItem(accessBlockPosition) != null) return false;
            foreach (var neighbour in accessBlockPosition.GetXzNeighbours())
            {
                var block = piece.GetItem(neighbour);
                if (block == null) continue;
                var distance = Math.Max(Math.Abs(neighbour.X + 0.5 - newPosition.X),
                    Math.Abs(neighbour.Z + 0.5 - newPosition.Z));
                if (distance < 0.5 + radius) return false;
                //if (newGeometry.IsCollision(Block.GetGeometry(neighbour))) return false;
            }

            return true;
        }
    }
}