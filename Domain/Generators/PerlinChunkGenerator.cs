using System;

namespace tmp
{
    public class PerlinChunkGenerator : IGenerator<Chunk>
    {
        private readonly IGenerator<byte> highGenerator;

        public PerlinChunkGenerator(IGenerator<byte> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            for (byte i = 0; i < Chunk.XLenght; i++)
            {
                for (byte k = 0; k < Chunk.ZLength; k++)
                {
                    var value = highGenerator.Generate(x * Chunk.XLenght + i, z * Chunk.ZLength + k);
                    for (int j = value; j>=0; j--)
                    {
                        var position = new PointB(i, (byte)j, k);
                        chunk[position] = new Block(j == value ? BaseBlocks.Grass : BaseBlocks.Dirt, position);
                    }
                }
            }

            return chunk;
        }
    }
}