using System;
using System.Numerics;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class Player
    {
        public int Hardness { get; private set; }
        public readonly EntityMover Mover;
        public float Height { get; }

        public Player(PointF position, Vector3 direction, int hardness)
        {
            Mover = new FreeFlyMover(position, direction);
            //Mover = new SurvivalMover(position, direction, 0.25f, 1.6f, 0.1f);
            //Mover = new FreeFlyMover2(position, direction);
            Hardness = hardness;
            Height = 1.8f;
        }
    }
}