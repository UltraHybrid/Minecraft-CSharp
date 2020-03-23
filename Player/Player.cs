using System;
using System.Deployment.Internal;
using System.Numerics;

namespace tmp
{
    public class Player
    {
        public int Hardness { get; private set; }
        public readonly PlayerMover Mover;
        public float Height { get; }

        public Player(Vector position, Vector direction, int hardness, float speed)
        {
            Mover = new PlayerMover(position, direction, speed);
            Hardness = hardness;
            Height = 1.8f;
        }
    }
}