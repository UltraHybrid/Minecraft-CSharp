using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class FlatGenerator : IGenerator<int, Chunk<Block>>
    {
        public Chunk<Block> Generate(int x, int z)
        {
            var chunk = new Chunk<Block>(PointI.CreateXZ(x, z));
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