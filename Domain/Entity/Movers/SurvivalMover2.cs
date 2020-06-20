using System;
using System.Collections.Generic;
using System.Numerics;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class SurvivalMover2 : EntityMover2
    {
        private float yaw;
        private float pitch;
        private float height;
        private float radius;
        private float gravity;
        private Geometry geometry;
        private Vector3 verticalSpeed = new Vector3(0, 0, 0);
        public float Speed { get; private set; }

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

        public override Geometry Geometry => geometry;


        public SurvivalMover2(PointF position, Vector3 front, float hitBotRadius, float hitBoxHeight, float gravity) : base(position, front)
        {
            pitch = 90;
            yaw = 0;
            Speed = 4f;
            this.gravity = gravity;
            radius = hitBotRadius;
            height = hitBoxHeight;
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = Vector3.Normalize(new Vector3(Front.X, 0, Front.Z));
            var resultMove = Vector3.Zero;
            verticalSpeed -= new Vector3(0, gravity * time, 0);
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
                        resultMove += Right;
                        break;
                    case Direction.Left:
                        resultMove -= Right;
                        break;
                    case Direction.Up:
                        if (Position.Y - (int) Position.Y < 0.01)
                        {
                            var underPoint = new PointL((long) Position.X, (long) Position.Y - 1, (long) Position.Z);
                            if (!HaveVerticalAccess(Position, underPoint, piece))
                                verticalSpeed = new Vector3(0, 0.035f, 0);
                        }
                        break;
                    case Direction.Down:
                        break;
                }
            }

            var move = verticalSpeed;
            if (!resultMove.Equals(Vector3.Zero))
                move += distance * Vector3.Normalize(resultMove);
            Position = Position.Add(CropMove(move, piece, time));
            geometry = Geometry.CreateFromPosition(Position, radius, height);
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            Front = Convert2Cartesian(Yaw, Pitch);
            Right = Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        
        private bool HaveVerticalAccess(PointF newPosition, PointL accessBlockPosition, Piece piece)
        {
            var newGeometry = Geometry.CreateFromPosition(newPosition, radius, height);
            if (piece.GetItem(accessBlockPosition) != null) return false;
            foreach (var neighbour in accessBlockPosition.GetXzNeighbours())
            {
                var block = piece.GetItem(neighbour);
                if (block == null) continue;
                if (newGeometry.IsCollision(Block.GetGeometry(neighbour))) return false;
            }
            return true;
        }
        
        private Vector3 CropMove(Vector3 move, Piece piece, float time)
        {
            var newPosition = Position.Add(move);
            newPosition = HorizontalCrop(newPosition, piece, time);
            var oldY = newPosition.Y;

            var bottomCoords = new PointL((long) newPosition.X, (long) newPosition.Y, (long) newPosition.Z);
            var upperCoords = new PointL(bottomCoords.X, (long) (newPosition.Y + height), bottomCoords.Z);

            if (move.Y > 0 && piece.GetItem(upperCoords) != null)
            {
                if (!HaveVerticalAccess(newPosition, upperCoords, piece))
                {
                    newPosition = new PointF(newPosition.X, upperCoords.Y - height, newPosition.Z);
                    verticalSpeed = new Vector3(0,0,0);
                }
            }
            else if (move.Y < 0)
            {
                if (!HaveVerticalAccess(newPosition, bottomCoords, piece))
                {
                    newPosition = new PointF(newPosition.X, bottomCoords.Y + 1, newPosition.Z);
                    verticalSpeed = new Vector3(0, 0, 0); 
                }
            }
            if (newPosition.Y != oldY)
            {
                newPosition = HorizontalCrop(newPosition, piece, time);
            }

            return newPosition.AsVector() - Position.AsVector();
        }
        
        /*private PointF HorizontalCrop(PointF newPosition, Piece piece, float time)
        {
            var geom = Geometry.CreateFromPosition(newPosition, radius, height);
            for (var y = (long) (newPosition.Y); y < (long) (newPosition.Y + height); y++)
            {
                var coords = new PointL((long) newPosition.X, y, (long) newPosition.Z);
                if (piece.GetItem(coords) != null) continue;
                foreach (var neighbour in coords.GetXzNeighbours())
                {
                    if (piece.GetItem(neighbour) == null) continue;
                    if (!geom.IsCollision(Block.GetGeometry(neighbour))) continue;
                    var bPos = neighbour.AsVector() + Vector3.One / 2;
                    var diff = bPos - newPosition.AsVector();
                    newPosition = Math.Abs(diff.X) > Math.Abs(diff.Z) 
                        ? new PointF(bPos.X - Math.Sign(diff.X) * (0.5f + radius), newPosition.Y, newPosition.Z) 
                        : new PointF(newPosition.X, newPosition.Y, bPos.Z - Math.Sign(diff.Z) * (0.5f + radius));
                    geom = Geometry.CreateFromPosition(newPosition, radius, height);
                }
            }
            return newPosition;
        }*/
        private PointF HorizontalCrop(PointF newPosition, Piece piece, float time)
        {
            for (var y = (long) (newPosition.Y + gravity * time); y < (long) (newPosition.Y + height); y++)
            {
                var coords = new PointL((long) newPosition.X, y, (long) newPosition.Z);
                foreach (var neighbour in coords.GetXzNeighbours())
                {
                    if (piece.GetItem(neighbour) == null) continue;
                    var bPos = new Vector3(neighbour.X + 0.5f, neighbour.Y + 0.5f, neighbour.Z + 0.5f);
                    for (var i = 0; i < 2; i++)
                    {
                        var xDiff = bPos.X - newPosition.X;
                        var zDiff = bPos.Z - newPosition.Z;
                        var distance = Math.Max(Math.Abs(xDiff), Math.Abs(zDiff));
                        if (distance >= 0.5 + radius) break;
                        if (Math.Abs(xDiff) > Math.Abs(zDiff))
                        {
                            newPosition =
                                newPosition.Add(new PointF(bPos.X - Math.Sign(xDiff) * (0.5f + radius) - newPosition.X,
                                    0, 0));
                        }
                        else
                        {
                            newPosition = newPosition.Add(new PointF(0, 0,
                                bPos.Z - Math.Sign(zDiff) * (0.5f + radius) - newPosition.Z));
                        }
                    }
                }
            }

            return newPosition;
        }
    }
}