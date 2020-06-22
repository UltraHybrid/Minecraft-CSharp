using System;
using System.Numerics;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class Player
    {
        public readonly string Name;
        public int Hardness { get; private set; }
        public readonly EntityMover Mover;
        public float Height { get; }
        public BlockType ActiveBlock { get; set; } = BaseBlocks.Dirt;

        public Player(string name, PointF position, Vector3 direction, int hardness)
        {
            //Mover = new FreeFlyMover(position, direction);
            //Mover = new SurvivalMover(position, direction, 0.25f, 1.6f, 0.1f);
            //Mover = new FreeFlyMover2(position, direction);
            Name = name;
            //Mover = new FreeFlyMover2(position, direction, new PointF(0.5f, 1.8f, 0.8f));
            
            Hardness = hardness;
            Height = 1.7f;
            Mover = new SurvivalMover(5f, position, direction, 0.25f, Height, 16f);
        }
    }
}