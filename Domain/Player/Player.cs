using System.Numerics;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
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
            Name = name;
            Hardness = hardness;
            Height = 1.7f;
            Mover = new FreeFlyMover(position, direction, new PointF(0.5f, 1.8f, 0.8f));
            //Mover = new SurvivalMover(5f, position, direction, 0.25f, Height, 8f);
        }
    }
}