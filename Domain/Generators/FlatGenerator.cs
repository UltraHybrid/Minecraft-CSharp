using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class FlatGenerator : IGenerator<PointI, Chunk<Block>>
    {
        public Chunk<Block> Generate(PointI point)
        {
            var chunk = new Chunk<Block>(point);
            var (xL, zL) = Chunk<Block>.GetSize;
            foreach (var p in Utils.TripleFor(xL, 5, zL))
            {
                chunk[p] = p.Y switch
                {
                    0 => new Block(BaseBlocks.Bedrock),
                    4 => new Block(BaseBlocks.Grass),
                    _ => new Block(BaseBlocks.Dirt)
                };
            }

            return chunk;
        }
    }
}