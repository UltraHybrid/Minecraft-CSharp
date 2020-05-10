using System;

namespace tmp
{
    public class PerlinChunkGenerator : IGenerator<int, Chunk>
    {
        private readonly IGenerator<float, float> highGenerator;

        public PerlinChunkGenerator(IGenerator<float, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            for (byte i = 0; i < Chunk.XLength; i++)
            {
                for (byte k = 0; k < Chunk.ZLength; k++)
                {
                    var value = (int) (highGenerator.Generate(x * Chunk.XLength + i, z * Chunk.ZLength + k) * 22f + 100);
                    if (value < 0)
                    {
                        var position = new PointB(i, 0, k);
                        chunk[position] = new Block(BaseBlocks.Grass, position);
                    }

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