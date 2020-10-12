/*using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class Perlin3DChunkGenerator : IGenerator<PointI, Chunk<Block>>
    {
        private readonly IGenerator<PointF, float> highGenerator;

        public Perlin3DChunkGenerator(IGenerator<PointF, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(PointI point)
        {
            const float coeff = 0.9f;
            var chunk = new Chunk<Block>(point);
            for (var i = 0; i < Chunk<Block>.XLength; i++)
            for (var j = 0; j < Chunk<Block>.YLength; j++)
            for (var k = 0; k < Chunk<Block>.ZLength; k++)
            {
                if (Get3DNoise(i * coeff + point.X * Chunk<Block>.XLength, j * coeff,
                    k * coeff + point.Z * Chunk<Block>.ZLength) >= 0.2)
                {
                    var position = new PointB((byte) i, (byte) j, (byte) k);
                    chunk[position] = new Block(BaseBlocks.Grass, position);
                }
            }

            return chunk;
        }

        private float Get3DNoise(float x, float y, float z)
        {
            var a = highGenerator.Generate(PointF.CreateXZ(x, y));
            var b = highGenerator.Generate(PointF.CreateXZ(y, z));
            var c = highGenerator.Generate(PointF.CreateXZ(z, x));
            return (a + b + c) / 3;
        }
    }
}*/