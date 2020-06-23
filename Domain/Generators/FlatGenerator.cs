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
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            for (byte j = 0; j < 5; j++)
            for (byte k = 0; k < Chunk<Block>.ZLength; k++)
            {
                var position = new PointB(i, j, k);
                chunk[position] = j switch
                {
                    0 => new Block(BaseBlocks.Bedrock, position),
                    4 => new Block(BaseBlocks.Grass, position),
                    _ => new Block(BaseBlocks.Dirt, position)
                };
            }

            return chunk;
        }
    }
}