using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class PerlinChunkGenerator : IGenerator<PointI, Chunk<Block>>
    {
        private readonly IGenerator<PointF, float> highGenerator;

        public PerlinChunkGenerator(IGenerator<PointF, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(PointI point)
        {
            var chunk = new Chunk<Block>(point);
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (byte k = 0; k < Chunk<Block>.ZLength; k++)
                {
                    var innerPoint = PointF.CreateXZ(point.X * Chunk<Block>.XLength + i,
                        point.Z * Chunk<Block>.ZLength + k);
                    var value = (int) (highGenerator.Generate(innerPoint) * 22f + 100);

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