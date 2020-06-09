using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class PerlinChunkGenerator : IGenerator<int, Chunk<Block>>
    {
        private readonly IGenerator<float, float> highGenerator;

        public PerlinChunkGenerator(IGenerator<float, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(int x, int z)
        {
            var chunk = new Chunk<Block>(PointI.CreateXZ(x,z));
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (byte k = 0; k < Chunk<Block>.ZLength; k++)
                {
                    var value = (int) (highGenerator.Generate(x * Chunk<Block>.XLength + i,
                        z * Chunk<Block>.ZLength + k) * 22f + 100);
                    /*if (value < 0)
                    {
                        Console.WriteLine("?????????");
                        var position = new PointB(i, 0, k);
                        chunk[position] = new Block(BaseBlocks.Grass, position);
                    }*/

                    for (var j = value; j >= 0; j--)
                    {
                        var position = new PointB(i, (byte) j, k);
                        chunk[position] = new Block(j == value ? BaseBlocks.Grass : BaseBlocks.Dirt, position);
                    }
                }
            }

            return chunk;
        }
    }
}