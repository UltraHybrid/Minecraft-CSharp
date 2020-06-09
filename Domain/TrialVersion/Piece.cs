using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class Piece
    {
        public readonly PointI Center;
        public readonly int Radius;
        private readonly IWorld<Chunk<Block>, Block> world;

        public Piece(IWorld<Chunk<Block>, Block> world, PointI center, int radius)
        {
            this.world = world;
            Center = center;
            Radius = radius;
        }

        public bool ContainsPosition(PointI position)
        {
            return Center.GetDistance(position) <= Radius;
        }

        public Block GetItem(PointI position)
        {
            if (!ContainsPosition(position))
                return Block.Either;
            return world.GetItem(position);
        }
    }
}