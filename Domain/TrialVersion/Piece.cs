using System.Collections.Generic;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public class Piece
    {
        public readonly PointL Center;
        public readonly int Radius;
        private readonly World<Chunk<Block>, Block> world;

        public Piece(World<Chunk<Block>, Block> world, PointL center, int radius)
        {
            this.world = world;
            Center = center;
            Radius = radius;
        }

        public bool ContainsPosition(PointL position)
        {
            return Center.GetDistance(position) <= Radius;
        }

        public Block GetItem(PointL position)
        {
            if (!ContainsPosition(position))
                return Block.Either;
            return world.GetItem(position);
        }

        public IEnumerable<(PointL position, Block block)> Helper()
        {
            /*for (var i = 1; i <= Radius; i++)
            {
                foreach (var p in Center.GetCubicNeighbourhood(i))
                {
                    yield return (p, GetItem(p));
                }
            }*/
            for (var i = -Radius; i <= Radius; i++)
            {
                for (var j = -Radius; j <= Radius; j++)
                {
                    for (var k = -Radius; k <= Radius; k++)
                    {
                        var position = new PointL(i,j,k).Add(Center);
                        yield return (position, GetItem(position));
                    }
                }
            }
        }
    }
}