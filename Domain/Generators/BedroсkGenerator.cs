using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class BedrockGenerator : IGenerator<Chunk<Block>, Chunk<Block>>
    {
        private readonly IGenerator<PointF, float> highGenerator;

        public BedrockGenerator(IGenerator<PointF, float> highGenerator)
        {
            this.highGenerator = highGenerator;
        }

        public Chunk<Block> Generate(Chunk<Block> chunk)
        {
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (byte k = 0; k < Chunk<Block>.ZLength; k++)
                {
                    var innerPoint = PointF.CreateXZ(chunk.Position.X * Chunk<Block>.XLength + i,
                        chunk.Position.Z * Chunk<Block>.ZLength + k);
                    var value = (int) (highGenerator.Generate(innerPoint) + 3);
                    for (var y = value; y >= 0; y--)
                    {
                        var position = new PointB(i, (byte)y, k);
                        chunk[position] = new Block(BaseBlocks.Bedrock, position);
                    }
                }
            }

            return chunk;
        }
    }
}