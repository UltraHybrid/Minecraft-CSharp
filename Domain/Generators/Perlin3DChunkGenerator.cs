using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class Perlin3DChunkGenerator : IGenerator<int, Chunk<Block>>
    {
        private readonly IGenerator<float, float> highGenerator;

        public Perlin3DChunkGenerator(IGenerator<float, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(int x, int z)
        {
            const float coeff = 0.9f;
            var chunk = new Chunk<Block> (PointI.CreateXZ(x,z));
            for (var i = 0; i < Chunk<Block>.XLength; i++)
            for (var j = 0; j < Chunk<Block>.YLength; j++)
            for (var k = 0; k < Chunk<Block>.ZLength; k++)
            {
                if (Get3DNoise(i * coeff + x * Chunk<Block>.XLength, j * coeff, k * coeff + z * Chunk<Block>.ZLength) >= 0.2)
                {
                    var position = new PointB((byte) i, (byte) j, (byte) k);
                    chunk[position] = new Block(BaseBlocks.Grass, position);
                }
            }

            return chunk;
        }

        private float Get3DNoise(float x, float y, float z)
        {
            var a = highGenerator.Generate(x, y);
            var b = highGenerator.Generate(y, z);
            var c = highGenerator.Generate(z, x);
            return (a + b + c) / 3;
        }
    }
}