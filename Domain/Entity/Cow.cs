using System;
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
        public EntityMover Mover => mover;
        private SurvivalMover mover;
        private bool isSlowed = false;

        public Cow(PointF position)
        {
            Name = "Cow";
            Health = 10;
            var rnd = new Random();
            var direction = new Vector3(rnd.Next(), 0, rnd.Next()) + Vector3.UnitX;
            mover = new SurvivalMover(2, position, direction, 0.4f, 0.9f, 16f);
        }

        public void GoTo(PointL target, Piece piece, float time)
        {
            Follow(target, piece, time, 0);
        }

        public void Follow(PointL target, Piece piece, float time, float distance)
        {
            if (Mover.Position.GetDistance(target.AsPointF()) <= distance) return;
            var targetView = target.AsVector() - Mover.Position.AsVector();
            if (targetView.Equals(Vector3.Zero)) return;
            targetView = Vector3.Normalize(targetView);
            var currentView = mover.Front;
            var coef = GetCoefficient(targetView, currentView, time);
            var view = targetView * coef + currentView * (1 - coef);
            if (view.Equals(Vector3.Zero)) return;
            mover.SetView(view);
            var pos = Mover.Position;
            var directions = new List<Direction> {Direction.Forward};
            if (isSlowed)
            {
                directions.Add(Direction.Up);
                isSlowed = false;
            }

            if (view.X != 0 && view.Z != 0)
                Mover.Move(piece, directions, time);
            if (Mover.Position.GetSquaredDistance(pos) < mover.Speed * time / 2) isSlowed = true;
        }

        private float GetCoefficient(Vector3 targetView, Vector3 currentView, float time)
        {
            var distance = (targetView - currentView).Length() / 2;
            var angle = Math.Asin(distance);
            var max = Math.PI * time;
            if (angle <= max) return 1;
            return (float) (max / angle);
        }
    }
}