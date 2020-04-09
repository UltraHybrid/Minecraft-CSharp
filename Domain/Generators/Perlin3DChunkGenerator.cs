namespace tmp
{
    public class Perlin3DChunkGenerator : IGenerator<int, Chunk>
    {
        private readonly IGenerator<float, float> highGenerator;

        public Perlin3DChunkGenerator(IGenerator<float, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk Generate(int x, int z)
        {
            const float coeff = 0.9f;
            var chunk = new Chunk {Position = new PointI(x, 0, z)};
            for (var i = 0; i < Chunk.XLenght; i++)
            for (var j = 0; j < Chunk.YLength; j++)
            for (var k = 0; k < Chunk.ZLength; k++)
            {
                if (Get3DNoise(i * coeff + x * Chunk.XLenght, j * coeff, k * coeff + z * Chunk.ZLength) >= 0.2)
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